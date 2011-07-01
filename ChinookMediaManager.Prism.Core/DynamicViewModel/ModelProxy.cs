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
//  This model has been heavily modified for my use  --Richard, CodeProgression
//
// http://blog.iovoodoo.com/2010/11/wpf-mvvm-made-simple-using-a-dynamic-view-model/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using ChinookMediaManager.Prism.Core.DynamicViewModel.Extensions;

namespace ChinookMediaManager.Prism.Core.DynamicViewModel
{
    public abstract class ModelProxy<VIEWMODEL, MODEL> : DynamicObject, INotifyPropertyChanged where MODEL : class, new()
    {
        protected readonly MODEL EmptyEntity = new MODEL();

        private readonly Lazy<ConcurrentDictionary<string, Tuple<Func<MODEL, object>, Action<MODEL, object>>>> _propertyMap;
        private MODEL _wrappedEntity;

        protected virtual bool CanDoRefresh() { return true; }
        protected ModelProxy()
        {
           _propertyMap =
                new Lazy<ConcurrentDictionary<string, Tuple<Func<MODEL, object>, Action<MODEL, object>>>>(
                    () => new ConcurrentDictionary<string, Tuple<Func<MODEL, object>, Action<MODEL, object>>>());
            AddProperty("HasCurrent", p => HasCurrent);
            ConfigurePropertyMap();
        }

        /// <summary>
        /// Run after property map has been configured. 
        /// Used to expose properties from the domain model to the view model (with overrideable setters)
        /// </summary>
        /// <code>
        /// <![CDATA[
        /// //Simple properties
        /// AddProperty(p => p.AccountId);
        /// AddProperty(p => p.Description);
        /// AddProperty(p => p.PaymentMethod);
        /// AddProperty(p => p.PostingDate);
        /// AddProperty(p => p.PaymentReference);
        /// 
        /// //Properties with set interceptors
        /// AddProperty("PaymentAmount", p => p.PaymentAmount, (p, v) => SetPaymentAmount((decimal?)v));
        /// AddProperty("CustomerId", p => p.CustomerId, (p, v) => SetCustomer((long)v));
        /// 
        /// //ViewModel data
        /// AddProperty("SubItems", p => GetSubItems());
        /// AddProperty("Customers", p => LookupService.GetCustomerLookup("", ""));
        /// AddProperty("PaymentMethods", p => LookupService.GetValues("PAYMENT_TYPE"));
        /// AddProperty("InvoiceNumber", p => InvoiceNumber);
        /// ]]>
        /// </code>
        protected abstract void ConfigurePropertyMap();

        protected virtual MODEL Entity
        {
            get { return _wrappedEntity ?? EmptyEntity; }
            set
            {
                if (value == _wrappedEntity) return;

                if ((_wrappedEntity as INotifyPropertyChanged) != null)
                {
                    ((INotifyPropertyChanged) _wrappedEntity).PropertyChanged -= RedirectPropertyChanged;
                }

                _wrappedEntity = value;

                if ((_wrappedEntity as INotifyPropertyChanged) != null)
                {
                    ((INotifyPropertyChanged) _wrappedEntity).PropertyChanged += RedirectPropertyChanged;
                }

                foreach (string property in PropertyMap.Keys)
                {
                    OnPropertyChanged(property);
                }
            }
        }

        protected ConcurrentDictionary<string, Tuple<Func<MODEL, object>, Action<MODEL, object>>> PropertyMap
        {
            get { return _propertyMap.Value; }
        }

        protected bool HasCurrent
        {
            get { return _wrappedEntity != null; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return PropertyMap.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string propertyName = binder.Name;
            result = null;
            bool success = false;
            if (PropertyMap.ContainsKey(propertyName))
            {
                Func<MODEL, object> value = PropertyMap[propertyName].Item1;
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
            string propertyName = binder.Name;
            bool success = false;
            if (PropertyMap.ContainsKey(propertyName))
            {
                Action<MODEL, object> setExpression = PropertyMap[propertyName].Item2;
                if (setExpression != null)
                {
                    if (Entity != null)
                    {
                        setExpression.Invoke(Entity, value);
                        OnPropertyChanged(propertyName);
                        success = true;
                    }
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
            MemberExpression memberExpression = expression.GetMemberExpression();
            if (!memberExpression.IsSimpleMemberAccess())
            {
                throw new ArgumentException(
                    @"This overload only works with simple member access expressions, to add a more complex expression, assign it an alias.",
                    "expression");
            }

            var propertyInfo = (PropertyInfo) memberExpression.Member;
            string propertyName = propertyInfo.Name;
            Func<MODEL, object> getExpression = propertyInfo.CanRead ? propertyInfo.GetValueGetter<MODEL>() : null;
            Action<MODEL, object> setExpression = propertyInfo.CanWrite ? propertyInfo.GetValueSetter<MODEL>() : null;
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
            MemberExpression memberExpression = expression.GetMemberExpression();
            if (memberExpression.IsNestedMemberAccess())
            {
                var property = (PropertyInfo) memberExpression.Member;
                Func<MODEL, object> parentExpression =
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
            bool success = PropertyMap.TryAdd(key, Tuple.Create(getExpression, setExpression));
            if (!success)
            {
                throw new InvalidOperationException(
                    String.Format("The specified item, {0}, has already been added to this view model's properties.",
                                  key));
            }
        }

        protected void RedirectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                RaiseCanExecuteEvents();
            }
        }

        protected virtual void NotifyPropertyChanged(params Expression<Func<VIEWMODEL, object>>[] properties)
        {
            foreach (var property in properties)
            {
                OnPropertyChanged(property.GetName());
            }
        }


        protected void SetProperty(string propertyName, object value)
        {
            var setMethod = GetType().GetMethod("set_"+propertyName, BindingFlags.NonPublic|BindingFlags.Instance);
            setMethod.Invoke(this, new[] { value });

        }
        protected virtual void RaiseCanExecuteEvents()
        {
        }
    }
}