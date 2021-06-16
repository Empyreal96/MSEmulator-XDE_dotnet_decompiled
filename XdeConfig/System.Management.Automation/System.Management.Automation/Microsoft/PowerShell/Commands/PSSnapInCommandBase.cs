using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000854 RID: 2132
	public abstract class PSSnapInCommandBase : PSCmdlet, IDisposable
	{
		// Token: 0x06005217 RID: 21015 RVA: 0x001B64D0 File Offset: 0x001B46D0
		public void Dispose()
		{
			if (!this._disposed)
			{
				if (this.resourceReader != null)
				{
					this.resourceReader.Dispose();
					this.resourceReader = null;
				}
				GC.SuppressFinalize(this);
			}
			this._disposed = true;
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x001B6501 File Offset: 0x001B4701
		protected override void EndProcessing()
		{
			if (this.resourceReader != null)
			{
				this.resourceReader.Dispose();
				this.resourceReader = null;
			}
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06005219 RID: 21017 RVA: 0x001B6520 File Offset: 0x001B4720
		internal RunspaceConfigForSingleShell Runspace
		{
			get
			{
				RunspaceConfigForSingleShell runspaceConfigForSingleShell = base.Context.RunspaceConfiguration as RunspaceConfigForSingleShell;
				if (runspaceConfigForSingleShell == null)
				{
					return null;
				}
				return runspaceConfigForSingleShell;
			}
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x001B6544 File Offset: 0x001B4744
		internal void WriteNonTerminatingError(object targetObject, string errorId, Exception innerException, ErrorCategory category)
		{
			base.WriteError(new ErrorRecord(innerException, errorId, category, targetObject));
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x001B6558 File Offset: 0x001B4758
		internal Collection<string> SearchListForPattern(Collection<PSSnapInInfo> searchList, string pattern)
		{
			Collection<string> collection = new Collection<string>();
			if (searchList == null)
			{
				return collection;
			}
			WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
			foreach (PSSnapInInfo pssnapInInfo in searchList)
			{
				if (wildcardPattern.IsMatch(pssnapInInfo.Name))
				{
					collection.Add(pssnapInInfo.Name);
				}
			}
			return collection;
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x001B65C8 File Offset: 0x001B47C8
		internal static PSSnapInInfo IsSnapInLoaded(Collection<PSSnapInInfo> loadedSnapins, PSSnapInInfo psSnapInInfo)
		{
			if (loadedSnapins == null)
			{
				return null;
			}
			foreach (PSSnapInInfo pssnapInInfo in loadedSnapins)
			{
				string assemblyName = pssnapInInfo.AssemblyName;
				if (string.Equals(pssnapInInfo.Name, psSnapInInfo.Name, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(assemblyName) && string.Equals(assemblyName, psSnapInInfo.AssemblyName, StringComparison.OrdinalIgnoreCase))
				{
					return pssnapInInfo;
				}
			}
			return null;
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x001B6648 File Offset: 0x001B4848
		protected internal Collection<PSSnapInInfo> GetSnapIns(string pattern)
		{
			if (this.Runspace == null)
			{
				WildcardPattern wildcardPattern = null;
				if (!string.IsNullOrEmpty(pattern))
				{
					if (!WildcardPattern.ContainsWildcardCharacters(pattern))
					{
						PSSnapInInfo.VerifyPSSnapInFormatThrowIfError(pattern);
					}
					wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
				}
				Collection<PSSnapInInfo> collection = new Collection<PSSnapInInfo>();
				if (this._shouldGetAll)
				{
					using (IEnumerator<PSSnapInInfo> enumerator = PSSnapInReader.ReadAll().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PSSnapInInfo pssnapInInfo = enumerator.Current;
							if (wildcardPattern == null || wildcardPattern.IsMatch(pssnapInInfo.Name))
							{
								collection.Add(pssnapInInfo);
							}
						}
						return collection;
					}
				}
				List<CmdletInfo> cmdlets = base.InvokeCommand.GetCmdlets();
				Dictionary<PSSnapInInfo, bool> dictionary = new Dictionary<PSSnapInInfo, bool>();
				foreach (CmdletInfo cmdletInfo in cmdlets)
				{
					PSSnapInInfo pssnapIn = cmdletInfo.PSSnapIn;
					if (pssnapIn != null && !dictionary.ContainsKey(pssnapIn))
					{
						dictionary.Add(pssnapIn, true);
					}
				}
				foreach (PSSnapInInfo pssnapInInfo2 in dictionary.Keys)
				{
					if (wildcardPattern == null || wildcardPattern.IsMatch(pssnapInInfo2.Name))
					{
						collection.Add(pssnapInInfo2);
					}
				}
				return collection;
			}
			if (pattern != null)
			{
				return this.Runspace.ConsoleInfo.GetPSSnapIn(pattern, this._shouldGetAll);
			}
			return this.Runspace.ConsoleInfo.PSSnapIns;
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x0600521E RID: 21022 RVA: 0x001B67DC File Offset: 0x001B49DC
		// (set) Token: 0x0600521F RID: 21023 RVA: 0x001B67E4 File Offset: 0x001B49E4
		protected internal bool ShouldGetAll
		{
			get
			{
				return this._shouldGetAll;
			}
			set
			{
				this._shouldGetAll = value;
			}
		}

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x06005220 RID: 21024 RVA: 0x001B67ED File Offset: 0x001B49ED
		internal RegistryStringResourceIndirect ResourceReader
		{
			get
			{
				if (this.resourceReader == null)
				{
					this.resourceReader = RegistryStringResourceIndirect.GetResourceIndirectReader();
				}
				return this.resourceReader;
			}
		}

		// Token: 0x04002A30 RID: 10800
		private bool _disposed;

		// Token: 0x04002A31 RID: 10801
		private bool _shouldGetAll;

		// Token: 0x04002A32 RID: 10802
		private RegistryStringResourceIndirect resourceReader;
	}
}
