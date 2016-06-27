using ATT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Client.ViewModels
{
    class InterfaceVM
    {
        public InterfaceVM(SAPInterfaces sapInterface) {
            Source = sapInterface;
            this.CompanyCodes = getString(sapInterface.SAPCompanyCodes.Select(s => s.Name).ToList());
            this.DocTypes = getString(sapInterface.SAPDocTypes.Select(s => s.Name).ToList());
        }

        private string getString(List<string> stringList) {
            var returnString = string.Empty;
            stringList.ForEach(s => returnString += s + ";");
            returnString = returnString.Substring(0, returnString.Length - 1);
            return returnString;
        }

        public SAPInterfaces Source { get; set; }

        public string CompanyCodes { get; set; }

        public string DocTypes { get; set; }
    }
}
