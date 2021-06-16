using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000023 RID: 35
	internal static class EnumMinimumDisambiguation
	{
		// Token: 0x0600015E RID: 350 RVA: 0x00006AE8 File Offset: 0x00004CE8
		static EnumMinimumDisambiguation()
		{
			EnumMinimumDisambiguation.specialDisambiguateCases.Add(typeof(FileAttributes), new string[]
			{
				"Directory",
				"ReadOnly",
				"System"
			});
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006B34 File Offset: 0x00004D34
		internal static string EnumDisambiguate(string text, Type enumType)
		{
			string[] names = Enum.GetNames(enumType);
			List<string> list = new List<string>();
			foreach (string text2 in names)
			{
				if (text2.StartsWith(text, StringComparison.OrdinalIgnoreCase))
				{
					list.Add(text2);
				}
			}
			if (list.Count == 0)
			{
				throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "NoEnumNameMatch", EnumExpressionEvaluatorStrings.NoEnumNameMatch, new object[]
				{
					text,
					EnumMinimumDisambiguation.EnumAllValues(enumType)
				});
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			foreach (string text3 in list)
			{
				if (text3.Equals(text, StringComparison.OrdinalIgnoreCase))
				{
					return text3;
				}
			}
			string[] array2;
			if (EnumMinimumDisambiguation.specialDisambiguateCases.TryGetValue(enumType, out array2))
			{
				foreach (string text4 in array2)
				{
					if (text4.StartsWith(text, StringComparison.OrdinalIgnoreCase))
					{
						return text4;
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder(list[0]);
			string value = ", ";
			for (int k = 1; k < list.Count; k++)
			{
				stringBuilder.Append(value);
				stringBuilder.Append(list[k]);
			}
			throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "MultipleEnumNameMatch", EnumExpressionEvaluatorStrings.MultipleEnumNameMatch, new object[]
			{
				text,
				stringBuilder.ToString()
			});
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006CD4 File Offset: 0x00004ED4
		internal static string EnumAllValues(Type enumType)
		{
			string[] names = Enum.GetNames(enumType);
			string text = ", ";
			StringBuilder stringBuilder = new StringBuilder();
			if (names.Length != 0)
			{
				for (int i = 0; i < names.Length; i++)
				{
					stringBuilder.Append(names[i]);
					stringBuilder.Append(text);
				}
				stringBuilder.Remove(stringBuilder.Length - text.Length, text.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400006F RID: 111
		private static Dictionary<Type, string[]> specialDisambiguateCases = new Dictionary<Type, string[]>();
	}
}
