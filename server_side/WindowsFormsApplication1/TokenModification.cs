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
    public partial class TokenModification : Form
    {
        static SqlConnection con = new SqlConnection("Data Source=(local);initial Catalog = jukebox2;Integrated Security=true"); 
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        int counter = 0;
        int rowindex, columnindex;
       
        public TokenModification()
        {
            InitializeComponent();
            
            
        
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            show1();
            show2();
            show3();
         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            int price =0;
            string type ="";
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }

                price = int.Parse(richTextBox1.Text);
                type = richTextBox2.Text;
           
               
                if(String.IsNullOrEmpty(richTextBox1.Text)||String.IsNullOrEmpty(richTextBox2.Text))
                 {
                    MessageBox.Show("Please provide  Token Type and Price", "About Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    richTextBox2.SelectionStart = richTextBox2.Text.Length;
                    richTextBox2.Focus();
                }
                else
                {
           
                    string query = "insert into tokendetails (tokentype,tokenprice) values('" + type + "','"+price+"')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    dataGridView1.Rows.Clear();
                    if (con.State == ConnectionState.Closed)
                    { con.Open(); }
                    string query2 = "select * from  tokendetails  where tokenid>1 ";
                    SqlCommand sqlCmd2 = new SqlCommand(query2, con);
                    SqlDataReader sdr = sqlCmd2.ExecuteReader();
                    while (sdr.Read())
                    {


                        dataGridView1.Rows.Add(sdr[0], sdr[1].ToString(), sdr[2]);

                    }
                    sdr.Close();
                    con.Close();
                    MessageBox.Show("Add Successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    richTextBox2.Text = "";
                    richTextBox1.Text = "";

                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please Provide type and price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("row mo:"+rowindex.ToString());
            //MessageBox.Show("column no"+columnindex.ToString());
            dataGridView1.Rows[rowindex].Cells[1].Value=richTextBox2.Text;
            dataGridView1.Rows[rowindex].Cells[2].Value =richTextBox1.Text;
            string val =dataGridView1.Rows[rowindex].Cells[0].Value.ToString();
            int value = int.Parse(val);
            if (con.State == ConnectionState.Closed) { con.Open(); }
                   string msg = "Are you sure do you want to update token details?";
                        MessageBoxButtons button = MessageBoxButtons.YesNo;
                        string cap = "About Update";
                        DialogResult res = MessageBox.Show(msg, cap, button, MessageBoxIcon.Warning);
                        if (res == System.Windows.Forms.DialogResult.Yes)
                        {

                            string query = "update tokendetails set tokentype='" + richTextBox2.Text + "',tokenprice='" + richTextBox1.Text + "' where tokenid='" + value + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            richTextBox1.Text = "";
                            richTextBox2.Text = "";
                            MessageBox.Show("Update perform Successfully!!!", "About Update", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            nn.TargetControl = button3;
            nn.CornerRadius = 20;
            this.Controls.Add(button3);
        }
        public void show3()
        {
            Elispsecontrol nn = new Elispsecontrol();
            nn.TargetControl = button8;
            nn.CornerRadius = 20;
            this.Controls.Add(button8);
        }
        private void TokenModification_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            string query = "select * from  tokendetails  where tokenid>1 ";
            SqlCommand sqlCmd = new SqlCommand(query, con);
            SqlDataReader sdr = sqlCmd.ExecuteReader();
            //string dta = "";
            //int price = 0;
            while (sdr.Read())
            {
                
                
                dataGridView1.Rows.Add(sdr[0],sdr[1].ToString(),sdr[2]);

            }
            sdr.Close();
            con.Close();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("Row index"+e.RowIndex.ToString());
            MessageBox.Show("Column Index"+e.ColumnIndex.ToString());
            richTextBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
            richTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
            rowindex = e.RowIndex;
            columnindex = e.ColumnIndex;


        }

        private void button8_Click(object sender, EventArgs e)
        {
            string val = dataGridView1.Rows[rowindex].Cells[0].Value.ToString();
            int value = int.Parse(val);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            string msg = "Are you sure do you want to delete token details?";
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            string cap = "About delete";
            DialogResult res = MessageBox.Show(msg, cap, button, MessageBoxIcon.Warning);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {

                string query = "delete from tokendetails where tokenid='" + value + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                richTextBox1.Text = "";
                richTextBox2.Text = "";
                MessageBox.Show("Delete perform Successfully!!!", "About Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            ///---------------
            dataGridView1.Rows.Clear();
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            string query2 = "select * from  tokendetails  where tokenid>1 ";
            SqlCommand sqlCmd2 = new SqlCommand(query2, con);
            SqlDataReader sdr = sqlCmd2.ExecuteReader();
            while (sdr.Read())
            {


                dataGridView1.Rows.Add(sdr[0], sdr[1].ToString(), sdr[2]);

            }
            sdr.Close();
            con.Close();



        }

        private void addSongsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Addsongs obj = new Addsongs();
            obj.Show();
       
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Category obj = new Category();
            obj.Show();
       
        
        }

        private void songListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            SongList obj = new SongList();
            obj.Show();
       
        }
        
    }
}
