using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000067 RID: 103
	internal static class ArgumentsExtensions
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x0000A7D8 File Offset: 0x000089D8
		public static IEnumerable<Error> Preprocess(this IEnumerable<string> arguments, IEnumerable<Func<IEnumerable<string>, IEnumerable<Error>>> preprocessorLookup)
		{
			return preprocessorLookup.TryHead<Func<IEnumerable<string>, IEnumerable<Error>>>().MapValueOrDefault(delegate(Func<IEnumerable<string>, IEnumerable<Error>> func)
			{
				IEnumerable<Error> enumerable = func(arguments);
				if (!enumerable.Any<Error>())
				{
					return arguments.Preprocess(preprocessorLookup.TailNoFail<Func<IEnumerable<string>, IEnumerable<Error>>>());
				}
				return enumerable;
			}, Enumerable.Empty<Error>());
		}
	}
}
