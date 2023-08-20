using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Threading;


namespace WindowsFormsApplication1
{
 
    public partial class player : Form
    {

        static int uoop = 0;
       int variable;
       static  List<String> jlist = new List<string>();
       static int counter;
       static int changer=0;
       static int changer2 = 0;
       static string firstitem;
       public string winform;
       List<String> playcounter = new List<String>();    //play counter
       List<String> Songdurat = new List<String>();
       List<String> mylist = new List<String>();
       List<int> listofid = new List<int>();   //---> for dedication
       List<String> listbox2 = new List<String>();
       List<string> lst = new List<string>();        
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
         int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        static SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true");
        public player()
        {
            InitializeComponent();
            menuStrip1.Items[1].Enabled = false; // this works
            menuStrip1.Items[2].Enabled = false; // this works
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            show1();
            show2();
            counter = 0;
            variable = 0; //for check play button
        }

        public static player prp { get; set; }
        static int i = 0;
        static string durationofeach = "";
        static WMPLib.IWMPPlaylist Playlist;
        int ft = 0;
  
        private void player_Load(object sender, EventArgs e)  //--------------------------------form on load
        {
            prp = this;
            button7.Enabled = false;
            winform = "player";
            try
            {
                string cal = Form1.selectedtheme.ToString();
                if (con.State == ConnectionState.Closed) { con.Open(); }
                String qv = "select * from queue1 order by token desc";
                SqlCommand sqlCmd = new SqlCommand(qv, con);
                SqlDataReader sdr = sqlCmd.ExecuteReader();

                while (sdr.Read())
                {
                    listBox1.Items.Add(sdr[6].ToString());
                    listbox2.Add(sdr[5].ToString());  //path 
                    lst.Add(sdr[5].ToString());       //path
                    mylist.Add(sdr[6].ToString());   //---song name propabley
                    listofid.Add((int)sdr[0]);//-->
                    playcounter.Add(sdr[12].ToString());
                    if (ft == 0)
                    {
                        firstitem = sdr[11].ToString();
                        ft++;
                    }
                }
                sdr.Close();
                con.Close();
                //insert status
                try
                {
                    int qid = listofid.ElementAt(0);
                    con.Open();
                    string query0 = "update queue1 set extra2='" + "Current" + "' where Qid='" + qid + "'";
                    SqlCommand cmd0 = new SqlCommand(query0, con);
                    cmd0.ExecuteNonQuery();
                    con.Close();

                    /////////////////

                    String var = playcounter.ElementAt(0);
                    int variable = int.Parse(var.Trim()) + 1;
                    con.Open();
                    string query1 = "update queue1 set extra1='" + variable.ToString() + "' where Qid='" + qid + "'";
                    SqlCommand cmd1 = new SqlCommand(query1, con);
                    cmd1.ExecuteNonQuery();
                    con.Close();



                }
                catch (Exception)
                { throw; }
            
               }

            catch (Exception )
            {
                MessageBox.Show("Please set theme first....!");

            }

        
        
        }//on load end
        
           
        private void button4_Click(object sender, EventArgs e)                                                //play button
        {

            MessageBox.Show(firstitem);
         
            if (i == 0)
            {

                int value=converttoms(firstitem);
                timer1.Interval = value;
                timer1.Enabled = true;
                WMPLib.IWMPPlaylist Playlist = axWindowsMediaPlayer1.newPlaylist("MyPlayList", "");
                foreach (string song in listbox2)
                {

                    Playlist.appendItem(axWindowsMediaPlayer1.newMedia(song));
                }
                axWindowsMediaPlayer1.currentPlaylist = Playlist;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlenabled = false;
                // axWindowsMediaPlayer1.Hide();
                i++;
                listBox1.SelectedIndex = 0;
                listBox1.Focus();

            }
        }
        int go = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (go == 0)
            //{
            //    MessageBox.Show("timer complete");
            //    timer1.Enabled = false;
            //    go++;
            //}
            //else 
            //{
             try
                {
                    
                ft = 0;
                timer1.Enabled=false;
                listBox1.Items.Clear();
                listbox2.Clear();
                lst.Clear();
                mylist.Clear();
                listofid.Clear();
                playcounter.Clear();
            //    Playlist.clear();
         
                if (con.State == ConnectionState.Closed) { con.Open(); }
                //String qv = "select * from queue1 where extra2='"+"Waiting"+"' order by token desc";
                String qv = "select * from queue1  order by token desc";
                SqlCommand sqlCmd = new SqlCommand(qv, con);
                SqlDataReader sdr = sqlCmd.ExecuteReader();

                while (sdr.Read())
                {
                    listBox1.Items.Add(sdr[6].ToString());
                    listbox2.Add(sdr[5].ToString());  //path 
                    lst.Add(sdr[5].ToString());       //path
                    mylist.Add(sdr[6].ToString());   //---song name propabley
                    listofid.Add((int)sdr[0]);//-->
                    playcounter.Add(sdr[12].ToString());
                    if (ft == 0)
                    {
                        firstitem = sdr[11].ToString();
                        ft++;
                    }
                }
                sdr.Close();
                con.Close();
                int value = converttoms(firstitem);
                timer1.Interval = value;
                timer1.Enabled = true;
              //  MessageBox.Show(timer1.Interval.ToString());
                int qid;
                try
                {
                     qid = listofid.ElementAt(0);
                    
                    con.Open();
                    string query0 = "update queue1 set extra2='" + "Current" + "' where Qid='" + qid + "'";
                    SqlCommand cmd0 = new SqlCommand(query0, con);
                    cmd0.ExecuteNonQuery();
                    con.Close();



                    String var = playcounter.ElementAt(0);
                    int variable = int.Parse(var.Trim()) + 1;
                    con.Open();
                    string query1 = "update queue1 set extra1='" + variable.ToString() + "' where Qid='" + qid + "'";
                    SqlCommand cmd1 = new SqlCommand(query1, con);
                    cmd1.ExecuteNonQuery();
                    con.Close();






                }
                catch (Exception)
                { throw; }
                 //-------------------
                try
                {
                    WMPLib.IWMPPlaylist Playlist2 = axWindowsMediaPlayer1.newPlaylist("MyPlayList", "");



                    foreach (string song in listbox2)
                    {

                        Playlist2.appendItem(axWindowsMediaPlayer1.newMedia(song));
                    }
                    axWindowsMediaPlayer1.currentPlaylist = Playlist2;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    listBox1.SelectedIndex = 0;
                    listBox1.Focus();
                    //-----------------------------
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    SqlDataAdapter aa = new SqlDataAdapter("setstatusofded", con);
                    aa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    aa.SelectCommand.Parameters.Add("@id", SqlDbType.Char, 20).Value = qid.ToString();
                    aa.SelectCommand.ExecuteNonQuery();
                    con.Close();
              
                    //-----------------------------
}
                catch (Exception) { } 


                 //-------------
            }catch (Exception o)
            {
               
                MessageBox.Show(o.ToString());
            }
        
         
  //  }   //else
        }


        private void button5_Click(object sender, EventArgs e)
        {
            //delete code
            try
            {
                        string msg = "Are you sure do you want to delete song..";
                        MessageBoxButtons button = MessageBoxButtons.YesNo;
                        string cap = "About Deletion";
                        DialogResult res = MessageBox.Show(msg, cap, button, MessageBoxIcon.Warning);
                        if (res == System.Windows.Forms.DialogResult.Yes)
                        {

                            string data = listBox1.SelectedItem.ToString();
                            backfor obj = new backfor();
                            int l = obj.deletefromqueue(data);
                            if (l == 1)
                            {
                                MessageBox.Show("Error in deleting!!!", "About Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            listBox1.Items.Remove(data);
                            MessageBox.Show("Song Delete Successfully!!!", "About Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                        }
          
            }
            catch (Exception r)
            {
                MessageBox.Show(r.ToString());
            }

            //end
            

        }
        string paths,delta;
        int myid = 0;
        private void axWindowsMediaPlayer1_CurrentItemChange(object sender, AxWMPLib._WMPOCXEvents_CurrentItemChangeEvent e)
        {
            //-----------------------------------------------------------------
            
            List<int>  list=new List<int>();
            string hj = axWindowsMediaPlayer1.currentMedia.name.ToString();
            paths = @"F:\music\" + hj + ".mp3";
            label4.Text = paths;
            if (con.State == ConnectionState.Closed) { con.Open(); }
                String qv ="select * from queue1 where  Paths not like '"+paths+"' and extra2 like '"+"Current"+"'";
                SqlCommand sqlCmd = new SqlCommand(qv, con);
                SqlDataReader sdr = sqlCmd.ExecuteReader();
                 while (sdr.Read())
                {
                    list.Add((int)sdr[0]);
                }
            sdr.Close();
            con.Close();
           try
            {
                foreach (var item in list)
                {

                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    SqlDataAdapter aa = new SqlDataAdapter("forstatus", con);
                    aa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    aa.SelectCommand.Parameters.Add("@para", SqlDbType.Char, 20).Value = item.ToString();
                    aa.SelectCommand.ExecuteNonQuery();
                    con.Close();


                }
             
            }
            catch (Exception r) { MessageBox.Show(r.ToString()); }
            //----------------------------------------------------------------dedication
            //try
            //{
            //    foreach (var item in list)
            //    {

            //        if (con.State == ConnectionState.Closed) { con.Open(); }
            //        SqlDataAdapter aamy = new SqlDataAdapter("myprocdedication", con);
            //        aamy.SelectCommand.CommandType = CommandType.StoredProcedure;
            //        aamy.SelectCommand.Parameters.Add("@data", SqlDbType.Char, 20).Value = item.ToString();
            //        aamy.SelectCommand.ExecuteNonQuery();
            //        con.Close();

            //    }
                
            //}
            //catch (Exception rr) { MessageBox.Show(rr.ToString()); }
        
       }

        int q=0;
        private void label4_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void label5_TextChanged(object sender, EventArgs e)
        {

        

        }//event close here
         
        
        //ms function
        public int converttoms(string data)
        {
            int value1, value2, milldata;
            string[] a = new string[2];
            a = data.Split(':');
            value1 = int.Parse(a[0]);
            value2 = int.Parse(a[1]);
            value1 = value1 * 60000;
            value2 = value2 * 1000;
            milldata = value1 + value2;
            return milldata;
        }
    
        
        
        
        
        // desgin and navigatuon code only
        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            SongList obj = new SongList();
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
        private void songListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongList obj = new SongList();
            this.Hide();
            obj.Show();

        }

        private void themeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 obj = new Form1();
            this.Hide();
            obj.Show();
//            axWindowsMediaPlayer1.newPlaylist("MyPlayList", "");
        }


        private void addSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Addsongs obj = new Addsongs();
          //  this.Hide();
            obj.Show();
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

       
        //end of class


     }
  }
 
   

