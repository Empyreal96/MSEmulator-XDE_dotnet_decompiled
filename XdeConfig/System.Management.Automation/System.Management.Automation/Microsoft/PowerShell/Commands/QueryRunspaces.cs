using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces;
using System.Xml;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000330 RID: 816
	internal class QueryRunspaces
	{
		// Token: 0x0600277C RID: 10108 RVA: 0x000DD112 File Offset: 0x000DB312
		internal QueryRunspaces()
		{
			this.stopProcessing = false;
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000DD164 File Offset: 0x000DB364
		internal Collection<PSSession> GetDisconnectedSessions(Collection<WSManConnectionInfo> connectionInfos, PSHost host, ObjectStream stream, RunspaceRepository runspaceRepository, int throttleLimit, SessionFilterState filterState, Guid[] matchIds, string[] matchNames, string configurationName)
		{
			Collection<PSSession> collection = new Collection<PSSession>();
			foreach (WSManConnectionInfo wsmanConnectionInfo in connectionInfos)
			{
				Runspace[] array = null;
				try
				{
					array = Runspace.GetRunspaces(wsmanConnectionInfo, host, QueryRunspaces.BuiltInTypesTable);
				}
				catch (RuntimeException ex)
				{
					if (!(ex.InnerException is InvalidOperationException))
					{
						throw;
					}
					if (stream.ObjectWriter != null && stream.ObjectWriter.IsOpen)
					{
						int transportErrorCode;
						string message = StringUtil.Format(RemotingErrorIdStrings.QueryForRunspacesFailed, wsmanConnectionInfo.ComputerName, QueryRunspaces.ExtractMessage(ex.InnerException, out transportErrorCode));
						string fqeidfromTransportError = WSManTransportManagerUtils.GetFQEIDFromTransportError(transportErrorCode, "RemotePSSessionQueryFailed");
						Exception exception = new RuntimeException(message, ex.InnerException);
						ErrorRecord errorRecord = new ErrorRecord(exception, fqeidfromTransportError, ErrorCategory.InvalidOperation, wsmanConnectionInfo);
						stream.ObjectWriter.Write(new Action<Cmdlet>(delegate(Cmdlet cmdlet)
						{
							cmdlet.WriteError(errorRecord);
						}));
					}
				}
				if (this.stopProcessing)
				{
					break;
				}
				if (array != null)
				{
					string text = null;
					if (!string.IsNullOrEmpty(configurationName))
					{
						text = ((configurationName.IndexOf("http://schemas.microsoft.com/powershell/", StringComparison.OrdinalIgnoreCase) != -1) ? configurationName : ("http://schemas.microsoft.com/powershell/" + configurationName));
					}
					Runspace[] array2 = array;
					int i = 0;
					while (i < array2.Length)
					{
						Runspace runspace = array2[i];
						if (text == null)
						{
							goto IL_141;
						}
						WSManConnectionInfo wsmanConnectionInfo2 = runspace.ConnectionInfo as WSManConnectionInfo;
						if (wsmanConnectionInfo2 == null || text.Equals(wsmanConnectionInfo2.ShellUri, StringComparison.OrdinalIgnoreCase))
						{
							goto IL_141;
						}
						IL_1A5:
						i++;
						continue;
						IL_141:
						PSSession pssession = null;
						if (runspaceRepository != null)
						{
							pssession = runspaceRepository.GetItem(runspace.InstanceId);
						}
						if (pssession != null && QueryRunspaces.UseExistingRunspace(pssession.Runspace, runspace))
						{
							if (this.TestRunspaceState(pssession.Runspace, filterState))
							{
								collection.Add(pssession);
								goto IL_1A5;
							}
							goto IL_1A5;
						}
						else
						{
							if (this.TestRunspaceState(runspace, filterState))
							{
								collection.Add(new PSSession(runspace as RemoteRunspace));
								goto IL_1A5;
							}
							goto IL_1A5;
						}
					}
				}
			}
			if (matchIds != null && collection.Count > 0)
			{
				Collection<PSSession> collection2 = new Collection<PSSession>();
				foreach (Guid guid in matchIds)
				{
					bool flag = false;
					foreach (PSSession pssession2 in collection)
					{
						if (this.stopProcessing)
						{
							break;
						}
						if (pssession2.Runspace.InstanceId.Equals(guid))
						{
							flag = true;
							collection2.Add(pssession2);
							break;
						}
					}
					if (!flag && stream.ObjectWriter != null && stream.ObjectWriter.IsOpen)
					{
						string message2 = StringUtil.Format(RemotingErrorIdStrings.SessionIdMatchFailed, guid);
						Exception exception2 = new RuntimeException(message2);
						ErrorRecord errorRecord = new ErrorRecord(exception2, "PSSessionIdMatchFail", ErrorCategory.InvalidOperation, guid);
						stream.ObjectWriter.Write(new Action<Cmdlet>(delegate(Cmdlet cmdlet)
						{
							cmdlet.WriteError(errorRecord);
						}));
					}
				}
				return collection2;
			}
			if (matchNames != null && collection.Count > 0)
			{
				Collection<PSSession> collection3 = new Collection<PSSession>();
				foreach (string text2 in matchNames)
				{
					WildcardPattern wildcardPattern = new WildcardPattern(text2, WildcardOptions.IgnoreCase);
					bool flag2 = false;
					foreach (PSSession pssession3 in collection)
					{
						if (this.stopProcessing)
						{
							break;
						}
						if (wildcardPattern.IsMatch(((RemoteRunspace)pssession3.Runspace).RunspacePool.RemoteRunspacePoolInternal.Name))
						{
							flag2 = true;
							collection3.Add(pssession3);
						}
					}
					if (!flag2 && stream.ObjectWriter != null && stream.ObjectWriter.IsOpen)
					{
						string message3 = StringUtil.Format(RemotingErrorIdStrings.SessionNameMatchFailed, text2);
						Exception exception3 = new RuntimeException(message3);
						ErrorRecord errorRecord = new ErrorRecord(exception3, "PSSessionNameMatchFail", ErrorCategory.InvalidOperation, text2);
						stream.ObjectWriter.Write(new Action<Cmdlet>(delegate(Cmdlet cmdlet)
						{
							cmdlet.WriteError(errorRecord);
						}));
					}
				}
				return collection3;
			}
			return collection;
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000DD5D4 File Offset: 0x000DB7D4
		private static bool UseExistingRunspace(Runspace existingRunspace, Runspace queriedrunspace)
		{
			if (existingRunspace.RunspaceStateInfo.State == RunspaceState.Broken)
			{
				return false;
			}
			if (existingRunspace.RunspaceStateInfo.State == RunspaceState.Disconnected && queriedrunspace.RunspaceAvailability == RunspaceAvailability.Busy)
			{
				return false;
			}
			existingRunspace.DisconnectedOn = queriedrunspace.DisconnectedOn;
			existingRunspace.ExpiresOn = queriedrunspace.ExpiresOn;
			return true;
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000DD624 File Offset: 0x000DB824
		internal static string ExtractMessage(Exception e, out int errorCode)
		{
			errorCode = 0;
			if (e == null || e.Message == null)
			{
				return string.Empty;
			}
			string text = null;
			try
			{
				XmlReaderSettings xmlReaderSettings = InternalDeserializer.XmlReaderSettingsForUntrustedXmlDocument.Clone();
				xmlReaderSettings.MaxCharactersInDocument = 4096L;
				xmlReaderSettings.MaxCharactersFromEntities = 1024L;
				xmlReaderSettings.DtdProcessing = DtdProcessing.Prohibit;
				using (XmlReader xmlReader = XmlReader.Create(new StringReader(e.Message), xmlReaderSettings))
				{
					while (xmlReader.Read())
					{
						if (xmlReader.NodeType == XmlNodeType.Element)
						{
							if (xmlReader.LocalName.Equals("Message", StringComparison.OrdinalIgnoreCase))
							{
								text = xmlReader.ReadElementContentAsString();
							}
							else if (xmlReader.LocalName.Equals("WSManFault", StringComparison.OrdinalIgnoreCase))
							{
								string attribute = xmlReader.GetAttribute("Code");
								if (attribute != null)
								{
									try
									{
										long num = Convert.ToInt64(attribute, NumberFormatInfo.InvariantInfo);
										errorCode = (int)num;
									}
									catch (FormatException)
									{
									}
									catch (OverflowException)
									{
									}
								}
							}
						}
					}
				}
			}
			catch (XmlException)
			{
			}
			if (text == null)
			{
				return e.Message;
			}
			return text;
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000DD740 File Offset: 0x000DB940
		internal void StopAllOperations()
		{
			this.stopProcessing = true;
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000DD74C File Offset: 0x000DB94C
		private bool TestRunspaceState(Runspace runspace, SessionFilterState filterState)
		{
			bool result;
			switch (filterState)
			{
			case SessionFilterState.All:
				result = true;
				break;
			case SessionFilterState.Opened:
				result = (runspace.RunspaceStateInfo.State == RunspaceState.Opened);
				break;
			case SessionFilterState.Disconnected:
				result = (runspace.RunspaceStateInfo.State == RunspaceState.Disconnected);
				break;
			case SessionFilterState.Closed:
				result = (runspace.RunspaceStateInfo.State == RunspaceState.Closed);
				break;
			case SessionFilterState.Broken:
				result = (runspace.RunspaceStateInfo.State == RunspaceState.Broken);
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x000DD7C4 File Offset: 0x000DB9C4
		internal static TypeTable BuiltInTypesTable
		{
			get
			{
				if (QueryRunspaces.s_TypeTable == null)
				{
					lock (QueryRunspaces.s_SyncObject)
					{
						if (QueryRunspaces.s_TypeTable == null)
						{
							QueryRunspaces.s_TypeTable = TypeTable.LoadDefaultTypeFiles();
						}
					}
				}
				return QueryRunspaces.s_TypeTable;
			}
		}

		// Token: 0x04001384 RID: 4996
		private bool stopProcessing;

		// Token: 0x04001385 RID: 4997
		private static readonly object s_SyncObject = new object();

		// Token: 0x04001386 RID: 4998
		private static TypeTable s_TypeTable;
	}
}
