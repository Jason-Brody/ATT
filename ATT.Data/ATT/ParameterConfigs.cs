namespace ATT.Data.ATT
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParameterConfigs
    {
        public int Id { get; set; }

        public int? NodeId { get; set; }

        [StringLength(50)]
        public string FromVal { get; set; }

        [StringLength(50)]
        public string ToVal { get; set; }

        public int? ProAwsysId { get; set; }

        public virtual ProAwsys ProAwsys { get; set; }

        public virtual XNodes XNodes { get; set; }
    }
}
