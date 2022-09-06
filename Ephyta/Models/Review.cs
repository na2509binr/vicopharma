using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ephyta.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Display(Name = "Lời nhận xét"), StringLength(700, ErrorMessage = "Tối đa 700 ký tự"), UIHint("TextArea")]
        public string Content { get; set; }
        [Display(Name = "Ảnh đại diện"), UIHint("ImageReview")]
        public string ImageAvata { get; set; }

        [Display(Name = "Danh sách ảnh"), UIHint("UploadMultiFile")]
        public string ListImage { get; set; }

        [Display(Name = "Họ tên", Description = "Tên dài tối đa 100 ký tự"),
         Required(ErrorMessage = "Hãy nhập Họ tên"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"),
         UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Email"), Required(ErrorMessage = "Hãy nhập địa chỉ email"), EmailAddress(ErrorMessage = "Email không hợp lệ"), UIHint("TextBox")]
        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Ngày đăng")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        [Display(Name = "Đánh giá sao")]
        public StarReview StarReview { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Review()
        {
            CreateDate = DateTime.Now;
            Active = false;
        }

    }
    public enum StarReview
    {
        
        [Display(Name = "1 sao")]
        One = 1,
        [Display(Name = "2 sao")]
        Two,
        [Display(Name = "3 sao")]
        Three,
        [Display(Name = "4 sao")]
        Four,
        [Display(Name = "5 sao")]
        Five,
    }
}