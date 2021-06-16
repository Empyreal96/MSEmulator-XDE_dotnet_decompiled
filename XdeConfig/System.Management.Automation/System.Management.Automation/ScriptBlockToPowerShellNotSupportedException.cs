using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000491 RID: 1169
	[Serializable]
	public class ScriptBlockToPowerShellNotSupportedException : RuntimeException
	{
		// Token: 0x0600342B RID: 13355 RVA: 0x0011CC2C File Offset: 0x0011AE2C
		public ScriptBlockToPowerShellNotSupportedException() : base(typeof(ScriptBlockToPowerShellNotSupportedException).FullName)
		{
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x0011CC43 File Offset: 0x0011AE43
		public ScriptBlockToPowerShellNotSupportedException(string message) : base(message)
		{
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x0011CC4C File Offset: 0x0011AE4C
		public ScriptBlockToPowerShellNotSupportedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x0011CC56 File Offset: 0x0011AE56
		internal ScriptBlockToPowerShellNotSupportedException(string errorId, Exception innerException, string message, params object[] arguments) : base(string.Format(CultureInfo.CurrentCulture, message, arguments), innerException)
		{
			base.SetErrorId(errorId);
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x0011CC73 File Offset: 0x0011AE73
		protected ScriptBlockToPowerShellNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
