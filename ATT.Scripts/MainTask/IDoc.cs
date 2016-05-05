using ATT.Scripts.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Scripts
{
    public class IDoc
    {
        public ATT.Data.AIF.IDocs GetIDocs() {
            ATT.Data.AIF.IDocs doc = new Data.AIF.IDocs();
            doc.AccDocNumber = this.AccDocNumber;
            doc.Application = this.Application;
            doc.CompanyCode = this.CompanyCode;
            doc.CreateDT = this.CreateDT;
            doc.Description = this.Description;
            doc.DocDate = this.DocDate;
            doc.DT = this.DT;
            doc.IDocNumber = this.IDocNumber;
            doc.Msg = this.Msg;
            doc.MsgFunction = this.MsgFunction;
            doc.PostDate = this.PostDate;
            doc.RefDocNumber = this.RefDocNumber;
            doc.Status = this.Status;
            doc.Time = this.Time;
            doc.UserId = this.UserId;
            return doc;
        }

        [ColMapping("Idoc No.")]
        public string IDocNumber { get; set; }

        [MyDateConverter]
        [ColMapping("Cr Date")]
        public DateTime? CreateDT { get; set; }

        [MyTimeConverter]
        public TimeSpan? Time { get; set; }

        [ColMapping("CS")]
        public string Status { get; set; }

        [ColMapping("Appl Area")]
        public string Application { get; set; }

        [ColMapping("MSG")]
        public string Msg { get; set; }

        [ColMapping("Msg Fn")]
        public string MsgFunction { get; set; }

        [ColMapping("CoCd")]
        public string CompanyCode { get; set; }

        [ColMapping("Message Description")]
        public string Description { get; set; }

        public string DT { get; set; }

        [ColMapping("Ref Doc No")]
        public string RefDocNumber { get; set; }

        [ColMapping("Acc Doc No")]
        public string AccDocNumber { get; set; }

        [MyDateConverter]
        public DateTime? PostDate { get; set; }

        [MyDateConverter]
        public DateTime? DocDate { get; set; }

        [ColMapping("User ID")]
        public string UserId { get; set; }
    }
}
