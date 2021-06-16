using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000064 RID: 100
	[Serializable]
	public class ScriptRequiresException : RuntimeException
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x00019D20 File Offset: 0x00017F20
		internal ScriptRequiresException(string commandName, string requiresShellId, string requiresShellPath, string errorId) : base(ScriptRequiresException.BuildMessage(commandName, requiresShellId, requiresShellPath, true))
		{
			this._commandName = commandName;
			this._requiresShellId = requiresShellId;
			this._requiresShellPath = requiresShellPath;
			base.SetErrorId(errorId);
			base.SetTargetObject(commandName);
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00019D84 File Offset: 0x00017F84
		internal ScriptRequiresException(string commandName, Version requiresPSVersion, string currentPSVersion, string errorId) : base(ScriptRequiresException.BuildMessage(commandName, requiresPSVersion.ToString(), currentPSVersion, false))
		{
			this._commandName = commandName;
			this._requiresPSVersion = requiresPSVersion;
			base.SetErrorId(errorId);
			base.SetTargetObject(commandName);
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00019DE6 File Offset: 0x00017FE6
		internal ScriptRequiresException(string commandName, Collection<string> missingItems, string errorId, bool forSnapins) : this(commandName, missingItems, errorId, forSnapins, null)
		{
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00019DF4 File Offset: 0x00017FF4
		internal ScriptRequiresException(string commandName, Collection<string> missingItems, string errorId, bool forSnapins, ErrorRecord errorRecord) : base(ScriptRequiresException.BuildMessage(commandName, missingItems, forSnapins), null, errorRecord)
		{
			this._commandName = commandName;
			this._missingPSSnapIns = new ReadOnlyCollection<string>(missingItems);
			base.SetErrorId(errorId);
			base.SetTargetObject(commandName);
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00019E58 File Offset: 0x00018058
		internal ScriptRequiresException(string commandName, string errorId) : base(ScriptRequiresException.BuildMessage(commandName))
		{
			this._commandName = commandName;
			base.SetErrorId(errorId);
			base.SetTargetObject(commandName);
			base.SetErrorCategory(ErrorCategory.PermissionDenied);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00019EAA File Offset: 0x000180AA
		public ScriptRequiresException()
		{
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00019ECE File Offset: 0x000180CE
		public ScriptRequiresException(string message) : base(message)
		{
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00019EF3 File Offset: 0x000180F3
		public ScriptRequiresException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00019F1C File Offset: 0x0001811C
		protected ScriptRequiresException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._commandName = info.GetString("CommandName");
			this._requiresPSVersion = (Version)info.GetValue("RequiresPSVersion", typeof(Version));
			this._missingPSSnapIns = (ReadOnlyCollection<string>)info.GetValue("MissingPSSnapIns", typeof(ReadOnlyCollection<string>));
			this._requiresShellId = info.GetString("RequiresShellId");
			this._requiresShellPath = info.GetString("RequiresShellPath");
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00019FC0 File Offset: 0x000181C0
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("CommandName", this._commandName);
			info.AddValue("RequiresPSVersion", this._requiresPSVersion, typeof(Version));
			info.AddValue("MissingPSSnapIns", this._missingPSSnapIns, typeof(ReadOnlyCollection<string>));
			info.AddValue("RequiresShellId", this._requiresShellId);
			info.AddValue("RequiresShellPath", this._requiresShellPath);
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0001A04C File Offset: 0x0001824C
		public string CommandName
		{
			get
			{
				return this._commandName;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0001A054 File Offset: 0x00018254
		public Version RequiresPSVersion
		{
			get
			{
				return this._requiresPSVersion;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0001A05C File Offset: 0x0001825C
		public ReadOnlyCollection<string> MissingPSSnapIns
		{
			get
			{
				return this._missingPSSnapIns;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0001A064 File Offset: 0x00018264
		public string RequiresShellId
		{
			get
			{
				return this._requiresShellId;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0001A06C File Offset: 0x0001826C
		public string RequiresShellPath
		{
			get
			{
				return this._requiresShellPath;
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001A074 File Offset: 0x00018274
		private static string BuildMessage(string commandName, Collection<string> missingItems, bool forSnapins)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (missingItems == null)
			{
				throw PSTraceSource.NewArgumentNullException("missingItems");
			}
			foreach (string value in missingItems)
			{
				stringBuilder.Append(value).Append(", ");
			}
			if (stringBuilder.Length > 1)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			if (forSnapins)
			{
				return StringUtil.Format(DiscoveryExceptions.RequiresMissingPSSnapIns, commandName, stringBuilder.ToString());
			}
			return StringUtil.Format(DiscoveryExceptions.RequiresMissingModules, commandName, stringBuilder.ToString());
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001A11C File Offset: 0x0001831C
		private static string BuildMessage(string commandName, string first, string second, bool forShellId)
		{
			string formatSpec;
			if (forShellId)
			{
				if (string.IsNullOrEmpty(first))
				{
					formatSpec = DiscoveryExceptions.RequiresShellIDInvalidForSingleShell;
				}
				else
				{
					formatSpec = (string.IsNullOrEmpty(second) ? DiscoveryExceptions.RequiresInterpreterNotCompatibleNoPath : DiscoveryExceptions.RequiresInterpreterNotCompatible);
				}
			}
			else
			{
				formatSpec = DiscoveryExceptions.RequiresPSVersionNotCompatible;
			}
			return StringUtil.Format(formatSpec, new object[]
			{
				commandName,
				first,
				second
			});
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001A175 File Offset: 0x00018375
		private static string BuildMessage(string commandName)
		{
			return StringUtil.Format(DiscoveryExceptions.RequiresElevation, commandName);
		}

		// Token: 0x04000232 RID: 562
		private string _commandName = string.Empty;

		// Token: 0x04000233 RID: 563
		private Version _requiresPSVersion;

		// Token: 0x04000234 RID: 564
		private ReadOnlyCollection<string> _missingPSSnapIns = new ReadOnlyCollection<string>(new string[0]);

		// Token: 0x04000235 RID: 565
		private string _requiresShellId;

		// Token: 0x04000236 RID: 566
		private string _requiresShellPath;
	}
}
