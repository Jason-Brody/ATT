namespace ATT.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClientInterfaces
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public int? InterfaceId { get; set; }

        public virtual SAPInterfaces SAPInterfaces { get; set; }
    }
}
