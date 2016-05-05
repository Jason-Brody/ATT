using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using SAPAutomation;
using SAPFEWSELib;
using ATT.Data.AIF;
using System.IO;
using System.Data.Entity;

namespace ATT.Scripts
{
    [Script("AIF Mass Upload")]
    public class AIFMassUpload : ScriptBase<AIFMassUploadData>
    {
        private ATT.Data.AIF.Tasks _aifTask;

        private List<string> _iDocNumbers;

        [Step(Id = 0, Name = "Get Task")]
        public void GetTask() {
            using (var db = new ATT.Data.AIF.AIFDbContext()) {
                _aifTask = db.Tasks.Include(t => t.Interfaces).Single(t => t.Id == _data.TaskId);
            }
        }

        [Step(Id = 1, Name = "Login to LH1")]
        public void LoginLH1() {
            UIHelper.Login(_data.LH1);
        }

        [Step(Id = 2, Name = "Check SAP User Configuration")]
        public void CheckUserConfig() {
            UIHelper.CheckUserConfig();
        }


        [Step(Id = 4, Name = "Download IDoc List")]
        public void DownloadIDocList() {


            fillTheForm(_data.Start, _data.End, 0);
        }

        [Step(Id = 5, Name = "Read IDoc List")]
        public void ReadIDocList() {
            if (!File.Exists(_data.GetIDocFile())) {
                using (var db = new ATT.Data.AIF.AIFDbContext()) {
                    var task = db.Tasks.Single(t => t.Id == _data.TaskId);
                    task.IsFinished = true;
                    task.DataCount = 0;
                    db.SaveChanges();
                }
                SAPTestHelper.Current.CloseSession();
                throw new BreakException("No data found");
            }

            var iDocNumbers = Tools.GetDataEntites<AIFIDocNumbers>(_data.GetIDocFile(), '|').Take(_data.DataCounts).ToList();
            _iDocNumbers = iDocNumbers.Select(i => i.IDocNumber).ToList();
            using (var db = new ATT.Data.AIF.AIFDbContext()) {
                var task = db.Tasks.Single(s => s.Id == _data.TaskId);
                foreach (var item in iDocNumbers) {
                    task.IDocNumbers.Add(item.GetIDocNumber());
                }
                db.SaveChanges();
            }
        }

        [Step(Id = 6, Name = "Download File")]
        public void DownloadFile() {
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("SA38");
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiCTextField>("wnd[0]/usr/ctxtRS38M-PROGRAMM").Text = "ZIDOC_SENDTO_FILE";
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiButton>("wnd[0]/tbar[1]/btn[8]").Press();

            SAPTestHelper.Current.SAPGuiSession.FindById<GuiRadioButton>("wnd[0]/usr/radR_FILE").Select();
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiTextField>("wnd[0]/usr/txtS_IDOC-LOW").Text = "1";
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiCTextField>("wnd[0]/usr/ctxtP_PATH").Text = _data.GetDownloadFile();
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiButton>("wnd[0]/usr/btn%_S_IDOC_%_APP_%-VALU_PUSH").Press();

            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(_iDocNumbers);
            SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();
            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            SAPTestHelper.Current.CloseSession();
        }

        [Step(Id = 7, Name = "Login to LH7")]
        public void LoginLH7() {
            UIHelper.Login(_data.LH7);
        }


        [Step(Id = 8, Name = "Upload to ITG")]
        public void Upload() {
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("SE38");
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiCTextField>("wnd[0]/usr/ctxtRS38M-PROGRAMM").Text = "ZH00RSMUPLOAD";
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiButton>("wnd[0]/tbar[1]/btn[8]").Press();
            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("P_FRNT").Select();

            if (SAPTestHelper.Current.SAPGuiSession.Info.SystemName == "LH7") {
                SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("FRNT_NAM").Text = _data.GetDownloadFile();
                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

                UIHelper.ExportFile(() => { SAPTestHelper.Current.SAPGuiSession.FindById<GuiButton>("wnd[0]/tbar[1]/btn[45]").Press(); }, _data.GetUploadedFile());
                _iDocNumbers = Tools.GetDataEntites<AIFUploadedIDoc>(_data.GetUploadedFile(), '|').Select(i => i.IDocNumber).ToList();
                //-- IDoc Number will change after the step
            }


        }

        [Step(Id = 9, Name = "Process if status equal to 64")]
        public void BD87() {
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("BD87");
            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_SX_DOCNU_%_APP_%-VALU_PUSH").Press();
            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(_iDocNumbers);
            SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDDA-LOW").Text = _data.Start.ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDDA-HIGH").Text = _data.End.ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDTI-HIGH").Text = "00:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDTI-HIGH").Text = "24:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_STATU-LOW").Text = "64";
            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            var tree = SAPTestHelper.Current.MainWindow.FindDescendantByProperty<GuiTree>();
            var n = tree.ChooseNode("LH7");
            var number = tree.GetItemText(n, "Column3");

            if (number != "0") {
                var n1 = tree.ChooseNode("LH7->IDoc->IDoc");
                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();
            }

        }

        [Step(Id = 10, Name = "Track Status")]
        public void TrackStatus() {
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("ZIDOCAUDREP");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CREDAT-LOW").Text = _data.Start.ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CREDAT-HIGH").Text = _data.End.ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CRETIM-LOW").Text = "00:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CRETIM-HIGH").Text = "24:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_STATUS-LOW").Text = "*";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("P_DIRECT").Text = "2";


            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("R_OTHERS").Select();
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MESTYP-LOW").Text = _aifTask.Interfaces.MsgType;

            SAPTestHelper.Current.MainWindow.FindByName<GuiCheckBox>("P_DOWNLD").Selected = true;
            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("P_FILE").Select();
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("P_CPATH").Text = _data.GetIDocITGFile();

            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("P_DOC").Select();

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_S_IDOCNO_%_APP_%-VALU_PUSH").Press();
            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(_iDocNumbers);
            SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            if (SAPTestHelper.Current.PopupWindow != null)
                SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[0]").Press();

            SAPTestHelper.Current.CloseSession();
        }

        [Step(Id = 11, Name = "Upload Status")]
        public void UploadStatus() {
            var itg_IDocNumbers = Tools.GetDataEntites<IDoc>(_data.GetIDocITGFile());
            using (var db = new ATT.Data.AIF.AIFDbContext()) {
                var task = db.Tasks.Single(s => s.Id == _data.TaskId);
                task.IsFinished = true;
                task.DataCount = itg_IDocNumbers.Count;
                foreach (var item in itg_IDocNumbers) {
                    task.IDocs.Add(item.GetIDocs());
                }

                db.SaveChanges();
            }
        }



        private void fillTheForm(DateTime start, DateTime end, int count) {

            SAPTestHelper.Current.SAPGuiSession.StartTransaction("we02");

            SAPTestHelper.Current.MainWindow.FindByName<GuiTab>("SOS_TAB").Select();

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("CREDAT-LOW").Text = start.ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("CREDAT-HIGH").Text = end.ToString("dd.MM.yyyy");

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("DIRECT-LOW").Text = _data.Direction;
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("STATUS-LOW").Text = _data.Status;

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("MESTYP-LOW").Text = _aifTask.Interfaces.MsgType;
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("MESCOD-LOW").Text = _aifTask.Interfaces.MsgCode;
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("MESFCT-LOW").Text = _aifTask.Interfaces.MsgFunction;
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("PPPRN-LOW").Text = _aifTask.Interfaces.PartnerNo;

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();


            var gridView = SAPTestHelper.Current.MainWindow.FindDescendantByProperty<GuiGridView>();

            if ((SAPTestHelper.Current.PopupWindow != null || gridView == null || gridView.RowCount < _data.DataCounts) && count < _data.RetryCounts) {
                count++;
                var interval = Math.Abs(_data.IntervalDays) * -1;

                fillTheForm(start.AddDays(interval), end, count);
            } else if (gridView == null) {
                return;
            } else {
                UIHelper.ExportFile(() => {
                    SAPTestHelper.Current.SAPGuiSession.FindById<GuiGridView>("wnd[0]/usr/cntlIDOCLISTE/shellcont/shell").PressToolbarContextButton("&MB_EXPORT");
                    SAPTestHelper.Current.SAPGuiSession.FindById<GuiGridView>("wnd[0]/usr/cntlIDOCLISTE/shellcont/shell").SelectContextMenuItem("&PC");
                }, _data.GetIDocFile());
            }
        }
    }
}
