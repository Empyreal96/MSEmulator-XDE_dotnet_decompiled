using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Language;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x020000C2 RID: 194
	internal class ExportVisitor : AstVisitor
	{
		// Token: 0x06000A9F RID: 2719 RVA: 0x0003F504 File Offset: 0x0003D704
		internal ExportVisitor()
		{
			this.DiscoveredExports = new List<string>();
			this.DiscoveredFunctions = new Dictionary<string, FunctionDefinitionAst>(StringComparer.OrdinalIgnoreCase);
			this.DiscoveredAliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			this.DiscoveredModules = new List<RequiredModuleInfo>();
			this.DiscoveredCommandFilters = new List<string>();
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0003F558 File Offset: 0x0003D758
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x0003F560 File Offset: 0x0003D760
		internal List<string> DiscoveredExports { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0003F569 File Offset: 0x0003D769
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x0003F571 File Offset: 0x0003D771
		internal List<RequiredModuleInfo> DiscoveredModules { get; set; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x0003F57A File Offset: 0x0003D77A
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x0003F582 File Offset: 0x0003D782
		internal Dictionary<string, FunctionDefinitionAst> DiscoveredFunctions { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0003F58B File Offset: 0x0003D78B
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x0003F593 File Offset: 0x0003D793
		internal Dictionary<string, string> DiscoveredAliases { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x0003F59C File Offset: 0x0003D79C
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x0003F5A4 File Offset: 0x0003D7A4
		internal List<string> DiscoveredCommandFilters { get; set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0003F5AD File Offset: 0x0003D7AD
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0003F5B5 File Offset: 0x0003D7B5
		internal bool AddsSelfToPath { get; set; }

		// Token: 0x06000AAC RID: 2732 RVA: 0x0003F5C0 File Offset: 0x0003D7C0
		public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			this.DiscoveredFunctions[functionDefinitionAst.Name] = functionDefinitionAst;
			ModuleIntrinsics.Tracer.WriteLine("Discovered function definition: " + functionDefinitionAst.Name, new object[0]);
			if (functionDefinitionAst.Body != null && functionDefinitionAst.Body.ParamBlock != null && functionDefinitionAst.Body.ParamBlock.Attributes != null)
			{
				foreach (AttributeAst attributeAst in functionDefinitionAst.Body.ParamBlock.Attributes)
				{
					if (attributeAst.TypeName.GetReflectionAttributeType() == typeof(AliasAttribute))
					{
						foreach (ExpressionAst expressionAst in attributeAst.PositionalArguments)
						{
							ConstantExpressionAst constantExpressionAst = expressionAst as ConstantExpressionAst;
							if (constantExpressionAst != null)
							{
								string text = constantExpressionAst.Value.ToString();
								this.DiscoveredAliases[text] = functionDefinitionAst.Name;
								ModuleIntrinsics.Tracer.WriteLine("Function defines alias: " + text + "=" + functionDefinitionAst.Name, new object[0]);
							}
						}
					}
				}
			}
			for (Ast parent = functionDefinitionAst.Parent; parent != null; parent = parent.Parent)
			{
				if (parent is FunctionDefinitionAst)
				{
					return AstVisitAction.Continue;
				}
			}
			this.DiscoveredExports.Add(functionDefinitionAst.Name);
			return AstVisitAction.Continue;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0003F758 File Offset: 0x0003D958
		public override AstVisitAction VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			if (string.Equals("$env:PATH", assignmentStatementAst.Left.ToString(), StringComparison.OrdinalIgnoreCase) && Regex.IsMatch(assignmentStatementAst.Right.ToString(), "\\$psScriptRoot", RegexOptions.IgnoreCase))
			{
				ModuleIntrinsics.Tracer.WriteLine("Module adds itself to the path.", new object[0]);
				this.AddsSelfToPath = true;
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0003F7B4 File Offset: 0x0003D9B4
		public override AstVisitAction VisitCommand(CommandAst commandAst)
		{
			string text = commandAst.GetCommandName();
			if (string.IsNullOrEmpty(text))
			{
				text = commandAst.CommandElements[0].ToString();
				text = text.Trim(new char[]
				{
					'"',
					'\''
				});
			}
			if (commandAst.InvocationOperator == TokenKind.Dot)
			{
				text = Regex.Replace(text, "\\$[^\\\\]*\\\\", "", RegexOptions.IgnoreCase);
				this.DiscoveredModules.Add(new RequiredModuleInfo
				{
					Name = text,
					CommandsToPostFilter = new List<string>()
				});
				ModuleIntrinsics.Tracer.WriteLine("Module dots " + text, new object[0]);
			}
			if (string.Equals(text, "New-Alias", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "Microsoft.PowerShell.Utility\\New-Alias", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "Set-Alias", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "Microsoft.PowerShell.Utility\\Set-Alias", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "nal", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "sal", StringComparison.OrdinalIgnoreCase))
			{
				string parameterByNameOrPosition = this.GetParameterByNameOrPosition("Name", 0, commandAst);
				string parameterByNameOrPosition2 = this.GetParameterByNameOrPosition("Value", 1, commandAst);
				if (!string.IsNullOrEmpty(parameterByNameOrPosition))
				{
					this.DiscoveredAliases[parameterByNameOrPosition] = parameterByNameOrPosition2;
					ModuleIntrinsics.Tracer.WriteLine("Module defines alias: " + parameterByNameOrPosition + "=" + parameterByNameOrPosition2, new object[0]);
				}
			}
			if (string.Equals(text, "Import-Module", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "ipmo", StringComparison.OrdinalIgnoreCase))
			{
				List<string> list = new List<string>();
				string parameterByNameOrPosition3 = this.GetParameterByNameOrPosition("Function", -1, commandAst);
				if (!string.IsNullOrEmpty(parameterByNameOrPosition3))
				{
					list.AddRange(this.ProcessExportedCommandList(parameterByNameOrPosition3));
				}
				string parameterByNameOrPosition4 = this.GetParameterByNameOrPosition("Cmdlet", -1, commandAst);
				if (!string.IsNullOrEmpty(parameterByNameOrPosition4))
				{
					list.AddRange(this.ProcessExportedCommandList(parameterByNameOrPosition4));
				}
				string parameterByNameOrPosition5 = this.GetParameterByNameOrPosition("Alias", -1, commandAst);
				if (!string.IsNullOrEmpty(parameterByNameOrPosition5))
				{
					list.AddRange(this.ProcessExportedCommandList(parameterByNameOrPosition5));
				}
				string parameterByNameOrPosition6 = this.GetParameterByNameOrPosition("Name", 0, commandAst);
				if (!string.IsNullOrEmpty(parameterByNameOrPosition6))
				{
					string[] array = parameterByNameOrPosition6.Split(new char[]
					{
						','
					});
					foreach (string text2 in array)
					{
						ModuleIntrinsics.Tracer.WriteLine("Discovered module import: " + text2, new object[0]);
						this.DiscoveredModules.Add(new RequiredModuleInfo
						{
							Name = this.TrimBegEndQuotes(text2.Trim()),
							CommandsToPostFilter = list
						});
					}
				}
			}
			if (string.Equals(text, "Export-ModuleMember", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "Microsoft.PowerShell.Core\\Export-ModuleMember", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "$script:ExportModuleMember", StringComparison.OrdinalIgnoreCase))
			{
				List<string> list2 = new List<string>();
				string parameterByNameOrPosition7 = this.GetParameterByNameOrPosition("Function", 0, commandAst);
				list2.AddRange(this.ExtractArgumentList(parameterByNameOrPosition7));
				string parameterByNameOrPosition8 = this.GetParameterByNameOrPosition("Cmdlet", -1, commandAst);
				list2.AddRange(this.ExtractArgumentList(parameterByNameOrPosition8));
				foreach (string text3 in list2)
				{
					this.DiscoveredCommandFilters.Add(text3);
					ModuleIntrinsics.Tracer.WriteLine("Discovered explicit export: " + text3, new object[0]);
					if (!WildcardPattern.ContainsWildcardCharacters(text3) && !this.DiscoveredExports.Contains(text3))
					{
						this.DiscoveredExports.Add(text3);
					}
				}
				list2 = new List<string>();
				string parameterByNameOrPosition9 = this.GetParameterByNameOrPosition("Alias", -1, commandAst);
				list2.AddRange(this.ExtractArgumentList(parameterByNameOrPosition9));
				foreach (string text4 in list2)
				{
					this.DiscoveredCommandFilters.Add(text4);
					if (!WildcardPattern.ContainsWildcardCharacters(text4))
					{
						this.DiscoveredAliases[text4] = null;
					}
				}
			}
			if (string.Equals(text, "public", StringComparison.OrdinalIgnoreCase) && commandAst.CommandElements.Count > 2)
			{
				string item = commandAst.CommandElements[2].ToString().Trim();
				this.DiscoveredExports.Add(item);
				this.DiscoveredCommandFilters.Add(item);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0003FC08 File Offset: 0x0003DE08
		private string TrimBegEndQuotes(string name)
		{
			int length = name.Length;
			if (length > 2)
			{
				char c = name[0];
				char c2 = name[length - 1];
				if ((c.IsSingleQuote() || c.IsDoubleQuote()) && (c2.IsSingleQuote() || c2.IsDoubleQuote()))
				{
					StringBuilder stringBuilder = new StringBuilder(name, 1, length - 2, length - 2);
					return stringBuilder.ToString();
				}
			}
			return name;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0003FC70 File Offset: 0x0003DE70
		private List<string> ExtractArgumentList(string arguments)
		{
			char[] trimChars = new char[]
			{
				'\'',
				'"',
				' ',
				'\t'
			};
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(arguments))
			{
				string[] array = arguments.Split(new char[]
				{
					','
				});
				foreach (string text in array)
				{
					list.Add(text.Trim(trimChars));
				}
			}
			return list;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0003FCE0 File Offset: 0x0003DEE0
		private List<string> ProcessExportedCommandList(string declaration)
		{
			List<string> list = this.ExtractArgumentList(declaration);
			List<string> list2 = new List<string>();
			foreach (string item in list)
			{
				list2.Add(item);
			}
			return list2;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0003FD40 File Offset: 0x0003DF40
		private string GetParameterByNameOrPosition(string name, int position, CommandAst commandAst)
		{
			Dictionary<string, string> parameters = this.GetParameters(commandAst.CommandElements);
			string result = null;
			if (!parameters.TryGetValue(name, out result))
			{
				parameters.TryGetValue(position.ToString(CultureInfo.InvariantCulture), out result);
			}
			return result;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0003FD80 File Offset: 0x0003DF80
		private Dictionary<string, string> GetParameters(ReadOnlyCollection<CommandElementAst> commandElements)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			bool flag = false;
			string text = null;
			int num = 0;
			foreach (CommandElementAst commandElementAst in commandElements)
			{
				if (!flag)
				{
					flag = true;
				}
				else
				{
					CommandParameterAst commandParameterAst = commandElementAst as CommandParameterAst;
					if (commandParameterAst != null)
					{
						string parameterName = commandParameterAst.ParameterName;
						if (commandParameterAst.Argument != null)
						{
							dictionary.Add(parameterName, commandParameterAst.Argument.ToString());
							text = null;
						}
						else
						{
							text = parameterName;
						}
					}
					else if (text != null)
					{
						ArrayExpressionAst arrayExpressionAst = commandElementAst as ArrayExpressionAst;
						if (arrayExpressionAst != null)
						{
							dictionary.Add(text, arrayExpressionAst.SubExpression.ToString());
						}
						else
						{
							dictionary.Add(text, commandElementAst.ToString());
						}
						text = null;
					}
					else
					{
						dictionary.Add(num.ToString(CultureInfo.InvariantCulture), commandElementAst.ToString());
						num++;
					}
				}
			}
			return dictionary;
		}
	}
}
