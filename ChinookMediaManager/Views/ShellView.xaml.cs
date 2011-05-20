using ChinookMediaManager.ViewModels;

using StructureMap;

namespace ChinookMediaManager.Views
{
	public partial class ShellView 
	{
        public ShellView(IContainer container)
		{
		    InitializeComponent();
		    DataContext = container.GetInstance<ShellViewModel>();
		}
	}
}
