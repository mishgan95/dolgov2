#region using

using Galaxy.Core.Environment;

#endregion

namespace Galaxy.Environments.Actors
{
    public class RedShip : Ship
    {
        public RedShip(ILevelInfo info) : base(info)
        {
            m_image = @"Assets\redship.png";
            m_shootInterval = 1000;
        }
    }
}