
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
using System.Data.Entity;
using SharedLib;
using ATT.Data;

namespace ATT.Scripts
{
    [Script("Get EDI Keys")]
    public class MSGIDTask : ScriptBase<MSGIDTaskData>
    {
        private SAPLogon sap;
        private ATTDbContext db;

        //private SAPInterfaces _interface;

        public override void Initial(MSGIDTaskData data, IProgress<ProgressInfo> StepReporter) {
            base.Initial(data, StepReporter);
            //using (db = new ATTDbContext()) {
            //    _interface = db.Tasks.Include(s => s.SAPInterfaces).Single(t => t.Id == _data.TaskId).SAPInterfaces;
            //}
        }

     

        [Step(Id = 0, Name = "Login to LH1")]
        public void Login() {
            sap = UIExtension.Login(_data.LH1);
            
        }
        
        [Step(Id =1,Name ="Check User Config")]
        public void UserConfigCheck() {
            sap.CheckUserConfig();
        }

        [Step(Id = 2, Name = "Get Message Ids")]
        public void GetMessageId() {

            _data.NewGuid(_data.SAPInterface.Name);
            sap.Session.StartTransaction("ZIDOCAUDREP");
            if (sap.PopupWindow != null) {
                sap.PopupWindow.FindDescendantByProperty<GuiRadioButton>((r => r.Text.Contains("Continue With"))).Select();
                sap.PopupWindow.FindDescendantByProperty<GuiButton>(r => r.Text.Contains("Confirm Selection")).Press();
            }


            // Fill Control/Status Record Search
            sap.MainWindow.FindByName<GuiCTextField>("S_CREDAT-LOW").Text = _data.Start.ToString("dd.MM.yyyy");
            //DateTime toDate = _data.Start.AddHours(_data.StartTime + _data.Interval);
            sap.MainWindow.FindByName<GuiCTextField>("S_CREDAT-HIGH").Text = _data.GetEnd().ToString("dd.MM.yyyy");


            sap.MainWindow.FindByName<GuiCTextField>("S_CRETIM-LOW").Text = _data.Start.ToString("HH:mm:ss");
            sap.MainWindow.FindByName<GuiCTextField>("S_CRETIM-HIGH").Text = _data.End;

            sap.MainWindow.FindByName<GuiCTextField>("S_SNDPRN-LOW").Text = _data.SAPInterface.PartnerNumber;
            sap.MainWindow.FindByName<GuiTextField>("S_STAMID-LOW").Text = "*";
            sap.MainWindow.FindByName<GuiCTextField>("S_STATUS-LOW").Text = _data.IDocStatus;
            sap.MainWindow.FindByName<GuiCTextField>("P_DIRECT").Text = "2";


            // Fill Message Type
            sap.MainWindow.FindByName<GuiRadioButton>("R_ACCDOC").Select();
            sap.MainWindow.FindByName<GuiCTextField>("S_MESTYP-LOW").Text = "ACC_DOCUMENT";
            
            sap.MainWindow.FindByName<GuiTextField>("S_MESFCT-LOW").SetFocus();
            sap.MainWindow.SendKey(SAPKeys.F2);
            sap.PopupWindow.FindDescendantByProperty<GuiGridView>().SelectedRows = "0";
            sap.PopupWindow.FindByName<GuiButton>("btn[0]").Press();
            sap.MainWindow.FindByName<GuiTextField>("S_MESFCT-LOW").Text = _data.SAPInterface.MsgFunction;

            //Fill IDoc Data Segments
            sap.MainWindow.FindByName<GuiButton>("%_S_BUKRS_%_APP_%-VALU_PUSH").Press();
            sap.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(_data.SAPInterface.SAPCompanyCodes.Select(c => c.Name).ToList());
            sap.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

            sap.MainWindow.FindByName<GuiButton>("%_S_BLART_%_APP_%-VALU_PUSH").Press();
            sap.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(_data.SAPInterface.SAPDocTypes.Select(c => c.Name).ToList());
            sap.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

            //Fill Download Option
            sap.MainWindow.FindByName<GuiCheckBox>("P_DOWNLD").Selected = true;

            sap.MainWindow.FindByName<GuiTextField>("P_CPATH").Text = _data.IDocReportFile;

            //Report Output Options
            sap.MainWindow.FindByName<GuiRadioButton>("P_DOC").Select();

            sap.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            if (sap.PopupWindow != null)
                sap.PopupWindow.FindByName<GuiButton>("btn[0]").Press();


            if (File.Exists(_data.IDocReportFile)) {
                var docs = Tools.GetDataEntites<IDoc>(_data.IDocReportFile);
                if (docs != null && docs.Count > 0) {
                    using (db = new ATTDbContext()) {
                        docs.ForEach(i => db.IDocNumbers.Add(i.GetATTIDoc()));
                        db.SaveChanges();
                    }
                    File.Delete(_data.IDocReportFile);
                }

                sap.Session.StartTransaction("SE16");
                sap.MainWindow.FindByName<GuiCTextField>("DATABROWSE-TABLENAME").Text = "EDIDC";
                sap.MainWindow.SendKey(SAPKeys.Enter);
                sap.MainWindow.FindByName<GuiButton>("%_I1_%_APP_%-VALU_PUSH").Press();
                sap.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(docs.Select(d => d.IDocNumber).ToList());
                
                sap.PopupWindow.FindByName<GuiButton>("btn[8]").Press();


                sap.MainWindow.FindByName<GuiTextField>("MAX_SEL").Text = "";
                sap.MainWindow.FindByName<GuiButton>("btn[8]").Press();

                string cols = "EDI Archive Key,IDoc number";
                sap.Export("wnd[0]/mbar/menu[0]/menu[10]/menu[3]/menu[2]", cols, _data.EDIKeyFile);


                if (File.Exists(_data.EDIKeyFile)) {
                    var msgIds = Tools.GetDataEntites<ATTMsg>(_data.EDIKeyFile, '|');
                    if (msgIds != null && msgIds.Count > 0) {
                        using (db = new ATTDbContext()) {
                            msgIds.ForEach(a => {
                                var attmsg = a.GetATTMsg();
                                attmsg.CreateDt = DateTime.UtcNow;
                                attmsg.InterfaceId = _data.SAPInterface.Id;
                                attmsg.Mid = _data.Mid;
                                db.MsgIDs.Add(attmsg);
                            });
                            db.SaveChanges();
                        }
                        File.Delete(_data.EDIKeyFile);
                    }
                }

            }

            sap.CloseSession();
        }

       
    }


   
}
