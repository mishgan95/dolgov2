using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Environments.Actors
{
    public class LightingBullet : EnemyBullet
    {

        public LightingBullet(ILevelInfo info, BaseActor owner) : base (info, owner)
        {
            Speed = 20;
            Width = 30;
            Height = 30;
            ActorType = ActorType.LightWeapon;
        }

        public override void Load()
        {
            Load(@"Assets\lighting.png");
        }
    }
}
