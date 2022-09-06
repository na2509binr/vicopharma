using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ephyta.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        [Display(Name = "Tên danh mục"), Required(ErrorMessage = "Hãy nhập tên danh mục"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Ảnh đại diện"), StringLength(500), UIHint("ImageProductCat")]
        public string Image { get; set; }
        [Display(Name = "Đường dẫn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Hiện trang chủ")]
        public bool Home { get; set; }
        [Display(Name = "Danh mục cha")]
        public int? ParentId { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }

        [Display(Name = "Loại danh mục")]
        public TypeProduct TypeProduct { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public ProductCategory()
        {
            Active = true;
        }
    }
    public enum TypeProduct
    {
        [Display(Name = "Sản phẩm")]
        Product,
        [Display(Name = "Combo")]
        Combo,
       
    }
}