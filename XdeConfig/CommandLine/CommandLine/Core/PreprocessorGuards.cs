using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Core
{
	// Token: 0x02000070 RID: 112
	internal static class PreprocessorGuards
	{
		// Token: 0x060002BA RID: 698 RVA: 0x0000B170 File Offset: 0x00009370
		public static IEnumerable<Func<IEnumerable<string>, IEnumerable<Error>>> Lookup(StringComparer nameComparer, bool autoHelp, bool autoVersion)
		{
			List<Func<IEnumerable<string>, IEnumerable<Error>>> list = new List<Func<IEnumerable<string>, IEnumerable<Error>>>();
			if (autoHelp)
			{
				list.Add(PreprocessorGuards.HelpCommand(nameComparer));
			}
			if (autoVersion)
			{
				list.Add(PreprocessorGuards.VersionCommand(nameComparer));
			}
			return list;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000B1A2 File Offset: 0x000093A2
		public static Func<IEnumerable<string>, IEnumerable<Error>> HelpCommand(StringComparer nameComparer)
		{
			return delegate(IEnumerable<string> arguments)
			{
				if (!nameComparer.Equals("--help", arguments.First<string>()))
				{
					return Enumerable.Empty<Error>();
				}
				return new Error[]
				{
					new HelpRequestedError()
				};
			};
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000B1BB File Offset: 0x000093BB
		public static Func<IEnumerable<string>, IEnumerable<Error>> VersionCommand(StringComparer nameComparer)
		{
			return delegate(IEnumerable<string> arguments)
			{
				if (!nameComparer.Equals("--version", arguments.First<string>()))
				{
					return Enumerable.Empty<Error>();
				}
				return new Error[]
				{
					new VersionRequestedError()
				};
			};
		}
	}
}
