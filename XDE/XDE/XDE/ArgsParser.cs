using System;
using System.Collections.Generic;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000022 RID: 34
	public class ArgsParser
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x0000886D File Offset: 0x00006A6D
		public static IEnumerable<ParsedArg> ParseArgs(string[] args)
		{
			int num;
			for (int i = 0; i < args.Length; i = num + 1)
			{
				ParsedArg parsedArg = new ParsedArg();
				string text = args[i];
				if (ArgsParser.IsNamedArg(text))
				{
					parsedArg.Name = text.Substring(1);
				}
				else
				{
					parsedArg.AddValue(text);
				}
				for (int j = i + 1; j < args.Length; j++)
				{
					string text2 = args[j];
					if (ArgsParser.IsNamedArg(text2))
					{
						break;
					}
					parsedArg.AddValue(text2);
					i = j;
				}
				yield return parsedArg;
				num = i;
			}
			yield break;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008880 File Offset: 0x00006A80
		private static bool IsNamedArg(string arg)
		{
			char c = arg[0];
			return c == '/' || c == '-';
		}
	}
}
