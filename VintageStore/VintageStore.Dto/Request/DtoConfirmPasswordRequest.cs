using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Request
{
    public class DtoConfirmPasswordRequest
    {
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Token { get; set; } = default!;

        [Required]
        public string NewPassword { get; set; } = default!;

        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; } = default!;
    }
}
