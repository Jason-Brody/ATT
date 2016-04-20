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

        [StringLength(50)]
        public string IDocType { get; set; }

        [StringLength(50)]
        public string Awsys { get; set; }

        public bool IsProcess { get; set; }

        public bool IsDownload { get; set; }

        public bool IsTransformed { get; set; }

        public bool IsSend { get; set; }

        public DateTime? CreateDt { get; set; }

        public DateTime? DownloadDt { get; set; }

        public DateTime? SentDt { get; set; }
    }
}
