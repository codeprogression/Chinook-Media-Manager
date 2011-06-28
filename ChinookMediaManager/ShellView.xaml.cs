using StructureMap;

namespace ChinookMediaManager
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
