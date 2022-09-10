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
    public class ProductController : Controller
    {
        // GET: Product
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IEnumerable<ProductCategory> ProductCategories => _unitOfWork.ProductCategoryRepository.Get();

        #region ProductCategory
        [ChildActionOnly]
        public ActionResult ListCategory()
        {
            var allcats = _unitOfWork.ProductCategoryRepository.Get(orderBy: q => q.OrderBy(a => a.Sort));
            return PartialView(allcats);
        }
        public ActionResult ProductCategory(string result = "")
        {
            ViewBag.ArticleCat = result;
            ViewBag.RootCats =
                new SelectList(
                    _unitOfWork.ProductCategoryRepository.Get(a => a.ParentId == null,
                                                              q => q.OrderBy(a => a.Sort)), "Id", "Name");
            var productCat = new ProductCategory();
            return View(productCat);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ProductCategory(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Image"];

                var json = HtmlHelpers.UploadFile(file, "/images/productCategory/");
                if(json != null)
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
                //            var imgPath = "/images/productCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
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
                    category.Url = HtmlHelpers.ConvertToUnSign(null, category.Url ?? category.Name);
                    _unitOfWork.ProductCategoryRepository.Insert(category);
                    _unitOfWork.Save();
                    return RedirectToAction("ProductCategory", new { result = "success" });
                }
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ProductCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.Sort)), "Id", "Name");
            return View(category);
        }
        public ActionResult UpdateCategory(int catId = 0)
        {
            var category = _unitOfWork.ProductCategoryRepository.GetById(catId);
            if (category == null)
            {
                return RedirectToAction("ProductCategory");
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ProductCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.Sort)), "Id", "Name");
            return View(category);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCategory(ProductCategory category, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Image"];
                var json = HtmlHelpers.UploadFile(file, "/images/productCategory/");
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
                //            var imgPath = "/images/productCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                //            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                //            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                //            if (System.IO.File.Exists(Server.MapPath("/images/productCategory/" + category.Image)))
                //            {
                //                System.IO.File.Delete(Server.MapPath("/images/productCategory/" + category.Image));
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
                    category.Url = HtmlHelpers.ConvertToUnSign(null, category.Url ?? category.Name);
                    _unitOfWork.ProductCategoryRepository.Update(category);
                    _unitOfWork.Save();
                    return RedirectToAction("ProductCategory", new { result = "update" });
                }
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ProductCategoryRepository.Get(a => a.ParentId == null, q => q.OrderBy(a => a.Sort)), "Id", "Name");
            return View(category);
        }
        [HttpPost]
        public bool DeleteCategory(int catId = 0)
        {
            var category = _unitOfWork.ProductCategoryRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.ProductCategoryRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        public bool UpdateProductCat(int sort = 1, bool active = false, bool home = false, int productCatId = 0)
        {
            var productCat = _unitOfWork.ProductCategoryRepository.GetById(productCatId);
            if (productCat == null)
            {
                return false;
            }
            productCat.Sort = sort;
            productCat.Active = active;
            productCat.Home = home;

            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Product
        public ActionResult ListProduct(int? page, string name, int catId = 0, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var product = _unitOfWork.ProductRepository.Get(orderBy: l => l.OrderByDescending(a => a.Id));
            if (catId > 0)
            {
                product = product.Where(l => l.ProductCategoryId == catId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                product = product.Where(l => l.Name.ToLower().Contains(name.ToLower()));
            }
            var model = new ListProductViewModel
            {
                SelectCategories = new SelectList(ProductCategories, "Id", "Name"),
                Products = product.ToPagedList(pageNumber, pageSize),
                CategoryId = catId,
                Name = name
            };
            return View(model);
        }
        public ActionResult Product()
        {
            var model = new InsertProductViewModel
            {
                Categories = ProductCategories,
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Product(InsertProductViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                model.Product.ListImage = fc["Pictures"];
                if (model.Price != null)
                {
                    model.Product.Price = Convert.ToDecimal(model.Price.Replace(",", ""));
                }
                if (model.SaleOff != null)
                {
                    model.Product.SaleOff = Convert.ToDecimal(model.SaleOff.Replace(".", ""));
                }
                if (model.Product.SaleOff >= model.Product.Price)
                {
                    ModelState.AddModelError("", @"Giá khuyến mãi lơn hơn giá bán. Bạn hãy nhập lại");
                    isPost = false;
                }
                if (isPost)
                {
                    model.Product.Url = HtmlHelpers.ConvertToUnSign(null, model.Product.Url ?? model.Product.Name);
                   
                    model.Product.ProductCategoryId = Convert.ToInt32(fc["CategoryId"]);
                    _unitOfWork.ProductRepository.Insert(model.Product);
                    _unitOfWork.Save();
                    var count = _unitOfWork.ProductRepository.GetQuery(a => a.Url == model.Product.Url).Count();
                    if (count > 1)
                    {
                        model.Product.Url += "-" + model.Product.Id;
                        _unitOfWork.Save();
                    }
                    return RedirectToAction("ListProduct", new { result = "success" });
                }
            }
            model.Categories = ProductCategories;
            return View(model);
        }
        public ActionResult UpdateProduct(int productId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(productId);
            if (product == null)
            {
                return RedirectToAction("ListProduct");
            }
            var model = new InsertProductViewModel
            {
                Product = product,
                Categories = ProductCategories,
                Price = product.Price?.ToString("N0"),
                SaleOff = product.SaleOff?.ToString("N0"),
                SelectCategories = new SelectList(ProductCategories, "Id", "Name"),
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProduct(InsertProductViewModel model, FormCollection fc)
        {
            var product = _unitOfWork.ProductRepository.GetById(model.Product.Id);
            if (product == null)
            {
                return RedirectToAction("ListProduct");
            }
            if (ModelState.IsValid)
            {
                var isPost = true;
                product.ListImage = fc["Pictures"] == "" ? null : fc["Pictures"];
                if (model.Price != null)
                {
                    product.Price = Convert.ToDecimal(model.Price.Replace(",", ""));
                }
                else
                {
                    product.Price = null;
                }
                if (model.SaleOff != null)
                {
                    product.SaleOff = Convert.ToDecimal(model.SaleOff.Replace(",", ""));
                }
                else
                {
                    product.SaleOff = null;
                }
                if (product.SaleOff >= product.Price || product.Price == null && product.SaleOff != null)
                {
                    ModelState.AddModelError("", @"Giá khuyến mãi lơn hơn giá bán. Bạn hãy nhập lại");
                    isPost = false;
                }
                if (isPost)
                {
                    product.ProductCategoryId = Convert.ToInt32(fc["CategoryId"]);
                    product.Name = model.Product.Name;
                    product.Description = model.Product.Description;
                    product.Active = model.Product.Active;
                    product.Home = model.Product.Home;
                    product.Function = model.Product.Function;
                    product.Usermanual = model.Product.Usermanual;
                    product.Use = model.Product.Use;
                    product.Intro = model.Product.Intro;
                    product.Specifications = model.Product.Specifications;
                    product.Ingredient = model.Product.Ingredient;
                    product.Sort = model.Product.Sort;
                    product.TitleMeta = model.Product.TitleMeta;
                    product.Hot = model.Product.Hot;
                    product.DescriptionMeta = model.Product.DescriptionMeta;
                    product.Url = HtmlHelpers.ConvertToUnSign(null, model.Product.Url ?? model.Product.Name);

                    _unitOfWork.Save();
                    var count = _unitOfWork.ProductRepository.GetQuery(a => a.Url == model.Product.Url).Count();
                    if (count > 1)
                    {
                        product.Url += "-" + product.Id;
                        _unitOfWork.Save();
                    }
                    return RedirectToAction("ListProduct", new { result = "update" });
                }
            }
            model.Categories = ProductCategories;
            return View(model);
        }
        [HttpPost]
        public bool CloneProduct(int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return false;
            }
            _unitOfWork.ProductRepository.Insert(product);
            _unitOfWork.Save();
            return true;
        }
        [HttpPost]
        public bool DeleteProduct(int productId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(productId);
            if (product == null)
            {
                return false;
            }
            _unitOfWork.ProductRepository.Delete(product);
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