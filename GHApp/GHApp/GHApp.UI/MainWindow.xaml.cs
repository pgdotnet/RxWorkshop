using System;
using System.Collections.Generic;
using System.Windows;
using GHApp.Communication;
using GHApp.Contracts.Queries;
using GHApp.Contracts.Responses;
using Microsoft.Practices.Unity;

namespace GHApp.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IUnityContainer _container;

		public MainWindow(IUnityContainer container)
		{
			InitializeComponent();

			_container = container;

			DataContext = this;
		}

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
			}
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			_container.Resolve<IService<UserQuery, UserResponse>>().Query(new UserQuery("alibaba")).Subscribe(x => { });
		}
	}
}