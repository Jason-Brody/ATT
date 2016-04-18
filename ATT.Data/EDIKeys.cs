namespace ATT.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Young.Data.Attributes;
    public partial class EDIKeys
    {
        public int Id { get; set; }

        [ColMapping("IDoc number")]
        [StringLength(50)]
        public string IDocNumber { get; set; }

        [EDIKeyConverter]
        [ColMapping("EDI Archive Key")]
        [StringLength(50)]
        public string EDIKey { get; set; }

        public int? StatusId { get; set; }
    }
}
