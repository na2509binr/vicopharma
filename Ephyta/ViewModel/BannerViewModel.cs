using Ephyta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ephyta.ViewModel
{
    public class BannerViewModel
    {
        public Banner Banner { get; set; }
        public SelectList SelectGroup { get; set; }
        public BannerViewModel()
        {
            var listgroup = new Dictionary<int, string> { { 1, "Banner Slide (1440 x 540)" }, { 2, "Ảnh Sản phẩm bán chạy" }, { 3, "Banner đối tác (105 x 110)" }, { 4, "Icon dịch vụ dưới banner" }, { 5, "Ảnh khuyến mại và combo" }, { 6, "Ảnh danh mục sản phẩm" } };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }
    public class ListBannerViewModel
    {
        //public IEnumerable<Banner> Banners { get; set; }
        public PagedList.IPagedList<Banner> Banners { get; set; }
        public int? GroupId { get; set; }
        public SelectList SelectGroup { get; set; }
        public ListBannerViewModel()
        {
            var listgroup = new Dictionary<int, string> { { 1, "Banner Slide (1440 x 540)" }, { 2, "Ảnh Sản phẩm bán chạy" }, { 3, "Banner đối tác (105 x 110)" }, { 4, "Icon dịch vụ dưới banner" }, { 5, "Ảnh khuyến mại và combo" }, { 6, "Ảnh danh mục sản phẩm" } };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }
}