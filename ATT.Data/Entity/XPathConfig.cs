namespace ATT.Data.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class XPathConfig
    {
        public int Id { get; set; }

        public int? IDocTypeId { get; set; }

        public int? XNodeId { get; set; }

        [StringLength(100)]
        public string XPath { get; set; }

        public virtual IDocType IDocType { get; set; }

        public virtual XNode XNode { get; set; }
    }
}
