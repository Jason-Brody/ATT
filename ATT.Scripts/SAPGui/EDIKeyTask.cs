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

namespace ATT.Scripts.SAPGui
{
    [Script("Get EDI Keys")]
    public class EDIKeyTask : ScriptBase<EDIKeyDataModel>
    {
        private EDIKeyDataModel _data;
        private EDIKeyOutput _output;
        private ATT.Data.AttDbContext db;

        public override void SetInputData(EDIKeyDataModel data) {
            this._data = data;
            _output = new EDIKeyOutput();
        }


        [Step(Id = 1, Name = "Login to SAP")]
        public void Login()
        {
            SAPLogon l = new SAPLogon();
            l.StartProcess();
            l.OpenConnection(_data.Address);
            l.Login(_data.UserName, _data.Password, _data.Client, _data.Language);
            SAPTestHelper.Current.SetSession(l);

            SAPTestHelper.Current.OnRequestError += (s, e) => { throw new Exception(e.Message); };
        }

        [Step(Id = 2, Name = "Get Message EDIKeys")]
        public void GetMessageId()
        {
            for (int i = 0; i < _data.InterfaceCount; i++)
            {
                _output.NewInterfaceId(_data.Interfaces[i].Interface);
                SAPTestHelper.Current.SAPGuiSession.StartTransaction("ZIDOCAUDREP");
                if (SAPTestHelper.Current.PopupWindow != null)
                {
                    SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiRadioButton>((r => r.Text.Contains("Continue With"))).Select();
                    SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiButton>(r => r.Text.Contains("Confirm Selection")).Press();
                }


                // Fill Control/Status Record Search
                SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CREDAT-LOW").Text = _data.Start.ToString("MM/dd/yyyy");

                DateTime toDate = _data.Start.AddHours(_data.StartTime+_data.Interval);

                if(toDate.Day != _data.Start.Day)
                { 
                    SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CREDAT-HIGH").Text = toDate.ToString("MM/dd/yyyy");
                }

                SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CRETIM-LOW").Text = _data.Start.AddHours(_data.StartTime).ToString("hh:mm:ss");
                SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CRETIM-HIGH").Text = toDate.ToString("hh:mm:ss");

                SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_SNDPRN-LOW").Text = _data.Interfaces[i].SendingPartnerNumber;
                SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("S_STAMID-LOW").Text = "*";
                SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_STATUS-LOW").Text = _data.IDocStatus;
                SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("P_DIRECT").Text = "2";


                // Fill Message Type
                SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("R_ACCDOC").Select();
                SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MESTYP-LOW").Text = "ACC_DOCUMENT";

                SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("S_MESFCT-LOW").SetFocus();
                SAPTestHelper.Current.MainWindow.SendKey(SAPKeys.F2);
                SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiGridView>().SelectedRows = "0";
                SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[0]").Press();
                SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("S_MESFCT-LOW").Text = _data.Interfaces[i].MessageFunction;

                //Fill IDoc Data Segments
                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_S_BUKRS_%_APP_%-VALU_PUSH").Press();
                var tempInterface = _data.Interfaces[i];
                SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(_data.CompanyCodes.Where(c => c.Interface == tempInterface.Interface && c.SendingPartnerNumber == tempInterface.SendingPartnerNumber).Select(c => c.Code).ToList());
                SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_S_BLART_%_APP_%-VALU_PUSH").Press();
                SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(_data.DocTypes.Where(c => c.Interface == tempInterface.Interface && c.SendingPartnerNumber == tempInterface.SendingPartnerNumber).Select(c => c.Type).ToList());
                SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

                //Fill Download Option
                SAPTestHelper.Current.MainWindow.FindByName<GuiCheckBox>("P_DOWNLD").Selected = true;
                SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("P_CPATH").Text = _output.IDocReportFile;
                //Report Output Options
                SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("P_DOC").Select();

                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

                if (SAPTestHelper.Current.PopupWindow != null)
                    SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[0]").Press();


                if(File.Exists(_output.IDocReportFile))
                {
                    var docs = Tools.GetDataEntites<ATT.Data.IDocNumbers>(_output.IDocReportFile);
                    if(docs!=null && docs.Count>0)
                    {
                        using (db = new Data.AttDbContext())
                        {
                            db.IDocNumbers.AddRange(docs);
                            db.SaveChanges();
                        }
                        File.Delete(_output.IDocReportFile);
                    }
                    SAPTestHelper.Current.SetSession();
                    SAPTestHelper.Current.SAPGuiSession.StartTransaction("SE16");
                    SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("DATABROWSE-TABLENAME").Text = "EDIDC";
                    SAPTestHelper.Current.MainWindow.SendKey(SAPKeys.Enter);
                    SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_I1_%_APP_%-VALU_PUSH").Press();
                    SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(docs.Select(d => d.IDocNumber).ToList());

                    SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();


                    SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("MAX_SEL").Text = "";
                    SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

                    string cols = "EDI Archive Key,IDoc number";
                    UIHelper.Export("wnd[0]/mbar/menu[0]/menu[10]/menu[3]/menu[2]", cols, _output.EDIKeyFile);

                    if(File.Exists(_output.EDIKeyFile))
                    {
                        var ediKeys = Tools.GetDataEntites<ATT.Data.EDIKeys>(_output.EDIKeyFile,"|");
                        if(ediKeys!=null && ediKeys.Count>0)
                        {
                            ediKeys.ForEach(k => k.CreateDt = DateTime.UtcNow);
                            using (db = new Data.AttDbContext())
                            {
                                db.EDIKeys.AddRange(ediKeys);
                                db.SaveChanges();
                            }
                            File.Delete(_output.EDIKeyFile);
                        }
                    }
                    
                }
               
               
                
            }
        }

        
    }
}
