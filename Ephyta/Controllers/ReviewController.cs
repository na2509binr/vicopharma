using Ephyta.DAL;
using Ephyta.Models;
using Ephyta.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ephyta.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IEnumerable<ReviewKol> ReviewKols => _unitOfWork.ReviewKolRepository.Get();
        private IEnumerable<Product> Products => _unitOfWork.ProductRepository.GetQuery(l => l.Active);
        #region ReviewKol  

        public ActionResult ListReviewKol(int? page, int? productId, string name, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var reviewKols = _unitOfWork.ReviewKolRepository.GetQuery(orderBy: q => q.OrderBy(a => a.CreateDate)).AsNoTracking();

            if (productId > 0)
            {
                reviewKols = reviewKols.Where(r => r.ProductId == productId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                reviewKols = reviewKols.Where(l => l.Name.ToLower().Contains(name.ToLower()));
            }

            var model = new ListReviewKolViewModel
            {
                //SelectProducts = new SelectList(Products.Where(a => a.Active), "Id", "Name"),
                Products = Products,
                ReviewKols = reviewKols.ToPagedList(pageNumber, pageSize),            
                Name = name,
            };
            return View(model);
        }

        public ActionResult ReviewKol()
        {
           
            var model = new InsertReviewKolViewModel
            {
                Products = Products,
                ReviewKol = new ReviewKol { Active = true, Sort = 1 }
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ReviewKol(InsertReviewKolViewModel model, FormCollection fc)
        {

            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["ReviewKol.Image"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        isPost = false;
                    }
                    else
                    {

                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            //var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);
                            var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(file.FileName)) + "-" + DateTime.Now.Millisecond + Path.GetExtension(file.FileName);
                            var imgPath = "/images/reviewKols/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            model.ReviewKol.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                            //file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 800, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }

                if (isPost)
                {
                    model.ReviewKol.ProductId = Convert.ToInt32(fc["ProductId"]);
                    _unitOfWork.ReviewKolRepository.Insert(model.ReviewKol);
                    _unitOfWork.Save();
                    return RedirectToAction("ListReviewKol", new { result = "success" });
                }
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteReviewKol(int reviewId = 0)
        {
            var review = _unitOfWork.ReviewKolRepository.GetById(reviewId);
            if (review == null)
            {
                return Json(new { status = false });
            }
            _unitOfWork.ReviewKolRepository.Delete(review);
            _unitOfWork.Save();
            return Json(new { status = true });
        }

        [HttpPost]
        public bool QuickUpdateReviewKol(bool? status, bool active, int proId = 0)
        {

            var product = _unitOfWork.ReviewKolRepository.GetById(proId);
            if (product == null)
            {
                return false;
            }
            if (status != null)
            {
                product.Active = Convert.ToBoolean(status);
            }
            product.Active = active;
            _unitOfWork.Save();
            return true;
        }
        #endregion

        public ActionResult ListReviewFalse(int? page, int? productId, string name, string result = "",  string sort = "date-desc")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var reviews = _unitOfWork.ReviewRepository.GetQuery( l => l.Active == false, l => l.OrderByDescending(a => a.CreateDate)).AsNoTracking();
            if(productId > 0)
            {
                reviews = reviews.Where(r => r.ProductId == productId);
            }

            var model = new ListReviewViewModel
            {
                //SelectProducts = new SelectList(Products.Where(a => a.Active), "Id", "Name"),
                Reviews = reviews.ToPagedList(pageNumber, pageSize),
                Name = name,
                Products = Products,
                ProductId = productId,
            };
            return View(model);
        }

        public ActionResult ListReview(int? page, int? productId, string name, string result = "", string sort = "date-desc")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var reviews = _unitOfWork.ReviewRepository.GetQuery(l => l.Active , l => l.OrderByDescending(a => a.CreateDate)).AsNoTracking();
            if (productId > 0)
            {
                reviews = reviews.Where(r => r.ProductId == productId);
            }

            var model = new ListReviewViewModel
            {
                //SelectProducts = new SelectList(Products.Where(a => a.Active), "Id", "Name"),
                Reviews = reviews.ToPagedList(pageNumber, pageSize),
                Name = name,
                Products = Products,
                ProductId = productId,
            };
            return View(model);
        }

        public ActionResult Review()
        {

            var model = new InsertReviewViewModel
            {
                Products = Products,
                Review = new Review { Active = true,StarReview = StarReview.Five}
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Review(InsertReviewViewModel model, FormCollection fc)
        {

            if (ModelState.IsValid)
            {
                var isPost = true;
                model.Review.ListImage = fc["Pictures"];
                var file = Request.Files["Review.ImageAvata"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        isPost = false;
                    }
                    else
                    {

                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            //var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);
                            var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(file.FileName)) + "-" + DateTime.Now.Millisecond + Path.GetExtension(file.FileName);
                            var imgPath = "/images/reviews/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            model.Review.ImageAvata = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                            //file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 800, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }

                if (isPost)
                {
                    model.Review.ProductId = Convert.ToInt32(fc["ProductId"]);
                    _unitOfWork.ReviewRepository.Insert(model.Review);
                    _unitOfWork.Save();
                    return RedirectToAction("ListReview", new { result = "success" });
                }
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteReview(int reviewId = 0)
        {
            var review = _unitOfWork.ReviewRepository.GetById(reviewId);
            if (review == null)
            {
                return Json(new { status = false });
            }
            _unitOfWork.ReviewRepository.Delete(review);
            _unitOfWork.Save();
            return Json(new { status = true });
        }

        [HttpPost]
        public bool QuickUpdateReview(bool? status, bool active, int proId = 0)
        {

            var product = _unitOfWork.ReviewRepository.GetById(proId);
            if (product == null)
            {
                return false;
            }
            if (status != null)
            {
                product.Active = Convert.ToBoolean(status);
            }
            product.Active = active;
            _unitOfWork.Save();
            return true;
        }


      
    }
}