using my_tank.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_tank
{
    
    //爆炸的显示效果和其他的显示效果不一样
    class Explosion : GameObject
    {

        public bool IsNeedDestroy { get; set; }

        private int playSpeed = 1;
        private int playCount = 0;
        private int index=0;


        private Bitmap[] bmpArray = new Bitmap[]
        {
            Resources.EXP1,
            Resources.EXP2,
            Resources.EXP3,
            Resources.EXP4,
            Resources.EXP5
        };
        public Explosion(int x,int y)
        {
            foreach(Bitmap bmp in bmpArray)
            {
                bmp.MakeTransparent(Color.Black);
            }
            //x y 表示我们传入的位置  ，this.X  this.Y 表示爆炸的位置
            this.X = x - bmpArray[0].Width / 2;
            this.Y = y - bmpArray[0].Height / 2;
            this.IsNeedDestroy = false;
        }
        protected override Image GetImage()
        {
            if (index > 4)
            {
                return bmpArray[4];
            }
            return bmpArray[index];
        }
        public override void Update()
        {
            playCount++;
            index = (playCount - 1) / playSpeed;
            if(index > 4)
            {
                this.IsNeedDestroy = true;
            }
            base.Update();

        }

    }
}
