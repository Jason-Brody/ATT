using SAPAutomation;
using SAPFEWSELib;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ATT.Data;
using SharedLib;

namespace ATT.Scripts
{
    [Script("LH Track")]
    public class LHTrack : ScriptBase<LHTrackData>
    {

        private SAPLogon sap;


        [Step(Id = 1, Name = "Login to SAP LH ITG")]
        public void Login() {
            sap = UIExtension.Login(_data.LH4);
        }

        [Step(Id =2,Name ="Check and Set User Config")]
        public void SetUserConfig() {
            sap.CheckUserConfig();
        }

        [Step(Id = 3, Name = "Get Message Ids")]
        public void GetMessageId() {

            _data.NewGuid();

            sap.Session.StartTransaction("ZIDOCAUDREP");
            if (sap.PopupWindow != null) {
                sap.PopupWindow.FindDescendantByProperty<GuiRadioButton>((r => r.Text.Contains("Continue With"))).Select();
                sap.PopupWindow.FindDescendantByProperty<GuiButton>(r => r.Text.Contains("Confirm Selection")).Press();
            }


            // Fill Control/Status Record Search
            sap.MainWindow.FindByName<GuiCTextField>("S_CREDAT-LOW").Text = _data.Start.ToString("dd.MM.yyyy");
           
            sap.MainWindow.FindByName<GuiCTextField>("S_CREDAT-HIGH").Text = _data.GetEnd().ToString("dd.MM.yyyy");
            
            sap.MainWindow.FindByName<GuiCTextField>("S_CRETIM-LOW").Text = _data.Start.ToString("HH:mm:ss");
            sap.MainWindow.FindByName<GuiCTextField>("S_CRETIM-HIGH").Text = _data.End;
            
            sap.MainWindow.FindByName<GuiCTextField>("S_STATUS-LOW").Text = _data.IDocStatus;
            sap.MainWindow.FindByName<GuiCTextField>("P_DIRECT").Text = "2";

            // Fill Message Type
            sap.MainWindow.FindByName<GuiRadioButton>("R_ACCDOC").Select();
            sap.MainWindow.FindByName<GuiCTextField>("S_MESTYP-LOW").Text = "ACC_DOCUMENT";

            //Fill IDoc Data Segments
            sap.MainWindow.FindByName<GuiTextField>("S_USNAM-LOW").Text = _data.UserId;

            //Fill Download Option
            sap.MainWindow.FindByName<GuiCheckBox>("P_DOWNLD").Selected = true;

            sap.MainWindow.FindByName<GuiTextField>("P_CPATH").Text = _data.File;

            //Report Output Options
            sap.MainWindow.FindByName<GuiRadioButton>("P_DOC").Select();

            sap.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            if (sap.PopupWindow != null)
                sap.PopupWindow.FindByName<GuiButton>("btn[0]").Press();


            sap.CloseSession();
        }


        [Step(Id =4,Name ="Upload File")]
        public void UploadFile() {
            if (File.Exists(_data.File)) {
                var idoc_ITGS = Tools.GetDataEntites<IDocNumbers_ITG>(_data.File);
                using(var db = new ATTDbContext()) {
                    
                    db.IDocNumbers_ITG.AddRange(idoc_ITGS);
                    db.SaveChanges();
                }
                File.Delete(_data.File);
            }
        }

        //[Step(Id =5, Name = "Update Schedule Info")]
        //public void UpdateSchedule() {
        //    _data.Start = _data.Start.GetNext(_data.Interval);
        //}
    }
}
