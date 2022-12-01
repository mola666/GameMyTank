using my_tank.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace my_tank
{
    class SoundMananger
    {
        public static SoundPlayer startPlayer = new SoundPlayer();
        public static SoundPlayer addPlayer = new SoundPlayer();
        public static SoundPlayer blastPlayer = new SoundPlayer();
        public static SoundPlayer firePlayer = new SoundPlayer();
        public static SoundPlayer hitPlayer = new SoundPlayer();
        public static void InitSound()
        {
            startPlayer.Stream = Resources.start;
            addPlayer.Stream = Resources.add;
            blastPlayer.Stream = Resources.blast;
            firePlayer.Stream = Resources.fire;
            hitPlayer.Stream = Resources.hit;
        }
        public static void PlayStart()
        {
            startPlayer.Play();
        }
        public static void PlayAdd()
        {
            addPlayer.Play();
        }
        public static void PlayBlast()
        {
            blastPlayer.Play();
        }
        public static void PlayFire()
        {
            firePlayer.Play();
        }
        public static void PlayHit()
        {
            hitPlayer.Play();
        }
    }
}
