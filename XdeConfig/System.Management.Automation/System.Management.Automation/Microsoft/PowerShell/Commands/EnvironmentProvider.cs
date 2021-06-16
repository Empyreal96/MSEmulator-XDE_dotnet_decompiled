using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000764 RID: 1892
	[CmdletProvider("Environment", ProviderCapabilities.ShouldProcess)]
	public sealed class EnvironmentProvider : SessionStateProviderBase
	{
		// Token: 0x06004BA3 RID: 19363 RVA: 0x0018BE3C File Offset: 0x0018A03C
		protected override Collection<PSDriveInfo> InitializeDefaultDrives()
		{
			string environmentDriveDescription = SessionStateStrings.EnvironmentDriveDescription;
			PSDriveInfo item = new PSDriveInfo("Env", base.ProviderInfo, string.Empty, environmentDriveDescription, null);
			return new Collection<PSDriveInfo>
			{
				item
			};
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x0018BE78 File Offset: 0x0018A078
		internal override object GetSessionStateItem(string name)
		{
			object result = null;
			string environmentVariable = Environment.GetEnvironmentVariable(name);
			if (environmentVariable != null)
			{
				result = new DictionaryEntry(name, environmentVariable);
			}
			return result;
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x0018BEA0 File Offset: 0x0018A0A0
		internal override void SetSessionStateItem(string name, object value, bool writeItem)
		{
			if (value == null)
			{
				Environment.SetEnvironmentVariable(name, null);
				return;
			}
			if (value is DictionaryEntry)
			{
				value = ((DictionaryEntry)value).Value;
			}
			string text = value as string;
			if (text == null)
			{
				PSObject psobject = PSObject.AsPSObject(value);
				text = psobject.ToString();
			}
			Environment.SetEnvironmentVariable(name, text);
			DictionaryEntry dictionaryEntry = new DictionaryEntry(name, text);
			if (writeItem)
			{
				base.WriteItemObject(dictionaryEntry, name, false);
			}
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x0018BF09 File Offset: 0x0018A109
		internal override void RemoveSessionStateItem(string name)
		{
			Environment.SetEnvironmentVariable(name, null);
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x0018BF14 File Offset: 0x0018A114
		internal override IDictionary GetSessionStateTable()
		{
			Dictionary<string, DictionaryEntry> dictionary = new Dictionary<string, DictionaryEntry>(StringComparer.OrdinalIgnoreCase);
			IDictionary environmentVariables = Environment.GetEnvironmentVariables();
			foreach (object obj in environmentVariables)
			{
				DictionaryEntry value = (DictionaryEntry)obj;
				dictionary.Add((string)value.Key, value);
			}
			return dictionary;
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x0018BF8C File Offset: 0x0018A18C
		internal override object GetValueOfItem(object item)
		{
			object result = item;
			if (item is DictionaryEntry)
			{
				result = ((DictionaryEntry)item).Value;
			}
			return result;
		}

		// Token: 0x04002480 RID: 9344
		public const string ProviderName = "Environment";
	}
}
