using ATT.Scripts.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Scripts
{
    public class AIFIDocNumbers
    {
        [ColMapping("IDoc Number")]
        public string IDocNumber { get; set; }

        public string Segments { get; set; }
        
        public string Status { get; set; }

        public string Partner { get; set; }

        [ColMapping("Basic type")]
        public string BasicType { get; set; }

        [DateConverter]
        public DateTime Date { get; set; }

        [TimeConverter]
        public TimeSpan Time { get; set; }

        [ColMapping("Message Type")]
        public string MsgType { get; set; }

        [ColMapping("Msg. Var.")]
        public string MsgCode { get; set; }

        [ColMapping("Msg.funct.")]
        public string MsgFunction { get; set; }

        public string Direction { get; set; }

        public string Port { get; set; }

        public string Description { get; set; }

        [ColMapping("Id")]
        public string IDocId { get; set; }

        public string Reference { get; set; }
    }
}
