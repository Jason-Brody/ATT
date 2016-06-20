using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Young.Data;

namespace SharedLib
{
    public class Tools
    {
        public static List<string> GetDatas(string filePath)
        {
            List<string> datas = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    datas.Add(sr.ReadLine());
                }

            }
            return datas;
        }



        public static DataTable ReadToTable(string path,char splitChar = '\t')
        {
            
            DataTable dt = Young.Data.Utils.ReadStringToTable(path, (s, h,t) =>
            {
                if (!s.Contains(splitChar) || s == h || s.Contains("*"))
                    return null;


                var vals = s.Split(splitChar);
                if(t!=null && vals.Count() > t.Columns.Count) {
                    List<int> tableColumnLength = new List<int>();
                    var tempHeader = h;
                    bool isFinished = false;
                    while (!isFinished) {
                        var index = tempHeader.IndexOf(splitChar);
                        switch (index) {
                            case -1:
                                isFinished = true;
                                tableColumnLength.Add(0);
                                break;
                            default:
                                tableColumnLength.Add(index);
                                tempHeader = tempHeader.Substring(index+1, tempHeader.Length - index-1);
                                break;
                        }
                        
                    }
                    vals = new string[tableColumnLength.Count];
                    var last = 0;
                   
                    for(int i =0;i<tableColumnLength.Count;i++) {
                        vals[i] = s.Substring(last, tableColumnLength[i]);
                        last += tableColumnLength[i] + 1;
                    }
                }
                

                var returnVals = new List<string>();
                for (int i = 0; i < vals.Count(); i++)
                {
                    returnVals.Add(vals[i].Trim());
                }
                return returnVals;

            });

            return dt;
        }

        public static List<T> GetDataEntites<T>(string filePath,char splitChar = '\t') where T : class, new()
        {
            var dt = Tools.ReadToTable(filePath,splitChar);
            List<T> datas = dt.ToList<T>();
            return datas;
        }
    }
}
