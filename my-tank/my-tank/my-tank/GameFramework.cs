using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_tank
{
    enum GameState
    {
        Running = 0,
        GameOver = 1
    }
    class GameFramework
    {
        public static Graphics g;  //这个是暂时的画布，我们后续的作画都是在这个结构体上做的

        private static GameState gameState = GameState.Running;
        public static void Start()
        {

            GameObjectManager.Start();
            GameObjectManager.CreateMap();
            GameObjectManager.CreateMyTank();
            //初始化声音
            SoundMananger.InitSound();
            SoundMananger.PlayStart();
        }
        public static void Update()
        {
            // FPS
            //GameObjectManager.DrawMap();
            //GameObjectManager.DrawMyTank();
            if(gameState == GameState.Running)
            {
                GameObjectManager.Update();
            }else if(gameState == GameState.GameOver)
            {
                GameOverUpDate();
            }
            
        }
        public static void GameOverUpDate()
        {
            Bitmap bmp = Properties.Resources.GameOver;
            bmp.MakeTransparent(Color.Black);
            int x = 450 / 2 - Properties.Resources.GameOver.Width/2;
            int y = 450 / 2 - Properties.Resources.GameOver.Width / 2;
            g.DrawImage(bmp, x,y);
        }
        public static void ChangeToGameOver()
        {
            gameState = GameState.GameOver;
        }
    }
}
