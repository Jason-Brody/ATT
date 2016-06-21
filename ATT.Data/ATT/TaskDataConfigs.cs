//namespace ATT.Data.ATT
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Data.Entity.Spatial;

//    public partial class TaskDataConfigs
//    {
//        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
//        //public int Id { get; set; }

//        [Key]
//        [Column("Id")]
//        public ATTTask AttTask { get; set; }

//        [StringLength(50)]
//        public string Name { get; set; }

//        [Column(TypeName = "xml")]
//        public string Data { get; set; }

//        [StringLength(255)]
//        public string TypeName { get; set; }
//    }
//}
