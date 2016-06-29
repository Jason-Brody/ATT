using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Client
{
    class GlobalConfig
    {
        
        public static string MSGIDTaskDataFile { get; } = "MSGIDTaskData.xml";

        public static string PayloadsDownloaderDataFile { get; } = "PayloadsDownloaderData.xml";

        public static string PayloadsUploaderDataFile { get; } = "PayloadsUploaderData.xml";

        public static string PIITrackDataFile { get; } = "PIITrackData.xml";

        public static string LHTrackDataFile { get; } = "LHTrackData.xml";
    }
}
