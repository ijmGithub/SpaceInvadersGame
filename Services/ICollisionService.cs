using System.Collections.Generic;
using SpaceInvadersGame.Models;

namespace SpaceInvadersGame.Services
{
    public interface ICollisionService
    {
        bool HasCollision(ICollidable source, ICollidable target);

        IEnumerable<CollisionResult> DetectCollisions(IEnumerable<ICollidable> entities);

        IEnumerable<CollisionResult> DetectCollisions(IEnumerable<ICollidable> first, IEnumerable<ICollidable> second);
    }
}
