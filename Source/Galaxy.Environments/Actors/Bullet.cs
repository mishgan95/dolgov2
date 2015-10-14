#region using

using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using System;
using System.Drawing;

#endregion

namespace Galaxy.Environments.Actors
{
    public class Bullet : BaseActor
    {
        #region Constant

        private const int Speed = 10;

        #endregion

        #region Constructors

        public Bullet(ILevelInfo info, BaseActor owner) : base(info)
        {
            Width = 3;
            Height = 6;
            ActorType = owner.ActorType;
            Point point = new Point();
            point.X = owner.Position.X + owner.Width / 2;
            point.Y = owner.Position.Y;
            switch (ActorType)
            {
                case ActorType.Enemy:
                    point.Y += owner.Height;
                    break;
            }
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
            int y = Position.Y;
            switch (ActorType)
            {
                case ActorType.Player:
                    y -= Speed;
                    break;
                case ActorType.Enemy:
                    y += Speed;
                    break;
                default:
                    throw new NotImplementedException();
            }
            Position = new Point(Position.X, y);
            if (y < 0 || y > Info.GetLevelSize().Height)
                CanDrop = true;
        }

        #endregion
    }
}