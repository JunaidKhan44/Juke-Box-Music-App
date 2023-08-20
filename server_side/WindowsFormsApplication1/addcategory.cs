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
    class addcategory
    {
        static SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true");
        public string addcat(string val)
        {

            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }


                if (val == "") { return null; }
                else
                {
                    string query = "insert into category values('" + val + "')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return   val;
                }
            }
            catch (Exception)
            {

                return null;
            }

        }
        //---
        public void deleteitem(string val) 
        {

            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }

               
               // if (var == "") { MessageBox.Show("please fill the text field then press add button"); }
                    string query = "delete from category where category_of_song='" + val + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
               
            }
            catch (Exception)
            {

                throw;
            }
        
        }



    }
}
