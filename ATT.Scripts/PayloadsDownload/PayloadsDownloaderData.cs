using ScriptRunner.Interface.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class PayloadsDownloaderData:PayloadsShareData
    {
        public string DownloadUrl { get; set; } = "http://sappip-ent.sapnet.hpecorp.net:50000/hp.com~com.hp.pi.core.web/svc/event/downloadPayloads";

        public string UserName { get; set; }

        public string Password { get; set; }

        public string GetTaskIdLog { get; } = "Get Download Task";

        public string DownloadFileLog { get; } = "Download File";

        public PayloadsDownloaderData Copy() {
            return new PayloadsDownloaderData() {
                DownloadUrl = DownloadUrl,
                Password = Password,
                UserName = UserName
            };
        }
    }
}
