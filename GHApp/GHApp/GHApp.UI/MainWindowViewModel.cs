using System;
using System.Collections.Generic;
using System.Windows.Input;
using GHApp.Communication;
using GHApp.Contracts.Queries;
using GHApp.Contracts.Responses;

namespace GHApp.UI
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IService<UserQuery, UserResponse> _userService;

        private List<string> _favourites = new List<string>
        {
            "shanselman\\SuperRepo",
            "bartsokol\\WuPeEf",
            "nikodemrafalski\\eRiXy"
        };

        public List<string> Favourites
        {
            get { return _favourites; }
            set
            {
                _favourites = value;
                OnPropertyChanged();
            }
        }

        private ICommand _clickCommand;

        public ICommand ClickCommand
        {
            get { return _clickCommand ?? (_clickCommand = new ReactiveCommand(Click)); }

        }

        public MainWindowViewModel(IService<UserQuery, UserResponse> userService)
        {
            _userService = userService;
        }

        private void Click(object parameter)
        {
            _userService.Query(new UserQuery("alibaba")).Subscribe(x => { });
        }
    }
}