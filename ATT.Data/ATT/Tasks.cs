//namespace ATT.Data.ATT
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Data.Entity.Spatial;

//    public partial class Tasks
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]
//        public int Id { get; set; }

//        public int? Mid { get; set; }

//        public bool IsProcess { get; set; }

//        public int? InterfaceId { get; set; }

//        public bool IsFinished { get; set; }

//        public virtual Missions Missions { get; set; }

//        public virtual SAPInterfaces SAPInterfaces { get; set; }
//    }
//}
