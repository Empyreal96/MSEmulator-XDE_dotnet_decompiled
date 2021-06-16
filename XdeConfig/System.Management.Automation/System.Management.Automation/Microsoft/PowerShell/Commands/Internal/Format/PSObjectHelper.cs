using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200090E RID: 2318
	internal static class PSObjectHelper
	{
		// Token: 0x0600571B RID: 22299 RVA: 0x001C7795 File Offset: 0x001C5995
		internal static string PSObjectIsOfExactType(Collection<string> typeNames)
		{
			if (typeNames.Count != 0)
			{
				return typeNames[0];
			}
			return null;
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x001C77A8 File Offset: 0x001C59A8
		internal static bool PSObjectIsEnum(Collection<string> typeNames)
		{
			return typeNames.Count >= 2 && !string.IsNullOrEmpty(typeNames[1]) && string.Equals(typeNames[1], "System.Enum", StringComparison.Ordinal);
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x001C77D5 File Offset: 0x001C59D5
		internal static bool IsWriteErrorStream(PSObject so)
		{
			return PSObjectHelper.IsStreamType(so, "WriteErrorStream");
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x001C77E2 File Offset: 0x001C59E2
		internal static bool IsWriteWarningStream(PSObject so)
		{
			return PSObjectHelper.IsStreamType(so, "WriteWarningStream");
		}

		// Token: 0x0600571F RID: 22303 RVA: 0x001C77EF File Offset: 0x001C59EF
		internal static bool IsWriteVerboseStream(PSObject so)
		{
			return PSObjectHelper.IsStreamType(so, "WriteVerboseStream");
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x001C77FC File Offset: 0x001C59FC
		internal static bool IsWriteDebugStream(PSObject so)
		{
			return PSObjectHelper.IsStreamType(so, "WriteDebugStream");
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x001C7809 File Offset: 0x001C5A09
		internal static bool IsWriteInformationStream(PSObject so)
		{
			return PSObjectHelper.IsStreamType(so, "WriteInformationStream");
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x001C7818 File Offset: 0x001C5A18
		internal static bool IsStreamType(PSObject so, string streamFlag)
		{
			bool result;
			try
			{
				PSPropertyInfo pspropertyInfo = so.Properties[streamFlag];
				if (pspropertyInfo != null && pspropertyInfo.Value is bool)
				{
					result = (bool)pspropertyInfo.Value;
				}
				else
				{
					result = false;
				}
			}
			catch (ExtendedTypeSystemException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x001C786C File Offset: 0x001C5A6C
		internal static MshExpression GetDisplayNameExpression(PSObject target, MshExpressionFactory expressionFactory)
		{
			MshExpression defaultNameExpression = PSObjectHelper.GetDefaultNameExpression(target);
			if (defaultNameExpression != null)
			{
				return defaultNameExpression;
			}
			string[] array = new string[]
			{
				"name",
				"id",
				"key",
				"*key",
				"*name",
				"*id"
			};
			foreach (string s in array)
			{
				MshExpression mshExpression = new MshExpression(s);
				List<MshExpression> list = mshExpression.ResolveNames(target);
				while (list.Count > 0 && (list[0].ToString().Equals(RemotingConstants.ComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase) || list[0].ToString().Equals(RemotingConstants.ShowComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase) || list[0].ToString().Equals(RemotingConstants.RunspaceIdNoteProperty, StringComparison.OrdinalIgnoreCase) || list[0].ToString().Equals(RemotingConstants.SourceJobInstanceId, StringComparison.OrdinalIgnoreCase)))
				{
					list.RemoveAt(0);
				}
				if (list.Count != 0)
				{
					return list[0];
				}
			}
			return null;
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x001C7990 File Offset: 0x001C5B90
		internal static MshExpressionResult GetDisplayName(PSObject target, MshExpressionFactory expressionFactory)
		{
			MshExpression displayNameExpression = PSObjectHelper.GetDisplayNameExpression(target, expressionFactory);
			if (displayNameExpression == null)
			{
				return null;
			}
			List<MshExpressionResult> values = displayNameExpression.GetValues(target);
			if (values.Count == 0 || values[0].Exception != null)
			{
				return null;
			}
			return values[0];
		}

		// Token: 0x06005725 RID: 22309 RVA: 0x001C79D4 File Offset: 0x001C5BD4
		internal static IEnumerable GetEnumerable(object obj)
		{
			PSObject psobject = obj as PSObject;
			if (psobject != null)
			{
				obj = psobject.BaseObject;
			}
			if (obj is IDictionary)
			{
				return (IEnumerable)obj;
			}
			return LanguagePrimitives.GetEnumerable(obj);
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x001C7A08 File Offset: 0x001C5C08
		private static string GetSmartToStringDisplayName(object x, MshExpressionFactory expressionFactory)
		{
			MshExpressionResult displayName = PSObjectHelper.GetDisplayName(PSObjectHelper.AsPSObject(x), expressionFactory);
			if (displayName != null && displayName.Exception == null)
			{
				return PSObjectHelper.AsPSObject(displayName.Result).ToString();
			}
			return PSObjectHelper.AsPSObject(x).ToString();
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x001C7A4C File Offset: 0x001C5C4C
		private static string GetObjectName(object x, MshExpressionFactory expressionFactory)
		{
			string text;
			if (x is PSObject && (LanguagePrimitives.IsBoolOrSwitchParameterType(((PSObject)x).BaseObject.GetType()) || LanguagePrimitives.IsNumeric(((PSObject)x).BaseObject.GetType().GetTypeCode()) || LanguagePrimitives.IsNull(x)))
			{
				text = x.ToString();
			}
			else if (x == null)
			{
				text = "$null";
			}
			else
			{
				MethodInfo method = x.GetType().GetMethod("ToString", PSTypeExtensions.EmptyTypes);
				if (method.DeclaringType == x.GetType())
				{
					text = PSObjectHelper.AsPSObject(x).ToString();
				}
				else
				{
					MshExpressionResult displayName = PSObjectHelper.GetDisplayName(PSObjectHelper.AsPSObject(x), expressionFactory);
					if (displayName != null && displayName.Exception == null)
					{
						text = PSObjectHelper.AsPSObject(displayName.Result).ToString();
					}
					else
					{
						text = PSObjectHelper.AsPSObject(x).ToString();
						if (text == string.Empty)
						{
							object obj = PSObject.Base(x);
							if (obj != null)
							{
								text = obj.ToString();
							}
						}
					}
				}
			}
			return text;
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x001C7B44 File Offset: 0x001C5D44
		internal static string SmartToString(PSObject so, MshExpressionFactory expressionFactory, int enumerationLimit, StringFormatError formatErrorObject)
		{
			if (so == null)
			{
				return "";
			}
			string result;
			try
			{
				IEnumerable enumerable = PSObjectHelper.GetEnumerable(so);
				if (enumerable != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("{");
					bool flag = true;
					int num = 0;
					IEnumerator enumerator = enumerable.GetEnumerator();
					if (enumerator != null)
					{
						IBlockingEnumerator<object> blockingEnumerator = enumerator as IBlockingEnumerator<object>;
						if (blockingEnumerator != null)
						{
							while (blockingEnumerator.MoveNext(false))
							{
								if (LocalPipeline.GetExecutionContextFromTLS().CurrentPipelineStopping)
								{
									throw new PipelineStoppedException();
								}
								if (enumerationLimit >= 0)
								{
									if (num == enumerationLimit)
									{
										stringBuilder.Append("...");
										break;
									}
									num++;
								}
								if (!flag)
								{
									stringBuilder.Append(", ");
								}
								stringBuilder.Append(PSObjectHelper.GetObjectName(blockingEnumerator.Current, expressionFactory));
								if (flag)
								{
									flag = false;
								}
							}
						}
						else
						{
							foreach (object x in enumerable)
							{
								if (LocalPipeline.GetExecutionContextFromTLS().CurrentPipelineStopping)
								{
									throw new PipelineStoppedException();
								}
								if (enumerationLimit >= 0)
								{
									if (num == enumerationLimit)
									{
										stringBuilder.Append("...");
										break;
									}
									num++;
								}
								if (!flag)
								{
									stringBuilder.Append(", ");
								}
								stringBuilder.Append(PSObjectHelper.GetObjectName(x, expressionFactory));
								if (flag)
								{
									flag = false;
								}
							}
						}
					}
					stringBuilder.Append("}");
					result = stringBuilder.ToString();
				}
				else
				{
					result = so.ToString();
				}
			}
			catch (ExtendedTypeSystemException exception)
			{
				if (formatErrorObject != null)
				{
					formatErrorObject.sourceObject = so;
					formatErrorObject.exception = exception;
				}
				result = "";
			}
			return result;
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x001C7CF8 File Offset: 0x001C5EF8
		internal static PSObject AsPSObject(object obj)
		{
			if (obj != null)
			{
				return PSObject.AsPSObject(obj);
			}
			return PSObjectHelper.emptyPSObject;
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x001C7D0C File Offset: 0x001C5F0C
		internal static string FormatField(FieldFormattingDirective directive, object val, int enumerationLimit, StringFormatError formatErrorObject, MshExpressionFactory expressionFactory)
		{
			PSObject psobject = PSObjectHelper.AsPSObject(val);
			if (directive != null && !string.IsNullOrEmpty(directive.formatString))
			{
				try
				{
					if (directive.formatString.Contains("{0") || directive.formatString.Contains("}"))
					{
						return string.Format(CultureInfo.CurrentCulture, directive.formatString, new object[]
						{
							psobject
						});
					}
					return psobject.ToString(directive.formatString, null);
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					if (formatErrorObject != null)
					{
						formatErrorObject.sourceObject = psobject;
						formatErrorObject.exception = ex;
						formatErrorObject.formatString = directive.formatString;
						return "";
					}
				}
			}
			return PSObjectHelper.SmartToString(psobject, expressionFactory, enumerationLimit, formatErrorObject);
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x001C7DD4 File Offset: 0x001C5FD4
		private static PSMemberSet MaskDeserializedAndGetStandardMembers(PSObject so)
		{
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			Collection<string> collection = Deserializer.MaskDeserializationPrefix(internalTypeNames);
			if (collection == null)
			{
				return null;
			}
			TypeTable typeTable = so.GetTypeTable();
			if (typeTable == null)
			{
				return null;
			}
			PSMemberInfoInternalCollection<PSMemberInfo> members = typeTable.GetMembers<PSMemberInfo>(new ConsolidatedString(collection));
			return members["PSStandardMembers"] as PSMemberSet;
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x001C7E20 File Offset: 0x001C6020
		private static List<MshExpression> GetDefaultPropertySet(PSMemberSet standardMembersSet)
		{
			if (standardMembersSet != null)
			{
				PSPropertySet pspropertySet = standardMembersSet.Members["DefaultDisplayPropertySet"] as PSPropertySet;
				if (pspropertySet != null)
				{
					List<MshExpression> list = new List<MshExpression>();
					foreach (string text in pspropertySet.ReferencedPropertyNames)
					{
						if (!string.IsNullOrEmpty(text))
						{
							list.Add(new MshExpression(text));
						}
					}
					return list;
				}
			}
			return new List<MshExpression>();
		}

		// Token: 0x0600572D RID: 22317 RVA: 0x001C7EA4 File Offset: 0x001C60A4
		internal static List<MshExpression> GetDefaultPropertySet(PSObject so)
		{
			List<MshExpression> defaultPropertySet = PSObjectHelper.GetDefaultPropertySet(so.PSStandardMembers);
			if (defaultPropertySet.Count == 0)
			{
				defaultPropertySet = PSObjectHelper.GetDefaultPropertySet(PSObjectHelper.MaskDeserializedAndGetStandardMembers(so));
			}
			return defaultPropertySet;
		}

		// Token: 0x0600572E RID: 22318 RVA: 0x001C7ED4 File Offset: 0x001C60D4
		private static MshExpression GetDefaultNameExpression(PSMemberSet standardMembersSet)
		{
			if (standardMembersSet != null)
			{
				PSNoteProperty psnoteProperty = standardMembersSet.Members["DefaultDisplayProperty"] as PSNoteProperty;
				if (psnoteProperty != null)
				{
					string text = psnoteProperty.Value.ToString();
					if (string.IsNullOrEmpty(text))
					{
						return null;
					}
					return new MshExpression(text);
				}
			}
			return null;
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x001C7F1C File Offset: 0x001C611C
		private static MshExpression GetDefaultNameExpression(PSObject so)
		{
			MshExpression defaultNameExpression = PSObjectHelper.GetDefaultNameExpression(so.PSStandardMembers);
			if (defaultNameExpression == null)
			{
				defaultNameExpression = PSObjectHelper.GetDefaultNameExpression(PSObjectHelper.MaskDeserializedAndGetStandardMembers(so));
			}
			return defaultNameExpression;
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x001C7F48 File Offset: 0x001C6148
		internal static string GetExpressionDisplayValue(PSObject so, int enumerationLimit, MshExpression ex, FieldFormattingDirective directive, StringFormatError formatErrorObject, MshExpressionFactory expressionFactory, out MshExpressionResult result)
		{
			result = null;
			List<MshExpressionResult> values = ex.GetValues(so);
			if (values.Count == 0)
			{
				return "";
			}
			result = values[0];
			if (result.Exception != null)
			{
				return "";
			}
			return PSObjectHelper.FormatField(directive, result.Result, enumerationLimit, formatErrorObject, expressionFactory);
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x001C7F9C File Offset: 0x001C619C
		internal static bool ShouldShowComputerNameProperty(PSObject so)
		{
			bool result = false;
			if (so != null)
			{
				try
				{
					PSPropertyInfo pspropertyInfo = so.Properties[RemotingConstants.ComputerNameNoteProperty];
					PSPropertyInfo pspropertyInfo2 = so.Properties[RemotingConstants.ShowComputerNameNoteProperty];
					if (pspropertyInfo != null && pspropertyInfo2 != null)
					{
						LanguagePrimitives.TryConvertTo<bool>(pspropertyInfo2.Value, out result);
					}
				}
				catch (ArgumentException)
				{
				}
				catch (ExtendedTypeSystemException)
				{
				}
			}
			return result;
		}

		// Token: 0x04002E6E RID: 11886
		internal const string ellipses = "...";

		// Token: 0x04002E6F RID: 11887
		private static readonly PSObject emptyPSObject = new PSObject("");
	}
}
