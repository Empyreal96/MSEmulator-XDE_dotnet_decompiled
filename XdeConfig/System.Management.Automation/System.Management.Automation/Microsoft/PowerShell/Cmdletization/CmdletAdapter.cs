using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Cmdletization
{
	// Token: 0x020009A4 RID: 2468
	public abstract class CmdletAdapter<TObjectInstance> where TObjectInstance : class
	{
		// Token: 0x06005ADD RID: 23261 RVA: 0x001E8B20 File Offset: 0x001E6D20
		internal void Initialize(PSCmdlet cmdlet, string className, string classVersion, IDictionary<string, string> privateData)
		{
			if (cmdlet == null)
			{
				throw new ArgumentNullException("cmdlet");
			}
			if (string.IsNullOrEmpty(className))
			{
				throw new ArgumentNullException("className");
			}
			if (classVersion == null)
			{
				throw new ArgumentNullException("classVersion");
			}
			if (privateData == null)
			{
				throw new ArgumentNullException("privateData");
			}
			this.cmdlet = cmdlet;
			this.className = className;
			this.classVersion = classVersion;
			this.privateData = privateData;
			PSScriptCmdlet psscriptCmdlet = this.Cmdlet as PSScriptCmdlet;
			if (psscriptCmdlet != null)
			{
				psscriptCmdlet.StoppingEvent += delegate(object param0, EventArgs param1)
				{
					this.StopProcessing();
				};
				psscriptCmdlet.DisposingEvent += delegate(object param0, EventArgs param1)
				{
					IDisposable disposable = this as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				};
			}
		}

		// Token: 0x06005ADE RID: 23262 RVA: 0x001E8BC9 File Offset: 0x001E6DC9
		public void Initialize(PSCmdlet cmdlet, string className, string classVersion, Version moduleVersion, IDictionary<string, string> privateData)
		{
			this.moduleVersion = moduleVersion;
			this.Initialize(cmdlet, className, classVersion, privateData);
		}

		// Token: 0x06005ADF RID: 23263 RVA: 0x001E8BDE File Offset: 0x001E6DDE
		public virtual QueryBuilder GetQueryBuilder()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005AE0 RID: 23264 RVA: 0x001E8BE5 File Offset: 0x001E6DE5
		public virtual void ProcessRecord(QueryBuilder query)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x001E8BEC File Offset: 0x001E6DEC
		public virtual void BeginProcessing()
		{
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x001E8BEE File Offset: 0x001E6DEE
		public virtual void EndProcessing()
		{
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x001E8BF0 File Offset: 0x001E6DF0
		public virtual void StopProcessing()
		{
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x001E8BF2 File Offset: 0x001E6DF2
		public virtual void ProcessRecord(TObjectInstance objectInstance, MethodInvocationInfo methodInvocationInfo, bool passThru)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x001E8BF9 File Offset: 0x001E6DF9
		public virtual void ProcessRecord(QueryBuilder query, MethodInvocationInfo methodInvocationInfo, bool passThru)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005AE6 RID: 23270 RVA: 0x001E8C00 File Offset: 0x001E6E00
		public virtual void ProcessRecord(MethodInvocationInfo methodInvocationInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06005AE7 RID: 23271 RVA: 0x001E8C07 File Offset: 0x001E6E07
		public PSCmdlet Cmdlet
		{
			get
			{
				return this.cmdlet;
			}
		}

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06005AE8 RID: 23272 RVA: 0x001E8C0F File Offset: 0x001E6E0F
		public string ClassName
		{
			get
			{
				return this.className;
			}
		}

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06005AE9 RID: 23273 RVA: 0x001E8C17 File Offset: 0x001E6E17
		public string ClassVersion
		{
			get
			{
				return this.classVersion;
			}
		}

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06005AEA RID: 23274 RVA: 0x001E8C1F File Offset: 0x001E6E1F
		public Version ModuleVersion
		{
			get
			{
				return this.moduleVersion;
			}
		}

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06005AEB RID: 23275 RVA: 0x001E8C27 File Offset: 0x001E6E27
		public IDictionary<string, string> PrivateData
		{
			get
			{
				return this.privateData;
			}
		}

		// Token: 0x040030AB RID: 12459
		private PSCmdlet cmdlet;

		// Token: 0x040030AC RID: 12460
		private string className;

		// Token: 0x040030AD RID: 12461
		private string classVersion;

		// Token: 0x040030AE RID: 12462
		private Version moduleVersion;

		// Token: 0x040030AF RID: 12463
		private IDictionary<string, string> privateData;
	}
}
