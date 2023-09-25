using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Domain.Configuration
{
    public class AppConfig
    {
        public Storageconfiguration StorageConfiguration { get; set; } = default!;

        public Jwt Jwt { get; set; } = default!;

        public SmtpConfiguration SmtpConfiguration { get; set; } = default!;
    }
    public class SmtpConfiguration
    {
        public string UserName { get; set; } = default!;
        public string Server { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int PortNumber { get; set; }
        public string FromName { get; set; } = default!;
        public bool EnableSsl { get; set; }
    }
    public class Jwt
    {
        public string SecretKey { get; set; } = default!;
        public string Audiencia { get; set; } = default!;
        public string Emisor { get; set; } = default!;
    }
    public class Storageconfiguration
    {
        public string PublicUrl { get; set; } = default!;
        public string Path { get; set; } = default!;
    }
}
