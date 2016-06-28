namespace ATT.Data
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
            MsgIDs = new HashSet<MsgIDs>();
        }

     
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDt { get; set; }

        public int? StartHour { get; set; }

        public int? MissionId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MsgIDs> MsgIDs { get; set; }
    }
}
