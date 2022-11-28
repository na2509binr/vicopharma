using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ephyta.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Display(Name = "Tên sản phẩm"), Required(ErrorMessage = "Hãy nhập tên sản phẩm"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Danh sách ảnh"), UIHint("UploadMultiFile")]
        public string ListImage { get; set; }
        [Display(Name = "Trích dẫn ngắn"), Required(ErrorMessage = "Hãy nhập trích dẫn ngắn"),
         UIHint("EditorBox")]
        public string Description { get; set; }
        [Display(Name = "Công dụng"), UIHint("EditorBox")]
        public string Function { get; set; }
        [Display(Name = "Hướng dẫn sử dụng"), UIHint("EditorBox")]
        public string Usermanual { get; set; }
        [Display(Name = "Giới thiệu chung"), UIHint("EditorBox")]
        public string Intro { get; set; }
        [Display(Name = "Cách dùng"), StringLength(2000, ErrorMessage = "Tối đa 2000 ký tự"), UIHint("TextArea")]
        public string Use { get; set; }
        [Display(Name = "Quy cách"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Specifications { get; set; }
        [Display(Name = "Thành phần"), UIHint("EditorBox")]
        public string Ingredient { get; set; }
        [Display(Name = "Danh mục sản phẩm"), Required(ErrorMessage = "Hãy chọn danh mục sản phẩm")]
        public int ProductCategoryId { get; set; }
        [Display(Name = "Số lượng"), RegularExpression(@"\d+", ErrorMessage = "Nhập số nguyên dương"), UIHint("NumberBox")]
        public int Quantity { get; set; }
        [Display(Name = "Giá gốc"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public decimal? Price { get; set; }
        [Display(Name = "Giảm giá"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public decimal? SaleOff { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Hiện trang chủ")]
        public bool Home { get; set; }
        [Display(Name = "Nổi bật bán chạy")]
        public bool Hot { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }
        [StringLength(500)]
        public string Url { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Ngày đăng")]
        public DateTime CreateDate { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<ReviewKol> ReviewKol { get; set; }
        public Product()
        {
            CreateDate = DateTime.Now;
            Active = true;
        }
        public string PriceOfProduct()
        {
            var p = "Liên hệ";
            if (SaleOff != null)
            {
                p = Convert.ToDecimal(SaleOff).ToString("N0") + "VND";
            }
            else if (Price != null)
            {
                p = Convert.ToDecimal(Price).ToString("N0") + "VND";
            }
            return p;
        }
    }
}