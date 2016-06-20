namespace AIF.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LH7IDocs
    {
    
        public int Id { get; set; }

        [StringLength(50)]
        public string IDocNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreateDT { get; set; }

        public TimeSpan? Time { get; set; }

        [StringLength(4)]
        public string Status { get; set; }

        [StringLength(100)]
        public string Application { get; set; }

        [StringLength(10)]
        public string Msg { get; set; }

        [StringLength(10)]
        public string MsgFunction { get; set; }

        [StringLength(10)]
        public string CompanyCode { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(10)]
        public string DT { get; set; }

        [StringLength(50)]
        public string RefDocNumber { get; set; }

        [StringLength(50)]
        public string AccDocNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PostDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DocDate { get; set; }

        [StringLength(15)]
        public string UserId { get; set; }

        public int? Tid { get; set; }

        public virtual Tasks Tasks { get; set; }
    }
}
