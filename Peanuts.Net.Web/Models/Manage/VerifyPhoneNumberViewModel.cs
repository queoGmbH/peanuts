using System.ComponentModel.DataAnnotations;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Manage {
    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }
    }
}