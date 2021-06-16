using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000356 RID: 854
	[Cmdlet("Get", "PSHostProcessInfo", DefaultParameterSetName = "ProcessNameParameterSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkId=517012")]
	[OutputType(new Type[]
	{
		typeof(PSHostProcessInfo)
	})]
	public sealed class GetPSHostProcessInfoCommand : PSCmdlet
	{
		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06002A7E RID: 10878 RVA: 0x000EAE77 File Offset: 0x000E9077
		// (set) Token: 0x06002A7F RID: 10879 RVA: 0x000EAE7F File Offset: 0x000E907F
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, ParameterSetName = "ProcessNameParameterSet")]
		public string[] Name { get; set; }

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x000EAE88 File Offset: 0x000E9088
		// (set) Token: 0x06002A81 RID: 10881 RVA: 0x000EAE90 File Offset: 0x000E9090
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ProcessParameterSet")]
		[ValidateNotNullOrEmpty]
		public Process[] Process { get; set; }

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x000EAE99 File Offset: 0x000E9099
		// (set) Token: 0x06002A83 RID: 10883 RVA: 0x000EAEA1 File Offset: 0x000E90A1
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "ProcessIdParameterSet")]
		public int[] Id { get; set; }

		// Token: 0x06002A84 RID: 10884 RVA: 0x000EAEAC File Offset: 0x000E90AC
		protected override void EndProcessing()
		{
			string parameterSetName;
			IReadOnlyCollection<PSHostProcessInfo> sendToPipeline;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (parameterSetName == "ProcessNameParameterSet")
				{
					sendToPipeline = GetPSHostProcessInfoCommand.GetAppDomainNamesFromProcessId(this.GetProcIdsFromNames(this.Name));
					goto IL_74;
				}
				if (parameterSetName == "ProcessIdParameterSet")
				{
					sendToPipeline = GetPSHostProcessInfoCommand.GetAppDomainNamesFromProcessId(this.Id);
					goto IL_74;
				}
				if (parameterSetName == "ProcessParameterSet")
				{
					sendToPipeline = GetPSHostProcessInfoCommand.GetAppDomainNamesFromProcessId(this.GetProcIdsFromProcs(this.Process));
					goto IL_74;
				}
			}
			sendToPipeline = new ReadOnlyCollection<PSHostProcessInfo>(new Collection<PSHostProcessInfo>());
			IL_74:
			base.WriteObject(sendToPipeline, true);
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000EAF38 File Offset: 0x000E9138
		private int[] GetProcIdsFromProcs(Process[] processes)
		{
			List<int> list = new List<int>();
			foreach (Process process in processes)
			{
				list.Add(process.Id);
			}
			return list.ToArray();
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000EAF74 File Offset: 0x000E9174
		private int[] GetProcIdsFromNames(string[] names)
		{
			if (names == null || names.Length == 0)
			{
				return null;
			}
			List<int> list = new List<int>();
			Process[] processes = System.Diagnostics.Process.GetProcesses();
			foreach (string pattern in names)
			{
				WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
				foreach (Process process in processes)
				{
					if (wildcardPattern.IsMatch(process.ProcessName))
					{
						list.Add(process.Id);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000EB01C File Offset: 0x000E921C
		internal static IReadOnlyCollection<PSHostProcessInfo> GetAppDomainNamesFromProcessId(int[] procIds)
		{
			List<PSHostProcessInfo> list = new List<PSHostProcessInfo>();
			List<string> list2;
			List<string> list3;
			Utils.NativeEnumerateDirectory("\\\\.\\pipe\\", out list2, out list3);
			foreach (string text in list3)
			{
				int num = text.IndexOf("PSHost.", StringComparison.OrdinalIgnoreCase);
				if (num > -1)
				{
					int num2 = text.IndexOf(".", num, StringComparison.OrdinalIgnoreCase);
					if (num2 > -1)
					{
						int num3 = text.IndexOf(".", num2 + 1, StringComparison.OrdinalIgnoreCase);
						if (num3 > -1)
						{
							int num4 = text.IndexOf(".", num3 + 1, StringComparison.OrdinalIgnoreCase);
							if (num4 > -1)
							{
								string s = text.Substring(num3 + 1, num4 - num3 - 1);
								int num5 = -1;
								if (int.TryParse(s, out num5) && procIds != null)
								{
									bool flag = false;
									foreach (int num6 in procIds)
									{
										if (num5 == num6)
										{
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										continue;
									}
								}
								int num7 = text.IndexOf(".", num4 + 1, StringComparison.OrdinalIgnoreCase);
								if (num7 > -1)
								{
									string appDomainName = text.Substring(num4 + 1, num7 - num4 - 1);
									string processName = text.Substring(num7 + 1);
									list.Add(new PSHostProcessInfo(processName, num5, appDomainName));
								}
							}
						}
					}
				}
			}
			if (list.Count > 1)
			{
				CompareInfo comparerInfo = CultureInfo.InvariantCulture.CompareInfo;
				list.Sort((PSHostProcessInfo firstItem, PSHostProcessInfo secondItem) => comparerInfo.Compare(firstItem.ProcessName, secondItem.ProcessName, CompareOptions.IgnoreCase));
			}
			return new ReadOnlyCollection<PSHostProcessInfo>(list);
		}

		// Token: 0x04001500 RID: 5376
		private const string ProcessParameterSet = "ProcessParameterSet";

		// Token: 0x04001501 RID: 5377
		private const string ProcessIdParameterSet = "ProcessIdParameterSet";

		// Token: 0x04001502 RID: 5378
		private const string ProcessNameParameterSet = "ProcessNameParameterSet";

		// Token: 0x04001503 RID: 5379
		private const string NamedPipePath = "\\\\.\\pipe\\";
	}
}
