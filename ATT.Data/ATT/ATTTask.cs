using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Data.ATT
{
    public enum ATTTask
    {
        GetMessageId = 1,
        DownloadPayloads = 2,
        UpdatePayloads = 3,
        UploadPayloads = 4,
        PIITrack = 5,
        LHTrack = 6,
        DownloadAndTransform = 7,
        AIFMassUpload = 8,
        GetMessageAll = 100,

    }
}
