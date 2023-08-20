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
using System.Drawing.Drawing2D;
using System.Data.SqlClient;


namespace WindowsFormsApplication1
{
    class ThemeClass
    {
        SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true");

        public string settheme(string p)
        {
            try
            {
                string vv="selected";
                string selectedtheme = p;
                string l="";
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                String qv = "select * from theme where theme_of_day='" + selectedtheme + "'";
                SqlCommand sqlCmd = new SqlCommand(qv, con);
                SqlDataReader sdr = sqlCmd.ExecuteReader();
                while (sdr.Read())
                {
                l=sdr[1].ToString();
                }
                sdr.Close();
                string v1 = "Update theme set  status='"+vv+"' where theme_of_day='" +selectedtheme + "'";
                SqlCommand cmd1 = new SqlCommand(v1, con);
                cmd1.ExecuteNonQuery();
                con.Close();
            
                return l;
            }
            catch (Exception)
            {
                return "";
            }

        }
        
        public void queuedeleteall()
        {
            try
            {


                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                //String qv = "delete from queue1";
                //SqlCommand sqlCmd = new SqlCommand(qv, con);
                //sqlCmd.ExecuteNonQuery();
                //con.Close();
                
                string v1 = "Update load1 set  Qid=Null ";
                SqlCommand cmd1 = new SqlCommand(v1, con);
                cmd1.ExecuteNonQuery();
                con.Close();

                con.Open();
                String qv2 = "delete from load1";
                SqlCommand sqlCmd2 = new SqlCommand(qv2, con);
                sqlCmd2.ExecuteNonQuery();
                con.Close();


                con.Open();
                String qv = "delete from queue1";
                SqlCommand sqlCmd = new SqlCommand(qv, con);
                sqlCmd.ExecuteNonQuery();
                con.Close();

                
            }
            catch (Exception o)
            {
                //throw;
                MessageBox.Show(o.ToString());
            }
        }
        //----
        int id;
        public void setthemeauto()
        {
            try
            {
                string vv = "selected";
               
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                String qv = "select * from autotheme where status='" +vv + "'";
                SqlCommand sqlCmd = new SqlCommand(qv, con);
                SqlDataReader sdr = sqlCmd.ExecuteReader();
                while (sdr.Read())
                {
                    id = (int)sdr[1];
                }
                sdr.Close();
                string v1 = "Update autotheme set  status=NULL where tid='" + id + "'";
                SqlCommand cmd1 = new SqlCommand(v1, con);
                cmd1.ExecuteNonQuery();
                con.Close();

              
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            }

        }
        // dedication empty logic
        public void deletealldedication()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                String query1 = "delete from dedication_private";
                SqlCommand sqlCmd1 = new SqlCommand(query1, con);
                sqlCmd1.ExecuteNonQuery();
                con.Close();

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                String query2 = "delete from dedication_public";
                SqlCommand sqlCmd2 = new SqlCommand(query2, con);
                sqlCmd2.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception)
            {
                throw;
            }
        }
     
        //function
      public  void loadtoload() 
        {
            try
            {
                string cc = "Data Source=(local);initial Catalog = jukebox2;Integrated Security=true";
                using (SqlConnection con1 = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true"))
                {
                    con1.Open();
                    SqlCommand cmd = new SqlCommand("select *  from queue1", con1);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        using (SqlConnection des = new SqlConnection(cc))
                        {
                            using (SqlBulkCopy bc = new SqlBulkCopy(cc))
                            {
                                bc.DestinationTableName = "load1";
                                bc.ColumnMappings.Add("sid", "sid");
                                bc.ColumnMappings.Add("Qid", "Qid");   
                                bc.WriteToServer(rd);

                            }
                        }
                    }

                }

            }
            catch (Exception)
            { throw; }
        
        }
        //-----end of class
    }
}
