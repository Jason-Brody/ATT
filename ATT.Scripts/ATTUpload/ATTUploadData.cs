using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ATT.Scripts
{
    public class ATTUploadData : ScheduleData
    {
        public PayloadsDownloaderData DownloadData { get; set; }

        public PayloadsUpdateData UpdateData { get; set; }

        public PayloadsUploaderData UploadData { get; set; }

        public MSGIDTaskData MessageData { get; set; }

        [XmlIgnore]
        public int Mid { get; set; }
    }
}
