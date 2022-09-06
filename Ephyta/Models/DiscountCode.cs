using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ephyta.Models
{
    public class DiscountCode
    {
        public int Id { get; set; }
        [Display(Name = "Nhập tên mã giảm giá"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Fullname { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }

        [Display(Name = "Số tiềm giảm giá"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public decimal? Price { get; set; }

        [Display(Name = "% Giảm giá"), RegularExpression(@"^(\d?[0-9]|[0-9]0)$", ErrorMessage = "chỉ được nhập từ 0 - 99"), UIHint("NumberBox")]
        public int Discount { get; set; }

        [Display(Name = "Loại mã giảm giá")]

        public TypeDiscount TypeDiscount { get; set; }

        [Display(Name = "Ngày hết hạn"), UIHint("TextBox")]
        public DateTime? ExpDay { get; set; }

        //public virtual ICollection<Order> Orders { get; set; }
        public DiscountCode()
        {
            CreateDate = DateTime.Now;
            Active = true;
        }
    }
    public enum TypeDiscount
    {
        [Display(Name = "Mã chỉ dùng 1 lần")]
        OneTimeuse,
        [Display(Name = "Mã dùng nhiều lần")]
        UsedManyTimes,

    }
}

//2022 / 08 / 08 15:06
//    2022 / 10 / 06 16:44