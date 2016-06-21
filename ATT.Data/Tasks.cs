namespace ATT.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tasks
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tasks()
        {
            MsgIDs = new HashSet<MsgIDs>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? Mid { get; set; }

        public bool? IsProcess { get; set; }

        public int? InterfaceId { get; set; }

        public bool? IsFinished { get; set; }

        public DateTime? StartDt { get; set; }

        public DateTime? EndDt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MsgIDs> MsgIDs { get; set; }

        public virtual SAPInterfaces SAPInterfaces { get; set; }
    }
}
