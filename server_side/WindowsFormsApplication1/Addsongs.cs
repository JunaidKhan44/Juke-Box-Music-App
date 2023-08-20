using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Addsongs : Form
    {
        static SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true");
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
       
        public Addsongs()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            show1();
            show2();
            
        }
        string path, singer, album, theme, lyrics, composer, duration;
        string v1;
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            {

                this.richTextBox1.Text = openFileDialog1.FileName;
                v1 = this.richTextBox1.Text;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            path = richTextBox1.Text;
            singer = richTextBox2.Text;
            album = richTextBox3.Text;
            theme = themecmb.SelectedItem.ToString().Trim();
            lyrics = richTextBox5.Text;
            composer = richTextBox6.Text;
            duration = richTextBox7.Text;
            if (richTextBox1.Text=="")
            {
                MessageBox.Show("Please enter data of song first!!!", "About Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                richTextBox1.Focus();
            }
            else
            {
                    SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true");
                
                try
                {
                    con.Open();
                    SqlDataAdapter aa = new SqlDataAdapter("thend", con);
                    aa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    aa.SelectCommand.Parameters.Add("@singer", SqlDbType.Char, 20).Value = richTextBox2.Text;
                    aa.SelectCommand.Parameters.Add("@album", SqlDbType.Char, 50).Value = richTextBox3.Text;
                    aa.SelectCommand.Parameters.Add("@theme", SqlDbType.Char, 20).Value = themecmb.SelectedItem.ToString().Trim();    
                    aa.SelectCommand.Parameters.Add("@duration", SqlDbType.Char, 10).Value = richTextBox7.Text;
                    aa.SelectCommand.Parameters.Add("@Paths", SqlDbType.Char, 100).Value = richTextBox1.Text;
                    aa.SelectCommand.Parameters.Add("@lyrics", SqlDbType.Char, 100).Value = richTextBox5.Text;
                    aa.SelectCommand.Parameters.Add("@composer", SqlDbType.Char, 40).Value = richTextBox6.Text;
                    aa.SelectCommand.Parameters.Add("@category", SqlDbType.Char, 20).Value = categorycmb.SelectedItem.ToString().Trim();
                    aa.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Song Add Successfully!!!", "Add Song", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error while adding !!!", "About Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //code here to insert into queue
                try
                {
                    string themeofday = "";
                    themeofday = Form1.selectedtheme.ToString();
                    themeofday = themeofday.Trim();
                    if (themeofday.Equals(categorycmb.SelectedItem.ToString().Trim()))
                    {
                        //select max(sid) from songs
                        int sid = 0;
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                            String myq = "select Max(sid) from song";
                            SqlCommand sqlCmd = new SqlCommand(myq, con);
                            SqlDataReader sdr = sqlCmd.ExecuteReader();
                            while (sdr.Read())
                            {
                                sid = (int)(sdr[0]);
                            }
                            sdr.Close();
                            string query = "insert into queue1 (singer,album,theme,duration,Paths,lyrics,composer,category,sid,token,duration2) values ('" + richTextBox2.Text + "','" + richTextBox3.Text + "','" + themecmb.SelectedItem.ToString().Trim() + "','" + richTextBox7.Text + "','" + richTextBox1.Text + "','" + richTextBox5.Text + "','" + richTextBox6.Text + "','" + categorycmb.SelectedItem.ToString().Trim() + "','" + sid + "','" + 40 + "','" + richTextBox7.Text + "')";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }//-------else end

                    }
                }
                catch (Exception y) { }
            }
        }


        private void Addsongs_Load(object sender, EventArgs e)
        {

            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                String append = "select *  from theme";
                String read;
                SqlCommand appendCmd = new SqlCommand(append, con);
                SqlDataReader appendsdr = appendCmd.ExecuteReader();
                while (appendsdr.Read())
                {
                    read = (String)(appendsdr[1]);
                    themecmb.Items.Add(read);
                }
                appendsdr.Close();
                con.Close();

                if (con.State == ConnectionState.Closed) { con.Open(); }
                String append2 = "select *  from category";
                String read2;
                SqlCommand appendCmd2 = new SqlCommand(append2, con);
                SqlDataReader appendsdr2 = appendCmd2.ExecuteReader();
                while (appendsdr2.Read())
                {
                    read2 = (String)(appendsdr2[0]);
                    categorycmb.Items.Add(read2);
                }
                appendsdr.Close();
                con.Close();
            }
            catch (Exception r) { MessageBox.Show(r.ToString()); }
            
        }

        //design and navigation code only
        private void themeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 obj = new Form1();
            this.Hide();
            obj.Show();
        }

        private void queueListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            player obj1 = new player();
            obj1.Show();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Category obj = new Category();
            obj.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Category obj = new Category();
            obj.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 obj = new Form1();
            obj.Show();
        }
        private void songListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SongList obj = new SongList();
                this.Hide();
                obj.Show();

            }
            catch (Exception)
            {
                MessageBox.Show("Please select theme first......!");
            }
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

        private void tokenModificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            TokenModification obj = new TokenModification();
            obj.Show();
        }


        //end of class
    }
}
