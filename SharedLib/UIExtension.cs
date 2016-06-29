using SAPAutomation;
using SAPFEWSELib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib
{
    public static class UIExtension
    {
        public static void ChangeLayout(this SAPLogon SAP,HashSet<string> columns)
        {
            SAP.MainWindow.FindByName<GuiButton>("btn[32]").Press();
            var displayedColumnsGrid = SAP.PopupWindow.FindById<GuiGridView>("usr/tabsG_TS_ALV/tabpALV_M_R1/ssubSUB_DYN0510:SAPLSKBH:0620/cntlCONTAINER2_LAYO/shellcont/shell");
            if (displayedColumnsGrid.RowCount > 0)
            {
                displayedColumnsGrid.SelectAll();
                SAP.PopupWindow.FindByName<GuiButton>("APP_FL_SING").Press();
            }
            var columnSetGrid = SAP.PopupWindow.FindById<GuiGridView>("usr/tabsG_TS_ALV/tabpALV_M_R1/ssubSUB_DYN0510:SAPLSKBH:0620/cntlCONTAINER1_LAYO/shellcont/shell");

            string selectedRow = "";

            for (int c = 0; c < columnSetGrid.RowCount; c++)
            {
                var col = columnSetGrid.GetCellValue(c, "SELTEXT");

                if (columns.Contains(col))
                {
                    selectedRow += c.ToString() + ",";
                }

            }

            columnSetGrid.SelectedRows = selectedRow;
            SAP.PopupWindow.FindByName<GuiButton>("APP_WL_SING").Press();

            SAP.PopupWindow.FindByName<GuiButton>("btn[0]").Press();
        }
        public static void ChangeLayout(this SAPLogon SAP, Dictionary<string, int> columns)
        {
            SAP.MainWindow.FindByName<GuiButton>("btn[32]").Press();
            var displayedColumnsGrid = SAP.PopupWindow.FindById<GuiGridView>("usr/tabsG_TS_ALV/tabpALV_M_R1/ssubSUB_DYN0510:SAPLSKBH:0620/cntlCONTAINER2_LAYO/shellcont/shell");
            if (displayedColumnsGrid.RowCount > 0)
            {
                displayedColumnsGrid.SelectAll();
                SAP.PopupWindow.FindByName<GuiButton>("APP_FL_SING").Press();
            }
            var columnSetGrid = SAP.PopupWindow.FindById<GuiGridView>("usr/tabsG_TS_ALV/tabpALV_M_R1/ssubSUB_DYN0510:SAPLSKBH:0620/cntlCONTAINER1_LAYO/shellcont/shell");

            string selectedRow = "";

            for (int c = 0; c < columnSetGrid.RowCount; c++)
            {
                var col = columnSetGrid.GetCellValue(c, "SELTEXT");

                if (columns.ContainsKey(col))
                {
                    selectedRow += c.ToString() + ",";
                    columns[col] = c;
                }
            }

            columnSetGrid.SelectedRows = selectedRow;
            SAP.PopupWindow.FindByName<GuiButton>("APP_WL_SING").Press();

            SAP.PopupWindow.FindByName<GuiButton>("btn[0]").Press();
        }

        public static void ExportFile(this SAPLogon SAP, string outputmenuId, string dir, string fileName)
        {
            SAP.MainWindow.FindById<GuiMenu>(outputmenuId).Select();
            SAP.PopupWindow.FindByName<GuiRadioButton>("SPOPLI-SELFLAG").Select();
            SAP.PopupWindow.FindByName<GuiButton>("btn[0]").Press();

            var filePath = Path.Combine(dir, fileName);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (File.Exists(filePath))
                File.Delete(filePath);

            SAP.PopupWindow.FindByName<GuiCTextField>("DY_PATH").Text = dir;
            SAP.PopupWindow.FindByName<GuiCTextField>("DY_FILENAME").Text = fileName;

            var windowName = SAP.MainWindow.Text;
            var args = "\"" + windowName + "\"";

            var ts = new CancellationTokenSource();
            var ct = ts.Token;

            Task.Run(() => { Utils.SetAccess(windowName, ct); });
            SAP.PopupWindow.FindByName<GuiButton>("btn[0]").Press();
            ts.Cancel();

        }

        public static void ExportFile(this SAPLogon SAP, Action openExportMenu,string fileName) {
            openExportMenu();
            SAP.PopupWindow.FindByName<GuiRadioButton>("SPOPLI-SELFLAG").Select();
            SAP.PopupWindow.FindByName<GuiButton>("btn[0]").Press();

            FileInfo f = new FileInfo(fileName);
            if (!f.Directory.Exists)
                f.Directory.Create();
            if (f.Exists)
                f.Delete();



            SAP.PopupWindow.FindByName<GuiCTextField>("DY_PATH").Text = f.DirectoryName;
            SAP.PopupWindow.FindByName<GuiCTextField>("DY_FILENAME").Text = f.Name;

            var windowName = SAP.MainWindow.Text;
            var args = "\"" + windowName + "\"";

            var ts = new CancellationTokenSource();
            var ct = ts.Token;

            Task.Run(() => { Utils.SetAccess(windowName, ct); });
            SAP.PopupWindow.FindByName<GuiButton>("btn[0]").Press();
            ts.Cancel();
        }

        public static void ExportFile(this SAPLogon SAP, string outputmenuId, string fileName)
        {
            SAP.ExportFile(() => {
                SAP.MainWindow.FindById<GuiMenu>(outputmenuId).Select();
            }, fileName);
            //SAP.MainWindow.FindById<GuiMenu>(outputmenuId).Select();
            //SAP.PopupWindow.FindByName<GuiRadioButton>("SPOPLI-SELFLAG").Select();
            //SAP.PopupWindow.FindByName<GuiButton>("btn[0]").Press();

            //FileInfo f = new FileInfo(fileName);
            //if (!f.Directory.Exists)
            //    f.Directory.Create();
            //if (f.Exists)
            //    f.Delete();



            //SAP.PopupWindow.FindByName<GuiCTextField>("DY_PATH").Text = f.DirectoryName;
            //SAP.PopupWindow.FindByName<GuiCTextField>("DY_FILENAME").Text = f.Name;

            //var windowName = SAP.MainWindow.Text;
            //var args = "\"" + windowName + "\"";

            //var ts = new CancellationTokenSource();
            //var ct = ts.Token;

            //Task.Run(() => { Utils.SetAccess(windowName, ct); });
            //SAP.PopupWindow.FindByName<GuiButton>("btn[0]").Press();
            //ts.Cancel();

        }

        public static bool Export(this SAPLogon SAP, string outputMenuId, string columnsDivideByComma, string fileName)
        {
            var grid = SAP.MainWindow.FindDescendantByProperty<GuiGridView>();
            if (grid.RowCount > 0)
            {
                Dictionary<string, int> columns = new Dictionary<string, int>();
                columnsDivideByComma.Split(',').ToList().ForEach(s => columns.Add(s, -1));
                SAP.ChangeLayout(columns);
                SAP.ExportFile("wnd[0]/mbar/menu[0]/menu[10]/menu[3]/menu[2]", fileName);
                return true;
            }
            return false;
        }

        public static void SAPAccessVerification(this SAPLogon SAP, string TCode, Func<Tuple<bool, string>> otherVerification = null)
        {
            SAP.Session.StartTransaction(TCode);
            if (SAP.Session.Info.Transaction != TCode)
                throw new Exception($"Can't access to TCode:{TCode}");

            if (otherVerification != null)
            {
                var result = otherVerification();
                if (!result.Item1)
                    throw new Exception(result.Item2);
            }
        }

        public static void SE16TableAccessVerification(this SAPLogon SAP, string tableName)
        {
            SAP.SAPAccessVerification("SE16", () =>
            {
                var page = SAP.Session.Info.ScreenNumber;
                SAP.MainWindow.FindByName<GuiCTextField>("DATABROWSE-TABLENAME").Text = tableName;
                SAP.MainWindow.SendKey(SAPKeys.Enter);
                var page1 = SAP.Session.Info.ScreenNumber;
                if (page == page1)
                {
                    return new Tuple<bool, string>(false, $"Don't have access to TCODE:SE16 ,table:{tableName}");
                }
                else
                {
                    return new Tuple<bool, string>(true, "");
                }
            });
        }


        public static SAPLogon Login(SAPLoginData data)
        {
            SAPLogon l = new SAPLogon();
            l.StartProcess();
            l.OpenConnection(data.Address);
            l.Login(data.UserName, data.Password, data.Client, data.Language);
            return l;

            //SAP.OnRequestError += (s, e) => { throw new Exception(e.Message); };
        }

        public static void GoToSE16Table(this SAPLogon SAP, string tableName)
        {
            SAP.Session.StartTransaction("SE16");
            SAP.MainWindow.FindByName<GuiCTextField>("DATABROWSE-TABLENAME").Text = tableName;
            SAP.MainWindow.SendKey(SAPKeys.Enter);
        }

        public static void CheckUserConfig(this SAPLogon SAP)
        {
            SAP.Session.StartTransaction("su3");
            SAP.MainWindow.FindByName<GuiTab>("DEFA").Select();

            var decimalNotation = SAP.MainWindow.FindByName<GuiComboBox>("SUID_ST_NODE_DEFAULTS-DCPFM");

            bool isChange = false;

            if (decimalNotation.Value != "1,234,567.89")
            {
                decimalNotation.Value = "1,234,567.89";
                isChange = true;
            }

            var dateFormat = SAP.MainWindow.FindByName<GuiComboBox>("SUID_ST_NODE_DEFAULTS-DATFM");
            if (dateFormat.Value != "DD.MM.YYYY")
            {
                dateFormat.Value = "DD.MM.YYYY";
                isChange = true;
            }

            if (isChange)
                SAP.MainWindow.SendKey(SAPKeys.Ctrl_S);
            else
                SAP.MainWindow.SendKey(SAPKeys.F3);
        }
    }
}
