using SAPAutomation;
using SAPFEWSELib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data;

namespace ATT
{
    partial class Program 
    {
        static void TrackError() {
            var patchSize = 200;
            using(var db = new ATT.Data.AIF.AIFDbContext()) {
                var idocs = db.IDocs.Where(i => i.Status == "51").ToList();

                var times = idocs.Count / patchSize;
                for(int i = 0; i < times; i++) {
                    var tempIDocs = idocs.Skip(i * patchSize).Take(patchSize).ToList();
                    var idocDic = getErrors(tempIDocs);
                    var errors = ATT.Scripts.Tools.GetDataEntites<ATT.Data.AIF.Errors>(@"C:\AIF\ErrorMsg.txt", '|');
                    foreach(var error in errors) {
                        error.IDocNumber = idocDic[error.Index];
                    }
                    db.Errors.AddRange(errors);
                    db.SaveChanges();
                }

            }
           
            

        }

        static Dictionary<int, string> getErrors(List<ATT.Data.AIF.IDocs> idocs) {
            SAPTestHelper.Current.SetSession();
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("/AIF/ERR");

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MDATE-LOW").Text = "";

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MDATE-HIGH").Text = "";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MTIME-LOW").Text = "";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MTIME-HIGH").Text = "";


            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_S_GUID32_%_APP_%-VALU_PUSH").Press();
            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(idocs.Select(i => i.IDocNumber).ToList());
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

            ATT.Scripts.UIHelper.ExportFile(() => {
                SAPTestHelper.Current.SAPGuiSession.FindById<GuiGridView>("wnd[0]/usr/cntlCUSTOM/shellcont/shell/shellcont[0]/shell/shellcont[1]/shell").PressToolbarContextButton("&MB_EXPORT");
                SAPTestHelper.Current.SAPGuiSession.FindById<GuiGridView>("wnd[0]/usr/cntlCUSTOM/shellcont/shell/shellcont[0]/shell/shellcont[1]/shell").SelectContextMenuItem("&PC");

            }, @"C:\AIF\ErrorMsg.txt");

            return idocDic;
        }




    }
}
