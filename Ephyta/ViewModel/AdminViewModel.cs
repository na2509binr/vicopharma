using Ephyta.Models;
using System.ComponentModel.DataAnnotations;

namespace Ephyta.ViewModel
{
    public class ChangePasswordModel
    {
        [Display(Name = "Mật khẩu hiện tại"), Required(ErrorMessage = "Hãy nhập mật khẩu hiện tại"), UIHint("Password")]
        public string OldPassword { get; set; }
        [Display(Name = "Mật khẩu mới"), Required(ErrorMessage = "Hãy nhập mật khẩu mới"),
         StringLength(16, MinimumLength = 4, ErrorMessage = "Mật khẩu từ 4, 16 ký tự"), UIHint("Password")]
        public string Password { get; set; }
        [Display(Name = "Nhập lại mật khẩu"), Compare("Password", ErrorMessage = "Nhập lại mật khẩu không chính xác"),
         UIHint("Password")]
        public string ConfirmPassword { get; set; }
    }
    public class AdminLoginModel
    {
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Hãy nhập mật khẩu")]
        public string Password { get; set; }
    }
    public class UpdateAdminModel
    {
        public int Id { get; set; }
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public RoleAdmin RoleAdmin { get; set; }
        public bool Active { get; set; }
    }
    public class InfoAdminViewModel
    {
        public int Articles { get; set; }
        public int Banners { get; set; }
        public int Contacts { get; set; }
        public int Admins { get; set; }
        public int Products { get; set; }
        public int Feedbacks { get; set; }
    }

    public class CodeDiscountViewModel
    {
        public DiscountCode DiscountCode { get; set; }

        [Display(Name = "Số tiền giảm")]
        public string Price { get; set; }
    }

    public class ListCodeDiscountViewModel
    {
        public PagedList.IPagedList<DiscountCode> DiscountCodes { get; set; }
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string Name { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        [Required]
        public int PageSize { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public int HSD { get; set; }

    }
}