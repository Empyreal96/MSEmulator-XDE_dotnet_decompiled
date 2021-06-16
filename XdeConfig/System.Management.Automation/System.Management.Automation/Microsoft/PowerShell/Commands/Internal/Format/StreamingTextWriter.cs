using System;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Text;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000526 RID: 1318
	internal class StreamingTextWriter : TextWriter
	{
		// Token: 0x0600372A RID: 14122 RVA: 0x00129760 File Offset: 0x00127960
		internal StreamingTextWriter(StreamingTextWriter.WriteLineCallback writeCall, CultureInfo culture) : base(culture)
		{
			if (writeCall == null)
			{
				throw PSTraceSource.NewArgumentNullException("writeCall");
			}
			this.writeCall = writeCall;
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x0600372B RID: 14123 RVA: 0x0012977E File Offset: 0x0012797E
		public override Encoding Encoding
		{
			get
			{
				return new UnicodeEncoding();
			}
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x00129785 File Offset: 0x00127985
		public override void WriteLine(string s)
		{
			this.writeCall(s);
		}

		// Token: 0x04001C3D RID: 7229
		[TraceSource("StreamingTextWriter", "StreamingTextWriter")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("StreamingTextWriter", "StreamingTextWriter");

		// Token: 0x04001C3E RID: 7230
		private StreamingTextWriter.WriteLineCallback writeCall;

		// Token: 0x02000527 RID: 1319
		// (Invoke) Token: 0x0600372F RID: 14127
		internal delegate void WriteLineCallback(string s);
	}
}
