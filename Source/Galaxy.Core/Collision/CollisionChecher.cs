#region using

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Galaxy.Core.Actors;

#endregion

namespace Galaxy.Core.Collision
{
    public static class CollisionChecher
    {
        #region Static Public methods

        public static IEnumerable<BaseActor> GetAllCollisions(List<BaseActor> allActors)//возвращает умершие актеры
        {
            List<BaseActor> collided = new List<BaseActor>();
            foreach (BaseActor actor in allActors)
            {
                var actorTmp = actor;
                collided.AddRange(allActors.Where(baseActor => h_collideCondition(baseActor, actorTmp)));//запоминает умерших актеров.
            }

            return collided;
        }

        #endregion

        #region Private methods

        private static bool h_collideCondition(BaseActor baseActor, BaseActor actorTmp) //Проверяет на касание актеров разных команд, если каснулись - умерли возвращают true, если не каснулись то false
        {
            var teamPlayer = new ActorType[] { ActorType.Player, ActorType.PlayerWeapon };
            // Contains показывает, содержит ли  коллекция teamPlayer тип актера baseActor.ActorType
            var isBaseActorPlayer = teamPlayer.Contains(baseActor.ActorType);
            // Contains показывает, содержит ли  коллекция teamPlayer тип актера actorTmp.ActorType
            var isActorTmpPlayer = teamPlayer.Contains(actorTmp.ActorType);
            return baseActor != actorTmp
                   && isBaseActorPlayer != isActorTmpPlayer
                   && h_collidedWith(actorTmp, baseActor);
        }

        private static bool h_collidedWith(BaseActor actor1, BaseActor actor2)
        {
            Rectangle rectangle1;
            {
                int actorX = (int)actor1.Position.X;
                int actorY = (int)actor1.Position.Y;
                rectangle1 = new Rectangle(actorX, actorY, actor1.Width, actor1.Height);
            }

            Rectangle rectangle2;
            {
                int actorX = (int)actor2.Position.X;
                int actorY = (int)actor2.Position.Y;
                rectangle2 = new Rectangle(actorX, actorY, actor2.Width, actor2.Height);
            }

            return rectangle1.IntersectsWith(rectangle2);//проверяет столкнулись или нет
        }

        #endregion
    }
}