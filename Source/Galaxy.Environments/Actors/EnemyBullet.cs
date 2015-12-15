#region using

using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using System;
using System.Drawing;

#endregion

namespace Galaxy.Environments.Actors
{

    public class EnemyBullet : BaseActor
    {
        #region Constant
                
        protected int Speed { get; set; }
        #endregion

        #region Constructors

        public EnemyBullet(ILevelInfo info, BaseActor owner) : base(info)
        {
            Speed = 10;
            Width = 3; 
            Height = 6; 
            var point = new Point();
            point.X = owner.Position.X + owner.Width / 2;
            point.Y = owner.Position.Y + owner.Height;
            ActorType = ActorType.EnemyWeapon;
            Position = point;
        }

        #endregion

        #region Overrides

        public override void Load()
        {
            Load(@"Assets\bullet.png");
        }

        public override void Update()
        {
            int y = Position.Y + Speed;
            
            if (y < 0 
                || y > Info.GetLevelSize().Height) 
                CanDrop = true; 
            Position = new Point(Position.X, y); 
        }

        #endregion
    }
}