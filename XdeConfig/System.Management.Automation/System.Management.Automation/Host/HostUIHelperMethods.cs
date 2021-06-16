using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace System.Management.Automation.Host
{
	// Token: 0x02000228 RID: 552
	internal static class HostUIHelperMethods
	{
		// Token: 0x060019FE RID: 6654 RVA: 0x0009B110 File Offset: 0x00099310
		internal static void BuildHotkeysAndPlainLabels(Collection<ChoiceDescription> choices, out string[,] hotkeysAndPlainLabels)
		{
			hotkeysAndPlainLabels = new string[2, choices.Count];
			for (int i = 0; i < choices.Count; i++)
			{
				hotkeysAndPlainLabels[0, i] = string.Empty;
				int num = choices[i].Label.IndexOf('&');
				if (num >= 0)
				{
					StringBuilder stringBuilder = new StringBuilder(choices[i].Label.Substring(0, num), choices[i].Label.Length);
					if (num + 1 < choices[i].Label.Length)
					{
						stringBuilder.Append(choices[i].Label.Substring(num + 1));
						hotkeysAndPlainLabels[0, i] = CultureInfo.CurrentCulture.TextInfo.ToUpper(choices[i].Label.Substring(num + 1, 1).Trim());
					}
					hotkeysAndPlainLabels[1, i] = stringBuilder.ToString().Trim();
				}
				else
				{
					hotkeysAndPlainLabels[1, i] = choices[i].Label;
				}
				if (string.Compare(hotkeysAndPlainLabels[0, i], "?", StringComparison.Ordinal) == 0)
				{
					Exception ex = PSTraceSource.NewArgumentException(string.Format(CultureInfo.InvariantCulture, "choices[{0}].Label", new object[]
					{
						i
					}), InternalHostUserInterfaceStrings.InvalidChoiceHotKeyError, new object[0]);
					throw ex;
				}
			}
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0009B270 File Offset: 0x00099470
		internal static int DetermineChoicePicked(string response, Collection<ChoiceDescription> choices, string[,] hotkeysAndPlainLabels)
		{
			int num = -1;
			for (int i = 0; i < choices.Count; i++)
			{
				if (string.Compare(response, hotkeysAndPlainLabels[1, i], StringComparison.CurrentCultureIgnoreCase) == 0)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				for (int j = 0; j < choices.Count; j++)
				{
					if (hotkeysAndPlainLabels[0, j].Length > 0 && string.Compare(response, hotkeysAndPlainLabels[0, j], StringComparison.CurrentCultureIgnoreCase) == 0)
					{
						num = j;
						break;
					}
				}
			}
			return num;
		}
	}
}
