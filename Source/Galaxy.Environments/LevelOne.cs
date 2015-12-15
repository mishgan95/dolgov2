#region using

using Galaxy.Core.Actors;
using Galaxy.Core.Collision;
using Galaxy.Core.Environment;
using Galaxy.Environments.Actors;
using System;
using System.Collections.Generic;
using System.Drawing;

#endregion

namespace Galaxy.Environments
{
    /// <summary>
    ///   The level class for Open Mario.  This will be the first level that the player interacts with.
    /// </summary>
    public class LevelOne : BaseLevel
    {
        private int m_frameCount;

        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="LevelOne" /> class.
        /// </summary>
        public LevelOne()
        {
            FileName = @"Assets\LevelOne.png";

            int positionX, positionY;

            for (int i = 0; i < 5; i++)
            {
                var ship = new Ship(this); 
                positionY = ship.Height + 10; 
                positionX = 150 + i * (ship.Width + 50);
                ship.Position = new Point(positionX, positionY);

                var redship = new RedShip(this); 
                positionY = redship.Height + 50; 
                positionX = 100 + i * (redship.Width + 50); 
                redship.Position = new Point(positionX, positionY);

                Actors.Add(ship);
                Actors.Add(redship);
            }

            Player = new PlayerShip(this);
            int playerPositionX = Size.Width / 2 - Player.Width / 2;
            int playerPositionY = Size.Height - Player.Height - 50;
            Player.Position = new Point(playerPositionX, playerPositionY);
            Actors.Add(Player);

            var lighting = new Lighting(this);
            int PositionX = Size.Width / 4 - lighting.Width / 2;
            int PositionY = Size.Height / 2 - lighting.Height / 2;
            lighting.Position = new Point(PositionX, PositionY);
            Actors.Add(lighting);
        }

        #endregion

        #region Overrides

        private void h_dispatchKey()
        {
            var shootable = new List<IShootable>();
            foreach (var actor in Actors)
            {
                if (actor is IShootable)
                {
                    shootable.Add((IShootable)actor);
                }
            }

            foreach (var actor in shootable)
            {
                if (actor.CanShoot)
                    Actors.Add(actor.Shoot());
            }
        }

        public override BaseLevel NextLevel()
        {
            return new StartScreen();
        }

        public override void Update()
        {
            m_frameCount++;
            h_dispatchKey();
            
            base.Update();

            var killedActors = CollisionChecher.GetAllCollisions(Actors);
            foreach (var killedActor in killedActors)
                if (killedActor.IsAlive)
                    killedActor.IsAlive = false;

            var allActors = Actors.ToArray();
            foreach (var actor in allActors)
                if (actor.CanDrop)
                    Actors.Remove(actor);

            if (Player.CanDrop)
                Failed = true;
            
            foreach (var actor in Actors)
                if (actor.ActorType == ActorType.Enemy)
                    return;

            Success = true;
        }

        public override bool HasPlayerBullet()
        {
            for (int i = 0; i < Actors.Count; i++)
                if (Actors[i].ActorType == ActorType.PlayerWeapon)
                {
                    return true;                   
                }
            return false;
        }

        #endregion
    }
}