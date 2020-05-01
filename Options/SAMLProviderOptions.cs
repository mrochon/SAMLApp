using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMLTEST.Options
{
    public class SAMLProviderOptions
    {
        public string Tenant { get; set; }
        public string Policy { get; set; }

        public string Issuer { get; set; }
        public string DCInfo { get; set; }
    }
}
