using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Internal;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x0200007A RID: 122
	public class CommandParameterSetInfo
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x0001F38C File Offset: 0x0001D58C
		internal CommandParameterSetInfo(string name, bool isDefaultParameterSet, uint parameterSetFlag, MergedCommandParameterMetadata parameterMetadata)
		{
			this.IsDefault = true;
			this.Name = string.Empty;
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (parameterMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterMetadata");
			}
			this.Name = name;
			this.IsDefault = isDefaultParameterSet;
			this.Initialize(parameterMetadata, parameterSetFlag);
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0001F3EA File Offset: 0x0001D5EA
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x0001F3F2 File Offset: 0x0001D5F2
		public string Name { get; private set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x0001F3FB File Offset: 0x0001D5FB
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x0001F403 File Offset: 0x0001D603
		public bool IsDefault { get; private set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0001F40C File Offset: 0x0001D60C
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x0001F414 File Offset: 0x0001D614
		public ReadOnlyCollection<CommandParameterInfo> Parameters { get; private set; }

		// Token: 0x0600066B RID: 1643 RVA: 0x0001F41D File Offset: 0x0001D61D
		public override string ToString()
		{
			return this.ToString(false);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001F498 File Offset: 0x0001D698
		internal string ToString(bool isCapabilityWorkflow)
		{
			StringBuilder result = new StringBuilder();
			this.GenerateParametersInDisplayOrder(isCapabilityWorkflow, delegate(CommandParameterInfo parameter)
			{
				CommandParameterSetInfo.AppendFormatCommandParameterInfo(parameter, ref result);
			}, delegate(string str)
			{
				if (result.Length > 0)
				{
					result.Append(" ");
				}
				result.Append("[");
				result.Append(str);
				result.Append("]");
			});
			return result.ToString();
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001F4E0 File Offset: 0x0001D6E0
		internal void GenerateParametersInDisplayOrder(bool isCapabilityWorkflow, Action<CommandParameterInfo> parameterAction, Action<string> commonParameterAction)
		{
			List<CommandParameterInfo> list = new List<CommandParameterInfo>();
			List<CommandParameterInfo> list2 = new List<CommandParameterInfo>();
			List<CommandParameterInfo> list3 = new List<CommandParameterInfo>();
			foreach (CommandParameterInfo commandParameterInfo in this.Parameters)
			{
				if (commandParameterInfo.Position == -2147483648)
				{
					if (commandParameterInfo.IsMandatory)
					{
						list2.Add(commandParameterInfo);
					}
					else
					{
						list3.Add(commandParameterInfo);
					}
				}
				else
				{
					if (commandParameterInfo.Position >= list.Count)
					{
						for (int i = list.Count; i <= commandParameterInfo.Position; i++)
						{
							list.Add(null);
						}
					}
					list[commandParameterInfo.Position] = commandParameterInfo;
				}
			}
			List<CommandParameterInfo> list4 = new List<CommandParameterInfo>();
			foreach (CommandParameterInfo commandParameterInfo2 in list)
			{
				if (commandParameterInfo2 != null)
				{
					if (!CommonParameters.CommonWorkflowParameters.Contains(commandParameterInfo2.Name, StringComparer.OrdinalIgnoreCase) || !isCapabilityWorkflow)
					{
						parameterAction(commandParameterInfo2);
					}
					else
					{
						list4.Add(commandParameterInfo2);
					}
				}
			}
			foreach (CommandParameterInfo commandParameterInfo3 in list2)
			{
				if (commandParameterInfo3 != null)
				{
					parameterAction(commandParameterInfo3);
				}
			}
			List<CommandParameterInfo> list5 = new List<CommandParameterInfo>();
			foreach (CommandParameterInfo commandParameterInfo4 in list3)
			{
				if (commandParameterInfo4 != null)
				{
					if (!Cmdlet.CommonParameters.Contains(commandParameterInfo4.Name, StringComparer.OrdinalIgnoreCase))
					{
						if (!CommonParameters.CommonWorkflowParameters.Contains(commandParameterInfo4.Name, StringComparer.OrdinalIgnoreCase) || !isCapabilityWorkflow)
						{
							parameterAction(commandParameterInfo4);
						}
						else
						{
							list4.Add(commandParameterInfo4);
						}
					}
					else
					{
						list5.Add(commandParameterInfo4);
					}
				}
			}
			if (list4.Count == CommonParameters.CommonWorkflowParameters.Length)
			{
				commonParameterAction(HelpDisplayStrings.CommonWorkflowParameters);
			}
			else
			{
				foreach (CommandParameterInfo obj in list4)
				{
					parameterAction(obj);
				}
			}
			if (list5.Count == Cmdlet.CommonParameters.Count)
			{
				commonParameterAction(HelpDisplayStrings.CommonParameters);
				return;
			}
			foreach (CommandParameterInfo obj2 in list5)
			{
				parameterAction(obj2);
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001F7AC File Offset: 0x0001D9AC
		private static void AppendFormatCommandParameterInfo(CommandParameterInfo parameter, ref StringBuilder result)
		{
			if (result.Length > 0)
			{
				result.Append(" ");
			}
			if (parameter.ParameterType == typeof(SwitchParameter))
			{
				result.AppendFormat(CultureInfo.InvariantCulture, parameter.IsMandatory ? "-{0}" : "[-{0}]", new object[]
				{
					parameter.Name
				});
				return;
			}
			string parameterTypeString = CommandParameterSetInfo.GetParameterTypeString(parameter.ParameterType, parameter.Attributes);
			if (parameter.IsMandatory)
			{
				result.AppendFormat(CultureInfo.InvariantCulture, (parameter.Position != int.MinValue) ? "[-{0}] <{1}>" : "-{0} <{1}>", new object[]
				{
					parameter.Name,
					parameterTypeString
				});
				return;
			}
			result.AppendFormat(CultureInfo.InvariantCulture, (parameter.Position != int.MinValue) ? "[[-{0}] <{1}>]" : "[-{0} <{1}>]", new object[]
			{
				parameter.Name,
				parameterTypeString
			});
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001F8AC File Offset: 0x0001DAAC
		internal static string GetParameterTypeString(Type type, IEnumerable<Attribute> attributes)
		{
			PSTypeNameAttribute pstypeNameAttribute;
			string text;
			if (attributes != null && (pstypeNameAttribute = attributes.OfType<PSTypeNameAttribute>().FirstOrDefault<PSTypeNameAttribute>()) != null)
			{
				Match match = Regex.Match(pstypeNameAttribute.PSTypeName, "(.*\\.)?(?<NetTypeName>.*)#(.*[/\\\\])?(?<CimClassName>.*)");
				if (match.Success)
				{
					text = match.Groups["NetTypeName"].Value + "#" + match.Groups["CimClassName"].Value;
				}
				else
				{
					text = pstypeNameAttribute.PSTypeName;
					int num = text.LastIndexOfAny(new char[]
					{
						'.'
					});
					if (num != -1 && num + 1 < text.Length)
					{
						text = text.Substring(num + 1);
					}
				}
				if (type.IsArray && text.IndexOf("[]", StringComparison.OrdinalIgnoreCase) == -1)
				{
					Type type2 = type;
					while (type2.IsArray)
					{
						text += "[]";
						type2 = type2.GetElementType();
					}
				}
			}
			else
			{
				Type type3 = Nullable.GetUnderlyingType(type) ?? type;
				text = ToStringCodeMethods.Type(type3, true);
			}
			return text;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001F9AC File Offset: 0x0001DBAC
		private void Initialize(MergedCommandParameterMetadata parameterMetadata, uint parameterSetFlag)
		{
			Collection<CommandParameterInfo> collection = new Collection<CommandParameterInfo>();
			Collection<MergedCompiledCommandParameter> parametersInParameterSet = parameterMetadata.GetParametersInParameterSet(parameterSetFlag);
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in parametersInParameterSet)
			{
				if (mergedCompiledCommandParameter != null)
				{
					collection.Add(new CommandParameterInfo(mergedCompiledCommandParameter.Parameter, parameterSetFlag));
				}
			}
			this.Parameters = new ReadOnlyCollection<CommandParameterInfo>(collection);
		}
	}
}
