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
       
        public void WriteLog(string msg) {
            string file = data.TaskFolder + ".txt";
            string time = DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss");
            string message = $"{msg} TaskId:[{data.TaskId}] time:[{time}]"; 
            using(var fs = new FileStream(file, FileMode.OpenOrCreate)) {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(message);
                sw.Close();
            } 
        }

        
    }
}
