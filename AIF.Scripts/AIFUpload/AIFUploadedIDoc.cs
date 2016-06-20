using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace AIF.Scripts
{
    public class AIFUploadedIDoc
    {
        [ColMapping("IDoc number")]
        public string IDocNumber { get; set; }

        [ColMapping("Basic type")]
        public string BasicType { get; set; }

        [ColMapping("Message type")]
        public string MsgType { get; set; }

        [ColMapping("Msg.code")]
        public string MsgCode { get; set; }

        [ColMapping("Msg.funct.")]
        public string MsgFunc { get; set; }


        public string Port { get; set; }

        public string Receiver { get; set; }

        [ColMapping("Partn.Type")]
        public string PartnerType { get; set; }

        public string Sender { get; set; }

        public string Identification { get; set; }
    }
}
