using ATT.Scripts.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;
using ATT.Data.ATT;

namespace ATT.Scripts
{ 
    public class ATTMsg
    {
        [ColMapping("IDoc number")]
        public string IDocNumber { get; set; }

        [MsgIdConverter]
        [ColMapping("EDI Archive Key")]
        public string MsgId { get; set; }

        public MsgIDs GetATTMsg() {
            MsgIDs m = new MsgIDs() {
                IDocNumber = this.IDocNumber,
                MsgId = this.MsgId
            };
            return m;
        }
    }
}
