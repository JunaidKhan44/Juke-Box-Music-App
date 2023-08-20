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
    public partial class SongList : Form
    {
        static SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true"); 
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
 
        public SongList()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            show1();
            show2();
       
        }

        string cl = null;
        String var;
        private void SongList_Load(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cl = Form1.selectedtheme.ToString();
                if (cl == null)
                {
                }
                else
                {
                    String qv = "select *  from song where theme='" + cl + "'";
                    SqlCommand sqlCmd = new SqlCommand(qv, con);
                    SqlDataReader sdr = sqlCmd.ExecuteReader();
                    
                    while (sdr.Read())
                    {
                        var = sdr[6].ToString();
                        listBox1.Items.Add(var);
                    }
                    sdr.Close();
                    con.Close();
                }
            }
            catch (Exception) 
            {
                MessageBox.Show("Please set theme first on theme page");
            }
        }
      

        private void button5_Click(object sender, EventArgs e)
        {
            string data = "";
            try
            {
                data = listBox1.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(data))
                {
                    backfor obj = new backfor();
                    int l = obj.deletefromqueue(data);
                    if (l == 1)
                    {
                        MessageBox.Show("Error in deleting!!!", "About Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        string msg = "Are you sure do you want to delete song..";
                        MessageBoxButtons button = MessageBoxButtons.YesNo;
                        string cap = "About Deletion";
                        DialogResult res = MessageBox.Show(msg, cap, button, MessageBoxIcon.Warning);
                        if (res == System.Windows.Forms.DialogResult.Yes)
                        {
                           if (con.State == ConnectionState.Closed) { con.Open(); }
                            string query = "update song set theme_id=Null,category_id=Null,tid=Null where lyrics='" + data + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            string query1 = "delete from song where lyrics='" + data + "'";
                            SqlCommand cmd1 = new SqlCommand(query1, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();
                            listBox1.Items.Remove(data);
                            MessageBox.Show("Deletion perform Successfully!!!", "About Deletion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select item first to delete!!!", "About Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
                    string msg = "Are you sure do you want to update songs list..";
                    MessageBoxButtons button = MessageBoxButtons.YesNo;
                    string cap = "About Update";
                    DialogResult res = MessageBox.Show(msg, cap, button, MessageBoxIcon.Information);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        listBox1.Items.Clear();
                        var = "";
                        String qv = "select *  from song where theme='" + cl + "'";
                        SqlCommand sqlCmd = new SqlCommand(qv, con);
                        SqlDataReader sdr = sqlCmd.ExecuteReader();
                        
                        while (sdr.Read())
                        {
                            var = sdr[6].ToString();
                            listBox1.Items.Add(var);
                        }
                        sdr.Close();
                        con.Close();
            
                   }
                    MessageBox.Show("Songs list update successfully", "About Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


//design and navigation code only
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
        }

        public void show1()
        {
            Elispsecontrol nn = new Elispsecontrol();
            nn.TargetControl = button2;
            nn.CornerRadius = 20;
            this.Controls.Add(button2);

        }
        public void show2()
        {
            Elispsecontrol nn = new Elispsecontrol();
            nn.TargetControl = button5;
            nn.CornerRadius = 20;
            this.Controls.Add(button5);

        }

        private void addSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Addsongs obj = new Addsongs();
            this.Hide();
            obj.Show();
        }

        private void queueListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            player oobj = new player();
            oobj.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            player obj = new player();
            obj.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Category obj = new Category();
            obj.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void tokenModificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            TokenModification obj = new TokenModification();
            obj.Show();
        }


//end of class
 }
 }

