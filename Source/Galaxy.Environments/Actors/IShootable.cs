using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galaxy.Core.Actors;

namespace Galaxy.Environments.Actors
{
    public interface IShootable
    {
        bool CanShoot { get; }
        BaseActor Shoot();
    }
}
