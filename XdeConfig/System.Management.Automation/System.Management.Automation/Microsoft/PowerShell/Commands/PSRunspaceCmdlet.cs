using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200032A RID: 810
	public abstract class PSRunspaceCmdlet : PSRemotingCmdlet
	{
		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06002714 RID: 10004 RVA: 0x000DB1C3 File Offset: 0x000D93C3
		// (set) Token: 0x06002715 RID: 10005 RVA: 0x000DB1CB File Offset: 0x000D93CB
		[Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "InstanceId")]
		[ValidateNotNull]
		public virtual Guid[] InstanceId
		{
			get
			{
				return this.remoteRunspaceIds;
			}
			set
			{
				this.remoteRunspaceIds = value;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06002716 RID: 10006 RVA: 0x000DB1D4 File Offset: 0x000D93D4
		// (set) Token: 0x06002717 RID: 10007 RVA: 0x000DB1DC File Offset: 0x000D93DC
		[ValidateNotNull]
		[Parameter(Position = 0, ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "Id")]
		public int[] Id
		{
			get
			{
				return this.sessionIds;
			}
			set
			{
				this.sessionIds = value;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06002718 RID: 10008 RVA: 0x000DB1E5 File Offset: 0x000D93E5
		// (set) Token: 0x06002719 RID: 10009 RVA: 0x000DB1ED File Offset: 0x000D93ED
		[Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Name")]
		[ValidateNotNullOrEmpty]
		public virtual string[] Name
		{
			get
			{
				return this.names;
			}
			set
			{
				this.names = value;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x0600271A RID: 10010 RVA: 0x000DB1F6 File Offset: 0x000D93F6
		// (set) Token: 0x0600271B RID: 10011 RVA: 0x000DB1FE File Offset: 0x000D93FE
		[Alias(new string[]
		{
			"Cn"
		})]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[ValidateNotNullOrEmpty]
		public virtual string[] ComputerName
		{
			get
			{
				return this.computerNames;
			}
			set
			{
				this.computerNames = value;
			}
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000DB208 File Offset: 0x000D9408
		protected Dictionary<Guid, PSSession> GetMatchingRunspaces(bool writeobject, bool writeErrorOnNoMatch)
		{
			string parameterSetName;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (parameterSetName == "ComputerName")
				{
					return this.GetMatchingRunspacesByComputerName(writeobject, writeErrorOnNoMatch);
				}
				if (parameterSetName == "InstanceId")
				{
					return this.GetMatchingRunspacesByRunspaceId(writeobject, writeErrorOnNoMatch);
				}
				if (parameterSetName == "Name")
				{
					return this.GetMatchingRunspacesByName(writeobject, writeErrorOnNoMatch);
				}
				if (parameterSetName == "Id")
				{
					return this.GetMatchingRunspacesBySessionId(writeobject, writeErrorOnNoMatch);
				}
			}
			return null;
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000DB27C File Offset: 0x000D947C
		internal Dictionary<Guid, PSSession> GetAllRunspaces(bool writeobject, bool writeErrorOnNoMatch)
		{
			Dictionary<Guid, PSSession> dictionary = new Dictionary<Guid, PSSession>();
			List<PSSession> runspaces = base.RunspaceRepository.Runspaces;
			foreach (PSSession pssession in runspaces)
			{
				if (writeobject)
				{
					base.WriteObject(pssession);
				}
				else
				{
					dictionary.Add(pssession.InstanceId, pssession);
				}
			}
			return dictionary;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000DB2F0 File Offset: 0x000D94F0
		private Dictionary<Guid, PSSession> GetMatchingRunspacesByComputerName(bool writeobject, bool writeErrorOnNoMatch)
		{
			if (this.computerNames == null || this.computerNames.Length == 0)
			{
				return this.GetAllRunspaces(writeobject, writeErrorOnNoMatch);
			}
			Dictionary<Guid, PSSession> dictionary = new Dictionary<Guid, PSSession>();
			List<PSSession> runspaces = base.RunspaceRepository.Runspaces;
			foreach (string text in this.computerNames)
			{
				WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
				bool flag = false;
				foreach (PSSession pssession in runspaces)
				{
					if (wildcardPattern.IsMatch(pssession.ComputerName))
					{
						flag = true;
						if (writeobject)
						{
							base.WriteObject(pssession);
						}
						else
						{
							try
							{
								dictionary.Add(pssession.InstanceId, pssession);
							}
							catch (ArgumentException)
							{
							}
						}
					}
				}
				if (!flag && writeErrorOnNoMatch)
				{
					this.WriteInvalidArgumentError(PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedComputer, RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedComputer, text);
				}
			}
			return dictionary;
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000DB3F0 File Offset: 0x000D95F0
		protected Dictionary<Guid, PSSession> GetMatchingRunspacesByName(bool writeobject, bool writeErrorOnNoMatch)
		{
			Dictionary<Guid, PSSession> dictionary = new Dictionary<Guid, PSSession>();
			List<PSSession> runspaces = base.RunspaceRepository.Runspaces;
			foreach (string text in this.names)
			{
				WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
				bool flag = false;
				foreach (PSSession pssession in runspaces)
				{
					if (wildcardPattern.IsMatch(pssession.Name))
					{
						flag = true;
						if (writeobject)
						{
							base.WriteObject(pssession);
						}
						else
						{
							try
							{
								dictionary.Add(pssession.InstanceId, pssession);
							}
							catch (ArgumentException)
							{
							}
						}
					}
				}
				if (!flag && writeErrorOnNoMatch && !WildcardPattern.ContainsWildcardCharacters(text))
				{
					this.WriteInvalidArgumentError(PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedName, RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedName, text);
				}
			}
			return dictionary;
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000DB4E0 File Offset: 0x000D96E0
		protected Dictionary<Guid, PSSession> GetMatchingRunspacesByRunspaceId(bool writeobject, bool writeErrorOnNoMatch)
		{
			Dictionary<Guid, PSSession> dictionary = new Dictionary<Guid, PSSession>();
			List<PSSession> runspaces = base.RunspaceRepository.Runspaces;
			foreach (Guid guid in this.remoteRunspaceIds)
			{
				bool flag = false;
				foreach (PSSession pssession in runspaces)
				{
					if (guid.Equals(pssession.InstanceId))
					{
						flag = true;
						if (writeobject)
						{
							base.WriteObject(pssession);
						}
						else
						{
							try
							{
								dictionary.Add(pssession.InstanceId, pssession);
							}
							catch (ArgumentException)
							{
							}
						}
					}
				}
				if (!flag && writeErrorOnNoMatch)
				{
					this.WriteInvalidArgumentError(PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedRunspaceId, RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedRunspaceId, guid);
				}
			}
			return dictionary;
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000DB5CC File Offset: 0x000D97CC
		private Dictionary<Guid, PSSession> GetMatchingRunspacesBySessionId(bool writeobject, bool writeErrorOnNoMatch)
		{
			Dictionary<Guid, PSSession> dictionary = new Dictionary<Guid, PSSession>();
			List<PSSession> runspaces = base.RunspaceRepository.Runspaces;
			foreach (int num in this.sessionIds)
			{
				bool flag = false;
				foreach (PSSession pssession in runspaces)
				{
					if (num == pssession.Id)
					{
						flag = true;
						if (writeobject)
						{
							base.WriteObject(pssession);
						}
						else
						{
							try
							{
								dictionary.Add(pssession.InstanceId, pssession);
							}
							catch (ArgumentException)
							{
							}
						}
					}
				}
				if (!flag && writeErrorOnNoMatch)
				{
					this.WriteInvalidArgumentError(PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedSessionId, RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedSessionId, num);
				}
			}
			return dictionary;
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000DB6A8 File Offset: 0x000D98A8
		private void WriteInvalidArgumentError(PSRemotingErrorId errorId, string resourceString, object errorArgument)
		{
			string message = base.GetMessage(resourceString, new object[]
			{
				errorArgument
			});
			base.WriteError(new ErrorRecord(new ArgumentException(message), errorId.ToString(), ErrorCategory.InvalidArgument, errorArgument));
		}

		// Token: 0x04001350 RID: 4944
		protected const string InstanceIdParameterSet = "InstanceId";

		// Token: 0x04001351 RID: 4945
		protected const string IdParameterSet = "Id";

		// Token: 0x04001352 RID: 4946
		protected const string NameParameterSet = "Name";

		// Token: 0x04001353 RID: 4947
		private Guid[] remoteRunspaceIds;

		// Token: 0x04001354 RID: 4948
		private int[] sessionIds;

		// Token: 0x04001355 RID: 4949
		private string[] names;

		// Token: 0x04001356 RID: 4950
		private string[] computerNames;
	}
}
