
using ATT.Data;
using SharedLib.Converters;
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
        public AIF.Data.LH7IDocs GetIDocs() {
            var doc = new AIF.Data.LH7IDocs();
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

        public IDocNumbers GetATTIDoc() {
            IDocNumbers doc = new IDocNumbers();
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

        [ColMapping("Idoc No")]
        [ColMapping("Idoc No.")]
        public string IDocNumber { get; set; }

        [MyDateConverter]
        [ColMapping("Cr Date")]
        public DateTime? CreateDT { get; set; }

        [ColMapping("Time")]
        [ColMapping("Cr Time")]
        [MyTimeConverter]
        public TimeSpan? Time { get; set; }

        [ColMapping("CS")]
        [ColMapping("Status")]
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
        [ColMapping("Msg Description")]
        public string Description { get; set; }

        [ColMapping("DT")]
        [ColMapping("Doc Type")]
        public string DT { get; set; }

        [ColMapping("Ref Doc No")]
        public string RefDocNumber { get; set; }

        [ColMapping("Acc Doc No")]
        [ColMapping("Acct Doc No")]
        public string AccDocNumber { get; set; }

        [ColMapping("PostDate")]
        [ColMapping("Post Date")]
        [MyDateConverter]
        public DateTime? PostDate { get; set; }

        [ColMapping("DocDate")]
        [ColMapping("Doc Date")]
        [MyDateConverter]
        public DateTime? DocDate { get; set; }

        [ColMapping("User ID")]
        public string UserId { get; set; }
    }
}
