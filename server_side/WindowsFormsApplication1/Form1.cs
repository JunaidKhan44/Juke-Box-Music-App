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
    public partial class Form1 : Form
    {
     public static int increment;
        static SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true"); 
        [DllImport("Gdi32.dll",EntryPoint="CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,int nTopRect,int nRightRect,int nBottomRect,int nWidthEllipse,int nHeightEllipse);
       
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0,0,Width,Height,20,20));
            show1();
            show2();
        
        }
        string vv = "selected";
        public static string selectedtheme { get; set; }
        
        DateTime date = DateTime.Now;
        public void checkfuntion() 
        {
            try
            {

                SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true");
                con.Open();
                SqlDataAdapter aa = new SqlDataAdapter("autothemeselection", con);
                aa.SelectCommand.CommandType = CommandType.StoredProcedure;
                aa.SelectCommand.Parameters.Add("@selection", SqlDbType.Char, 20).Value = "selected";
                aa.SelectCommand.ExecuteNonQuery();
                con.Close();
            
            }
            catch (Exception)
            {
                
                throw;
            }

            //auto theme logic

            try
            {
                //--
                if (con.State == ConnectionState.Closed) { con.Open(); }
                String qv = "select *  from autotheme where Day='" + date.DayOfWeek + "'";
                SqlCommand sqlCmd = new SqlCommand(qv, con);
                SqlDataReader sdr2 = sqlCmd.ExecuteReader();
                while (sdr2.Read())
                {
                    label3.Text = sdr2[0].ToString();
                    selectedtheme = sdr2[0].ToString();
                    comboBox1.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    checkBox1.Enabled = false;   //this line
                    break;

                }
                sdr2.Close();
                bold=selectedtheme;
                string v1 = "Update autotheme set  status='" + "selected" + "' where Day='" + date.DayOfWeek + "'";
                SqlCommand cmd1 = new SqlCommand(v1, con);
                cmd1.ExecuteNonQuery();
                con.Close();

                try
                {
                    string cc = "Data Source=(local);initial Catalog = jukebox2;Integrated Security=true";
                    using (SqlConnection con1 = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true"))
                    {
                        con1.Open();
                        SqlCommand cmd = new SqlCommand("select *  from song where theme='" + Form1.selectedtheme + "'", con1);
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            using (SqlConnection des = new SqlConnection(cc))
                            {
                                using (SqlBulkCopy bc = new SqlBulkCopy(cc))
                                {
                                    bc.DestinationTableName = "queue1";
                                    bc.ColumnMappings.Add("singer", "singer");
                                    bc.ColumnMappings.Add("album", "album");
                                    bc.ColumnMappings.Add("theme", "theme");
                                    bc.ColumnMappings.Add("duration", "duration");
                                    bc.ColumnMappings.Add("Paths", "Paths");
                                    bc.ColumnMappings.Add("lyrics", "lyrics");
                                    bc.ColumnMappings.Add("composer", "composer");
                                    bc.ColumnMappings.Add("category", "category");
                                    bc.ColumnMappings.Add("sid", "sid");
                                    bc.ColumnMappings.Add("duration", "duration2");  //for duration do this for auto theme
                                    bc.WriteToServer(rd);

                                }
                            }
                        }

                    }

                }
                catch (Exception)
                {

                    throw;
                }
                ThemeClass obj = new ThemeClass();
                obj.loadtoload();
                //---for manual theme set all to null when auto selected
                int vivo = 0;
                string version23 = "selected";
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    String myversion23 = "select *  from theme where status='" +version23 + "'";
                    SqlCommand sqlCmdversion23 = new SqlCommand(myversion23, con);
                    SqlDataReader sdrversion23 = sqlCmdversion23.ExecuteReader();
                    while (sdrversion23.Read())
                    {
                       vivo  = (int)(sdrversion23[0]);
                    }
                    sdrversion23.Close();
                    string v1version23 = "Update theme set  status=Null where theme_id='" + vivo + "'";
                    SqlCommand cmd1version23 = new SqlCommand(v1version23, con);
                    cmd1version23.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            { MessageBox.Show(e + ""); }
            
        }
        public string bold;
        private void Form1_Load(object sender, EventArgs e)
        {
            //---
            if (con.State == ConnectionState.Closed) { con.Open(); }
            String append = "select *  from theme";
            String read;
            SqlCommand appendCmd = new SqlCommand(append, con);
            SqlDataReader appendsdr = appendCmd.ExecuteReader();
            while (appendsdr.Read())
            {
                read = (String)(appendsdr[1]);
                comboBox1.Items.Add(read);
            }
            appendsdr.Close();
            con.Close();
            
            //---
            if (increment > 0) { label3.Text = selectedtheme.ToString(); }
            else 
            {
                ThemeClass obj = new ThemeClass();
                obj.queuedeleteall();
                obj.deletealldedication();
            }
            if (jol > 0) { label3.Text = selectedtheme.ToString();
           
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
           //--remove theme manual
           try
            {
                string val = comboBox1.SelectedItem.ToString();
                ThemeClass obj = new ThemeClass();
               
                string msg = "Are you sure do you want to remove theme...!";
                MessageBoxButtons button = MessageBoxButtons.YesNo;
                string cap = "About Remove";
                DialogResult res = MessageBox.Show(msg, cap, button, MessageBoxIcon.Warning);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    comboBox1.Items.Remove(val);
                    label3.Text = "";
                    obj.queuedeleteall();
                    player.prp.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("To remove Theme \n\tPlease select  theme from DropDown", "About Theme Remove", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        //--
        int id,idversion;
        
        private void button4_Click(object sender, EventArgs e)
        {
            
              checkBox1.Enabled = false;
            //--set  theme manual
            try 
            {
                string v = "selected"; 
                if (con.State == ConnectionState.Closed) { con.Open(); 
                 String myq = "select *  from theme where status='"+v+"'";
                SqlCommand sqlCmd = new SqlCommand(myq, con);
                SqlDataReader sdr = sqlCmd.ExecuteReader();
                while (sdr.Read())
                {
                    id = (int)(sdr[0]);
                }
                sdr.Close();
                string v1 = "Update theme set  status=Null where theme_id='" + id + "'";
                SqlCommand cmd1 = new SqlCommand(v1, con);
                cmd1.ExecuteNonQuery();
                con.Close();
           }
            }catch(Exception)
            {
               MessageBox.Show("error updating selected");
            }
            try
            {
            string  val= comboBox1.SelectedItem.ToString();
           ThemeClass obj = new ThemeClass();
            string val2=obj.settheme(val);
           label3.Text = val2.ToString();
           selectedtheme = val;
           increment++;
            try
           {
               string cc = "Data Source=(local);initial Catalog = jukebox2;Integrated Security=true";
               using (SqlConnection con1 = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true")) 
               {
                   con1.Open();
                   SqlCommand cmd = new SqlCommand("select *  from song where theme='" + val + "'",con1);
                   using (SqlDataReader rd = cmd.ExecuteReader())
                   {
                       using (SqlConnection des = new SqlConnection(cc))
                       {
                           using (SqlBulkCopy bc = new SqlBulkCopy(cc))
                           {
                               bc.DestinationTableName = "queue1";
                               bc.ColumnMappings.Add("singer", "singer");
                               bc.ColumnMappings.Add("album", "album");
                               bc.ColumnMappings.Add("theme", "theme");
                               bc.ColumnMappings.Add("duration", "duration");
                               bc.ColumnMappings.Add("paths", "Paths");
                               bc.ColumnMappings.Add("lyrics", "lyrics");
                               bc.ColumnMappings.Add("composer", "composer");
                               bc.ColumnMappings.Add("category", "category");
                               bc.ColumnMappings.Add("sid", "sid");
                               bc.ColumnMappings.Add("duration", "duration2");   //for duration1 for play after  do this manual theme //is column ko duration2 se change karna hai
                               
                               
                               bc.WriteToServer(rd);

                           }
                       }
                   }
               
               }

           }
           catch (Exception)
           {  throw;}

            obj.loadtoload();

           
        }
        catch(Exception ee)
        {
            MessageBox.Show(ee.ToString());
           // MessageBox.Show("Please Select Theme of Day from Drop Down List!!!\nNow you can also use Auto Theme to select automatically!", "About Theme", MessageBoxButtons.OK, MessageBoxIcon.Error);
         //   comboBox1.Focus();
        }
            //auto theme set all to null 9-july-2020
                string version = "selected";
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    String myversion = "select *  from autotheme where status='" + version + "'";
                    SqlCommand sqlCmdversion = new SqlCommand(myversion, con);
                    SqlDataReader sdrversion = sqlCmdversion.ExecuteReader();
                    while (sdrversion.Read())
                    {
                        idversion = (int)(sdrversion[1]);
                    }
                    sdrversion.Close();
                    string v1version = "Update autotheme set  status=Null where tid='" + idversion + "'";
                    SqlCommand cmd1version = new SqlCommand(v1version, con);
                    cmd1version.ExecuteNonQuery();
                    con.Close();
                }
        }
        //-------------------------------------------------------------------------------------      
        int jol;
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkfuntion();
                jol++;   
            }
            else 
            {
                comboBox1.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
            }
        }
        //---------------------------------------------------------------------------------------------------------------
        //design code and naviagtiononly 
        private void addSongsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Hide();
            Addsongs obj1 = new Addsongs();
            obj1.Show();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Hide();
            Category obj2 = new Category();
            obj2.Show();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Addsongs obj = new Addsongs();
            obj.Show();
        }
        private void songListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedtheme != null)
            {
                //    this.Hide();
                SongList obj = new SongList();
                obj.Show();
            }
            else
            {
                MessageBox.Show("Please select theme first...........!", "About Navg.", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void queueListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedtheme != null)
            {
                //    this.Hide();
                player obj = new player();
                obj.Show();
            }
            else
            {

                MessageBox.Show("Please select theme first...........!", "About Navg.", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        public void show1()
        {
            Elispsecontrol nn = new Elispsecontrol();
            nn.TargetControl = button4;
            nn.CornerRadius = 20;
            this.Controls.Add(button4);
        }

        public void show2()
        {
            Elispsecontrol nn = new Elispsecontrol();
            nn.TargetControl = button5;
            nn.CornerRadius = 20;
            this.Controls.Add(button5);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Addsongs obj = new Addsongs();
            this.Hide();
            obj.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void tokenModificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TokenModification obj = new TokenModification();
            obj.Show();
        }
       



    }  //end of class
}
