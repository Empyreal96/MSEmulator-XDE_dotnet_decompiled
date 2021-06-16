using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Language
{
	// Token: 0x02000614 RID: 1556
	internal class PSVariableAssignmentBinder : DynamicMetaObjectBinder
	{
		// Token: 0x0600438C RID: 17292 RVA: 0x00163B40 File Offset: 0x00161D40
		internal static PSVariableAssignmentBinder Get()
		{
			return PSVariableAssignmentBinder._binder;
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x00163B47 File Offset: 0x00161D47
		private PSVariableAssignmentBinder()
		{
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x00163B50 File Offset: 0x00161D50
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]);
			}
			object value = target.Value;
			if (value == null)
			{
				return new DynamicMetaObject(ExpressionCache.NullConstant, target.PSGetTypeRestriction()).WriteToDebugLog(this);
			}
			PSObject psobject = value as PSObject;
			if (psobject != null)
			{
				BindingRestrictions bindingRestrictions = BindingRestrictions.GetTypeRestriction(target.Expression, psobject.GetType());
				Expression expression = target.Expression;
				object baseObject = psobject.BaseObject;
				MemberExpression memberExpression = Expression.Property(expression.Cast(typeof(PSObject)), CachedReflectionInfo.PSObject_BaseObject);
				if (baseObject != null)
				{
					Type type = baseObject.GetType();
					bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetTypeRestriction(memberExpression, type));
					if (type.GetTypeInfo().IsValueType)
					{
						expression = PSVariableAssignmentBinder.GetExprForValueType(type, Expression.Convert(memberExpression, type), expression, ref bindingRestrictions);
					}
				}
				else
				{
					bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Equal(memberExpression, ExpressionCache.NullConstant)));
				}
				return new DynamicMetaObject(expression, bindingRestrictions).WriteToDebugLog(this);
			}
			Type type2 = value.GetType();
			if (type2.GetTypeInfo().IsValueType)
			{
				Expression expression2 = target.Expression;
				BindingRestrictions restrictions = target.PSGetTypeRestriction();
				expression2 = PSVariableAssignmentBinder.GetExprForValueType(type2, Expression.Convert(expression2, type2), expression2, ref restrictions);
				return new DynamicMetaObject(expression2, restrictions).WriteToDebugLog(this);
			}
			return new DynamicMetaObject(target.Expression, BindingRestrictions.GetExpressionRestriction(Expression.AndAlso(Expression.Not(Expression.TypeIs(target.Expression, typeof(ValueType))), Expression.Not(Expression.TypeIs(target.Expression, typeof(PSObject)))))).WriteToDebugLog(this);
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x00163CE0 File Offset: 0x00161EE0
		private static Expression GetExprForValueType(Type type, Expression convertedExpr, Expression originalExpr, ref BindingRestrictions restrictions)
		{
			bool flag = true;
			Expression result;
			if (PSVariableAssignmentBinder.MutableValueTypesWithInstanceMembers.ContainsKey(type))
			{
				MethodInfo method = CachedReflectionInfo.PSVariableAssignmentBinder_CopyInstanceMembersOfValueType.MakeGenericMethod(new Type[]
				{
					type
				});
				result = Expression.Call(method, convertedExpr, originalExpr);
				flag = false;
			}
			else if (PSVariableAssignmentBinder.IsValueTypeMutable(type))
			{
				ParameterExpression parameterExpression = Expression.Variable(type);
				result = Expression.Block(new ParameterExpression[]
				{
					parameterExpression
				}, new Expression[]
				{
					Expression.Assign(parameterExpression, convertedExpr),
					parameterExpression.Cast(typeof(object))
				});
			}
			else
			{
				result = originalExpr;
			}
			if (flag)
			{
				restrictions = restrictions.Merge(PSVariableAssignmentBinder.GetVersionCheck(PSVariableAssignmentBinder._mutableValueWithInstanceMemberVersion));
			}
			return result;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x00163D8C File Offset: 0x00161F8C
		public override T BindDelegate<T>(CallSite<T> site, object[] args)
		{
			object obj = args[0];
			if (obj != null && obj.GetType().GetTypeInfo().IsClass && !(obj is PSObject) && typeof(T) == typeof(Func<CallSite, object, object>))
			{
				T t = (T)((object)new Func<CallSite, object, object>(PSVariableAssignmentBinder.ObjectRule));
				base.CacheTarget<T>(t);
				return t;
			}
			return base.BindDelegate<T>(site, args);
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x00163DF8 File Offset: 0x00161FF8
		private static object ObjectRule(CallSite site, object obj)
		{
			if (!(obj is ValueType) && !(obj is PSObject))
			{
				return obj;
			}
			return ((CallSite<Func<CallSite, object, object>>)site).Update(site, obj);
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x00163E20 File Offset: 0x00162020
		internal static bool IsValueTypeMutable(Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			if (typeInfo.IsPrimitive || typeInfo.IsEnum)
			{
				return false;
			}
			if (type.GetFields(BindingFlags.Instance | BindingFlags.Public).Any<FieldInfo>())
			{
				return true;
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if (propertyInfo.CanWrite)
				{
					return true;
				}
			}
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] properties;
			return methods.Length != properties.Length;
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x00163E92 File Offset: 0x00162092
		internal static void NoteTypeHasInstanceMemberOrTypeName(Type type)
		{
			if (!type.GetTypeInfo().IsValueType || !PSVariableAssignmentBinder.IsValueTypeMutable(type))
			{
				return;
			}
			if (PSVariableAssignmentBinder.MutableValueTypesWithInstanceMembers.TryAdd(type, true))
			{
				PSVariableAssignmentBinder._mutableValueWithInstanceMemberVersion++;
			}
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x00163EC4 File Offset: 0x001620C4
		internal static object CopyInstanceMembersOfValueType<T>(T t, object boxedT) where T : struct
		{
			PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
			ConsolidatedString consolidatedString;
			if (PSObject.HasInstanceMembers(boxedT, out psmemberInfoInternalCollection) || PSObject.HasInstanceTypeName(boxedT, out consolidatedString))
			{
				PSObject psobject = PSObject.AsPSObject(boxedT);
				return PSObject.Base(psobject.Copy());
			}
			return t;
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x00163EFE File Offset: 0x001620FE
		internal static BindingRestrictions GetVersionCheck(int expectedVersionNumber)
		{
			return BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Field(null, CachedReflectionInfo.PSVariableAssignmentBinder__mutableValueWithInstanceMemberVersion), ExpressionCache.Constant(expectedVersionNumber)));
		}

		// Token: 0x040021AE RID: 8622
		private static readonly PSVariableAssignmentBinder _binder = new PSVariableAssignmentBinder();

		// Token: 0x040021AF RID: 8623
		private static int _mutableValueWithInstanceMemberVersion;

		// Token: 0x040021B0 RID: 8624
		private static readonly ConcurrentDictionary<Type, bool> MutableValueTypesWithInstanceMembers = new ConcurrentDictionary<Type, bool>();
	}
}
