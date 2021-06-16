using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004A1 RID: 1185
	internal abstract class CommandParameterDefinition
	{
		// Token: 0x060034F0 RID: 13552 RVA: 0x0011F5C2 File Offset: 0x0011D7C2
		internal CommandParameterDefinition()
		{
			this.SetEntries();
		}

		// Token: 0x060034F1 RID: 13553
		protected abstract void SetEntries();

		// Token: 0x060034F2 RID: 13554 RVA: 0x0011F5DB File Offset: 0x0011D7DB
		internal virtual MshParameter CreateInstance()
		{
			return new MshParameter();
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x0011F5E4 File Offset: 0x0011D7E4
		internal HashtableEntryDefinition MatchEntry(string keyName, TerminatingErrorContext invocationContext)
		{
			if (string.IsNullOrEmpty(keyName))
			{
				PSTraceSource.NewArgumentNullException("keyName");
			}
			HashtableEntryDefinition hashtableEntryDefinition = null;
			for (int i = 0; i < this.hashEntries.Count; i++)
			{
				if (this.hashEntries[i].IsKeyMatch(keyName))
				{
					if (hashtableEntryDefinition == null)
					{
						hashtableEntryDefinition = this.hashEntries[i];
					}
					else
					{
						CommandParameterDefinition.ProcessAmbiguousKey(invocationContext, keyName, hashtableEntryDefinition, this.hashEntries[i]);
					}
				}
			}
			if (hashtableEntryDefinition != null)
			{
				return hashtableEntryDefinition;
			}
			CommandParameterDefinition.ProcessIllegalKey(invocationContext, keyName);
			return null;
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x0011F663 File Offset: 0x0011D863
		internal static bool FindPartialMatch(string key, string normalizedKey)
		{
			return (key.Length < normalizedKey.Length && string.Equals(key, normalizedKey.Substring(0, key.Length), StringComparison.OrdinalIgnoreCase)) || string.Equals(key, normalizedKey, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x0011F698 File Offset: 0x0011D898
		private static void ProcessAmbiguousKey(TerminatingErrorContext invocationContext, string keyName, HashtableEntryDefinition matchingEntry, HashtableEntryDefinition currentEntry)
		{
			string msg = StringUtil.Format(FormatAndOut_MshParameter.AmbiguousKeyError, new object[]
			{
				keyName,
				matchingEntry.KeyName,
				currentEntry.KeyName
			});
			ParameterProcessor.ThrowParameterBindingException(invocationContext, "DictionaryKeyAmbiguous", msg);
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x0011F6DC File Offset: 0x0011D8DC
		private static void ProcessIllegalKey(TerminatingErrorContext invocationContext, string keyName)
		{
			string msg = StringUtil.Format(FormatAndOut_MshParameter.IllegalKeyError, keyName);
			ParameterProcessor.ThrowParameterBindingException(invocationContext, "DictionaryKeyIllegal", msg);
		}

		// Token: 0x04001B1D RID: 6941
		internal List<HashtableEntryDefinition> hashEntries = new List<HashtableEntryDefinition>();
	}
}
