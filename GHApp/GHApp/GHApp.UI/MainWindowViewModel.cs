using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using GHApp.Communication;
using GHApp.Contracts.Dto;
using GHApp.Contracts.Notifications;
using GHApp.Contracts.Queries;
using GHApp.Contracts.Responses;

namespace GHApp.UI
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IService<UserQuery, UserResponse> _userService;
        private readonly ITopic<RepoNotification> _repoTopic;

        private ObservableCollection<FavouriteRepoViewModel> _favourites = new ObservableCollection<FavouriteRepoViewModel>();

        public ObservableCollection<FavouriteRepoViewModel> Favourites
        {
            get { return _favourites; }
            set { _favourites = value; }
        }

        private ObservableCollection<Repo> _searchResults = new ObservableCollection<Repo>();

        public ObservableCollection<Repo> SearchResults
        {
            get { return _searchResults; }
            set { _searchResults = value; }
        }

        public MainWindowViewModel(IService<UserQuery, UserResponse> userService, ITopic<RepoNotification> repoTopic)
        {
            _userService = userService;
            _repoTopic = repoTopic;

            _repoTopic.Messages
                .ObserveOnDispatcher()
                .Subscribe(x => Favourites.Add(new FavouriteRepoViewModel { NewCommitsCount = 0, Repo = new Repo { Name = "xxx" } }));
        }

        private ICommand _findUserCommand;

        public ICommand FindUserCommand
        {
            get { return _findUserCommand ?? (_findUserCommand = new ReactiveCommand(FindUser)); }
        }

        private void FindUser(object parameter)
        {
            string user = parameter?.ToString();
            if (user != null)
                _userService.Query(new UserQuery(user)).Subscribe(x => { });
        }
    }
}