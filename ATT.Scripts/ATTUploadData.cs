//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SharedLib;
//using System.IO;

//namespace ATT.Scripts
//{
//    public class ATTUploadData:ScheduleData
//    {
//        private string _baseFolder;

//        private string _guid;

//        public ATTUploadData(string folder = "ATT") {
//            this._baseFolder = folder;;
//            GlobalConfig.CreateDirectory(_baseFolder);
            
//        }

//        public void NewGuid(string interfaceName) {
//            _guid = interfaceName + "_" + GlobalConfig.GetGuid();
//        }

//        public SAPLoginData LH1 { get; set; }

//        public string MessageFolder {
//            get {
//                string folder = Path.Combine(_baseFolder, "MessageIds");
//                GlobalConfig.CreateDirectory(folder);
//                return folder;
//            }
//        }

//        public string IDocStatus { get; } = "53";

//        public string IDocReportPrefix { get; } = "IDocReport";

//        public string EDIKeyPrefix { get; } = "EDIArchiveKey";

//        public string IDocReportFile {
//            get {
//                var file = GlobalConfig.GetAttFile(MessageFolder, IDocReportPrefix + "_" + _guid, "txt");
//                return file;
//            }
//        }

//        public string EDIKeyFile {
//            get {
//                var file = GlobalConfig.GetAttFile(MessageFolder, EDIKeyPrefix + "_" + _guid, "txt");
//                return file;
//            }
//        }
//    }
//}
