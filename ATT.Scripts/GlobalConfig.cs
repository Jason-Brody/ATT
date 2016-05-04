using ScriptRunner.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public static class GlobalConfig
    {
        public static readonly string AttWorkDir = @"C:\ATT";

        public static readonly string AIFWorkDir = @"C:\AIF";

        public static void BindingStepInfo(IStepProcess script) {
            script.BeforeStepExecution += s => Console.WriteLine($"{s.Name} is Running.");
            script.AfterStepExecution += s => Console.WriteLine($"{s.Name} is Complete");

        }

        static GlobalConfig() {
            CreateDirectory(AttWorkDir);
            CreateDirectory(AIFWorkDir);
        }

        public static string CreateDirectory(string dir) {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        public static string GetGuid() {
            return Guid.NewGuid().ToString();
        }

        public static string GetAttFile(string subFolder,string Prefix,string extension) {
            return Path.Combine(AttWorkDir, $"{Prefix}.{extension}");
        }

        public static int DownloadPatchSize { get; } = 50;

        //public static Lazy<EDIKeyConfig> EdiKey { get; } = new Lazy<EDIKeyConfig>();

        //public static Lazy<PayloadsConfig> PayLoads { get; } = new Lazy<PayloadsConfig>();

        //public static Lazy<PayLoadsDownloadConfig> PayLoadsDownload { get; } = new Lazy<PayLoadsDownloadConfig>();

        //public static Lazy<MSGID_ReportConfig> MSGID_ReportConfig { get; } = new Lazy<MSGID_ReportConfig>();

    }

    //public abstract class BaseFolder
    //{
    //    public BaseFolder() {
    //        GlobalConfig.CreateDirectory(WorkFolder);
    //    }

    //    public abstract string WorkFolder {get; }
    //}


    //public class EDIKeyConfig:BaseFolder
    //{
    //    private string guid;

    //    public readonly string IDocReportPrefix = "IDocReport";

    //    public readonly string EDIKeyPrefix = "EDIArchiveKey";

    //    public void NewInterfacePrefix(string Interface) {
    //        guid = Interface + "_" + Guid.NewGuid().ToString();
    //    }

    //    public string IDocReportFile {
    //        get { return Path.Combine(WorkFolder, $"{IDocReportPrefix}_{guid}.txt");  }
    //    }

    //    public string EDIKeyFile {
    //        get { return Path.Combine(WorkFolder, $"{EDIKeyPrefix}_{guid}.txt"); }
    //    }

    //    public override string WorkFolder { get; } = Path.Combine(GlobalConfig.WorkDir, "EDIKeyFiles");

    //}

    //public class PayloadsConfig:BaseFolder
    //{
        
    //    public override string WorkFolder { get; }= Path.Combine(GlobalConfig.WorkDir, "Payloads");

    //    public string XPath_AWSYS { get; } = "//AWSYS";

    //    public string XPath_OBJ_SYS { get; } = "//OBJ_SYS";

    //    public string XPath_OTC_rec_type { get; } = "//OTC_rec_type";

    //}

    //public class PayLoadsDownloadConfig : BaseFolder
    //{
    //    public override string WorkFolder { get; } = Path.Combine(GlobalConfig.WorkDir, "PayloadsDownloads");
    //}

    //public class MSGID_ReportConfig : BaseFolder
    //{
    //    public override string WorkFolder { get; } = Path.Combine(GlobalConfig.WorkDir, "MSGID_Report");
          
    //}
}
