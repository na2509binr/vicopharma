using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ephyta.Models
{
    public class Register
    {
        public int Id { get; set; }
        [Display(Name = "Họ và tên"), Required(ErrorMessage = "Hãy nhập họ và tên"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Fullname { get; set; }
      
        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập số điện thoại"), Phone(ErrorMessage = "Số điện thoại không hợp lệ"), UIHint("TextBox")]
        public string Mobile { get; set; }

        [Display(Name = "Khung giờ tư vấn"), UIHint("TextBox")]
        public string Time { get; set; }

        [Display(Name = "Sản phẩm"), Required(ErrorMessage = "Nhập tên sản phẩm"), UIHint("TextBox")]
        public string Product { get; set; }

        [Display(Name = "Lưu ý"), UIHint("TextBox")]
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }

        public Register()
        {
            CreateDate = DateTime.Now;
        }
    }
}