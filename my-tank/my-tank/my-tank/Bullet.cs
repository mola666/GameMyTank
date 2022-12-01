using my_tank.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_tank
{
    enum Tag
    {
        MyTank,
        EnemyTank
    }
    class Bullet:Movething
    {
        public Tag Tag { get; set; }

        //添加一个变量 IsDestroy 来表示 子弹是否需要销毁
        public bool IsDestroy { get; set; }

        public Bullet(int x, int y, int speed,Direction dir,Tag tag)
        {
            IsDestroy = false;
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            //这里为 Movething 里面的 坦克变量 进行赋值
            BitmapDown = Resources.BulletDown;
            BitmapUp = Resources.BulletUp;
            BitmapRight = Resources.BulletRight;
            BitmapLeft = Resources.BulletLeft;
            //设置默认方向
            this.Dir = dir;
            this.Tag = tag;

            //因为子弹图片也有大小，所以我们将子弹的位置重新规划一下
            this.X -= Width / 2;
            this.Y -= Height / 2;
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
                //因为我们是要判断的中间的 子弹 的中心 的位置
                if (Y+Height/2+3 < 0)
                {
                    IsDestroy = true; return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y +  Height / 2 - 3 > 450)
                {
                    IsDestroy = true; return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X + Width/2 - 3 < 0)
                {
                    IsDestroy = true; return;
                }

            }
            else if (Dir == Direction.Right)
            {
                if (X + Width/2 + 3 > 450)
                {
                    IsDestroy = true; return;
                }
            }
            #endregion

            #region 检查有没有和其他元素碰撞
            Rectangle rect = GetRectangle();

            rect.X = X + Width / 2 - 3;
            rect.Y = Y + Height / 2 - 3;
            //将现在的子弹的长宽设为 3，3
            rect.Width = 3;
            rect.Height = 3;

            //爆炸的中心位置
            int xExplosion = this.X + Width / 2;
            int yExplosion = this.Y + Height / 2;

            //1、墙  2、钢墙  3、坦克
            NotMovething wall = null;
            if ((wall = GameObjectManager.IsCollidedWall(rect)) != null)
            {
                IsDestroy = true;
                GameObjectManager.DestroyWall(wall);
                GameObjectManager.CreateExplosion(xExplosion, yExplosion);

                //子弹碰到墙的时候产生爆炸声音
                SoundMananger.PlayBlast();

                return;
            }
            if (GameObjectManager.IsCollidedSteel(rect) != null)
            {
                IsDestroy = true;
                return;
            }

            //检查我们的子弹有没有和 BOSS 发生碰撞，如果发生了碰撞，那么就是游戏结束了
            if (GameObjectManager.IsCollidedBoss(rect) != false)
            {
                //碰到 BOSS 的时候也会产生爆炸
                SoundMananger.PlayBlast();

                GameFramework.ChangeToGameOver(); return;
            }

            //有两种子弹，一种子弹是我们坦克的子弹，另一种子弹是敌人坦克的子弹
            if(Tag == Tag.MyTank)
            {
                EnemyTank tank = null;
                if ((tank = GameObjectManager.IsCollidedEnemyTank(rect)) != null)
                {
                    IsDestroy = true;
                    GameObjectManager.DestroyTank(tank);
                    GameObjectManager.CreateExplosion(xExplosion, yExplosion);
                    //声音
                    SoundMananger.PlayHit();
                    return;
                }
            }else if(Tag == Tag.EnemyTank)
            {
                MyTank myTank;
                if((myTank = GameObjectManager.IsCollidedMyTank(rect)) != null)
                {
                    IsDestroy = true;
                    GameObjectManager.CreateExplosion(xExplosion, yExplosion);
                    myTank.TakeDamage();
                }
            }
            #endregion
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
        
    }
}
