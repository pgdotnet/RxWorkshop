using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
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
        private readonly IService<FavQuery, FavResponse> _favService;
        private readonly ITopic<RepoNotification> _repoTopic;

        private ObservableCollection<Commit> _commits = new ObservableCollection<Commit>();

        public ObservableCollection<Commit> Commits
        {
            get { return _commits; }
            set { _commits = value; }
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

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        private string _property;

        public string Property
        {
            get { return _property; }
            set
            {
                _property = value;
                OnPropertyChanged();
            }
        }
        
        public MainWindowViewModel(IService<UserQuery, UserResponse> userService, IService<RepoQuery, RepoResponse> repoService, IService<FavQuery, FavResponse> favService, ITopic<RepoNotification> repoTopic)
        {
            _userService = userService;
            _repoService = repoService;
            _favService = favService;
            _repoTopic = repoTopic;

            _repoTopic.Messages
                .Select(n => n.Commit)
                .ObserveOnDispatcher()
                .Subscribe(Commits.Add);

            PropertyChangedStream.Where(p => p != "Property").Subscribe(p => Property = p);
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
            UserSearchResults.Clear();
            string user = parameter == null ? null : parameter.ToString();
            if (user != null)
                _userService.Query(new UserQuery(user))
                    .ObserveOnDispatcher()
                    .Subscribe(res => { res.Users.ForEach(UserSearchResults.Add); });
        }

        private void GetRepos(object parameter)
        {
            RepoSearchResults.Clear();
            User user = parameter as User;
            if (user != null)
                _repoService.Query(new RepoQuery(user))
                    .ObserveOn(DispatcherScheduler.Current)
                    .Subscribe(res => { res.Repos.ForEach(RepoSearchResults.Add); });
        }

        private void AddToFavourites(object parameter)
        {
            Repo repo = parameter as Repo;
            if (repo != null)
                _favService.Query(new FavQuery(repo))
                    .ObserveOn(DispatcherScheduler.Current)
                    .Subscribe();
        }
    }
}