using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigioHrServices.Model.Authentication
{
    public class UpdatePinSignatureRequest
    {
        public string NIK { get; set; } = string.Empty;
        public string oldPinSignature { get; set; } = string.Empty;
        public string newPinSignature { get; set; } = string.Empty;
    }
}