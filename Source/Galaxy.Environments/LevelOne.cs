#region using

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Galaxy.Core.Actors;
using Galaxy.Core.Collision;
using Galaxy.Core.Environment;
using Galaxy.Environments.Actors;

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
            // Backgrounds
            FileName = @"Assets\LevelOne.png";

            // Enemies
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

            // Player
            Player = new PlayerShip(this);
            int playerPositionX = Size.Width / 2 - Player.Width / 2;
            int playerPositionY = Size.Height - Player.Height - 50;
            Player.Position = new Point(playerPositionX, playerPositionY);
            Actors.Add(Player);
        }

        #endregion

        #region Overrides

        private void h_dispatchKey()
        {
            if (m_frameCount % 10 == 0)
            {
                var enemies = Actors.OfType<Ship>().Where(p => p.ActorType == ActorType.Enemy && p.CanShoot).ToArray();
                foreach (var enemy in enemies)
                    Actors.Add(enemy.Shoot());
            }
            if (!IsPressed(VirtualKeyStates.Space)) return;

            //if (m_frameCount % 10 != 0) return;
            if (!Actors.Any(p => p.GetType() == typeof(Bullet) && p.ActorType == ActorType.Player))
            {
                var bullet = new Bullet(this, Player);
                bullet.Load();
                Actors.Add(bullet);
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

            var killedActors = CollisionChecher.GetAllCollisions(Actors).Where(p => p.IsAlive).ToList();

            foreach (var killedActor in killedActors)
                killedActor.IsAlive = false;

            List<BaseActor> canDropActors = Actors.Where(actor => actor.CanDrop).ToList();

            foreach (var actor in canDropActors)
                Actors.Remove(actor);

            if (Player.CanDrop)
                Failed = true;

            if (Actors.All(actor => actor.ActorType != ActorType.Enemy))
                Success = true;
        }

        #endregion
    }
}