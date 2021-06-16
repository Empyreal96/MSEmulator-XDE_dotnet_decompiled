using System;
using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x02000607 RID: 1543
	internal class PSPipeWriterBinder : DynamicMetaObjectBinder
	{
		// Token: 0x06004351 RID: 17233 RVA: 0x001621AF File Offset: 0x001603AF
		internal static PSPipeWriterBinder Get()
		{
			return PSPipeWriterBinder._binder;
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x001621B6 File Offset: 0x001603B6
		private PSPipeWriterBinder()
		{
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x001621BE File Offset: 0x001603BE
		public override string ToString()
		{
			return "PipelineWriter";
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x001621C8 File Offset: 0x001603C8
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, args);
			}
			if (target.Value == AutomationNull.Value)
			{
				return new DynamicMetaObject(Expression.Block(typeof(void), new Expression[]
				{
					Expression.Call(CachedReflectionInfo.PipelineOps_Nop, new Expression[0])
				}), BindingRestrictions.GetInstanceRestriction(target.Expression, AutomationNull.Value)).WriteToDebugLog(this);
			}
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null)
			{
				DynamicMetaObject dynamicMetaObject2 = PSVariableAssignmentBinder.Get().Bind(target, new DynamicMetaObject[0]);
				BindingRestrictions restrictions = target.LimitType.GetTypeInfo().IsValueType ? dynamicMetaObject2.Restrictions : target.PSGetTypeRestriction();
				return new DynamicMetaObject(Expression.Call(args[0].Expression, CachedReflectionInfo.Pipe_Add, new Expression[]
				{
					dynamicMetaObject2.Expression.Cast(typeof(object))
				}), restrictions).WriteToDebugLog(this);
			}
			bool b = !(PSObject.Base(target.Value) is IEnumerator);
			return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_WriteEnumerableToPipe, dynamicMetaObject.Expression, args[0].Expression, args[1].Expression, ExpressionCache.Constant(b)), dynamicMetaObject.Restrictions).WriteToDebugLog(this);
		}

		// Token: 0x04002197 RID: 8599
		private static readonly PSPipeWriterBinder _binder = new PSPipeWriterBinder();
	}
}
