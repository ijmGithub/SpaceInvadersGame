using System;
using System.Collections.Generic;
using SpaceInvadersGame.Models;

namespace SpaceInvadersGame.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<ViewType, object> _viewModels;

        public NavigationService(object menuViewModel, object gameViewModel, object scoreViewModel)
        {
            _viewModels = new Dictionary<ViewType, object>
            {
                { ViewType.Menu, menuViewModel },
                { ViewType.Game, gameViewModel },
                { ViewType.Score, scoreViewModel }
            };

            CurrentViewType = ViewType.Menu;
            CurrentViewModel = _viewModels[CurrentViewType];
        }

        public event EventHandler? CurrentViewModelChanged;

        public object? CurrentViewModel { get; private set; }

        public ViewType CurrentViewType { get; private set; }

        public void NavigateTo(ViewType viewType)
        {
            if (CurrentViewType == viewType)
            {
                return;
            }

            if (!_viewModels.TryGetValue(viewType, out var viewModel))
            {
                return;
            }

            CurrentViewType = viewType;
            CurrentViewModel = viewModel;
            CurrentViewModelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
