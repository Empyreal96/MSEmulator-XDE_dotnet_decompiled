using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommandLine
{
	// Token: 0x02000041 RID: 65
	public static class HelpTextExtensions
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00005450 File Offset: 0x00003650
		public static bool IsHelp(this IEnumerable<Error> errs)
		{
			if (errs.Any((Error x) => x.Tag == ErrorType.HelpRequestedError || x.Tag == ErrorType.HelpVerbRequestedError))
			{
				return true;
			}
			return errs.Any(delegate(Error x)
			{
				UnknownOptionError unknownOptionError = x as UnknownOptionError;
				return ((unknownOptionError != null) ? unknownOptionError.Token : "") == "help";
			});
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000054AC File Offset: 0x000036AC
		public static bool IsVersion(this IEnumerable<Error> errs)
		{
			if (errs.Any((Error x) => x.Tag == ErrorType.VersionRequestedError))
			{
				return true;
			}
			return errs.Any(delegate(Error x)
			{
				UnknownOptionError unknownOptionError = x as UnknownOptionError;
				return ((unknownOptionError != null) ? unknownOptionError.Token : "") == "version";
			});
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005507 File Offset: 0x00003707
		public static TextWriter Output(this IEnumerable<Error> errs)
		{
			if (errs.IsHelp() || errs.IsVersion())
			{
				return Console.Out;
			}
			return Console.Error;
		}
	}
}
