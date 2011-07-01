//Copyright 2010 LeaseTeam Inc. All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are
//permitted provided that the following conditions are met:
//
//   1. Redistributions of source code must retain the above copyright notice, this list of
//      conditions and the following disclaimer.
//
//   2. Redistributions in binary form must reproduce the above copyright notice, this list
//      of conditions and the following disclaimer in the documentation and/or other materials
//      provided with the distribution.
//
//
// http://blog.iovoodoo.com/2010/11/wpf-mvvm-made-simple-using-a-dynamic-view-model/

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ChinookMediaManager.Prism.Core.DynamicViewModel.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static Func<T, object> GetValueGetter<T>(this PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead)
            {
                throw new ArgumentException(String.Format("The property {0} on type {1} cannot be read.",
                                                          propertyInfo.Name, propertyInfo.DeclaringType.Name));
            }
            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var property = Expression.Property(instance, propertyInfo);
            var convert = Expression.TypeAs(property, typeof(object));
            return (Func<T, object>)Expression.Lambda(convert, instance).Compile();
        }

        public static Action<T, object> GetValueSetter<T>(this PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead)
            {
                throw new ArgumentException(String.Format("The property {0} on type {1} cannot be written.",
                                                          propertyInfo.Name, propertyInfo.DeclaringType.Name));
            }
            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var argument = Expression.Parameter(typeof(object), "a");
            var setterCall = Expression.Call(instance, propertyInfo.GetSetMethod(),
                                             Expression.Convert(argument, propertyInfo.PropertyType));
            return (Action<T, object>)Expression.Lambda(setterCall, instance, argument).Compile();
        }
    }
}