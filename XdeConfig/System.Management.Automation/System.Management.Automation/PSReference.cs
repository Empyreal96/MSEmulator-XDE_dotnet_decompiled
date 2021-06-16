using System;
using System.Dynamic;
using System.Management.Automation.Language;
using System.Runtime.CompilerServices;

namespace System.Management.Automation
{
	// Token: 0x02000173 RID: 371
	public class PSReference
	{
		// Token: 0x060012A9 RID: 4777 RVA: 0x000742CA File Offset: 0x000724CA
		public PSReference(object value)
		{
			this._value = value;
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x000742DC File Offset: 0x000724DC
		// (set) Token: 0x060012AB RID: 4779 RVA: 0x00074308 File Offset: 0x00072508
		public object Value
		{
			get
			{
				PSVariable psvariable = this._value as PSVariable;
				if (psvariable != null)
				{
					return psvariable.Value;
				}
				return this._value;
			}
			set
			{
				PSVariable psvariable = this._value as PSVariable;
				if (psvariable != null)
				{
					psvariable.Value = value;
					return;
				}
				this._value = value;
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00074334 File Offset: 0x00072534
		internal static PSReference CreateInstance(object value, Type typeOfValue)
		{
			Type arg = typeof(PSReference<>).MakeGenericType(new Type[]
			{
				typeOfValue
			});
			return (PSReference)PSReference.CreatePsReferenceInstance.Target(PSReference.CreatePsReferenceInstance, arg, value);
		}

		// Token: 0x040007F0 RID: 2032
		private object _value;

		// Token: 0x040007F1 RID: 2033
		internal static readonly CallSite<Func<CallSite, object, object, object>> CreatePsReferenceInstance = CallSite<Func<CallSite, object, object, object>>.Create(PSCreateInstanceBinder.Get(new CallInfo(1, new string[0]), null, false));
	}
}
