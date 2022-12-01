using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_tank
{
    class EnemyTank:Movething
    {
        //改变方向帧
        public int ChangeDirSpeed { get; set; }
        private int changeDirCount = 0;
        public int AttackSpeed { get; set; }
        private int attackCount = 0;

        private Random r = new Random();
        public EnemyTank(int x,int y,int speed,Bitmap bmpDown,Bitmap bmpUp,Bitmap  bmpRight, Bitmap bmpLeft)
        {
            
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            //这里为 Movething 里面的 坦克变量 进行赋值
            BitmapDown = bmpDown;
            BitmapUp = bmpUp;
            BitmapRight = bmpRight;
            BitmapLeft = bmpLeft;
            //设置默认方向
            this.Dir = Direction.Down;
            AttackSpeed = 60;
            ChangeDirSpeed = 70;
        }
        public override void Update()
        {
            MoveCheck();
            Move();
            AttackCheck();
            AutoChangeDirection();
            base.Update();
        }

        private void MoveCheck()
        {
            #region 检查有没有超出边界
            if (Dir == Direction.Up)
            {
                if (Y - Speed < 0)
                {
                    changeDirection(); return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y + Speed + Height > 450)
                {
                    changeDirection(); return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X - Speed < 0)
                {
                    changeDirection(); return;
                }

            }
            else if (Dir == Direction.Right)
            {
                if (X + Speed + Height > 450)
                {
                    changeDirection(); return;
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
                changeDirection(); return;
            }
            if (GameObjectManager.IsCollidedSteel(rect) != null)
            {
                changeDirection(); return;
            }
            if (GameObjectManager.IsCollidedBoss(rect) != false)
            {
                changeDirection(); return;
            }
            #endregion
        }

        private void changeDirection()
        {
            //有 4 种方向
            //Random r = new Random();   //随机数种子
            //算法 生成的伪随机数 ，使用时尽量使用同一颗种子
            while(true)
            {
                Direction dir = (Direction)r.Next(0, 4);  //通过算法保证 0 1 2 3 生成的随机数的出现保证是平均的
                if(Dir == dir)
                {
                    continue;
                }else
                {
                    Dir = dir;
                    break;
                }
            }
            MoveCheck();
        }
        private void Move()
        {
           

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

        private void AutoChangeDirection()
        {
            changeDirCount++;
            if (changeDirCount < ChangeDirSpeed) return;
            changeDirection();
            changeDirCount = 0;
        }

        private void AttackCheck()
        {
            attackCount++;
            if (attackCount < AttackSpeed) return;

            Attack();
            attackCount = 0;
        }

        //发射子弹函数
        private void Attack()
        {
            int x = this.X;
            int y = this.Y;

            switch (Dir)
            {
                case Direction.Up:
                    x = x + Width / 2;
                    break;
                case Direction.Down:
                    x = x + Width / 2;
                    y += Height;
                    break;
                case Direction.Left:
                    y = y + Height / 2;
                    break;
                case Direction.Right:
                    x += Width;
                    y = y + Height / 2;
                    break;
            }

            GameObjectManager.CreateBullet(x, y, Tag.EnemyTank, Dir);

        }
    }
}
