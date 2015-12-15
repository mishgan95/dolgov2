using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Galaxy.Core.Actors;
using Galaxy.Core.Environment;

namespace Galaxy.Environments.Actors
{
    public class PlayerBullet : BaseActor
    {
        #region Constant

        protected int Speed { get; set; }
        #endregion

        #region Constructors

        public PlayerBullet(ILevelInfo info, BaseActor owner) : base(info)
        {
            Speed = 10;
            Width = 3;
            Height = 6;
            var point = new Point();
            point.X = owner.Position.X + owner.Width / 2;
            point.Y = owner.Position.Y;
            ActorType = ActorType.PlayerWeapon;
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
            int y = Position.Y - Speed;

            if (y < 0 
                || y > Info.GetLevelSize().Height)
                CanDrop = true;
            Position = new Point(Position.X, y);
        }

        #endregion
    }
}
