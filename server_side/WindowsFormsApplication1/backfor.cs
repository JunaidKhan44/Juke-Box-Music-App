using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
namespace WindowsFormsApplication1
{
    class backfor
    {
    
        public int  deletefromqueue(string ly)
        {
        try
            {
            SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true");

                  if (con.State == ConnectionState.Closed) { con.Open(); }
                  string query = "update queue1 set sid=Null where lyrics='"+ly+"'";
                  SqlCommand cmd = new SqlCommand(query, con);
                  cmd.ExecuteNonQuery();
                  if (con.State == ConnectionState.Closed) { con.Open(); }
                    string query1 = "delete from queue1 where lyrics='"+ly+"'";
                    SqlCommand cmd1 = new SqlCommand(query1, con);
                    cmd1.ExecuteNonQuery();
                    con.Close();
                    
            return 0;

            }
            catch (Exception)
            {

                return 1;
            }
        
        
        }
        
        
     }


    }

