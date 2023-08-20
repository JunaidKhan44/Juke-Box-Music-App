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
    public partial class Category : Form
    {
        static SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true"); 
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        
        public Category()
        {
            InitializeComponent();
            
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            show1();
            show2();
            show3();

        }
   private void Category_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            String qv = "select *  from category ";
            SqlCommand sqlCmd = new SqlCommand(qv, con);
            SqlDataReader sdr = sqlCmd.ExecuteReader();
            String var;
            while (sdr.Read())
            {
                var = sdr[0].ToString();
               listBox1.Items.Add(var);
            }
            sdr.Close();
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string var = textBox1.Text;
            addcategory obj = new addcategory();
            string item=obj.addcat(var);
            if (item == null)
            {
                MessageBox.Show("please add item in text field then press add button", "About Catg.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
            }
            else
            {
                listBox1.Items.Add(var);
                textBox1.Text = "";
            }
        }
        string var;
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var = listBox1.SelectedItem.ToString();
                if (var == "") { MessageBox.Show("please fill the text field then press add button"); }
                else
                {
                    textBox1.Text = var;
                    listBox1.Items.Remove(var);
                    //        addcategory obj = new addcategory();
                    //        obj.deleteitem(var);
                    string msg = "Are you sure do you want to delete Category!..." + "' " + var + " '";
                    MessageBoxButtons button = MessageBoxButtons.YesNo;
                    string cap = "About Catg. Deletion";
                    DialogResult res = MessageBox.Show(msg, cap, button, MessageBoxIcon.Warning);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        textBox1.Text = "";
                        if (con.State == ConnectionState.Closed) con.Open();
                        string query = "delete from category where category_of_song='" + var + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("' " + var + " '" + "  is remove successfully!", "About Catg. Deletion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else 
                    {
                        MessageBox.Show("Error in deleting category"+"'"+var+"'", "About Catg. Deletion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         
                    }
                }
               
            }catch(Exception)
            {
            MessageBox.Show("Please first selected category from list to remove!", "About Catg. Deletion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }
        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var = listBox1.SelectedItem.ToString();
            textBox1.Text = var;
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "^[a-zA-Z ]"))
            {
                try
                {
                   // MessageBox.Show("This textbox accepts only alphabetical characters");
                    textBox1.Text.Remove(textBox1.Text.Length - 1);
                }
                catch(Exception)
                {
                    
                }
            }
        }
     
        //design and navigation code only
        public void show1()
        {
            Elispsecontrol nn = new Elispsecontrol();
            nn.TargetControl = button5;
            //button5.BackColor = Color.Blue;
            nn.CornerRadius = 20;
            this.Controls.Add(button5);

        }

        public void show2()
        {
            Elispsecontrol nn = new Elispsecontrol();
            nn.TargetControl = button4;
            //button5.BackColor = Color.Blue;
            nn.CornerRadius = 20;
            this.Controls.Add(button4);

        }
        public void show3()
        {
            Elispsecontrol nn = new Elispsecontrol();
            nn.TargetControl = textBox1;
            //button5.BackColor = Color.Blue;
            nn.CornerRadius = 6;
            this.Controls.Add(textBox1);

        }

        private void addSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Addsongs obj = new Addsongs();
            this.Hide();
            obj.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SongList obj = new SongList();
            this.Hide();
            obj.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Addsongs obj = new Addsongs();
            obj.Show();
        }
        private void themeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 obj = new Form1();
            obj.Show();

        }

        private void playerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player obj = new player();
            obj.Show();
            this.Hide();
        }

        private void songListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongList obj = new SongList();
            this.Hide();
            obj.Show();
        }

        private void queueListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player obj = new player();
            this.Hide();
            obj.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
