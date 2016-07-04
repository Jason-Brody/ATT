using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ATT.Data;
using System.Data.SqlClient;

namespace ATT.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public int ClientId { get; set; }

        public App() {
            using (ATTDbContext db = new ATTDbContext()) {
                SqlParameter para = new SqlParameter("@cid", SqlDbType.Int);
                para.Direction = ParameterDirection.Output;
                db.Database.ExecuteSqlCommand("exec sp_GetClient @cid output", para);
                ClientId = int.Parse(para.Value.ToString());

                SqlParameter para1 = new SqlParameter("@cid", ((App)App.Current).ClientId);
                db.Database.ExecuteSqlCommand("exec sp_UpdateMissionInterfaces @cid", para1);
            }
        }

        
    }
}
