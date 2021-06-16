using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200049F RID: 1183
	internal class HashtableEntryDefinition
	{
		// Token: 0x060034E4 RID: 13540 RVA: 0x0011F4A3 File Offset: 0x0011D6A3
		internal HashtableEntryDefinition(string name, IEnumerable<string> secondaryNames, Type[] types, bool mandatory) : this(name, types, mandatory)
		{
			this.secondaryNames = secondaryNames;
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x0011F4B6 File Offset: 0x0011D6B6
		internal HashtableEntryDefinition(string name, Type[] types, bool mandatory)
		{
			this.key = name;
			this.allowedTypes = types;
			this.mandatory = mandatory;
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x0011F4D3 File Offset: 0x0011D6D3
		internal HashtableEntryDefinition(string name, Type[] types) : this(name, types, false)
		{
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x0011F4DE File Offset: 0x0011D6DE
		internal virtual Hashtable CreateHashtableFromSingleType(object val)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x0011F4E8 File Offset: 0x0011D6E8
		internal bool IsKeyMatch(string key)
		{
			if (CommandParameterDefinition.FindPartialMatch(key, this.KeyName))
			{
				return true;
			}
			if (this.SecondaryNames != null)
			{
				foreach (string normalizedKey in this.SecondaryNames)
				{
					if (CommandParameterDefinition.FindPartialMatch(key, normalizedKey))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x0011F558 File Offset: 0x0011D758
		internal virtual object Verify(object val, TerminatingErrorContext invocationContext, bool originalParameterWasHashTable)
		{
			return null;
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x0011F55B File Offset: 0x0011D75B
		internal virtual object ComputeDefaultValue()
		{
			return AutomationNull.Value;
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x060034EB RID: 13547 RVA: 0x0011F562 File Offset: 0x0011D762
		internal string KeyName
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x060034EC RID: 13548 RVA: 0x0011F56A File Offset: 0x0011D76A
		internal Type[] AllowedTypes
		{
			get
			{
				return this.allowedTypes;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x060034ED RID: 13549 RVA: 0x0011F572 File Offset: 0x0011D772
		internal bool Mandatory
		{
			get
			{
				return this.mandatory;
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x060034EE RID: 13550 RVA: 0x0011F57A File Offset: 0x0011D77A
		internal IEnumerable<string> SecondaryNames
		{
			get
			{
				return this.secondaryNames;
			}
		}

		// Token: 0x04001B18 RID: 6936
		private string key;

		// Token: 0x04001B19 RID: 6937
		private Type[] allowedTypes;

		// Token: 0x04001B1A RID: 6938
		private bool mandatory;

		// Token: 0x04001B1B RID: 6939
		private IEnumerable<string> secondaryNames;
	}
}
