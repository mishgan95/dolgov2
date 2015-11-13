#region using

using Galaxy.Core.Environment;

#endregion

namespace Galaxy.Environments.Actors
{
    /// <summary>
    /// Класс для представления вражеского корабля. Красненький.
    /// </summary>
    public class RedShip : Ship
    {
        /// <summary>
        /// Базовый конструктор корабля.
        /// </summary>
        /// <param name="info">Информация об уровне.</param>
        public RedShip(ILevelInfo info) : base(info)
        {
            ImageName = @"Assets\redship.png";
            ShootInterval = 1000;
        }
    }
}