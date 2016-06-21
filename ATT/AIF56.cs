using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATT.Data.AIF;
using SAPAutomation;
using SAPFEWSELib;
using System.Data.Entity;
using ATT.Scripts;

namespace ATT
{
    class AIF56
    {
        public static void Test() {

            SAPTestHelper.Current.SetSession();

            int patchSize = 100;


            AIFDbContext db = new AIFDbContext();
            var totalIdocs = db.IDocs.Include(i=>i.Tasks.Interfaces).Where(s => s.Status == "56").ToList();

            int timers = (int)Math.Ceiling(totalIdocs.Count / double.Parse(patchSize.ToString()));

            for (int i = 0; i < timers; i++) {
                var idocs = totalIdocs.Skip(i * patchSize).Take(patchSize);
                //bd87(idocs.Select(s => s.IDocNumber).ToList(),"56");
                bd87(idocs.Select(s => s.IDocNumber).ToList(), "64");
            }


            var interfaces = totalIdocs.GroupBy(g => g.Tasks.Interfaces).Select(g => g.Key).ToList();

            foreach (var i in interfaces) {
                TrackStatus(i);
            }


        }

        private static void bd87(List<string> idocNumbers,string status) {
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("BD87");
            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_SX_DOCNU_%_APP_%-VALU_PUSH").Press();

           
            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(idocNumbers);
            SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_CREDA-LOW").Text = DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_CREDA-HIGH").Text = DateTime.Now.ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_CRETI-LOW").Text = "00:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_CRETI-HIGH").Text = "24:00:00";

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDDA-LOW").Text = DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDDA-HIGH").Text = DateTime.Now.ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDTI-LOW").Text = "00:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDTI-HIGH").Text = "24:00:00";


            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_STATU-LOW").Text = status;
            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            var tree = SAPTestHelper.Current.MainWindow.FindDescendantByProperty<GuiTree>();
            var n = tree.ChooseNode("LH7");
            var number = tree.GetItemText(n, "Column3");

            if (number != "0") {
                var n1 = tree.ChooseNode("LH7->IDoc->IDoc");
                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();
            }
        }

       

        private static void TrackStatus(Interfaces i) {
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("ZIDOCAUDREP");

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CREDAT-LOW").Text = DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CREDAT-HIGH").Text = DateTime.Now.ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CRETIM-LOW").Text = "00:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CRETIM-HIGH").Text = "24:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_STATUS-LOW").Text = "*";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("P_DIRECT").Text = "2";


            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("R_OTHERS").Select();
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MESTYP-LOW").Text = i.MsgType;

            SAPTestHelper.Current.MainWindow.FindByName<GuiCheckBox>("P_DOWNLD").Selected = true;
            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("P_FILE").Select();
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("P_CPATH").Text = $"C:\\AIF\\Temp_{i.Id}.txt";

            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("P_DOC").Select();

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_S_IDOCNO_%_APP_%-VALU_PUSH").Press();

            List<string> idocNumbers = new List<string>();
            foreach (var t in i.Tasks) {
                idocNumbers.AddRange(t.IDocs.Select(s => s.IDocNumber));
            }


            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(idocNumbers);
            SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            if (SAPTestHelper.Current.PopupWindow != null)
                SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[0]").Press();


            var itg_IDocNumbers = Tools.GetDataEntites<IDoc>($"C:\\AIF\\Temp_{i.Id}.txt");
            using (var db = new ATT.Data.AIF.AIFDbContext()) {

                foreach(var item in itg_IDocNumbers) {
                    var iDoc = item.GetIDocs();
                    var iDocInDB = db.IDocs.SingleOrDefault(s => s.IDocNumber == iDoc.IDocNumber);
                    if (iDocInDB != null)
                        db.IDocs.Remove(iDocInDB);
                    db.IDocs.Add(iDoc);
                }


                

                db.SaveChanges();
            }


           
        }
    }
}
