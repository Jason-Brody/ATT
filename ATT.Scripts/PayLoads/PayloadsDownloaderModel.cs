using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts.DataFetch
{
    public class PayloadsDownloaderModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string CookieUrl { get; set; }

        public string DownloadUrl { get; set; }

        public List<string> MessageIds { get; set; }

        public string ZipFileName { get; set; }
    }
}
