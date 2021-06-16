using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Security;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000857 RID: 2135
	[OutputType(new Type[]
	{
		typeof(PSSnapInInfo)
	})]
	[Cmdlet("Get", "PSSnapin", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113330")]
	public sealed class GetPSSnapinCommand : PSSnapInCommandBase
	{
		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x0600522F RID: 21039 RVA: 0x001B6D90 File Offset: 0x001B4F90
		// (set) Token: 0x06005230 RID: 21040 RVA: 0x001B6D98 File Offset: 0x001B4F98
		[Parameter(Position = 0, Mandatory = false)]
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

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06005231 RID: 21041 RVA: 0x001B6DA1 File Offset: 0x001B4FA1
		// (set) Token: 0x06005232 RID: 21042 RVA: 0x001B6DAE File Offset: 0x001B4FAE
		[Parameter(Mandatory = false)]
		public SwitchParameter Registered
		{
			get
			{
				return base.ShouldGetAll;
			}
			set
			{
				base.ShouldGetAll = value;
			}
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x001B6DBC File Offset: 0x001B4FBC
		protected override void BeginProcessing()
		{
			if (this._pssnapins != null)
			{
				string[] pssnapins = this._pssnapins;
				int i = 0;
				while (i < pssnapins.Length)
				{
					string text = pssnapins[i];
					Exception ex = null;
					try
					{
						Collection<PSSnapInInfo> snapIns = base.GetSnapIns(text);
						if (snapIns.Count == 0)
						{
							base.WriteNonTerminatingError(text, "NoPSSnapInsFound", PSTraceSource.NewArgumentException(text, MshSnapInCmdletResources.NoPSSnapInsFound, new object[]
							{
								text
							}), ErrorCategory.InvalidArgument);
							goto IL_B9;
						}
						foreach (PSSnapInInfo pssnapInInfo in snapIns)
						{
							pssnapInInfo.LoadIndirectResources(base.ResourceReader);
							base.WriteObject(pssnapInInfo);
						}
					}
					catch (SecurityException ex2)
					{
						ex = ex2;
					}
					catch (PSArgumentException ex3)
					{
						ex = ex3;
					}
					goto IL_A8;
					IL_B9:
					i++;
					continue;
					IL_A8:
					if (ex != null)
					{
						base.WriteNonTerminatingError(text, "GetPSSnapInRead", ex, ErrorCategory.InvalidArgument);
						goto IL_B9;
					}
					goto IL_B9;
				}
				return;
			}
			if (base.ShouldGetAll)
			{
				Exception ex4 = null;
				try
				{
					Collection<PSSnapInInfo> collection = PSSnapInReader.ReadAll();
					foreach (PSSnapInInfo pssnapInInfo2 in collection)
					{
						pssnapInInfo2.LoadIndirectResources(base.ResourceReader);
						base.WriteObject(pssnapInInfo2);
					}
				}
				catch (SecurityException ex5)
				{
					ex4 = ex5;
				}
				catch (PSArgumentException ex6)
				{
					ex4 = ex6;
				}
				if (ex4 != null)
				{
					base.WriteNonTerminatingError(this, "GetPSSnapInRead", ex4, ErrorCategory.InvalidArgument);
					return;
				}
			}
			else
			{
				Collection<PSSnapInInfo> snapIns2 = base.GetSnapIns(null);
				foreach (PSSnapInInfo pssnapInInfo3 in snapIns2)
				{
					pssnapInInfo3.LoadIndirectResources(base.ResourceReader);
					base.WriteObject(pssnapInInfo3);
				}
			}
		}

		// Token: 0x04002A37 RID: 10807
		private string[] _pssnapins;
	}
}
