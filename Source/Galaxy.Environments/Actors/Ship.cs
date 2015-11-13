#region using

using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using System.Diagnostics;
using System.Windows;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

#endregion

namespace Galaxy.Environments.Actors
{
    /// <summary>
    /// Класс для представления базового вражеского корабля. Зеленый.
    /// </summary>
    public class Ship : DethAnimationActor
    {
        #region Constant

        private const int MaxSpeed = 2;
        private const long StartFlyMs = 1000;

        #endregion

        #region Private fields

        private bool m_flying;
        protected Stopwatch m_flyTimer;
        protected Stopwatch m_shootTimer; // счетчик времени для стрельбы
        private string m_image; 
        private int m_shootInterval; 

        protected string ImageName
        {
            get { return m_image; }
            set { m_image = value; }
        }

        protected int ShootInterval
        {
            get { return m_shootInterval; }
            set { m_shootInterval = value; }
        }

        #endregion

        #region Public properties
        /// <summary>
        /// Может ли корабль стрелять.
        /// </summary>
        public bool CanShoot { get; protected set; }
        #endregion

        #region Constructors

        public Ship(ILevelInfo info) : base(info)
        {
            Width = 40;
            Height = 30;
            ActorType = ActorType.Enemy;
            ImageName = @"Assets\greenship.png";
            ShootInterval = 2000;
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
            /// Проверяем стрельбу
            if (!CanShoot)
                // проверяем, перезарядился ли он
                if (m_shootTimer.ElapsedMilliseconds > m_shootInterval)
                {
                    m_shootTimer.Stop();
                    m_shootTimer = null;
                    CanShoot = true;
                }
        }
        /// <summary>
        /// Провести выстрел.
        /// </summary>
        /// <returns>Пуля, выпущенная данным кораблем.</returns>
        public Bullet Shoot()
        {
            h_startShootTimer(); // запуск счетчика перезарядки
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
        /// <summary>
        /// Запустить счетчик перезарядки
        /// </summary>
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