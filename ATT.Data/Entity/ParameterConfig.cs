namespace ATT.Data.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParameterConfig
    {
        public int Id { get; set; }

        public int? NodeId { get; set; }

        [StringLength(50)]
        public string FromVal { get; set; }

        [StringLength(50)]
        public string ToVal { get; set; }

        public int? ProAwsysId { get; set; }

        public virtual ProAwsy ProAwsy { get; set; }

        public virtual XNode XNode { get; set; }
    }
}
