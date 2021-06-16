using System;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000202 RID: 514
	[OutputType(new Type[]
	{
		typeof(HistoryInfo)
	})]
	[Cmdlet("Add", "History", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113279")]
	public class AddHistoryCommand : PSCmdlet
	{
		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x00093A41 File Offset: 0x00091C41
		// (set) Token: 0x060017DF RID: 6111 RVA: 0x00093A38 File Offset: 0x00091C38
		[Parameter(Position = 0, ValueFromPipeline = true)]
		public PSObject[] InputObject
		{
			get
			{
				return this._inputObjects;
			}
			set
			{
				this._inputObjects = value;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x060017E1 RID: 6113 RVA: 0x00093A49 File Offset: 0x00091C49
		// (set) Token: 0x060017E2 RID: 6114 RVA: 0x00093A56 File Offset: 0x00091C56
		[Parameter]
		public SwitchParameter Passthru
		{
			get
			{
				return this._passthru;
			}
			set
			{
				this._passthru = value;
			}
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00093A64 File Offset: 0x00091C64
		protected override void BeginProcessing()
		{
			LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)base.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
			localPipeline.AddHistoryEntryFromAddHistoryCmdlet();
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00093A94 File Offset: 0x00091C94
		protected override void ProcessRecord()
		{
			History history = ((LocalRunspace)base.Context.CurrentRunspace).History;
			if (this.InputObject != null)
			{
				foreach (PSObject mshObject in this.InputObject)
				{
					HistoryInfo historyInfoObject = this.GetHistoryInfoObject(mshObject);
					if (historyInfoObject != null)
					{
						long id = history.AddEntry(0L, historyInfoObject.CommandLine, historyInfoObject.ExecutionStatus, historyInfoObject.StartExecutionTime, historyInfoObject.EndExecutionTime, false);
						if (this.Passthru)
						{
							HistoryInfo entry = history.GetEntry(id);
							base.WriteObject(entry);
						}
					}
				}
			}
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00093B2C File Offset: 0x00091D2C
		private HistoryInfo GetHistoryInfoObject(PSObject mshObject)
		{
			if (mshObject != null)
			{
				string text = AddHistoryCommand.GetPropertyValue(mshObject, "CommandLine") as string;
				if (text != null)
				{
					object propertyValue = AddHistoryCommand.GetPropertyValue(mshObject, "ExecutionStatus");
					if (propertyValue != null)
					{
						PipelineState pipelineState;
						if (propertyValue is PipelineState)
						{
							pipelineState = (PipelineState)propertyValue;
						}
						else if (propertyValue is PSObject)
						{
							PSObject psobject = propertyValue as PSObject;
							object baseObject = psobject.BaseObject;
							if (!(baseObject is int))
							{
								goto IL_146;
							}
							pipelineState = (PipelineState)baseObject;
							if (pipelineState < PipelineState.NotStarted)
							{
								goto IL_146;
							}
							if (pipelineState > PipelineState.Failed)
							{
								goto IL_146;
							}
						}
						else
						{
							if (!(propertyValue is string))
							{
								goto IL_146;
							}
							try
							{
								pipelineState = (PipelineState)Enum.Parse(typeof(PipelineState), (string)propertyValue);
							}
							catch (ArgumentException)
							{
								goto IL_146;
							}
						}
						object propertyValue2 = AddHistoryCommand.GetPropertyValue(mshObject, "StartExecutionTime");
						if (propertyValue2 != null)
						{
							DateTime startTime;
							if (propertyValue2 is DateTime)
							{
								startTime = (DateTime)propertyValue2;
							}
							else
							{
								if (!(propertyValue2 is string))
								{
									goto IL_146;
								}
								try
								{
									startTime = DateTime.Parse((string)propertyValue2, CultureInfo.CurrentCulture);
								}
								catch (FormatException)
								{
									goto IL_146;
								}
							}
							propertyValue2 = AddHistoryCommand.GetPropertyValue(mshObject, "EndExecutionTime");
							if (propertyValue2 != null)
							{
								DateTime endTime;
								if (propertyValue2 is DateTime)
								{
									endTime = (DateTime)propertyValue2;
								}
								else
								{
									if (!(propertyValue2 is string))
									{
										goto IL_146;
									}
									try
									{
										endTime = DateTime.Parse((string)propertyValue2, CultureInfo.CurrentCulture);
									}
									catch (FormatException)
									{
										goto IL_146;
									}
								}
								return new HistoryInfo(0L, text, pipelineState, startTime, endTime);
							}
						}
					}
				}
			}
			IL_146:
			Exception exception = new InvalidDataException(StringUtil.Format(HistoryStrings.AddHistoryInvalidInput, new object[0]));
			base.WriteError(new ErrorRecord(exception, "AddHistoryInvalidInput", ErrorCategory.InvalidData, mshObject));
			return null;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00093CD4 File Offset: 0x00091ED4
		private static object GetPropertyValue(PSObject mshObject, string propertyName)
		{
			PSMemberInfo psmemberInfo = mshObject.Properties[propertyName];
			if (psmemberInfo == null)
			{
				return null;
			}
			return psmemberInfo.Value;
		}

		// Token: 0x04000A12 RID: 2578
		private PSObject[] _inputObjects;

		// Token: 0x04000A13 RID: 2579
		private bool _passthru;
	}
}
