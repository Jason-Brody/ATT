using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIF.Scripts
{
    public class AIF56Data:AIFBaseData
    {
        public string GetAIF56File(int interfaceId) {
            return $"C:\\AIF\\LH7IDocs_56_{interfaceId}.txt";
        }
    }
}
