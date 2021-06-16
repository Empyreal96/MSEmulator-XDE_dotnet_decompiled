using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000078 RID: 120
	internal sealed class CommandLineParameters
	{
		// Token: 0x0600064C RID: 1612 RVA: 0x0001F085 File Offset: 0x0001D285
		internal bool ContainsKey(string name)
		{
			return this._dictionary.ContainsKey(name);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001F093 File Offset: 0x0001D293
		internal void Add(string name, object value)
		{
			this._dictionary[name] = value;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001F0A2 File Offset: 0x0001D2A2
		internal void MarkAsBoundPositionally(string name)
		{
			this._dictionary.BoundPositionally.Add(name);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001F0B5 File Offset: 0x0001D2B5
		internal void SetPSBoundParametersVariable(ExecutionContext context)
		{
			context.SetVariable(SpecialVariables.PSBoundParametersVarPath, this._dictionary);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001F0C8 File Offset: 0x0001D2C8
		internal void SetImplicitUsingParameters(object obj)
		{
			this._dictionary.ImplicitUsingParameters = (PSObject.Base(obj) as IDictionary);
			if (this._dictionary.ImplicitUsingParameters == null)
			{
				IList list = PSObject.Base(obj) as IList;
				if (list != null && list.Count > 0)
				{
					this._dictionary.ImplicitUsingParameters = new Hashtable();
					for (int i = 0; i < list.Count; i++)
					{
						this._dictionary.ImplicitUsingParameters.Add(i, list[i]);
					}
				}
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001F14E File Offset: 0x0001D34E
		internal IDictionary GetImplicitUsingParameters()
		{
			return this._dictionary.ImplicitUsingParameters;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001F15B File Offset: 0x0001D35B
		internal object GetValueToBindToPSBoundParameters()
		{
			return this._dictionary;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001F163 File Offset: 0x0001D363
		internal void UpdateInvocationInfo(InvocationInfo invocationInfo)
		{
			invocationInfo.BoundParameters = this._dictionary;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001F174 File Offset: 0x0001D374
		internal HashSet<string> CopyBoundPositionalParameters()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
			foreach (string item in this._dictionary.BoundPositionally)
			{
				hashSet.Add(item);
			}
			return hashSet;
		}

		// Token: 0x04000293 RID: 659
		private readonly PSBoundParametersDictionary _dictionary = new PSBoundParametersDictionary();
	}
}
