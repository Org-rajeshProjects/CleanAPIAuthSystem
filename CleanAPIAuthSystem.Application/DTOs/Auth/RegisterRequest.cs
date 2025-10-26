using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.DTOs.Auth
{
    /// <summary>
    /// RegisterRequest DTO - Data Transfer Object for user registration
    /// Theory: DTOs decouple API contracts from domain entities
    /// Benefits:
    /// 1. API changes don't affect domain model
    /// 2. Can combine data from multiple entities
    /// 3. Apply validation rules specific to this use case
    /// 4. Control what data is exposed (security)
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Email validation using Data Annotations
        /// Theory: Declarative validation - rules defined with attributes
        /// Runs before business logic, fails fast for invalid input
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        // Theory: Regex ensures strong passwords (uppercase, lowercase, digit, special char)
        // Reduces risk of brute force attacks
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]",
            ErrorMessage = "Password must contain uppercase, lowercase, digit, and special character")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;
    }

}
