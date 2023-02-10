using Ephyta.DAL;
using Ephyta.Filters;
using Ephyta.Models;
using Ephyta.ViewModel;
using Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ephyta.Controllers
{
    [Authorize, AdminRoleFilters]
    public class VcmsController : Controller
    {
        // GET: Vcms
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private RoleAdmin Role => (RoleAdmin)Enum.Parse(typeof(RoleAdmin), RouteData.Values["Role"].ToString());

        #region Admin

        public ActionResult Index()
        {
            var model = new InfoAdminViewModel
            {
                Admins = _unitOfWork.AdminRepository.GetQuery().Count(),
                Articles = _unitOfWork.ArticleRepository.GetQuery().Count(),
                Banners = _unitOfWork.BannerRepository.GetQuery().Count(),
                Contacts = _unitOfWork.ContactRepository.GetQuery().Count(),
                Products = _unitOfWork.ProductRepository.GetQuery().Count(),
                Feedbacks = _unitOfWork.FeedbackRepository.GetQuery().Count()
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult ListAdmin()
        {
            var admins = _unitOfWork.AdminRepository.Get();
            return PartialView("ListAdmin", admins);
        }
        public ActionResult CreateAdmin(string result = "")
        {
            ViewBag.Result = result;
            if (Role != RoleAdmin.Admin)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateAdmin(Admin model)
        {
            if (Role != RoleAdmin.Admin)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var admin =
                    _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (admin != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                }
                else
                {
                    var hashPass = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.AdminRepository.Insert(new Admin { Username = model.Username, Password = hashPass, Active = model.Active });
                    _unitOfWork.Save();
                    return RedirectToAction("CreateAdmin", new { result = "success" });
                }
            }
            return View(model);
        }
        public ActionResult EditAdmin(int adminId = 0)
        {
            if (Role != RoleAdmin.Admin)
            {
                return RedirectToAction("Index");
            }
            var admin = _unitOfWork.AdminRepository.GetById(adminId);
            if (admin == null)
            {
                return RedirectToAction("CreateAdmin");
            }

            var model = new UpdateAdminModel
            {
                Username = admin.Username,
                Active = admin.Active,
                Id = admin.Id,
                RoleAdmin = admin.Role
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult EditAdmin(UpdateAdminModel model)
        {
            if (Role != RoleAdmin.Admin)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (admin == null)
                {
                    return RedirectToAction("CreateAdmin");
                }

                if (model.Username != "admin")
                {
                    admin.Active = model.Active;
                    admin.Role = model.RoleAdmin;
                    admin.Username = model.Username;
                }

                if (model.Password != null)
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                }
                _unitOfWork.Save();
                return RedirectToAction("CreateAdmin", new { result = "update" });
            }
            return View(model);
        }
        public bool DeleteAdmin(string username)
        {
            if (Role != RoleAdmin.Admin)
            {
                return false;
            }
            var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(username)).SingleOrDefault();
            if (admin == null)
            {
                return false;
            }
            if (username == "admin")
            {
                return false;
            }
            _unitOfWork.AdminRepository.Delete(admin);
            _unitOfWork.Save();
            return true;
        }
        public ActionResult ChangePassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (admin == null)
                {
                    return HttpNotFound();
                }
                if (HtmlHelpers.VerifyHash(model.OldPassword, "SHA256", admin.Password))
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.Save();
                    return RedirectToAction("ChangePassword", new { result = 1 });
                }
            }
            return View(model);
        }
        #endregion

        #region Login
        [AllowAnonymous, OverrideActionFilters]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous, OverrideActionFilters]
        [HttpPost]
        public ActionResult Login(AdminLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.Get(a => a.Username == model.Username).SingleOrDefault();

                if (admin != null && HtmlHelpers.VerifyHash(model.Password, "SHA256", admin.Password))
                {
                    var ticket = new FormsAuthenticationTicket(1, model.Username.ToLower(), DateTime.Now, DateTime.Now.AddDays(30), true,
                        admin.Role.ToString(),
                        FormsAuthentication.FormsCookiePath);

                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket) { SameSite = SameSiteMode.Lax, Secure = true });

                    //FormsAuthentication.SetAuthCookie(model.Username, true);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Vcms");
                }
                ModelState.AddModelError("", @"Tên đăng nhập  hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }
        public RedirectToRouteResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Vcms");
        }
        #endregion

        #region ConfigSite
        public ActionResult ConfigSite(string result = "")
        {
            ViewBag.Result = result;
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            return View(config);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ConfigSite(ConfigSite model)
        {
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            if (config == null)
            {
                _unitOfWork.ConfigSiteRepository.Insert(model);
            }
            else
            {
                config.Facebook = model.Facebook;
                config.Linkedin = model.Linkedin;
                config.Instagram = model.Instagram;
                config.GoogleMap = model.GoogleMap;
                config.Title = model.Title;
                config.Description = model.Description;
                config.GoogleAnalytics = model.GoogleAnalytics;
                config.Hotline = model.Hotline;
                config.Email = model.Email;
                config.LiveChat = model.LiveChat;
                config.Twitter = model.Twitter;
                config.Place = model.Place;
                config.Instagram = model.Instagram;
                config.Youtube = model.Youtube;
                config.InfoFooter = model.InfoFooter;
                config.UrlMessenger = model.UrlMessenger;
                config.VideoIntro = model.VideoIntro;
                var file = Request.Files["Image"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        return View(config);
                    }

                    if (file.ContentLength > 4000 * 1024)
                    {
                        ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                        return View(config);
                    }

                    var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);
                    var imgPath = "/images/configs/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    config.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                    file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                }

                var file1 = Request.Files["ImageFooter"];
                if (file1 != null && file1.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file1.FileName, "jpg|jpeg|png|gif"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        return View(config);
                    }

                    if (file1.ContentLength > 4000 * 1024)
                    {
                        ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                        return View(config);
                    }

                    var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file1.FileName);
                    var imgPath = "/images/configs/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    config.ImageFooter = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                    file1.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                }

                _unitOfWork.Save();
                HttpContext.Application["ConfigSite"] = config;
                return RedirectToAction("ConfigSite", "Vcms", new { result = "success" });
            }
            return View("ConfigSite", model);
        }
        #endregion

        #region City
        public ActionResult City()
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult City(City model)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.CityRepository.Insert(model);
                _unitOfWork.Save();
                return RedirectToAction("City");
            }
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ListCity()
        {
            var cities = _unitOfWork.CityRepository.Get(orderBy: q => q.OrderBy(c => c.Sort));
            return PartialView("ListCityPartial", cities);
        }

        public ActionResult EditCity(int cityId = 0)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }

            var city = _unitOfWork.CityRepository.GetById(cityId);
            if (city == null)
            {
                return RedirectToAction("City");
            }
            return View(city);
        }

        [HttpPost]
        public ActionResult EditCity(City model)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.CityRepository.Update(model);
                _unitOfWork.Save();
                return RedirectToAction("City");
            }
            return View(model);
        }

        [HttpPost]
        public bool DeleteCity(int cityId = 0)
        {
            if (Role != RoleAdmin.Admin)
            {
                return false;
            }

            var city = _unitOfWork.CityRepository.GetById(cityId);
            if (city == null)
            {
                return false;
            }

            city.Active = false;
            //_unitOfWork.CityRepository.Delete(city);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region District
        public ActionResult AddOrUpdateDistrict(int? districtId, int cityId, int result = 0)
        {
            if (Role != RoleAdmin.Admin)
            {
                return RedirectToAction("Index");
            }

            var model = new District
            {
                CityId = cityId
            };
            if (districtId.HasValue)
            {
                model = _unitOfWork.DistrictRepository.GetById(districtId);
            }
            ViewBag.Districts = _unitOfWork.DistrictRepository.Get(a => a.CityId == cityId, q => q.OrderBy(a => a.Sort));
            return View(model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateDistrict(District model)
        {
            if (Role != RoleAdmin.Admin)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.DistrictRepository.Insert(model);
                _unitOfWork.Save();
                return RedirectToAction("AddOrUpdateDistrict", new { cityId = model.CityId, result = 1 });
            }
            ViewBag.Districts = _unitOfWork.DistrictRepository.Get(a => a.CityId == model.CityId, q => q.OrderBy(a => a.Sort));
            return View(model);
        }

        [HttpPost]
        public bool DeleteDistrict(int districtId = 0)
        {
            if (Role != RoleAdmin.Admin)
            {
                return false;
            }

            var district = _unitOfWork.DistrictRepository.GetById(districtId);
            if (district == null)
            {
                return false;
            }

            district.Active = false;
            //_unitOfWork.DistrictRepository.Delete(district);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region CodeDiscount

        //[ChildActionOnly]
        //public PartialViewResult ListCodeDiscount()
        //{
        //    var code = _unitOfWork.DiscountCodeRepository.Get();
        //    return PartialView("ListCodeDiscount", code);
        //}

        public ActionResult ListCodeDiscount(int? page, string name, string fromdate, string todate, int pageSize = 50, int status = 3, int type = 3, int hsd = 0)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }

            var pageNumber = page ?? 1;
            //const int pageSize = 15;
            var code = _unitOfWork.DiscountCodeRepository.GetQuery(orderBy: l => l.OrderByDescending(a => a.Id));
            if (!string.IsNullOrEmpty(name))
            {
                code = code.Where(l => l.Fullname.ToLower().Contains(name.ToLower()));
            }
            if (DateTime.TryParse(fromdate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var fd))
            {
                code = code.Where(a => DbFunctions.TruncateTime(a.CreateDate) >= DbFunctions.TruncateTime(fd));
            }
            if (DateTime.TryParse(todate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var td))
            {
                code = code.Where(a => DbFunctions.TruncateTime(a.CreateDate) <= DbFunctions.TruncateTime(td));
            }
            if (status == 0)
            {
                code = code.Where(a => a.Active == true);
            }
            else if (status == 1)
            {
                code = code.Where(a => a.Active == false);

            }

            if (type == 0)
            {
                code = code.Where(a => a.TypeDiscount == TypeDiscount.OneTimeuse);
            }
            else if (status == 1)
            {
                code = code.Where(a => a.TypeDiscount == TypeDiscount.UsedManyTimes);

            }

            if (hsd == 1)
            {
                code = code.Where(a => a.ExpDay != null && a.ExpDay < DateTime.Now);
            }
            else if (hsd == 2)
            {
                code = code.Where(a => a.ExpDay == null || a.ExpDay > DateTime.Now);

            }
            else if (status == 2)
            {
                code = code.Where(a => a.TypeDiscount == TypeDiscount.UsedManyTimes);

            }

            var model = new ListCodeDiscountViewModel
            {
                DiscountCodes = code.ToPagedList(pageNumber, pageSize),
                PageSize = pageSize,
                Status = status,
                FromDate = fromdate,
                ToDate = todate,
                Name = name,
                Type = type,
                HSD = hsd,
            };
            return View(model);
        }

        public ActionResult CreateCodeAll(string result = "")
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }

            var model = new CodeDiscountViewModel
            {
                DiscountCode = new DiscountCode(),

            };
            ViewBag.Result = result;
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateCodeAll(CodeDiscountViewModel model, FormCollection fc)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var isPost = true;
                var count = Convert.ToInt32(fc["number"]);

                for (var i = 1; i <= count; i++)
                {
                    string randomStr = "";
                    string[] myIntArray = new string[12];
                    int x;
                    Random autoRand = new Random();
                    for (x = 0; x < 10; x++)
                    {
                        myIntArray[x] = Convert.ToChar(Convert.ToInt32(autoRand.Next(65, 87))).ToString();
                        randomStr += (myIntArray[x].ToString());
                    }
                    model.DiscountCode.Fullname = randomStr;


                    if (model.Price != null)
                    {
                        model.DiscountCode.Price = Convert.ToDecimal(model.Price.Replace(".", ""));
                    }
                    if (isPost)
                    {
                        //model.DiscountCode.Price = Convert.ToDecimal(model.Price.Replace(".", ""));
                        _unitOfWork.DiscountCodeRepository.Insert(model.DiscountCode);
                        var code = _unitOfWork.DiscountCodeRepository.GetQuery(a => a.Fullname.Equals(model.DiscountCode.Fullname)).Count();
                        if (code > 0)
                        {
                            model.DiscountCode.Fullname += +model.DiscountCode.Id;
                            _unitOfWork.Save();
                        }
                        _unitOfWork.Save();
                        return RedirectToAction("CreateCodeAll", new { result = "success" });
                    }

                }
                //var isPost = true;
                //var code = _unitOfWork.DiscountCodeRepository.GetQuery(a => a.Fullname.Equals(model.DiscountCode.Fullname)).SingleOrDefault();
                //if (code != null)
                //{

                //    ModelState.AddModelError("", @"Mã code này đã tồn tại");
                //    isPost = false;
                //}

            }
            return View(model);
        }

        public ActionResult CodeDiscount(string result = "")
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }

            string randomStr = "";
            string[] myIntArray = new string[12];
            int x;
            Random autoRand = new Random();
            for (x = 0; x < 10; x++)
            {
                myIntArray[x] = Convert.ToChar(Convert.ToInt32(autoRand.Next(65, 87))).ToString();
                randomStr += (myIntArray[x].ToString());
            }

            //Random random = new Random();
            //for (int i = 0; i < 10; i++)
            //{
            //    string s = random.Nex;
            //    Console.WriteLine($"Chuỗi {i + 1}: {s}");
            //}

            var model = new CodeDiscountViewModel
            {
                DiscountCode = new DiscountCode
                {
                    Fullname = randomStr,
                }
            };
            ViewBag.Result = result;
            return View(model);
        }

        [HttpPost]
        public ActionResult CodeDiscount(CodeDiscountViewModel model)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var isPost = true;
                var code = _unitOfWork.DiscountCodeRepository.GetQuery(a => a.Fullname.Equals(model.DiscountCode.Fullname)).SingleOrDefault();
                if (code != null)
                {
                    ModelState.AddModelError("", @"Mã code này đã tồn tại");
                    isPost = false;
                }
                if (model.Price != null)
                {
                    model.DiscountCode.Price = Convert.ToDecimal(model.Price.Replace(".", ""));
                }
                if (isPost)
                {
                    //model.DiscountCode.Price = Convert.ToDecimal(model.Price.Replace(".", ""));
                    _unitOfWork.DiscountCodeRepository.Insert(model.DiscountCode);
                    _unitOfWork.Save();
                    return RedirectToAction("CodeDiscount", new { result = "success" });
                }
            }
            return View(model);
        }

        public ActionResult EditCodeDiscount(int codeId = 0)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }
            var code = _unitOfWork.DiscountCodeRepository.GetById(codeId);
            if (code == null)
            {
                return RedirectToAction("CodeDiscount");
            }
            var model = new CodeDiscountViewModel
            {
                DiscountCode = code,
                Price = code.Price?.ToString("N0")
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditCodeDiscount(CodeDiscountViewModel model)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var code = _unitOfWork.DiscountCodeRepository.GetQuery(a => a.Fullname.Equals(model.DiscountCode.Fullname)).SingleOrDefault();
                if (code == null)
                {
                    return RedirectToAction("CodeDiscount");
                }
                if (model.Price != null)
                {
                    code.Price = Convert.ToDecimal(model.Price.Replace(",", ""));
                }
                code.Fullname = model.DiscountCode.Fullname;
                code.Active = model.DiscountCode.Active;
                code.ExpDay = model.DiscountCode.ExpDay;
                code.TypeDiscount = model.DiscountCode.TypeDiscount;
                code.Discount = model.DiscountCode.Discount;
                _unitOfWork.Save();
                return RedirectToAction("CodeDiscount", new { result = "update" });
            }
            return View(model);
        }

        [HttpPost]
        public bool DeleteCodeDiscount(int codeId = 0)
        {
            if (Role != RoleAdmin.Admin)
            {
                return false;
            }

            var code = _unitOfWork.DiscountCodeRepository.GetById(codeId);
            if (code == null)
            {
                return false;
            }
            _unitOfWork.DiscountCodeRepository.Delete(codeId);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        public void ExportCodeDiscount(int? cityId, string fromdate, string todate, int status, int type, int payment = 0)
        {


            var codes = _unitOfWork.DiscountCodeRepository.GetQuery(orderBy: q => q.OrderByDescending(a => a.Id));

            if (DateTime.TryParse(fromdate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var fd))
            {
                codes = codes.Where(a => DbFunctions.TruncateTime(a.CreateDate) >= DbFunctions.TruncateTime(fd));
            }
            if (DateTime.TryParse(todate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var td))
            {
                codes = codes.Where(a => DbFunctions.TruncateTime(a.CreateDate) <= DbFunctions.TruncateTime(td));
            }

            if (status == 0)
            {
                codes = codes.Where(a => a.Active == true);
            }
            else if (status == 1)
            {
                codes = codes.Where(a => a.Active == false);

            }
            if (type == 0)
            {
                codes = codes.Where(a => a.TypeDiscount == TypeDiscount.OneTimeuse);
            }
            else if (type == 1)
            {
                codes = codes.Where(a => a.TypeDiscount == TypeDiscount.UsedManyTimes);

            }
            //else
            //{
            //    codes = codes.Where(a => a.Status != 3);
            //}


            var items = codes.Select(a => new
            {
                code = a,
                FullName = a.Fullname,
                Discount = a.Discount,
                Price = a.Price,
                ExpDay = a.ExpDay,
                //typeUser = a.User != null ? a.User.TypeUser.ToString() : "Vãng lai"
            });
            var dt = new DataTable();
            dt.Columns.Add("STT");
            dt.Columns.Add("Ngày tạo");
            dt.Columns.Add("Mã code");
            dt.Columns.Add("Số tiền giảm");
            dt.Columns.Add("Số phần trăm giảm");
            dt.Columns.Add("Trạng thái");
            dt.Columns.Add("Loại mã code");
            dt.Columns.Add("Ngày hết hạn");
            //dt.Columns.Add("Địện thoại");
            //dt.Columns.Add("Địa chỉ");
            //dt.Columns.Add("Phường xã");
            //dt.Columns.Add("Quận huyện");
            //dt.Columns.Add("Thành phố");
            //dt.Columns.Add("Loại KH");

            var filename = $"danh-sach-ma-code.xlsx";
            var i = 1;
            foreach (var item in items)
            {
                dt.Rows.Add(i, item.code.CreateDate.ToString("dd/MM/yyyy HH:mm"), item.code.Fullname, item.code.Price/*.ToString().Substring(0,-3)*/, item.code.Discount, item.code.Active, item.code.TypeDiscount, item.code.ExpDay);
                i++;
            }
            using (var pck = new ExcelPackage())
            {
                //Create the worksheet
                var ws = pck.Workbook.Worksheets.Add("Danh sách mã code");

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(dt, true);

                //Format the header for column 1-14
                using (var rng = ws.Cells["A1:O1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                //Example how to Format Column 7 as numeric
                //using (var col = ws.Cells[2, 7, 2 + dt.Rows.Count, 7])
                //{
                //    col.Style.Numberformat.Format = "#,##0";
                //    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //}

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename + "");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        public JsonResult GetTags(string term)
        {
            var tags = _unitOfWork.DiscountCodeRepository.GetQuery(t => t.Active);

            var tag = tags.Where(x => x.Fullname.ToLower().Contains(term.ToLower()));
            var cout = tag.Count();
            return Json(tag.Select(a => new { label = a.Fullname }), JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}