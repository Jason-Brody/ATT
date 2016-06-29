using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class PayloadsUploaderData:PayloadsShareData
    {
        public string Host { get; set; } = "pi-itg-01-idoc.sapnet.hpecorp.net";

        public int Port { get; set; } = 63100;

        public string ProxyHost { get; set; } = "web-proxy.austin.hp.com";

        public int ProxyHostPort { get; set; } = 8080;

        public string UserName { get; set; }

        public string Password { get; set; }

        public string UploadLog { get; } = "Upload Payload";

        public string UpdateMsgLog { get; } ="Update Download Status to DB";

        public PayloadsUploaderData Copy() {
            return new PayloadsUploaderData() {
                Host = Host,
                Port = Port,
                UserName = UserName,
                Password = Password,
                ProxyHost =ProxyHost,
                ProxyHostPort = ProxyHostPort
            };
        }
    }
}
