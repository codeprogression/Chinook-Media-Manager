using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace ChinookMediaManager.Prism.Core.Behaviors
{
    public class DataGridDoubleClickItemCommandBehavior : CommandBehaviorBase<DataGrid>
    {
        public DataGridDoubleClickItemCommandBehavior(DataGrid targetObject) : base(targetObject)
        {
            targetObject.MouseDoubleClick += (s, e) =>
            {
                var grid = e.Source as DataGrid;
                if (grid == null || grid.SelectedItem == null) return;
                CommandParameter = grid.SelectedItem;
                ExecuteCommand();
            };
        }
    }
    
    public static class DataGridDoubleClickItem
    {
        private static readonly DependencyProperty DataGridSelectedItemCommandBehaviorProperty
            = DependencyProperty.RegisterAttached(
                "DataGridDoubleClickItemCommandBehavior",
                typeof (DataGridDoubleClickItemCommandBehavior),
                typeof (DataGridDoubleClickItem),
                null);

        public static readonly DependencyProperty CommandProperty
            = DependencyProperty.RegisterAttached(
                "Command",
                typeof (ICommand),
                typeof (DataGridDoubleClickItem),
                new PropertyMetadata(OnSetCommandCallback));

        public static readonly DependencyProperty CommandParameterProperty
            = DependencyProperty.RegisterAttached(
                "CommandParameter",
                typeof (object),
                typeof (DataGridDoubleClickItem),
                new PropertyMetadata(OnSetCommandParameterCallback));

        public static ICommand GetCommand(DataGrid control)
        {
            return control.GetValue(CommandProperty) as ICommand;
        }

        public static void SetCommand(DataGrid control, ICommand command)
        {
            control.SetValue(CommandProperty, command);
        }

        public static void SetCommandParameter(DataGrid control, object parameter)
        {
            control.SetValue(CommandParameterProperty, parameter);
        }

        public static object GetCommandParameter(DataGrid control)
        {
            return control.GetValue(CommandParameterProperty);
        }

        private static void OnSetCommandCallback(DependencyObject dependencyObject,
                                                 DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as DataGrid;
            if (control == null) return;
            var behavior = GetOrCreateBehavior(control);
            behavior.Command = e.NewValue as ICommand;
        }

        private static void OnSetCommandParameterCallback(DependencyObject dependencyObject,
                                                          DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as DataGrid;
            if (control == null) return;
            var behavior = GetOrCreateBehavior(control);
            behavior.CommandParameter = e.NewValue;
        }

        private static DataGridDoubleClickItemCommandBehavior GetOrCreateBehavior(DataGrid control)
        {
            var behavior = control.GetValue(DataGridSelectedItemCommandBehaviorProperty)
                           as DataGridDoubleClickItemCommandBehavior;
            if (behavior == null)
            {
                behavior = new DataGridDoubleClickItemCommandBehavior(control);
                control.SetValue(DataGridSelectedItemCommandBehaviorProperty, behavior);
            }
            return behavior;
        }
    }
}