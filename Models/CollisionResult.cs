namespace SpaceInvadersGame.Models
{
    public sealed class CollisionResult
    {
        public CollisionResult(ICollidable source, ICollidable target)
        {
            Source = source;
            Target = target;
        }

        public ICollidable Source { get; }
        public ICollidable Target { get; }
    }
}
