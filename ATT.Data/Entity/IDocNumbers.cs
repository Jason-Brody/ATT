namespace ATT.Data.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Young.Data.Attributes;

    
    public partial class IDocNumbers
    {
        public int Id { get; set; }

        [ColMapping("Idoc No.")]
        [StringLength(50)]
        public string IDocNumber { get; set; }

        [MyDateConverter]
        [ColMapping("Cr Date")]
        [Column(TypeName = "date")]
        public DateTime? CreateDT { get; set; }

        [MyTimeConverter]
        public TimeSpan? Time { get; set; }

        [ColMapping("CS")]
        [StringLength(4)]
        public string Status { get; set; }

        [ColMapping("Appl Area")]
        [StringLength(100)]
        public string Application { get; set; }

        [ColMapping("MSG")]
        [StringLength(10)]
        public string Msg { get; set; }

        [ColMapping("Msg Fn")]
        [StringLength(10)]
        public string MsgFunction { get; set; }

        [ColMapping("CoCd")]
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [ColMapping("Message Description")]
        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(10)]
        public string DT { get; set; }

        [ColMapping("Ref Doc No")]
        [StringLength(50)]
        public string RefDocNumber { get; set; }

        [ColMapping("Acc Doc No")]
        [StringLength(50)]
        public string AccDocNumber { get; set; }

        [MyDateConverter]
        [Column(TypeName = "date")]
        public DateTime? PostDate { get; set; }

        [MyDateConverter]
        [Column(TypeName = "date")]
        public DateTime? DocDate { get; set; }

        [ColMapping("User ID")]
        [StringLength(50)]
        public string UserId { get; set; }
    }
}
