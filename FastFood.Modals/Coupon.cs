using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Modals
{
    public class Coupon
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Type { get; set; }
        public double Discount { get ; set; }  
        public double MinimumAmount { get; set; }
        public byte[] CouponPicture { get; set; }
        public bool IsActive { get; set; }

    }
    public enum CouponType
    {
        [Display(Name = "Percent")]
        Percent=0,
        [Display(Name = "Currency")]
        Currency=1
    }
}
