namespace ATT.Data.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    //public partial class SenderConfig
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }

    //    [Key]
    //    [Column(Order = 0)]
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int SourceId { get; set; }

    //    [StringLength(50)]
    //    public string system { get; set; }

    //    [StringLength(50)]
    //    public string prosendercomponent { get; set; }

    //    [StringLength(50)]
    //    public string itgsendercomponent { get; set; }

    //    [StringLength(255)]
    //    public string pfolder { get; set; }

    //    [Key]
    //    [Column(Order = 1)]
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int IDocTypeId { get; set; }

    //    [StringLength(100)]
    //    public string senderinterface { get; set; }

    //    [StringLength(255)]
    //    public string sendernamespace { get; set; }

    //    [StringLength(255)]
    //    public string requiretransform { get; set; }

    //    public bool? IsNeedTransform { get; set; }

    //    public virtual IDocType IDocType { get; set; }

    //    public virtual Source Source { get; set; }
    //}
}
