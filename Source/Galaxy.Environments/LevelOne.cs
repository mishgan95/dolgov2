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
            // Backgrounds
            FileName = @"Assets\LevelOne.png";

            // Enemies
            // Добавление на уровень врагов
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
                var enemies = new List<Ship>();
                for (int i = 0; i < Actors.Count; i++)
                {
                    if (Actors[i].ActorType == ActorType.Enemy && Actors[i] is Ship)
                        enemies.Add(
                            (Ship)Actors[i]); // приводим объект типа BaseActor к типу Ship
                }
                /// Выстрелим
                foreach (var enemy in enemies)
                    if (enemy.CanShoot)
                        Actors.Add( // добавляем в коллекцию актеров уровня, чтобы пуля обрабатывалась движком
                            enemy.Shoot());
            }
            /// Атака игрока

            if (!IsPressed(VirtualKeyStates.Space)) return; 
            /// Выстрелить он сможет, если на уровне нет его пули
            bool isPlayerCanShoot = true;
            for (int i = 0; i < Actors.Count; i++)
                if (Actors[i] is Bullet && Actors[i].ActorType == ActorType.PlayerWeapon)
                {
                    isPlayerCanShoot = false;
                    break;
                }

            if (isPlayerCanShoot)
            {
                /// Стреляем
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
            // получим всех актеров, которые будут мертвы
            var killedActors = CollisionChecher.GetAllCollisions(Actors);
            foreach (var killedActor in killedActors) // для каждого умирающего актера
                if (killedActor.IsAlive)
                    killedActor.IsAlive = false; // установим, что он мертв
            // удалим из коллекции актеров уровня
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

        #endregion
    }
}