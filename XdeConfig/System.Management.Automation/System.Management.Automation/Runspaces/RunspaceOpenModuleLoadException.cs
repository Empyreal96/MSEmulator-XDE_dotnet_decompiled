using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000217 RID: 535
	[Serializable]
	public class RunspaceOpenModuleLoadException : RuntimeException
	{
		// Token: 0x0600191A RID: 6426 RVA: 0x00098401 File Offset: 0x00096601
		public RunspaceOpenModuleLoadException() : base(typeof(ScriptBlockToPowerShellNotSupportedException).FullName)
		{
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x00098418 File Offset: 0x00096618
		public RunspaceOpenModuleLoadException(string message) : base(message)
		{
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x00098421 File Offset: 0x00096621
		public RunspaceOpenModuleLoadException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0009842C File Offset: 0x0009662C
		internal RunspaceOpenModuleLoadException(string moduleName, PSDataCollection<ErrorRecord> errors) : base(StringUtil.Format(RunspaceStrings.ErrorLoadingModulesOnRunspaceOpen, moduleName, (errors != null && errors.Count > 0 && errors[0] != null) ? errors[0].ToString() : string.Empty), null)
		{
			this._errors = errors;
			base.SetErrorId("ErrorLoadingModulesOnRunspaceOpen");
			base.SetErrorCategory(ErrorCategory.OpenError);
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600191E RID: 6430 RVA: 0x0009848C File Offset: 0x0009668C
		public PSDataCollection<ErrorRecord> ErrorRecords
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x00098494 File Offset: 0x00096694
		protected RunspaceOpenModuleLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0009849E File Offset: 0x0009669E
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
		}

		// Token: 0x04000A56 RID: 2646
		private PSDataCollection<ErrorRecord> _errors;
	}
}
