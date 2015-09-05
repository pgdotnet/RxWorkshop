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
        private readonly IService<RepoQuery, RepoResponse> _repoService;
        private readonly ITopic<RepoNotification> _repoTopic;

        private ObservableCollection<FavouriteRepoViewModel> _favourites = new ObservableCollection<FavouriteRepoViewModel>();

        public ObservableCollection<FavouriteRepoViewModel> Favourites
        {
            get { return _favourites; }
            set { _favourites = value; }
        }

        private ObservableCollection<User> _userSearchResults = new ObservableCollection<User>();

        public ObservableCollection<User> UserSearchResults
        {
            get { return _userSearchResults; }
            set { _userSearchResults = value; }
        }

        private ObservableCollection<Repo> _repoSearchResults = new ObservableCollection<Repo>();

        public ObservableCollection<Repo> RepoSearchResults
        {
            get { return _repoSearchResults; }
            set { _repoSearchResults = value; }
        }

        public MainWindowViewModel(IService<UserQuery, UserResponse> userService, IService<RepoQuery, RepoResponse> repoService, ITopic<RepoNotification> repoTopic)
        {
            _userService = userService;
            _repoService = repoService;
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

        private ICommand _getReposCommand;

        public ICommand GetReposCommand
        {
            get { return _getReposCommand ?? (_getReposCommand = new ReactiveCommand(GetRepos)); }
        }

        private ICommand _addToFavouritesCommand;

        public ICommand AddToFavouritesCommand
        {
            get { return _addToFavouritesCommand ?? (_addToFavouritesCommand = new ReactiveCommand(AddToFavourites)); }
        }

        private void FindUser(object parameter)
        {
            string user = parameter == null ? null : parameter.ToString();
            if (user != null)
                _userService.Query(new UserQuery(user))
                    .ObserveOnDispatcher()
                    .Subscribe(res => { res.Users.ForEach(UserSearchResults.Add); });
        }

        private void GetRepos(object parameter)
        {
            string user = parameter == null ? null : parameter.ToString();
            if (user != null)
                _repoService.Query(new RepoQuery(user)).Subscribe(x => { });
        }

        private void AddToFavourites(object parameter)
        {
            Repo repo = parameter as Repo;
            if (repo != null)
                ;
        }
    }
}