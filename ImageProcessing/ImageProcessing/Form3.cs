using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ImageProcessing
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Shown(object sender, EventArgs e)
        {

            // 載入圖片
            Bitmap src1 = new Bitmap(Form1.path_src1);
            Bitmap src2 = new Bitmap(Form1.path_src2);

            // 判斷圖片大小是否相同
            if(src1.Height != src2.Height || src1.Width != src2.Width)
            {
                MessageBox.Show(
                    "兩張圖片大小不相同，請改用\"一般開始\"，謝謝！", 
                    "錯誤", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
                Form1 form1 = new Form1();
                form1.Show();
                Hide();
                return; //結束
            }

            // 縮放視窗 + 圖片
            pictureBox1.Height = src1.Height;
            pictureBox1.Width = src1.Width;

            Bitmap dest = new Bitmap(Form1.path_src1);
            pictureBox1.Image = dest;

            // 鎖定記憶體
            Rectangle destRect = new Rectangle(0, 0, dest.Width, dest.Height);
            BitmapData destBmpData = dest.LockBits(destRect, ImageLockMode.ReadWrite, dest.PixelFormat);
            Rectangle src1Rect = new Rectangle(0, 0, src1.Width, src1.Height);
            BitmapData src1BmpData = src1.LockBits(src1Rect, ImageLockMode.ReadWrite, src1.PixelFormat);
            Rectangle src2Rect = new Rectangle(0, 0, src2.Width, src2.Height);
            BitmapData src2BmpData = src2.LockBits(src2Rect, ImageLockMode.ReadWrite, src2.PixelFormat);
            
            // 設定指標
            IntPtr destptr = destBmpData.Scan0;
            IntPtr src1ptr = src1BmpData.Scan0;
            IntPtr src2ptr = src2BmpData.Scan0;

            // 設定變數
            int destBytes = Math.Abs(destBmpData.Stride) * dest.Height;
            byte[] destRgbValues = new byte[destBytes];
            int src1Bytes = Math.Abs(src1BmpData.Stride) * src1.Height;
            byte[] src1RgbValues = new byte[src1Bytes];
            int src2Bytes = Math.Abs(src2BmpData.Stride) * src2.Height;
            byte[] src2RgbValues = new byte[src2Bytes];

            // 判斷兩張圖片的位元深度是否相同
            if(src1BmpData.Stride != src2BmpData.Stride)
            {
                // 先解鎖再操作
                src1.UnlockBits(src1BmpData);
                src2.UnlockBits(src2BmpData);
                dest.UnlockBits(destBmpData);
                MessageBox.Show(
                    "兩張圖片顏色位元深度不同，請改用\"一般開始\"，謝謝！", 
                    "錯誤", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
                Form1 form1 = new Form1();
                form1.Show();
                Hide();
                return; //結束
            }

            // 讀取整塊記憶體
            Marshal.Copy(destptr, destRgbValues, 0, destBytes);
            Marshal.Copy(src1ptr, src1RgbValues, 0, src1Bytes);
            Marshal.Copy(src2ptr, src2RgbValues, 0, src2Bytes);

            // 套用每一個blend
            for(double blend = 0; blend <= 1; blend += 0.01)
            {
                // 走訪每一個Bytes
                for(int i = 0; i < destBytes; i++)
                {
                    destRgbValues[i] = (byte)(src1RgbValues[i] * (1 - blend) + src2RgbValues[i] * (blend));
                }

                // 存回 + 解鎖記憶體位址
                Marshal.Copy(destRgbValues, 0, destptr, destBytes);
                dest.UnlockBits(destBmpData);
                
                pictureBox1.Image = dest;
                pictureBox1.Refresh();
                
                // 記憶體位置上鎖
                dest.LockBits(destRect, ImageLockMode.ReadWrite, dest.PixelFormat);
                
                //暫停1ms -> 取得約100fps
                Thread.Sleep(1);
            }

            // 鎖定回去
            Marshal.Copy(src1RgbValues, 0, src1ptr, src1Bytes);
            Marshal.Copy(src2RgbValues, 0, src2ptr, src2Bytes);
            Marshal.Copy(destRgbValues, 0, destptr, destBytes);
            src1.UnlockBits(src1BmpData);
            src2.UnlockBits(src2BmpData);
            dest.UnlockBits(destBmpData);
            MessageBox.Show(
                "使用Marshal執行完成！按一下視窗的關閉鍵可回到首頁！", 
                "謝謝使用", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information
            );
        }

        // 關閉視窗 -> 回到第一個視窗
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }
    }
}
