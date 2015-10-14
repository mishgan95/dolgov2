#region using

using System;
using System.Diagnostics;
using System.Windows;
using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

#endregion

namespace Galaxy.Environments.Actors
{
    public class Ship : DethAnimationActor
    {
        #region Constant

        private const int MaxSpeed = 2;
        private const long StartFlyMs = 1000;

        #endregion

        #region Private fields

        private bool m_flying;
        protected Stopwatch m_flyTimer;
        protected Stopwatch m_shootTimer;
        protected string m_image = @"Assets\greenship.png";
        protected int m_shootInterval = 2000;

        #endregion

        #region Public properties
        public bool CanShoot { get; protected set; }
        #endregion

        #region Constructors

        public Ship(ILevelInfo info) : base(info)
        {
            Width = 40;
            Height = 30;
            ActorType = ActorType.Enemy;
        }

        #endregion

        #region Overrides

        public override void Update()
        {
            base.Update();

            if (!IsAlive)
                return;

            if (!m_flying)
            {
                if (m_flyTimer.ElapsedMilliseconds > StartFlyMs)
                {
                    m_flyTimer.Stop();
                    m_flyTimer = null;
                    h_changePosition();
                    m_flying = true;
                }
            }
            else h_changePosition();
            if (!CanShoot)
                if (m_shootTimer.ElapsedMilliseconds > m_shootInterval)
                {
                    m_shootTimer.Stop();
                    m_shootTimer = null;
                    CanShoot = true;
                }
        }
        public Bullet Shoot()
        {
            h_startShootTimer();
            var bullet = new Bullet(Info, this);
            bullet.Load();
            return bullet;
        }

        #endregion

        #region Overrides

        public override void Load()
        {
            Load(m_image);
            if (m_flyTimer == null)
            {
                m_flyTimer = new Stopwatch();
                m_flyTimer.Start();
            }
            h_startShootTimer();
        }

        #endregion

        #region Private methods
        private void h_startShootTimer()
        {
            if (m_shootTimer == null)
            {
                CanShoot = false;
                m_shootTimer = new Stopwatch();
                m_shootTimer.Start();
            }
        }
        private void h_changePosition()
        {
            Point playerPosition = Info.GetPlayerPosition();

            Vector distance = new Vector(playerPosition.X - Position.X, playerPosition.Y - Position.Y);
            double coef = distance.X / MaxSpeed;
            coef *= 2; // для более плавного движения.

            Vector movement = Vector.Divide(distance, coef);

            Size levelSize = Info.GetLevelSize();

            if (movement.X > levelSize.Width)
                movement = new Vector(levelSize.Width, movement.Y);

            if (movement.X < 0 || double.IsNaN(movement.X))
                movement = new Vector(0, movement.Y);

            if (movement.Y > levelSize.Height)
                movement = new Vector(movement.X, levelSize.Height);

            if (movement.Y < 0 || double.IsNaN(movement.Y))
                movement = new Vector(movement.X, 0);

            Position = new Point((int)(Position.X + movement.X), (int)(Position.Y + movement.Y));
        }

        #endregion
    }
}