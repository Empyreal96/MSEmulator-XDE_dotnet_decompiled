using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using Microsoft.PowerShell.Cmdletization.Xml;
using Microsoft.PowerShell.Commands;

namespace Microsoft.PowerShell.Cmdletization
{
	// Token: 0x020009A6 RID: 2470
	internal sealed class ScriptWriter
	{
		// Token: 0x06005AF3 RID: 23283 RVA: 0x001E8DB4 File Offset: 0x001E6FB4
		static ScriptWriter()
		{
			ScriptWriter.xmlReaderSettings = new XmlReaderSettings();
			ScriptWriter.xmlReaderSettings.CheckCharacters = true;
			ScriptWriter.xmlReaderSettings.CloseInput = false;
			ScriptWriter.xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
			ScriptWriter.xmlReaderSettings.IgnoreComments = true;
			ScriptWriter.xmlReaderSettings.IgnoreProcessingInstructions = true;
			ScriptWriter.xmlReaderSettings.IgnoreWhitespace = false;
			ScriptWriter.xmlReaderSettings.MaxCharactersFromEntities = 16384L;
			ScriptWriter.xmlReaderSettings.MaxCharactersInDocument = 134217728L;
			ScriptWriter.xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;
			ScriptWriter.xmlReaderSettings.XmlResolver = null;
			ScriptWriter.xmlReaderSettings.ValidationFlags = (XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints);
			ScriptWriter.xmlReaderSettings.ValidationType = ValidationType.Schema;
			string xml_cmdletsOverObjectsXsd = CmdletizationCoreResources.Xml_cmdletsOverObjectsXsd;
			XmlReader schemaDocument = XmlReader.Create(new StringReader(xml_cmdletsOverObjectsXsd), ScriptWriter.xmlReaderSettings);
			ScriptWriter.xmlReaderSettings.Schemas = new XmlSchemaSet();
			ScriptWriter.xmlReaderSettings.Schemas.Add(null, schemaDocument);
			ScriptWriter.xmlReaderSettings.Schemas.XmlResolver = null;
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x001E8EAC File Offset: 0x001E70AC
		internal ScriptWriter(TextReader cmdletizationXmlReader, string moduleName, string defaultObjectModelWrapper, InvocationInfo invocationInfo, ScriptWriter.GenerationOptions generationOptions)
		{
			XmlReader xmlReader = XmlReader.Create(cmdletizationXmlReader, ScriptWriter.xmlReaderSettings);
			try
			{
				PowerShellMetadataSerializer powerShellMetadataSerializer = new PowerShellMetadataSerializer();
				this.cmdletizationMetadata = (PowerShellMetadata)powerShellMetadataSerializer.Deserialize(xmlReader);
			}
			catch (InvalidOperationException ex)
			{
				XmlSchemaException ex2 = ex.InnerException as XmlSchemaException;
				if (ex2 != null)
				{
					throw new XmlException(ex2.Message, ex2, ex2.LineNumber, ex2.LinePosition);
				}
				XmlException ex3 = ex.InnerException as XmlException;
				if (ex3 != null)
				{
					throw ex3;
				}
				if (ex.InnerException != null)
				{
					string message = string.Format(CultureInfo.CurrentCulture, CmdletizationCoreResources.ScriptWriter_ConcatenationOfDeserializationExceptions, new object[]
					{
						ex.Message,
						ex.InnerException.Message
					});
					throw new InvalidOperationException(message, ex.InnerException);
				}
				throw;
			}
			string text = this.cmdletizationMetadata.Class.CmdletAdapter ?? defaultObjectModelWrapper;
			this.objectModelWrapper = (Type)LanguagePrimitives.ConvertTo(text, typeof(Type), CultureInfo.InvariantCulture);
			TypeInfo typeInfo = this.objectModelWrapper.GetTypeInfo();
			if (typeInfo.IsGenericType)
			{
				string message2 = string.Format(CultureInfo.CurrentCulture, CmdletizationCoreResources.ScriptWriter_ObjectModelWrapperIsStillGeneric, new object[]
				{
					text
				});
				throw new XmlException(message2);
			}
			Type baseType = this.objectModelWrapper;
			TypeInfo typeInfo2 = typeInfo;
			while (!typeInfo2.IsGenericType || typeInfo2.GetGenericTypeDefinition() != typeof(CmdletAdapter<>))
			{
				baseType = typeInfo2.BaseType;
				if (baseType == typeof(object))
				{
					string message3 = string.Format(CultureInfo.CurrentCulture, CmdletizationCoreResources.ScriptWriter_ObjectModelWrapperNotDerivedFromObjectModelWrapper, new object[]
					{
						text,
						typeof(CmdletAdapter<>).FullName
					});
					throw new XmlException(message3);
				}
				typeInfo2 = baseType.GetTypeInfo();
			}
			this.objectInstanceType = baseType.GetGenericArguments()[0];
			this.moduleName = moduleName;
			this.invocationInfo = invocationInfo;
			this.generationOptions = generationOptions;
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x001E90D4 File Offset: 0x001E72D4
		private void WriteModulePreamble(TextWriter output)
		{
			output.WriteLine("\r\n#requires -version 3.0\r\n\r\ntry {{ Microsoft.PowerShell.Core\\Set-StrictMode -Off }} catch {{ }}\r\n\r\n$script:MyModule = $MyInvocation.MyCommand.ScriptBlock.Module\r\n\r\n$script:ClassName = '{0}'\r\n$script:ClassVersion = '{1}'\r\n$script:ModuleVersion = '{2}'\r\n$script:ObjectModelWrapper = [{3}]\r\n\r\n$script:PrivateData = [System.Collections.Generic.Dictionary[string,string]]::new()\r\n\r\nMicrosoft.PowerShell.Core\\Export-ModuleMember -Function @()\r\n        ", new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(this.cmdletizationMetadata.Class.ClassName),
				CodeGeneration.EscapeSingleQuotedStringContent(this.cmdletizationMetadata.Class.ClassVersion ?? string.Empty),
				CodeGeneration.EscapeSingleQuotedStringContent(new Version(this.cmdletizationMetadata.Class.Version).ToString()),
				CodeGeneration.EscapeSingleQuotedStringContent(this.objectModelWrapper.FullName)
			});
			if (this.cmdletizationMetadata.Class.CmdletAdapterPrivateData != null)
			{
				foreach (ClassMetadataData classMetadataData in this.cmdletizationMetadata.Class.CmdletAdapterPrivateData)
				{
					output.WriteLine("$script:PrivateData.Add('{0}', '{1}')", CodeGeneration.EscapeSingleQuotedStringContent(classMetadataData.Name), CodeGeneration.EscapeSingleQuotedStringContent(classMetadataData.Value));
				}
			}
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x001E91B8 File Offset: 0x001E73B8
		private void WriteBindCommonParametersFunction(TextWriter output)
		{
			output.WriteLine("\r\nfunction __cmdletization_BindCommonParameters\r\n{\r\n    param(\r\n        $__cmdletization_objectModelWrapper,\r\n        $myPSBoundParameters\r\n    )       \r\n                ");
			foreach (ParameterMetadata parameterMetadata in this.GetCommonParameters().Values)
			{
				output.WriteLine("\r\n        if ($myPSBoundParameters.ContainsKey('{0}')) {{ \r\n            $__cmdletization_objectModelWrapper.PSObject.Properties['{0}'].Value = $myPSBoundParameters['{0}'] \r\n        }}\r\n                    ", CodeGeneration.EscapeSingleQuotedStringContent(parameterMetadata.Name));
			}
			output.WriteLine("\r\n}\r\n                ");
		}

		// Token: 0x06005AF7 RID: 23287 RVA: 0x001E9238 File Offset: 0x001E7438
		private string GetCmdletName(CommonCmdletMetadata cmdletMetadata)
		{
			string str = cmdletMetadata.Noun ?? this.cmdletizationMetadata.Class.DefaultNoun;
			string verb = cmdletMetadata.Verb;
			return verb + "-" + str;
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x001E927C File Offset: 0x001E747C
		private string GetCmdletAttributes(CommonCmdletMetadata cmdletMetadata)
		{
			StringBuilder stringBuilder = new StringBuilder(150);
			if (cmdletMetadata.Aliases != null)
			{
				stringBuilder.Append("[Alias('" + string.Join("','", from alias in cmdletMetadata.Aliases
				select CodeGeneration.EscapeSingleQuotedStringContent(alias)) + "')]");
				this.aliasesToExport.AddRange(cmdletMetadata.Aliases);
			}
			if (cmdletMetadata.Obsolete != null)
			{
				string text = (cmdletMetadata.Obsolete.Message != null) ? ("'" + CodeGeneration.EscapeSingleQuotedStringContent(cmdletMetadata.Obsolete.Message) + "'") : string.Empty;
				string text2 = (stringBuilder.Length > 0) ? Environment.NewLine : string.Empty;
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}[Obsolete({1})]", new object[]
				{
					text2,
					text
				});
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005AF9 RID: 23289 RVA: 0x001E9370 File Offset: 0x001E7570
		private Dictionary<string, ParameterMetadata> GetCommonParameters()
		{
			Dictionary<string, ParameterMetadata> dictionary = new Dictionary<string, ParameterMetadata>(StringComparer.OrdinalIgnoreCase);
			InternalParameterMetadata internalParameterMetadata = new InternalParameterMetadata(this.objectModelWrapper, false);
			foreach (CompiledCommandParameter cmdParameterMD in internalParameterMetadata.BindableParameters.Values)
			{
				ParameterMetadata parameterMetadata = new ParameterMetadata(cmdParameterMD);
				foreach (ParameterSetMetadata parameterSetMetadata in parameterMetadata.ParameterSets.Values)
				{
					if (parameterSetMetadata.ValueFromPipeline)
					{
						string message = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_ObjectModelWrapperUsesIgnoredParameterMetadata, new object[]
						{
							this.objectModelWrapper.FullName,
							parameterMetadata.Name,
							"ValueFromPipeline"
						});
						throw new XmlException(message);
					}
					if (parameterSetMetadata.ValueFromPipelineByPropertyName)
					{
						string message2 = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_ObjectModelWrapperUsesIgnoredParameterMetadata, new object[]
						{
							this.objectModelWrapper.FullName,
							parameterMetadata.Name,
							"ValueFromPipelineByPropertyName"
						});
						throw new XmlException(message2);
					}
					if (parameterSetMetadata.ValueFromRemainingArguments)
					{
						string message3 = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_ObjectModelWrapperUsesIgnoredParameterMetadata, new object[]
						{
							this.objectModelWrapper.FullName,
							parameterMetadata.Name,
							"ValueFromRemainingArguments"
						});
						throw new XmlException(message3);
					}
					parameterSetMetadata.ValueFromPipeline = false;
					parameterSetMetadata.ValueFromPipelineByPropertyName = false;
					parameterSetMetadata.ValueFromRemainingArguments = false;
				}
				dictionary.Add(parameterMetadata.Name, parameterMetadata);
			}
			List<string> commonParameterSets = ScriptWriter.GetCommonParameterSets(dictionary);
			if (commonParameterSets.Count > 1)
			{
				string message4 = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_ObjectModelWrapperDefinesMultipleParameterSets, new object[]
				{
					this.objectModelWrapper.FullName
				});
				throw new XmlException(message4);
			}
			foreach (ParameterMetadata parameterMetadata2 in dictionary.Values)
			{
				if (parameterMetadata2.ParameterSets.Count == 1 && parameterMetadata2.ParameterSets.ContainsKey("__AllParameterSets"))
				{
					ParameterSetMetadata value = parameterMetadata2.ParameterSets["__AllParameterSets"];
					parameterMetadata2.ParameterSets.Clear();
					foreach (string key in commonParameterSets)
					{
						parameterMetadata2.ParameterSets.Add(key, value);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06005AFA RID: 23290 RVA: 0x001E9680 File Offset: 0x001E7880
		private static List<string> GetCommonParameterSets(Dictionary<string, ParameterMetadata> commonParameters)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			foreach (ParameterMetadata parameterMetadata in commonParameters.Values)
			{
				foreach (string text in parameterMetadata.ParameterSets.Keys)
				{
					if (!text.Equals("__AllParameterSets"))
					{
						dictionary[text] = null;
					}
				}
			}
			if (dictionary.Count == 0)
			{
				dictionary.Add("__AllParameterSets", null);
			}
			List<string> list = new List<string>(dictionary.Keys);
			list.Sort(StringComparer.Ordinal);
			return list;
		}

		// Token: 0x06005AFB RID: 23291 RVA: 0x001E975C File Offset: 0x001E795C
		private string GetMethodParameterSet(StaticMethodMetadata staticMethod)
		{
			return staticMethod.CmdletParameterSet ?? this.GetMethodParameterSet(staticMethod);
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x001E9770 File Offset: 0x001E7970
		private List<string> GetMethodParameterSets(StaticCmdletMetadata staticCmdlet)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			foreach (StaticMethodMetadata staticMethod in staticCmdlet.Method)
			{
				string methodParameterSet = this.GetMethodParameterSet(staticMethod);
				if (dictionary.ContainsKey(methodParameterSet))
				{
					string message = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_DuplicateParameterSetInStaticCmdlet, new object[]
					{
						this.GetCmdletName(staticCmdlet.CmdletMetadata),
						methodParameterSet
					});
					throw new XmlException(message);
				}
				dictionary.Add(methodParameterSet, null);
			}
			return new List<string>(dictionary.Keys);
		}

		// Token: 0x06005AFD RID: 23293 RVA: 0x001E9804 File Offset: 0x001E7A04
		private string GetMethodParameterSet(CommonMethodMetadata methodMetadata)
		{
			int count;
			if (!this._staticMethodMetadataToUniqueId.TryGetValue(methodMetadata, out count))
			{
				count = this._staticMethodMetadataToUniqueId.Count;
				this._staticMethodMetadataToUniqueId.Add(methodMetadata, count);
			}
			return methodMetadata.MethodName + count;
		}

		// Token: 0x06005AFE RID: 23294 RVA: 0x001E984C File Offset: 0x001E7A4C
		private List<string> GetMethodParameterSets(InstanceCmdletMetadata instanceCmdlet)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			InstanceMethodMetadata method = instanceCmdlet.Method;
			string methodParameterSet = this.GetMethodParameterSet(method);
			dictionary.Add(methodParameterSet, null);
			return new List<string>(dictionary.Keys);
		}

		// Token: 0x06005AFF RID: 23295 RVA: 0x001E9888 File Offset: 0x001E7A88
		private GetCmdletParameters GetGetCmdletParameters(InstanceCmdletMetadata instanceCmdlet)
		{
			if (instanceCmdlet == null)
			{
				if (this.cmdletizationMetadata.Class.InstanceCmdlets.GetCmdlet != null && this.cmdletizationMetadata.Class.InstanceCmdlets.GetCmdlet.GetCmdletParameters != null)
				{
					return this.cmdletizationMetadata.Class.InstanceCmdlets.GetCmdlet.GetCmdletParameters;
				}
			}
			else if (instanceCmdlet.GetCmdletParameters != null)
			{
				return instanceCmdlet.GetCmdletParameters;
			}
			return this.cmdletizationMetadata.Class.InstanceCmdlets.GetCmdletParameters;
		}

		// Token: 0x06005B00 RID: 23296 RVA: 0x001E990C File Offset: 0x001E7B0C
		private List<string> GetQueryParameterSets(InstanceCmdletMetadata instanceCmdlet)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			List<CmdletParameterMetadataForGetCmdletParameter> list = new List<CmdletParameterMetadataForGetCmdletParameter>();
			bool flag = false;
			GetCmdletParameters getCmdletParameters = this.GetGetCmdletParameters(instanceCmdlet);
			if (getCmdletParameters.QueryableProperties != null)
			{
				foreach (PropertyMetadata propertyMetadata in getCmdletParameters.QueryableProperties)
				{
					if (propertyMetadata.Items != null)
					{
						foreach (PropertyQuery propertyQuery in propertyMetadata.Items)
						{
							flag = true;
							if (propertyQuery.CmdletParameterMetadata != null)
							{
								list.Add(propertyQuery.CmdletParameterMetadata);
							}
						}
					}
				}
			}
			if (getCmdletParameters.QueryableAssociations != null)
			{
				foreach (Association association in getCmdletParameters.QueryableAssociations)
				{
					if (association.AssociatedInstance != null)
					{
						flag = true;
						if (association.AssociatedInstance.CmdletParameterMetadata != null)
						{
							list.Add(association.AssociatedInstance.CmdletParameterMetadata);
						}
					}
				}
			}
			if (getCmdletParameters.QueryOptions != null)
			{
				foreach (QueryOption queryOption in getCmdletParameters.QueryOptions)
				{
					flag = true;
					if (queryOption.CmdletParameterMetadata != null)
					{
						list.Add(queryOption.CmdletParameterMetadata);
					}
				}
			}
			foreach (CmdletParameterMetadataForGetCmdletParameter cmdletParameterMetadataForGetCmdletParameter in list)
			{
				if (cmdletParameterMetadataForGetCmdletParameter.CmdletParameterSets != null)
				{
					foreach (string key in cmdletParameterMetadataForGetCmdletParameter.CmdletParameterSets)
					{
						dictionary[key] = null;
					}
				}
			}
			if (flag && dictionary.Count == 0)
			{
				dictionary.Add("Query (cdxml)", null);
				getCmdletParameters.DefaultCmdletParameterSet = "Query (cdxml)";
			}
			if (instanceCmdlet != null)
			{
				dictionary.Add("InputObject (cdxml)", null);
			}
			return new List<string>(dictionary.Keys);
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x001E9B38 File Offset: 0x001E7D38
		private Type GetDotNetType(TypeMetadata typeMetadata)
		{
			EnumMetadataEnum[] enums = this.cmdletizationMetadata.Enums;
			List<EnumMetadataEnum> list = (from e in (enums != null) ? ((IEnumerable<EnumMetadataEnum>)enums) : Enumerable.Empty<EnumMetadataEnum>()
			where Regex.IsMatch(typeMetadata.PSType, string.Format(CultureInfo.InvariantCulture, "\\b{0}\\b", new object[]
			{
				Regex.Escape(e.EnumName)
			}), RegexOptions.CultureInvariant)
			select e).ToList<EnumMetadataEnum>();
			EnumMetadataEnum enumMetadataEnum = (list.Count == 1) ? list[0] : null;
			string valueToConvert;
			if (enumMetadataEnum != null)
			{
				valueToConvert = typeMetadata.PSType.Replace(enumMetadataEnum.EnumName, EnumWriter.GetEnumFullName(enumMetadataEnum));
			}
			else
			{
				valueToConvert = typeMetadata.PSType;
			}
			return (Type)LanguagePrimitives.ConvertTo(valueToConvert, typeof(Type), CultureInfo.InvariantCulture);
		}

		// Token: 0x06005B02 RID: 23298 RVA: 0x001E9BE8 File Offset: 0x001E7DE8
		private ParameterMetadata GetParameter(string parameterSetName, string objectModelParameterName, TypeMetadata parameterTypeMetadata, CmdletParameterMetadata parameterCmdletization, bool isValueFromPipeline, bool isValueFromPipelineByPropertyName)
		{
			string name;
			if (parameterCmdletization != null && !string.IsNullOrEmpty(parameterCmdletization.PSName))
			{
				name = parameterCmdletization.PSName;
			}
			else
			{
				name = objectModelParameterName;
			}
			ParameterMetadata parameterMetadata = new ParameterMetadata(name);
			parameterMetadata.ParameterType = this.GetDotNetType(parameterTypeMetadata);
			if (typeof(PSCredential).Equals(parameterMetadata.ParameterType))
			{
				parameterMetadata.Attributes.Add(new CredentialAttribute());
			}
			if (parameterTypeMetadata.ETSType != null)
			{
				parameterMetadata.Attributes.Add(new PSTypeNameAttribute(parameterTypeMetadata.ETSType));
			}
			if (parameterCmdletization != null)
			{
				if (parameterCmdletization.Aliases != null)
				{
					foreach (string text in parameterCmdletization.Aliases)
					{
						if (!string.IsNullOrEmpty(text))
						{
							parameterMetadata.Aliases.Add(text);
						}
					}
				}
				if (parameterCmdletization.AllowEmptyCollection != null)
				{
					parameterMetadata.Attributes.Add(new AllowEmptyCollectionAttribute());
				}
				if (parameterCmdletization.AllowEmptyString != null)
				{
					parameterMetadata.Attributes.Add(new AllowEmptyStringAttribute());
				}
				if (parameterCmdletization.AllowNull != null)
				{
					parameterMetadata.Attributes.Add(new AllowNullAttribute());
				}
				if (parameterCmdletization.ValidateCount != null)
				{
					int minLength = (int)LanguagePrimitives.ConvertTo(parameterCmdletization.ValidateCount.Min, typeof(int), CultureInfo.InvariantCulture);
					int maxLength = (int)LanguagePrimitives.ConvertTo(parameterCmdletization.ValidateCount.Max, typeof(int), CultureInfo.InvariantCulture);
					parameterMetadata.Attributes.Add(new ValidateCountAttribute(minLength, maxLength));
				}
				if (parameterCmdletization.ValidateLength != null)
				{
					int minLength2 = (int)LanguagePrimitives.ConvertTo(parameterCmdletization.ValidateLength.Min, typeof(int), CultureInfo.InvariantCulture);
					int maxLength2 = (int)LanguagePrimitives.ConvertTo(parameterCmdletization.ValidateLength.Max, typeof(int), CultureInfo.InvariantCulture);
					parameterMetadata.Attributes.Add(new ValidateLengthAttribute(minLength2, maxLength2));
				}
				if (parameterCmdletization.Obsolete != null)
				{
					string message = parameterCmdletization.Obsolete.Message;
					parameterMetadata.Attributes.Add((message != null) ? new ObsoleteAttribute(message) : new ObsoleteAttribute());
				}
				if (parameterCmdletization.ValidateNotNull != null)
				{
					parameterMetadata.Attributes.Add(new ValidateNotNullAttribute());
				}
				if (parameterCmdletization.ValidateNotNullOrEmpty != null)
				{
					parameterMetadata.Attributes.Add(new ValidateNotNullOrEmptyAttribute());
				}
				if (parameterCmdletization.ValidateRange != null)
				{
					Type parameterType = parameterMetadata.ParameterType;
					Type resultType;
					if (parameterType == null)
					{
						resultType = typeof(string);
					}
					else
					{
						resultType = (parameterType.HasElementType ? parameterType.GetElementType() : parameterType);
					}
					object minRange = LanguagePrimitives.ConvertTo(parameterCmdletization.ValidateRange.Min, resultType, CultureInfo.InvariantCulture);
					object maxRange = LanguagePrimitives.ConvertTo(parameterCmdletization.ValidateRange.Max, resultType, CultureInfo.InvariantCulture);
					parameterMetadata.Attributes.Add(new ValidateRangeAttribute(minRange, maxRange));
				}
				if (parameterCmdletization.ValidateSet != null)
				{
					List<string> list = new List<string>();
					foreach (string item in parameterCmdletization.ValidateSet)
					{
						list.Add(item);
					}
					parameterMetadata.Attributes.Add(new ValidateSetAttribute(list.ToArray()));
				}
			}
			int position = int.MinValue;
			ParameterSetMetadata.ParameterFlags parameterFlags = (ParameterSetMetadata.ParameterFlags)0U;
			if (parameterCmdletization != null)
			{
				if (!string.IsNullOrEmpty(parameterCmdletization.Position))
				{
					position = (int)LanguagePrimitives.ConvertTo(parameterCmdletization.Position, typeof(int), CultureInfo.InvariantCulture);
				}
				if (parameterCmdletization.IsMandatorySpecified && parameterCmdletization.IsMandatory)
				{
					parameterFlags |= ParameterSetMetadata.ParameterFlags.Mandatory;
				}
			}
			if (isValueFromPipeline)
			{
				parameterFlags |= ParameterSetMetadata.ParameterFlags.ValueFromPipeline;
			}
			if (isValueFromPipelineByPropertyName)
			{
				parameterFlags |= ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName;
			}
			parameterMetadata.ParameterSets.Add(parameterSetName, new ParameterSetMetadata(position, parameterFlags, null));
			return parameterMetadata;
		}

		// Token: 0x06005B03 RID: 23299 RVA: 0x001E9F92 File Offset: 0x001E8192
		private ParameterMetadata GetParameter(string parameterSetName, string objectModelParameterName, TypeMetadata parameterType, CmdletParameterMetadataForInstanceMethodParameter parameterCmdletization)
		{
			return this.GetParameter(parameterSetName, objectModelParameterName, parameterType, parameterCmdletization, false, parameterCmdletization != null && parameterCmdletization.ValueFromPipelineByPropertyNameSpecified && parameterCmdletization.ValueFromPipelineByPropertyName);
		}

		// Token: 0x06005B04 RID: 23300 RVA: 0x001E9FB8 File Offset: 0x001E81B8
		private ParameterMetadata GetParameter(IEnumerable<string> queryParameterSets, string objectModelParameterName, TypeMetadata parameterType, CmdletParameterMetadataForGetCmdletParameter parameterCmdletization)
		{
			ParameterMetadata parameter = this.GetParameter("__AllParameterSets", objectModelParameterName, parameterType, parameterCmdletization, parameterCmdletization != null && parameterCmdletization.ValueFromPipelineSpecified && parameterCmdletization.ValueFromPipeline, parameterCmdletization != null && parameterCmdletization.ValueFromPipelineByPropertyNameSpecified && parameterCmdletization.ValueFromPipelineByPropertyName);
			ParameterSetMetadata value = parameter.ParameterSets["__AllParameterSets"];
			parameter.ParameterSets.Clear();
			if (parameterCmdletization != null && parameterCmdletization.CmdletParameterSets != null && parameterCmdletization.CmdletParameterSets.Length > 0)
			{
				queryParameterSets = parameterCmdletization.CmdletParameterSets;
			}
			foreach (string text in queryParameterSets)
			{
				if (!text.Equals("InputObject (cdxml)", StringComparison.OrdinalIgnoreCase))
				{
					parameter.ParameterSets.Add(text, value);
				}
			}
			return parameter;
		}

		// Token: 0x06005B05 RID: 23301 RVA: 0x001EA094 File Offset: 0x001E8294
		private ParameterMetadata GetParameter(string parameterSetName, string objectModelParameterName, TypeMetadata parameterType, CmdletParameterMetadataForStaticMethodParameter parameterCmdletization)
		{
			return this.GetParameter(parameterSetName, objectModelParameterName, parameterType, parameterCmdletization, parameterCmdletization != null && parameterCmdletization.ValueFromPipelineSpecified && parameterCmdletization.ValueFromPipeline, parameterCmdletization != null && parameterCmdletization.ValueFromPipelineByPropertyNameSpecified && parameterCmdletization.ValueFromPipelineByPropertyName);
		}

		// Token: 0x06005B06 RID: 23302 RVA: 0x001EA0D0 File Offset: 0x001E82D0
		private void SetParameters(CommandMetadata commandMetadata, params Dictionary<string, ParameterMetadata>[] allParameters)
		{
			commandMetadata.Parameters.Clear();
			foreach (Dictionary<string, ParameterMetadata> dictionary in allParameters)
			{
				foreach (KeyValuePair<string, ParameterMetadata> keyValuePair in dictionary)
				{
					if (commandMetadata.Parameters.ContainsKey(keyValuePair.Key))
					{
						if (this.GetCommonParameters().ContainsKey(keyValuePair.Key))
						{
							string message = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_ParameterNameConflictsWithCommonParameters, new object[]
							{
								keyValuePair.Key,
								commandMetadata.Name,
								this.objectModelWrapper.FullName
							});
							throw new XmlException(message);
						}
						string message2 = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_ParameterNameConflictsWithQueryParameters, new object[]
						{
							keyValuePair.Key,
							commandMetadata.Name,
							"<GetCmdletParameters>"
						});
						throw new XmlException(message2);
					}
					else
					{
						commandMetadata.Parameters.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
		}

		// Token: 0x06005B07 RID: 23303 RVA: 0x001EA214 File Offset: 0x001E8414
		private CommandMetadata GetCommandMetadata(CommonCmdletMetadata cmdletMetadata)
		{
			string defaultParameterSetName = null;
			StaticCmdletMetadataCmdletMetadata staticCmdletMetadataCmdletMetadata = cmdletMetadata as StaticCmdletMetadataCmdletMetadata;
			if (staticCmdletMetadataCmdletMetadata != null && !string.IsNullOrEmpty(staticCmdletMetadataCmdletMetadata.DefaultCmdletParameterSet))
			{
				defaultParameterSetName = staticCmdletMetadataCmdletMetadata.DefaultCmdletParameterSet;
			}
			System.Management.Automation.ConfirmImpact confirmImpact = System.Management.Automation.ConfirmImpact.None;
			if (cmdletMetadata.ConfirmImpactSpecified)
			{
				confirmImpact = (System.Management.Automation.ConfirmImpact)cmdletMetadata.ConfirmImpact;
			}
			Dictionary<string, ParameterMetadata> parameters = new Dictionary<string, ParameterMetadata>(StringComparer.OrdinalIgnoreCase);
			CommandMetadata commandMetadata = new CommandMetadata(this.GetCmdletName(cmdletMetadata), CommandTypes.Cmdlet, true, defaultParameterSetName, confirmImpact != System.Management.Automation.ConfirmImpact.None, confirmImpact, false, false, false, parameters);
			if (!string.IsNullOrEmpty(cmdletMetadata.HelpUri))
			{
				commandMetadata.HelpUri = cmdletMetadata.HelpUri;
			}
			return commandMetadata;
		}

		// Token: 0x06005B08 RID: 23304 RVA: 0x001EA298 File Offset: 0x001E8498
		private static string EscapeModuleNameForHelpComment(string name)
		{
			StringBuilder stringBuilder = new StringBuilder(name.Length);
			foreach (char c in name)
			{
				if ("\"'`$#".IndexOf(c) == -1 && !char.IsControl(c) && !char.IsWhiteSpace(c))
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x001EA2F8 File Offset: 0x001E84F8
		private static List<List<string>> GetCombinations(params IEnumerable<string>[] x)
		{
			if (x.Length == 1)
			{
				List<List<string>> list = new List<List<string>>();
				foreach (string item in x[0])
				{
					list.Add(new List<string>
					{
						item
					});
				}
				return list;
			}
			IEnumerable<string>[] array = new IEnumerable<string>[x.Length - 1];
			Array.Copy(x, 0, array, 0, array.Length);
			List<List<string>> combinations = ScriptWriter.GetCombinations(array);
			List<List<string>> list2 = new List<List<string>>();
			foreach (List<string> collection in combinations)
			{
				foreach (string item2 in x[x.Length - 1])
				{
					list2.Add(new List<string>(collection)
					{
						item2
					});
				}
			}
			return list2;
		}

		// Token: 0x06005B0A RID: 23306 RVA: 0x001EA41C File Offset: 0x001E861C
		private static void EnsureOrderOfPositionalParameters(Dictionary<string, ParameterMetadata> beforeParameters, Dictionary<string, ParameterMetadata> afterParameters)
		{
			int num = int.MinValue;
			foreach (ParameterMetadata parameterMetadata in beforeParameters.Values)
			{
				foreach (ParameterSetMetadata parameterSetMetadata in parameterMetadata.ParameterSets.Values)
				{
					num = Math.Max(parameterSetMetadata.Position, num);
				}
			}
			int num2 = int.MaxValue;
			foreach (ParameterMetadata parameterMetadata2 in afterParameters.Values)
			{
				foreach (ParameterSetMetadata parameterSetMetadata2 in parameterMetadata2.ParameterSets.Values)
				{
					if (parameterSetMetadata2.Position != -2147483648)
					{
						num2 = Math.Min(parameterSetMetadata2.Position, num2);
					}
				}
			}
			if (num >= 0 && num2 <= num)
			{
				int num3 = 1001 - num2 % 1000;
				checked
				{
					foreach (ParameterMetadata parameterMetadata3 in afterParameters.Values)
					{
						foreach (ParameterSetMetadata parameterSetMetadata3 in parameterMetadata3.ParameterSets.Values)
						{
							if (parameterSetMetadata3.Position != -2147483648)
							{
								parameterSetMetadata3.Position += num3;
							}
						}
					}
				}
			}
		}

		// Token: 0x06005B0B RID: 23307 RVA: 0x001EA61C File Offset: 0x001E881C
		private static void MultiplyParameterSets(Dictionary<string, ParameterMetadata> parameters, string parameterSetNameTemplate, params IEnumerable<string>[] otherParameterSets)
		{
			List<List<string>> combinations = ScriptWriter.GetCombinations(otherParameterSets);
			foreach (ParameterMetadata parameterMetadata in parameters.Values)
			{
				List<KeyValuePair<string, ParameterSetMetadata>> list = new List<KeyValuePair<string, ParameterSetMetadata>>(parameterMetadata.ParameterSets);
				parameterMetadata.ParameterSets.Clear();
				foreach (KeyValuePair<string, ParameterSetMetadata> keyValuePair in list)
				{
					foreach (List<string> list2 in combinations)
					{
						string[] array = new string[otherParameterSets.Length + 1];
						array[0] = keyValuePair.Key;
						list2.CopyTo(array, 1);
						string key = string.Format(CultureInfo.InvariantCulture, parameterSetNameTemplate, array);
						parameterMetadata.ParameterSets.Add(key, keyValuePair.Value);
					}
				}
			}
		}

		// Token: 0x06005B0C RID: 23308 RVA: 0x001EA744 File Offset: 0x001E8944
		private static IEnumerable<string> MultiplyParameterSets(string mainParameterSet, string parameterSetNameTemplate, params IEnumerable<string>[] otherParameterSets)
		{
			List<string> list = new List<string>();
			List<List<string>> combinations = ScriptWriter.GetCombinations(otherParameterSets);
			foreach (List<string> list2 in combinations)
			{
				string[] array = new string[otherParameterSets.Length + 1];
				array[0] = mainParameterSet;
				list2.CopyTo(array, 1);
				string item = string.Format(CultureInfo.InvariantCulture, parameterSetNameTemplate, array);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06005B0D RID: 23309 RVA: 0x001EA7C8 File Offset: 0x001E89C8
		private static MethodParameterBindings GetMethodParameterKind(InstanceMethodParameterMetadata methodParameter)
		{
			MethodParameterBindings methodParameterBindings = (MethodParameterBindings)0;
			if (methodParameter.CmdletParameterMetadata != null)
			{
				methodParameterBindings |= MethodParameterBindings.In;
			}
			if (methodParameter.CmdletOutputMetadata != null)
			{
				if (methodParameter.CmdletOutputMetadata.ErrorCode == null)
				{
					methodParameterBindings |= MethodParameterBindings.Out;
				}
				else
				{
					methodParameterBindings |= MethodParameterBindings.Error;
				}
			}
			return methodParameterBindings;
		}

		// Token: 0x06005B0E RID: 23310 RVA: 0x001EA804 File Offset: 0x001E8A04
		private static MethodParameterBindings GetMethodParameterKind(StaticMethodParameterMetadata methodParameter)
		{
			MethodParameterBindings methodParameterBindings = (MethodParameterBindings)0;
			if (methodParameter.CmdletParameterMetadata != null)
			{
				methodParameterBindings |= MethodParameterBindings.In;
			}
			if (methodParameter.CmdletOutputMetadata != null)
			{
				if (methodParameter.CmdletOutputMetadata.ErrorCode == null)
				{
					methodParameterBindings |= MethodParameterBindings.Out;
				}
				else
				{
					methodParameterBindings |= MethodParameterBindings.Error;
				}
			}
			return methodParameterBindings;
		}

		// Token: 0x06005B0F RID: 23311 RVA: 0x001EA840 File Offset: 0x001E8A40
		private static MethodParameterBindings GetMethodParameterKind(CommonMethodMetadataReturnValue returnValue)
		{
			MethodParameterBindings methodParameterBindings = (MethodParameterBindings)0;
			if (returnValue.CmdletOutputMetadata != null)
			{
				if (returnValue.CmdletOutputMetadata.ErrorCode == null)
				{
					methodParameterBindings |= MethodParameterBindings.Out;
				}
				else
				{
					methodParameterBindings |= MethodParameterBindings.Error;
				}
			}
			return methodParameterBindings;
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x001EA870 File Offset: 0x001E8A70
		private static void GenerateSingleMethodParameterProcessing(TextWriter output, string prefix, string cmdletParameterName, Type cmdletParameterType, string etsParameterTypeName, string cmdletParameterDefaultValue, string methodParameterName, MethodParameterBindings methodParameterBindings)
		{
			string fullName = (cmdletParameterType ?? typeof(object)).FullName;
			if (cmdletParameterDefaultValue != null)
			{
				output.WriteLine("{0}[object]$__cmdletization_defaultValue = [System.Management.Automation.LanguagePrimitives]::ConvertTo('{1}', '{2}')", prefix, CodeGeneration.EscapeSingleQuotedStringContent(cmdletParameterDefaultValue), CodeGeneration.EscapeSingleQuotedStringContent(fullName));
				output.WriteLine("{0}[object]$__cmdletization_defaultValueIsPresent = $true", prefix);
			}
			else
			{
				output.WriteLine("{0}[object]$__cmdletization_defaultValue = $null", prefix);
				output.WriteLine("{0}[object]$__cmdletization_defaultValueIsPresent = $false", prefix);
			}
			if (MethodParameterBindings.In == (methodParameterBindings & MethodParameterBindings.In))
			{
				output.WriteLine("{0}if ($PSBoundParameters.ContainsKey('{1}')) {{", prefix, CodeGeneration.EscapeSingleQuotedStringContent(cmdletParameterName));
				output.WriteLine("{0}  [object]$__cmdletization_value = ${{{1}}}", prefix, CodeGeneration.EscapeVariableName(cmdletParameterName));
				output.WriteLine("{0}  $__cmdletization_methodParameter = [Microsoft.PowerShell.Cmdletization.MethodParameter]@{{Name = '{1}'; ParameterType = '{2}'; Bindings = '{3}'; Value = $__cmdletization_value; IsValuePresent = $true}}", new object[]
				{
					prefix,
					CodeGeneration.EscapeSingleQuotedStringContent(methodParameterName),
					CodeGeneration.EscapeSingleQuotedStringContent(fullName),
					CodeGeneration.EscapeSingleQuotedStringContent(methodParameterBindings.ToString())
				});
				output.WriteLine("{0}}} else {{", prefix);
			}
			output.WriteLine("{0}  $__cmdletization_methodParameter = [Microsoft.PowerShell.Cmdletization.MethodParameter]@{{Name = '{1}'; ParameterType = '{2}'; Bindings = '{3}'; Value = $__cmdletization_defaultValue; IsValuePresent = $__cmdletization_defaultValueIsPresent}}", new object[]
			{
				prefix,
				CodeGeneration.EscapeSingleQuotedStringContent(methodParameterName),
				CodeGeneration.EscapeSingleQuotedStringContent(fullName),
				CodeGeneration.EscapeSingleQuotedStringContent(methodParameterBindings.ToString())
			});
			if (MethodParameterBindings.In == (methodParameterBindings & MethodParameterBindings.In))
			{
				output.WriteLine("{0}}}", prefix);
			}
			if (!string.IsNullOrEmpty(etsParameterTypeName))
			{
				output.WriteLine("{0}$__cmdletization_methodParameter.ParameterTypeName = '{1}'", prefix, CodeGeneration.EscapeSingleQuotedStringContent(etsParameterTypeName));
			}
			output.WriteLine("{0}$__cmdletization_methodParameters.Add($__cmdletization_methodParameter)", prefix);
			output.WriteLine();
		}

		// Token: 0x06005B11 RID: 23313 RVA: 0x001EA9CC File Offset: 0x001E8BCC
		private void GenerateMethodParametersProcessing(StaticCmdletMetadata staticCmdlet, IEnumerable<string> commonParameterSets, out string scriptCode, out Dictionary<string, ParameterMetadata> methodParameters, out string outputTypeAttributeDeclaration)
		{
			methodParameters = new Dictionary<string, ParameterMetadata>(StringComparer.OrdinalIgnoreCase);
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			stringWriter.WriteLine("      $__cmdletization_methodParameters = [System.Collections.Generic.List[Microsoft.PowerShell.Cmdletization.MethodParameter]]::new()");
			stringWriter.WriteLine();
			bool flag = staticCmdlet.Method.Length > 1;
			if (flag)
			{
				stringWriter.WriteLine("      switch -exact ($PSCmdlet.ParameterSetName) { ");
			}
			foreach (StaticMethodMetadata staticMethodMetadata in staticCmdlet.Method)
			{
				if (flag)
				{
					stringWriter.Write("        { @(");
					bool flag2 = true;
					foreach (string value in ScriptWriter.MultiplyParameterSets(this.GetMethodParameterSet(staticMethodMetadata), "{0}", new IEnumerable<string>[]
					{
						commonParameterSets
					}))
					{
						if (!flag2)
						{
							stringWriter.Write(", ");
						}
						flag2 = false;
						stringWriter.Write("'{0}'", CodeGeneration.EscapeSingleQuotedStringContent(value));
					}
					stringWriter.WriteLine(") -contains $_ } {");
				}
				List<Type> list = new List<Type>();
				List<string> list2 = new List<string>();
				if (staticMethodMetadata.Parameters != null)
				{
					foreach (StaticMethodParameterMetadata staticMethodParameterMetadata in staticMethodMetadata.Parameters)
					{
						string cmdletParameterName = null;
						if (staticMethodParameterMetadata.CmdletParameterMetadata != null)
						{
							string methodParameterSet = this.GetMethodParameterSet(staticMethodMetadata);
							ParameterMetadata parameter = this.GetParameter(methodParameterSet, staticMethodParameterMetadata.ParameterName, staticMethodParameterMetadata.Type, staticMethodParameterMetadata.CmdletParameterMetadata);
							cmdletParameterName = parameter.Name;
							ParameterMetadata parameterMetadata;
							if (methodParameters.TryGetValue(parameter.Name, out parameterMetadata))
							{
								try
								{
									parameterMetadata.ParameterSets.Add(methodParameterSet, parameter.ParameterSets[methodParameterSet]);
									goto IL_1D8;
								}
								catch (ArgumentException innerException)
								{
									string message = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_DuplicateQueryParameterName, new object[]
									{
										"<StaticCmdlets>...<Cmdlet>...<Method>",
										parameter.Name
									});
									throw new XmlException(message, innerException);
								}
							}
							methodParameters.Add(parameter.Name, parameter);
						}
						IL_1D8:
						MethodParameterBindings methodParameterKind = ScriptWriter.GetMethodParameterKind(staticMethodParameterMetadata);
						Type dotNetType = this.GetDotNetType(staticMethodParameterMetadata.Type);
						ScriptWriter.GenerateSingleMethodParameterProcessing(stringWriter, "        ", cmdletParameterName, dotNetType, staticMethodParameterMetadata.Type.ETSType, staticMethodParameterMetadata.DefaultValue, staticMethodParameterMetadata.ParameterName, methodParameterKind);
						if (MethodParameterBindings.Out == (methodParameterKind & MethodParameterBindings.Out))
						{
							list.Add(dotNetType);
							list2.Add(staticMethodParameterMetadata.Type.ETSType);
						}
					}
				}
				if (staticMethodMetadata.ReturnValue != null)
				{
					MethodParameterBindings methodParameterKind2 = ScriptWriter.GetMethodParameterKind(staticMethodMetadata.ReturnValue);
					Type dotNetType2 = this.GetDotNetType(staticMethodMetadata.ReturnValue.Type);
					stringWriter.WriteLine("      $__cmdletization_returnValue = [Microsoft.PowerShell.Cmdletization.MethodParameter]@{{ Name = 'ReturnValue'; ParameterType = '{0}'; Bindings = '{1}'; Value = $null; IsValuePresent = $false }}", CodeGeneration.EscapeSingleQuotedStringContent(dotNetType2.FullName), CodeGeneration.EscapeSingleQuotedStringContent(methodParameterKind2.ToString()));
					if (!string.IsNullOrEmpty(staticMethodMetadata.ReturnValue.Type.ETSType))
					{
						stringWriter.WriteLine("      $__cmdletization_methodParameter.ParameterTypeName = '{0}'", CodeGeneration.EscapeSingleQuotedStringContent(staticMethodMetadata.ReturnValue.Type.ETSType));
					}
					if (MethodParameterBindings.Out == (methodParameterKind2 & MethodParameterBindings.Out))
					{
						list.Add(dotNetType2);
						list2.Add(staticMethodMetadata.ReturnValue.Type.ETSType);
					}
				}
				else
				{
					stringWriter.WriteLine("      $__cmdletization_returnValue = $null");
				}
				stringWriter.WriteLine("      $__cmdletization_methodInvocationInfo = [Microsoft.PowerShell.Cmdletization.MethodInvocationInfo]::new('{0}', $__cmdletization_methodParameters, $__cmdletization_returnValue)", CodeGeneration.EscapeSingleQuotedStringContent(staticMethodMetadata.MethodName));
				stringWriter.WriteLine("      $__cmdletization_objectModelWrapper.ProcessRecord($__cmdletization_methodInvocationInfo)");
				if (flag)
				{
					stringWriter.WriteLine("        }");
				}
				if (list.Count == 1)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "[OutputType([{0}])]", new object[]
					{
						list[0].FullName
					});
					if (list2.Count == 1 && !string.IsNullOrEmpty(list2[0]))
					{
						stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "[OutputType('{0}')]", new object[]
						{
							CodeGeneration.EscapeSingleQuotedStringContent(list2[0])
						});
					}
				}
			}
			if (flag)
			{
				stringWriter.WriteLine("    }");
			}
			scriptCode = stringWriter.ToString();
			outputTypeAttributeDeclaration = stringBuilder.ToString();
		}

		// Token: 0x06005B12 RID: 23314 RVA: 0x001EADDC File Offset: 0x001E8FDC
		private void GenerateMethodParametersProcessing(InstanceCmdletMetadata instanceCmdlet, IEnumerable<string> commonParameterSets, IEnumerable<string> queryParameterSets, out string scriptCode, out Dictionary<string, ParameterMetadata> methodParameters, out string outputTypeAttributeDeclaration)
		{
			methodParameters = new Dictionary<string, ParameterMetadata>(StringComparer.OrdinalIgnoreCase);
			outputTypeAttributeDeclaration = string.Empty;
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			stringWriter.WriteLine("    $__cmdletization_methodParameters = [System.Collections.Generic.List[Microsoft.PowerShell.Cmdletization.MethodParameter]]::new()");
			stringWriter.WriteLine("    switch -exact ($PSCmdlet.ParameterSetName) { ");
			InstanceMethodMetadata method = instanceCmdlet.Method;
			stringWriter.Write("        { @(");
			bool flag = true;
			foreach (string value in ScriptWriter.MultiplyParameterSets(this.GetMethodParameterSet(method), "{2}", new IEnumerable<string>[]
			{
				commonParameterSets,
				queryParameterSets
			}))
			{
				if (!flag)
				{
					stringWriter.Write(", ");
				}
				flag = false;
				stringWriter.Write("'{0}'", CodeGeneration.EscapeSingleQuotedStringContent(value));
			}
			stringWriter.WriteLine(") -contains $_ } {");
			List<Type> list = new List<Type>();
			List<string> list2 = new List<string>();
			if (method.Parameters != null)
			{
				foreach (InstanceMethodParameterMetadata instanceMethodParameterMetadata in method.Parameters)
				{
					string cmdletParameterName = null;
					if (instanceMethodParameterMetadata.CmdletParameterMetadata != null)
					{
						ParameterMetadata parameter = this.GetParameter(this.GetMethodParameterSet(method), instanceMethodParameterMetadata.ParameterName, instanceMethodParameterMetadata.Type, instanceMethodParameterMetadata.CmdletParameterMetadata);
						cmdletParameterName = parameter.Name;
						try
						{
							methodParameters.Add(parameter.Name, parameter);
						}
						catch (ArgumentException innerException)
						{
							string message = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_DuplicateQueryParameterName, new object[]
							{
								"<InstanceCmdlets>...<Cmdlet>",
								parameter.Name
							});
							throw new XmlException(message, innerException);
						}
					}
					MethodParameterBindings methodParameterKind = ScriptWriter.GetMethodParameterKind(instanceMethodParameterMetadata);
					Type dotNetType = this.GetDotNetType(instanceMethodParameterMetadata.Type);
					ScriptWriter.GenerateSingleMethodParameterProcessing(stringWriter, "          ", cmdletParameterName, dotNetType, instanceMethodParameterMetadata.Type.ETSType, instanceMethodParameterMetadata.DefaultValue, instanceMethodParameterMetadata.ParameterName, methodParameterKind);
					if (MethodParameterBindings.Out == (methodParameterKind & MethodParameterBindings.Out))
					{
						list.Add(dotNetType);
						list2.Add(instanceMethodParameterMetadata.Type.ETSType);
					}
				}
			}
			if (method.ReturnValue != null)
			{
				MethodParameterBindings methodParameterKind2 = ScriptWriter.GetMethodParameterKind(method.ReturnValue);
				Type dotNetType2 = this.GetDotNetType(method.ReturnValue.Type);
				stringWriter.WriteLine("      $__cmdletization_returnValue = [Microsoft.PowerShell.Cmdletization.MethodParameter]@{{ Name = 'ReturnValue'; ParameterType = '{0}'; Bindings = '{1}'; Value = $null; IsValuePresent = $false }}", CodeGeneration.EscapeSingleQuotedStringContent(dotNetType2.FullName), CodeGeneration.EscapeSingleQuotedStringContent(methodParameterKind2.ToString()));
				if (!string.IsNullOrEmpty(method.ReturnValue.Type.ETSType))
				{
					stringWriter.WriteLine("      $__cmdletization_methodParameter.ParameterTypeName = '{0}'", CodeGeneration.EscapeSingleQuotedStringContent(method.ReturnValue.Type.ETSType));
				}
				if (MethodParameterBindings.Out == (methodParameterKind2 & MethodParameterBindings.Out))
				{
					list.Add(dotNetType2);
					list2.Add(method.ReturnValue.Type.ETSType);
				}
			}
			else
			{
				stringWriter.WriteLine("      $__cmdletization_returnValue = $null");
			}
			stringWriter.WriteLine("      $__cmdletization_methodInvocationInfo = [Microsoft.PowerShell.Cmdletization.MethodInvocationInfo]::new('{0}', $__cmdletization_methodParameters, $__cmdletization_returnValue)", CodeGeneration.EscapeSingleQuotedStringContent(method.MethodName));
			if (list.Count == 0)
			{
				stringWriter.WriteLine("      $__cmdletization_passThru = $PSBoundParameters.ContainsKey('PassThru') -and $PassThru");
			}
			else
			{
				stringWriter.WriteLine("      $__cmdletization_passThru = $false");
			}
			stringWriter.WriteLine("            if ($PSBoundParameters.ContainsKey('InputObject')) {");
			stringWriter.WriteLine("                foreach ($x in $InputObject) { $__cmdletization_objectModelWrapper.ProcessRecord($x, $__cmdletization_methodInvocationInfo, $__cmdletization_PassThru) }");
			stringWriter.WriteLine("            } else {");
			stringWriter.WriteLine("                $__cmdletization_objectModelWrapper.ProcessRecord($__cmdletization_queryBuilder, $__cmdletization_methodInvocationInfo, $__cmdletization_PassThru)");
			stringWriter.WriteLine("            }");
			stringWriter.WriteLine("        }");
			stringWriter.WriteLine("    }");
			scriptCode = stringWriter.ToString();
			if (list.Count == 0)
			{
				outputTypeAttributeDeclaration = this.GetOutputAttributeForGetCmdlet();
				return;
			}
			if (list.Count == 1)
			{
				outputTypeAttributeDeclaration = string.Format(CultureInfo.InvariantCulture, "[OutputType([{0}])]", new object[]
				{
					list[0].FullName
				});
				if (list2.Count == 1 && !string.IsNullOrEmpty(list2[0]))
				{
					outputTypeAttributeDeclaration += string.Format(CultureInfo.InvariantCulture, "[OutputType('{0}')]", new object[]
					{
						CodeGeneration.EscapeSingleQuotedStringContent(list2[0])
					});
				}
			}
		}

		// Token: 0x06005B13 RID: 23315 RVA: 0x001EB1D8 File Offset: 0x001E93D8
		private void GenerateIfBoundParameter(IEnumerable<string> commonParameterSets, IEnumerable<string> methodParameterSets, ParameterMetadata cmdletParameterMetadata, TextWriter output)
		{
			output.Write("    if ($PSBoundParameters.ContainsKey('{0}') -and (@(", CodeGeneration.EscapeSingleQuotedStringContent(cmdletParameterMetadata.Name));
			bool flag = true;
			foreach (string mainParameterSet in cmdletParameterMetadata.ParameterSets.Keys)
			{
				foreach (string value in ScriptWriter.MultiplyParameterSets(mainParameterSet, "{0}", new IEnumerable<string>[]
				{
					commonParameterSets,
					methodParameterSets
				}))
				{
					if (!flag)
					{
						output.Write(", ");
					}
					flag = false;
					output.Write("'{0}'", CodeGeneration.EscapeSingleQuotedStringContent(value));
				}
			}
			output.WriteLine(") -contains $PSCmdlet.ParameterSetName )) {");
		}

		// Token: 0x06005B14 RID: 23316 RVA: 0x001EB2C4 File Offset: 0x001E94C4
		private ParameterMetadata GenerateQueryClause(IEnumerable<string> commonParameterSets, IEnumerable<string> queryParameterSets, IEnumerable<string> methodParameterSets, string queryBuilderMethodName, PropertyMetadata property, PropertyQuery query, TextWriter output)
		{
			ParameterMetadata parameter = this.GetParameter(queryParameterSets, property.PropertyName, property.Type, query.CmdletParameterMetadata);
			WildcardablePropertyQuery wildcardablePropertyQuery = query as WildcardablePropertyQuery;
			if (wildcardablePropertyQuery != null && !parameter.SwitchParameter)
			{
				if (parameter.ParameterType == null)
				{
					parameter.ParameterType = typeof(object);
				}
				parameter.ParameterType = parameter.ParameterType.MakeArrayType();
			}
			this.GenerateIfBoundParameter(commonParameterSets, methodParameterSets, parameter, output);
			string text = (wildcardablePropertyQuery == null) ? "__cmdletization_value" : "__cmdletization_values";
			if (wildcardablePropertyQuery == null)
			{
				output.WriteLine("        [object]${0} = ${{{1}}}", text, CodeGeneration.EscapeVariableName(parameter.Name));
			}
			else
			{
				output.WriteLine("        ${0} = @(${{{1}}})", text, CodeGeneration.EscapeVariableName(parameter.Name));
			}
			output.Write("        $__cmdletization_queryBuilder.{0}('{1}', ${2}", queryBuilderMethodName, CodeGeneration.EscapeSingleQuotedStringContent(property.PropertyName), text);
			if (wildcardablePropertyQuery == null)
			{
				output.WriteLine(", '{0}')", ScriptWriter.GetBehaviorWhenNoMatchesFound(query.CmdletParameterMetadata));
			}
			else
			{
				bool flag = (!wildcardablePropertyQuery.AllowGlobbingSpecified && parameter.ParameterType.Equals(typeof(string[]))) || (wildcardablePropertyQuery.AllowGlobbingSpecified && wildcardablePropertyQuery.AllowGlobbing);
				output.WriteLine(", {0}, '{1}')", flag ? "$true" : "$false", ScriptWriter.GetBehaviorWhenNoMatchesFound(query.CmdletParameterMetadata));
			}
			output.WriteLine("    }");
			return parameter;
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x001EB42A File Offset: 0x001E962A
		private static BehaviorOnNoMatch GetBehaviorWhenNoMatchesFound(CmdletParameterMetadataForGetCmdletFilteringParameter cmdletParameterMetadata)
		{
			if (cmdletParameterMetadata == null || !cmdletParameterMetadata.ErrorOnNoMatchSpecified)
			{
				return BehaviorOnNoMatch.Default;
			}
			if (cmdletParameterMetadata.ErrorOnNoMatch)
			{
				return BehaviorOnNoMatch.ReportErrors;
			}
			return BehaviorOnNoMatch.SilentlyContinue;
		}

		// Token: 0x06005B16 RID: 23318 RVA: 0x001EB444 File Offset: 0x001E9644
		private ParameterMetadata GenerateAssociationClause(IEnumerable<string> commonParameterSets, IEnumerable<string> queryParameterSets, IEnumerable<string> methodParameterSets, Association associationMetadata, AssociationAssociatedInstance associatedInstanceMetadata, TextWriter output)
		{
			ParameterMetadata parameter = this.GetParameter(queryParameterSets, associationMetadata.SourceRole, associatedInstanceMetadata.Type, associatedInstanceMetadata.CmdletParameterMetadata);
			parameter.Attributes.Add(new ValidateNotNullAttribute());
			this.GenerateIfBoundParameter(commonParameterSets, methodParameterSets, parameter, output);
			output.WriteLine("    $__cmdletization_queryBuilder.FilterByAssociatedInstance(${{{0}}}, '{1}', '{2}', '{3}', '{4}')", new object[]
			{
				CodeGeneration.EscapeVariableName(parameter.Name),
				CodeGeneration.EscapeSingleQuotedStringContent(associationMetadata.Association1),
				CodeGeneration.EscapeSingleQuotedStringContent(associationMetadata.SourceRole),
				CodeGeneration.EscapeSingleQuotedStringContent(associationMetadata.ResultRole),
				ScriptWriter.GetBehaviorWhenNoMatchesFound(associatedInstanceMetadata.CmdletParameterMetadata)
			});
			output.WriteLine("    }");
			return parameter;
		}

		// Token: 0x06005B17 RID: 23319 RVA: 0x001EB4FC File Offset: 0x001E96FC
		private ParameterMetadata GenerateOptionClause(IEnumerable<string> commonParameterSets, IEnumerable<string> queryParameterSets, IEnumerable<string> methodParameterSets, QueryOption queryOptionMetadata, TextWriter output)
		{
			ParameterMetadata parameter = this.GetParameter(queryParameterSets, queryOptionMetadata.OptionName, queryOptionMetadata.Type, queryOptionMetadata.CmdletParameterMetadata);
			this.GenerateIfBoundParameter(commonParameterSets, methodParameterSets, parameter, output);
			output.WriteLine("    $__cmdletization_queryBuilder.AddQueryOption('{0}', ${{{1}}})", CodeGeneration.EscapeSingleQuotedStringContent(queryOptionMetadata.OptionName), CodeGeneration.EscapeVariableName(parameter.Name));
			output.WriteLine("    }");
			return parameter;
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x001EB580 File Offset: 0x001E9780
		private void GenerateQueryParametersProcessing(InstanceCmdletMetadata instanceCmdlet, IEnumerable<string> commonParameterSets, IEnumerable<string> queryParameterSets, IEnumerable<string> methodParameterSets, out string scriptCode, out Dictionary<string, ParameterMetadata> queryParameters)
		{
			queryParameters = new Dictionary<string, ParameterMetadata>(StringComparer.OrdinalIgnoreCase);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			stringWriter.WriteLine("    $__cmdletization_queryBuilder = $__cmdletization_objectModelWrapper.GetQueryBuilder()");
			GetCmdletParameters getCmdletParameters = this.GetGetCmdletParameters(instanceCmdlet);
			if (getCmdletParameters.QueryableProperties != null)
			{
				foreach (PropertyMetadata propertyMetadata in from p in getCmdletParameters.QueryableProperties
				where p.Items != null
				select p)
				{
					for (int i = 0; i < propertyMetadata.Items.Length; i++)
					{
						string queryBuilderMethodName;
						switch (propertyMetadata.ItemsElementName[i])
						{
						case ItemsChoiceType.ExcludeQuery:
							queryBuilderMethodName = "ExcludeByProperty";
							break;
						case ItemsChoiceType.MaxValueQuery:
							queryBuilderMethodName = "FilterByMaxPropertyValue";
							break;
						case ItemsChoiceType.MinValueQuery:
							queryBuilderMethodName = "FilterByMinPropertyValue";
							break;
						case ItemsChoiceType.RegularQuery:
							queryBuilderMethodName = "FilterByProperty";
							break;
						default:
							queryBuilderMethodName = "NotAValidMethod";
							break;
						}
						ParameterMetadata parameterMetadata = this.GenerateQueryClause(commonParameterSets, queryParameterSets, methodParameterSets, queryBuilderMethodName, propertyMetadata, propertyMetadata.Items[i], stringWriter);
						ItemsChoiceType itemsChoiceType = propertyMetadata.ItemsElementName[i];
						if (itemsChoiceType == ItemsChoiceType.ExcludeQuery || itemsChoiceType == ItemsChoiceType.RegularQuery)
						{
							parameterMetadata.Attributes.Add(new ValidateNotNullAttribute());
						}
						try
						{
							queryParameters.Add(parameterMetadata.Name, parameterMetadata);
						}
						catch (ArgumentException innerException)
						{
							string message = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_DuplicateQueryParameterName, new object[]
							{
								"<GetCmdletParameters>",
								parameterMetadata.Name
							});
							throw new XmlException(message, innerException);
						}
					}
				}
			}
			if (getCmdletParameters.QueryableAssociations != null)
			{
				foreach (Association association in from a in getCmdletParameters.QueryableAssociations
				where a.AssociatedInstance != null
				select a)
				{
					ParameterMetadata parameterMetadata2 = this.GenerateAssociationClause(commonParameterSets, queryParameterSets, methodParameterSets, association, association.AssociatedInstance, stringWriter);
					try
					{
						queryParameters.Add(parameterMetadata2.Name, parameterMetadata2);
					}
					catch (ArgumentException innerException2)
					{
						string message2 = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_DuplicateQueryParameterName, new object[]
						{
							"<GetCmdletParameters>",
							parameterMetadata2.Name
						});
						throw new XmlException(message2, innerException2);
					}
				}
			}
			if (getCmdletParameters.QueryOptions != null)
			{
				foreach (QueryOption queryOptionMetadata in getCmdletParameters.QueryOptions)
				{
					ParameterMetadata parameterMetadata3 = this.GenerateOptionClause(commonParameterSets, queryParameterSets, methodParameterSets, queryOptionMetadata, stringWriter);
					try
					{
						queryParameters.Add(parameterMetadata3.Name, parameterMetadata3);
					}
					catch (ArgumentException innerException3)
					{
						string message3 = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_DuplicateQueryParameterName, new object[]
						{
							"<GetCmdletParameters>",
							parameterMetadata3.Name
						});
						throw new XmlException(message3, innerException3);
					}
				}
			}
			if (instanceCmdlet != null)
			{
				ParameterMetadata parameterMetadata4 = new ParameterMetadata("InputObject", this.objectInstanceType.MakeArrayType());
				ParameterSetMetadata.ParameterFlags parameterFlags = ParameterSetMetadata.ParameterFlags.ValueFromPipeline;
				if (queryParameters.Count > 0)
				{
					parameterFlags |= ParameterSetMetadata.ParameterFlags.Mandatory;
				}
				string psTypeName;
				if (this.objectModelWrapper.FullName.Equals("Microsoft.PowerShell.Cmdletization.Cim.CimCmdletAdapter"))
				{
					int val = this.cmdletizationMetadata.Class.ClassName.LastIndexOf('\\');
					int val2 = this.cmdletizationMetadata.Class.ClassName.LastIndexOf('/');
					int num = Math.Max(val, val2);
					string text = this.cmdletizationMetadata.Class.ClassName.Substring(num + 1, this.cmdletizationMetadata.Class.ClassName.Length - num - 1);
					psTypeName = string.Format(CultureInfo.InvariantCulture, "{0}#{1}", new object[]
					{
						this.objectInstanceType.FullName,
						text
					});
				}
				else
				{
					psTypeName = string.Format(CultureInfo.InvariantCulture, "{0}#{1}", new object[]
					{
						this.objectInstanceType.FullName,
						this.cmdletizationMetadata.Class.ClassName
					});
				}
				parameterMetadata4.Attributes.Add(new PSTypeNameAttribute(psTypeName));
				parameterMetadata4.Attributes.Add(new ValidateNotNullAttribute());
				parameterMetadata4.ParameterSets.Clear();
				ParameterSetMetadata value = new ParameterSetMetadata(int.MinValue, parameterFlags, null);
				parameterMetadata4.ParameterSets.Add("InputObject (cdxml)", value);
				queryParameters.Add(parameterMetadata4.Name, parameterMetadata4);
			}
			stringWriter.WriteLine();
			scriptCode = stringWriter.ToString();
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x001EBA70 File Offset: 0x001E9C70
		private string GetHelpDirectiveForExternalHelp()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (ScriptWriter.GenerationOptions.HelpXml == (this.generationOptions & ScriptWriter.GenerationOptions.HelpXml))
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "# .EXTERNALHELP {0}.cdxml-Help.xml", new object[]
				{
					ScriptWriter.EscapeModuleNameForHelpComment(this.moduleName)
				});
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005B1A RID: 23322 RVA: 0x001EBABC File Offset: 0x001E9CBC
		private void WriteCmdlet(TextWriter output, StaticCmdletMetadata staticCmdlet)
		{
			string cmdletAttributes = this.GetCmdletAttributes(staticCmdlet.CmdletMetadata);
			Dictionary<string, ParameterMetadata> commonParameters = this.GetCommonParameters();
			List<string> commonParameterSets = ScriptWriter.GetCommonParameterSets(commonParameters);
			string arg;
			Dictionary<string, ParameterMetadata> dictionary;
			string text;
			this.GenerateMethodParametersProcessing(staticCmdlet, commonParameterSets, out arg, out dictionary, out text);
			List<string> methodParameterSets = this.GetMethodParameterSets(staticCmdlet);
			CommandMetadata commandMetadata = this.GetCommandMetadata(staticCmdlet.CmdletMetadata);
			if (!string.IsNullOrEmpty(commandMetadata.DefaultParameterSetName))
			{
				commandMetadata.DefaultParameterSetName = string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
				{
					commandMetadata.DefaultParameterSetName,
					commonParameterSets[0]
				});
			}
			ScriptWriter.MultiplyParameterSets(commonParameters, "{1}", new IEnumerable<string>[]
			{
				methodParameterSets
			});
			ScriptWriter.MultiplyParameterSets(dictionary, "{0}", new IEnumerable<string>[]
			{
				commonParameterSets
			});
			ScriptWriter.EnsureOrderOfPositionalParameters(commonParameters, dictionary);
			this.SetParameters(commandMetadata, new Dictionary<string, ParameterMetadata>[]
			{
				dictionary,
				commonParameters
			});
			output.WriteLine("\r\nfunction {0}\r\n{{\r\n    {1}\r\n    {2}\r\n    {3}\r\n    param(\r\n    {4})\r\n\r\n    DynamicParam {{\r\n        try \r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n                $__cmdletization_objectModelWrapper = $script:ObjectModelWrapper::new()\r\n                $__cmdletization_objectModelWrapper.Initialize($PSCmdlet, $script:ClassName, $script:ClassVersion, $script:ModuleVersion, $script:PrivateData)\r\n\r\n                if ($__cmdletization_objectModelWrapper -is [System.Management.Automation.IDynamicParameters])\r\n                {{\r\n                    ([System.Management.Automation.IDynamicParameters]$__cmdletization_objectModelWrapper).GetDynamicParameters()\r\n                }}\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n\r\n    Begin {{\r\n        $__cmdletization_exceptionHasBeenThrown = $false\r\n        try \r\n        {{\r\n            __cmdletization_BindCommonParameters $__cmdletization_objectModelWrapper $PSBoundParameters\r\n            $__cmdletization_objectModelWrapper.BeginProcessing()\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n        ", new object[]
			{
				commandMetadata.Name,
				ProxyCommand.GetCmdletBindingAttribute(commandMetadata),
				cmdletAttributes,
				text,
				ProxyCommand.GetParamBlock(commandMetadata)
			});
			output.WriteLine("\r\n    Process {{\r\n        try \r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n{0}\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n        ", arg);
			output.WriteLine("\r\n    End {{\r\n        try\r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n                $__cmdletization_objectModelWrapper.EndProcessing()\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            throw\r\n        }}\r\n    }}\r\n\r\n    {0}\r\n}}\r\nMicrosoft.PowerShell.Core\\Export-ModuleMember -Function '{1}' -Alias '*'\r\n        ", this.GetHelpDirectiveForExternalHelp(), CodeGeneration.EscapeSingleQuotedStringContent(commandMetadata.Name));
			this.functionsToExport.Add(commandMetadata.Name);
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x001EBC24 File Offset: 0x001E9E24
		private static void AddPassThruParameter(IDictionary<string, ParameterMetadata> commonParameters, InstanceCmdletMetadata instanceCmdletMetadata)
		{
			bool flag = false;
			if (instanceCmdletMetadata.Method.Parameters != null)
			{
				foreach (InstanceMethodParameterMetadata instanceMethodParameterMetadata in instanceCmdletMetadata.Method.Parameters)
				{
					if (instanceMethodParameterMetadata.CmdletOutputMetadata != null && instanceMethodParameterMetadata.CmdletOutputMetadata.ErrorCode == null)
					{
						flag = true;
						break;
					}
				}
			}
			if (instanceCmdletMetadata.Method.ReturnValue != null && instanceCmdletMetadata.Method.ReturnValue.CmdletOutputMetadata != null && instanceCmdletMetadata.Method.ReturnValue.CmdletOutputMetadata.ErrorCode == null)
			{
				flag = true;
			}
			if (!flag)
			{
				ParameterMetadata parameterMetadata = new ParameterMetadata("PassThru", typeof(SwitchParameter));
				parameterMetadata.ParameterSets.Clear();
				ParameterSetMetadata value = new ParameterSetMetadata(int.MinValue, (ParameterSetMetadata.ParameterFlags)0U, null);
				parameterMetadata.ParameterSets.Add("__AllParameterSets", value);
				commonParameters.Add(parameterMetadata.Name, parameterMetadata);
			}
		}

		// Token: 0x06005B1C RID: 23324 RVA: 0x001EBD08 File Offset: 0x001E9F08
		private void WriteCmdlet(TextWriter output, InstanceCmdletMetadata instanceCmdlet)
		{
			string cmdletAttributes = this.GetCmdletAttributes(instanceCmdlet.CmdletMetadata);
			Dictionary<string, ParameterMetadata> commonParameters = this.GetCommonParameters();
			List<string> commonParameterSets = ScriptWriter.GetCommonParameterSets(commonParameters);
			List<string> methodParameterSets = this.GetMethodParameterSets(instanceCmdlet);
			List<string> queryParameterSets = this.GetQueryParameterSets(instanceCmdlet);
			string str;
			Dictionary<string, ParameterMetadata> dictionary;
			this.GenerateQueryParametersProcessing(instanceCmdlet, commonParameterSets, queryParameterSets, methodParameterSets, out str, out dictionary);
			string str2;
			Dictionary<string, ParameterMetadata> dictionary2;
			string text;
			this.GenerateMethodParametersProcessing(instanceCmdlet, commonParameterSets, queryParameterSets, out str2, out dictionary2, out text);
			CommandMetadata commandMetadata = this.GetCommandMetadata(instanceCmdlet.CmdletMetadata);
			GetCmdletParameters getCmdletParameters = this.GetGetCmdletParameters(instanceCmdlet);
			if (!string.IsNullOrEmpty(getCmdletParameters.DefaultCmdletParameterSet))
			{
				commandMetadata.DefaultParameterSetName = getCmdletParameters.DefaultCmdletParameterSet;
			}
			else if (queryParameterSets.Count == 1)
			{
				commandMetadata.DefaultParameterSetName = queryParameterSets.Single<string>();
			}
			ScriptWriter.AddPassThruParameter(commonParameters, instanceCmdlet);
			ScriptWriter.MultiplyParameterSets(commonParameters, "{1}", new IEnumerable<string>[]
			{
				queryParameterSets,
				methodParameterSets
			});
			ScriptWriter.MultiplyParameterSets(dictionary, "{0}", new IEnumerable<string>[]
			{
				commonParameterSets,
				methodParameterSets
			});
			ScriptWriter.MultiplyParameterSets(dictionary2, "{2}", new IEnumerable<string>[]
			{
				commonParameterSets,
				queryParameterSets
			});
			ScriptWriter.EnsureOrderOfPositionalParameters(commonParameters, dictionary);
			ScriptWriter.EnsureOrderOfPositionalParameters(dictionary, dictionary2);
			this.SetParameters(commandMetadata, new Dictionary<string, ParameterMetadata>[]
			{
				dictionary,
				dictionary2,
				commonParameters
			});
			output.WriteLine("\r\nfunction {0}\r\n{{\r\n    {1}\r\n    {2}\r\n    {3}\r\n    param(\r\n    {4})\r\n\r\n    DynamicParam {{\r\n        try \r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n                $__cmdletization_objectModelWrapper = $script:ObjectModelWrapper::new()\r\n                $__cmdletization_objectModelWrapper.Initialize($PSCmdlet, $script:ClassName, $script:ClassVersion, $script:ModuleVersion, $script:PrivateData)\r\n\r\n                if ($__cmdletization_objectModelWrapper -is [System.Management.Automation.IDynamicParameters])\r\n                {{\r\n                    ([System.Management.Automation.IDynamicParameters]$__cmdletization_objectModelWrapper).GetDynamicParameters()\r\n                }}\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n\r\n    Begin {{\r\n        $__cmdletization_exceptionHasBeenThrown = $false\r\n        try \r\n        {{\r\n            __cmdletization_BindCommonParameters $__cmdletization_objectModelWrapper $PSBoundParameters\r\n            $__cmdletization_objectModelWrapper.BeginProcessing()\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n        ", new object[]
			{
				commandMetadata.Name,
				ProxyCommand.GetCmdletBindingAttribute(commandMetadata),
				cmdletAttributes,
				text,
				ProxyCommand.GetParamBlock(commandMetadata)
			});
			output.WriteLine("\r\n    Process {{\r\n        try \r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n{0}\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n        ", str + "\r\n" + str2);
			output.WriteLine("\r\n    End {{\r\n        try\r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n                $__cmdletization_objectModelWrapper.EndProcessing()\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            throw\r\n        }}\r\n    }}\r\n\r\n    {0}\r\n}}\r\nMicrosoft.PowerShell.Core\\Export-ModuleMember -Function '{1}' -Alias '*'\r\n        ", this.GetHelpDirectiveForExternalHelp(), CodeGeneration.EscapeSingleQuotedStringContent(commandMetadata.Name));
			this.functionsToExport.Add(commandMetadata.Name);
		}

		// Token: 0x06005B1D RID: 23325 RVA: 0x001EBED4 File Offset: 0x001EA0D4
		private string GetOutputAttributeForGetCmdlet()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "[OutputType([{0}])]", new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(this.objectInstanceType.FullName)
			});
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "[OutputType('{0}#{1}')]", new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(this.objectInstanceType.FullName),
				CodeGeneration.EscapeSingleQuotedStringContent(this.cmdletizationMetadata.Class.ClassName)
			});
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x06005B1E RID: 23326 RVA: 0x001EBF6C File Offset: 0x001EA16C
		private CommonCmdletMetadata GetGetCmdletMetadata()
		{
			CommonCmdletMetadata commonCmdletMetadata;
			if (this.cmdletizationMetadata.Class.InstanceCmdlets.GetCmdlet != null)
			{
				commonCmdletMetadata = this.cmdletizationMetadata.Class.InstanceCmdlets.GetCmdlet.CmdletMetadata;
			}
			else
			{
				commonCmdletMetadata = new CommonCmdletMetadata();
				commonCmdletMetadata.Noun = this.cmdletizationMetadata.Class.DefaultNoun;
				commonCmdletMetadata.Verb = "Get";
			}
			return commonCmdletMetadata;
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x001EBFD8 File Offset: 0x001EA1D8
		private void WriteGetCmdlet(TextWriter output)
		{
			Dictionary<string, ParameterMetadata> commonParameters = this.GetCommonParameters();
			List<string> commonParameterSets = ScriptWriter.GetCommonParameterSets(commonParameters);
			List<string> list = new List<string>();
			list.Add(string.Empty);
			List<string> queryParameterSets = this.GetQueryParameterSets(null);
			string str;
			Dictionary<string, ParameterMetadata> dictionary;
			this.GenerateQueryParametersProcessing(null, commonParameterSets, queryParameterSets, list, out str, out dictionary);
			CommonCmdletMetadata getCmdletMetadata = this.GetGetCmdletMetadata();
			CommandMetadata commandMetadata = this.GetCommandMetadata(getCmdletMetadata);
			string cmdletAttributes = this.GetCmdletAttributes(getCmdletMetadata);
			GetCmdletParameters getCmdletParameters = this.GetGetCmdletParameters(null);
			if (!string.IsNullOrEmpty(getCmdletParameters.DefaultCmdletParameterSet))
			{
				commandMetadata.DefaultParameterSetName = getCmdletParameters.DefaultCmdletParameterSet;
			}
			ScriptWriter.MultiplyParameterSets(commonParameters, "{1}", new IEnumerable<string>[]
			{
				queryParameterSets,
				list
			});
			ScriptWriter.MultiplyParameterSets(dictionary, "{0}", new IEnumerable<string>[]
			{
				commonParameterSets,
				list
			});
			ScriptWriter.EnsureOrderOfPositionalParameters(commonParameters, dictionary);
			this.SetParameters(commandMetadata, new Dictionary<string, ParameterMetadata>[]
			{
				dictionary,
				commonParameters
			});
			output.WriteLine("\r\nfunction {0}\r\n{{\r\n    {1}\r\n    {2}\r\n    {3}\r\n    param(\r\n    {4})\r\n\r\n    DynamicParam {{\r\n        try \r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n                $__cmdletization_objectModelWrapper = $script:ObjectModelWrapper::new()\r\n                $__cmdletization_objectModelWrapper.Initialize($PSCmdlet, $script:ClassName, $script:ClassVersion, $script:ModuleVersion, $script:PrivateData)\r\n\r\n                if ($__cmdletization_objectModelWrapper -is [System.Management.Automation.IDynamicParameters])\r\n                {{\r\n                    ([System.Management.Automation.IDynamicParameters]$__cmdletization_objectModelWrapper).GetDynamicParameters()\r\n                }}\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n\r\n    Begin {{\r\n        $__cmdletization_exceptionHasBeenThrown = $false\r\n        try \r\n        {{\r\n            __cmdletization_BindCommonParameters $__cmdletization_objectModelWrapper $PSBoundParameters\r\n            $__cmdletization_objectModelWrapper.BeginProcessing()\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n        ", new object[]
			{
				commandMetadata.Name,
				ProxyCommand.GetCmdletBindingAttribute(commandMetadata),
				cmdletAttributes,
				this.GetOutputAttributeForGetCmdlet(),
				ProxyCommand.GetParamBlock(commandMetadata)
			});
			output.WriteLine("\r\n    Process {{\r\n        try \r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n{0}\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n        ", str + "\r\n    $__cmdletization_objectModelWrapper.ProcessRecord($__cmdletization_queryBuilder)");
			output.WriteLine("\r\n    End {{\r\n        try\r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n                $__cmdletization_objectModelWrapper.EndProcessing()\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            throw\r\n        }}\r\n    }}\r\n\r\n    {0}\r\n}}\r\nMicrosoft.PowerShell.Core\\Export-ModuleMember -Function '{1}' -Alias '*'\r\n        ", this.GetHelpDirectiveForExternalHelp(), CodeGeneration.EscapeSingleQuotedStringContent(commandMetadata.Name));
			this.functionsToExport.Add(commandMetadata.Name);
		}

		// Token: 0x06005B20 RID: 23328 RVA: 0x001EC14C File Offset: 0x001EA34C
		private static void CompileEnum(EnumMetadataEnum enumMetadata)
		{
			try
			{
				string enumFullName = EnumWriter.GetEnumFullName(enumMetadata);
				lock (ScriptWriter._enumCompilationLock)
				{
					Type type;
					if (!LanguagePrimitives.TryConvertTo<Type>(enumFullName, CultureInfo.InvariantCulture, out type))
					{
						EnumWriter.Compile(enumMetadata);
					}
				}
			}
			catch (Exception ex)
			{
				string message = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ScriptWriter_InvalidEnum, new object[]
				{
					enumMetadata.EnumName,
					ex.Message
				});
				throw new XmlException(message, ex);
			}
		}

		// Token: 0x06005B21 RID: 23329 RVA: 0x001EC1EC File Offset: 0x001EA3EC
		internal void WriteScriptModule(TextWriter output)
		{
			this.WriteModulePreamble(output);
			this.WriteBindCommonParametersFunction(output);
			if (this.cmdletizationMetadata.Enums != null)
			{
				foreach (EnumMetadataEnum enumMetadata in this.cmdletizationMetadata.Enums)
				{
					ScriptWriter.CompileEnum(enumMetadata);
				}
			}
			if (this.cmdletizationMetadata.Class.StaticCmdlets != null)
			{
				foreach (StaticCmdletMetadata staticCmdlet in this.cmdletizationMetadata.Class.StaticCmdlets)
				{
					this.WriteCmdlet(output, staticCmdlet);
				}
			}
			if (this.cmdletizationMetadata.Class.InstanceCmdlets != null)
			{
				this.WriteGetCmdlet(output);
				if (this.cmdletizationMetadata.Class.InstanceCmdlets.Cmdlet != null)
				{
					foreach (InstanceCmdletMetadata instanceCmdlet in this.cmdletizationMetadata.Class.InstanceCmdlets.Cmdlet)
					{
						this.WriteCmdlet(output, instanceCmdlet);
					}
				}
			}
		}

		// Token: 0x06005B22 RID: 23330 RVA: 0x001EC2EC File Offset: 0x001EA4EC
		internal void PopulatePSModuleInfo(PSModuleInfo moduleInfo)
		{
			moduleInfo.SetModuleType(ModuleType.Cim);
			moduleInfo.SetVersion(new Version(this.cmdletizationMetadata.Class.Version));
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			hashtable.Add("ClassName", this.cmdletizationMetadata.Class.ClassName);
			hashtable.Add("CmdletAdapter", this.objectModelWrapper);
			moduleInfo.PrivateData = new Hashtable(StringComparer.OrdinalIgnoreCase)
			{
				{
					"CmdletsOverObjects",
					hashtable
				}
			};
		}

		// Token: 0x06005B23 RID: 23331 RVA: 0x001EC380 File Offset: 0x001EA580
		internal void ReportExportedCommands(PSModuleInfo moduleInfo, string prefix)
		{
			if (moduleInfo.ExportedCommands.Count != 0)
			{
				return;
			}
			moduleInfo.DeclaredAliasExports = new Collection<string>();
			moduleInfo.DeclaredFunctionExports = new Collection<string>();
			IEnumerable<CommonCmdletMetadata> enumerable = Enumerable.Empty<CommonCmdletMetadata>();
			if (this.cmdletizationMetadata.Class.InstanceCmdlets != null)
			{
				enumerable = enumerable.Append(this.GetGetCmdletMetadata());
				if (this.cmdletizationMetadata.Class.InstanceCmdlets.Cmdlet != null)
				{
					enumerable = enumerable.Concat(from c in this.cmdletizationMetadata.Class.InstanceCmdlets.Cmdlet
					select c.CmdletMetadata);
				}
			}
			if (this.cmdletizationMetadata.Class.StaticCmdlets != null)
			{
				enumerable = enumerable.Concat(from c in this.cmdletizationMetadata.Class.StaticCmdlets
				select c.CmdletMetadata);
			}
			foreach (CommonCmdletMetadata commonCmdletMetadata in enumerable)
			{
				if (commonCmdletMetadata.Aliases != null)
				{
					foreach (string commandName in commonCmdletMetadata.Aliases)
					{
						moduleInfo.DeclaredAliasExports.Add(ModuleCmdletBase.AddPrefixToCommandName(commandName, prefix));
					}
				}
				CommandMetadata commandMetadata = this.GetCommandMetadata(commonCmdletMetadata);
				moduleInfo.DeclaredFunctionExports.Add(ModuleCmdletBase.AddPrefixToCommandName(commandMetadata.Name, prefix));
			}
		}

		// Token: 0x040030B3 RID: 12467
		private const string HeaderTemplate = "\r\n#requires -version 3.0\r\n\r\ntry {{ Microsoft.PowerShell.Core\\Set-StrictMode -Off }} catch {{ }}\r\n\r\n$script:MyModule = $MyInvocation.MyCommand.ScriptBlock.Module\r\n\r\n$script:ClassName = '{0}'\r\n$script:ClassVersion = '{1}'\r\n$script:ModuleVersion = '{2}'\r\n$script:ObjectModelWrapper = [{3}]\r\n\r\n$script:PrivateData = [System.Collections.Generic.Dictionary[string,string]]::new()\r\n\r\nMicrosoft.PowerShell.Core\\Export-ModuleMember -Function @()\r\n        ";

		// Token: 0x040030B4 RID: 12468
		private const string StaticCommonParameterSetTemplate = "{1}";

		// Token: 0x040030B5 RID: 12469
		private const string StaticMethodParameterSetTemplate = "{0}";

		// Token: 0x040030B6 RID: 12470
		private const string InstanceCommonParameterSetTemplate = "{1}";

		// Token: 0x040030B7 RID: 12471
		private const string InstanceQueryParameterSetTemplate = "{0}";

		// Token: 0x040030B8 RID: 12472
		private const string InstanceMethodParameterSetTemplate = "{2}";

		// Token: 0x040030B9 RID: 12473
		private const string InputObjectQueryParameterSetName = "InputObject (cdxml)";

		// Token: 0x040030BA RID: 12474
		private const string SingleQueryParameterSetName = "Query (cdxml)";

		// Token: 0x040030BB RID: 12475
		private const string CmdletBeginBlockTemplate = "\r\nfunction {0}\r\n{{\r\n    {1}\r\n    {2}\r\n    {3}\r\n    param(\r\n    {4})\r\n\r\n    DynamicParam {{\r\n        try \r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n                $__cmdletization_objectModelWrapper = $script:ObjectModelWrapper::new()\r\n                $__cmdletization_objectModelWrapper.Initialize($PSCmdlet, $script:ClassName, $script:ClassVersion, $script:ModuleVersion, $script:PrivateData)\r\n\r\n                if ($__cmdletization_objectModelWrapper -is [System.Management.Automation.IDynamicParameters])\r\n                {{\r\n                    ([System.Management.Automation.IDynamicParameters]$__cmdletization_objectModelWrapper).GetDynamicParameters()\r\n                }}\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n\r\n    Begin {{\r\n        $__cmdletization_exceptionHasBeenThrown = $false\r\n        try \r\n        {{\r\n            __cmdletization_BindCommonParameters $__cmdletization_objectModelWrapper $PSBoundParameters\r\n            $__cmdletization_objectModelWrapper.BeginProcessing()\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n        ";

		// Token: 0x040030BC RID: 12476
		private const string CmdletProcessBlockTemplate = "\r\n    Process {{\r\n        try \r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n{0}\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            $__cmdletization_exceptionHasBeenThrown = $true\r\n            throw\r\n        }}\r\n    }}\r\n        ";

		// Token: 0x040030BD RID: 12477
		private const string CmdletEndBlockTemplate = "\r\n    End {{\r\n        try\r\n        {{\r\n            if (-not $__cmdletization_exceptionHasBeenThrown)\r\n            {{\r\n                $__cmdletization_objectModelWrapper.EndProcessing()\r\n            }}\r\n        }}\r\n        catch\r\n        {{\r\n            throw\r\n        }}\r\n    }}\r\n\r\n    {0}\r\n}}\r\nMicrosoft.PowerShell.Core\\Export-ModuleMember -Function '{1}' -Alias '*'\r\n        ";

		// Token: 0x040030BE RID: 12478
		internal const string PrivateDataKey_CmdletsOverObjects = "CmdletsOverObjects";

		// Token: 0x040030BF RID: 12479
		internal const string PrivateDataKey_ClassName = "ClassName";

		// Token: 0x040030C0 RID: 12480
		internal const string PrivateDataKey_ObjectModelWrapper = "CmdletAdapter";

		// Token: 0x040030C1 RID: 12481
		internal const string PrivateDataKey_DefaultSession = "DefaultSession";

		// Token: 0x040030C2 RID: 12482
		private static readonly XmlReaderSettings xmlReaderSettings;

		// Token: 0x040030C3 RID: 12483
		private readonly PowerShellMetadata cmdletizationMetadata;

		// Token: 0x040030C4 RID: 12484
		private readonly string moduleName;

		// Token: 0x040030C5 RID: 12485
		private readonly Type objectModelWrapper;

		// Token: 0x040030C6 RID: 12486
		private readonly Type objectInstanceType;

		// Token: 0x040030C7 RID: 12487
		private readonly InvocationInfo invocationInfo;

		// Token: 0x040030C8 RID: 12488
		private readonly ScriptWriter.GenerationOptions generationOptions;

		// Token: 0x040030C9 RID: 12489
		private readonly List<string> aliasesToExport = new List<string>();

		// Token: 0x040030CA RID: 12490
		private readonly List<string> functionsToExport = new List<string>();

		// Token: 0x040030CB RID: 12491
		private Dictionary<CommonMethodMetadata, int> _staticMethodMetadataToUniqueId = new Dictionary<CommonMethodMetadata, int>();

		// Token: 0x040030CC RID: 12492
		private static object _enumCompilationLock = new object();

		// Token: 0x020009A7 RID: 2471
		[Flags]
		internal enum GenerationOptions
		{
			// Token: 0x040030D3 RID: 12499
			TypesPs1Xml = 1,
			// Token: 0x040030D4 RID: 12500
			FormatPs1Xml = 2,
			// Token: 0x040030D5 RID: 12501
			HelpXml = 4
		}
	}
}
