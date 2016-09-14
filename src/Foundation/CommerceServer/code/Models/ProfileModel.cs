namespace Sitecore.Foundation.CommerceServer.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Used to represent a user profile
    /// </summary>
    public class ProfileModel
    {
        private readonly List<string> _errors = new List<string>();

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<string> Errors
        {
            get
            {
                return this._errors;
            }
        }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the external id
        /// </summary>
        [Display(Name = "External Id")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the reentered email.
        /// </summary>
        /// <value>
        /// The reentered email.
        /// </value>
        [Compare("Email", ErrorMessage = "The email and repeat email do not match.")]
        public string EmailRepeat { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the telephone
        /// </summary>
        [Required]
        [Display(Name = "Telephone")]
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the reentered password.
        /// </summary>
        /// <value>
        /// The reentered password.
        /// </value>
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordRepeat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether direct mail is set or not
        /// </summary>
        [Display(Name = "Direct mail")]
        public bool DirectMailOptOut { get; set; }
    }
}
