using AIF.Data;
using SAPAutomation;
using SAPFEWSELib;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data;

namespace AIF.Scripts
{
    [Script("AIF Error Track")]
    public class AIFErrorTrack:ScriptBase<AIFErrorTrackData>
    {
        [Step(Id = 0,Name ="Clean Errors")]
        public void CleanError() {
            using(var db = new AIFDbContext()) {
                SqlParameter para = new SqlParameter("mid", _data.MissionId);
                db.Database.ExecuteSqlCommand("Delete from Errors where Mid = @mid", para);
            }
        }

        [Step(Id = 1,Name ="Login to LH7")]
        public void Login() {
            UIHelper.Login(_data.LH7);
        }

        [Step(Id =2,Name ="Track Errors")]
        public void TrackError() {
            var patchSize = 200;
            using (var db = new AIFDbContext()) {
                var idocs = db.LH7IDocs.Where(i => i.Status == "51").ToList();
                var mod = idocs.Count % patchSize;
                int times = idocs.Count / patchSize;
                if(mod != 0) {
                    times++;
                }
                
                for (int i = 0; i < times; i++) {
                    _stepReporter.Report(new ProgressInfo(i+1, times, ""));
                    var tempIDocs = idocs.Skip(i * patchSize).Take(patchSize).Select(s => s.IDocNumber).ToList();
                    var idocDic = getErrors(tempIDocs);
                    var errors = Tools.GetDataEntites<Errors>(_data.FilePath, '|');
                    foreach (var error in errors) {
                        error.IDocNumber = idocDic[error.Index];
                        error.Mid = _data.MissionId;
                    }
                    db.Errors.AddRange(errors);
                    db.SaveChanges();
                }

            }
            SAPTestHelper.Current.CloseSession();
        }

        private Dictionary<int, string> getErrors(List<string> idocs) {
            SAPTestHelper.Current.SetSession();
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("/AIF/ERR");

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MDATE-LOW").Text = "";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MDATE-HIGH").Text = "";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MTIME-LOW").Text = "";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MTIME-HIGH").Text = "";


            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_S_GUID32_%_APP_%-VALU_PUSH").Press();
            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(idocs);
            SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();
            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            var tree = SAPTestHelper.Current.MainWindow.FindDescendantByProperty<GuiTree>();
            tree.ExpandAll();



            Dictionary<int, string> idocDic = new Dictionary<int, string>();


            foreach (var key in tree.GetAllNodeKeys()) {
                int children = tree.GetNodeChildrenCount(key);
                string idoc = tree.GetItemText(key, "C          8");
                var message = tree.GetItemText(key, "&Hierarchy");

                if (children == 0 && !string.IsNullOrEmpty(idoc.Trim())) {
                    tree.SelectNode(key);
                    var index = int.Parse(message.Split(':')[0]);
                    idocDic.Add(index, idoc);

                }



            }

            SAPTestHelper.Current.SAPGuiSession.FindById<GuiToolbarControl>("wnd[0]/usr/cntlCUSTOM/shellcont/shell/shellcont[0]/shell/shellcont[0]/shell/shell" +
"cont[1]/shell[0]").PressButton("READ");

            if (SAPTestHelper.Current.PopupWindow != null) {
                SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiButton>(b => b.Text.Trim().ToLower() == "continue").Press();
            }

            UIHelper.ExportFile(() => {
                SAPTestHelper.Current.SAPGuiSession.FindById<GuiGridView>("wnd[0]/usr/cntlCUSTOM/shellcont/shell/shellcont[0]/shell/shellcont[1]/shell").PressToolbarContextButton("&MB_EXPORT");
                SAPTestHelper.Current.SAPGuiSession.FindById<GuiGridView>("wnd[0]/usr/cntlCUSTOM/shellcont/shell/shellcont[0]/shell/shellcont[1]/shell").SelectContextMenuItem("&PC");

            }, _data.FilePath);

            return idocDic;
        }


    }


    
}
