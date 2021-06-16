using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Host;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000967 RID: 2407
	public sealed class FormatTable
	{
		// Token: 0x06005841 RID: 22593 RVA: 0x001CB205 File Offset: 0x001C9405
		internal FormatTable()
		{
			this.formatDBMgr = new TypeInfoDataBaseManager();
		}

		// Token: 0x06005842 RID: 22594 RVA: 0x001CB218 File Offset: 0x001C9418
		public FormatTable(IEnumerable<string> formatFiles) : this(formatFiles, null, null)
		{
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x001CB223 File Offset: 0x001C9423
		public void AppendFormatData(IEnumerable<ExtendedTypeDefinition> formatData)
		{
			if (formatData == null)
			{
				throw PSTraceSource.NewArgumentNullException("formatData");
			}
			this.formatDBMgr.AddFormatData(formatData, false);
		}

		// Token: 0x06005844 RID: 22596 RVA: 0x001CB240 File Offset: 0x001C9440
		public void PrependFormatData(IEnumerable<ExtendedTypeDefinition> formatData)
		{
			if (formatData == null)
			{
				throw PSTraceSource.NewArgumentNullException("formatData");
			}
			this.formatDBMgr.AddFormatData(formatData, true);
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x001CB25D File Offset: 0x001C945D
		internal FormatTable(IEnumerable<string> formatFiles, AuthorizationManager authorizationManager, PSHost host)
		{
			if (formatFiles == null)
			{
				throw PSTraceSource.NewArgumentNullException("formatFiles");
			}
			this.formatDBMgr = new TypeInfoDataBaseManager(formatFiles, true, authorizationManager, host);
		}

		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x06005846 RID: 22598 RVA: 0x001CB282 File Offset: 0x001C9482
		internal TypeInfoDataBaseManager FormatDBManager
		{
			get
			{
				return this.formatDBMgr;
			}
		}

		// Token: 0x06005847 RID: 22599 RVA: 0x001CB28A File Offset: 0x001C948A
		internal void Add(string formatFile, bool shouldPrepend)
		{
			this.formatDBMgr.Add(formatFile, shouldPrepend);
		}

		// Token: 0x06005848 RID: 22600 RVA: 0x001CB299 File Offset: 0x001C9499
		internal void Remove(string formatFile)
		{
			this.formatDBMgr.Remove(formatFile);
		}

		// Token: 0x06005849 RID: 22601 RVA: 0x001CB2C0 File Offset: 0x001C94C0
		public static FormatTable LoadDefaultFormatFiles()
		{
			string defaultPowerShellShellID = Utils.DefaultPowerShellShellID;
			string psHome = Utils.GetApplicationBase(defaultPowerShellShellID);
			List<string> list = new List<string>();
			List<string> source = new List<string>
			{
				"Certificate.Format.ps1xml",
				"Event.Format.ps1xml",
				"Diagnostics.Format.ps1xml",
				"DotNetTypes.Format.ps1xml",
				"FileSystem.Format.ps1xml",
				"Help.Format.ps1xml",
				"HelpV3.Format.ps1xml",
				"PowerShellCore.format.ps1xml",
				"PowerShellTrace.format.ps1xml",
				"Registry.format.ps1xml",
				"WSMan.Format.ps1xml"
			};
			if (!string.IsNullOrEmpty(psHome))
			{
				list.AddRange(from file in source
				select Path.Combine(psHome, file));
			}
			return new FormatTable(list);
		}

		// Token: 0x04002F57 RID: 12119
		private TypeInfoDataBaseManager formatDBMgr;
	}
}
