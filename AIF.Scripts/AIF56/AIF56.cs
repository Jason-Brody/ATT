using ScriptRunner.Interface.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRunner.Interface;
using SharedLib;
using AIF.Data;
using System.Data.Entity;
using SAPAutomation;
using SAPFEWSELib;

namespace AIF.Scripts
{
    [Script("Reprocess idoc which status is 56")]
    public class AIF56:ScriptBase<AIF56Data>
    {
        private List<LH7IDocs> totalIdocs;
        private AIFDbContext db;

        [Step(Id =0,Name ="Get Data which status is 56")]
        public void DataCheck() {
            db = new AIFDbContext();
            totalIdocs = db.LH7IDocs.Include(i => i.Tasks.Interfaces).Where(s => s.Status == "56").ToList();
            if (totalIdocs.Count == 0)
                Return("No 56 Data Found");
        }


        [Step(Id =1,Name ="Login to LH7")]
        public void Login() {
            UIHelper.Login(_data.LH7);
        }

        [Step(Id =2,Name ="Reprocess BD87 Program")]
        public void BD87() {
            int patchSize = 100;


            

            int timers = (int)Math.Ceiling(totalIdocs.Count / double.Parse(patchSize.ToString()));

            for (int i = 0; i < timers; i++) {

                _stepReporter.Report(new ProgressInfo(i + 1, timers, ""));

                var idocs = totalIdocs.Skip(i * patchSize).Take(patchSize);

                var idocList = idocs.Select(s => s.IDocNumber).ToList();
                AIFMassUpload.BD87(idocList, "56");
                AIFMassUpload.BD87(idocList, "64");

               
            }


            var interfaces = totalIdocs.GroupBy(g => g.Tasks.Interfaces).Select(g => g.Key).ToList();

            foreach (var i in interfaces) {
                TrackStatus(i);
            }
        }

        private  void TrackStatus(Interfaces i) {
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
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("P_CPATH").Text = _data.GetAIF56File(i.Id);

            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("P_DOC").Select();

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_S_IDOCNO_%_APP_%-VALU_PUSH").Press();

            List<string> idocNumbers = new List<string>();

            var myTasks = i.Tasks.Where(m => m.Mid == _data.MissionId).ToList();

            foreach (var t in myTasks) {
                idocNumbers.AddRange(t.LH7IDocs.Select(s => s.IDocNumber));
            }


            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(idocNumbers);
            SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            if (SAPTestHelper.Current.PopupWindow != null)
                SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[0]").Press();


            var itg_IDocNumbers = Tools.GetDataEntites<IDoc>(_data.GetAIF56File(i.Id));
            using (var db = new AIFDbContext()) {

                foreach (var item in itg_IDocNumbers) {
                    var iDoc = item.GetIDocs();
                    var iDocInDB = db.LH7IDocs.SingleOrDefault(s => s.IDocNumber == iDoc.IDocNumber);
                    if (iDocInDB != null)
                        db.LH7IDocs.Remove(iDocInDB);
                    db.LH7IDocs.Add(iDoc);
                }
                db.SaveChanges();
            }



        }
    }
}
