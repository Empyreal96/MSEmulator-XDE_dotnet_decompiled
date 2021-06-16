using System;

namespace System.Management.Automation
{
	// Token: 0x02000092 RID: 146
	internal static class StringToMinishellStreamConverter
	{
		// Token: 0x06000738 RID: 1848 RVA: 0x00022DA8 File Offset: 0x00020FA8
		internal static MinishellStream ToMinishellStream(string stream)
		{
			MinishellStream result = MinishellStream.Unknown;
			if ("output".Equals(stream, StringComparison.OrdinalIgnoreCase))
			{
				result = MinishellStream.Output;
			}
			else if ("error".Equals(stream, StringComparison.OrdinalIgnoreCase))
			{
				result = MinishellStream.Error;
			}
			else if ("debug".Equals(stream, StringComparison.OrdinalIgnoreCase))
			{
				result = MinishellStream.Debug;
			}
			else if ("verbose".Equals(stream, StringComparison.OrdinalIgnoreCase))
			{
				result = MinishellStream.Verbose;
			}
			else if ("warning".Equals(stream, StringComparison.OrdinalIgnoreCase))
			{
				result = MinishellStream.Warning;
			}
			else if ("progress".Equals(stream, StringComparison.OrdinalIgnoreCase))
			{
				result = MinishellStream.Progress;
			}
			else if ("information".Equals(stream, StringComparison.OrdinalIgnoreCase))
			{
				result = MinishellStream.Information;
			}
			return result;
		}

		// Token: 0x04000322 RID: 802
		internal const string OutputStream = "output";

		// Token: 0x04000323 RID: 803
		internal const string ErrorStream = "error";

		// Token: 0x04000324 RID: 804
		internal const string DebugStream = "debug";

		// Token: 0x04000325 RID: 805
		internal const string VerboseStream = "verbose";

		// Token: 0x04000326 RID: 806
		internal const string WarningStream = "warning";

		// Token: 0x04000327 RID: 807
		internal const string ProgressStream = "progress";

		// Token: 0x04000328 RID: 808
		internal const string InformationStream = "information";
	}
}
