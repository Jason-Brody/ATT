using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using SAPAutomation;
using SAPFEWSELib;

namespace ATT.Scripts
{
    [Script("AIF Mass Upload")]
    public class AIFMassUpload : ScriptBase<AIFMassUploadData>
    {
        [Step(Id = 1, Name = "Login to LH1")]
        public void LoginLH1() {
            UIHelper.Login(_data.LH1);
        }

        [Step(Id = 2, Name = "Check SAP User Configuration")]
        public void CheckUserConfig() {
            UIHelper.CheckUserConfig();
        }

        [Step(Id = 3, Name = "Get Data From DB")]
        public void GetDataFromDB() {

        }

        [Step(Id = 4, Name = "Download IDoc List")]
        public void DownloadIDocList() {
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("we02");

            SAPTestHelper.Current.MainWindow.FindByName<GuiTab>("SOS_TAB").Select();

            fillTheForm(_data.Start, _data.End, 0);
        }

        [Step(Id = 5, Name = "Read IDoc List")]
        public void ReadIDocList() {

        }

        [Step(Id = 6, Name = "Download File")]
        public void DownloadFile() {

            SAPTestHelper.Current.SAPGuiSession.StartTransaction("SA38");
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiCTextField>("wnd[0]/usr/ctxtRS38M-PROGRAMM").Text = "ZIDOC_SENDTO_FILE";
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiButton>("wnd[0]/tbar[1]/btn[8]").Press();

            SAPTestHelper.Current.SAPGuiSession.FindById<GuiRadioButton>("wnd[0]/usr/radR_FILE").Select();
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiTextField>("wnd[0]/usr/txtS_IDOC-LOW").Text = "1";
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiCTextField>("wnd[0]/usr/ctxtP_PATH").Text = "[File]";
            SAPTestHelper.Current.SAPGuiSession.FindById<GuiButton>("wnd[0]/usr/btn%_S_IDOC_%_APP_%-VALU_PUSH").Press();

            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(new List<string>());
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
                SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("FRNT_NAM").Text = "[FILE Path]";
                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();
            }

            //Video 00:32
        }



        private void fillTheForm(DateTime start, DateTime end, int count) {
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("CREDAT-LOW").Text = start.ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("CREDAT-HIGH").Text = end.ToString("dd.MM.yyyy");

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("DIRECT-LOW").Text = _data.Direction;
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("STATUS-LOW").Text = _data.Status;

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("MESTYP-LOW").Text = "[Message Type]";
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("MESCOD-LOW").Text = "[Msg Code]";
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("MESFCT-LOW").Text = "[Msg.function]";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("PPPRN-LOW").Text = "[Partner No.]";

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            var gridView = SAPTestHelper.Current.MainWindow.FindDescendantByProperty<GuiGridView>();
            if (gridView.RowCount < _data.DataCounts && count < _data.IntervalCounts) {
                count++;
                var interval = Math.Abs(_data.IntervalDays) * -1;

                fillTheForm(start.AddDays(interval), end, count);
            } else {
                UIHelper.ExportFile(() => {
                    SAPTestHelper.Current.SAPGuiSession.FindById<GuiGridView>("wnd[0]/usr/cntlIDOCLISTE/shellcont/shell").SelectContextMenuItem("&PC");
                }, "[Output File]");
            }
        }
    }
}
