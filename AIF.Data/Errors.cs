namespace AIF.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Young.Data.Attributes;
    public partial class Errors
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string IDocNumber { get; set; }

        [StringLength(255)]
        public string Functions { get; set; }

        [StringLength(255)]
        public string Hints { get; set; }

        public int Index { get; set; }

        [ColMapping("Message text")]
        [StringLength(255)]
        public string MsgText { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public TimeSpan? Time { get; set; }

        public int? Eid { get; set; }

        public int? Mid { get; set; }
    }
}
