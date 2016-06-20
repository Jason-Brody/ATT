namespace AIF.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Missions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Missions()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDt { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDt { get; set; }

        public int? DataLimit { get; set; }

        public bool IsUpload { get; set; }

        public bool Is56Process { get; set; }

        public bool IsErrorTrack { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
