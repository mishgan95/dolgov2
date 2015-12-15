#region using

using Galaxy.Core.Environment;
using System.Windows;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

#endregion

namespace Galaxy.Environments.Actors
{
    public class RedShip : Ship
    {
        private const int MaxSpeed = 1;

        public RedShip(ILevelInfo info) : base(info)
        {
            ImageName = @"Assets\redship.png";
            ShootInterval = 1000;
        }

        protected override void h_changePosition()
        {
            var movement = GetMovement();
            Position = new Point((int)(Position.X - movement.X), (int)(Position.Y + movement.Y));
        }
    }
}