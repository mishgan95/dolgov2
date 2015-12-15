using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Environments.Actors
{
    public class Lighting : BaseActor, IShootable
    {
        private const int MaxSpeed = 5;
        private readonly Random random;
        private const int MinShootTime = 500;
        private const int MaxShootTime = 5000;
        protected Stopwatch m_shootTimer;
        private int shoottime;

        public Lighting(ILevelInfo info) : base(info)
        {
            Width = 50;
            Height = 50;
            ActorType = ActorType.Light;
            random = new Random();
            
        }
        public bool CanShoot { get; set; }     
        

        public override void Load()
        {
            Load(@"Assets\lighting.png");
            h_startShootTimer();
        }

        public BaseActor Shoot()
        {
            h_startShootTimer();
            var bullet = new LightingBullet(Info, this);
            bullet.Load();
            return bullet;
        }

        private void h_startShootTimer()
        {
            if (m_shootTimer == null)
            {
                CanShoot = false;
                m_shootTimer = new Stopwatch();
                shoottime = random.Next(MinShootTime, MaxShootTime);
                m_shootTimer.Start();
            }
        }

        public override void Update()
        {
            var degree = random.Next(0, 360);
            var degreerad = degree*Math.PI/180;   
            var point = new Point();
            var sin = Math.Sin(degreerad);
            var cos = Math.Cos(degreerad);
            point.X = (int)(Position.X + cos*MaxSpeed);
            point.Y = (int)(Position.Y + sin*MaxSpeed);
            Position = point;
            base.Update();

            if (!CanShoot)
                if (m_shootTimer.ElapsedMilliseconds > shoottime)
                {
                    m_shootTimer.Stop();
                    m_shootTimer = null;
                    CanShoot = true;
                }
        }

        public override bool IsAlive
        {
            get
            {
                return true;
            }

            set
            {
                base.IsAlive = true;
            }
        }        
    }
}
