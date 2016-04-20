using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts.PayLoads
{
    public class PayloadsDownloaderModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string DownloadUrl { get; set; }

        public int DownloadPatchSize { get; set; } = 100;

    }
}
