using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChinookMediaManager.Core.DynamicViewModel.Extensions
{
    public static class EnumerableExtensions
    {
        public static ObservableCollection<T> AsObservable<T>(this IEnumerable<T> list)
        {
            return new ObservableCollection<T>(list);
        }
    }
}