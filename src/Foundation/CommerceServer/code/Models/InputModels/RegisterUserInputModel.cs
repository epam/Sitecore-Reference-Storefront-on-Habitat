namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the RegisterUserInputModel class.
    /// </summary>
    public class RegisterUserInputModel
    {
        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a check to make sure the user password is correct
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}