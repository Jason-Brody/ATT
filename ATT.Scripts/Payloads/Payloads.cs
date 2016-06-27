using ATT.Data;
using ATT.Data.ATT;
using ATT.Data.Entity;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ATT.Scripts
{
    [Script("Payloads")]
    public class Payloads : ScriptBase<PayloadsData>
    {
        [Step(Id =1,Name ="Download and Update payloads")]
        public void DispatchTask() {
            using (var db = new ATTDbContext()) {
                var p1 = new SqlParameter("patchSize", GlobalConfig.DownloadPatchSize);
                var p2 = new SqlParameter("from", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                var p3 = new SqlParameter("to", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                db.Database.ExecuteSqlCommand("exec SP_DispatchTask @patchSize,@from output,@to output", p1, p2, p3);
                var from = int.Parse(p2.Value.ToString());
                var to = int.Parse(p3.Value.ToString());
                List<Task> payloadsTasks = new List<Task>();
                int count = 0;

                if (from > to) {
                    throw new BreakException("No Data Found");
                }


                for (int i = from; i <= to; i++) {
                    if (count >= 5) {
                        int tid = Task.WaitAny(payloadsTasks.ToArray());
                        payloadsTasks.RemoveAt(tid);
                    }
                    int a = i;
                    Task t = GetDownloadAndUpdateTask(a);
                    payloadsTasks.Add(t);
                    count++;
                }

              

                Task.WaitAll(payloadsTasks.ToArray());

                //ExitWithMsg($"Download and Update Finished from {from} to {to}");
            }
        }

       


        private Task GetDownloadAndUpdateTask(int taskId) {
            Task t = new Task(() => {
                //Console.WriteLine("Thread {0} is running task {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, taskId);
                ScriptEngine<PayloadsDownloader, PayloadsDownloaderData> downloadScript = new ScriptEngine<PayloadsDownloader, PayloadsDownloaderData>();
                var d = _data.Copy(_data.DownloadData);
                d.SetTaskId(taskId);
                GlobalConfig.BindingStepInfo(downloadScript);
                downloadScript.Run(d);

                ScriptEngine<PayloadsUpdate, PayloadsUpdateData> updateScript = new ScriptEngine<PayloadsUpdate, PayloadsUpdateData>();
                var d1 = _data.Copy(_data.UpdateData);
                d1.SetTaskId(taskId);
                GlobalConfig.BindingStepInfo(updateScript);
                updateScript.Run(d1);
                

                ScriptEngine<PayloadsUploader, PayloadsUploaderData> uploadScript = new ScriptEngine<PayloadsUploader, PayloadsUploaderData>();
                var d3 = _data.Copy(_data.UploadData);
                d3.SetTaskId(taskId);
                GlobalConfig.BindingStepInfo(uploadScript);
                uploadScript.StepProgress.ProgressChanged += (s, e) => { Console.WriteLine($"{e.Msg} sent,{e.Current} of {e.Total} finished,TaskId:{taskId}"); };
                uploadScript.Run(d3);
                //Console.WriteLine("Thread {0} Finished task {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, taskId);
            });
            t.Start();
            return t;

        }

       
    }
}
