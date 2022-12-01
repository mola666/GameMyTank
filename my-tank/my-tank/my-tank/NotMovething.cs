using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_tank
{
    class NotMovething: GameObject
    {
        private Image img;
        public Image Img 
        {
            get { return img; } 
            set
            {
                img = value;
                //提取静态物体的长宽
                Width = img.Width;
                Height = img.Height;
            }
        }

        public NotMovething(int x, int y, Image img)
        {
            this.X = x;
            this.Y = y;
            this.Img = img;
        }

        protected override Image GetImage()
        {
            return Img;
        }
    }
}
