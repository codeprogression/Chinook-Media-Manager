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

namespace ChinookMediaManager.Prism.Core.DynamicViewModel.Extensions
{
    public static class ExpressionExtensions
    {
        public static bool IsSimpleMemberAccess(this MemberExpression expression)
        {
            var result = (expression.NodeType == ExpressionType.MemberAccess);
            result &= (expression.Expression.NodeType == ExpressionType.Parameter);
            return result;
        }

        public static bool IsConstantMemberAccess(this MemberExpression expression)
        {
            var result = (expression.NodeType == ExpressionType.MemberAccess);
            result &= (expression.Expression.NodeType == ExpressionType.Constant);
            return result;
        }

        public static bool IsNestedMemberAccess(this MemberExpression expression)
        {
            var result = (expression != null);
            while (result && expression.Expression is MemberExpression)
            {
                result = (expression.NodeType == ExpressionType.MemberAccess);
                expression = (MemberExpression)expression.Expression;
            }
            result = result && (expression.Expression != null) && (expression.Expression.NodeType == ExpressionType.Parameter);
            return result;
        }

        public static MemberExpression GetMemberExpression<T>(this Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression = null;
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                    {
                        var body = (UnaryExpression)expression.Body;
                        memberExpression = body.Operand as MemberExpression;
                    }
                    break;
                case ExpressionType.MemberAccess:
                    memberExpression = expression.Body as MemberExpression;
                    break;
            }

            return memberExpression;
        }

        public static string GetName<T>(this Expression<Func<T, object>> property)
        {
            var expression = property.GetMemberExpression();
            return expression.Member.Name;
        }
    }
}