using System.Configuration;
using System.Windows;
using GHApp.Communication;
using GHApp.Contracts;
using Microsoft.Practices.Unity;

namespace GHApp.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var container = new UnityContainer();

			container.RegisterType<IUdpClientServer, UdpClientServer>(new ContainerControlledLifetimeManager());

			var serverAddress = ConfigurationManager.AppSettings.Get("ServerAddress");
			var serverPort = int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"));
			container.RegisterInstance<IChannelConfig>(ChannelNames.Server, new ChannelConfig { Address = serverAddress, Port = serverPort }, new ContainerControlledLifetimeManager());
			container.RegisterType<ICommunicationChannel, UdpCommunicationChannel>(
				ChannelNames.Server,
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => new UdpCommunicationChannel(c.Resolve<IUdpClientServer>(), c.Resolve<IChannelConfig>(ChannelNames.Server))));

			var clientAddress = ConfigurationManager.AppSettings.Get("ClientAddress");
			var clientPort = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort"));
			container.RegisterInstance<IChannelConfig>(ChannelNames.Client, new ChannelConfig { Address = clientAddress, Port = clientPort }, new ContainerControlledLifetimeManager());
			container.RegisterType<ICommunicationChannel, UdpCommunicationChannel>(
				ChannelNames.Client,
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => new UdpCommunicationChannel(c.Resolve<IUdpClientServer>(), c.Resolve<IChannelConfig>(ChannelNames.Client))));

			container.RegisterType(typeof(IService<,>), typeof(Service<,>), new ContainerControlledLifetimeManager());
			container.RegisterType(typeof(ITopic<>), typeof(Topic<>), new ContainerControlledLifetimeManager());

			//container.RegisterType<IInteraction, Interaction>(new ContainerControlledLifetimeManager());
			//container.RegisterType<ICommandProxy, CommandProxy>();
			//container.RegisterType<IMainWindowController, MainWindowController>(new ContainerControlledLifetimeManager());
			//container.RegisterType<IMainWindowViewModel, MainWindowViewModel>(new ContainerControlledLifetimeManager());
			container.RegisterType<MainWindow>(new ContainerControlledLifetimeManager());

			container.Resolve<MainWindow>().Show();
		}
	}
}