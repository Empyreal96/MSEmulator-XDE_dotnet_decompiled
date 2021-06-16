using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace System.Management.Automation
{
	// Token: 0x0200004C RID: 76
	public class ApplicationInfo : CommandInfo
	{
		// Token: 0x060003F6 RID: 1014 RVA: 0x0000E398 File Offset: 0x0000C598
		internal ApplicationInfo(string name, string path, ExecutionContext context) : base(name, CommandTypes.Application)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			this.path = path;
			this.extension = System.IO.Path.GetExtension(path);
			this.context = context;
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0000E3FF File Offset: 0x0000C5FF
		public string Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0000E407 File Offset: 0x0000C607
		public string Extension
		{
			get
			{
				return this.extension;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0000E40F File Offset: 0x0000C60F
		public override string Definition
		{
			get
			{
				return this.Path;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000E417 File Offset: 0x0000C617
		public override string Source
		{
			get
			{
				return this.Definition;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000E420 File Offset: 0x0000C620
		public override Version Version
		{
			get
			{
				if (this._version == null)
				{
					FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(this.path);
					this._version = new Version(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, versionInfo.ProductPrivatePart);
				}
				return this._version;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0000E470 File Offset: 0x0000C670
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x0000E488 File Offset: 0x0000C688
		public override SessionStateEntryVisibility Visibility
		{
			get
			{
				return this.context.EngineSessionState.CheckApplicationVisibility(this.path);
			}
			set
			{
				throw PSTraceSource.NewNotImplementedException();
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0000E490 File Offset: 0x0000C690
		public override ReadOnlyCollection<PSTypeName> OutputType
		{
			get
			{
				if (this._outputType == null)
				{
					this._outputType = new ReadOnlyCollection<PSTypeName>(new List<PSTypeName>
					{
						new PSTypeName(typeof(string))
					});
				}
				return this._outputType;
			}
		}

		// Token: 0x04000171 RID: 369
		private ExecutionContext context;

		// Token: 0x04000172 RID: 370
		private string path = string.Empty;

		// Token: 0x04000173 RID: 371
		private string extension = string.Empty;

		// Token: 0x04000174 RID: 372
		private Version _version;

		// Token: 0x04000175 RID: 373
		private ReadOnlyCollection<PSTypeName> _outputType;
	}
}
