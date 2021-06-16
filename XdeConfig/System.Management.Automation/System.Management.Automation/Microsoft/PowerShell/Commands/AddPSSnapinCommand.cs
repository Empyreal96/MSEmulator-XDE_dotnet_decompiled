using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000855 RID: 2133
	[OutputType(new Type[]
	{
		typeof(PSSnapInInfo)
	})]
	[Cmdlet("Add", "PSSnapin", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113281")]
	public sealed class AddPSSnapinCommand : PSSnapInCommandBase
	{
		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x06005222 RID: 21026 RVA: 0x001B6810 File Offset: 0x001B4A10
		// (set) Token: 0x06005223 RID: 21027 RVA: 0x001B6818 File Offset: 0x001B4A18
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true)]
		public string[] Name
		{
			get
			{
				return this._pssnapins;
			}
			set
			{
				this._pssnapins = value;
			}
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x06005224 RID: 21028 RVA: 0x001B6821 File Offset: 0x001B4A21
		// (set) Token: 0x06005225 RID: 21029 RVA: 0x001B682E File Offset: 0x001B4A2E
		[Parameter]
		public SwitchParameter PassThru
		{
			get
			{
				return this._passThru;
			}
			set
			{
				this._passThru = value;
			}
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x001B683C File Offset: 0x001B4A3C
		protected override void ProcessRecord()
		{
			Collection<PSSnapInInfo> collection = null;
			string[] pssnapins = this._pssnapins;
			int i = 0;
			while (i < pssnapins.Length)
			{
				string text = pssnapins[i];
				Exception ex = null;
				Collection<string> collection2 = new Collection<string>();
				try
				{
					bool flag = WildcardPattern.ContainsWildcardCharacters(text);
					if (flag)
					{
						if (collection == null)
						{
							collection = PSSnapInReader.ReadAll(PSVersionInfo.RegistryVersion1Key);
						}
						collection2 = base.SearchListForPattern(collection, text);
						if (collection2.Count == 0)
						{
							if (this._passThru)
							{
								base.WriteNonTerminatingError(text, "NoPSSnapInsFound", PSTraceSource.NewArgumentException(text, MshSnapInCmdletResources.NoPSSnapInsFound, new object[]
								{
									text
								}), ErrorCategory.InvalidArgument);
							}
							goto IL_AB;
						}
					}
					else
					{
						collection2.Add(text);
					}
					this.AddPSSnapIns(collection2);
				}
				catch (PSArgumentException ex2)
				{
					ex = ex2;
				}
				catch (SecurityException ex3)
				{
					ex = ex3;
				}
				goto IL_9A;
				IL_AB:
				i++;
				continue;
				IL_9A:
				if (ex != null)
				{
					base.WriteNonTerminatingError(text, "AddPSSnapInRead", ex, ErrorCategory.InvalidArgument);
					goto IL_AB;
				}
				goto IL_AB;
			}
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x001B6924 File Offset: 0x001B4B24
		private void AddPSSnapIns(Collection<string> snapInList)
		{
			if (snapInList == null)
			{
				return;
			}
			if (base.Context.RunspaceConfiguration == null)
			{
				Collection<PSSnapInInfo> snapIns = base.GetSnapIns(null);
				InitialSessionState initialSessionState = InitialSessionState.Create();
				bool flag = false;
				foreach (string text in snapInList)
				{
					if (InitialSessionState.IsEngineModule(text))
					{
						base.WriteNonTerminatingError(text, "LoadSystemSnapinAsModule", PSTraceSource.NewArgumentException(text, MshSnapInCmdletResources.LoadSystemSnapinAsModule, new object[]
						{
							text
						}), ErrorCategory.InvalidArgument);
					}
					else
					{
						try
						{
							PSSnapInInfo psSnapInInfo = PSSnapInReader.Read(Utils.GetCurrentMajorVersion(), text);
							PSSnapInInfo pssnapInInfo = PSSnapInCommandBase.IsSnapInLoaded(snapIns, psSnapInInfo);
							if (pssnapInInfo == null)
							{
								PSSnapInException ex;
								pssnapInInfo = initialSessionState.ImportPSSnapIn(text, out ex);
								flag = true;
								base.Context.InitialSessionState.ImportedSnapins.Add(pssnapInInfo.Name, pssnapInInfo);
							}
							if (this._passThru)
							{
								pssnapInInfo.LoadIndirectResources(base.ResourceReader);
								base.WriteObject(pssnapInInfo);
							}
						}
						catch (PSSnapInException innerException)
						{
							base.WriteNonTerminatingError(text, "AddPSSnapInRead", innerException, ErrorCategory.InvalidData);
						}
					}
				}
				if (flag)
				{
					initialSessionState.Bind(base.Context, true);
				}
				return;
			}
			foreach (string text2 in snapInList)
			{
				Exception ex2 = null;
				try
				{
					PSSnapInException ex3 = null;
					PSSnapInInfo pssnapInInfo2 = base.Runspace.AddPSSnapIn(text2, out ex3);
					if (ex3 != null)
					{
						base.WriteNonTerminatingError(text2, "AddPSSnapInRead", ex3, ErrorCategory.InvalidData);
					}
					if (this._passThru)
					{
						pssnapInInfo2.LoadIndirectResources(base.ResourceReader);
						base.WriteObject(pssnapInInfo2);
					}
				}
				catch (PSArgumentException ex4)
				{
					ex2 = ex4;
				}
				catch (PSSnapInException ex5)
				{
					ex2 = ex5;
				}
				catch (SecurityException ex6)
				{
					ex2 = ex6;
				}
				if (ex2 != null)
				{
					base.WriteNonTerminatingError(text2, "AddPSSnapInRead", ex2, ErrorCategory.InvalidArgument);
				}
			}
		}

		// Token: 0x04002A33 RID: 10803
		private string[] _pssnapins;

		// Token: 0x04002A34 RID: 10804
		private bool _passThru;
	}
}
