using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x0200007C RID: 124
	internal static class Switch
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0000BF1C File Offset: 0x0000A11C
		public static IEnumerable<Token> Partition(IEnumerable<Token> tokens, Func<string, Maybe<TypeDescriptor>> typeLookup)
		{
			return from t in tokens
			where typeLookup(t.Text).MapValueOrDefault((TypeDescriptor info) => t.IsName() && info.TargetType == TargetType.Switch, false)
			select t;
		}
	}
}
