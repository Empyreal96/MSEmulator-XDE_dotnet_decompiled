using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation.Help;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001D0 RID: 464
	internal class HelpCommentsParser
	{
		// Token: 0x0600155A RID: 5466 RVA: 0x00084E70 File Offset: 0x00083070
		private HelpCommentsParser()
		{
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x00084EC8 File Offset: 0x000830C8
		private HelpCommentsParser(List<string> parameterDescriptions)
		{
			this.parameterDescriptions = parameterDescriptions;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x00084F24 File Offset: 0x00083124
		private HelpCommentsParser(CommandInfo commandInfo, List<string> parameterDescriptions)
		{
			FunctionInfo functionInfo = commandInfo as FunctionInfo;
			if (functionInfo != null)
			{
				this.scriptBlock = functionInfo.ScriptBlock;
				this.commandName = functionInfo.Name;
			}
			else
			{
				ExternalScriptInfo externalScriptInfo = commandInfo as ExternalScriptInfo;
				if (externalScriptInfo != null)
				{
					this.scriptBlock = externalScriptInfo.ScriptBlock;
					this.commandName = externalScriptInfo.Path;
				}
			}
			this.commandMetadata = commandInfo.CommandMetadata;
			this.parameterDescriptions = parameterDescriptions;
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x00084FD4 File Offset: 0x000831D4
		private void DetermineParameterDescriptions()
		{
			int num = 0;
			foreach (string text in this.commandMetadata.StaticCommandParameterMetadata.BindableParameters.Keys)
			{
				string text2;
				if (!this._parameters.TryGetValue(text.ToUpperInvariant(), out text2) && num < this.parameterDescriptions.Count)
				{
					this._parameters.Add(text.ToUpperInvariant(), this.parameterDescriptions[num]);
				}
				num++;
			}
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00085070 File Offset: 0x00083270
		private string GetParameterDescription(string parameterName)
		{
			string result;
			this._parameters.TryGetValue(parameterName.ToUpperInvariant(), out result);
			return result;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00085094 File Offset: 0x00083294
		private XmlElement BuildXmlForParameter(string parameterName, bool isMandatory, bool valueFromPipeline, bool valueFromPipelineByPropertyName, string position, Type type, string description, bool supportsWildcards, string defaultValue, bool forSyntax)
		{
			XmlElement xmlElement = this.doc.CreateElement("command:parameter", HelpCommentsParser.commandURI);
			xmlElement.SetAttribute("required", isMandatory ? "true" : "false");
			xmlElement.SetAttribute("globbing", supportsWildcards ? "true" : "false");
			string value;
			if (valueFromPipeline && valueFromPipelineByPropertyName)
			{
				value = "true (ByValue, ByPropertyName)";
			}
			else if (valueFromPipeline)
			{
				value = "true (ByValue)";
			}
			else if (valueFromPipelineByPropertyName)
			{
				value = "true (ByPropertyName)";
			}
			else
			{
				value = "false";
			}
			xmlElement.SetAttribute("pipelineInput", value);
			xmlElement.SetAttribute("position", position);
			XmlElement newChild = this.doc.CreateElement("maml:name", HelpCommentsParser.mamlURI);
			XmlText newChild2 = this.doc.CreateTextNode(parameterName);
			xmlElement.AppendChild(newChild).AppendChild(newChild2);
			if (!string.IsNullOrEmpty(description))
			{
				XmlElement newChild3 = this.doc.CreateElement("maml:description", HelpCommentsParser.mamlURI);
				XmlElement newChild4 = this.doc.CreateElement("maml:para", HelpCommentsParser.mamlURI);
				XmlText newChild5 = this.doc.CreateTextNode(description);
				xmlElement.AppendChild(newChild3).AppendChild(newChild4).AppendChild(newChild5);
			}
			if (type == null)
			{
				type = typeof(object);
			}
			if (type.GetTypeInfo().IsEnum)
			{
				XmlElement xmlElement2 = this.doc.CreateElement("command:parameterValueGroup", HelpCommentsParser.commandURI);
				foreach (string text in Enum.GetNames(type))
				{
					XmlElement xmlElement3 = this.doc.CreateElement("command:parameterValue", HelpCommentsParser.commandURI);
					xmlElement3.SetAttribute("required", "false");
					XmlText newChild6 = this.doc.CreateTextNode(text);
					xmlElement2.AppendChild(xmlElement3).AppendChild(newChild6);
				}
				xmlElement.AppendChild(xmlElement2);
			}
			else
			{
				bool flag = type == typeof(SwitchParameter);
				if (!forSyntax || !flag)
				{
					XmlElement xmlElement4 = this.doc.CreateElement("command:parameterValue", HelpCommentsParser.commandURI);
					xmlElement4.SetAttribute("required", flag ? "false" : "true");
					XmlText newChild7 = this.doc.CreateTextNode(type.Name);
					xmlElement.AppendChild(xmlElement4).AppendChild(newChild7);
				}
			}
			if (!forSyntax)
			{
				XmlElement newChild8 = this.doc.CreateElement("dev:type", HelpCommentsParser.devURI);
				XmlElement newChild9 = this.doc.CreateElement("maml:name", HelpCommentsParser.mamlURI);
				XmlText newChild10 = this.doc.CreateTextNode(type.Name);
				xmlElement.AppendChild(newChild8).AppendChild(newChild9).AppendChild(newChild10);
				XmlElement newChild11 = this.doc.CreateElement("dev:defaultValue", HelpCommentsParser.devURI);
				XmlText newChild12 = this.doc.CreateTextNode(defaultValue);
				xmlElement.AppendChild(newChild11).AppendChild(newChild12);
			}
			return xmlElement;
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0008537C File Offset: 0x0008357C
		internal XmlDocument BuildXmlFromComments()
		{
			this.doc = new XmlDocument();
			XmlElement xmlElement = this.doc.CreateElement("command:command", HelpCommentsParser.commandURI);
			xmlElement.SetAttribute("xmlns:maml", HelpCommentsParser.mamlURI);
			xmlElement.SetAttribute("xmlns:command", HelpCommentsParser.commandURI);
			xmlElement.SetAttribute("xmlns:dev", HelpCommentsParser.devURI);
			this.doc.AppendChild(xmlElement);
			XmlElement xmlElement2 = this.doc.CreateElement("command:details", HelpCommentsParser.commandURI);
			xmlElement.AppendChild(xmlElement2);
			XmlElement newChild = this.doc.CreateElement("command:name", HelpCommentsParser.commandURI);
			XmlText newChild2 = this.doc.CreateTextNode(this.commandName);
			xmlElement2.AppendChild(newChild).AppendChild(newChild2);
			if (!string.IsNullOrEmpty(this._sections.Synopsis))
			{
				XmlElement newChild3 = this.doc.CreateElement("maml:description", HelpCommentsParser.mamlURI);
				XmlElement newChild4 = this.doc.CreateElement("maml:para", HelpCommentsParser.mamlURI);
				XmlText newChild5 = this.doc.CreateTextNode(this._sections.Synopsis);
				xmlElement2.AppendChild(newChild3).AppendChild(newChild4).AppendChild(newChild5);
			}
			this.DetermineParameterDescriptions();
			XmlElement syntax = this.doc.CreateElement("command:syntax", HelpCommentsParser.commandURI);
			MergedCommandParameterMetadata staticCommandParameterMetadata = this.commandMetadata.StaticCommandParameterMetadata;
			if (staticCommandParameterMetadata.ParameterSetCount > 0)
			{
				for (int i = 0; i < staticCommandParameterMetadata.ParameterSetCount; i++)
				{
					this.BuildSyntaxForParameterSet(xmlElement, syntax, staticCommandParameterMetadata, i);
				}
			}
			else
			{
				this.BuildSyntaxForParameterSet(xmlElement, syntax, staticCommandParameterMetadata, int.MaxValue);
			}
			XmlElement xmlElement3 = this.doc.CreateElement("command:parameters", HelpCommentsParser.commandURI);
			foreach (KeyValuePair<string, MergedCompiledCommandParameter> keyValuePair in staticCommandParameterMetadata.BindableParameters)
			{
				MergedCompiledCommandParameter value = keyValuePair.Value;
				if (value.BinderAssociation != ParameterBinderAssociation.CommonParameters)
				{
					string key = keyValuePair.Key;
					string parameterDescription = this.GetParameterDescription(key);
					bool isMandatory = false;
					bool valueFromPipeline = false;
					bool valueFromPipelineByPropertyName = false;
					string position = "named";
					int num = 0;
					CompiledCommandParameter parameter = value.Parameter;
					ParameterSetSpecificMetadata parameterSetData;
					parameter.ParameterSetData.TryGetValue("__AllParameterSets", out parameterSetData);
					while (parameterSetData == null && num < 32)
					{
						parameterSetData = parameter.GetParameterSetData(1U << num++);
					}
					if (parameterSetData != null)
					{
						isMandatory = parameterSetData.IsMandatory;
						valueFromPipeline = parameterSetData.ValueFromPipeline;
						valueFromPipelineByPropertyName = parameterSetData.ValueFromPipelineByPropertyName;
						position = (parameterSetData.IsPositional ? (1 + parameterSetData.Position).ToString(CultureInfo.InvariantCulture) : "named");
					}
					Collection<Attribute> compiledAttributes = parameter.CompiledAttributes;
					bool supportsWildcards = compiledAttributes.OfType<SupportsWildcardsAttribute>().Any<SupportsWildcardsAttribute>();
					string text = "";
					object obj = null;
					PSDefaultValueAttribute psdefaultValueAttribute = compiledAttributes.OfType<PSDefaultValueAttribute>().FirstOrDefault<PSDefaultValueAttribute>();
					if (psdefaultValueAttribute != null)
					{
						text = psdefaultValueAttribute.Help;
						if (string.IsNullOrEmpty(text))
						{
							obj = psdefaultValueAttribute.Value;
						}
					}
					if (string.IsNullOrEmpty(text))
					{
						RuntimeDefinedParameter runtimeDefinedParameter;
						if (obj == null && this.scriptBlock.RuntimeDefinedParameters.TryGetValue(key, out runtimeDefinedParameter))
						{
							obj = runtimeDefinedParameter.Value;
						}
						Compiler.DefaultValueExpressionWrapper defaultValueExpressionWrapper = obj as Compiler.DefaultValueExpressionWrapper;
						if (defaultValueExpressionWrapper != null)
						{
							text = defaultValueExpressionWrapper.Expression.Extent.Text;
						}
						else if (obj != null)
						{
							text = PSObject.ToStringParser(null, obj);
						}
					}
					XmlElement newChild6 = this.BuildXmlForParameter(key, isMandatory, valueFromPipeline, valueFromPipelineByPropertyName, position, parameter.Type, parameterDescription, supportsWildcards, text, false);
					xmlElement3.AppendChild(newChild6);
				}
			}
			xmlElement.AppendChild(xmlElement3);
			if (!string.IsNullOrEmpty(this._sections.Description))
			{
				XmlElement newChild7 = this.doc.CreateElement("maml:description", HelpCommentsParser.mamlURI);
				XmlElement newChild8 = this.doc.CreateElement("maml:para", HelpCommentsParser.mamlURI);
				XmlText newChild9 = this.doc.CreateTextNode(this._sections.Description);
				xmlElement.AppendChild(newChild7).AppendChild(newChild8).AppendChild(newChild9);
			}
			if (!string.IsNullOrEmpty(this._sections.Notes))
			{
				XmlElement newChild10 = this.doc.CreateElement("maml:alertSet", HelpCommentsParser.mamlURI);
				XmlElement newChild11 = this.doc.CreateElement("maml:alert", HelpCommentsParser.mamlURI);
				XmlElement newChild12 = this.doc.CreateElement("maml:para", HelpCommentsParser.mamlURI);
				XmlText newChild13 = this.doc.CreateTextNode(this._sections.Notes);
				xmlElement.AppendChild(newChild10).AppendChild(newChild11).AppendChild(newChild12).AppendChild(newChild13);
			}
			if (this._examples.Count > 0)
			{
				XmlElement xmlElement4 = this.doc.CreateElement("command:examples", HelpCommentsParser.commandURI);
				int num2 = 1;
				foreach (string content in this._examples)
				{
					XmlElement xmlElement5 = this.doc.CreateElement("command:example", HelpCommentsParser.commandURI);
					XmlElement newChild14 = this.doc.CreateElement("maml:title", HelpCommentsParser.mamlURI);
					string text2 = string.Format(CultureInfo.InvariantCulture, "\t\t\t\t-------------------------- {0} {1} --------------------------", new object[]
					{
						HelpDisplayStrings.ExampleUpperCase,
						num2++
					});
					XmlText newChild15 = this.doc.CreateTextNode(text2);
					xmlElement5.AppendChild(newChild14).AppendChild(newChild15);
					string text3;
					string text4;
					string text5;
					HelpCommentsParser.GetExampleSections(content, out text3, out text4, out text5);
					XmlElement newChild16 = this.doc.CreateElement("maml:introduction", HelpCommentsParser.mamlURI);
					XmlElement newChild17 = this.doc.CreateElement("maml:para", HelpCommentsParser.mamlURI);
					XmlText newChild18 = this.doc.CreateTextNode(text3);
					xmlElement5.AppendChild(newChild16).AppendChild(newChild17).AppendChild(newChild18);
					XmlElement newChild19 = this.doc.CreateElement("dev:code", HelpCommentsParser.devURI);
					XmlText newChild20 = this.doc.CreateTextNode(text4);
					xmlElement5.AppendChild(newChild19).AppendChild(newChild20);
					XmlElement xmlElement6 = this.doc.CreateElement("dev:remarks", HelpCommentsParser.devURI);
					XmlElement newChild21 = this.doc.CreateElement("maml:para", HelpCommentsParser.mamlURI);
					XmlText newChild22 = this.doc.CreateTextNode(text5);
					xmlElement5.AppendChild(xmlElement6).AppendChild(newChild21).AppendChild(newChild22);
					for (int j = 0; j < 4; j++)
					{
						xmlElement6.AppendChild(this.doc.CreateElement("maml:para", HelpCommentsParser.mamlURI));
					}
					xmlElement4.AppendChild(xmlElement5);
				}
				xmlElement.AppendChild(xmlElement4);
			}
			if (this._inputs.Count > 0)
			{
				XmlElement xmlElement7 = this.doc.CreateElement("command:inputTypes", HelpCommentsParser.commandURI);
				foreach (string text6 in this._inputs)
				{
					XmlElement newChild23 = this.doc.CreateElement("command:inputType", HelpCommentsParser.commandURI);
					XmlElement newChild24 = this.doc.CreateElement("dev:type", HelpCommentsParser.devURI);
					XmlElement newChild25 = this.doc.CreateElement("maml:name", HelpCommentsParser.mamlURI);
					XmlText newChild26 = this.doc.CreateTextNode(text6);
					xmlElement7.AppendChild(newChild23).AppendChild(newChild24).AppendChild(newChild25).AppendChild(newChild26);
				}
				xmlElement.AppendChild(xmlElement7);
			}
			IEnumerable enumerable = null;
			if (this._outputs.Count > 0)
			{
				enumerable = this._outputs;
			}
			else if (this.scriptBlock.OutputType.Count > 0)
			{
				enumerable = this.scriptBlock.OutputType;
			}
			if (enumerable != null)
			{
				XmlElement xmlElement8 = this.doc.CreateElement("command:returnValues", HelpCommentsParser.commandURI);
				foreach (object obj2 in enumerable)
				{
					XmlElement newChild27 = this.doc.CreateElement("command:returnValue", HelpCommentsParser.commandURI);
					XmlElement newChild28 = this.doc.CreateElement("dev:type", HelpCommentsParser.devURI);
					XmlElement newChild29 = this.doc.CreateElement("maml:name", HelpCommentsParser.mamlURI);
					string text7 = (obj2 as string) ?? ((PSTypeName)obj2).Name;
					XmlText newChild30 = this.doc.CreateTextNode(text7);
					xmlElement8.AppendChild(newChild27).AppendChild(newChild28).AppendChild(newChild29).AppendChild(newChild30);
				}
				xmlElement.AppendChild(xmlElement8);
			}
			if (this._links.Count > 0)
			{
				XmlElement xmlElement9 = this.doc.CreateElement("maml:relatedLinks", HelpCommentsParser.mamlURI);
				foreach (string text8 in this._links)
				{
					XmlElement newChild31 = this.doc.CreateElement("maml:navigationLink", HelpCommentsParser.mamlURI);
					string qualifiedName = Uri.IsWellFormedUriString(Uri.EscapeUriString(text8), UriKind.Absolute) ? "maml:uri" : "maml:linkText";
					XmlElement newChild32 = this.doc.CreateElement(qualifiedName, HelpCommentsParser.mamlURI);
					XmlText newChild33 = this.doc.CreateTextNode(text8);
					xmlElement9.AppendChild(newChild31).AppendChild(newChild32).AppendChild(newChild33);
				}
				xmlElement.AppendChild(xmlElement9);
			}
			return this.doc;
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00085D74 File Offset: 0x00083F74
		private void BuildSyntaxForParameterSet(XmlElement command, XmlElement syntax, MergedCommandParameterMetadata parameterMetadata, int i)
		{
			XmlElement xmlElement = this.doc.CreateElement("command:syntaxItem", HelpCommentsParser.commandURI);
			XmlElement newChild = this.doc.CreateElement("maml:name", HelpCommentsParser.mamlURI);
			XmlText newChild2 = this.doc.CreateTextNode(this.commandName);
			xmlElement.AppendChild(newChild).AppendChild(newChild2);
			Collection<MergedCompiledCommandParameter> parametersInParameterSet = parameterMetadata.GetParametersInParameterSet(1U << i);
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in parametersInParameterSet)
			{
				if (mergedCompiledCommandParameter.BinderAssociation != ParameterBinderAssociation.CommonParameters)
				{
					CompiledCommandParameter parameter = mergedCompiledCommandParameter.Parameter;
					ParameterSetSpecificMetadata parameterSetData = parameter.GetParameterSetData(1U << i);
					string parameterDescription = this.GetParameterDescription(parameter.Name);
					bool supportsWildcards = parameter.CompiledAttributes.Any((Attribute attribute) => attribute is SupportsWildcardsAttribute);
					XmlElement newChild3 = this.BuildXmlForParameter(parameter.Name, parameterSetData.IsMandatory, parameterSetData.ValueFromPipeline, parameterSetData.ValueFromPipelineByPropertyName, parameterSetData.IsPositional ? (1 + parameterSetData.Position).ToString(CultureInfo.InvariantCulture) : "named", parameter.Type, parameterDescription, supportsWildcards, "", true);
					xmlElement.AppendChild(newChild3);
				}
			}
			command.AppendChild(syntax).AppendChild(xmlElement);
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00085EF0 File Offset: 0x000840F0
		private static void GetExampleSections(string content, out string prompt_str, out string code_str, out string remarks_str)
		{
			string text;
			code_str = (text = "");
			prompt_str = text;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 1;
			foreach (char c in content)
			{
				if (c == '>' && num == 1)
				{
					stringBuilder.Append(c);
					prompt_str = stringBuilder.ToString().Trim();
					stringBuilder = new StringBuilder();
					num++;
				}
				else if (c == '\n' && num < 3)
				{
					if (num == 1)
					{
						prompt_str = "PS C:\\>";
					}
					code_str = stringBuilder.ToString().Trim();
					stringBuilder = new StringBuilder();
					num = 3;
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			if (num == 1)
			{
				prompt_str = "PS C:\\>";
				code_str = stringBuilder.ToString().Trim();
				remarks_str = "";
				return;
			}
			remarks_str = stringBuilder.ToString();
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00085FBC File Offset: 0x000841BC
		private static void CollectCommentText(Token comment, List<string> commentLines)
		{
			string text = comment.Text;
			HelpCommentsParser.CollectCommentText(text, commentLines);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00085FD8 File Offset: 0x000841D8
		private static void CollectCommentText(string text, List<string> commentLines)
		{
			int i = 0;
			if (text[0] == '<')
			{
				int num = 2;
				for (i = 2; i < text.Length - 2; i++)
				{
					if (text[i] == '\n')
					{
						commentLines.Add(text.Substring(num, i - num));
						num = i + 1;
					}
					else if (text[i] == '\r')
					{
						commentLines.Add(text.Substring(num, i - num));
						if (text[i + 1] == '\n')
						{
							i++;
						}
						num = i + 1;
					}
				}
				commentLines.Add(text.Substring(num, i - num));
				return;
			}
			while (i < text.Length && text[i] == '#')
			{
				i++;
			}
			commentLines.Add(text.Substring(i));
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00086094 File Offset: 0x00084294
		private static string GetSection(List<string> commentLines, ref int i)
		{
			bool flag = false;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			for (i++; i < commentLines.Count; i++)
			{
				string text = commentLines[i];
				if (flag || !Regex.IsMatch(text, "^\\s*$"))
				{
					if (Regex.IsMatch(text, "^\\s*\\.(\\w+)(\\s+(\\S.*))?\\s*$"))
					{
						i--;
						break;
					}
					if (!flag)
					{
						int num2 = 0;
						while (num2 < text.Length && (text[num2] == ' ' || text[num2] == '\t' || text[num2] == '\u00a0'))
						{
							num++;
							num2++;
						}
					}
					flag = true;
					int num3 = 0;
					while (num3 < text.Length && num3 < num && (text[num3] == ' ' || text[num3] == '\t' || text[num3] == '\u00a0'))
					{
						num3++;
					}
					stringBuilder.Append(text.Substring(num3));
					stringBuilder.Append('\n');
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x000861A4 File Offset: 0x000843A4
		internal string GetHelpFile(CommandInfo commandInfo)
		{
			if (this._sections.MamlHelpFile == null)
			{
				return null;
			}
			string file = this._sections.MamlHelpFile;
			Collection<string> searchPaths = new Collection<string>();
			string file2 = ((IScriptCommandInfo)commandInfo).ScriptBlock.File;
			if (!string.IsNullOrEmpty(file2))
			{
				file = Path.Combine(Path.GetDirectoryName(file2), this._sections.MamlHelpFile);
			}
			else if (commandInfo.Module != null)
			{
				file = Path.Combine(Path.GetDirectoryName(commandInfo.Module.Path), this._sections.MamlHelpFile);
			}
			return MUIFileSearcher.LocateFile(file, searchPaths);
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00086238 File Offset: 0x00084438
		internal RemoteHelpInfo GetRemoteHelpInfo(ExecutionContext context, CommandInfo commandInfo)
		{
			if (string.IsNullOrEmpty(this._sections.ForwardHelpTargetName) || string.IsNullOrEmpty(this._sections.RemoteHelpRunspace))
			{
				return null;
			}
			IScriptCommandInfo scriptCommandInfo = (IScriptCommandInfo)commandInfo;
			SessionState sessionState = scriptCommandInfo.ScriptBlock.SessionState;
			object value = sessionState.PSVariable.GetValue(this._sections.RemoteHelpRunspace);
			PSSession pssession;
			if (value == null || !LanguagePrimitives.TryConvertTo<PSSession>(value, out pssession))
			{
				string remoteRunspaceNotAvailable = HelpErrors.RemoteRunspaceNotAvailable;
				throw new InvalidOperationException(remoteRunspaceNotAvailable);
			}
			return new RemoteHelpInfo(context, (RemoteRunspace)pssession.Runspace, commandInfo.Name, this._sections.ForwardHelpTargetName, this._sections.ForwardHelpCategory, commandInfo.HelpCategory);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x000862E4 File Offset: 0x000844E4
		internal bool AnalyzeCommentBlock(List<Token> comments)
		{
			if (comments == null || comments.Count == 0)
			{
				return false;
			}
			List<string> commentLines = new List<string>();
			foreach (Token comment in comments)
			{
				HelpCommentsParser.CollectCommentText(comment, commentLines);
			}
			return this.AnalyzeCommentBlock(commentLines);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0008634C File Offset: 0x0008454C
		private bool AnalyzeCommentBlock(List<string> commentLines)
		{
			bool result = false;
			for (int i = 0; i < commentLines.Count; i++)
			{
				Match match = Regex.Match(commentLines[i], "^\\s*\\.(\\w+)(\\s+(\\S.*))?\\s*$");
				if (match.Success)
				{
					result = true;
					if (match.Groups[3].Success)
					{
						string a;
						if ((a = match.Groups[1].Value.ToUpperInvariant()) != null)
						{
							if (!(a == "PARAMETER"))
							{
								if (a == "FORWARDHELPTARGETNAME")
								{
									this._sections.ForwardHelpTargetName = match.Groups[3].Value.Trim();
									goto IL_3A4;
								}
								if (a == "FORWARDHELPCATEGORY")
								{
									this._sections.ForwardHelpCategory = match.Groups[3].Value.Trim();
									goto IL_3A4;
								}
								if (a == "REMOTEHELPRUNSPACE")
								{
									this._sections.RemoteHelpRunspace = match.Groups[3].Value.Trim();
									goto IL_3A4;
								}
								if (a == "EXTERNALHELP")
								{
									this._sections.MamlHelpFile = match.Groups[3].Value.Trim();
									this.isExternalHelpSet = true;
									goto IL_3A4;
								}
							}
							else
							{
								string key = match.Groups[3].Value.ToUpperInvariant().Trim();
								string section = HelpCommentsParser.GetSection(commentLines, ref i);
								if (!this._parameters.ContainsKey(key))
								{
									this._parameters.Add(key, section);
									goto IL_3A4;
								}
								goto IL_3A4;
							}
						}
						return false;
					}
					string key2;
					switch (key2 = match.Groups[1].Value.ToUpperInvariant())
					{
					case "SYNOPSIS":
						this._sections.Synopsis = HelpCommentsParser.GetSection(commentLines, ref i);
						goto IL_3A4;
					case "DESCRIPTION":
						this._sections.Description = HelpCommentsParser.GetSection(commentLines, ref i);
						goto IL_3A4;
					case "NOTES":
						this._sections.Notes = HelpCommentsParser.GetSection(commentLines, ref i);
						goto IL_3A4;
					case "LINK":
						this._links.Add(HelpCommentsParser.GetSection(commentLines, ref i).Trim());
						goto IL_3A4;
					case "EXAMPLE":
						this._examples.Add(HelpCommentsParser.GetSection(commentLines, ref i));
						goto IL_3A4;
					case "INPUTS":
						this._inputs.Add(HelpCommentsParser.GetSection(commentLines, ref i));
						goto IL_3A4;
					case "OUTPUTS":
						this._outputs.Add(HelpCommentsParser.GetSection(commentLines, ref i));
						goto IL_3A4;
					case "COMPONENT":
						this._sections.Component = HelpCommentsParser.GetSection(commentLines, ref i).Trim();
						goto IL_3A4;
					case "ROLE":
						this._sections.Role = HelpCommentsParser.GetSection(commentLines, ref i).Trim();
						goto IL_3A4;
					case "FUNCTIONALITY":
						this._sections.Functionality = HelpCommentsParser.GetSection(commentLines, ref i).Trim();
						goto IL_3A4;
					}
					return false;
				}
				else if (!Regex.IsMatch(commentLines[i], "^\\s*$"))
				{
					return false;
				}
				IL_3A4:;
			}
			this._sections.Examples = new ReadOnlyCollection<string>(this._examples);
			this._sections.Inputs = new ReadOnlyCollection<string>(this._inputs);
			this._sections.Outputs = new ReadOnlyCollection<string>(this._outputs);
			this._sections.Links = new ReadOnlyCollection<string>(this._links);
			this._sections.Parameters = new Dictionary<string, string>(this._parameters);
			return result;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0008677C File Offset: 0x0008497C
		internal void SetAdditionalData(MamlCommandHelpInfo helpInfo)
		{
			helpInfo.SetAdditionalDataFromHelpComment(this._sections.Component, this._sections.Functionality, this._sections.Role);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x000867A8 File Offset: 0x000849A8
		internal static CommentHelpInfo GetHelpContents(List<Token> comments, List<string> parameterDescriptions)
		{
			HelpCommentsParser helpCommentsParser = new HelpCommentsParser(parameterDescriptions);
			helpCommentsParser.AnalyzeCommentBlock(comments);
			return helpCommentsParser._sections;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x000867CC File Offset: 0x000849CC
		internal static HelpInfo CreateFromComments(ExecutionContext context, CommandInfo commandInfo, List<Token> comments, List<string> parameterDescriptions, bool dontSearchOnRemoteComputer, out string helpFile, out string helpUriFromDotLink)
		{
			HelpCommentsParser helpCommentsParser = new HelpCommentsParser(commandInfo, parameterDescriptions);
			helpCommentsParser.AnalyzeCommentBlock(comments);
			if (helpCommentsParser._sections.Links != null && helpCommentsParser._sections.Links.Count != 0)
			{
				helpUriFromDotLink = helpCommentsParser._sections.Links[0];
			}
			else
			{
				helpUriFromDotLink = null;
			}
			helpFile = helpCommentsParser.GetHelpFile(commandInfo);
			if (comments.Count == 1 && helpCommentsParser.isExternalHelpSet && helpFile == null)
			{
				return null;
			}
			return HelpCommentsParser.CreateFromComments(context, commandInfo, helpCommentsParser, dontSearchOnRemoteComputer);
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x00086850 File Offset: 0x00084A50
		internal static HelpInfo CreateFromComments(ExecutionContext context, CommandInfo commandInfo, HelpCommentsParser helpCommentsParser, bool dontSearchOnRemoteComputer)
		{
			if (!dontSearchOnRemoteComputer)
			{
				RemoteHelpInfo remoteHelpInfo = helpCommentsParser.GetRemoteHelpInfo(context, commandInfo);
				if (remoteHelpInfo != null)
				{
					if (remoteHelpInfo.GetUriForOnlineHelp() == null)
					{
						DefaultCommandHelpObjectBuilder.AddRelatedLinksProperties(remoteHelpInfo.FullHelp, commandInfo.CommandMetadata.HelpUri);
					}
					return remoteHelpInfo;
				}
			}
			XmlDocument xmlDocument = helpCommentsParser.BuildXmlFromComments();
			HelpCategory helpCategory = commandInfo.HelpCategory;
			MamlCommandHelpInfo mamlCommandHelpInfo = MamlCommandHelpInfo.Load(xmlDocument.DocumentElement, helpCategory);
			if (mamlCommandHelpInfo != null)
			{
				helpCommentsParser.SetAdditionalData(mamlCommandHelpInfo);
				if (!string.IsNullOrEmpty(helpCommentsParser._sections.ForwardHelpTargetName) || !string.IsNullOrEmpty(helpCommentsParser._sections.ForwardHelpCategory))
				{
					if (string.IsNullOrEmpty(helpCommentsParser._sections.ForwardHelpTargetName))
					{
						mamlCommandHelpInfo.ForwardTarget = mamlCommandHelpInfo.Name;
					}
					else
					{
						mamlCommandHelpInfo.ForwardTarget = helpCommentsParser._sections.ForwardHelpTargetName;
					}
					if (!string.IsNullOrEmpty(helpCommentsParser._sections.ForwardHelpCategory))
					{
						try
						{
							mamlCommandHelpInfo.ForwardHelpCategory = (HelpCategory)Enum.Parse(typeof(HelpCategory), helpCommentsParser._sections.ForwardHelpCategory, true);
							goto IL_FA;
						}
						catch (ArgumentException)
						{
							goto IL_FA;
						}
					}
					mamlCommandHelpInfo.ForwardHelpCategory = (HelpCategory.Alias | HelpCategory.Cmdlet | HelpCategory.ScriptCommand | HelpCategory.Function | HelpCategory.Filter | HelpCategory.ExternalScript | HelpCategory.Workflow);
				}
				IL_FA:
				WorkflowInfo workflowInfo = commandInfo as WorkflowInfo;
				if (workflowInfo != null)
				{
					bool flag = DefaultCommandHelpObjectBuilder.HasCommonParameters(commandInfo.Parameters);
					bool flag2 = (commandInfo.CommandType & CommandTypes.Workflow) == CommandTypes.Workflow;
					mamlCommandHelpInfo.FullHelp.Properties.Add(new PSNoteProperty("CommonParameters", flag));
					mamlCommandHelpInfo.FullHelp.Properties.Add(new PSNoteProperty("WorkflowCommonParameters", flag2));
					DefaultCommandHelpObjectBuilder.AddDetailsProperties(mamlCommandHelpInfo.FullHelp, workflowInfo.Name, workflowInfo.Noun, workflowInfo.Verb, "MamlCommandHelpInfo", mamlCommandHelpInfo.Synopsis);
					DefaultCommandHelpObjectBuilder.AddSyntaxProperties(mamlCommandHelpInfo.FullHelp, workflowInfo.Name, workflowInfo.ParameterSets, flag, flag2, "MamlCommandHelpInfo");
				}
				if (mamlCommandHelpInfo.GetUriForOnlineHelp() == null)
				{
					DefaultCommandHelpObjectBuilder.AddRelatedLinksProperties(mamlCommandHelpInfo.FullHelp, commandInfo.CommandMetadata.HelpUri);
				}
			}
			return mamlCommandHelpInfo;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00086A4C File Offset: 0x00084C4C
		internal static bool IsCommentHelpText(List<Token> commentBlock)
		{
			if (commentBlock == null || commentBlock.Count == 0)
			{
				return false;
			}
			HelpCommentsParser helpCommentsParser = new HelpCommentsParser();
			return helpCommentsParser.AnalyzeCommentBlock(commentBlock);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x00086A74 File Offset: 0x00084C74
		private static List<Token> GetCommentBlock(Token[] tokens, ref int startIndex)
		{
			List<Token> list = new List<Token>();
			int num = int.MaxValue;
			for (int i = startIndex; i < tokens.Length; i++)
			{
				Token token = tokens[i];
				if (token.Extent.StartLineNumber > num)
				{
					startIndex = i;
					break;
				}
				if (token.Kind == TokenKind.Comment)
				{
					list.Add(token);
					num = token.Extent.EndLineNumber + 1;
				}
				else if (token.Kind != TokenKind.NewLine)
				{
					startIndex = i;
					break;
				}
			}
			return list;
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x00086AE4 File Offset: 0x00084CE4
		private static List<Token> GetPrecedingCommentBlock(Token[] tokens, int tokenIndex, int proximity)
		{
			List<Token> list = new List<Token>();
			int num = tokens[tokenIndex].Extent.StartLineNumber - proximity;
			for (int i = tokenIndex - 1; i >= 0; i--)
			{
				Token token = tokens[i];
				if (token.Extent.EndLineNumber < num)
				{
					break;
				}
				if (token.Kind == TokenKind.Comment)
				{
					list.Add(token);
					num = token.Extent.StartLineNumber - 1;
				}
				else if (token.Kind != TokenKind.NewLine)
				{
					break;
				}
			}
			list.Reverse();
			return list;
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00086B58 File Offset: 0x00084D58
		private static int FirstTokenInExtent(Token[] tokens, IScriptExtent extent, int startIndex = 0)
		{
			int num = startIndex;
			while (num < tokens.Length && tokens[num].Extent.IsBefore(extent))
			{
				num++;
			}
			return num;
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x00086B84 File Offset: 0x00084D84
		private static int LastTokenInExtent(Token[] tokens, IScriptExtent extent, int startIndex)
		{
			int num = startIndex;
			while (num < tokens.Length && !tokens[num].Extent.IsAfter(extent))
			{
				num++;
			}
			return num - 1;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00086BB4 File Offset: 0x00084DB4
		private static List<string> GetParameterComments(Token[] tokens, IParameterMetadataProvider ipmp, int startIndex)
		{
			List<string> list = new List<string>();
			ReadOnlyCollection<ParameterAst> parameters = ipmp.Parameters;
			if (parameters == null || parameters.Count == 0)
			{
				return list;
			}
			foreach (ParameterAst parameterAst in parameters)
			{
				List<string> commentLines = new List<string>();
				int num = HelpCommentsParser.FirstTokenInExtent(tokens, parameterAst.Extent, startIndex);
				List<Token> list2 = HelpCommentsParser.GetPrecedingCommentBlock(tokens, num, 2);
				if (list2 != null)
				{
					foreach (Token comment in list2)
					{
						HelpCommentsParser.CollectCommentText(comment, commentLines);
					}
				}
				int num2 = HelpCommentsParser.LastTokenInExtent(tokens, parameterAst.Extent, num);
				for (int i = num; i < num2; i++)
				{
					if (tokens[i].Kind == TokenKind.Comment)
					{
						HelpCommentsParser.CollectCommentText(tokens[i], commentLines);
					}
				}
				num2++;
				list2 = HelpCommentsParser.GetCommentBlock(tokens, ref num2);
				if (list2 != null)
				{
					foreach (Token comment2 in list2)
					{
						HelpCommentsParser.CollectCommentText(comment2, commentLines);
					}
				}
				int num3 = -1;
				list.Add(HelpCommentsParser.GetSection(commentLines, ref num3));
			}
			return list;
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x00086D44 File Offset: 0x00084F44
		internal static Tuple<List<Token>, List<string>> GetHelpCommentTokens(IParameterMetadataProvider ipmp, Dictionary<Ast, Token[]> scriptBlockTokenCache)
		{
			Ast ast = (Ast)ipmp;
			Ast ast2 = ast;
			Ast ast3 = null;
			while (ast2.Parent != null)
			{
				ast2 = ast2.Parent;
				if (ast2 is ConfigurationDefinitionAst)
				{
					ast3 = ast2;
				}
			}
			Token[] array = null;
			scriptBlockTokenCache.TryGetValue(ast2, out array);
			if (array == null)
			{
				ParseError[] array2;
				Parser.ParseInput(ast2.Extent.Text, out array, out array2);
				scriptBlockTokenCache[ast2] = array;
			}
			FunctionDefinitionAst functionDefinitionAst = ast as FunctionDefinitionAst;
			int startIndex;
			List<Token> list;
			int startIndex2;
			int num2;
			if (functionDefinitionAst != null || ast3 != null)
			{
				int num;
				startIndex = (num = HelpCommentsParser.FirstTokenInExtent(array, (ast3 == null) ? ast.Extent : ast3.Extent, 0));
				list = HelpCommentsParser.GetPrecedingCommentBlock(array, num, 2);
				if (HelpCommentsParser.IsCommentHelpText(list))
				{
					return Tuple.Create<List<Token>, List<string>>(list, HelpCommentsParser.GetParameterComments(array, ipmp, startIndex));
				}
				if (functionDefinitionAst == null)
				{
					return null;
				}
				startIndex2 = HelpCommentsParser.FirstTokenInExtent(array, functionDefinitionAst.Body.Extent, 0) + 1;
				num2 = HelpCommentsParser.LastTokenInExtent(array, ast.Extent, num);
			}
			else if (ast == ast2)
			{
				startIndex = (startIndex2 = 0);
				num2 = array.Length - 1;
			}
			else
			{
				startIndex = (startIndex2 = HelpCommentsParser.FirstTokenInExtent(array, ast.Extent, 0) - 1);
				num2 = HelpCommentsParser.LastTokenInExtent(array, ast.Extent, startIndex2);
			}
			do
			{
				list = HelpCommentsParser.GetCommentBlock(array, ref startIndex2);
				if (list.Count == 0)
				{
					goto IL_1B5;
				}
			}
			while (!HelpCommentsParser.IsCommentHelpText(list));
			if (ast == ast2)
			{
				NamedBlockAst endBlock = ((ScriptBlockAst)ast).EndBlock;
				if (endBlock == null || !endBlock.Unnamed)
				{
					return Tuple.Create<List<Token>, List<string>>(list, HelpCommentsParser.GetParameterComments(array, ipmp, startIndex));
				}
				StatementAst statementAst = endBlock.Statements.FirstOrDefault<StatementAst>();
				if (statementAst is FunctionDefinitionAst)
				{
					int num3 = statementAst.Extent.StartLineNumber - list.Last<Token>().Extent.EndLineNumber;
					if (num3 > 2)
					{
						return Tuple.Create<List<Token>, List<string>>(list, HelpCommentsParser.GetParameterComments(array, ipmp, startIndex));
					}
					goto IL_1B5;
				}
			}
			return Tuple.Create<List<Token>, List<string>>(list, HelpCommentsParser.GetParameterComments(array, ipmp, startIndex));
			IL_1B5:
			list = HelpCommentsParser.GetPrecedingCommentBlock(array, num2, array[num2].Extent.StartLineNumber);
			if (HelpCommentsParser.IsCommentHelpText(list))
			{
				return Tuple.Create<List<Token>, List<string>>(list, HelpCommentsParser.GetParameterComments(array, ipmp, startIndex));
			}
			return null;
		}

		// Token: 0x04000910 RID: 2320
		private const string directive = "^\\s*\\.(\\w+)(\\s+(\\S.*))?\\s*$";

		// Token: 0x04000911 RID: 2321
		private const string blankline = "^\\s*$";

		// Token: 0x04000912 RID: 2322
		internal const int CommentBlockProximity = 2;

		// Token: 0x04000913 RID: 2323
		private readonly CommentHelpInfo _sections = new CommentHelpInfo();

		// Token: 0x04000914 RID: 2324
		private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();

		// Token: 0x04000915 RID: 2325
		private readonly List<string> _examples = new List<string>();

		// Token: 0x04000916 RID: 2326
		private readonly List<string> _inputs = new List<string>();

		// Token: 0x04000917 RID: 2327
		private readonly List<string> _outputs = new List<string>();

		// Token: 0x04000918 RID: 2328
		private readonly List<string> _links = new List<string>();

		// Token: 0x04000919 RID: 2329
		internal bool isExternalHelpSet;

		// Token: 0x0400091A RID: 2330
		private ScriptBlock scriptBlock;

		// Token: 0x0400091B RID: 2331
		private CommandMetadata commandMetadata;

		// Token: 0x0400091C RID: 2332
		private string commandName;

		// Token: 0x0400091D RID: 2333
		private List<string> parameterDescriptions;

		// Token: 0x0400091E RID: 2334
		private XmlDocument doc;

		// Token: 0x0400091F RID: 2335
		internal static readonly string mamlURI = "http://schemas.microsoft.com/maml/2004/10";

		// Token: 0x04000920 RID: 2336
		internal static readonly string commandURI = "http://schemas.microsoft.com/maml/dev/command/2004/10";

		// Token: 0x04000921 RID: 2337
		internal static readonly string devURI = "http://schemas.microsoft.com/maml/dev/2004/10";

		// Token: 0x04000922 RID: 2338
		internal static readonly string ProviderHelpCommandXPath = "/helpItems/providerHelp/CmdletHelpPaths/CmdletHelpPath{0}/command:command[command:details/command:verb='{1}' and command:details/command:noun='{2}']";
	}
}
