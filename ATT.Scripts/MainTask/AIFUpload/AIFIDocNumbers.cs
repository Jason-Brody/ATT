using ATT.Scripts.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;
using ATT.Data.AIF;

namespace ATT.Scripts
{
    public class AIFIDocNumbers
    {
        public ATT.Data.AIF.IDocNumbers GetIDocNumber() {
            ATT.Data.AIF.IDocNumbers iDoc = new IDocNumbers();
            iDoc.IDocNumber = this.IDocNumber;
            iDoc.BasicType = this.BasicType;
            iDoc.Date = this.Date;
            iDoc.Description = this.Description;
            iDoc.Direction = this.Direction;
            iDoc.IDocId = this.IDocId;
            iDoc.MsgCode = this.MsgCode;
            iDoc.MsgFunction = this.MsgFunction;
            iDoc.MsgType = this.MsgType;
            iDoc.Partner = this.Partner;
            iDoc.Port = this.Port;
            iDoc.Reference = this.Reference;
            iDoc.Segments = this.Segments;
            iDoc.Status = this.Status;
            iDoc.Time = this.Time;
            return iDoc;
        }

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
