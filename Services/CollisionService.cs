using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SpaceInvadersGame.Models;

namespace SpaceInvadersGame.Services
{
    public class CollisionService : ICollisionService
    {
        public bool HasCollision(ICollidable source, ICollidable target)
        {
            if (source is null || target is null)
            {
                return false;
            }

            if (!source.IsActive || !target.IsActive)
            {
                return false;
            }

            return source.Bounds.IntersectsWith(target.Bounds);
        }

        public IEnumerable<CollisionResult> DetectCollisions(IEnumerable<ICollidable> entities)
        {
            var list = entities?.Where(e => e.IsActive).ToList() ?? new List<ICollidable>();

            for (var i = 0; i < list.Count; i++)
            {
                for (var j = i + 1; j < list.Count; j++)
                {
                    var first = list[i];
                    var second = list[j];

                    if (HasCollision(first, second))
                    {
                        yield return new CollisionResult(first, second);
                    }
                }
            }
        }

        public IEnumerable<CollisionResult> DetectCollisions(IEnumerable<ICollidable> first, IEnumerable<ICollidable> second)
        {
            var left = first?.Where(e => e.IsActive).ToList() ?? new List<ICollidable>();
            var right = second?.Where(e => e.IsActive).ToList() ?? new List<ICollidable>();

            foreach (var source in left)
            {
                foreach (var target in right)
                {
                    if (HasCollision(source, target))
                    {
                        yield return new CollisionResult(source, target);
                    }
                }
            }
        }
    }
}
