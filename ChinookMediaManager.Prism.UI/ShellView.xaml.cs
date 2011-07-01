using StructureMap;

namespace ChinookMediaManager.Prism.UI
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
