using ATT.Data;
using ATT.Data.ATT;
using SAPAutomation;
using SAPFEWSELib;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    [Script("PII Track")]
    public class PIITrack : ScriptBase<PIITrackData>
    {
        private SAPLogon sap;
        private string _file;

        [Step(Id =1,Name ="Login to SAP PII")]
        public void Login() {
            sap = UIExtension.Login(_data.SAPAccount);
            Guid guid = Guid.NewGuid();
            _file = Path.Combine(_data.WorkFolder,$"{guid.ToString()}.txt");
        }

        [Step(Id =2,Name ="Down load Report")]
        public void DownloadFile() {
            sap.Session.StartTransaction("SA38");
            sap.MainWindow.FindByName<GuiCTextField>("RS38M-PROGRAMM").Text = "AQ20SYSTQV000002ZIF_ATT_REPORT";
            sap.MainWindow.FindByName<GuiButton>("btn[8]").Press();
            
            sap.MainWindow.FindByName<GuiTextField>("SP$00001-LOW").Text = _data.SAPAccount.UserName;
            sap.MainWindow.FindByName<GuiTextField>("SP$00002-LOW").Text = _data.Start.ToString("yyyyMMddHH0000");
            sap.MainWindow.FindByName<GuiTextField>("SP$00002-HIGH").Text = _data.GetEnd().ToString("yyyyMMddHH0000");
            
            sap.MainWindow.FindByName<GuiRadioButton>("%DOWN").Select();
            sap.MainWindow.FindByName<GuiTextField>("%PATH").Text = _file;
            if (File.Exists(_file)) {
                File.Delete(_file);
            }
            sap.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            if(sap.Session.Info.ScreenNumber != 1000) {
                sap.PopupWindow.FindByName<GuiCheckBox>("RSAQDOWN-COLUMN").Selected = true;
                sap.PopupWindow.FindByName<GuiButton>("btn[0]").Press();
            }

            sap.CloseSession();
        }

        [Step(Id =3,Name ="Read the report")]
        public void ReadFile() {
            if (File.Exists(_file)) {
                var msgId_ITGs = Tools.GetDataEntites<MsgIDs_ITG>(_file);
                using(var db = new ATTDbContext()) {
                    db.MsgIDs_ITG.AddRange(msgId_ITGs);
                    db.SaveChanges();
                }
            }
        }

        //[Step(Id =4,Name ="Update Schedule Info")]
        //public void UpdateSchedule() {
        //    _data.Start = _data.Start.GetNext(_data.Interval);
        //}


    }
}
