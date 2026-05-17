using System;
using SpaceInvadersGame.Models;

namespace SpaceInvadersGame.Services
{
    public interface INavigationService
    {
        event EventHandler? CurrentViewModelChanged;

        object? CurrentViewModel { get; }

        ViewType CurrentViewType { get; }

        void NavigateTo(ViewType viewType);
    }
}
