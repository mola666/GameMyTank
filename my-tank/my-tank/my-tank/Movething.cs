using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_tank
{
    //做了 我的坦克 的四个方向的枚举
    enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }
    class Movething : GameObject
    {
        //为了解决资源冲突问题，特意创建了一把锁
        private Object _lock = new object();
        
        public int Speed { get; set; }



        private Direction dir;

        //这里设置了一个 坦克的 变量
        public Bitmap BitmapUp { get; set; }
        public Bitmap BitmapDown { get; set; }
        public Bitmap BitmapLeft { get; set; }
        public Bitmap BitmapRight { get; set; }

        //为了获取坦克图像的大小
        public Direction Dir
        {
            get { return dir; }
            set
            {
                dir = value;
                Bitmap bmp = null;

                switch(dir)
                {
                    case Direction.Up:
                        bmp = BitmapUp;
                        break;
                    case Direction.Down:
                        bmp = BitmapDown;
                        break;
                    case Direction.Left:
                        bmp = BitmapLeft;
                        break;
                    case Direction.Right:
                        bmp = BitmapRight;
                        break;

                }
                lock(_lock)
                {
                    Width = bmp.Width;
                    Height = bmp.Height;
                }
            }
        }

        //为了获取坦克的图像数据
        protected override Image GetImage()
        {
            Bitmap bitmap = null;
            switch(Dir)
            {
                case Direction.Up:
                    bitmap = BitmapUp;
                    break;
                case Direction.Down:
                    bitmap = BitmapDown;
                    break;
                case Direction.Left:
                    bitmap = BitmapLeft;
                    break;
                case Direction.Right:
                    bitmap = BitmapRight;
                    break;
            }
            //将图片的黑色背景去除掉
            bitmap.MakeTransparent(Color.Black);

            return bitmap;
        }

        public override void DrawSelf()
        {
            lock(_lock)
            {
                base.DrawSelf();
            }
        }
    }
}
