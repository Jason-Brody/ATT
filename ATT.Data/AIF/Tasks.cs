namespace ATT.Data.AIF
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
            IDocNumbers = new HashSet<IDocNumbers>();
            IDocs = new HashSet<IDocs>();
        }

        public int Id { get; set; }

        public int? InterfaceId { get; set; }

        public bool? IsProcess { get; set; }

        public bool? IsFinished { get; set; }

        public int? Mid { get; set; }

        public int? DataCount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IDocNumbers> IDocNumbers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IDocs> IDocs { get; set; }

        public virtual Interfaces Interfaces { get; set; }

        public virtual Missions Missions { get; set; }
    }
}
