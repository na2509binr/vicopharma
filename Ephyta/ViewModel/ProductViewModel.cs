using Ephyta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ephyta.ViewModel
{
    public class ListProductViewModel
    {
        public PagedList.IPagedList<Product> Products { get; set; }
        public SelectList SelectCategories { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
    }
    public class InsertProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<ProductCategory> Categories { get; set; }
        public SelectList SelectCategories { get; set; }
        public int? CategoryId { get; set; }
        [Display(Name = "Giá gốc")]
        public string Price { get; set; }
        [Display(Name = "Giá giảm")]
        public string SaleOff { get; set; }
    }
}