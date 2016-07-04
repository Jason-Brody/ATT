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

        public int ClientId { get; set; }

        public override ScheduleData Copy() {
            ATTUploadData d = new ATTUploadData();
            d.Start = this.Start;
            d.ExpireDate = this.ExpireDate;
            d.Interval = this.Interval;
            d.DownloadData = this.DownloadData.Copy();
            d.UpdateData = this.UpdateData.Copy();
            d.MessageData = this.MessageData.Copy() as MSGIDTaskData;
            d.UploadData = this.UploadData.Copy();
            return d;
        }
    }
}
