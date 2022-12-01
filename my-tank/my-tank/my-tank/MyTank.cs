using my_tank.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_tank
{
    class MyTank:Movething
    {
        public bool IsMoving { get; set; }
        
        //我的坦克的血量
        public int HP { get; set; }
        //记录坦克的初始位置
        private int originalX;
        private int originalY;

        public MyTank(int x,int y,int speed)
        {
            IsMoving = false;
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            //这里为 Movething 里面的 坦克变量 进行赋值
            BitmapDown = Resources.MyTankDown;
            BitmapUp = Resources.MyTankUp;
            BitmapRight = Resources.MyTankRight;
            BitmapLeft = Resources.MyTankLeft;
            //设置默认方向
            this.Dir = Direction.Up;
            //我的坦克的血量
            HP = 4;
            //将初始位置进行保存
            originalX = x;
            originalY = y;
        }

        public override void Update()
        {
            MoveCheck();
            Move();
            base.Update();
        }

        private void MoveCheck()
        {
            #region 检查有没有超出边界
            if (Dir == Direction.Up)
            {
                if (Y - Speed < 0)
                {
                    IsMoving = false; return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y + Speed + Height > 450)
                {
                    IsMoving = false; return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X - Speed < 0)
                {
                    IsMoving = false; return;
                }

            }
            else if (Dir == Direction.Right)
            {
                if (X + Speed + Height > 450)
                {
                    IsMoving = false; return;
                }
            }
            #endregion

            #region 检查有没有和其他元素碰撞
            Rectangle rect = GetRectangle();
            switch (Dir)
            {
                case Direction.Up:
                    rect.Y -= Speed;
                    break;
                case Direction.Down:
                    rect.Y += Speed;
                    break;
                case Direction.Left:
                    rect.X -= Speed;
                    break;
                case Direction.Right:
                    rect.X += Speed;
                    break;
            }

            if (GameObjectManager.IsCollidedWall(rect) != null)
            {
                IsMoving = false; return;
            }
            if (GameObjectManager.IsCollidedSteel(rect) != null)
            {
                IsMoving = false; return;
            }
            if (GameObjectManager.IsCollidedBoss(rect) != false)
            {
                IsMoving = false; return;
            }
            #endregion
        }
        private void Move()
        {
            if (IsMoving == false) return;

            switch (Dir)
            {
                case Direction.Up:
                    Y -= Speed;
                    break;
                case Direction.Down:
                    Y += Speed;
                    break;
                case Direction.Left:
                    X -= Speed;
                    break;
                case Direction.Right:
                    X += Speed;
                    break;
            }
        }

        public void KeyDown(KeyEventArgs args)
        {
            switch(args.KeyCode)
            {
                case Keys.W:
                    Dir = Direction.Up;
                    IsMoving = true;
                    break;
                case Keys.S:
                    Dir = Direction.Down;
                    IsMoving = true;
                    break;
                case Keys.A:
                    Dir = Direction.Left;
                    IsMoving = true;
                    break;
                case Keys.D:
                    Dir = Direction.Right;
                    IsMoving = true;
                    break;
                case Keys.Space:
                    //发射子弹
                    Attack();
                    break;
            }
        }
        //发射子弹函数
        private void Attack()
        {
            int x = this.X;
            int y = this.Y;

            //声音
            SoundMananger.PlayFire();

            switch (Dir)
            {
                case Direction.Up:
                    x = x + Width / 2;
                    break;
                case Direction.Down:
                    x = x+ Width / 2;
                    y += Height;
                    break;
                case Direction.Left:
                    y = y + Height/2;
                    break;
                case Direction.Right:
                    x += Width;
                    y = y + Height / 2;
                    break;
            }

            GameObjectManager.CreateBullet(x,y,Tag.MyTank,Dir);

        }


        public void KeyUp(KeyEventArgs args)
        {
            switch(args.KeyCode)
            {
                case Keys.W:
                    IsMoving = false;
                    break;
                case Keys.S:
                    IsMoving = false;
                    break;
                case Keys.A:
                    IsMoving = false;
                    break;
                case Keys.D:
                    IsMoving = false;
                    break;
            }
        }

        public void TakeDamage()
        {
            HP--;
            if(HP <= 0)
            {
                X = originalX;
                Y = originalY;
                HP = 4;
            }

        }
    }
}
