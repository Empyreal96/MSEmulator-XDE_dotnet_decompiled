using System;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation.Remoting.Internal
{
	// Token: 0x020002AE RID: 686
	public class PSStreamObject
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x0600212B RID: 8491 RVA: 0x000BF511 File Offset: 0x000BD711
		// (set) Token: 0x0600212C RID: 8492 RVA: 0x000BF519 File Offset: 0x000BD719
		public PSStreamObjectType ObjectType { get; set; }

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x000BF522 File Offset: 0x000BD722
		// (set) Token: 0x0600212E RID: 8494 RVA: 0x000BF52A File Offset: 0x000BD72A
		internal object Value { get; set; }

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x000BF533 File Offset: 0x000BD733
		// (set) Token: 0x06002130 RID: 8496 RVA: 0x000BF53B File Offset: 0x000BD73B
		internal Guid Id { get; set; }

		// Token: 0x06002131 RID: 8497 RVA: 0x000BF544 File Offset: 0x000BD744
		internal PSStreamObject(PSStreamObjectType objectType, object value, Guid id)
		{
			this.ObjectType = objectType;
			this.Value = value;
			this.Id = id;
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000BF561 File Offset: 0x000BD761
		public PSStreamObject(PSStreamObjectType objectType, object value) : this(objectType, value, Guid.Empty)
		{
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000BF570 File Offset: 0x000BD770
		public void WriteStreamObject(Cmdlet cmdlet, bool overrideInquire = false)
		{
			if (cmdlet != null)
			{
				switch (this.ObjectType)
				{
				case PSStreamObjectType.Output:
					cmdlet.WriteObject(this.Value);
					return;
				case PSStreamObjectType.Error:
				{
					ErrorRecord errorRecord = (ErrorRecord)this.Value;
					errorRecord.PreserveInvocationInfoOnce = true;
					MshCommandRuntime mshCommandRuntime = cmdlet.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime != null)
					{
						mshCommandRuntime.WriteError(errorRecord, overrideInquire);
						return;
					}
					break;
				}
				case PSStreamObjectType.MethodExecutor:
				{
					ClientMethodExecutor clientMethodExecutor = (ClientMethodExecutor)this.Value;
					clientMethodExecutor.Execute(cmdlet);
					return;
				}
				case PSStreamObjectType.Warning:
				{
					string message = (string)this.Value;
					WarningRecord record = new WarningRecord(message);
					MshCommandRuntime mshCommandRuntime2 = cmdlet.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime2 != null)
					{
						mshCommandRuntime2.WriteWarning(record, overrideInquire);
						return;
					}
					break;
				}
				case PSStreamObjectType.BlockingError:
				{
					CmdletMethodInvoker<object> cmdletMethodInvoker = (CmdletMethodInvoker<object>)this.Value;
					PSStreamObject.InvokeCmdletMethodAndWaitForResults<object>(cmdletMethodInvoker, cmdlet);
					return;
				}
				case PSStreamObjectType.ShouldMethod:
				{
					CmdletMethodInvoker<bool> cmdletMethodInvoker2 = (CmdletMethodInvoker<bool>)this.Value;
					PSStreamObject.InvokeCmdletMethodAndWaitForResults<bool>(cmdletMethodInvoker2, cmdlet);
					return;
				}
				case PSStreamObjectType.WarningRecord:
				{
					WarningRecord obj = (WarningRecord)this.Value;
					MshCommandRuntime mshCommandRuntime3 = cmdlet.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime3 != null)
					{
						mshCommandRuntime3.AppendWarningVarList(obj);
						return;
					}
					break;
				}
				case PSStreamObjectType.Debug:
				{
					string message2 = (string)this.Value;
					DebugRecord record2 = new DebugRecord(message2);
					MshCommandRuntime mshCommandRuntime4 = cmdlet.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime4 != null)
					{
						mshCommandRuntime4.WriteDebug(record2, overrideInquire);
						return;
					}
					break;
				}
				case PSStreamObjectType.Progress:
				{
					MshCommandRuntime mshCommandRuntime5 = cmdlet.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime5 != null)
					{
						mshCommandRuntime5.WriteProgress((ProgressRecord)this.Value, overrideInquire);
						return;
					}
					break;
				}
				case PSStreamObjectType.Verbose:
				{
					string message3 = (string)this.Value;
					VerboseRecord record3 = new VerboseRecord(message3);
					MshCommandRuntime mshCommandRuntime6 = cmdlet.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime6 != null)
					{
						mshCommandRuntime6.WriteVerbose(record3, overrideInquire);
						return;
					}
					break;
				}
				case PSStreamObjectType.Information:
				{
					MshCommandRuntime mshCommandRuntime7 = cmdlet.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime7 != null)
					{
						mshCommandRuntime7.WriteInformation((InformationRecord)this.Value, overrideInquire);
						return;
					}
					break;
				}
				case PSStreamObjectType.Exception:
				{
					Exception ex = (Exception)this.Value;
					throw ex;
				}
				default:
					return;
				}
			}
			else if (this.ObjectType == PSStreamObjectType.Exception)
			{
				Exception ex2 = (Exception)this.Value;
				throw ex2;
			}
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000BF780 File Offset: 0x000BD980
		private static void GetIdentifierInfo(string message, out Guid jobInstanceId, out string computerName)
		{
			jobInstanceId = Guid.Empty;
			computerName = string.Empty;
			if (message == null)
			{
				return;
			}
			string[] array = message.Split(new char[]
			{
				':'
			}, 3);
			if (array.Length != 3)
			{
				return;
			}
			if (!Guid.TryParse(array[0], out jobInstanceId))
			{
				jobInstanceId = Guid.Empty;
			}
			computerName = array[1];
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000BF7DC File Offset: 0x000BD9DC
		internal void WriteStreamObject(Cmdlet cmdlet, Guid instanceId, bool overrideInquire = false)
		{
			switch (this.ObjectType)
			{
			case PSStreamObjectType.Output:
				if (instanceId != Guid.Empty)
				{
					PSObject psobject = this.Value as PSObject;
					if (psobject != null)
					{
						PSStreamObject.AddSourceJobNoteProperty(psobject, instanceId);
					}
				}
				cmdlet.WriteObject(this.Value);
				return;
			case PSStreamObjectType.Error:
			{
				ErrorRecord errorRecord = (ErrorRecord)this.Value;
				RemotingErrorRecord remotingErrorRecord = errorRecord as RemotingErrorRecord;
				if (remotingErrorRecord == null)
				{
					if (errorRecord.ErrorDetails != null && !string.IsNullOrEmpty(errorRecord.ErrorDetails.RecommendedAction))
					{
						Guid instanceID;
						string computerName;
						PSStreamObject.GetIdentifierInfo(errorRecord.ErrorDetails.RecommendedAction, out instanceID, out computerName);
						errorRecord = new RemotingErrorRecord(errorRecord, new OriginInfo(computerName, Guid.Empty, instanceID));
					}
				}
				else
				{
					errorRecord = remotingErrorRecord;
				}
				errorRecord.PreserveInvocationInfoOnce = true;
				MshCommandRuntime mshCommandRuntime = cmdlet.CommandRuntime as MshCommandRuntime;
				if (mshCommandRuntime != null)
				{
					mshCommandRuntime.WriteError(errorRecord, overrideInquire);
					return;
				}
				break;
			}
			case PSStreamObjectType.MethodExecutor:
			case PSStreamObjectType.BlockingError:
			case PSStreamObjectType.ShouldMethod:
			case PSStreamObjectType.WarningRecord:
				this.WriteStreamObject(cmdlet, overrideInquire);
				break;
			case PSStreamObjectType.Warning:
			{
				string message = (string)this.Value;
				WarningRecord record = new WarningRecord(message);
				MshCommandRuntime mshCommandRuntime2 = cmdlet.CommandRuntime as MshCommandRuntime;
				if (mshCommandRuntime2 != null)
				{
					mshCommandRuntime2.WriteWarning(record, overrideInquire);
					return;
				}
				break;
			}
			case PSStreamObjectType.Debug:
			{
				string message2 = (string)this.Value;
				DebugRecord record2 = new DebugRecord(message2);
				MshCommandRuntime mshCommandRuntime3 = cmdlet.CommandRuntime as MshCommandRuntime;
				if (mshCommandRuntime3 != null)
				{
					mshCommandRuntime3.WriteDebug(record2, overrideInquire);
					return;
				}
				break;
			}
			case PSStreamObjectType.Progress:
			{
				ProgressRecord progressRecord = (ProgressRecord)this.Value;
				RemotingProgressRecord remotingProgressRecord = progressRecord as RemotingProgressRecord;
				if (remotingProgressRecord == null)
				{
					Guid instanceID2;
					string computerName2;
					PSStreamObject.GetIdentifierInfo(progressRecord.CurrentOperation, out instanceID2, out computerName2);
					OriginInfo originInfo = new OriginInfo(computerName2, Guid.Empty, instanceID2);
					progressRecord = new RemotingProgressRecord(progressRecord, originInfo);
				}
				else
				{
					progressRecord = remotingProgressRecord;
				}
				MshCommandRuntime mshCommandRuntime4 = cmdlet.CommandRuntime as MshCommandRuntime;
				if (mshCommandRuntime4 != null)
				{
					mshCommandRuntime4.WriteProgress(progressRecord, overrideInquire);
					return;
				}
				break;
			}
			case PSStreamObjectType.Verbose:
			{
				string message3 = (string)this.Value;
				VerboseRecord record3 = new VerboseRecord(message3);
				MshCommandRuntime mshCommandRuntime5 = cmdlet.CommandRuntime as MshCommandRuntime;
				if (mshCommandRuntime5 != null)
				{
					mshCommandRuntime5.WriteVerbose(record3, overrideInquire);
					return;
				}
				break;
			}
			case PSStreamObjectType.Information:
			{
				InformationRecord informationRecord = (InformationRecord)this.Value;
				RemotingInformationRecord remotingInformationRecord = informationRecord as RemotingInformationRecord;
				if (remotingInformationRecord == null)
				{
					if (!string.IsNullOrEmpty(informationRecord.Source))
					{
						Guid instanceID3;
						string computerName3;
						PSStreamObject.GetIdentifierInfo(informationRecord.Source, out instanceID3, out computerName3);
						informationRecord = new RemotingInformationRecord(informationRecord, new OriginInfo(computerName3, Guid.Empty, instanceID3));
					}
				}
				else
				{
					informationRecord = remotingInformationRecord;
				}
				MshCommandRuntime mshCommandRuntime6 = cmdlet.CommandRuntime as MshCommandRuntime;
				if (mshCommandRuntime6 != null)
				{
					mshCommandRuntime6.WriteInformation(informationRecord, overrideInquire);
					return;
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000BFA5A File Offset: 0x000BDC5A
		internal void WriteStreamObject(Cmdlet cmdlet, bool writeSourceIdentifier, bool overrideInquire)
		{
			if (writeSourceIdentifier)
			{
				this.WriteStreamObject(cmdlet, this.Id, overrideInquire);
				return;
			}
			this.WriteStreamObject(cmdlet, overrideInquire);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000BFA78 File Offset: 0x000BDC78
		private static void InvokeCmdletMethodAndWaitForResults<T>(CmdletMethodInvoker<T> cmdletMethodInvoker, Cmdlet cmdlet)
		{
			cmdletMethodInvoker.MethodResult = default(T);
			try
			{
				T methodResult = cmdletMethodInvoker.Action(cmdlet);
				lock (cmdletMethodInvoker.SyncObject)
				{
					cmdletMethodInvoker.MethodResult = methodResult;
				}
			}
			catch (Exception exceptionThrownOnCmdletThread)
			{
				lock (cmdletMethodInvoker.SyncObject)
				{
					cmdletMethodInvoker.ExceptionThrownOnCmdletThread = exceptionThrownOnCmdletThread;
				}
				throw;
			}
			finally
			{
				if (cmdletMethodInvoker.Finished != null)
				{
					cmdletMethodInvoker.Finished.Set();
				}
			}
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000BFB40 File Offset: 0x000BDD40
		internal static void AddSourceJobNoteProperty(PSObject psObj, Guid instanceId)
		{
			if (psObj.Properties[RemotingConstants.SourceJobInstanceId] != null)
			{
				psObj.Properties.Remove(RemotingConstants.SourceJobInstanceId);
			}
			psObj.Properties.Add(new PSNoteProperty(RemotingConstants.SourceJobInstanceId, instanceId));
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000BFB80 File Offset: 0x000BDD80
		internal static string CreateInformationalMessage(Guid instanceId, string message)
		{
			StringBuilder stringBuilder = new StringBuilder(instanceId.ToString());
			stringBuilder.Append(":");
			stringBuilder.Append(message);
			return stringBuilder.ToString();
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000BFBBA File Offset: 0x000BDDBA
		internal static ErrorRecord AddSourceTagToError(ErrorRecord errorRecord, Guid sourceId)
		{
			if (errorRecord == null)
			{
				return null;
			}
			if (errorRecord.ErrorDetails == null)
			{
				errorRecord.ErrorDetails = new ErrorDetails(string.Empty);
			}
			errorRecord.ErrorDetails.RecommendedAction = PSStreamObject.CreateInformationalMessage(sourceId, errorRecord.ErrorDetails.RecommendedAction);
			return errorRecord;
		}
	}
}
