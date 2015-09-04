using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GHApp.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
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
	}
}
