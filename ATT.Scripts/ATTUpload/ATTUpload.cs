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

        private Task payloadsDownloadTasks;

        private List<Task> allTasks;

        private List<Task> allPayloadsTasks;

        public ATTUpload() {
            _db = new ATTDbContext();
            payloadsDownloadTasks = new Task(() => { downloadPayloads(); });
            allTasks = new List<Task>();
            allPayloadsTasks = new List<Task>();
        }

        [Step(Id = 1, Name = "Run")]
        public void Run() {

            var interfaces = _db.SAPInterfaces.Where(s => s.IsSelected == true).ToList();

            //var mission = new Missions() {
            //    StartDt = _data.Start,
            //    StartHour = _data.Start.Hour
            //};

            //_db.Missions.Add(mission);

            //_db.SaveChanges();

            _data.MessageData.Mid = _data.Mid;




            foreach (var i in interfaces) {
                _data.MessageData.SAPInterface = i;
                _data.MessageData.Start = _data.Start;
                _data.MessageData.ExpireDate = _data.ExpireDate;
                _data.MessageData.Interval = _data.Interval;
                ScriptEngine<MSGIDTask, MSGIDTaskData> sapScript = new ScriptEngine<MSGIDTask, MSGIDTaskData>();
                sapScript.Run(_data.MessageData);

                Task.Run(() => downloadPayloads()).AppendTo(allTasks);

                //if (payloadsDownloadTasks.Status == TaskStatus.Created) {
                //    payloadsDownloadTasks.Start();
                //} else {
                //    Task.Run(() => downloadPayloads());
                //}
                //if (payloadsDownloadTasks.IsCompleted) {

                //    payloadsDownloadTasks = new Task(() => {
                //        downloadPayloads();
                //    });
                //    payloadsDownloadTasks.Start();
                //}
            }
            Task.WaitAll(allTasks.ToArray());

            //if(payloadsDownloadTasks.IsCompleted == false) {
            //    payloadsDownloadTasks.Wait();
            //}

            //ScriptEngine<Payloads, PayloadsData> payloadsScript = new ScriptEngine<Payloads, PayloadsData>();
            //PayloadsData d = new PayloadsData();
            //d.DownloadData = _data.DownloadData;
            //d.UpdateData = _data.UpdateData;
            //d.UploadData = _data.UploadData;
            //payloadsScript.Run(d);
        }

        private object _lockObj = new object();

        private void downloadPayloads() {
            //List<Task> tasks = new List<Task>();

            lock (_lockObj) {
                int taskId = 0;

                do {
                    using (var db = new ATTDbContext()) {
                        SqlParameter para = new SqlParameter("tid", System.Data.SqlDbType.Int);
                        para.Direction = System.Data.ParameterDirection.Output;
                        db.Database.ExecuteSqlCommand("exec SP_GetTaskId @tid output", para);
                        taskId = int.Parse(para.Value.ToString());
                    }


                    if (taskId > 0) {
                        allPayloadsTasks.Add(getPayloadsDownloadTask(taskId));
                    }


                    if (allPayloadsTasks.Count >= 5) {
                        int tid = Task.WaitAny(allPayloadsTasks.ToArray());
                        allPayloadsTasks.RemoveAt(tid);
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
