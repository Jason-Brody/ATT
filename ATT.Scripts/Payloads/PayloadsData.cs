using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class PayloadsData
    {
        public PayloadsDownloaderData DownloadData { get; set; }

        public PayloadsUpdateData UpdateData { get; set; }

        public PayloadsUploaderData UploadData { get; set; }

        public PayloadsDownloaderData Copy(PayloadsDownloaderData data) {
            return new PayloadsDownloaderData() {
                DownloadUrl = data.DownloadUrl,
                Password = data.Password,
                UserName = data.UserName
            };
        }

        public PayloadsUpdateData Copy(PayloadsUpdateData data) {
            return new PayloadsUpdateData();
        }

        public PayloadsUploaderData Copy(PayloadsUploaderData data) {
            return new PayloadsUploaderData() {
                Host = data.Host,
                Port = data.Port,
                UserName = data.UserName,
                Password = data.Password,
                ProxyHost = data.ProxyHost,
                ProxyHostPort = data.ProxyHostPort
            };
        }
    }
}
