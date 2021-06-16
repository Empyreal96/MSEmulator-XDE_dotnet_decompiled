using System;
using System.Collections;
using System.Collections.Specialized;
using System.Dynamic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000612 RID: 1554
	internal class PSCustomObjectConverter : DynamicMetaObjectBinder
	{
		// Token: 0x06004384 RID: 17284 RVA: 0x001639EC File Offset: 0x00161BEC
		internal static PSCustomObjectConverter Get()
		{
			return PSCustomObjectConverter._binder;
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x001639F3 File Offset: 0x00161BF3
		private PSCustomObjectConverter()
		{
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x001639FC File Offset: 0x00161BFC
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]).WriteToDebugLog(this);
			}
			object obj = PSObject.Base(target.Value);
			Type resultType = (obj is OrderedDictionary || obj is Hashtable) ? typeof(LanguagePrimitives.InternalPSCustomObject) : typeof(PSObject);
			bool debase;
			LanguagePrimitives.ConversionData conversion = LanguagePrimitives.FigureConversion(target.Value, resultType, out debase);
			return new DynamicMetaObject(PSConvertBinder.InvokeConverter(conversion, target.Expression, resultType, debase, ExpressionCache.InvariantCulture), target.PSGetTypeRestriction()).WriteToDebugLog(this);
		}

		// Token: 0x040021AC RID: 8620
		private static readonly PSCustomObjectConverter _binder = new PSCustomObjectConverter();
	}
}
