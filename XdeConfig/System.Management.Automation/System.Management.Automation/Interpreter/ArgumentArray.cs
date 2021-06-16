using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000750 RID: 1872
	internal sealed class ArgumentArray
	{
		// Token: 0x06004ADC RID: 19164 RVA: 0x00188819 File Offset: 0x00186A19
		internal ArgumentArray(object[] arguments, int first, int count)
		{
			this._arguments = arguments;
			this._first = first;
			this._count = count;
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06004ADD RID: 19165 RVA: 0x00188836 File Offset: 0x00186A36
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x0018883E File Offset: 0x00186A3E
		public object GetArgument(int index)
		{
			return this._arguments[this._first + index];
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x0018884F File Offset: 0x00186A4F
		public DynamicMetaObject GetMetaObject(Expression parameter, int index)
		{
			return DynamicMetaObject.Create(this.GetArgument(index), Expression.Call(ArgumentArray._GetArgMethod, Utils.Convert(parameter, typeof(ArgumentArray)), Utils.Constant(index)));
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x00188882 File Offset: 0x00186A82
		public static object GetArg(ArgumentArray array, int index)
		{
			return array._arguments[array._first + index];
		}

		// Token: 0x0400242D RID: 9261
		private readonly object[] _arguments;

		// Token: 0x0400242E RID: 9262
		private readonly int _first;

		// Token: 0x0400242F RID: 9263
		private readonly int _count;

		// Token: 0x04002430 RID: 9264
		private static readonly MethodInfo _GetArgMethod = new Func<ArgumentArray, int, object>(ArgumentArray.GetArg).GetMethodInfo();
	}
}
