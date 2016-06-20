namespace AIF.Data
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
            LH1IDocs = new HashSet<LH1IDocs>();
            LH7IDocs = new HashSet<LH7IDocs>();
        }

        public int Id { get; set; }

        public int? InterfaceId { get; set; }

        public bool IsProcess { get; set; }

        public DateTime? ProcessDt { get; set; }

        public bool IsFinished { get; set; }

        public DateTime? FinishDt { get; set; }

        public bool IsSelected { get; set; }

        public int? Mid { get; set; }

        public int? DataCount { get; set; }


        public virtual Interfaces Interfaces { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LH1IDocs> LH1IDocs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LH7IDocs> LH7IDocs { get; set; }

        public virtual Missions Missions { get; set; }
    }
}
