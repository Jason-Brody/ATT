using ATT.Data.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{

    public class ATTLog
    {
        public LogType LogTp { get; set; }

        public DateTime LogDt { get; set; }

        public int TaskId { get; set; }

        public string Description { get; set; }

        public string FileName { get; set; }
    }

    public delegate void OnLogHandler(ATTLog l);

    public class ATTPayLoadsLog
    {
        public static event OnLogHandler OnLog;

        public static void Write(ATTLog logItem) {
            string msg = $"{logItem.LogDt.ToString("yyyy.MM.dd HH:mm:ss")}--[{logItem.LogTp.ToString()}]--[{logItem.TaskId}]--{logItem.Description}";
            using (var fs = new FileStream(logItem.FileName, FileMode.Append)) {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(msg);
                sw.Close();
            }
            OnLog?.Invoke(logItem);
        }



        //private PayloadsShareData data;

        //public ATTLog(PayloadsShareData d) {
        //    this.data = d;
        //}

        //public void WriteLog(string msg, LogType tp) {

        //    if (tp != LogType.Normal) {
        //        string file = Path.Combine(data.TaskFolder, $"{data.TaskId}.txt");
        //        string time = DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss");
        //        string message = $"{tp.ToString()}------{msg} TaskId:[{data.TaskId}] time:[{time}]";
        //        Console.WriteLine(message);
        //        using (var fs = new FileStream(file, FileMode.Append)) {
        //            StreamWriter sw = new StreamWriter(fs);
        //            sw.WriteLine(message);
        //            sw.Close();
        //        }
        //    }


        //}


    }
}
