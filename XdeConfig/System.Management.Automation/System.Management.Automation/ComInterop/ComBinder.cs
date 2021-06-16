using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Language;
using System.Reflection;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A52 RID: 2642
	internal static class ComBinder
	{
		// Token: 0x060069A5 RID: 27045 RVA: 0x002141D1 File Offset: 0x002123D1
		public static bool IsComObject(object value)
		{
			return value != null && !WinRTHelper.IsWinRTType(value.GetType()) && ComObject.IsComObject(value);
		}

		// Token: 0x060069A6 RID: 27046 RVA: 0x002141EB File Offset: 0x002123EB
		public static bool CanComBind(object value)
		{
			return ComBinder.IsComObject(value) || value is IPseudoComObject;
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x00214200 File Offset: 0x00212400
		public static bool TryBindGetMember(GetMemberBinder binder, DynamicMetaObject instance, out DynamicMetaObject result, bool delayInvocation)
		{
			if (ComBinder.TryGetMetaObject(ref instance))
			{
				ComBinder.ComGetMemberBinder binder2 = new ComBinder.ComGetMemberBinder(binder, delayInvocation);
				result = instance.BindGetMember(binder2);
				if (result.Expression.Type.IsValueType)
				{
					result = new DynamicMetaObject(Expression.Convert(result.Expression, typeof(object)), result.Restrictions);
				}
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x00214264 File Offset: 0x00212464
		public static bool TryBindGetMember(GetMemberBinder binder, DynamicMetaObject instance, out DynamicMetaObject result)
		{
			return ComBinder.TryBindGetMember(binder, instance, out result, false);
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x0021426F File Offset: 0x0021246F
		public static bool TryBindSetMember(SetMemberBinder binder, DynamicMetaObject instance, DynamicMetaObject value, out DynamicMetaObject result)
		{
			if (ComBinder.TryGetMetaObject(ref instance))
			{
				result = instance.BindSetMember(binder, value);
				result = new DynamicMetaObject(result.Expression, result.Restrictions.Merge(value.PSGetMethodArgumentRestriction()));
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x002142AA File Offset: 0x002124AA
		public static bool TryBindInvoke(InvokeBinder binder, DynamicMetaObject instance, DynamicMetaObject[] args, out DynamicMetaObject result)
		{
			if (ComBinder.TryGetMetaObjectInvoke(ref instance))
			{
				result = instance.BindInvoke(binder, args);
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x002142D4 File Offset: 0x002124D4
		public static bool TryBindInvokeMember(InvokeMemberBinder binder, bool isSetProperty, DynamicMetaObject instance, DynamicMetaObject[] args, out DynamicMetaObject result)
		{
			if (ComBinder.TryGetMetaObject(ref instance))
			{
				ComBinder.ComInvokeMemberBinder binder2 = new ComBinder.ComInvokeMemberBinder(binder, isSetProperty);
				result = instance.BindInvokeMember(binder2, args);
				BindingRestrictions restrictions = args.Aggregate(BindingRestrictions.Empty, (BindingRestrictions current, DynamicMetaObject arg) => current.Merge(arg.PSGetMethodArgumentRestriction()));
				BindingRestrictions restrictions2 = result.Restrictions.Merge(restrictions);
				if (result.Expression.Type.IsValueType)
				{
					result = new DynamicMetaObject(Expression.Convert(result.Expression, typeof(object)), restrictions2);
				}
				else
				{
					result = new DynamicMetaObject(result.Expression, restrictions2);
				}
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x060069AC RID: 27052 RVA: 0x00214386 File Offset: 0x00212586
		public static bool TryBindGetIndex(GetIndexBinder binder, DynamicMetaObject instance, DynamicMetaObject[] args, out DynamicMetaObject result)
		{
			if (ComBinder.TryGetMetaObjectInvoke(ref instance))
			{
				result = instance.BindGetIndex(binder, args);
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x060069AD RID: 27053 RVA: 0x002143A4 File Offset: 0x002125A4
		public static bool TryBindSetIndex(SetIndexBinder binder, DynamicMetaObject instance, DynamicMetaObject[] args, DynamicMetaObject value, out DynamicMetaObject result)
		{
			if (ComBinder.TryGetMetaObjectInvoke(ref instance))
			{
				result = instance.BindSetIndex(binder, args, value);
				result = new DynamicMetaObject(result.Expression, result.Restrictions.Merge(value.PSGetMethodArgumentRestriction()));
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x060069AE RID: 27054 RVA: 0x002143F0 File Offset: 0x002125F0
		public static bool TryConvert(ConvertBinder binder, DynamicMetaObject instance, out DynamicMetaObject result)
		{
			if (ComBinder.IsComObject(instance.Value) && binder.Type.IsInterface)
			{
				result = new DynamicMetaObject(Expression.Convert(instance.Expression, binder.Type), BindingRestrictions.GetExpressionRestriction(Expression.Call(typeof(ComObject).GetMethod("IsComObject", BindingFlags.Static | BindingFlags.NonPublic), Helpers.Convert(instance.Expression, typeof(object)))));
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x060069AF RID: 27055 RVA: 0x0021446A File Offset: 0x0021266A
		public static IEnumerable<string> GetDynamicMemberNames(object value)
		{
			return ComObject.ObjectToComObject(value).GetMemberNames(false);
		}

		// Token: 0x060069B0 RID: 27056 RVA: 0x00214478 File Offset: 0x00212678
		internal static IList<string> GetDynamicDataMemberNames(object value)
		{
			return ComObject.ObjectToComObject(value).GetMemberNames(true);
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x00214486 File Offset: 0x00212686
		internal static IList<KeyValuePair<string, object>> GetDynamicDataMembers(object value, IEnumerable<string> names)
		{
			return ComObject.ObjectToComObject(value).GetMembers(names);
		}

		// Token: 0x060069B2 RID: 27058 RVA: 0x00214494 File Offset: 0x00212694
		private static bool TryGetMetaObject(ref DynamicMetaObject instance)
		{
			if (instance is ComUnwrappedMetaObject)
			{
				return false;
			}
			if (ComBinder.IsComObject(instance.Value))
			{
				instance = new ComMetaObject(instance.Expression, instance.Restrictions, instance.Value);
				return true;
			}
			return false;
		}

		// Token: 0x060069B3 RID: 27059 RVA: 0x002144CE File Offset: 0x002126CE
		private static bool TryGetMetaObjectInvoke(ref DynamicMetaObject instance)
		{
			if (ComBinder.TryGetMetaObject(ref instance))
			{
				return true;
			}
			if (instance.Value is IPseudoComObject)
			{
				instance = ((IPseudoComObject)instance.Value).GetMetaObject(instance.Expression);
				return true;
			}
			return false;
		}

		// Token: 0x02000A53 RID: 2643
		internal class ComGetMemberBinder : GetMemberBinder
		{
			// Token: 0x060069B5 RID: 27061 RVA: 0x00214505 File Offset: 0x00212705
			internal ComGetMemberBinder(GetMemberBinder originalBinder, bool CanReturnCallables) : base(originalBinder.Name, originalBinder.IgnoreCase)
			{
				this._originalBinder = originalBinder;
				this._CanReturnCallables = CanReturnCallables;
			}

			// Token: 0x060069B6 RID: 27062 RVA: 0x00214527 File Offset: 0x00212727
			public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
			{
				return this._originalBinder.FallbackGetMember(target, errorSuggestion);
			}

			// Token: 0x060069B7 RID: 27063 RVA: 0x00214536 File Offset: 0x00212736
			public override int GetHashCode()
			{
				return this._originalBinder.GetHashCode() ^ (this._CanReturnCallables ? 1 : 0);
			}

			// Token: 0x060069B8 RID: 27064 RVA: 0x00214550 File Offset: 0x00212750
			public override bool Equals(object obj)
			{
				ComBinder.ComGetMemberBinder comGetMemberBinder = obj as ComBinder.ComGetMemberBinder;
				return comGetMemberBinder != null && this._CanReturnCallables == comGetMemberBinder._CanReturnCallables && this._originalBinder.Equals(comGetMemberBinder._originalBinder);
			}

			// Token: 0x0400329A RID: 12954
			private readonly GetMemberBinder _originalBinder;

			// Token: 0x0400329B RID: 12955
			internal bool _CanReturnCallables;
		}

		// Token: 0x02000A54 RID: 2644
		internal class ComInvokeMemberBinder : InvokeMemberBinder
		{
			// Token: 0x060069B9 RID: 27065 RVA: 0x00214588 File Offset: 0x00212788
			internal ComInvokeMemberBinder(InvokeMemberBinder originalBinder, bool isPropertySet) : base(originalBinder.Name, originalBinder.IgnoreCase, originalBinder.CallInfo)
			{
				this._originalBinder = originalBinder;
				this.IsPropertySet = isPropertySet;
			}

			// Token: 0x060069BA RID: 27066 RVA: 0x002145B0 File Offset: 0x002127B0
			public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
			{
				return this._originalBinder.FallbackInvoke(target, args, errorSuggestion);
			}

			// Token: 0x060069BB RID: 27067 RVA: 0x002145C0 File Offset: 0x002127C0
			public override DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
			{
				return this._originalBinder.FallbackInvokeMember(target, args, errorSuggestion);
			}

			// Token: 0x060069BC RID: 27068 RVA: 0x002145D0 File Offset: 0x002127D0
			public override int GetHashCode()
			{
				return this._originalBinder.GetHashCode() ^ (this.IsPropertySet ? 1 : 0);
			}

			// Token: 0x060069BD RID: 27069 RVA: 0x002145EC File Offset: 0x002127EC
			public override bool Equals(object obj)
			{
				ComBinder.ComInvokeMemberBinder comInvokeMemberBinder = obj as ComBinder.ComInvokeMemberBinder;
				return comInvokeMemberBinder != null && this.IsPropertySet == comInvokeMemberBinder.IsPropertySet && this._originalBinder.Equals(comInvokeMemberBinder._originalBinder);
			}

			// Token: 0x0400329C RID: 12956
			private readonly InvokeMemberBinder _originalBinder;

			// Token: 0x0400329D RID: 12957
			internal bool IsPropertySet;
		}
	}
}
