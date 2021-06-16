using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.PowerShell.Cmdletization
{
	// Token: 0x0200099E RID: 2462
	public sealed class MethodInvocationInfo
	{
		// Token: 0x06005AC2 RID: 23234 RVA: 0x001E88A8 File Offset: 0x001E6AA8
		public MethodInvocationInfo(string name, IEnumerable<MethodParameter> parameters, MethodParameter returnValue)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			this.methodName = name;
			this.returnValue = returnValue;
			KeyedCollection<string, MethodParameter> keyedCollection = new MethodParametersCollection();
			foreach (MethodParameter item in parameters)
			{
				keyedCollection.Add(item);
			}
			this.parameters = keyedCollection;
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06005AC3 RID: 23235 RVA: 0x001E8930 File Offset: 0x001E6B30
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06005AC4 RID: 23236 RVA: 0x001E8938 File Offset: 0x001E6B38
		public KeyedCollection<string, MethodParameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06005AC5 RID: 23237 RVA: 0x001E8940 File Offset: 0x001E6B40
		public MethodParameter ReturnValue
		{
			get
			{
				return this.returnValue;
			}
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x001E8948 File Offset: 0x001E6B48
		internal IEnumerable<T> GetArgumentsOfType<T>() where T : class
		{
			List<T> list = new List<T>();
			foreach (MethodParameter methodParameter in this.Parameters)
			{
				if (MethodParameterBindings.In == (methodParameter.Bindings & MethodParameterBindings.In))
				{
					T t = methodParameter.Value as T;
					if (t != null)
					{
						list.Add(t);
					}
					else
					{
						IEnumerable enumerable = methodParameter.Value as IEnumerable;
						if (enumerable != null)
						{
							foreach (object obj in enumerable)
							{
								T t2 = obj as T;
								if (t2 != null)
								{
									list.Add(t2);
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0400309A RID: 12442
		private readonly string methodName;

		// Token: 0x0400309B RID: 12443
		private readonly KeyedCollection<string, MethodParameter> parameters;

		// Token: 0x0400309C RID: 12444
		private readonly MethodParameter returnValue;
	}
}
