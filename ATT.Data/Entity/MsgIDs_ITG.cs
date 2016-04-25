namespace ATT.Data.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Young.Data.Attributes;
    public partial class MsgIDs_ITG
    {
        
        public int Id { get; set; }

        [ColMapping("Message ID")]
        [StringLength(50)]
        public string MsgId { get; set; }

        [ColMapping("Message Status")]
        public int MsgStatus { get; set; }

        [ColMapping("Outbound Status")]
        public int OutboundStatus { get; set; }

        [StringLength(50)]
        public string Sender { get; set; }

        [ColMapping("Sr If Name")]
        [StringLength(50)]
        public string SrIfName { get; set; }

        [TimeStampConverter]
        [ColMapping("Time Stamp")]
        public DateTime? Dt { get; set; }

        [TimeStampConverter]
        [ColMapping("Time Stamp1")]
        public DateTime? Dt1 { get; set; }

        [TimeStampConverter]
        [ColMapping("Time Stamp11")]
        public DateTime? Dt2 { get; set; }
    }
}
