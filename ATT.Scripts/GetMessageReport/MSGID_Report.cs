using ATT.Data;
using ATT.Data.Entity;
using SAPAutomation;
using SAPFEWSELib;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class MSGID_Report:ScriptBase<MSGID_ReportData>
    {
        private string _file;

        [Step(Id =1,Name ="Login to SAP PII")]
        public void Login() {
            UIHelper.Login(_data);
            Guid guid = Guid.NewGuid();
            _file = Path.Combine(_data.WorkFolder,$"{guid.ToString()}.txt");
        }

        [Step(Id =2,Name ="Down load Report")]
        public void DownloadFile() {
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("SA38");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("RS38M-PROGRAMM").Text = "AQ20SYSTQV000002ZIF_ATT_REPORT";
            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("SP$00001-LOW").Text = _data.UserName;
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("SP$00002-LOW").Text = _data.GetStart().ToString("yyyyMMddHH0000");
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("SP$00002-HIGH").Text = _data.GetEnd().ToString("yyyyMMddHH0000");

            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("%DOWN").Select();
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("%PATH").Text = _file;
            if (File.Exists(_file)) {
                File.Delete(_file);
            }
            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            if(SAPTestHelper.Current.SAPGuiSession.Info.ScreenNumber != 1000) {
                SAPTestHelper.Current.PopupWindow.FindByName<GuiCheckBox>("RSAQDOWN-COLUMN").Selected = true;
                SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[0]").Press();
            }

            SAPTestHelper.Current.CloseSession();
        }

        [Step(Id =3,Name ="Read the report")]
        public void ReadFile() {
            if (File.Exists(_file)) {
                var msgId_ITGs = Tools.GetDataEntites<MsgIDs_ITG>(_file);
                using(var db = new AttDbContext()) {
                    db.MsgIDs_ITG.AddRange(msgId_ITGs);
                    db.SaveChanges();
                }
            }
        }

        [Step(Id =4,Name ="Update Schedule Info")]
        public void UpdateSchedule() {
            _data.Start = _data.Start.GetNext(_data.Interval);
        }


    }
}
