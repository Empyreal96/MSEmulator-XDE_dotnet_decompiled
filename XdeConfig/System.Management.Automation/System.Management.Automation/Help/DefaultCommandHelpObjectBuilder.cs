using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;

namespace System.Management.Automation.Help
{
	// Token: 0x020001D5 RID: 469
	internal class DefaultCommandHelpObjectBuilder
	{
		// Token: 0x06001588 RID: 5512 RVA: 0x000871F4 File Offset: 0x000853F4
		internal static PSObject GetPSObjectFromCmdletInfo(CommandInfo input)
		{
			CommandInfo commandInfo = input.CreateGetCommandCopy(null);
			PSObject psobject = new PSObject();
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#{1}#command", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp,
				commandInfo.ModuleName
			}));
			psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#{1}", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp,
				commandInfo.ModuleName
			}));
			psobject.TypeNames.Add(DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp);
			psobject.TypeNames.Add("CmdletHelpInfo");
			psobject.TypeNames.Add("HelpInfo");
			if (commandInfo is CmdletInfo)
			{
				CmdletInfo cmdletInfo = commandInfo as CmdletInfo;
				bool flag = false;
				bool flag2 = false;
				if (cmdletInfo.Parameters != null)
				{
					flag = DefaultCommandHelpObjectBuilder.HasCommonParameters(cmdletInfo.Parameters);
					flag2 = ((cmdletInfo.CommandType & CommandTypes.Workflow) == CommandTypes.Workflow);
				}
				psobject.Properties.Add(new PSNoteProperty("CommonParameters", flag));
				psobject.Properties.Add(new PSNoteProperty("WorkflowCommonParameters", flag2));
				DefaultCommandHelpObjectBuilder.AddDetailsProperties(psobject, cmdletInfo.Name, cmdletInfo.Noun, cmdletInfo.Verb, DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp, null);
				DefaultCommandHelpObjectBuilder.AddSyntaxProperties(psobject, cmdletInfo.Name, cmdletInfo.ParameterSets, flag, flag2, DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp);
				DefaultCommandHelpObjectBuilder.AddParametersProperties(psobject, cmdletInfo.Parameters, flag, flag2, DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp);
				DefaultCommandHelpObjectBuilder.AddInputTypesProperties(psobject, cmdletInfo.Parameters);
				DefaultCommandHelpObjectBuilder.AddRelatedLinksProperties(psobject, commandInfo.CommandMetadata.HelpUri);
				try
				{
					DefaultCommandHelpObjectBuilder.AddOutputTypesProperties(psobject, cmdletInfo.OutputType);
				}
				catch (PSInvalidOperationException)
				{
					DefaultCommandHelpObjectBuilder.AddOutputTypesProperties(psobject, new ReadOnlyCollection<PSTypeName>(new List<PSTypeName>()));
				}
				DefaultCommandHelpObjectBuilder.AddAliasesProperties(psobject, cmdletInfo.Name, cmdletInfo.Context);
				if (DefaultCommandHelpObjectBuilder.HasHelpInfoUri(cmdletInfo.Module, cmdletInfo.ModuleName))
				{
					DefaultCommandHelpObjectBuilder.AddRemarksProperties(psobject, cmdletInfo.Name, cmdletInfo.CommandMetadata.HelpUri);
				}
				else
				{
					psobject.Properties.Add(new PSNoteProperty("remarks", HelpDisplayStrings.None));
				}
				psobject.Properties.Add(new PSNoteProperty("PSSnapIn", cmdletInfo.PSSnapIn));
			}
			else if (commandInfo is FunctionInfo)
			{
				FunctionInfo functionInfo = commandInfo as FunctionInfo;
				bool flag3 = DefaultCommandHelpObjectBuilder.HasCommonParameters(functionInfo.Parameters);
				bool flag4 = (commandInfo.CommandType & CommandTypes.Workflow) == CommandTypes.Workflow;
				psobject.Properties.Add(new PSNoteProperty("CommonParameters", flag3));
				psobject.Properties.Add(new PSNoteProperty("WorkflowCommonParameters", flag4));
				DefaultCommandHelpObjectBuilder.AddDetailsProperties(psobject, functionInfo.Name, string.Empty, string.Empty, DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp, null);
				DefaultCommandHelpObjectBuilder.AddSyntaxProperties(psobject, functionInfo.Name, functionInfo.ParameterSets, flag3, flag4, DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp);
				DefaultCommandHelpObjectBuilder.AddParametersProperties(psobject, functionInfo.Parameters, flag3, flag4, DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp);
				DefaultCommandHelpObjectBuilder.AddInputTypesProperties(psobject, functionInfo.Parameters);
				DefaultCommandHelpObjectBuilder.AddRelatedLinksProperties(psobject, functionInfo.CommandMetadata.HelpUri);
				try
				{
					DefaultCommandHelpObjectBuilder.AddOutputTypesProperties(psobject, functionInfo.OutputType);
				}
				catch (PSInvalidOperationException)
				{
					DefaultCommandHelpObjectBuilder.AddOutputTypesProperties(psobject, new ReadOnlyCollection<PSTypeName>(new List<PSTypeName>()));
				}
				DefaultCommandHelpObjectBuilder.AddAliasesProperties(psobject, functionInfo.Name, functionInfo.Context);
				if (DefaultCommandHelpObjectBuilder.HasHelpInfoUri(functionInfo.Module, functionInfo.ModuleName))
				{
					DefaultCommandHelpObjectBuilder.AddRemarksProperties(psobject, functionInfo.Name, functionInfo.CommandMetadata.HelpUri);
				}
				else
				{
					psobject.Properties.Add(new PSNoteProperty("remarks", HelpDisplayStrings.None));
				}
			}
			psobject.Properties.Add(new PSNoteProperty("alertSet", null));
			psobject.Properties.Add(new PSNoteProperty("description", null));
			psobject.Properties.Add(new PSNoteProperty("examples", null));
			psobject.Properties.Add(new PSNoteProperty("Synopsis", commandInfo.Syntax));
			psobject.Properties.Add(new PSNoteProperty("ModuleName", commandInfo.ModuleName));
			psobject.Properties.Add(new PSNoteProperty("nonTerminatingErrors", string.Empty));
			psobject.Properties.Add(new PSNoteProperty("xmlns:command", "http://schemas.microsoft.com/maml/dev/command/2004/10"));
			psobject.Properties.Add(new PSNoteProperty("xmlns:dev", "http://schemas.microsoft.com/maml/dev/2004/10"));
			psobject.Properties.Add(new PSNoteProperty("xmlns:maml", "http://schemas.microsoft.com/maml/2004/10"));
			return psobject;
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0008768C File Offset: 0x0008588C
		internal static void AddDetailsProperties(PSObject obj, string name, string noun, string verb, string typeNameForHelp, string synopsis = null)
		{
			PSObject psobject = new PSObject();
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#details", new object[]
			{
				typeNameForHelp
			}));
			psobject.Properties.Add(new PSNoteProperty("name", name));
			psobject.Properties.Add(new PSNoteProperty("noun", noun));
			psobject.Properties.Add(new PSNoteProperty("verb", verb));
			if (!string.IsNullOrEmpty(synopsis))
			{
				PSObject psobject2 = new PSObject();
				psobject2.TypeNames.Clear();
				psobject2.TypeNames.Add("MamlParaTextItem");
				psobject2.Properties.Add(new PSNoteProperty("Text", synopsis));
				psobject.Properties.Add(new PSNoteProperty("Description", psobject2));
			}
			obj.Properties.Add(new PSNoteProperty("details", psobject));
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x00087780 File Offset: 0x00085980
		internal static void AddSyntaxProperties(PSObject obj, string cmdletName, ReadOnlyCollection<CommandParameterSetInfo> parameterSets, bool common, bool commonWorkflow, string typeNameForHelp)
		{
			PSObject psobject = new PSObject();
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#syntax", new object[]
			{
				typeNameForHelp
			}));
			DefaultCommandHelpObjectBuilder.AddSyntaxItemProperties(psobject, cmdletName, parameterSets, common, commonWorkflow, typeNameForHelp);
			obj.Properties.Add(new PSNoteProperty("Syntax", psobject));
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x000877EC File Offset: 0x000859EC
		private static void AddSyntaxItemProperties(PSObject obj, string cmdletName, ReadOnlyCollection<CommandParameterSetInfo> parameterSets, bool common, bool commonWorkflow, string typeNameForHelp)
		{
			ArrayList arrayList = new ArrayList();
			foreach (CommandParameterSetInfo commandParameterSetInfo in parameterSets)
			{
				PSObject psobject = new PSObject();
				psobject.TypeNames.Clear();
				psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#syntaxItem", new object[]
				{
					typeNameForHelp
				}));
				psobject.Properties.Add(new PSNoteProperty("name", cmdletName));
				psobject.Properties.Add(new PSNoteProperty("CommonParameters", common));
				psobject.Properties.Add(new PSNoteProperty("WorkflowCommonParameters", commonWorkflow));
				Collection<CommandParameterInfo> collection = new Collection<CommandParameterInfo>();
				commandParameterSetInfo.GenerateParametersInDisplayOrder(commonWorkflow, new Action<CommandParameterInfo>(collection.Add), delegate(string param0)
				{
				});
				DefaultCommandHelpObjectBuilder.AddSyntaxParametersProperties(psobject, collection, common, commonWorkflow, commandParameterSetInfo.Name);
				arrayList.Add(psobject);
			}
			obj.Properties.Add(new PSNoteProperty("syntaxItem", arrayList.ToArray()));
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x00087930 File Offset: 0x00085B30
		private static void AddSyntaxParametersProperties(PSObject obj, IEnumerable<CommandParameterInfo> parameters, bool common, bool commonWorkflow, string parameterSetName)
		{
			ArrayList arrayList = new ArrayList();
			foreach (CommandParameterInfo commandParameterInfo in parameters)
			{
				if ((!commonWorkflow || !DefaultCommandHelpObjectBuilder.IsCommonWorkflowParameter(commandParameterInfo.Name)) && (!common || !Cmdlet.CommonParameters.Contains(commandParameterInfo.Name)))
				{
					PSObject psobject = new PSObject();
					psobject.TypeNames.Clear();
					psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#parameter", new object[]
					{
						DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
					}));
					Collection<Attribute> attributes = new Collection<Attribute>(commandParameterInfo.Attributes);
					DefaultCommandHelpObjectBuilder.AddParameterProperties(psobject, commandParameterInfo.Name, new Collection<string>(commandParameterInfo.Aliases), commandParameterInfo.IsDynamic, commandParameterInfo.ParameterType, attributes, parameterSetName);
					Collection<ValidateSetAttribute> validateSetAttribute = DefaultCommandHelpObjectBuilder.GetValidateSetAttribute(attributes);
					List<string> list = new List<string>();
					foreach (ValidateSetAttribute validateSetAttribute2 in validateSetAttribute)
					{
						foreach (string item in validateSetAttribute2.ValidValues)
						{
							list.Add(item);
						}
					}
					if (list.Count != 0)
					{
						DefaultCommandHelpObjectBuilder.AddParameterValueGroupProperties(psobject, list.ToArray());
					}
					else if (commandParameterInfo.ParameterType.GetTypeInfo().IsEnum && Enum.GetNames(commandParameterInfo.ParameterType) != null)
					{
						DefaultCommandHelpObjectBuilder.AddParameterValueGroupProperties(psobject, Enum.GetNames(commandParameterInfo.ParameterType));
					}
					else if (commandParameterInfo.ParameterType.IsArray)
					{
						if (commandParameterInfo.ParameterType.GetElementType().GetTypeInfo().IsEnum && Enum.GetNames(commandParameterInfo.ParameterType.GetElementType()) != null)
						{
							DefaultCommandHelpObjectBuilder.AddParameterValueGroupProperties(psobject, Enum.GetNames(commandParameterInfo.ParameterType.GetElementType()));
						}
					}
					else if (commandParameterInfo.ParameterType.GetTypeInfo().IsGenericType)
					{
						Type[] genericArguments = commandParameterInfo.ParameterType.GetGenericArguments();
						if (genericArguments.Length != 0)
						{
							Type type = genericArguments[0];
							if (type.GetTypeInfo().IsEnum && Enum.GetNames(type) != null)
							{
								DefaultCommandHelpObjectBuilder.AddParameterValueGroupProperties(psobject, Enum.GetNames(type));
							}
							else if (type.IsArray && type.GetElementType().GetTypeInfo().IsEnum && Enum.GetNames(type.GetElementType()) != null)
							{
								DefaultCommandHelpObjectBuilder.AddParameterValueGroupProperties(psobject, Enum.GetNames(type.GetElementType()));
							}
						}
					}
					arrayList.Add(psobject);
				}
			}
			obj.Properties.Add(new PSNoteProperty("parameter", arrayList.ToArray()));
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x00087C24 File Offset: 0x00085E24
		private static void AddParameterValueGroupProperties(PSObject obj, string[] values)
		{
			PSObject psobject = new PSObject();
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#parameterValueGroup", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
			}));
			ArrayList arrayList = new ArrayList(values);
			psobject.Properties.Add(new PSNoteProperty("parameterValue", arrayList.ToArray()));
			obj.Properties.Add(new PSNoteProperty("parameterValueGroup", psobject));
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x00087CA4 File Offset: 0x00085EA4
		internal static void AddParametersProperties(PSObject obj, Dictionary<string, ParameterMetadata> parameters, bool common, bool commonWorkflow, string typeNameForHelp)
		{
			PSObject psobject = new PSObject();
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#parameters", new object[]
			{
				typeNameForHelp
			}));
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			if (parameters != null)
			{
				foreach (KeyValuePair<string, ParameterMetadata> keyValuePair in parameters)
				{
					arrayList2.Add(keyValuePair.Key);
				}
			}
			arrayList2.Sort(StringComparer.Ordinal);
			foreach (object obj2 in arrayList2)
			{
				string text = (string)obj2;
				if ((!commonWorkflow || !DefaultCommandHelpObjectBuilder.IsCommonWorkflowParameter(text)) && (!common || !Cmdlet.CommonParameters.Contains(text)))
				{
					PSObject psobject2 = new PSObject();
					psobject2.TypeNames.Clear();
					psobject2.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#parameter", new object[]
					{
						DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
					}));
					DefaultCommandHelpObjectBuilder.AddParameterProperties(psobject2, text, parameters[text].Aliases, parameters[text].IsDynamic, parameters[text].ParameterType, parameters[text].Attributes, null);
					arrayList.Add(psobject2);
				}
			}
			psobject.Properties.Add(new PSNoteProperty("parameter", arrayList.ToArray()));
			obj.Properties.Add(new PSNoteProperty("parameters", psobject));
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x00087E78 File Offset: 0x00086078
		private static void AddParameterProperties(PSObject obj, string name, Collection<string> aliases, bool dynamic, Type type, Collection<Attribute> attributes, string parameterSetName = null)
		{
			Collection<ParameterAttribute> parameterAttribute = DefaultCommandHelpObjectBuilder.GetParameterAttribute(attributes);
			obj.Properties.Add(new PSNoteProperty("name", name));
			if (parameterAttribute.Count == 0)
			{
				obj.Properties.Add(new PSNoteProperty("required", ""));
				obj.Properties.Add(new PSNoteProperty("pipelineInput", ""));
				obj.Properties.Add(new PSNoteProperty("isDynamic", ""));
				obj.Properties.Add(new PSNoteProperty("parameterSetName", ""));
				obj.Properties.Add(new PSNoteProperty("description", ""));
				obj.Properties.Add(new PSNoteProperty("position", ""));
				obj.Properties.Add(new PSNoteProperty("aliases", ""));
				return;
			}
			ParameterAttribute parameterAttribute2 = parameterAttribute[0];
			if (!string.IsNullOrEmpty(parameterSetName))
			{
				foreach (ParameterAttribute parameterAttribute3 in parameterAttribute)
				{
					if (string.Equals(parameterAttribute3.ParameterSetName, parameterSetName, StringComparison.OrdinalIgnoreCase))
					{
						parameterAttribute2 = parameterAttribute3;
						break;
					}
				}
			}
			obj.Properties.Add(new PSNoteProperty("required", CultureInfo.CurrentCulture.TextInfo.ToLower(parameterAttribute2.Mandatory.ToString())));
			obj.Properties.Add(new PSNoteProperty("pipelineInput", DefaultCommandHelpObjectBuilder.GetPipelineInputString(parameterAttribute2)));
			obj.Properties.Add(new PSNoteProperty("isDynamic", CultureInfo.CurrentCulture.TextInfo.ToLower(dynamic.ToString())));
			if (parameterAttribute2.ParameterSetName.Equals("__AllParameterSets", StringComparison.OrdinalIgnoreCase))
			{
				obj.Properties.Add(new PSNoteProperty("parameterSetName", StringUtil.Format(HelpDisplayStrings.AllParameterSetsName, new object[0])));
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < parameterAttribute.Count; i++)
				{
					stringBuilder.Append(parameterAttribute[i].ParameterSetName);
					if (i != parameterAttribute.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				obj.Properties.Add(new PSNoteProperty("parameterSetName", stringBuilder.ToString()));
			}
			if (parameterAttribute2.HelpMessage != null)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine(parameterAttribute2.HelpMessage);
				obj.Properties.Add(new PSNoteProperty("description", stringBuilder2.ToString()));
			}
			if (type != typeof(SwitchParameter))
			{
				DefaultCommandHelpObjectBuilder.AddParameterValueProperties(obj, type, attributes);
			}
			DefaultCommandHelpObjectBuilder.AddParameterTypeProperties(obj, type, attributes);
			if (parameterAttribute2.Position == -2147483648)
			{
				obj.Properties.Add(new PSNoteProperty("position", StringUtil.Format(HelpDisplayStrings.NamedParameter, new object[0])));
			}
			else
			{
				obj.Properties.Add(new PSNoteProperty("position", parameterAttribute2.Position.ToString(CultureInfo.InvariantCulture)));
			}
			if (aliases.Count == 0)
			{
				obj.Properties.Add(new PSNoteProperty("aliases", StringUtil.Format(HelpDisplayStrings.None, new object[0])));
				return;
			}
			StringBuilder stringBuilder3 = new StringBuilder();
			for (int j = 0; j < aliases.Count; j++)
			{
				stringBuilder3.Append(aliases[j]);
				if (j != aliases.Count - 1)
				{
					stringBuilder3.Append(", ");
				}
			}
			obj.Properties.Add(new PSNoteProperty("aliases", stringBuilder3.ToString()));
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00088224 File Offset: 0x00086424
		private static void AddParameterTypeProperties(PSObject obj, Type parameterType, IEnumerable<Attribute> attributes)
		{
			PSObject psobject = new PSObject();
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#type", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
			}));
			string parameterTypeString = CommandParameterSetInfo.GetParameterTypeString(parameterType, attributes);
			psobject.Properties.Add(new PSNoteProperty("name", parameterTypeString));
			obj.Properties.Add(new PSNoteProperty("type", psobject));
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x000882A0 File Offset: 0x000864A0
		private static void AddParameterValueProperties(PSObject obj, Type parameterType, IEnumerable<Attribute> attributes)
		{
			PSObject psobject;
			if (parameterType != null)
			{
				Nullable.GetUnderlyingType(parameterType);
				string parameterTypeString = CommandParameterSetInfo.GetParameterTypeString(parameterType, attributes);
				psobject = new PSObject(parameterTypeString);
				psobject.Properties.Add(new PSNoteProperty("variableLength", parameterType.IsArray));
			}
			else
			{
				psobject = new PSObject("System.Object");
				psobject.Properties.Add(new PSNoteProperty("variableLength", StringUtil.Format(HelpDisplayStrings.FalseShort, new object[0])));
			}
			psobject.Properties.Add(new PSNoteProperty("required", "true"));
			obj.Properties.Add(new PSNoteProperty("parameterValue", psobject));
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00088350 File Offset: 0x00086550
		internal static void AddInputTypesProperties(PSObject obj, Dictionary<string, ParameterMetadata> parameters)
		{
			Collection<string> collection = new Collection<string>();
			if (parameters != null)
			{
				foreach (KeyValuePair<string, ParameterMetadata> keyValuePair in parameters)
				{
					Collection<ParameterAttribute> parameterAttribute = DefaultCommandHelpObjectBuilder.GetParameterAttribute(keyValuePair.Value.Attributes);
					foreach (ParameterAttribute parameterAttribute2 in parameterAttribute)
					{
						if ((parameterAttribute2.ValueFromPipeline || parameterAttribute2.ValueFromPipelineByPropertyName || parameterAttribute2.ValueFromRemainingArguments) && !collection.Contains(keyValuePair.Value.ParameterType.FullName))
						{
							collection.Add(keyValuePair.Value.ParameterType.FullName);
						}
					}
				}
			}
			if (collection.Count == 0)
			{
				collection.Add(StringUtil.Format(HelpDisplayStrings.None, new object[0]));
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in collection)
			{
				stringBuilder.AppendLine(value);
			}
			PSObject psobject = new PSObject();
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#inputTypes", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
			}));
			PSObject psobject2 = new PSObject();
			psobject2.TypeNames.Clear();
			psobject2.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#inputType", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
			}));
			PSObject psobject3 = new PSObject();
			psobject3.TypeNames.Clear();
			psobject3.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#type", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
			}));
			psobject3.Properties.Add(new PSNoteProperty("name", stringBuilder.ToString()));
			psobject2.Properties.Add(new PSNoteProperty("type", psobject3));
			psobject.Properties.Add(new PSNoteProperty("inputType", psobject2));
			obj.Properties.Add(new PSNoteProperty("inputTypes", psobject));
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x000885C8 File Offset: 0x000867C8
		private static void AddOutputTypesProperties(PSObject obj, ReadOnlyCollection<PSTypeName> outputTypes)
		{
			PSObject psobject = new PSObject();
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#returnValues", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
			}));
			PSObject psobject2 = new PSObject();
			psobject2.TypeNames.Clear();
			psobject2.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#returnValue", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
			}));
			PSObject psobject3 = new PSObject();
			psobject3.TypeNames.Clear();
			psobject3.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#type", new object[]
			{
				DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
			}));
			if (outputTypes.Count == 0)
			{
				psobject3.Properties.Add(new PSNoteProperty("name", "System.Object"));
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (PSTypeName pstypeName in outputTypes)
				{
					stringBuilder.AppendLine(pstypeName.Name);
				}
				psobject3.Properties.Add(new PSNoteProperty("name", stringBuilder.ToString()));
			}
			psobject2.Properties.Add(new PSNoteProperty("type", psobject3));
			psobject.Properties.Add(new PSNoteProperty("returnValue", psobject2));
			obj.Properties.Add(new PSNoteProperty("returnValues", psobject));
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x00088760 File Offset: 0x00086960
		private static void AddAliasesProperties(PSObject obj, string name, ExecutionContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			if (context != null)
			{
				foreach (string value in context.SessionState.Internal.GetAliasesByCommandName(name))
				{
					flag = true;
					stringBuilder.AppendLine(value);
				}
			}
			if (!flag)
			{
				stringBuilder.AppendLine(StringUtil.Format(HelpDisplayStrings.None, new object[0]));
			}
			obj.Properties.Add(new PSNoteProperty("aliases", stringBuilder.ToString()));
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x000887FC File Offset: 0x000869FC
		private static void AddRemarksProperties(PSObject obj, string cmdletName, string helpUri)
		{
			if (string.IsNullOrEmpty(helpUri))
			{
				obj.Properties.Add(new PSNoteProperty("remarks", StringUtil.Format(HelpDisplayStrings.GetLatestHelpContentWithoutHelpUri, cmdletName)));
				return;
			}
			obj.Properties.Add(new PSNoteProperty("remarks", StringUtil.Format(HelpDisplayStrings.GetLatestHelpContent, cmdletName, helpUri)));
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00088854 File Offset: 0x00086A54
		internal static void AddRelatedLinksProperties(PSObject obj, string relatedLink)
		{
			if (!string.IsNullOrEmpty(relatedLink))
			{
				PSObject psobject = new PSObject();
				psobject.TypeNames.Clear();
				psobject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#navigationLinks", new object[]
				{
					DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
				}));
				psobject.Properties.Add(new PSNoteProperty("uri", relatedLink));
				List<PSObject> list = new List<PSObject>
				{
					psobject
				};
				PSNoteProperty psnoteProperty = obj.Properties["relatedLinks"] as PSNoteProperty;
				if (psnoteProperty != null && psnoteProperty.Value != null)
				{
					PSObject psobject2 = PSObject.AsPSObject(psnoteProperty.Value);
					PSNoteProperty psnoteProperty2 = psobject2.Properties["navigationLink"] as PSNoteProperty;
					if (psnoteProperty2 != null && psnoteProperty2.Value != null)
					{
						PSObject psobject3 = psnoteProperty2.Value as PSObject;
						if (psobject3 != null)
						{
							list.Add(psobject3);
						}
						else
						{
							PSObject[] array = psnoteProperty2.Value as PSObject[];
							if (array != null)
							{
								foreach (PSObject item in array)
								{
									list.Add(item);
								}
							}
						}
					}
				}
				PSObject psobject4 = new PSObject();
				psobject4.TypeNames.Clear();
				psobject4.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "{0}#relatedLinks", new object[]
				{
					DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp
				}));
				psobject4.Properties.Add(new PSNoteProperty("navigationLink", list.ToArray()));
				obj.Properties.Add(new PSNoteProperty("relatedLinks", psobject4));
			}
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x000889F4 File Offset: 0x00086BF4
		private static Collection<ParameterAttribute> GetParameterAttribute(Collection<Attribute> attributes)
		{
			Collection<ParameterAttribute> collection = new Collection<ParameterAttribute>();
			foreach (Attribute attribute in attributes)
			{
				ParameterAttribute parameterAttribute = attribute as ParameterAttribute;
				if (parameterAttribute != null)
				{
					collection.Add(parameterAttribute);
				}
			}
			return collection;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x00088A50 File Offset: 0x00086C50
		private static Collection<ValidateSetAttribute> GetValidateSetAttribute(Collection<Attribute> attributes)
		{
			Collection<ValidateSetAttribute> collection = new Collection<ValidateSetAttribute>();
			foreach (Attribute attribute in attributes)
			{
				ValidateSetAttribute validateSetAttribute = attribute as ValidateSetAttribute;
				if (validateSetAttribute != null)
				{
					collection.Add(validateSetAttribute);
				}
			}
			return collection;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00088AAC File Offset: 0x00086CAC
		private static string GetPipelineInputString(ParameterAttribute paramAttrib)
		{
			ArrayList arrayList = new ArrayList();
			if (paramAttrib.ValueFromPipeline)
			{
				arrayList.Add(StringUtil.Format(HelpDisplayStrings.PipelineByValue, new object[0]));
			}
			if (paramAttrib.ValueFromPipelineByPropertyName)
			{
				arrayList.Add(StringUtil.Format(HelpDisplayStrings.PipelineByPropertyName, new object[0]));
			}
			if (paramAttrib.ValueFromRemainingArguments)
			{
				arrayList.Add(StringUtil.Format(HelpDisplayStrings.PipelineFromRemainingArguments, new object[0]));
			}
			if (arrayList.Count == 0)
			{
				return StringUtil.Format(HelpDisplayStrings.FalseShort, new object[0]);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(StringUtil.Format(HelpDisplayStrings.TrueShort, new object[0]));
			stringBuilder.Append(" (");
			for (int i = 0; i < arrayList.Count; i++)
			{
				stringBuilder.Append((string)arrayList[i]);
				if (i != arrayList.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x00088BAC File Offset: 0x00086DAC
		internal static bool HasCommonParameters(Dictionary<string, ParameterMetadata> parameters)
		{
			Collection<string> collection = new Collection<string>();
			foreach (KeyValuePair<string, ParameterMetadata> keyValuePair in parameters)
			{
				if (Cmdlet.CommonParameters.Contains(keyValuePair.Value.Name))
				{
					collection.Add(keyValuePair.Value.Name);
				}
			}
			return collection.Count == Cmdlet.CommonParameters.Count;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00088C38 File Offset: 0x00086E38
		private static bool IsCommonWorkflowParameter(string name)
		{
			foreach (string b in CommonParameters.CommonWorkflowParameters)
			{
				if (name == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00088C6D File Offset: 0x00086E6D
		private static bool HasHelpInfoUri(PSModuleInfo module, string moduleName)
		{
			return (!string.IsNullOrEmpty(moduleName) && moduleName.Equals(InitialSessionState.CoreModule, StringComparison.OrdinalIgnoreCase)) || (module != null && !string.IsNullOrEmpty(module.HelpInfoUri));
		}

		// Token: 0x04000928 RID: 2344
		internal static string TypeNameForDefaultHelp = "ExtendedCmdletHelpInfo";
	}
}
