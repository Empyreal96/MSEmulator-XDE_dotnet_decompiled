using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004A6 RID: 1190
	internal abstract class ViewGenerator
	{
		// Token: 0x06003518 RID: 13592 RVA: 0x0012036C File Offset: 0x0011E56C
		internal virtual void Initialize(TerminatingErrorContext terminatingErrorContext, MshExpressionFactory mshExpressionFactory, TypeInfoDataBase db, ViewDefinition view, FormattingCommandLineParameters formatParameters)
		{
			this.errorContext = terminatingErrorContext;
			this.expressionFactory = mshExpressionFactory;
			this.parameters = formatParameters;
			this.dataBaseInfo.db = db;
			this.dataBaseInfo.view = view;
			this.dataBaseInfo.applicableTypes = DisplayDataQuery.GetAllApplicableTypes(db, view.appliesTo);
			this.InitializeHelper();
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x001203C6 File Offset: 0x0011E5C6
		internal virtual void Initialize(TerminatingErrorContext terminatingErrorContext, MshExpressionFactory mshExpressionFactory, PSObject so, TypeInfoDataBase db, FormattingCommandLineParameters formatParameters)
		{
			this.errorContext = terminatingErrorContext;
			this.expressionFactory = mshExpressionFactory;
			this.parameters = formatParameters;
			this.dataBaseInfo.db = db;
			this.InitializeHelper();
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x001203F1 File Offset: 0x0011E5F1
		internal virtual void PrepareForRemoteObjects(PSObject so)
		{
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x001203F3 File Offset: 0x0011E5F3
		private void InitializeHelper()
		{
			this.InitializeFormatErrorManager();
			this.InitializeGroupBy();
			this.InitializeAutoSize();
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x00120408 File Offset: 0x0011E608
		private void InitializeFormatErrorManager()
		{
			FormatErrorPolicy formatErrorPolicy = new FormatErrorPolicy();
			if (this.parameters != null && this.parameters.showErrorsAsMessages != null)
			{
				formatErrorPolicy.ShowErrorsAsMessages = this.parameters.showErrorsAsMessages.Value;
			}
			else
			{
				formatErrorPolicy.ShowErrorsAsMessages = this.dataBaseInfo.db.defaultSettingsSection.formatErrorPolicy.ShowErrorsAsMessages;
			}
			if (this.parameters != null && this.parameters.showErrorsInFormattedOutput != null)
			{
				formatErrorPolicy.ShowErrorsInFormattedOutput = this.parameters.showErrorsInFormattedOutput.Value;
			}
			else
			{
				formatErrorPolicy.ShowErrorsInFormattedOutput = this.dataBaseInfo.db.defaultSettingsSection.formatErrorPolicy.ShowErrorsInFormattedOutput;
			}
			this.errorManager = new FormatErrorManager(formatErrorPolicy);
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x001204CC File Offset: 0x0011E6CC
		private void InitializeGroupBy()
		{
			if (this.parameters != null && this.parameters.groupByParameter != null)
			{
				MshExpression groupingExpression = this.parameters.groupByParameter.GetEntry("expression") as MshExpression;
				string displayLabel = null;
				object entry = this.parameters.groupByParameter.GetEntry("label");
				if (entry != AutomationNull.Value)
				{
					displayLabel = (entry as string);
				}
				this.groupingManager = new GroupingInfoManager();
				this.groupingManager.Initialize(groupingExpression, displayLabel);
				return;
			}
			if (this.dataBaseInfo.view != null)
			{
				GroupBy groupBy = this.dataBaseInfo.view.groupBy;
				if (groupBy == null)
				{
					return;
				}
				if (groupBy.startGroup == null || groupBy.startGroup.expression == null)
				{
					return;
				}
				MshExpression groupingExpression2 = this.expressionFactory.CreateFromExpressionToken(groupBy.startGroup.expression, this.dataBaseInfo.view.loadingInfo);
				this.groupingManager = new GroupingInfoManager();
				this.groupingManager.Initialize(groupingExpression2, null);
			}
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x001205C4 File Offset: 0x0011E7C4
		private void InitializeAutoSize()
		{
			if (this.parameters != null && this.parameters.autosize != null)
			{
				this.autosize = this.parameters.autosize.Value;
				return;
			}
			if (this.dataBaseInfo.view != null && this.dataBaseInfo.view.mainControl != null)
			{
				ControlBody controlBody = this.dataBaseInfo.view.mainControl as ControlBody;
				if (controlBody != null && controlBody.autosize != null)
				{
					this.autosize = controlBody.autosize.Value;
				}
			}
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x00120658 File Offset: 0x0011E858
		internal virtual FormatStartData GenerateStartData(PSObject so)
		{
			FormatStartData formatStartData = new FormatStartData();
			if (this.autosize)
			{
				formatStartData.autosizeInfo = new AutosizeInfo();
			}
			return formatStartData;
		}

		// Token: 0x06003520 RID: 13600
		internal abstract FormatEntryData GeneratePayload(PSObject so, int enumerationLimit);

		// Token: 0x06003521 RID: 13601 RVA: 0x00120680 File Offset: 0x0011E880
		internal GroupStartData GenerateGroupStartData(PSObject firstObjectInGroup, int enumerationLimit)
		{
			GroupStartData groupStartData = new GroupStartData();
			if (this.groupingManager == null)
			{
				return groupStartData;
			}
			object currentGroupingKeyPropertyValue = this.groupingManager.CurrentGroupingKeyPropertyValue;
			if (currentGroupingKeyPropertyValue == AutomationNull.Value)
			{
				return groupStartData;
			}
			PSObject so = PSObjectHelper.AsPSObject(currentGroupingKeyPropertyValue);
			ControlBase controlBase = null;
			TextToken textToken = null;
			if (this.dataBaseInfo.view != null && this.dataBaseInfo.view.groupBy != null && this.dataBaseInfo.view.groupBy.startGroup != null)
			{
				controlBase = this.dataBaseInfo.view.groupBy.startGroup.control;
				textToken = this.dataBaseInfo.view.groupBy.startGroup.labelTextToken;
			}
			groupStartData.groupingEntry = new GroupingEntry();
			if (controlBase == null)
			{
				StringFormatError stringFormatError = null;
				if (this.errorManager.DisplayFormatErrorString)
				{
					stringFormatError = new StringFormatError();
				}
				string propertyValue = PSObjectHelper.SmartToString(so, this.expressionFactory, enumerationLimit, stringFormatError);
				if (stringFormatError != null && stringFormatError.exception != null)
				{
					this.errorManager.LogStringFormatError(stringFormatError);
					if (this.errorManager.DisplayFormatErrorString)
					{
						propertyValue = this.errorManager.FormatErrorString;
					}
				}
				FormatEntry formatEntry = new FormatEntry();
				groupStartData.groupingEntry.formatValueList.Add(formatEntry);
				FormatTextField formatTextField = new FormatTextField();
				string o;
				if (textToken != null)
				{
					o = this.dataBaseInfo.db.displayResourceManagerCache.GetTextTokenString(textToken);
				}
				else
				{
					o = this.groupingManager.GroupingKeyDisplayName;
				}
				formatTextField.text = StringUtil.Format(FormatAndOut_format_xxx.GroupStartDataIndentedAutoGeneratedLabel, o);
				formatEntry.formatValueList.Add(formatTextField);
				FormatPropertyField formatPropertyField = new FormatPropertyField();
				formatPropertyField.propertyValue = propertyValue;
				formatEntry.formatValueList.Add(formatPropertyField);
			}
			else
			{
				ComplexControlGenerator complexControlGenerator = new ComplexControlGenerator(this.dataBaseInfo.db, this.dataBaseInfo.view.loadingInfo, this.expressionFactory, this.dataBaseInfo.view.formatControlDefinitionHolder.controlDefinitionList, this.ErrorManager, enumerationLimit, this.errorContext);
				complexControlGenerator.GenerateFormatEntries(50, controlBase, firstObjectInGroup, groupStartData.groupingEntry.formatValueList);
			}
			return groupStartData;
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x00120887 File Offset: 0x0011EA87
		internal bool UpdateGroupingKeyValue(PSObject so)
		{
			return this.groupingManager != null && this.groupingManager.UpdateGroupingKeyValue(so);
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x0012089F File Offset: 0x0011EA9F
		internal GroupEndData GenerateGroupEndData()
		{
			return new GroupEndData();
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x001208A8 File Offset: 0x0011EAA8
		internal bool IsObjectApplicable(Collection<string> typeNames)
		{
			if (this.dataBaseInfo.view == null)
			{
				return true;
			}
			if (typeNames.Count == 0)
			{
				return false;
			}
			TypeMatch typeMatch = new TypeMatch(this.expressionFactory, this.dataBaseInfo.db, typeNames);
			if (typeMatch.PerfectMatch(new TypeMatchItem(this, this.dataBaseInfo.applicableTypes)))
			{
				return true;
			}
			bool flag = typeMatch.BestMatch != null;
			if (!flag)
			{
				Collection<string> collection = Deserializer.MaskDeserializationPrefix(typeNames);
				if (collection != null)
				{
					flag = this.IsObjectApplicable(collection);
				}
			}
			return flag;
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06003525 RID: 13605 RVA: 0x00120924 File Offset: 0x0011EB24
		protected bool AutoSize
		{
			get
			{
				return this.autosize;
			}
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x0012092C File Offset: 0x0011EB2C
		protected string GetExpressionDisplayValue(PSObject so, int enumerationLimit, MshExpression ex, FieldFormattingDirective directive)
		{
			MshExpressionResult mshExpressionResult;
			return this.GetExpressionDisplayValue(so, enumerationLimit, ex, directive, out mshExpressionResult);
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x00120948 File Offset: 0x0011EB48
		protected string GetExpressionDisplayValue(PSObject so, int enumerationLimit, MshExpression ex, FieldFormattingDirective directive, out MshExpressionResult expressionResult)
		{
			StringFormatError stringFormatError = null;
			if (this.errorManager.DisplayFormatErrorString)
			{
				stringFormatError = new StringFormatError();
			}
			string result = PSObjectHelper.GetExpressionDisplayValue(so, enumerationLimit, ex, directive, stringFormatError, this.expressionFactory, out expressionResult);
			if (expressionResult != null)
			{
				if (expressionResult.Exception != null)
				{
					this.errorManager.LogMshExpressionFailedResult(expressionResult, so);
					if (this.errorManager.DisplayErrorStrings)
					{
						result = this.errorManager.ErrorString;
					}
				}
				else if (stringFormatError != null && stringFormatError.exception != null)
				{
					this.errorManager.LogStringFormatError(stringFormatError);
					if (this.errorManager.DisplayErrorStrings)
					{
						result = this.errorManager.FormatErrorString;
					}
				}
			}
			return result;
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x001209E8 File Offset: 0x0011EBE8
		protected bool EvaluateDisplayCondition(PSObject so, ExpressionToken conditionToken)
		{
			if (conditionToken == null)
			{
				return true;
			}
			MshExpression ex = this.expressionFactory.CreateFromExpressionToken(conditionToken, this.dataBaseInfo.view.loadingInfo);
			MshExpressionResult mshExpressionResult;
			bool result = DisplayCondition.Evaluate(so, ex, out mshExpressionResult);
			if (mshExpressionResult != null && mshExpressionResult.Exception != null)
			{
				this.errorManager.LogMshExpressionFailedResult(mshExpressionResult, so);
			}
			return result;
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06003529 RID: 13609 RVA: 0x00120A3A File Offset: 0x0011EC3A
		internal FormatErrorManager ErrorManager
		{
			get
			{
				return this.errorManager;
			}
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x00120A44 File Offset: 0x0011EC44
		protected FormatPropertyField GenerateFormatPropertyField(List<FormatToken> formatTokenList, PSObject so, int enumerationLimit)
		{
			MshExpressionResult mshExpressionResult;
			return this.GenerateFormatPropertyField(formatTokenList, so, enumerationLimit, out mshExpressionResult);
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x00120A5C File Offset: 0x0011EC5C
		protected FormatPropertyField GenerateFormatPropertyField(List<FormatToken> formatTokenList, PSObject so, int enumerationLimit, out MshExpressionResult result)
		{
			result = null;
			FormatPropertyField formatPropertyField = new FormatPropertyField();
			if (formatTokenList.Count != 0)
			{
				FormatToken formatToken = formatTokenList[0];
				FieldPropertyToken fieldPropertyToken = formatToken as FieldPropertyToken;
				if (fieldPropertyToken != null)
				{
					MshExpression ex = this.expressionFactory.CreateFromExpressionToken(fieldPropertyToken.expression, this.dataBaseInfo.view.loadingInfo);
					formatPropertyField.propertyValue = this.GetExpressionDisplayValue(so, enumerationLimit, ex, fieldPropertyToken.fieldFormattingDirective, out result);
				}
				else
				{
					TextToken textToken = formatToken as TextToken;
					if (textToken != null)
					{
						formatPropertyField.propertyValue = this.dataBaseInfo.db.displayResourceManagerCache.GetTextTokenString(textToken);
					}
				}
			}
			else
			{
				formatPropertyField.propertyValue = "";
			}
			return formatPropertyField;
		}

		// Token: 0x04001B27 RID: 6951
		private GroupingInfoManager groupingManager;

		// Token: 0x04001B28 RID: 6952
		private bool autosize;

		// Token: 0x04001B29 RID: 6953
		protected TerminatingErrorContext errorContext;

		// Token: 0x04001B2A RID: 6954
		protected FormattingCommandLineParameters parameters;

		// Token: 0x04001B2B RID: 6955
		protected MshExpressionFactory expressionFactory;

		// Token: 0x04001B2C RID: 6956
		protected ViewGenerator.DataBaseInfo dataBaseInfo = new ViewGenerator.DataBaseInfo();

		// Token: 0x04001B2D RID: 6957
		protected List<MshResolvedExpressionParameterAssociation> activeAssociationList;

		// Token: 0x04001B2E RID: 6958
		protected FormattingCommandLineParameters inputParameters;

		// Token: 0x04001B2F RID: 6959
		private FormatErrorManager errorManager;

		// Token: 0x020004A7 RID: 1191
		protected class DataBaseInfo
		{
			// Token: 0x04001B30 RID: 6960
			internal TypeInfoDataBase db;

			// Token: 0x04001B31 RID: 6961
			internal ViewDefinition view;

			// Token: 0x04001B32 RID: 6962
			internal AppliesTo applicableTypes;
		}
	}
}
