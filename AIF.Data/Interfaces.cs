namespace AIF.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Interfaces
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Interfaces()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string MsgType { get; set; }

        [StringLength(10)]
        public string MsgCode { get; set; }

        [StringLength(20)]
        public string MsgFunction { get; set; }

        [StringLength(50)]
        public string PartnerNo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
