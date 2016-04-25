namespace ATT.Data.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SAPDocTypes
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        public int? InterfaceId { get; set; }

        public virtual SAPInterfaces SAPInterfaces { get; set; }
    }
}
