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
            PayloadsData d = GetConfigData<PayloadsData>();
            ScriptEngine<Payloads, PayloadsData> script = new ScriptEngine<Payloads, PayloadsData>();
            BindingStepInfo(script);
            script.Run(d);
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
            BindingStepInfo(script);
            d.SetTaskId(TaskId);
            script.StepProgress.ProgressChanged += (s, e) => { Console.WriteLine($"{e.Msg} sent,{e.Current} of {e.Total} finished,TaskId:{TaskId}"); };
            script.Run(d);
        }

        static void GetMessageReport() {
            MSGID_ReportData d = GetConfigData<MSGID_ReportData>();
            ScriptEngine<MSGID_Report, MSGID_ReportData> script = new ScriptEngine<MSGID_Report, MSGID_ReportData>();
            BindingStepInfo(script);
            script.Run(d);
            SetConfigData(d);
        }

        static void TrackITG() {
            ITGTrackData d = GetConfigData<ITGTrackData>();
            ScriptEngine<ITGTrack, ITGTrackData> script = new ScriptEngine<ITGTrack, ITGTrackData>();
            BindingStepInfo(script);
            script.Run(d);
            SetConfigData(d);
        }

        static void DownloadPayloads(int id) {
            PayloadsDownloaderData d = GetConfigData<PayloadsDownloaderData>();
            ScriptEngine<PayloadsDownloader, PayloadsDownloaderData> script = new ScriptEngine<PayloadsDownloader, PayloadsDownloaderData>();
            d.SetTaskId(id);
            BindingStepInfo(script);
            script.Run(d);
        }

        

        

        
    }
}
