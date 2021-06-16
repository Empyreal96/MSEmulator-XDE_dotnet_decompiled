using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x02000606 RID: 1542
	internal class PSToObjectArrayBinder : DynamicMetaObjectBinder
	{
		// Token: 0x0600434B RID: 17227 RVA: 0x00162062 File Offset: 0x00160262
		internal static PSToObjectArrayBinder Get()
		{
			return PSToObjectArrayBinder._binder;
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x00162069 File Offset: 0x00160269
		private PSToObjectArrayBinder()
		{
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x00162071 File Offset: 0x00160271
		public override string ToString()
		{
			return "ToObjectArray";
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x0016208C File Offset: 0x0016028C
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, args);
			}
			if (target.Value is PSObject && PSObject.Base(target.Value) != target.Value)
			{
				return this.DeferForPSObject(new DynamicMetaObject[]
				{
					target
				}).WriteToDebugLog(this);
			}
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null)
			{
				return new DynamicMetaObject(Expression.NewArrayInit(typeof(object), new Expression[]
				{
					target.Expression.Cast(typeof(object))
				}), target.PSGetTypeRestriction()).WriteToDebugLog(this);
			}
			object obj = PSObject.Base(target.Value);
			if (obj is List<object>)
			{
				return new DynamicMetaObject(Expression.Call(PSEnumerableBinder.MaybeDebase(this, (Expression e) => e.Cast(typeof(List<object>)), target), CachedReflectionInfo.ObjectList_ToArray), PSEnumerableBinder.GetRestrictions(target)).WriteToDebugLog(this);
			}
			return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_ToArray, dynamicMetaObject.Expression), target.PSGetTypeRestriction()).WriteToDebugLog(this);
		}

		// Token: 0x04002195 RID: 8597
		private static readonly PSToObjectArrayBinder _binder = new PSToObjectArrayBinder();
	}
}
