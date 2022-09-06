using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ephyta.Models
{
    public class ReviewKol
    {
        public int Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Hãy nhập tên "), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Hình ảnh"), StringLength(500)]
        public string Image { get; set; }
        [Display(Name = "Đường dẫn Youtube"), Required(ErrorMessage = "Hãy nhập đường dẫn Youtube"), UIHint("TextBox")]
        public string VideoLink { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Hiển thị")]
        public bool Active { get; set; }
       
        [Display(Name = "Số thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên."), UIHint("NumberBox")]
        public int Sort { get; set; }
        public DateTime CreateDate { get; set; }
        [Display(Name = "Sản phẩm"), Required(ErrorMessage = "Hãy chọn sản phẩm")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public ReviewKol()
        {
            CreateDate = DateTime.Now;
        }
    }
}