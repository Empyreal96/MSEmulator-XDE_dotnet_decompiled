using System;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000038 RID: 56
	public sealed class ProxyCommand
	{
		// Token: 0x060002D9 RID: 729 RVA: 0x0000A551 File Offset: 0x00008751
		private ProxyCommand()
		{
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000A559 File Offset: 0x00008759
		public static string Create(CommandMetadata commandMetadata)
		{
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetaData");
			}
			return commandMetadata.GetProxyCommand("", true);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000A575 File Offset: 0x00008775
		public static string Create(CommandMetadata commandMetadata, string helpComment)
		{
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetaData");
			}
			return commandMetadata.GetProxyCommand(helpComment, true);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000A58D File Offset: 0x0000878D
		public static string Create(CommandMetadata commandMetadata, string helpComment, bool generateDynamicParameters)
		{
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetaData");
			}
			return commandMetadata.GetProxyCommand(helpComment, generateDynamicParameters);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000A5A5 File Offset: 0x000087A5
		public static string GetCmdletBindingAttribute(CommandMetadata commandMetadata)
		{
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetaData");
			}
			return commandMetadata.GetDecl();
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000A5BB File Offset: 0x000087BB
		public static string GetParamBlock(CommandMetadata commandMetadata)
		{
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetaData");
			}
			return commandMetadata.GetParamBlock();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000A5D1 File Offset: 0x000087D1
		public static string GetBegin(CommandMetadata commandMetadata)
		{
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetaData");
			}
			return commandMetadata.GetBeginBlock();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000A5E7 File Offset: 0x000087E7
		public static string GetProcess(CommandMetadata commandMetadata)
		{
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetaData");
			}
			return commandMetadata.GetProcessBlock();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000A5FD File Offset: 0x000087FD
		public static string GetDynamicParam(CommandMetadata commandMetadata)
		{
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetaData");
			}
			return commandMetadata.GetDynamicParamBlock();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000A613 File Offset: 0x00008813
		public static string GetEnd(CommandMetadata commandMetadata)
		{
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetaData");
			}
			return commandMetadata.GetEndBlock();
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000A62C File Offset: 0x0000882C
		private static T GetProperty<T>(PSObject obj, string property) where T : class
		{
			T result = default(T);
			if (obj != null && obj.Properties[property] != null)
			{
				result = (obj.Properties[property].Value as T);
			}
			return result;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000A670 File Offset: 0x00008870
		private static string GetObjText(object obj)
		{
			string text = null;
			PSObject psobject = obj as PSObject;
			if (psobject != null)
			{
				text = ProxyCommand.GetProperty<string>(psobject, "Text");
			}
			if (text == null)
			{
				text = obj.ToString();
			}
			return text;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000A6A0 File Offset: 0x000088A0
		private static void AppendContent(StringBuilder sb, string section, object obj)
		{
			if (obj != null)
			{
				string objText = ProxyCommand.GetObjText(obj);
				if (!string.IsNullOrEmpty(objText))
				{
					sb.Append("\n");
					sb.Append(section);
					sb.Append("\n\n");
					sb.Append(objText);
					sb.Append("\n");
				}
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000A6F4 File Offset: 0x000088F4
		private static void AppendContent(StringBuilder sb, string section, PSObject[] array)
		{
			if (array != null)
			{
				bool flag = true;
				foreach (PSObject obj in array)
				{
					string objText = ProxyCommand.GetObjText(obj);
					if (!string.IsNullOrEmpty(objText))
					{
						if (flag)
						{
							flag = false;
							sb.Append("\n\n");
							sb.Append(section);
							sb.Append("\n\n");
						}
						sb.Append(objText);
						sb.Append("\n");
					}
				}
				if (!flag)
				{
					sb.Append("\n");
				}
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000A778 File Offset: 0x00008978
		private static void AppendType(StringBuilder sb, string section, PSObject parent)
		{
			PSObject property = ProxyCommand.GetProperty<PSObject>(parent, "type");
			PSObject property2 = ProxyCommand.GetProperty<PSObject>(property, "name");
			if (property2 != null)
			{
				sb.Append("\n\n");
				sb.Append(section);
				sb.Append("\n\n");
				sb.Append(ProxyCommand.GetObjText(property2));
				sb.Append("\n");
				return;
			}
			PSObject property3 = ProxyCommand.GetProperty<PSObject>(property, "uri");
			if (property3 != null)
			{
				sb.Append("\n\n");
				sb.Append(section);
				sb.Append("\n\n");
				sb.Append(ProxyCommand.GetObjText(property3));
				sb.Append("\n");
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000A824 File Offset: 0x00008A24
		public static string GetHelpComments(PSObject help)
		{
			if (help == null)
			{
				throw new ArgumentNullException("help");
			}
			bool flag = false;
			foreach (string text in help.InternalTypeNames)
			{
				if (text.Contains("HelpInfo"))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				string helpInfoObjectRequired = ProxyCommandStrings.HelpInfoObjectRequired;
				throw new InvalidOperationException(helpInfoObjectRequired);
			}
			StringBuilder stringBuilder = new StringBuilder();
			ProxyCommand.AppendContent(stringBuilder, ".SYNOPSIS", ProxyCommand.GetProperty<string>(help, "Synopsis"));
			ProxyCommand.AppendContent(stringBuilder, ".DESCRIPTION", ProxyCommand.GetProperty<PSObject[]>(help, "Description"));
			PSObject property = ProxyCommand.GetProperty<PSObject>(help, "Parameters");
			PSObject[] property2 = ProxyCommand.GetProperty<PSObject[]>(property, "Parameter");
			if (property2 != null)
			{
				foreach (PSObject obj in property2)
				{
					PSObject property3 = ProxyCommand.GetProperty<PSObject>(obj, "Name");
					PSObject[] property4 = ProxyCommand.GetProperty<PSObject[]>(obj, "Description");
					stringBuilder.Append("\n.PARAMETER ");
					stringBuilder.Append(property3);
					stringBuilder.Append("\n\n");
					foreach (PSObject psobject in property4)
					{
						string text2 = ProxyCommand.GetProperty<string>(psobject, "Text");
						if (text2 == null)
						{
							text2 = psobject.ToString();
						}
						if (!string.IsNullOrEmpty(text2))
						{
							stringBuilder.Append(text2);
							stringBuilder.Append("\n");
						}
					}
				}
			}
			PSObject property5 = ProxyCommand.GetProperty<PSObject>(help, "examples");
			PSObject[] property6 = ProxyCommand.GetProperty<PSObject[]>(property5, "example");
			if (property6 != null)
			{
				foreach (PSObject obj2 in property6)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					PSObject[] property7 = ProxyCommand.GetProperty<PSObject[]>(obj2, "introduction");
					if (property7 != null)
					{
						foreach (PSObject psobject2 in property7)
						{
							if (psobject2 != null)
							{
								stringBuilder2.Append(ProxyCommand.GetObjText(psobject2));
							}
						}
					}
					PSObject property8 = ProxyCommand.GetProperty<PSObject>(obj2, "code");
					if (property8 != null)
					{
						stringBuilder2.Append(property8.ToString());
					}
					PSObject[] property9 = ProxyCommand.GetProperty<PSObject[]>(obj2, "remarks");
					if (property9 != null)
					{
						stringBuilder2.Append("\n");
						foreach (PSObject obj3 in property9)
						{
							string property10 = ProxyCommand.GetProperty<string>(obj3, "text");
							stringBuilder2.Append(property10.ToString());
						}
					}
					if (stringBuilder2.Length > 0)
					{
						stringBuilder.Append("\n\n.EXAMPLE\n\n");
						stringBuilder.Append(stringBuilder2.ToString());
					}
				}
			}
			PSObject property11 = ProxyCommand.GetProperty<PSObject>(help, "alertSet");
			ProxyCommand.AppendContent(stringBuilder, ".NOTES", ProxyCommand.GetProperty<PSObject[]>(property11, "alert"));
			PSObject property12 = ProxyCommand.GetProperty<PSObject>(help, "inputTypes");
			PSObject property13 = ProxyCommand.GetProperty<PSObject>(property12, "inputType");
			ProxyCommand.AppendType(stringBuilder, ".INPUTS", property13);
			PSObject property14 = ProxyCommand.GetProperty<PSObject>(help, "returnValues");
			PSObject property15 = ProxyCommand.GetProperty<PSObject>(property14, "returnValue");
			ProxyCommand.AppendType(stringBuilder, ".OUTPUTS", property15);
			PSObject property16 = ProxyCommand.GetProperty<PSObject>(help, "relatedLinks");
			PSObject[] property17 = ProxyCommand.GetProperty<PSObject[]>(property16, "navigationLink");
			if (property17 != null)
			{
				foreach (PSObject obj4 in property17)
				{
					ProxyCommand.AppendContent(stringBuilder, ".LINK", ProxyCommand.GetProperty<PSObject>(obj4, "uri"));
					ProxyCommand.AppendContent(stringBuilder, ".LINK", ProxyCommand.GetProperty<PSObject>(obj4, "linkText"));
				}
			}
			ProxyCommand.AppendContent(stringBuilder, ".COMPONENT", ProxyCommand.GetProperty<PSObject>(help, "Component"));
			ProxyCommand.AppendContent(stringBuilder, ".ROLE", ProxyCommand.GetProperty<PSObject>(help, "Role"));
			ProxyCommand.AppendContent(stringBuilder, ".FUNCTIONALITY", ProxyCommand.GetProperty<PSObject>(help, "Functionality"));
			return stringBuilder.ToString();
		}
	}
}
