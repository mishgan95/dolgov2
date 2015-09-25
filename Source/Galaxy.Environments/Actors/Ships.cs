#region using

using System;
using System.Diagnostics;
using System.Windows;
using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

#endregion

namespace Galaxy.Environments.Actors
{
    public class Ships : Ship
    {
        public Ships(ILevelInfo info):base(info)
    {
            Width = 30;
            Height = 30;
            ActorType = ActorType.Enemy;
        }

        public override void Load()
        {
            base.Load();
            Load(@"Assets\shar.jpg");
        }
    }
}
