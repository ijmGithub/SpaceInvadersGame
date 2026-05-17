using System.Windows;

namespace SpaceInvadersGame.Models
{
    public interface ICollidable
    {
        int Id { get; }

        Rect Bounds { get; }

        bool IsActive { get; }
    }
}
