using Ephyta.DAL;
using Ephyta.Models;
using Ephyta.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using WebMarkupMin.AspNet4.Mvc;

namespace Ephyta.Controllers
{
    public class HomeController : Controller
    {
        private static string Email => WebConfigurationManager.AppSettings["email"];
        private static string Password => WebConfigurationManager.AppSettings["password"];
        private ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["Config"];

        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        private IEnumerable<Banner> Banners => _unitOfWork.BannerRepository.Get(a => a.Active, q => q.OrderBy(a => a.Sort));
        private IEnumerable<ArticleCategory> ArticleCategories => _unitOfWork.ArticleCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));
        private IEnumerable<ProductCategory> ProductCategories => _unitOfWork.ProductCategoryRepository.Get(a => a.Active, q => q.OrderBy(a => a.Sort));

        [MinifyXhtml, CompressContent]
        public ActionResult Index()
        {
            var items = _unitOfWork.ProductRepository
                .GetQuery(l => l.Active && l.Home && l.Hot, a => a.OrderByDescending(c => c.Id)).Select(a =>
                    new ItemProductViewModel
                    {
                        Product = a,
                        Rating = a.Reviews.Where(c => c.Active).Average(c => (decimal?)c.StarReview) ?? 0
                    }).Take(12);

            var combos = _unitOfWork.ProductRepository
                .GetQuery(l => l.Active && l.ProductCategory.TypeProduct == TypeProduct.Combo && l.SaleOff != null, a => a.OrderByDescending(c => c.Id)).Select(a =>
                    new ItemProductViewModel
                    {
                        Product = a,
                        Rating = a.Reviews.Where(c => c.Active).Average(c => (decimal?)c.StarReview) ?? 0
                    }).Take(12);

            var allProducts = _unitOfWork.ProductRepository
                .GetQuery(l => l.Active && l.ProductCategory.TypeProduct == TypeProduct.Product, a => a.OrderByDescending(c => c.Id)).Select(a =>
                    new ItemProductViewModel
                    {
                        Product = a,
                        Rating = a.Reviews.Where(c => c.Active).Average(c => (decimal?)c.StarReview) ?? 0
                    }).Take(12);


            var model = new HomeViewModel
            {
                ProductCategories = ProductCategories.Where(l => l.Home),
                Articles = _unitOfWork.ArticleRepository.GetQuery(l => l.Active, a => a.OrderByDescending(c => c.CreateDate)),
                Products = items,// _unitOfWork.ProductRepository.GetQuery(l => l.Active && l.Home, a => a.OrderByDescending(c => c.Id)),
                ComboProducts = combos,
                AllProducts = allProducts,
                Feedbacks = _unitOfWork.FeedbackRepository.Get(l => l.Active, a => a.OrderByDescending(c => c.Id), 20),
                Banners = Banners
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var model = new HeaderViewModel
            {
                ProductCategories = ProductCategories,
                ArticleCategories = ArticleCategories.Where(l => l.ShowMenu),
                Products = _unitOfWork.ProductRepository.GetQuery(l => l.Active, l => l.OrderByDescending(a => a.Id))
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooterViewModel
            {
                ArticleCategories = ArticleCategories,
                Articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.ArticleCategory.TypePost == TypePost.Policy, q => q.OrderBy(a => a.CreateDate))
            };
            return PartialView(model);
        }
        [MinifyXml, CompressContent]
        [Route("category/san-pham")]
        public ActionResult AllProductCategory(int? page, string sort)
        {
            var products = _unitOfWork.ProductRepository.GetQuery(l => l.Active, l => l.OrderByDescending(a => a.Id));
            switch (sort)
            {
                case "name":
                    products = products.OrderBy(a => a.Name);
                    break;
                case "price":
                    products = products.OrderBy(a => a.Price);
                    break;
                case "price-desc":
                    products = products.OrderByDescending(a => a.Price);
                    break;
                case "sort-desc":
                    products = products.OrderByDescending(a => a.Sort);
                    break;
                default:
                    products = products.OrderByDescending(a => a.Id);
                    break;
            }
            var pageNumber = page ?? 1;

            var items = products.Select(a => new ItemProductViewModel
            {
                Product = a,
                Rating = a.Reviews.Where(c => c.Active).Average(c => (decimal?)c.StarReview) ?? 0
            });

            var model = new CategoryProductViewModel
            {
                Products = items.ToPagedList(pageNumber, 12),
                Sort = sort
            };
            return View(model);
        }
        [MinifyXml, CompressContent]
        [Route("category/{url}.html")]
        public ActionResult ProductCategory(int? page, string sort, string url)
        {
            var category = ProductCategories.SingleOrDefault(a => a.Url == url);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            var products = _unitOfWork.ProductRepository.GetQuery(l => l.Active && (l.ProductCategoryId == category.Id || l.ProductCategory.ParentId == category.Id), c => c.OrderByDescending(a => a.Id));
            switch (sort)
            {
                case "name":
                    products = products.OrderBy(a => a.Name);
                    break;
                case "price":
                    products = products.OrderBy(a => a.Price);
                    break;
                case "price-desc":
                    products = products.OrderByDescending(a => a.Price);
                    break;
                case "sort-desc":
                    products = products.OrderByDescending(a => a.Sort);
                    break;
                default:
                    products = products.OrderByDescending(a => a.Id);
                    break;
            }
            var pageNumber = page ?? 1;

            var items = products.Select(a => new ItemProductViewModel
            {
                Product = a,
                Rating = a.Reviews.Where(c => c.Active).Average(c => (decimal?)c.StarReview) ?? 0
            });


            var model = new CategoryProductViewModel
            {
                Products = items.ToPagedList(pageNumber, 12),
                Category = category,
                Sort = sort,
                ParentId = category.ParentId ?? category.Id,
            };
            return View(model);
        }
        [MinifyXml, CompressContent]
        [Route("{url:regex(^(?!.*(vcms|article|banner|contact|product|uploader)).*$)}")]
        public ActionResult ProductDetail(string url)
        {
            var product = _unitOfWork.ProductRepository.GetQuery(a => a.Active && a.Url == url).SingleOrDefault();
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            var products = _unitOfWork.ProductRepository.GetQuery(l => l.Active && l.ProductCategoryId == product.ProductCategoryId && l.Id != product.Id, a => a.OrderByDescending(c => c.Id));

            var items = products.Select(a => new ItemProductViewModel
            {
                Product = a,
                Rating = a.Reviews.Where(c => c.Active).Average(c => (decimal?)c.StarReview) ?? 0
            }).Take(4);

            var itemHots = _unitOfWork.ProductRepository
                .GetQuery(l => l.Active && l.Hot, a => a.OrderByDescending(c => c.Id)).Select(a =>
                    new ItemProductViewModel
                    {
                        Product = a,
                        Rating = a.Reviews.Where(c => c.Active).Average(c => (decimal?)c.StarReview) ?? 0
                    }).Take(4);

            var model = new ProductDetailViewModel
            {
                ReviewKols = _unitOfWork.ReviewKolRepository.Get(l => l.Active && l.ProductId == product.Id),
                //Reviews = product.Reviews.Where(a => a.Active).OrderByDescending(a => a.Id),
                Product = product,
                Products = items,
                ProductHots = itemHots,
                //Rating = Math.Round(rating, 1),
            };
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult MenuProduct(int? parentId)
        {
            var model = new MenuProductViewModel
            {
                ProductCategories = ProductCategories,
                Products = _unitOfWork.ProductRepository.GetQuery(l => l.Active, a => a.OrderByDescending(c => c.Id), 4),
                ParentId = parentId,
            };
            return PartialView(model);
        }
        [MinifyXml, CompressContent]
        [Route("blog")]
        public ActionResult AllArticleCategory(int? page)
        {
            var products = _unitOfWork.ArticleRepository.GetQuery(l => l.Active, l => l.OrderByDescending(a => a.CreateDate));
            var pageNumber = page ?? 1;
            var model = new CategoryAticleViewModel
            {
                Articles = products.ToPagedList(pageNumber, 6),
            };
            return View(model);
        }
        [MinifyXml, CompressContent]
        [Route("blog/{url}", Order = 2)]
        public ActionResult ArticleCategory(int? page, string url)
        {
            var category = ArticleCategories.SingleOrDefault(a => a.Url == url);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            var article = _unitOfWork.ArticleRepository.GetQuery(
                a => a.Active && (a.ArticleCategoryId == category.Id || a.ArticleCategory.ParentId == category.Id),
                q => q.OrderByDescending(a => a.CreateDate));
            var pageNumber = page ?? 1;
            if (article.Count() == 1)
            {
                var fi = article.First();
                return RedirectToAction("ArticleDetail", new { url = fi.Url });
            }
            var model = new CategoryAticleViewModel
            {
                Articles = article.ToPagedList(pageNumber, 6),
                Category = category,
            };
            return View(model);
        }
        [MinifyXml, CompressContent]
        [Route("blog/{url}.html", Order = 1)]
        public ActionResult ArticleDetail(string url)
        {
            var article = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.Url == url).SingleOrDefault();
            if (article == null)
            {
                return RedirectToAction("Index");
            }
            var articles = _unitOfWork.ArticleRepository.GetQuery(l => l.Active && l.ArticleCategoryId == article.ArticleCategoryId && l.Id != article.Id, c => c.OrderByDescending(a => a.CreateDate), 3);
            var model = new ArticleDetailViewModel
            {
                Article = article,
                Articles = articles
            };
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult MenuArticle()
        {
            var model = new MenuArticleViewModel
            {
                ArticleCategories = ArticleCategories,
                Articles = _unitOfWork.ArticleRepository.GetQuery(l => l.Active, a => a.OrderByDescending(c => c.Id), 5),
                TopViews = _unitOfWork.ArticleRepository.GetQuery(l => l.Active, q => q.OrderByDescending(a => a.View), 5),
            };
            return PartialView(model);
        }
        [MinifyXml, CompressContent]
        [Route("lien-he")]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Contact(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }
            _unitOfWork.ContactRepository.Insert(model);
            _unitOfWork.Save();
            var subject = "melinka.vn - Email liên hệ từ: " + model.Email;
            var body = $"<p>Tên người liên hệ: {model.Fullname},</p>" +
                        $"<p>Địa chỉ email người liên hệ: {model.Email},</p>" +
                        $"<p>Số điện thoại: {model.Mobile},</p>" +
                        $"Địa chỉ: {model.Address},</p>" +
                        $"</p>Nội dung:{model.Body}</p>";

            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSite.Email, Email,
           Email, Password, "Ephyta"));

            return Json(new { status = true, msg = "Gửi liên hệ thành công.\nChúng tôi sẽ liên lạc lại với bạn sớm nhất có thể." });
        }
        [MinifyXml, CompressContent]
        [Route("tim-kiem")]
        public ActionResult Search(int? page, string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                return RedirectToAction("Index");
            }
            var products = _unitOfWork.ProductRepository.GetQuery(l => l.Active && l.Name.ToLower().Contains(keywords.ToLower()), a => a.OrderByDescending(c => c.Id));
            var pageNumber = page ?? 1;
            var model = new SearchViewModel
            {
                Keywords = keywords,
                Products = products.ToPagedList(pageNumber, 6),
            };
            return View(model);
        }
        [Route("dat-hang-nhanh")]
        [HttpPost]
        public bool QuickOrder(FormCollection fc)
        {
            try
            {
                var name = fc["FullName"];
                var address = fc["Address"];
                var email = fc["Email"];
                var mobile = fc["Mobile"];
                var content = fc["Content"];
                var proName = fc["ProductName"];
                var proPrice = fc["productPrice"];
                var quantity = fc["Quantity"];
                var proUrl = fc["ProductUrl"];
                var proImg = fc["ProductImg"];
                var tongtien = 0;
                if (!string.IsNullOrEmpty(proPrice))
                {
                    tongtien = Convert.ToInt32(proPrice) * Convert.ToInt32(quantity);
                }
                var body = new StringBuilder();
                body.Append("<p>Xin chào,</p>");
                body.Append("<p>Dưới đây là thông tin đặt hàng website " + Request.Url?.Host + ":</p>");
                body.Append("<p>Sản phẩm: <strong>" + proName + "</strong></p>");
                body.Append("<p>Giá: <strong>" + proPrice + "</strong></p>");
                body.Append("<p>Số lượng: <strong>" + quantity + "</strong></p>");
                body.Append("<p>Tổng tiền: <strong>" + tongtien.ToString("N0") + "</strong></p>");
                body.Append("<p>Hình ảnh: <img src='" + Request.Url?.GetLeftPart(UriPartial.Authority) + "/images/products/" + proImg + "?w=200' /></p>");
                body.Append("<p>Link SP: " + Request.Url?.GetLeftPart(UriPartial.Authority) + proUrl + "</p>");
                body.Append("<p>Họ và tên: " + name + "</p>");
                body.Append("<p>Di động: " + mobile + "</p>");
                body.Append("<p>Email: " + email + "</p>");
                body.Append("<p>Địa chỉ: " + address + "</p>");
                body.Append("<p>Nội dung: " + content + "</p>");
                body.Append("<p>Ngày đặt: " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + "</p>");
                body.Append("<p>Đây là email tự động vui lòng không phản hồi lại, vì phản hồi lại chúng tôi sẽ không tiếp nhận được thông tin.</p>");
                var subject = "Đặt hàng từ website " + Request.Url?.Host;

                Task.Run(() =>
                {
                    HtmlHelpers.SendEmail("gmail", subject, body.ToString(), ConfigSite.Email, Email, Email, Password, "Đặt hàng Online - EPHYTA", email, "maiph0978@gmail.com");
                });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        [ChildActionOnly]
        public PartialViewResult FormRegister()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult FormRegister(Register model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }
            if (string.IsNullOrEmpty(model.Fullname))
            {
                model.Fullname = "Unknown";
            }
            if (string.IsNullOrEmpty(model.Note))
            {
                model.Note = "Tôi muốn nhận tư vấn";
            }

            _unitOfWork.RegisterRepository.Insert(model);
            _unitOfWork.Save();

            var subject = "Email liên hệ từ website: " + Request.Url?.Host;
            var body = $"<p>Tên người liên hệ: {model.Fullname},</p>" +
                        $"<p>Số điện thoại:{model.Mobile}</p>" +
                        $"<p>Khung giờ tư vấn:{model.Time}</p>" +
                        $"<p>Tên sản phẩm:{model.Product}</p>" +
                        $"<p>Ghi chú:{model.Note}</p>" +
                        $"<p>Đây là hệ thống gửi email tự động, vui lòng không phản hồi lại email này.</p>";

            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSite.Email, Email, Email, Password, "Ephyta"));

            return Json(new { status = true, msg = "Gửi liên hệ thành công.\nChúng tôi sẽ liên lạc lại với bạn sớm nhất có thể." });
        }


        public ActionResult GetReview(int? page, int id)
        {
            var review = _unitOfWork.ReviewRepository.GetQuery(l => l.Active && l.ProductId == id, c => c.OrderByDescending(l => l.CreateDate));
            int pageSize = 4;

            int pageNumber = (page ?? 1);
            var model = new GetReviewViewModel
            {
                Reviews = review.ToPagedList(pageNumber, pageSize),
                Id = id,
            };

            return View(model);
        }
        public PartialViewResult ItemProduct(int id)
        {
            var product = _unitOfWork.ProductRepository.GetQuery(a => a.Active && a.Id == id).SingleOrDefault();
            var reviews = _unitOfWork.ReviewRepository.GetQuery(l => l.Active && l.ProductId == product.Id);

            decimal rating = 5;
            if (reviews.Any())
            {
                rating = (decimal)reviews.Average(s => (int)s.StarReview);
            }
            var model = new ItemProductViewModel
            {

                Product = product,
                Rating = Math.Round(rating, 1),
            };
            return PartialView(model);
        }

        [Route("reviews/{url}")]
        public ActionResult FormComment(string url)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToActionPermanent("Index");
            }
            var product = _unitOfWork.ProductRepository.GetQuery(a => a.Active && a.Url == url).FirstOrDefault();
            if (product == null)
            {
                return null;
            }
            var model = new CommentFormViewModel
            {
                Product = product,
                Review = new Review()
            };
            return PartialView(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult FormComment(CommentFormViewModel model, FormCollection fc)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }

            if (string.IsNullOrEmpty(model.Review.Content))
            {
                model.Review.Content = "Sản phẩm rất tốt";
            }
            model.Review.ImageAvata = fc["ImageAvata"];
            model.Review.ListImage = fc["Pictures"];
            _unitOfWork.ReviewRepository.Insert(model.Review);
            _unitOfWork.Save();
            return Json(new { status = true, msg = "Đánh giá thành công" });
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}