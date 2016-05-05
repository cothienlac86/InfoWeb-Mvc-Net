using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace TestSQLite
{
    public partial class frmReadNews : Form
    {
        public frmReadNews()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string condition = " where host_key like '%.rongbay.com'";
            string order = " order by creation_utc desc LIMIT 200";
            string query = "Select * From cookies";
            if (cboSourceUrls.SelectedIndex == -1)
            {
                MessageBox.Show("Please select source URL...!");
                return;                
            }
            string srcUrl = cboSourceUrls.SelectedValue.ToString();            
            if (!string.IsNullOrEmpty(srcUrl))
            {
                
            }            
            //URLs:  batdongsan.com.vn, rongbay.com, vatgia.com, enbac.com, vnexpress.net =====
            SQLiteConnection conn = new SQLiteConnection
                    (@"Data Source=C:\Users\VUTHANH\AppData\Local\Google\Chrome\User Data\Default\Cookies;Pooling=true;FailIfMissing=false");
            conn.Open();
            try
            {
                SQLiteCommand cmd = new SQLiteCommand();               
                cmd.Connection = conn;
                //  cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
                //  Use the above query to get all the table names
                //cmd.CommandText = "Select * From urls where url like '%rongbay%'";
                query = !String.IsNullOrEmpty(condition) ? query += condition : query;
                query = !String.IsNullOrEmpty(order) ? query += order : query;
                cmd.CommandText = query;                
                //SQLiteDataReader dr = cmd.ExecuteReader();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("No data...!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                MessageBox.Show("Data loaded..!");
            }
        }
        
    }
}
