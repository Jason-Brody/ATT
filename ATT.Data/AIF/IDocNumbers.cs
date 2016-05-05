namespace ATT.Data.AIF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IDocNumbers
    {
        public int Id { get; set; }

        [StringLength(30)]
        public string IDocNumber { get; set; }

        [StringLength(20)]
        public string Segments { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(20)]
        public string Partner { get; set; }

        [StringLength(50)]
        public string BasicType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public TimeSpan? Time { get; set; }

        [StringLength(50)]
        public string MsgType { get; set; }

        [StringLength(20)]
        public string MsgCode { get; set; }

        [StringLength(20)]
        public string MsgFunction { get; set; }

        [StringLength(20)]
        public string Direction { get; set; }

        [StringLength(20)]
        public string Port { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string IDocId { get; set; }

        [StringLength(255)]
        public string Reference { get; set; }

        public int? Tid { get; set; }

        public virtual Tasks Tasks { get; set; }
    }
}
