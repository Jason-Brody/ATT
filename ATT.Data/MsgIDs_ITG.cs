namespace ATT.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MsgIDs_ITG
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MsgId { get; set; }

        public int? MsgStatus { get; set; }

        public int? OutboundStatus { get; set; }

        [StringLength(50)]
        public string Sender { get; set; }

        [StringLength(50)]
        public string SrIfName { get; set; }

        public DateTime? Dt { get; set; }

        public DateTime? Dt1 { get; set; }

        public DateTime? Dt2 { get; set; }
    }
}
