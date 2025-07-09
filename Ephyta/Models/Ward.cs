
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ephyta.Models
{
    public class Ward
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Hãy nhập tên thành phố"), Display(Name = "Tên thành phố"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string Name { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên")]
        public int WardSort { get; set; }
        [Display(Name = "Hoạt động")]
        public bool WardActive { get; set; }
        [StringLength(20)]
        public string Prefix { get; set; }
        public int ShipFee { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}