// Based on http://blog.iovoodoo.com/2010/11/wpf-mvvm-made-simple-using-a-dynamic-view-model/
//
// Copyright 2010 LeaseTeam Inc. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//
//   1. Redistributions of source code must retain the above copyright notice, this list of
//      conditions and the following disclaimer.
//
//   2. Redistributions in binary form must reproduce the above copyright notice, this list
//      of conditions and the following disclaimer in the documentation and/or other materials
//      provided with the distribution.
//
//  05/18/2011  Removed requirement for wrapped type to implement INotifyPropertyChanged. 
//              I added this interface/behavior to an NHibernate interceptor instead so NHibernate can fire the changed events.
//              Checking for interface before subscribing/unsubscribing to property changed events.
//
//      --richard, CodeProgression
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace ChinookMediaManager.Core.DynamicViewModel
{
    public abstract class ViewModelProxy<VIEWMODEL, MODEL> : DynamicObject, INotifyPropertyChanged where MODEL : class, new()
    {
        protected readonly MODEL EmptyEntity = new MODEL();
        private readonly Lazy<ConcurrentDictionary<string, Tuple<Func<MODEL, object>, Action<MODEL, object>>>> _propertyMap;
        private dynamic _properties;
        private MODEL _entity;

        protected ViewModelProxy()
        {
            _propertyMap =
                new Lazy<ConcurrentDictionary<string, Tuple<Func<MODEL, object>, Action<MODEL, object>>>>(
                    () => new ConcurrentDictionary<string, Tuple<Func<MODEL, object>, Action<MODEL, object>>>());
            AddProperty("HasCurrent", p => HasCurrent);
            ConfigurePropertyMap();
        }

        protected ViewModelProxy(MODEL entity) : this()
        {
            Entity = entity;
        }

        /// <summary>
        /// Run after property map has been configured. 
        /// Used to expose properties from the domain model to the view model (with overrideable setters)
        /// </summary>
        /// <example>Simple Property: <![CDATA[AddProperty(p => p.Id);]]></example>
        /// <example>Property with set interceptor: <![CDATA[AddProperty("Description", p => p.Description, (p,v) => SetDescription((string)v);]]></example>
        /// <example>Viewmodel data: <![CDATA[AddProperty("Contacts", p => p.Service.GetContacts(p.Id));]]></example>
        protected abstract void ConfigurePropertyMap();

        protected virtual MODEL Entity
        {
            get { return _entity ?? EmptyEntity; }
            set
            {
                if (value == _entity) return;
                
                if ((_entity as INotifyPropertyChanged) != null)
                    ((INotifyPropertyChanged) _entity).PropertyChanged -= RedirectPropertyChanged;
                
                _entity = value;
                
                if ((_entity as INotifyPropertyChanged) != null)
                    ((INotifyPropertyChanged) _entity).PropertyChanged += RedirectPropertyChanged;

                foreach (var property in PropertyMap.Keys)
                    OnPropertyChanged(property);
            }
        }

        protected ConcurrentDictionary<string, Tuple<Func<MODEL, object>, Action<MODEL, object>>> PropertyMap
        {
            get { return _propertyMap.Value; }
        }

        protected bool HasCurrent
        {
            get { return _entity != null; }
        }

        protected dynamic DynamicProperties
        {
            get { return _properties ?? (_properties = this); }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return PropertyMap.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var propertyName = binder.Name;
            result = null;
            var success = false;
            if (PropertyMap.ContainsKey(propertyName))
            {
                var value = PropertyMap[propertyName].Item1;
                if (Entity != null)
                {
                    result = value.Invoke(Entity);
                }
                success = true;
            }

            return success;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var propertyName = binder.Name;
            var success = false;
            if (PropertyMap.ContainsKey(propertyName))
            {
                var setExpression = PropertyMap[propertyName].Item2;
                if (setExpression != null && Entity != null)
                {
                    setExpression.Invoke(Entity, value);
                    OnPropertyChanged(propertyName);
                    success = true;
                }
            }

            return success;
        }

        /// <summary>
        /// Add a simple property of the domain object to the view model, 
        /// it will infer a setter expression from the provided get expression
        /// </summary>
        protected void AddProperty(Expression<Func<MODEL, object>> expression)
        {
            var memberExpression = expression.GetMemberExpression();
            if (!memberExpression.IsSimpleMemberAccess())
            {
                throw new ArgumentException(
                    @"This overload only works with simple member access expressions, to add a more complex expression, assign it an alias.",
                    "expression");
            }
            
            var propertyInfo = (PropertyInfo) memberExpression.Member;
            var propertyName = propertyInfo.Name;
            var getExpression = propertyInfo.CanRead ? propertyInfo.GetValueGetter<MODEL>() : null;
            var setExpression = propertyInfo.CanWrite ? propertyInfo.GetValueSetter<MODEL>() : null;
            AddProperty(propertyName, getExpression, setExpression);
        }

        /// <summary>
        /// Add an aliased expression of the domain object to the view model, 
        /// if it is a simple or nested property access, it will infer a setter
        /// expression from the provided get expression
        /// </summary>
        protected void AddProperty(string propertyAlias, Expression<Func<MODEL, object>> expression)
        {
            Action<MODEL, object> setExpression = null;
            var memberExpression = expression.GetMemberExpression();
            if (memberExpression.IsNestedMemberAccess())
            {
                var property = (PropertyInfo) memberExpression.Member;
                var parentExpression =
                    Expression.Lambda<Func<MODEL, object>>(memberExpression.Expression, expression.Parameters).Compile();
                setExpression = (p, v) => property.SetValue(parentExpression.Invoke(p), v, null);
            }
            AddProperty(propertyAlias, expression.Compile(), setExpression);
        }

        /// <summary>
        /// Add an aliased property of the domain object to the view model, 
        /// can specify any get and any set operation.
        /// </summary>
        protected void AddProperty(string propertyAlias, Expression<Func<MODEL, object>> getExpression,
                                   Expression<Action<MODEL, object>> setExpression)
        {
            AddProperty(propertyAlias, getExpression.Compile(), setExpression.Compile());
        }

      

        private void AddProperty(string key, Func<MODEL, object> getExpression, Action<MODEL, object> setExpression)
        {
            var success = PropertyMap.TryAdd(key, Tuple.Create(getExpression, setExpression));
            if (!success)
            {
                throw new InvalidOperationException(
                    String.Format("The specified item, {0}, has already been added to this view model's properties.",
                                  key));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RedirectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void NotifyPropertyChanged(params Expression<Func<VIEWMODEL, object>>[] properties)
        {
            foreach (var property in properties)
            {
                OnPropertyChanged(property.GetName());
            }
        }
    }
}