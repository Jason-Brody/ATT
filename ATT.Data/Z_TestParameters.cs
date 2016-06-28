namespace ATT.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Z_TestParameters
    {
        public int Id { get; set; }

        public int? SourceId { get; set; }

        [StringLength(50)]
        public string System { get; set; }

        [StringLength(50)]
        public string Sndpor { get; set; }

        [StringLength(50)]
        public string sndprn { get; set; }

        [StringLength(50)]
        public string proawsys { get; set; }

        [StringLength(50)]
        public string itgawsys { get; set; }

        public virtual Sources Sources { get; set; }
    }
}
