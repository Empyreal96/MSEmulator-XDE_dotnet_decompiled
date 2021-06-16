using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004A9 RID: 1193
	internal sealed class ComplexControlGenerator
	{
		// Token: 0x06003534 RID: 13620 RVA: 0x00120C4C File Offset: 0x0011EE4C
		internal ComplexControlGenerator(TypeInfoDataBase dataBase, DatabaseLoadingInfo loadingInfo, MshExpressionFactory expressionFactory, List<ControlDefinition> controlDefinitionList, FormatErrorManager resultErrorManager, int enumerationLimit, TerminatingErrorContext errorContext)
		{
			this.db = dataBase;
			this.loadingInfo = loadingInfo;
			this.expressionFactory = expressionFactory;
			this.controlDefinitionList = controlDefinitionList;
			this.errorManager = resultErrorManager;
			this.enumerationLimit = enumerationLimit;
			this.errorContext = errorContext;
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x00120C89 File Offset: 0x0011EE89
		internal void GenerateFormatEntries(int maxTreeDepth, ControlBase control, PSObject so, List<FormatValue> formatValueList)
		{
			if (control == null)
			{
				throw PSTraceSource.NewArgumentNullException("control");
			}
			this.ExecuteFormatControl(new TraversalInfo(0, maxTreeDepth), control, so, formatValueList);
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x00120CAC File Offset: 0x0011EEAC
		private bool ExecuteFormatControl(TraversalInfo level, ControlBase control, PSObject so, List<FormatValue> formatValueList)
		{
			ControlReference controlReference = control as ControlReference;
			ComplexControlBody complexControlBody;
			if (controlReference != null && controlReference.controlType == typeof(ComplexControlBody))
			{
				complexControlBody = (DisplayDataQuery.ResolveControlReference(this.db, this.controlDefinitionList, controlReference) as ComplexControlBody);
			}
			else
			{
				complexControlBody = (control as ComplexControlBody);
			}
			if (complexControlBody != null)
			{
				this.ExecuteFormatControlBody(level, so, complexControlBody, formatValueList);
				return true;
			}
			return false;
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x00120D10 File Offset: 0x0011EF10
		private void ExecuteFormatControlBody(TraversalInfo level, PSObject so, ComplexControlBody complexBody, List<FormatValue> formatValueList)
		{
			ComplexControlEntryDefinition activeComplexControlEntryDefinition = this.GetActiveComplexControlEntryDefinition(complexBody, so);
			this.ExecuteFormatTokenList(level, so, activeComplexControlEntryDefinition.itemDefinition.formatTokenList, formatValueList);
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x00120D3C File Offset: 0x0011EF3C
		private ComplexControlEntryDefinition GetActiveComplexControlEntryDefinition(ComplexControlBody complexBody, PSObject so)
		{
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			TypeMatch typeMatch = new TypeMatch(this.expressionFactory, this.db, internalTypeNames);
			foreach (ComplexControlEntryDefinition complexControlEntryDefinition in complexBody.optionalEntryList)
			{
				if (typeMatch.PerfectMatch(new TypeMatchItem(complexControlEntryDefinition, complexControlEntryDefinition.appliesTo)))
				{
					return complexControlEntryDefinition;
				}
			}
			if (typeMatch.BestMatch != null)
			{
				return typeMatch.BestMatch as ComplexControlEntryDefinition;
			}
			Collection<string> collection = Deserializer.MaskDeserializationPrefix(internalTypeNames);
			if (collection != null)
			{
				typeMatch = new TypeMatch(this.expressionFactory, this.db, collection);
				foreach (ComplexControlEntryDefinition complexControlEntryDefinition2 in complexBody.optionalEntryList)
				{
					if (typeMatch.PerfectMatch(new TypeMatchItem(complexControlEntryDefinition2, complexControlEntryDefinition2.appliesTo)))
					{
						return complexControlEntryDefinition2;
					}
				}
				if (typeMatch.BestMatch != null)
				{
					return typeMatch.BestMatch as ComplexControlEntryDefinition;
				}
			}
			return complexBody.defaultEntry;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x00120E68 File Offset: 0x0011F068
		private void ExecuteFormatTokenList(TraversalInfo level, PSObject so, List<FormatToken> formatTokenList, List<FormatValue> formatValueList)
		{
			if (so == null)
			{
				throw PSTraceSource.NewArgumentNullException("so");
			}
			if (level.Level == level.MaxDepth)
			{
				return;
			}
			FormatEntry formatEntry = new FormatEntry();
			formatValueList.Add(formatEntry);
			foreach (FormatToken formatToken in formatTokenList)
			{
				TextToken textToken = formatToken as TextToken;
				if (textToken != null)
				{
					FormatTextField formatTextField = new FormatTextField();
					formatTextField.text = this.db.displayResourceManagerCache.GetTextTokenString(textToken);
					formatEntry.formatValueList.Add(formatTextField);
				}
				else if (formatToken is NewLineToken)
				{
					formatEntry.formatValueList.Add(new FormatNewLine());
				}
				else
				{
					FrameToken frameToken = formatToken as FrameToken;
					if (frameToken != null)
					{
						FormatEntry formatEntry2 = new FormatEntry();
						formatEntry2.frameInfo = new FrameInfo();
						formatEntry2.frameInfo.firstLine = frameToken.frameInfoDefinition.firstLine;
						formatEntry2.frameInfo.leftIndentation = frameToken.frameInfoDefinition.leftIndentation;
						formatEntry2.frameInfo.rightIndentation = frameToken.frameInfoDefinition.rightIndentation;
						this.ExecuteFormatTokenList(level, so, frameToken.itemDefinition.formatTokenList, formatEntry2.formatValueList);
						formatEntry.formatValueList.Add(formatEntry2);
					}
					else
					{
						CompoundPropertyToken compoundPropertyToken = formatToken as CompoundPropertyToken;
						if (compoundPropertyToken != null && this.EvaluateDisplayCondition(so, compoundPropertyToken.conditionToken))
						{
							object obj = null;
							if (compoundPropertyToken.expression == null || string.IsNullOrEmpty(compoundPropertyToken.expression.expressionValue))
							{
								obj = so;
							}
							else
							{
								MshExpression mshExpression = this.expressionFactory.CreateFromExpressionToken(compoundPropertyToken.expression, this.loadingInfo);
								List<MshExpressionResult> values = mshExpression.GetValues(so);
								if (values.Count > 0)
								{
									obj = values[0].Result;
									if (values[0].Exception != null)
									{
										this.errorManager.LogMshExpressionFailedResult(values[0], so);
									}
								}
							}
							if (compoundPropertyToken.control == null || compoundPropertyToken.control is FieldControlBody)
							{
								if (obj == null)
								{
									obj = "";
								}
								FieldFormattingDirective fieldFormattingDirective = null;
								StringFormatError stringFormatError = null;
								if (compoundPropertyToken.control != null)
								{
									fieldFormattingDirective = ((FieldControlBody)compoundPropertyToken.control).fieldFormattingDirective;
									if (fieldFormattingDirective != null && this.errorManager.DisplayFormatErrorString)
									{
										stringFormatError = new StringFormatError();
									}
								}
								IEnumerable enumerable = PSObjectHelper.GetEnumerable(obj);
								FormatPropertyField formatPropertyField = new FormatPropertyField();
								if (compoundPropertyToken.enumerateCollection && enumerable != null)
								{
									using (IEnumerator enumerator2 = enumerable.GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											object obj2 = enumerator2.Current;
											if (obj2 != null)
											{
												formatPropertyField = new FormatPropertyField();
												formatPropertyField.propertyValue = PSObjectHelper.FormatField(fieldFormattingDirective, obj2, this.enumerationLimit, stringFormatError, this.expressionFactory);
												formatEntry.formatValueList.Add(formatPropertyField);
											}
										}
										goto IL_2EA;
									}
									goto IL_2B8;
								}
								goto IL_2B8;
								IL_2EA:
								if (stringFormatError != null && stringFormatError.exception != null)
								{
									this.errorManager.LogStringFormatError(stringFormatError);
									formatPropertyField.propertyValue = this.errorManager.FormatErrorString;
									continue;
								}
								continue;
								IL_2B8:
								formatPropertyField = new FormatPropertyField();
								formatPropertyField.propertyValue = PSObjectHelper.FormatField(fieldFormattingDirective, obj, this.enumerationLimit, stringFormatError, this.expressionFactory);
								formatEntry.formatValueList.Add(formatPropertyField);
								goto IL_2EA;
							}
							if (obj != null)
							{
								IEnumerable enumerable2 = PSObjectHelper.GetEnumerable(obj);
								if (compoundPropertyToken.enumerateCollection && enumerable2 != null)
								{
									using (IEnumerator enumerator3 = enumerable2.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											object obj3 = enumerator3.Current;
											if (obj3 != null)
											{
												this.ExecuteFormatControl(level.NextLevel, compoundPropertyToken.control, PSObject.AsPSObject(obj3), formatEntry.formatValueList);
											}
										}
										continue;
									}
								}
								this.ExecuteFormatControl(level.NextLevel, compoundPropertyToken.control, PSObjectHelper.AsPSObject(obj), formatEntry.formatValueList);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x00121298 File Offset: 0x0011F498
		private bool EvaluateDisplayCondition(PSObject so, ExpressionToken conditionToken)
		{
			if (conditionToken == null)
			{
				return true;
			}
			MshExpression ex = this.expressionFactory.CreateFromExpressionToken(conditionToken, this.loadingInfo);
			MshExpressionResult mshExpressionResult;
			bool result = DisplayCondition.Evaluate(so, ex, out mshExpressionResult);
			if (mshExpressionResult != null && mshExpressionResult.Exception != null)
			{
				this.errorManager.LogMshExpressionFailedResult(mshExpressionResult, so);
			}
			return result;
		}

		// Token: 0x04001B33 RID: 6963
		private TypeInfoDataBase db;

		// Token: 0x04001B34 RID: 6964
		private DatabaseLoadingInfo loadingInfo;

		// Token: 0x04001B35 RID: 6965
		private MshExpressionFactory expressionFactory;

		// Token: 0x04001B36 RID: 6966
		private List<ControlDefinition> controlDefinitionList;

		// Token: 0x04001B37 RID: 6967
		private FormatErrorManager errorManager;

		// Token: 0x04001B38 RID: 6968
		private TerminatingErrorContext errorContext;

		// Token: 0x04001B39 RID: 6969
		private int enumerationLimit;
	}
}
