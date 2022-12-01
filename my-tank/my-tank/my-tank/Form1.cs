using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using my_tank.Properties;

namespace my_tank
{

    public partial class Form1 : Form
    {
        
        static public Graphics windowG;  //这个是窗体画布
        private Thread t;
        private static Bitmap tempBmp;  //这个是一个图片
        public int X { get; set; }
        public int Y { get; set; }


        public Form1()
        {
            InitializeComponent();

            //1、屏幕居中
            this.StartPosition = FormStartPosition.CenterScreen;

            //2、创建画布
            windowG = this.CreateGraphics();

            //4、创建一个图片，将墙体画到图片上，再将图片放到窗体上
            tempBmp = new Bitmap(450,450);
            Graphics bmpG =  Graphics.FromImage(tempBmp);  //这个方法会将图片自动生成一个画布
            GameFramework.g = bmpG;//将画布 g 中的内容画到图片 tempBmp上

            //3、创建线程，避免出现图片与画布的阻塞
            t = new Thread(new ThreadStart(GameMainThread));
            t.Start();

        }

        private static void GameMainThread()
        {
            GameFramework.Start();

            //设置延时时间
            int sleepTime = 1000 / 60;
            while(true)
            {
                //1、使用黑色清空画布
                GameFramework.g.Clear(Color.Black);

                GameFramework.Update();

                windowG.DrawImage(tempBmp,0,0);

                Thread.Sleep(sleepTime);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            GameObjectManager.KeyDown(e);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            GameObjectManager.KeyUp(e);
        }
    }
}
