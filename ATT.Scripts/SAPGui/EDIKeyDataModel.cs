using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Scripts.SAPGui
{
    public class EDIKeyDataModel : SAPLoginModel
    {
        private DateTime _startDate;
        private DateTime _endDate;

        public string StartDate
        {
            get { return _startDate.ToString("dd.MM.yyyy"); }
            set { _startDate = setDate(value); }
        }

        public string EndDate
        {
            get { return _endDate.ToString("dd.MM.yyyy"); }
            set { _endDate = setDate(value); }
        }

        public DateTime Start { get { return _startDate; } }

        public DateTime End { get { return _endDate; } }


        public int StartTime { get; set; }

        public int Interval { get; set; }

        public int InterfaceCount { get; set; } = 1;

        public string IDocStatus { get; set; } = "53";
        private DateTime setDate(string date)
        {
            var dataList = date.Split('.');
            var dd = int.Parse(dataList[0]);
            var MM = int.Parse(dataList[1]);
            var yyyy = int.Parse(dataList[2]);
            return new DateTime(yyyy, MM, dd);
        }

        public List<SAPInterface> Interfaces { get; set; }

        public List<DocType> DocTypes { get; set; }

        public List<CompanyCode> CompanyCodes { get; set; }
    }

    public abstract class InterfaceBase
    {
        [ColMapping("Interface")]
        public string Interface { get; set; }

        [ColMapping("Sending Partner Number")]
        public string SendingPartnerNumber { get; set; }
    }

    public class SAPInterface:InterfaceBase
    {
        [ColMapping("MessageFunctions")]
        public string MessageFunction { get; set; }
    }

    public class DocType:InterfaceBase
    {
        [ColMapping("Document type")]
        public string Type { get; set; }

    }

    public class CompanyCode:InterfaceBase
    {
        [ColMapping("CompanyCode")]
        public string Code { get; set; }
    }
}
