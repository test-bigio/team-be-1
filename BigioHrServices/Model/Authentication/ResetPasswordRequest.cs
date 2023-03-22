using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigioHrServices.Model.Authentication
{
    public class ResetPasswordRequest
    {
        public string NIK { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}