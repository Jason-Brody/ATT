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
        private PayloadsShareData data;

        public ATTLog(PayloadsShareData d) {
            this.data = d;
        }

        public void WriteLog(string msg, LogType tp) {

            if (tp != LogType.Normal) {
                string file = Path.Combine(data.TaskFolder, $"{data.TaskId}.txt");
                string time = DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss");
                string message = $"{tp.ToString()}------{msg} TaskId:[{data.TaskId}] time:[{time}]";
                Console.WriteLine(message);
                using (var fs = new FileStream(file, FileMode.Append)) {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(message);
                    sw.Close();
                }
            }


        }


    }
}
