using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using ATT.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using FluencyCSharp;

namespace ATT.Scripts
{
    [Script("ATT Upload")]
    public class ATTUpload : ScriptBase<ATTUploadData>
    {
        private ATTDbContext _db;

       

        private List<Task> allTasks;

        private List<Task> allPayloadsTasks;
    

        //private List<Task> allPayloadsTasks;

        public ATTUpload() {
            _db = new ATTDbContext();
            
            allTasks =new List<Task>();
            allPayloadsTasks =new List<Task>();
        }

        [Step(Id = 1, Name = "Run")]
        public void Run() {

            var interfaces = _db.SAPInterfaces.Where(s => s.IsSelected == true).ToList();
            var messageData = _data.MessageData.Copy() as MSGIDTaskData;
            messageData.Mid = _data.Mid;
            messageData.Start = _data.Start;
            messageData.ExpireDate = _data.ExpireDate;
            messageData.Interval = _data.Interval;

            int mid = _data.Mid;

            foreach (var i in interfaces) {
                messageData.SAPInterface = i;
                ScriptEngine<MSGIDTask, MSGIDTaskData> sapScript = new ScriptEngine<MSGIDTask, MSGIDTaskData>();
                sapScript.Run(messageData);
                Task.Run(() => downloadPayloads(mid)).AppendTo(allTasks);
            }
            Task.WaitAll(allTasks.ToArray());
        }

        private object _lockObj = new object();

        private void downloadPayloads(int mid) {
            lock (_lockObj) {

                int taskId = 0;

                do {
                    using (var db = new ATTDbContext()) {
                        SqlParameter para1 = new SqlParameter("mid",mid);
                        SqlParameter para2 = new SqlParameter("tid", System.Data.SqlDbType.Int);
                        para2.Direction = System.Data.ParameterDirection.Output;
                        db.Database.ExecuteSqlCommand("exec SP_GetTaskId @mid, @tid output", para1,para2);
                        taskId = int.Parse(para2.Value.ToString());
                    }


                    if (taskId > 0) {
                        var t = getPayloadsDownloadTask(taskId);
                        t.AppendTo(allPayloadsTasks);
                    }


                    if (allPayloadsTasks.Count >= 5) {
                        int tid = Task.WaitAny(allPayloadsTasks.ToArray());
                    }

                }
                while (taskId > 0);
            }

            Task.WaitAll(allPayloadsTasks.ToArray());
        }

        private Task getPayloadsDownloadTask(int taskId) {
            return Task.Run(() => {

                //Process.GetCurrentProcess().Threads.Count;

                ScriptEngine<PayloadsDownloader, PayloadsDownloaderData> downloadScript = new ScriptEngine<PayloadsDownloader, PayloadsDownloaderData>();
                PayloadsDownloaderData d = _data.DownloadData.Copy();
                d.TaskId = taskId;

                PayloadsUpdateData d1 = _data.UpdateData.Copy();
                d1.TaskId = taskId;

                PayloadsUploaderData d2 = _data.UploadData.Copy();
                d2.TaskId = taskId;
                Debug.WriteLine("Current has {0} Threads", Process.GetCurrentProcess().Threads.Count);
                Debug.WriteLine("Thread:{0} is running payloads task:{1}", Thread.CurrentThread.ManagedThreadId, taskId);
                downloadScript.Run(d);
                //Debug.WriteLine("Thread:{0} Finished Download task:{1}", Thread.CurrentThread.ManagedThreadId, taskId);

                ScriptEngine<PayloadsUpdate, PayloadsUpdateData> updateScript = new ScriptEngine<PayloadsUpdate, PayloadsUpdateData>();

                //Debug.WriteLine("Thread:{0} is running update task:{1}", Thread.CurrentThread.ManagedThreadId, taskId);
                updateScript.Run(d1);



                ScriptEngine<PayloadsUploader, PayloadsUploaderData> uploadScript = new ScriptEngine<PayloadsUploader, PayloadsUploaderData>();
                uploadScript.Run(d2);
                Debug.WriteLine("Thread:{0} Finished payloads task:{1}", Thread.CurrentThread.ManagedThreadId, taskId);
            });
        }


    }
}
