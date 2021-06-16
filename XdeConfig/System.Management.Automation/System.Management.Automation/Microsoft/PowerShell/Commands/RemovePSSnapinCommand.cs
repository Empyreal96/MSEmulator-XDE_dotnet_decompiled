using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000856 RID: 2134
	[OutputType(new Type[]
	{
		typeof(PSSnapInInfo)
	})]
	[Cmdlet("Remove", "PSSnapin", SupportsShouldProcess = true, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113378")]
	public sealed class RemovePSSnapinCommand : PSSnapInCommandBase
	{
		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x06005229 RID: 21033 RVA: 0x001B6B38 File Offset: 0x001B4D38
		// (set) Token: 0x0600522A RID: 21034 RVA: 0x001B6B40 File Offset: 0x001B4D40
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

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x0600522B RID: 21035 RVA: 0x001B6B49 File Offset: 0x001B4D49
		// (set) Token: 0x0600522C RID: 21036 RVA: 0x001B6B56 File Offset: 0x001B4D56
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

		// Token: 0x0600522D RID: 21037 RVA: 0x001B6B64 File Offset: 0x001B4D64
		protected override void ProcessRecord()
		{
			foreach (string text in this._pssnapins)
			{
				Collection<PSSnapInInfo> snapIns = base.GetSnapIns(text);
				if (snapIns.Count == 0)
				{
					base.WriteNonTerminatingError(text, "NoPSSnapInsFound", PSTraceSource.NewArgumentException(text, MshSnapInCmdletResources.NoPSSnapInsFound, new object[]
					{
						text
					}), ErrorCategory.InvalidArgument);
				}
				else
				{
					foreach (PSSnapInInfo pssnapInInfo in snapIns)
					{
						if (base.ShouldProcess(pssnapInInfo.Name))
						{
							Exception ex = null;
							if (base.Runspace == null && base.Context.InitialSessionState != null)
							{
								try
								{
									PSSnapInInfo.VerifyPSSnapInFormatThrowIfError(pssnapInInfo.Name);
									if (MshConsoleInfo.IsDefaultPSSnapIn(pssnapInInfo.Name, base.Context.InitialSessionState.defaultSnapins))
									{
										throw PSTraceSource.NewArgumentException(pssnapInInfo.Name, ConsoleInfoErrorStrings.CannotRemoveDefault, new object[]
										{
											pssnapInInfo.Name
										});
									}
									InitialSessionState initialSessionState = InitialSessionState.Create();
									PSSnapInException ex2;
									initialSessionState.ImportPSSnapIn(pssnapInInfo, out ex2);
									initialSessionState.Unbind(base.Context);
									base.Context.InitialSessionState.ImportedSnapins.Remove(pssnapInInfo.Name);
								}
								catch (PSArgumentException ex3)
								{
									ex = ex3;
								}
								if (ex != null)
								{
									base.WriteNonTerminatingError(text, "RemovePSSnapIn", ex, ErrorCategory.InvalidArgument);
								}
							}
							else
							{
								try
								{
									PSSnapInException ex4 = null;
									PSSnapInInfo pssnapInInfo2 = base.Runspace.RemovePSSnapIn(pssnapInInfo.Name, out ex4);
									if (ex4 != null)
									{
										base.WriteNonTerminatingError(pssnapInInfo.Name, "RemovePSSnapInRead", ex4, ErrorCategory.InvalidData);
									}
									if (this._passThru)
									{
										pssnapInInfo2.LoadIndirectResources(base.ResourceReader);
										base.WriteObject(pssnapInInfo2);
									}
								}
								catch (PSArgumentException ex5)
								{
									ex = ex5;
								}
								if (ex != null)
								{
									base.WriteNonTerminatingError(text, "RemovePSSnapIn", ex, ErrorCategory.InvalidArgument);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04002A35 RID: 10805
		private string[] _pssnapins;

		// Token: 0x04002A36 RID: 10806
		private bool _passThru;
	}
}
