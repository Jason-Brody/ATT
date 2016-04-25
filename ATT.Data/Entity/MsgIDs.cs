namespace ATT.Data.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Young.Data.Attributes;
    public partial class MsgIDs
    {
        public int Id { get; set; }

        [ColMapping("IDoc number")]
        [StringLength(50)]
        public string IDocNumber { get; set; }

        [MsgIdConverter]
        [ColMapping("EDI Archive Key")]
        [StringLength(50)]
        public string MsgId { get; set; }

        public int? IDocTypeId { get; set; }

        public int? ProAwsysId { get; set; }

        public int? InterfaceId { get; set; }

        public bool IsProcess { get; set; }

        public bool IsDownload { get; set; }

        public bool IsTransformed { get; set; }

        public bool IsSend { get; set; }

        public DateTime? CreateDt { get; set; }

        public DateTime? DownloadDt { get; set; }

        public DateTime? TransformDt { get; set; }

        public DateTime? SentDt { get; set; }

        public int? TaskId { get; set; }

        public bool? IsNeedTransform { get; set; }

        public virtual IDocType IDocType { get; set; }

        public virtual ProAwsy ProAwsy { get; set; }
    }
}
