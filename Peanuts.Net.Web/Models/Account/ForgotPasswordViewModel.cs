using System.ComponentModel.DataAnnotations;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Account {
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
    }
}