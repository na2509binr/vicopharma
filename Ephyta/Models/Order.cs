using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Ephyta.Models
{
    public class Order
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string MaDonHang { get; set; }
        public DateTime CreateDate { get; set; }
        [Display(Name = "Tình trang thanh toán"), UIHint("Payment")]
        public bool Payment { get; set; }
        [Display(Name = "Thanh toán"), UIHint("DisplayTypePay")]
        public int TypePay { get; set; }
        [Display(Name = "Chuyển hàng"), UIHint("DisplayTypeTransport")]
        public int Transport { get; set; }
        [Display(Name = "Ngày giao hàng"), UIHint("DateTimePicker"), Required(ErrorMessage = "Hãy chọn ngày giao hàng")]
        public DateTime TransportDate { get; set; }
        [Display(Name = "Trạng thái đơn hàng"), UIHint("DisplayOrderStatus")]
        public int Status { get; set; }
        public bool Viewed { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        [Display(Name = "Thanh toán trước")]
        public int ThanhToanTruoc { get; set; }
        [Display(Name = "Phí Ship")]
        public int ShipFee { get; set; }

        [StringLength(50), Display(Name = "Mã giảm giá")]
        public string DiscountCode { get; set; }
        [Display(Name = "Số tiền giảm theo mã"),DisplayFormat(DataFormatString = "{0:N0}đ")]
        //public int DiscountAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        [Display(Name = "% giảm theo mã")]
        public int DiscountPercent { get; set; }
        //public float DiscountPercent { get; set; }

        public int? OrderMemberId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? TotalPropertyPrice { get; set; }

        public long Total()
        {
            return OrderDetails.Sum(a => (long?)(a.Price * a.Quantity)) ?? 0;
        }

        public long TotalDebt()
        {
            return TotalFee() - ThanhToanTruoc;
        }

        public long TotalFee()
        {
            var final = Total();
            if (DiscountAmount > 0)
            {
                final = (long)(Total() - DiscountAmount);
            }
            else if (DiscountPercent > 0)
            {
                final = (int)(final * ((100 - DiscountPercent) / 100));
            }
            return final + ShipFee;
        }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }

        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual Ward Ward { get; set; }

        //public virtual DiscountCode DiscountCode { get; set; }

        public Order()
        {
            CreateDate = DateTime.Now;
            TransportDate = DateTime.Now.AddDays(5);
            Payment = false;
            TypePay = 1;
            Viewed = false;
        }
    }
    [ComplexType]
    public class CustomerInfo
    {
        [Display(Name = "Họ và tên *"), Required(ErrorMessage = "Hãy nhập họ và tên"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Fullname { get; set; }
        [Display(Name = "Địa chỉ *"), Required(ErrorMessage = "Hãy nhập địa chỉ"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string Address { get; set; }
        [Display(Name = "Điện thoại *"), Required(ErrorMessage = "Hãy nhập điện thoại"), StringLength(11, MinimumLength = 7, ErrorMessage = "Điện thoại từ 7, 11 ký tự"), UIHint("TextBox")]
        public string Mobile { get; set; }
        [EmailAddress(ErrorMessage = "Email không hợp lệ"), Display(Name = "Email *"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Email { get; set; }
        [Display(Name = "Yêu cầu thêm"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), DataType(DataType.MultilineText)]
        public string Body { get; set; }


        [Display(Name = "Thành viên mới")]
        public bool IsNewMember { get; set; }
    }
}