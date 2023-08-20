using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class SongAddition
    {
        static SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukeBox;Integrated Security=true");
        public int addsong(string v1,string v2,string v3,string v4,string v5,string v6,string v7) 
        {
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }


                string query = "insert into songs (singer,album,theme,duration,Paths,lyrics,composer) values('" +v1 + "','" + v2 + "','" + v3 + "','" + v4 + "','" + v5 + "','" + v6 + "','" + v6 + "')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }

        
        }
    }
}
