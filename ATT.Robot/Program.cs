using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATT.Scripts;
using ScriptRunner.Interface;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;
using ATT.Data;
using ATT.Data.Entity;
using System.Xml.Serialization;
namespace ATT.Robot
{
    
    public partial class Program
    {
        //static string getUTCDate(DateTime dt) {
        //   var dateTemp = dt.ToUniversalTime().ToString("yyyyMMddHH0000");
        //}

        static void Main(string[] args) {


            TrackLH();

            ATTTask t;
            int taskId;

            if (args.Count() == 0) {
                
                foreach(var item in Enum.GetNames(typeof(ATTTask))) {
                    Console.WriteLine("{0} -- {1}", (int)Enum.Parse(typeof(ATTTask),item)  ,item);
                }
                Console.Write("Please choose task:");
                int taskType = int.Parse(Console.ReadLine());
                t = (ATTTask)Enum.ToObject(typeof(ATTTask), taskType);
                Console.WriteLine("{0} -- {1}",taskType, t);
                Console.Write("Please choose a task id:");
                taskId = int.Parse(Console.ReadLine());

            } else {
                t = (ATTTask)Enum.Parse(typeof(ATTTask), args[0]);
                taskId = int.Parse(args[1]);
            }
            
            RunScript(t, taskId);
        }

        static void ExitWithMsg(string Msg, int secondsToDelay = 10) {
            Console.WriteLine(Msg);
            for (int i = secondsToDelay; i >= 0; i--) {
                Task.Delay(1000).Wait();
                Console.SetCursorPosition(0, Console.CursorTop);                
                Console.Write("Quit in {0} seconds    ", i);
                
            }
        }

        public static void RunTask(ATTTask task, int taskId) {
            var exeFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ATT.Robot.exe");
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = exeFile;
            psInfo.Arguments = $"{task} {taskId}";
            Process.Start(psInfo);
        }

        static void RunScript(ATTTask script, int TaskId) {
            try {
                switch (script) {
                    case ATTTask.GetMessageId:
                        GetMessageId();
                        break;
                    case ATTTask.DownloadAndTransform:
                        TransformPayloads();
                        break;
                    case ATTTask.DownloadPayloads:
                        DownloadPayloads(TaskId);
                        break;
                    case ATTTask.UpdatePayloads:
                        UpdatePayloads(TaskId);
                        break;
                    case ATTTask.UploadPayloads:
                        UploadPayloads(TaskId);
                        break;
                    case ATTTask.PIITrack:
                        GetMessageReport();
                        break;
                    case ATTTask.LHTrack:
                        TrackLH();
                        break;
                    case ATTTask.GetMessageAll:
                        GetMessageAll();
                        break;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

       

        static void BindingStepInfo(IStepProcess script ) {
            script.BeforeStepExecution += s => Console.WriteLine($"{s.Name} is Running.");
            script.AfterStepExecution += s => Console.WriteLine($"{s.Name} is Complete");
            
        }

       
        public static T GetConfigData<T>() {
            string xmlData = null;
            using (var db = new AttDbContext()) {
                var tData = db.TaskDataConfigs.Single(t=>t.TypeName == typeof(T).FullName);
                xmlData = tData.Data;
            }
            StringReader sr = new StringReader(xmlData);
            XmlSerializer xs = new XmlSerializer(typeof(T));
         
            var obj = xs.Deserialize(sr);
            return (T)obj;
        }

        static object GetConfigData<T>(ATTTask task) {
            string xmlData = null;
            string typeName = null;
            using(var db = new AttDbContext()) {
                var tData = db.TaskDataConfigs.Single(t => t.AttTask == task);
                xmlData = tData.Data;
                typeName = tData.TypeName;
            }

            var tp = typeof(T);

            StringReader sr = new StringReader(xmlData);

            XmlSerializer xs = new XmlSerializer(tp);
            var obj = xs.Deserialize(sr);
            return obj;

        }

        public static void SetConfigData<T>(T data) {
            using (var db = new AttDbContext()) {
                var tData = db.TaskDataConfigs.Single(t => t.TypeName == typeof(T).FullName);
                var typeName = tData.TypeName;
                XmlSerializer xs = new XmlSerializer(typeof(T));
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                xs.Serialize(sw, data);
                tData.Data = sb.ToString();
                db.SaveChanges();
            }
        }

        static void SetConfigData<T>(ATTTask task,T data) {
            using (var db = new AttDbContext()) {
                var tData = db.TaskDataConfigs.Single(t => t.AttTask == task);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                xs.Serialize(sw, data);
                tData.Data = sb.ToString();
                db.SaveChanges();
            }
        }

        
    }
}
