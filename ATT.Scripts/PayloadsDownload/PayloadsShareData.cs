using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{ 
    public class PayloadsShareData:UserInfo
    {
        protected int taskId;

        public int TaskId { get { return taskId; } }
        public string WorkFolder { get; } = Path.Combine(GlobalConfig.WorkDir, "PayloadsDownloads");

        public void SetTaskId(int TaskId) {
            this.taskId = TaskId;
        }

    }
}
