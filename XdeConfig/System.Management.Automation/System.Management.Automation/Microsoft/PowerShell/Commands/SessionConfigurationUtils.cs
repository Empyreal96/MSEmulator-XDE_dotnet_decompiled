using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Remoting;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000352 RID: 850
	internal class SessionConfigurationUtils
	{
		// Token: 0x06002A5A RID: 10842 RVA: 0x000EA2A4 File Offset: 0x000E84A4
		internal static string ConfigFragment(string key, string resourceString, string value, StreamWriter streamWriter, bool isExample)
		{
			string newLine = streamWriter.NewLine;
			if (isExample)
			{
				return string.Format(CultureInfo.InvariantCulture, "# {0}{1}# {2:19} = {3}{4}{5}", new object[]
				{
					resourceString,
					newLine,
					key,
					value,
					newLine,
					newLine
				});
			}
			return string.Format(CultureInfo.InvariantCulture, "# {0}{1}{2:19} = {3}{4}{5}", new object[]
			{
				resourceString,
				newLine,
				key,
				value,
				newLine,
				newLine
			});
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x000EA31B File Offset: 0x000E851B
		internal static string QuoteName(object name)
		{
			if (name == null)
			{
				return "''";
			}
			return "'" + CodeGeneration.EscapeSingleQuotedStringContent(name.ToString()) + "'";
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x000EA340 File Offset: 0x000E8540
		internal static string WrapScriptBlock(object sb)
		{
			if (sb == null)
			{
				return "{}";
			}
			return "{" + sb.ToString() + "}";
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000EA360 File Offset: 0x000E8560
		internal static string WriteBoolean(bool booleanToEmit)
		{
			if (booleanToEmit)
			{
				return "$true";
			}
			return "$false";
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000EA370 File Offset: 0x000E8570
		internal static string GetVisibilityDefault(object[] values, StreamWriter writer, PSCmdlet caller)
		{
			if (values != null && values.Length > 0)
			{
				return SessionConfigurationUtils.CombineHashTableOrStringArray(values, writer, caller);
			}
			return "'Item1', 'Item2'";
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000EA38C File Offset: 0x000E858C
		internal static string CombineHashtable(IDictionary table, StreamWriter writer, int? indent = 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("@{");
			IOrderedEnumerable<string> orderedEnumerable = from string x in table.Keys
			orderby x
			select x;
			foreach (string text in orderedEnumerable)
			{
				stringBuilder.Append(writer.NewLine);
				stringBuilder.AppendFormat("{0," + 4 * (indent + 1) + "}", "");
				stringBuilder.Append(SessionConfigurationUtils.QuoteName(text));
				stringBuilder.Append(" = ");
				if (table[text] is ScriptBlock)
				{
					stringBuilder.Append(SessionConfigurationUtils.WrapScriptBlock(table[text].ToString()));
				}
				else
				{
					IDictionary dictionary = table[text] as IDictionary;
					if (dictionary != null)
					{
						stringBuilder.Append(SessionConfigurationUtils.CombineHashtable(dictionary, writer, indent + 1));
					}
					else
					{
						IDictionary[] array = DISCPowerShellConfiguration.TryGetHashtableArray(table[text]);
						if (array != null)
						{
							stringBuilder.Append(SessionConfigurationUtils.CombineHashtableArray(array, writer, indent + 1));
						}
						else
						{
							string[] array2 = DISCPowerShellConfiguration.TryGetStringArray(table[text]);
							if (array2 != null)
							{
								stringBuilder.Append(SessionConfigurationUtils.CombineStringArray(array2));
							}
							else
							{
								stringBuilder.Append(SessionConfigurationUtils.QuoteName(table[text]));
							}
						}
					}
				}
			}
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000EA5B8 File Offset: 0x000E87B8
		internal static string CombineHashtableArray(IDictionary[] tables, StreamWriter writer, int? indent = 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < tables.Length; i++)
			{
				stringBuilder.Append(SessionConfigurationUtils.CombineHashtable(tables[i], writer, indent));
				if (i < tables.Length - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000EA604 File Offset: 0x000E8804
		internal static string CombineStringArray(string[] values)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < values.Length; i++)
			{
				if (!string.IsNullOrEmpty(values[i]))
				{
					stringBuilder.Append(SessionConfigurationUtils.QuoteName(values[i]));
					if (i < values.Length - 1)
					{
						stringBuilder.Append(", ");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000EA658 File Offset: 0x000E8858
		internal static string CombineHashTableOrStringArray(object[] values, StreamWriter writer, PSCmdlet caller)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < values.Length; i++)
			{
				string text = values[i] as string;
				if (!string.IsNullOrEmpty(text))
				{
					stringBuilder.Append(SessionConfigurationUtils.QuoteName(text));
				}
				else
				{
					Hashtable hashtable = values[i] as Hashtable;
					if (hashtable == null)
					{
						string message = StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeStringOrHashtableArray, ConfigFileConstants.ModulesToImport);
						PSArgumentException ex = new PSArgumentException(message);
						caller.ThrowTerminatingError(ex.ErrorRecord);
					}
					stringBuilder.Append(SessionConfigurationUtils.CombineHashtable(hashtable, writer, new int?(0)));
				}
				if (i < values.Length - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
