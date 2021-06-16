using System;
using System.Management.Automation.Language;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000280 RID: 640
	[Serializable]
	public class JobFailedException : SystemException
	{
		// Token: 0x06001E5F RID: 7775 RVA: 0x000B0858 File Offset: 0x000AEA58
		public JobFailedException()
		{
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000B0860 File Offset: 0x000AEA60
		public JobFailedException(string message) : base(message)
		{
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000B0869 File Offset: 0x000AEA69
		public JobFailedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000B0873 File Offset: 0x000AEA73
		public JobFailedException(Exception innerException, ScriptExtent displayScriptPosition)
		{
			this.reason = innerException;
			this.displayScriptPosition = displayScriptPosition;
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x000B088C File Offset: 0x000AEA8C
		protected JobFailedException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
			this.reason = (Exception)serializationInfo.GetValue("Reason", typeof(Exception));
			this.displayScriptPosition = (ScriptExtent)serializationInfo.GetValue("DisplayScriptPosition", typeof(ScriptExtent));
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x000B08E1 File Offset: 0x000AEAE1
		public Exception Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x000B08E9 File Offset: 0x000AEAE9
		public ScriptExtent DisplayScriptPosition
		{
			get
			{
				return this.displayScriptPosition;
			}
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x000B08F1 File Offset: 0x000AEAF1
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("Reason", this.reason);
			info.AddValue("DisplayScriptPosition", this.displayScriptPosition);
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x000B092B File Offset: 0x000AEB2B
		public override string Message
		{
			get
			{
				return this.Reason.Message;
			}
		}

		// Token: 0x04000D68 RID: 3432
		private Exception reason;

		// Token: 0x04000D69 RID: 3433
		private ScriptExtent displayScriptPosition;
	}
}
