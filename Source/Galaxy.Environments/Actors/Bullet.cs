#region using

using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using System;
using System.Drawing;

#endregion

namespace Galaxy.Environments.Actors
{
    /// <summary>
    /// Базовая пуля, которая летит вертикально.
    /// </summary>
    public class Bullet : BaseActor
    {
        #region Constant

        private const int Speed = 10;

        #endregion

        #region Constructors
        /// <summary>
        /// Базовый конструктор пульки.
        /// </summary>
        /// <param name="info">Информация о уровне.</param>
        /// <param name="owner">Тот актер, кто выстрелил данной пулей.</param>
        public Bullet(ILevelInfo info, BaseActor owner) : base(info)
        {
            Width = 3;
            Height = 6; 
            var point = new Point(); // создадим точку для начальной позиции пули
            point.X = owner.Position.X + owner.Width / 2; // пуля по X должна быть посередине стреляющего
            point.Y = owner.Position.Y; // установим позицию по Y равную позиции стреляющего
            // но если стреляющим является враг
            switch (owner.ActorType)
            {
                case ActorType.Player:
                    ActorType = ActorType.PlayerWeapon;
                    break;
                case ActorType.Enemy:
                    ActorType = ActorType.EnemyWeapon;
                    point.Y += owner.Height; // добавим к позицию по Y высоту стреляющего
                    break;
            }
            Position = point; // установим позицию пули равную ранее полученной точке
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
                case ActorType.PlayerWeapon: 
                    y -= Speed; 
                    break;
                case ActorType.EnemyWeapon: 
                    y += Speed; 
                    break;
                default:
                    throw new NotImplementedException();
            }
            /// Проверяем, не вылезает ли пуля за пределы карты
            if (y < 0 
                || y > Info.GetLevelSize().Height) 
                CanDrop = true; 
            Position = new Point(Position.X, y);
        }

        #endregion
    }
}