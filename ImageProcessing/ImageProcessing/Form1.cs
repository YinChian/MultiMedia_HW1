using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        private bool pic1_selected = false;
        private bool pic2_selected = false;
        public static string path_src1 = "";
        public static string path_src2 = "";
        
        Form2 form2 = new Form2();
        Form3 form3 = new Form3();

        public Form1()
        {
            InitializeComponent();
        }

        //圖片一
        private void button1_Click(object sender, EventArgs e)
        {
            pic1_selected = true;
            //openFileDialog1.InitialDirectory = "C:\\Pictures";
            openFileDialog1.InitialDirectory = ".\\";
            openFileDialog1.Filter = "PNG|*.png|JPG|*.jpg|BMP|*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                button1.Text = openFileDialog1.FileName;
                path_src1 = openFileDialog1.FileName;
            }
        }

        //圖片二
        private void button2_Click(object sender, EventArgs e)
        {
            pic2_selected = true;
            //openFileDialog2.InitialDirectory = "C:\\Pictures";
            openFileDialog2.InitialDirectory = ".\\";
            openFileDialog2.Filter = "PNG|*.png|JPG|*.jpg|BMP|*.bmp";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                button2.Text = openFileDialog2.FileName;
                path_src2 = openFileDialog2.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pic1_selected && pic2_selected)
            {
                
                Hide();
                form2.Show();
            }
            else
            {
                MessageBox.Show(
                    "尚未選完圖片！",
                    "注意",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Hand
                );
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 不知道為什麼有時候關掉這個視窗，他還會繼續跑。所以我只好加上這行來解決這個問題。
            Environment.Exit(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pic1_selected && pic2_selected)
            {
                
                Hide();
                form3.Show();
            }
            else
            {
                MessageBox.Show(
                    "尚未選完圖片！", 
                    "注意", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Hand
                );
            }
        }
    }
}
