using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;



namespace ImageProcessing
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
        }


        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            //Hide();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            Bitmap src1 = new Bitmap(Form1.path_src1);
            Bitmap src2 = new Bitmap(Form1.path_src2);

            // 縮放視窗 + 圖片
            pictureBox1.Height = (src1.Height > src2.Height) ? src1.Height : src2.Height;
            pictureBox1.Width = (src1.Width > src2.Width) ? src1.Width : src2.Width;

            // 先載入一次圖片 -- 避免一開始等待過久
            pictureBox1.Image = src1;
            pictureBox1.Refresh();

            // 先讀取兩張圖片的Bitmap節省時間
            Bitmap dest = new Bitmap(Width, Height);
            Color[,] src1_colors = new Color[Width, Height];
            Color[,] src2_colors = new Color[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if(y < src1.Height && x < src1.Width) { src1_colors[x, y] = src1.GetPixel(x, y); }
                    if(y < src2.Height && x < src2.Width) { src2_colors[x, y] = src2.GetPixel(x, y); }
                }
            }

            // 重頭戲
            int total_step = 10;
            int R = 0, G = 0, B = 0;
            for (int step = 1; step <= total_step; step++)
            {
                double blend = (double)step / total_step;
                for (int x = 0; x < dest.Width - 1; x++)
                {
                    for (int y = 0; y < dest.Height - 1; y++)
                    {
                        if (y >= src1.Height || x >= src1.Width)
                        {
                            R = (int)(src2_colors[x, y].R * blend);
                            G = (int)(src2_colors[x, y].G * blend);
                            B = (int)(src2_colors[x, y].B * blend);
                        }
                        else if (y >= src2.Height || x >= src2.Width)
                        {
                            R = (int)(src1_colors[x, y].R * (1 - blend));
                            G = (int)(src1_colors[x, y].G * (1 - blend));
                            B = (int)(src1_colors[x, y].B * (1 - blend));
                        }
                        else
                        {
                            R = (int)(src2_colors[x, y].R * blend + src1_colors[x, y].R * (1 - blend));
                            G = (int)(src2_colors[x, y].G * blend + src1_colors[x, y].G * (1 - blend));
                            B = (int)(src2_colors[x, y].B * blend + src1_colors[x, y].B * (1 - blend));
                        }
                        
                        dest.SetPixel(x, y, Color.FromArgb(R, G, B));
                    }
                }
                Console.WriteLine(step);
                pictureBox1.Image = dest;
                pictureBox1.Refresh();
                // 保底等待時間
                Thread.Sleep(10);
            }

            MessageBox.Show(
                "完成！按一下視窗的關閉鍵可回到首頁。\n您也可以嘗試使用兩張規格相同圖片，並使用Marshal開始。", 
                "謝謝使用", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information
            );

        }
    }
}
