#region using

using System;
using System.Drawing;
using Galaxy.Core.Actors;
using Galaxy.Core.Environment;

#endregion

namespace Galaxy.Environments.Actors
{
    public class PlayerShip : DethAnimationActor, IShootable
    {
        #region Constant

        private const int Speed = 3;

        #endregion

        #region Constructors

        public PlayerShip(ILevelInfo info)
          : base(info)
        {
            Width = 22;
            Height = 26;
            ActorType = ActorType.Player;
        }

        public bool CanShoot
        {
            get
            {
                if (!IsPressed(VirtualKeyStates.Space)) return false;
                return Info.HasPlayerBullet() == false;
            }
        }        
      

        #endregion

        #region Overrides

        public override void Load()
        {
            Load(@"Assets\player.png");
        }

        public BaseActor Shoot()
        {     
                var bullet = new PlayerBullet(Info, this); 
                bullet.Load(); 
                return bullet;            
        }

        #region Overrides of DethAnimationActor

        public override void Update()
        {
            base.Update();

            if (IsPressed(VirtualKeyStates.Left))
                Position = new Point(Position.X - Speed, Position.Y);
            if (IsPressed(VirtualKeyStates.Right))
                Position = new Point(Position.X + Speed, Position.Y);
            if (IsPressed(VirtualKeyStates.Up))
                Position = new Point(Position.X, Position.Y - Speed);
            if (IsPressed(VirtualKeyStates.Down))
                Position = new Point(Position.X, Position.Y + Speed);
        }

        #endregion

        #endregion
    }
}