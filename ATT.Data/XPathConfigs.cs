namespace ATT.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class XPathConfigs
    {
        public int Id { get; set; }

        public int? IDocTypeId { get; set; }

        public int? XNodeId { get; set; }

        [StringLength(100)]
        public string XPath { get; set; }

        public virtual IDocTypes IDocTypes { get; set; }

        public virtual XNodes XNodes { get; set; }
    }
}
