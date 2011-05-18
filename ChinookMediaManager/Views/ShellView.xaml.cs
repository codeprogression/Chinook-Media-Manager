using ChinookMediaManager.ViewModels;
using Microsoft.Practices.Unity;

namespace ChinookMediaManager.Views
{
	public partial class ShellView 
	{
		public ShellView(IUnityContainer container)
		{
		    InitializeComponent();
		    DataContext = container.Resolve<ShellViewModel>();
		}
	}
}
