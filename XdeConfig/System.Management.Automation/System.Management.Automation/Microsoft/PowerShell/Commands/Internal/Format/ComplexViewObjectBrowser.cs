using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004AB RID: 1195
	internal sealed class ComplexViewObjectBrowser
	{
		// Token: 0x0600353F RID: 13631 RVA: 0x0012131B File Offset: 0x0011F51B
		internal ComplexViewObjectBrowser(FormatErrorManager resultErrorManager, MshExpressionFactory mshExpressionFactory, int enumerationLimit)
		{
			this.errorManager = resultErrorManager;
			this.expressionFactory = mshExpressionFactory;
			this.enumerationLimit = enumerationLimit;
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x00121340 File Offset: 0x0011F540
		internal ComplexViewEntry GenerateView(PSObject so, FormattingCommandLineParameters inputParameters)
		{
			this.complexSpecificParameters = (ComplexSpecificParameters)inputParameters.shapeParameters;
			int maxDepth = this.complexSpecificParameters.maxDepth;
			TraversalInfo traversalInfo = new TraversalInfo(0, maxDepth);
			List<MshParameter> parameterList = null;
			if (inputParameters != null)
			{
				parameterList = inputParameters.mshParameterList;
			}
			ComplexViewEntry complexViewEntry = new ComplexViewEntry();
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			if (ComplexViewObjectBrowser.TreatAsScalarType(internalTypeNames))
			{
				FormatEntry formatEntry = new FormatEntry();
				complexViewEntry.formatValueList.Add(formatEntry);
				this.DisplayRawObject(so, formatEntry.formatValueList);
			}
			else
			{
				IEnumerable enumerable = PSObjectHelper.GetEnumerable(so);
				if (enumerable != null)
				{
					FormatEntry formatEntry2 = new FormatEntry();
					complexViewEntry.formatValueList.Add(formatEntry2);
					this.DisplayEnumeration(enumerable, traversalInfo, formatEntry2.formatValueList);
				}
				else
				{
					this.DisplayObject(so, traversalInfo, parameterList, complexViewEntry.formatValueList);
				}
			}
			return complexViewEntry;
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x001213FC File Offset: 0x0011F5FC
		private void DisplayRawObject(PSObject so, List<FormatValue> formatValueList)
		{
			FormatPropertyField formatPropertyField = new FormatPropertyField();
			StringFormatError stringFormatError = null;
			if (this.errorManager.DisplayFormatErrorString)
			{
				stringFormatError = new StringFormatError();
			}
			formatPropertyField.propertyValue = PSObjectHelper.SmartToString(so, this.expressionFactory, this.enumerationLimit, stringFormatError);
			if (stringFormatError != null && stringFormatError.exception != null)
			{
				this.errorManager.LogStringFormatError(stringFormatError);
				if (this.errorManager.DisplayFormatErrorString)
				{
					formatPropertyField.propertyValue = this.errorManager.FormatErrorString;
				}
			}
			formatValueList.Add(formatPropertyField);
			formatValueList.Add(new FormatNewLine());
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x00121484 File Offset: 0x0011F684
		private void DisplayObject(PSObject so, TraversalInfo currentLevel, List<MshParameter> parameterList, List<FormatValue> formatValueList)
		{
			List<MshResolvedExpressionParameterAssociation> activeAssociationList = AssociationManager.SetupActiveProperties(parameterList, so, this.expressionFactory);
			FormatEntry formatEntry = new FormatEntry();
			formatValueList.Add(formatEntry);
			string text = this.GetObjectDisplayName(so);
			if (text != null)
			{
				text = "class " + text;
			}
			ComplexViewObjectBrowser.AddPrologue(formatEntry.formatValueList, "{", text);
			this.ProcessActiveAssociationList(so, currentLevel, activeAssociationList, this.AddIndentationLevel(formatEntry.formatValueList));
			ComplexViewObjectBrowser.AddEpilogue(formatEntry.formatValueList, "}");
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x001214FC File Offset: 0x0011F6FC
		private void ProcessActiveAssociationList(PSObject so, TraversalInfo currentLevel, List<MshResolvedExpressionParameterAssociation> activeAssociationList, List<FormatValue> formatValueList)
		{
			foreach (MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation in activeAssociationList)
			{
				formatValueList.Add(new FormatTextField
				{
					text = mshResolvedExpressionParameterAssociation.ResolvedExpression.ToString() + " = "
				});
				List<MshExpressionResult> values = mshResolvedExpressionParameterAssociation.ResolvedExpression.GetValues(so);
				object obj = null;
				if (values.Count >= 1)
				{
					MshExpressionResult mshExpressionResult = values[0];
					if (mshExpressionResult.Exception != null)
					{
						this.errorManager.LogMshExpressionFailedResult(mshExpressionResult, so);
						if (this.errorManager.DisplayErrorStrings)
						{
							obj = this.errorManager.ErrorString;
						}
						else
						{
							obj = "";
						}
					}
					else
					{
						obj = mshExpressionResult.Result;
					}
				}
				TraversalInfo traversalInfo = currentLevel;
				if (mshResolvedExpressionParameterAssociation.OriginatingParameter != null)
				{
					object entry = mshResolvedExpressionParameterAssociation.OriginatingParameter.GetEntry("depth");
					if (entry != AutomationNull.Value)
					{
						int maxDepth = (int)entry;
						traversalInfo = new TraversalInfo(currentLevel.Level, maxDepth);
					}
				}
				IEnumerable enumerable = null;
				if (obj != null || traversalInfo.Level >= traversalInfo.MaxDepth)
				{
					enumerable = PSObjectHelper.GetEnumerable(obj);
				}
				if (enumerable != null)
				{
					formatValueList.Add(new FormatNewLine());
					this.DisplayEnumeration(enumerable, traversalInfo.NextLevel, this.AddIndentationLevel(formatValueList));
				}
				else if (obj == null || ComplexViewObjectBrowser.TreatAsLeafNode(obj, traversalInfo))
				{
					this.DisplayLeaf(obj, formatValueList);
				}
				else
				{
					formatValueList.Add(new FormatNewLine());
					this.DisplayObject(PSObject.AsPSObject(obj), traversalInfo.NextLevel, null, this.AddIndentationLevel(formatValueList));
				}
			}
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x001216A8 File Offset: 0x0011F8A8
		private void DisplayEnumeration(IEnumerable e, TraversalInfo level, List<FormatValue> formatValueList)
		{
			ComplexViewObjectBrowser.AddPrologue(formatValueList, "[", null);
			this.DisplayEnumerationInner(e, level, this.AddIndentationLevel(formatValueList));
			ComplexViewObjectBrowser.AddEpilogue(formatValueList, "]");
			formatValueList.Add(new FormatNewLine());
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x001216DC File Offset: 0x0011F8DC
		private void DisplayEnumerationInner(IEnumerable e, TraversalInfo level, List<FormatValue> formatValueList)
		{
			int num = 0;
			foreach (object obj in e)
			{
				if (LocalPipeline.GetExecutionContextFromTLS().CurrentPipelineStopping)
				{
					throw new PipelineStoppedException();
				}
				if (this.enumerationLimit >= 0)
				{
					if (this.enumerationLimit == num)
					{
						this.DisplayLeaf("...", formatValueList);
						break;
					}
					num++;
				}
				if (ComplexViewObjectBrowser.TreatAsLeafNode(obj, level))
				{
					this.DisplayLeaf(obj, formatValueList);
				}
				else
				{
					IEnumerable enumerable = PSObjectHelper.GetEnumerable(obj);
					if (enumerable != null)
					{
						formatValueList.Add(new FormatNewLine());
						this.DisplayEnumeration(enumerable, level.NextLevel, this.AddIndentationLevel(formatValueList));
					}
					else
					{
						this.DisplayObject(PSObjectHelper.AsPSObject(obj), level.NextLevel, null, formatValueList);
					}
				}
			}
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x001217B8 File Offset: 0x0011F9B8
		private void DisplayLeaf(object val, List<FormatValue> formatValueList)
		{
			formatValueList.Add(new FormatPropertyField
			{
				propertyValue = PSObjectHelper.FormatField(null, PSObjectHelper.AsPSObject(val), this.enumerationLimit, null, this.expressionFactory)
			});
			formatValueList.Add(new FormatNewLine());
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x001217FC File Offset: 0x0011F9FC
		private static bool TreatAsLeafNode(object val, TraversalInfo level)
		{
			return level.Level >= level.MaxDepth || val == null || ComplexViewObjectBrowser.TreatAsScalarType(PSObject.GetTypeNames(val));
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x0012181C File Offset: 0x0011FA1C
		private static bool TreatAsScalarType(Collection<string> typeNames)
		{
			return DefaultScalarTypes.IsTypeInList(typeNames);
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x00121824 File Offset: 0x0011FA24
		private string GetObjectDisplayName(PSObject so)
		{
			if (this.complexSpecificParameters.classDisplay == ComplexSpecificParameters.ClassInfoDisplay.none)
			{
				return null;
			}
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			if (internalTypeNames.Count == 0)
			{
				return "PSObject";
			}
			if (this.complexSpecificParameters.classDisplay == ComplexSpecificParameters.ClassInfoDisplay.shortName)
			{
				string[] array = internalTypeNames[0].Split(new char[]
				{
					'.'
				});
				if (array.Length > 0)
				{
					return array[array.Length - 1];
				}
			}
			return internalTypeNames[0];
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x00121894 File Offset: 0x0011FA94
		private static void AddPrologue(List<FormatValue> formatValueList, string openTag, string label)
		{
			if (label != null)
			{
				formatValueList.Add(new FormatTextField
				{
					text = label
				});
				formatValueList.Add(new FormatNewLine());
			}
			formatValueList.Add(new FormatTextField
			{
				text = openTag
			});
			formatValueList.Add(new FormatNewLine());
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x001218E4 File Offset: 0x0011FAE4
		private static void AddEpilogue(List<FormatValue> formatValueList, string closeTag)
		{
			formatValueList.Add(new FormatTextField
			{
				text = closeTag
			});
			formatValueList.Add(new FormatNewLine());
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x00121910 File Offset: 0x0011FB10
		private List<FormatValue> AddIndentationLevel(List<FormatValue> formatValueList)
		{
			FormatEntry formatEntry = new FormatEntry();
			formatEntry.frameInfo = new FrameInfo();
			formatEntry.frameInfo.firstLine = 0;
			formatEntry.frameInfo.leftIndentation = this.indentationStep;
			formatEntry.frameInfo.rightIndentation = 0;
			formatValueList.Add(formatEntry);
			return formatEntry.formatValueList;
		}

		// Token: 0x04001B3C RID: 6972
		private ComplexSpecificParameters complexSpecificParameters;

		// Token: 0x04001B3D RID: 6973
		private int indentationStep = 2;

		// Token: 0x04001B3E RID: 6974
		private FormatErrorManager errorManager;

		// Token: 0x04001B3F RID: 6975
		private MshExpressionFactory expressionFactory;

		// Token: 0x04001B40 RID: 6976
		private int enumerationLimit;
	}
}
