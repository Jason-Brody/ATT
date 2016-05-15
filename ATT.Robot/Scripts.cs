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
        static void GetMessageAll() {
            MSGIDTaskData d = GetConfigData<MSGIDTaskData>();
            ScriptEngine<MSGIDTask, MSGIDTaskData> script = new ScriptEngine<MSGIDTask, MSGIDTaskData>();
            BindingStepInfo(script);
            script.StepProgress.ProgressChanged += (s, e) => { Console.WriteLine("Interface {0} Finished", e.Msg); RunTask(ATTTask.DownloadAndTransform, 0); };
            script.Run(d);
            SetConfigData(d);
        }

        static void GetMessageId() {
            MSGIDTaskData d = GetConfigData<MSGIDTaskData>();
            ScriptEngine<MSGIDTask, MSGIDTaskData> script = new ScriptEngine<MSGIDTask, MSGIDTaskData>();
            BindingStepInfo(script);
            script.StepProgress.ProgressChanged += (s, e) => Console.WriteLine("Interface {0} Finished", e.Msg);
            script.Run(d);
            SetConfigData(d);
        }

        static void TransformPayloads() {
            PayloadsData d = new PayloadsData();
            d.DownloadData = GetConfigData<PayloadsDownloaderData>();
            d.UpdateData = GetConfigData<PayloadsUpdateData>();
            d.UploadData = GetConfigData<PayloadsUploaderData>();
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
            PIITrackData d = GetConfigData<PIITrackData>();
            ScriptEngine<PIITrack, PIITrackData> script = new ScriptEngine<PIITrack, PIITrackData>();
            BindingStepInfo(script);
            script.Run(d);
            SetConfigData(d);
        }

        static void TrackLH() {
            LHTrackData d = GetConfigData<LHTrackData>();
            ScriptEngine<LHTrack, LHTrackData> script = new ScriptEngine<LHTrack, LHTrackData>();
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



        static void AIFMassUpload(int id) {
            AIFMassUploadData d = GetAIFConfigData(id);
            d.TaskId = id;
            ScriptEngine<AIFMassUpload, AIFMassUploadData> script = new ScriptEngine<AIFMassUpload, AIFMassUploadData>();
            BindingStepInfo(script);
            script.Run(d);
        }




    }
}
