using ATT.Data;
using ATT.Data.Entity;
using ATT.Scripts;
using ScriptRunner.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Robot
{
    partial class Program
    {
        static void GetMessageId() {
            MSGIDTaskData d = GetConfigData<MSGIDTaskData>();
            ScriptEngine<MSGIDTask, MSGIDTaskData> script = new ScriptEngine<MSGIDTask, MSGIDTaskData>();
            BindingStepInfo(script);
            script.StepProgress.ProgressChanged += (s,e) => RunTask(ATTTask.DownloadAndTransform, 0);
            script.Run(d);
            SetConfigData(d);
        }

        static void DownloadAndUpdatePayloads() {
            using (var db = new AttDbContext()) {
                var p1 = new SqlParameter("patchSize", GlobalConfig.DownloadPatchSize);
                var p2 = new SqlParameter("from", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                var p3 = new SqlParameter("to", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                db.Database.ExecuteSqlCommand("exec SP_DispatchTask @patchSize,@from output,@to output", p1, p2, p3);
                var from = int.Parse(p2.Value.ToString());
                var to = int.Parse(p3.Value.ToString());
                List<Task> payloadsTasks = new List<Task>();
                int count = 0;

                if (from > to) {
                    ExitWithMsg("No Data Found.");
                    return;
                }

                for (int i = from; i <= to; i++) {
                    if (count >= 5) {
                        int tid = Task.WaitAny(payloadsTasks.ToArray());
                        payloadsTasks.RemoveAt(tid);
                    }
                    payloadsTasks.Add(GetDownloadAndUpdateTask(i));
                    count++;
                }

                Task.WaitAll(payloadsTasks.ToArray());

                ExitWithMsg($"Download and Update Finished from {from} to {to}");
            }
        }

        static void UpdatePayloads(int TaskId) {
            PayloadsUpdateData d = GetConfigData<PayloadsUpdateData>();
            d.SetTaskId(TaskId);
            ScriptEngine<PayloadsUpdate, PayloadsUpdateData> script = new ScriptEngine<PayloadsUpdate, PayloadsUpdateData>();
            BindingStepInfo(script);
            script.Run(d);
        }

        static void UploadPayloads(int TaskId) {
            PayloadsUploaderData d = GetConfigData<PayloadsUploaderData>();
            ScriptEngine<PayloadsUploader, PayloadsUploaderData> script = new ScriptEngine<PayloadsUploader, PayloadsUploaderData>();
            script.Run(d);
        }

        static void GetMessageReport() {
            MSGID_ReportData d = GetConfigData<MSGID_ReportData>();
            ScriptEngine<MSGID_Report, MSGID_ReportData> script = new ScriptEngine<MSGID_Report, MSGID_ReportData>();
            script.Run(d);
            SetConfigData(d);
        }

        static void TrackITG() {
            ITGTrackData d = GetConfigData<ITGTrackData>();
            ScriptEngine<ITGTrack, ITGTrackData> script = new ScriptEngine<ITGTrack, ITGTrackData>();
            script.Run(d);
            SetConfigData(d);
        }

        static void DownloadPayloads() {
            PayloadsDownloaderData d = GetConfigData<PayloadsDownloaderData>();
            ScriptEngine<PayloadsDownloader, PayloadsDownloaderData> script = new ScriptEngine<PayloadsDownloader, PayloadsDownloaderData>();
            script.Run(d);
        }

        static Task GetDownloadAndUpdateTask(int taskId) {
            return Task.Run(() => {
                Console.WriteLine("Thread {0} is running task {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, taskId);
                var d = GetConfigData<PayloadsTransformData>();
                d.SetTaskId(taskId);
                ScriptEngine<PayloadsTransform, PayloadsTransformData> script = new ScriptEngine<PayloadsTransform, PayloadsTransformData>();
                BindingStepInfo(script);
                script.Run(d);
                Console.WriteLine("Thread {0} Finished task {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, taskId);
            });

        }

        

        
    }
}
