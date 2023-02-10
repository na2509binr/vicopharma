using Ephyta.DAL;
using Ephyta.Filters;
using Ephyta.Models;
using Ephyta.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Ephyta.Controllers
{
    [Authorize, AdminRoleFilters]
    public class ContactController : Controller
    {
        // GET: Contact
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private RoleAdmin Role => (RoleAdmin)Enum.Parse(typeof(RoleAdmin), RouteData.Values["Role"].ToString());

        #region Contact
        public ActionResult ListContact(int? page, string name)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToActionPermanent("Index", "Vcms");
            }
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var contact = _unitOfWork.ContactRepository.Get(orderBy: l => l.OrderByDescending(a => a.Id));
            if (!string.IsNullOrEmpty(name))
            {
                contact = contact.Where(l => l.Email.ToLower().Contains(name.ToLower()));
            }
            var model = new ListContactViewModel
            {
                Contacts = contact.ToPagedList(pageNumber, pageSize),
                Name = name
            };
            return View(model);
        }
        [HttpPost]
        public bool DeleteContact(int contactId = 0)
        {
            if (Role != RoleAdmin.Admin)
            {
                return false;
            }

            var contact = _unitOfWork.ContactRepository.GetById(contactId);
            if (contact == null)
            {
                return false;
            }
            _unitOfWork.ContactRepository.Delete(contact);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Feedback
        public ActionResult ListFeeback(int? page, string name, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 10;
            var feedback = _unitOfWork.FeedbackRepository.Get(orderBy: l => l.OrderByDescending(a => a.Id));
            if (!string.IsNullOrEmpty(name))
            {
                feedback = feedback.Where(l => l.Name.ToLower().Contains(name.ToLower()));
            }
            var model = new ListFeedbackViewModel
            {
                Feedbacks = feedback.ToPagedList(pageNumber, pageSize),
                Name = name
            };
            return View(model);
        }
        public ActionResult Feedback()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Feedback(Feedback model)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Image"];
                var json = HtmlHelpers.UploadFile(file, "/images/feedbacks/");
                if (json != null)
                {
                    int status = json.GetType().GetProperty("status").GetValue(json, null);
                    if (status == 1)
                    {
                        string msg = json.GetType().GetProperty("msg").GetValue(json, null);
                        ModelState.AddModelError("", msg);
                        isPost = false;
                    }
                    else
                    {
                        string fileUrl = json.GetType().GetProperty("fileUrl").GetValue(json, null);
                        model.Image = fileUrl;
                    }
                }
                //if (file != null && file.ContentLength > 0)
                //{
                //    if (file.ContentType != "image/jpeg" & file.ContentType != "image/png" && file.ContentType != "image/gif")
                //    {
                //        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                //    }
                //    else
                //    {
                //        if (file.ContentLength > 4000 * 1024)
                //        {
                //            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                //        }
                //        else
                //        {
                //            var imgPath = "/images/feedbacks/" + DateTime.Now.ToString("yyyy/MM/dd");
                //            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                //            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                //            model.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                //            var newImage = Image.FromStream(file.InputStream);
                //            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 600, 600, false);
                //            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                //        }
                //    }
                //}
                if (isPost)
                {
                    _unitOfWork.FeedbackRepository.Insert(model);
                    _unitOfWork.Save();
                    return RedirectToAction("ListFeeback", new { result = "success" });
                }
            }
            return View(model);
        }
        public ActionResult UpdateFeedback(int feedbackId = 0)
        {
            var feedback = _unitOfWork.FeedbackRepository.GetById(feedbackId);
            if (feedback == null)
            {
                return RedirectToAction("ListFeeback");
            }
            return View(feedback);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateFeedback(Feedback model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Image"];
                var json = HtmlHelpers.UploadFile(file, "/images/feedbacks/");
                if (json != null)
                {
                    int status = json.GetType().GetProperty("status").GetValue(json, null);
                    if (status == 1)
                    {
                        string msg = json.GetType().GetProperty("msg").GetValue(json, null);
                        ModelState.AddModelError("", msg);
                        isPost = false;
                    }
                    else
                    {
                        string fileUrl = json.GetType().GetProperty("fileUrl").GetValue(json, null);
                        model.Image = fileUrl;
                    }
                }
                //if (file != null && file.ContentLength > 0)
                //{
                //    if (file.ContentType != "image/jpeg" & file.ContentType != "image/png" && file.ContentType != "image/gif")
                //    {
                //        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                //    }
                //    else
                //    {
                //        if (file.ContentLength > 4000 * 1024)
                //        {
                //            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                //        }
                //        else
                //        {
                //            var imgPath = "/images/feedbacks/" + DateTime.Now.ToString("yyyy/MM/dd");
                //            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                //            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                //            if (System.IO.File.Exists(Server.MapPath("/images/feedbacks/" + model.Image)))
                //            {
                //                System.IO.File.Delete(Server.MapPath("/images/feedbacks/" + model.Image));
                //            }
                //            model.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                //            var newImage = Image.FromStream(file.InputStream);
                //            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 600, 600, false);
                //            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                //        }
                //    }
                //}
                else
                {
                    model.Image = fc["CurrentFile"];
                }

                if (isPost)
                {
                    _unitOfWork.FeedbackRepository.Update(model);
                    _unitOfWork.Save();

                    return RedirectToAction("ListFeeback", new { result = "update" });
                }
            }
            return View(model);
        }
        [HttpPost]
        public bool DeleteFeedback(int feedbackId = 0)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return false;
            }

            var feedback = _unitOfWork.FeedbackRepository.GetById(feedbackId);
            if (feedback == null)
            {
                return false;
            }
            _unitOfWork.FeedbackRepository.Delete(feedback);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Register
        public ActionResult ListRegister(int? page, string phone)
        {
            if (Role == RoleAdmin.Copywriter)
            {
                return RedirectToActionPermanent("Index", "Vcms");
            }

            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var contact = _unitOfWork.RegisterRepository.Get(orderBy: l => l.OrderByDescending(a => a.Id));
            if (!string.IsNullOrEmpty(phone))
            {
                contact = contact.Where(l => l.Mobile.ToLower().Contains(phone.ToLower()));
            }
            var model = new ListRegisterViewModel
            {
                Registers = contact.ToPagedList(pageNumber, pageSize),
                Phone = phone
            };
            return View(model);
        }
        [HttpPost]
        public bool DeleteRegister(int contactId = 0)
        {
            if (Role != RoleAdmin.Admin)
            {
                return false;
            }

            var contact = _unitOfWork.RegisterRepository.GetById(contactId);
            if (contact == null)
            {
                return false;
            }
            _unitOfWork.RegisterRepository.Delete(contact);
            _unitOfWork.Save();
            return true;
        }
        #endregion


        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}