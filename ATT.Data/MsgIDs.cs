namespace ATT.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MsgIDs
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string IDocNumber { get; set; }

        [StringLength(50)]
        public string MsgId { get; set; }

        public int? IDocTypeId { get; set; }

        public int? ProAwsysId { get; set; }

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

        public virtual IDocTypes IDocTypes { get; set; }

        public virtual ProAwsys ProAwsys { get; set; }

        public virtual Tasks Tasks { get; set; }
    }
}
