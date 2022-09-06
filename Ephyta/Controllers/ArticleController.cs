using Helpers;
using Ephyta.DAL;
using Ephyta.Models;
using Ephyta.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ephyta.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        // GET: Article
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IEnumerable<ArticleCategory> ArticleCategories => _unitOfWork.ArticleCategoryRepository.Get();

        #region ArticleCategory
        [ChildActionOnly]
        public ActionResult ListCategory()
        {
            var allcats = _unitOfWork.ArticleCategoryRepository.Get(orderBy: q => q.OrderBy(a => a.CategorySort));
            return PartialView(allcats);
        }
        public ActionResult ArticleCategory(string result = "")
        {
            ViewBag.ArticleCat = result;
            ViewBag.RootCats =
                new SelectList(
                    _unitOfWork.ArticleCategoryRepository.Get(a => a.ParentId == null,
                                                              q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");
            var articleCat = new ArticleCategory();
            return View(articleCat);
        }
        [HttpPost]
        public ActionResult ArticleCategory(ArticleCategory category)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Image"];
                dynamic json = Utils.UploadFile(file, "/images/articleCategory/");

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
                        category.Image = fileUrl;
                    }
                }

                //if (file != null && file.ContentLength > 0)
                //{
                //    if (file.ContentType != "image/jpeg" & file.ContentType != "image/png" && file.ContentType != "image/gif")
                //    {
                //        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                //        isPost = false;
                //    }
                //    else
                //    {
                //        if (file.ContentLength > 4000 * 1024)
                //        {
                //            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại"); 
                //            isPost = false;
                //        }
                //        else
                //        {
                //            var imgPath = "/images/articleCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                //            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                //            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                //            category.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                //            var newImage = Image.FromStream(file.InputStream);
                //            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 600, 600, false);
                //            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                //            //file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                //        }
                //    }
                //}

                if (isPost)
                {
                    category.Url = HtmlHelpers.ConvertToUnSign(null, category.Url ?? category.CategoryName);
                    _unitOfWork.ArticleCategoryRepository.Insert(category);
                    _unitOfWork.Save();
                    return RedirectToAction("ArticleCategory", new { result = "success" });

                }
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.CategorySort)), "Id", "CategoryName");
            return View(category);
        }
        public ActionResult UpdateCategory(int catId = 0)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetById(catId);
            if (category == null)
            {
                return RedirectToAction("ArticleCategory");
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.CategorySort)), "Id", "CategoryName");
            return View(category);
        }
        [HttpPost]
        public ActionResult UpdateCategory(ArticleCategory category, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Image"];
                dynamic json = Utils.UploadFile(file, "/images/articleCategory/");

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
                        category.Image = fileUrl;
                    }
                }
                //if (file != null && file.ContentLength > 0)
                //{
                //    if (file.ContentType != "image/jpeg" & file.ContentType != "image/png" && file.ContentType != "image/gif")
                //    {
                //        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                //        isPost = false;
                //    }
                //    else
                //    {
                //        if (file.ContentLength > 4000 * 1024)
                //        {
                //            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                //            isPost = false;
                //        }
                //        else
                //        {
                //            var imgPath = "/images/articleCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                //            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                //            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                //            if (System.IO.File.Exists(Server.MapPath("/images/articleCategory/" + category.Image)))
                //            {
                //                System.IO.File.Delete(Server.MapPath("/images/articleCategory/" + category.Image));
                //            }
                //            category.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                //            var newImage = Image.FromStream(file.InputStream);
                //            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 600, 600, false);
                //            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                //        }
                //    }
                //}
                else
                {
                    category.Image = fc["CurrentFile"];
                }

                if (isPost)
                {
                    category.Url = HtmlHelpers.ConvertToUnSign(null, category.Url ?? category.CategoryName);
                    _unitOfWork.ArticleCategoryRepository.Update(category);
                    _unitOfWork.Save();
                    return RedirectToAction("ArticleCategory", new { result = "update" });
                }
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(a => a.ParentId == null, q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");
            return View(category);
        }
        [HttpPost]
        public bool DeleteCategory(int catId = 0)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.ArticleCategoryRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        public bool UpdateArticleCat(int sort = 1, bool active = false, bool menu = false, bool home = false, int articleCatId = 0)
        {
            var articleCat = _unitOfWork.ArticleCategoryRepository.GetById(articleCatId);
            if (articleCat == null)
            {
                return false;
            }
            articleCat.CategorySort = sort;
            articleCat.CategoryActive = active;
            articleCat.ShowMenu = menu;
            articleCat.ShowHome = home;

            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Article
        public ActionResult ListArticle(int? page, string name, int catId = 0, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var article = _unitOfWork.ArticleRepository.GetQuery(orderBy: l => l.OrderByDescending(a => a.Id));
            if (catId > 0)
            {
                article = article.Where(l => l.ArticleCategoryId == catId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                article = article.Where(l => l.Subject.ToLower().Contains(name.ToLower()));
            }
            var model = new ListArticleViewModel
            {
                SelectCategories = new SelectList(ArticleCategories, "Id", "CategoryName"),
                Articles = article.ToPagedList(pageNumber, pageSize),
                CategoryId = catId,
                Name = name
            };
            return View(model);
        }
        public ActionResult Article()
        {
            var model = new InsertArticleViewModel
            {
                Categories = ArticleCategories,
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Article(InsertArticleViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Article.Image"];

                dynamic json = Utils.UploadFile(file, "/images/article/");

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
                        model.Article.Image = fileUrl;
                    }
                }
                //if (file != null && file.ContentLength > 0)
                //{
                //    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
                //    {
                //        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                //        isPost = false;
                //    }
                //    else
                //    {

                //        if (file.ContentLength > 4000 * 1024)
                //        {
                //            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                //            isPost = false;
                //        }
                //        else
                //        {
                //            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);
                //            var imgPath = "/images/articles/" + DateTime.Now.ToString("yyyy/MM/dd");
                //            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                //            model.Article.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                //            //file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));

                //            var newImage = Image.FromStream(file.InputStream);
                //            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 800, 600, false);
                //            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                //        }
                //    }
                //}

                if (isPost)
                {
                    model.Article.Url = HtmlHelpers.ConvertToUnSign(null, model.Article.Url ?? model.Article.Subject);
                    model.Article.ArticleCategoryId = Convert.ToInt32(fc["CategoryId"]);
                    _unitOfWork.ArticleRepository.Insert(model.Article);
                    _unitOfWork.Save();
                    return RedirectToAction("ListArticle", new { result = "success" });
                }
            }
            model.Categories = ArticleCategories;
            return View(model);
        }
        public ActionResult UpdateArticle(int articleId = 0)
        {
            var article = _unitOfWork.ArticleRepository.GetById(articleId);
            if (article == null)
            {
                return RedirectToAction("ListArticle");
            }
            var model = new InsertArticleViewModel
            {
                Article = article,
                Categories = ArticleCategories,
                SelectCategories = new SelectList(ArticleCategories, "Id", "CategoryName"),
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateArticle(InsertArticleViewModel model, FormCollection fc)
        {
            var article = _unitOfWork.ArticleRepository.GetById(model.Article.Id);
            if (article == null)
            {
                return RedirectToAction("ListArticle");
            }
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Article.Image"];
                dynamic json = Utils.UploadFile(file, "/images/articles/");

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
                        article.Image = fileUrl;
                    }
                }
                //if (file != null && file.ContentLength > 0)
                //{
                //    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
                //    {
                //        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                //        isPost = false;
                //    }
                //    else
                //    {
                //        var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);
                //        if (file.ContentLength > 4000 * 1024)
                //        {
                //            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                //            isPost = false;
                //        }
                //        else
                //        {
                //            var imgPath = "/images/articles/" + DateTime.Now.ToString("yyyy/MM/dd");
                //            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                //            HtmlHelpers.DeleteFile(Server.MapPath("/images/articles/" + article.Image));
                //            article.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                //            var newImage = Image.FromStream(file.InputStream);
                //            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 800, 600, false);
                //            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                //        }
                //    }
                //}
                //else
                //{
                //    article.Image = fc["CurrentFile"] == "" ? null : fc["CurrentFile"];
                //}
                if (isPost)
                {
                    article.Url = HtmlHelpers.ConvertToUnSign(null, model.Article.Url ?? model.Article.Subject);
                    article.ArticleCategoryId = Convert.ToInt32(fc["CategoryId"]);
                    article.Subject = model.Article.Subject;
                    article.Description = model.Article.Description;
                    article.Body = model.Article.Body;
                    article.Active = model.Article.Active;
                    article.Col = model.Article.Col;
                    article.Home = model.Article.Home;
                    article.View = model.Article.View;
                    article.TitleMeta = model.Article.TitleMeta;
                    article.DescriptionMeta = model.Article.DescriptionMeta;

                    _unitOfWork.Save();
                    return RedirectToAction("ListArticle", new { result = "update" });
                }
            }
            model.Categories = ArticleCategories;
            return View(model);
        }
        [HttpPost]
        public bool DeleteArticle(int articleId = 0)
        {
            var article = _unitOfWork.ArticleRepository.GetById(articleId);
            if (article == null)
            {
                return false;
            }
            _unitOfWork.ArticleRepository.Delete(article);
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