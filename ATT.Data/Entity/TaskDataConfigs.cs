using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Data.Entity
{
    public partial class TaskDataConfigs
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public int Id { get; set; }

        [Key]
        [Column("Id")]
        public ATTTask AttTask { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "xml")]
        public string Data { get; set; }

        [StringLength(255)]
        public string TypeName { get; set; }
    }

    public enum ATTTask
    {
        GetMessageId = 1,
        DownloadPayloads = 2,
        UpdatePayloads = 3,
        UploadPayloads = 4,
        PIITrack = 5,
        LHTrack = 6,
        DownloadAndTransform = 7,
        AIFMassUpload = 8,
        GetMessageAll = 100,

    }
}
