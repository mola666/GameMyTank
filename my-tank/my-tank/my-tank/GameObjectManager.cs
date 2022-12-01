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
    class GameObjectManager
    {
        //创建 墙、钢铁 的集合
        public static List<NotMovething> wallList = new List<NotMovething>(); //应该是创建了一个容器
        private static List<NotMovething> steelList = new List<NotMovething>();
        private static NotMovething boss;
        private static MyTank myTank;  //创建一个MyTank的变量

        private static List<EnemyTank> tankList = new List<EnemyTank>();

        private static List<Bullet> bulletList = new List<Bullet>();

        //爆炸效果集合
        private static List<Explosion> expList = new List<Explosion>();

        private static int enemyBornSpeed = 60;
        private static int enemyBornCount = 60;

        //敌人生成位置
        private static Point[] points = new Point[3];
        public static void Start()
        {
            //生成敌人位置
            points[0].X = 0; points[0].Y = 0;

            points[1].X = 7*30; points[1].Y = 0;

            points[2].X = 14 * 30; points[2].Y = 0;
        }

        public static void Update()
        {
            //1、生成墙、boss、铁墙、我的坦克
            foreach (NotMovething nm in wallList)
            {
                nm.Update();
            }
            foreach (NotMovething nm in steelList)
            {
                nm.Update();
            }
            boss.Update();
            myTank.Update();

            foreach(EnemyTank tank in tankList)
            {
                tank.Update();
            }
            //生成敌人
            EnemyBorn();

            //绘制子弹,并销毁子弹
            CheckAndDestroyBullet();
            foreach (Bullet bullet in bulletList)
            {
                bullet.Update();
            }

            //绘制爆炸效果
            CheckAndDestroyExplosion();
            foreach (Explosion exp in expList)
            {
                exp.Update();
            }

        }

        //创建爆炸效果
        public static void CreateExplosion(int x,int y)
        {
            Explosion exp = new Explosion(x,y);
            expList.Add(exp);
        }

        //销毁墙
        public static void DestroyWall(NotMovething wall)
        {
            wallList.Remove(wall);
        }
        //销毁坦克
        public static void DestroyTank(EnemyTank tank)
        {
            tankList.Remove(tank);
        }

        //创建子弹
        public static void CreateBullet(int x,int y,Tag tag,Direction dir)
        {
            Bullet bullet = new Bullet(x,y,5,dir,tag);
            bulletList.Add(bullet);
        }

        //检测子弹是否需要销毁,如果检测到我们就将它直接销毁
        private static void CheckAndDestroyBullet()
        {
            List<Bullet> needToDestroy = new List<Bullet>();
            foreach (Bullet bullet in bulletList)
            {
                if(bullet.IsDestroy == true)
                {
                    needToDestroy.Add(bullet);
                }
            }
            foreach (Bullet bullet in needToDestroy)
            {
                bulletList.Remove(bullet);
            }
        }

        //检测爆炸效果是否需要销毁，,如果检测到我们就将它直接销毁
        private static void CheckAndDestroyExplosion()
        {
            List<Explosion> needToDestroy = new List<Explosion>();
            foreach (Explosion exp in expList)
            {
                if (exp.IsNeedDestroy == true)
                {
                    needToDestroy.Add(exp);
                }
            }
            foreach (Explosion exp in needToDestroy)
            {
                expList.Remove(exp);
            }
        }

        private static void EnemyBorn()
        {
            enemyBornCount++;
            if (enemyBornCount < enemyBornSpeed) return;

            //敌人出生声音
            SoundMananger.PlayAdd();

            //生成敌人的位置
            //随机生成 0~2 个位置,生成的敌人坦克只能从那3个地方出生
            Random rd = new Random();  //Random 生成的随机数不包含最大值
            int index = rd.Next(0,3);
            Point postion = points[index];
            int enemyType = rd.Next(1, 5);
            switch(enemyType)
            {
                case 1:
                    CreateEnemyTank1(postion.X, postion.Y);
                    break;
                case 2:
                    CreateEnemyTank2(postion.X, postion.Y);
                    break;
                case 3:
                    CreateEnemyTank3(postion.X, postion.Y);
                    break;
                case 4:
                    CreateEnemyTank4(postion.X, postion.Y);
                    break;
            }

            enemyBornCount = 0;
        }

        #region 生成敌人坦克
        private static void CreateEnemyTank1(int x,int y)
        {
            EnemyTank tank = new EnemyTank(x,y,2,Resources.GrayDown, Resources.GrayUp, Resources.GrayRight, Resources.GrayLeft);
            tankList.Add(tank);
        }
        private static void CreateEnemyTank2(int x, int y)
        {
            EnemyTank tank = new EnemyTank(x, y, 2, Resources.GreenDown, Resources.GreenUp, Resources.GreenRight, Resources.GreenLeft);
            tankList.Add(tank);
        }
        private static void CreateEnemyTank3(int x, int y)
        {
            EnemyTank tank = new EnemyTank(x, y, 4, Resources.QuickDown, Resources.QuickUp, Resources.QuickRight, Resources.QuickLeft);
            tankList.Add(tank);
        }
        private static void CreateEnemyTank4(int x, int y)
        {
            EnemyTank tank = new EnemyTank(x, y, 1, Resources.SlowDown, Resources.SlowUp, Resources.SlowRight, Resources.SlowLeft);
            tankList.Add(tank);
        }

        #endregion

        #region 判断坦克是否与墙体发生碰撞
        public static NotMovething IsCollidedWall(Rectangle rt)
        {
            foreach(NotMovething wall in wallList)
            {
                if(wall.GetRectangle().IntersectsWith(rt))
                {
                    return wall;
                }
            }
            return null;
        }
        public static NotMovething IsCollidedSteel(Rectangle rt)
        {
            foreach (NotMovething wall in steelList)
            {
                if (wall.GetRectangle().IntersectsWith(rt))
                {
                    return wall;
                }
            }
            return null;
        }
        public static bool IsCollidedBoss(Rectangle rt)
        {
            return boss.GetRectangle().IntersectsWith(rt);
            /*
            if(boss.GetRectangle().IntersectsWith(rt))
            {
                return true;
            }
            return false;
            */
        }

        #endregion

        #region 判断我们的子弹是否与敌方坦克发生碰撞,与敌方的子弹是否与我们的坦克发生碰撞
        public static EnemyTank IsCollidedEnemyTank(Rectangle rt)
        {
            foreach (EnemyTank tank in tankList)
            {
                if (tank.GetRectangle().IntersectsWith(rt))
                {
                    return tank;
                }
            }
            return null;
        }

        public static MyTank IsCollidedMyTank(Rectangle rt)
        {
            if(myTank.GetRectangle().IntersectsWith(rt)) return myTank;
            return null;
        }


        #endregion

        /*
        public static void DrawMap()
        {
            foreach(NotMovething nm in wallList)
            {
                nm.DrawSelf();
            }
            foreach (NotMovething nm in steelList)
            {
                nm.DrawSelf();
            }
            boss.DrawSelf();
        }
        public static void DrawMyTank()
        {
            myTank.DrawSelf();
        }
        */

        public static void CreateMyTank()
        {
            int x = 5 * 30;
            int y = 14 * 30;
            myTank = new MyTank(x,y,2);//2是速度
        }

        public static void CreateMap()
        {
            CreateWall(1, 1, 5, Resources.wall, wallList);
            CreateWall(3, 1, 5, Resources.wall, wallList);
            CreateWall(5, 1, 4, Resources.wall, wallList);
            CreateWall(7, 1, 3, Resources.wall, wallList);
            CreateWall(9, 1, 4, Resources.wall, wallList);
            CreateWall(11, 1, 5, Resources.wall, wallList);
            CreateWall(13, 1, 5, Resources.wall, wallList);

            CreateWall(7, 5, 1, Resources.steel, steelList);

            CreateWall(0, 7, 1, Resources.steel, steelList);

            CreateWall(2, 7, 1, Resources.wall, wallList);
            CreateWall(3, 7, 1, Resources.wall, wallList);
            CreateWall(4, 7, 1, Resources.wall, wallList);
            CreateWall(6, 7, 1, Resources.wall, wallList);
            CreateWall(7, 6, 2, Resources.wall, wallList);
            CreateWall(8, 7, 1, Resources.wall, wallList);
            CreateWall(10, 7, 1, Resources.wall, wallList);
            CreateWall(11, 7, 1, Resources.wall, wallList);
            CreateWall(12, 7, 1, Resources.wall, wallList);

            CreateWall(14, 7, 1, Resources.steel, steelList);

            CreateWall(1, 9, 5, Resources.wall, wallList);
            CreateWall(3, 9, 5, Resources.wall, wallList);
            CreateWall(5, 9, 3, Resources.wall, wallList);

            CreateWall(6, 10, 1, Resources.wall, wallList);
            CreateWall(7, 10, 2, Resources.wall, wallList);
            CreateWall(8, 10, 1, Resources.wall, wallList);

            CreateWall(9, 9, 3, Resources.wall, wallList);
            CreateWall(11, 9, 5, Resources.wall, wallList);
            CreateWall(13, 9, 5, Resources.wall, wallList);


            CreateWall(6, 13, 2, Resources.wall, wallList);
            CreateWall(7, 13, 1, Resources.wall, wallList);
            CreateWall(8, 13, 2, Resources.wall, wallList);

            CreateBoss(7, 14, Resources.Boss);
        }
        private static void CreateBoss(int x, int y, Image img)
        {
            int xPosition = x * 30;
            int yPosition = y * 30;
            boss = new NotMovething(xPosition, yPosition, img);
        }

        //1、创建纵向的墙
        private static void CreateWall(int x, int y, int count, Image img, List<NotMovething> wallList)
        {

            int xPosition = x * 30;
            int yPosition = y * 30;
            for (int i = yPosition; i < yPosition + count * 30; i += 15)
            {
                NotMovething wall1 = new NotMovething(xPosition, i, img);  //创建了一个类，并初始化了它的构造函数
                NotMovething wall2 = new NotMovething(xPosition + 15, i, img);
                wallList.Add(wall1);
                wallList.Add(wall2);
            }
        }

        //配置两个按键事件
        public static void KeyDown(KeyEventArgs args)
        {
            myTank.KeyDown(args);
        }

        public static void KeyUp(KeyEventArgs args)
        {
            myTank.KeyUp(args);
        }
    }
}
