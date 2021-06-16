using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004AD RID: 1197
	internal sealed class TableViewGenerator : ViewGenerator
	{
		// Token: 0x06003557 RID: 13655 RVA: 0x00121F06 File Offset: 0x00120106
		internal override void Initialize(TerminatingErrorContext terminatingErrorContext, MshExpressionFactory mshExpressionFactory, TypeInfoDataBase db, ViewDefinition view, FormattingCommandLineParameters formatParameters)
		{
			base.Initialize(terminatingErrorContext, mshExpressionFactory, db, view, formatParameters);
			if (this.dataBaseInfo != null && this.dataBaseInfo.view != null)
			{
				this.tableBody = (TableControlBody)this.dataBaseInfo.view.mainControl;
			}
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x00121F48 File Offset: 0x00120148
		internal override void Initialize(TerminatingErrorContext errorContext, MshExpressionFactory expressionFactory, PSObject so, TypeInfoDataBase db, FormattingCommandLineParameters parameters)
		{
			base.Initialize(errorContext, expressionFactory, so, db, parameters);
			if (this.dataBaseInfo != null && this.dataBaseInfo.view != null)
			{
				this.tableBody = (TableControlBody)this.dataBaseInfo.view.mainControl;
			}
			List<MshParameter> list = null;
			if (parameters != null)
			{
				list = parameters.mshParameterList;
			}
			if (list != null && list.Count > 0)
			{
				this.activeAssociationList = AssociationManager.ExpandTableParameters(list, so);
				return;
			}
			this.activeAssociationList = AssociationManager.ExpandDefaultPropertySet(so, this.expressionFactory);
			if (this.activeAssociationList.Count > 0)
			{
				if (PSObjectHelper.ShouldShowComputerNameProperty(so))
				{
					this.activeAssociationList.Add(new MshResolvedExpressionParameterAssociation(null, new MshExpression(RemotingConstants.ComputerNameNoteProperty)));
				}
				return;
			}
			this.activeAssociationList = AssociationManager.ExpandAll(so);
			if (this.activeAssociationList.Count > 0)
			{
				AssociationManager.HandleComputerNameProperties(so, this.activeAssociationList);
				this.FilterActiveAssociationList();
				return;
			}
			this.activeAssociationList = new List<MshResolvedExpressionParameterAssociation>();
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x00122038 File Offset: 0x00120238
		internal override void PrepareForRemoteObjects(PSObject so)
		{
			PSPropertyInfo pspropertyInfo = so.Properties[RemotingConstants.ComputerNameNoteProperty];
			if (this.dataBaseInfo != null && this.dataBaseInfo.view != null && this.dataBaseInfo.view.mainControl != null)
			{
				this.tableBody = (TableControlBody)this.dataBaseInfo.view.mainControl.Copy();
				TableRowItemDefinition tableRowItemDefinition = new TableRowItemDefinition();
				PropertyTokenBase propertyTokenBase = new FieldPropertyToken();
				propertyTokenBase.expression = new ExpressionToken(RemotingConstants.ComputerNameNoteProperty, false);
				tableRowItemDefinition.formatTokenList.Add(propertyTokenBase);
				this.tableBody.defaultDefinition.rowItemDefinitionList.Add(tableRowItemDefinition);
				if (this.tableBody.header.columnHeaderDefinitionList.Count > 0)
				{
					TableColumnHeaderDefinition tableColumnHeaderDefinition = new TableColumnHeaderDefinition();
					tableColumnHeaderDefinition.label = new TextToken();
					tableColumnHeaderDefinition.label.text = RemotingConstants.ComputerNameNoteProperty;
					this.tableBody.header.columnHeaderDefinitionList.Add(tableColumnHeaderDefinition);
				}
			}
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x00122134 File Offset: 0x00120334
		internal override FormatStartData GenerateStartData(PSObject so)
		{
			FormatStartData formatStartData = base.GenerateStartData(so);
			if (this.dataBaseInfo.view != null)
			{
				formatStartData.shapeInfo = this.GenerateTableHeaderInfoFromDataBaseInfo(so);
			}
			else
			{
				formatStartData.shapeInfo = this.GenerateTableHeaderInfoFromProperties(so);
			}
			return formatStartData;
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x00122174 File Offset: 0x00120374
		private void FilterActiveAssociationList()
		{
			int num = 10;
			if (this.activeAssociationList.Count > num)
			{
				List<MshResolvedExpressionParameterAssociation> activeAssociationList = this.activeAssociationList;
				this.activeAssociationList = new List<MshResolvedExpressionParameterAssociation>();
				for (int i = 0; i < num; i++)
				{
					this.activeAssociationList.Add(activeAssociationList[i]);
				}
			}
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x001221C4 File Offset: 0x001203C4
		private TableHeaderInfo GenerateTableHeaderInfoFromDataBaseInfo(PSObject so)
		{
			TableHeaderInfo tableHeaderInfo = new TableHeaderInfo();
			bool flag;
			List<TableRowItemDefinition> activeTableRowDefinition = this.GetActiveTableRowDefinition(this.tableBody, so, out flag);
			tableHeaderInfo.hideHeader = this.HideHeaders;
			int num = 0;
			foreach (TableRowItemDefinition tableRowItemDefinition in activeTableRowDefinition)
			{
				TableColumnInfo tableColumnInfo = new TableColumnInfo();
				TableColumnHeaderDefinition tableColumnHeaderDefinition = null;
				if (this.tableBody.header.columnHeaderDefinitionList.Count > 0)
				{
					tableColumnHeaderDefinition = this.tableBody.header.columnHeaderDefinitionList[num];
				}
				if (tableColumnHeaderDefinition != null)
				{
					tableColumnInfo.width = tableColumnHeaderDefinition.width;
					tableColumnInfo.alignment = tableColumnHeaderDefinition.alignment;
					if (tableColumnHeaderDefinition.label != null)
					{
						tableColumnInfo.label = this.dataBaseInfo.db.displayResourceManagerCache.GetTextTokenString(tableColumnHeaderDefinition.label);
					}
				}
				if (tableColumnInfo.alignment == 0)
				{
					tableColumnInfo.alignment = tableRowItemDefinition.alignment;
				}
				if (tableColumnInfo.label == null)
				{
					FormatToken formatToken = null;
					if (tableRowItemDefinition.formatTokenList.Count > 0)
					{
						formatToken = tableRowItemDefinition.formatTokenList[0];
					}
					if (formatToken != null)
					{
						FieldPropertyToken fieldPropertyToken = formatToken as FieldPropertyToken;
						if (fieldPropertyToken != null)
						{
							tableColumnInfo.label = fieldPropertyToken.expression.expressionValue;
						}
						else
						{
							TextToken textToken = formatToken as TextToken;
							if (textToken != null)
							{
								tableColumnInfo.label = this.dataBaseInfo.db.displayResourceManagerCache.GetTextTokenString(textToken);
							}
						}
					}
					else
					{
						tableColumnInfo.label = "";
					}
				}
				tableHeaderInfo.tableColumnInfoList.Add(tableColumnInfo);
				num++;
			}
			return tableHeaderInfo;
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x00122380 File Offset: 0x00120580
		private TableHeaderInfo GenerateTableHeaderInfoFromProperties(PSObject so)
		{
			TableHeaderInfo tableHeaderInfo = new TableHeaderInfo();
			tableHeaderInfo.hideHeader = this.HideHeaders;
			for (int i = 0; i < this.activeAssociationList.Count; i++)
			{
				MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation = this.activeAssociationList[i];
				TableColumnInfo tableColumnInfo = new TableColumnInfo();
				if (mshResolvedExpressionParameterAssociation.OriginatingParameter != null)
				{
					object entry = mshResolvedExpressionParameterAssociation.OriginatingParameter.GetEntry("label");
					if (entry != AutomationNull.Value)
					{
						tableColumnInfo.propertyName = (string)entry;
					}
				}
				if (tableColumnInfo.propertyName == null)
				{
					tableColumnInfo.propertyName = this.activeAssociationList[i].ResolvedExpression.ToString();
				}
				if (mshResolvedExpressionParameterAssociation.OriginatingParameter != null)
				{
					object entry2 = mshResolvedExpressionParameterAssociation.OriginatingParameter.GetEntry("width");
					if (entry2 != AutomationNull.Value)
					{
						tableColumnInfo.width = (int)entry2;
					}
					else
					{
						tableColumnInfo.width = 0;
					}
				}
				else
				{
					tableColumnInfo.width = 0;
				}
				if (mshResolvedExpressionParameterAssociation.OriginatingParameter != null)
				{
					object entry3 = mshResolvedExpressionParameterAssociation.OriginatingParameter.GetEntry("alignment");
					if (entry3 != AutomationNull.Value)
					{
						tableColumnInfo.alignment = (int)entry3;
					}
					else
					{
						tableColumnInfo.alignment = TableViewGenerator.ComputeDefaultAlignment(so, mshResolvedExpressionParameterAssociation.ResolvedExpression);
					}
				}
				else
				{
					tableColumnInfo.alignment = TableViewGenerator.ComputeDefaultAlignment(so, mshResolvedExpressionParameterAssociation.ResolvedExpression);
				}
				tableHeaderInfo.tableColumnInfoList.Add(tableColumnInfo);
			}
			return tableHeaderInfo;
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600355E RID: 13662 RVA: 0x001224CC File Offset: 0x001206CC
		private bool HideHeaders
		{
			get
			{
				if (this.parameters != null && this.parameters.shapeParameters != null)
				{
					TableSpecificParameters tableSpecificParameters = (TableSpecificParameters)this.parameters.shapeParameters;
					if (tableSpecificParameters != null && tableSpecificParameters.hideHeaders != null)
					{
						return tableSpecificParameters.hideHeaders.Value;
					}
				}
				return this.dataBaseInfo.view != null && this.tableBody.header.hideHeader;
			}
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x0012253C File Offset: 0x0012073C
		private static int ComputeDefaultAlignment(PSObject so, MshExpression ex)
		{
			List<MshExpressionResult> values = ex.GetValues(so);
			if (values.Count == 0 || values[0].Exception != null)
			{
				return 1;
			}
			object result = values[0].Result;
			if (result == null)
			{
				return 1;
			}
			PSObject psobject = PSObject.AsPSObject(result);
			ConsolidatedString internalTypeNames = psobject.InternalTypeNames;
			if (string.Equals(PSObjectHelper.PSObjectIsOfExactType(internalTypeNames), "System.String", StringComparison.OrdinalIgnoreCase))
			{
				return 1;
			}
			if (DefaultScalarTypes.IsTypeInList(internalTypeNames))
			{
				return 3;
			}
			return 1;
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x001225AC File Offset: 0x001207AC
		internal override FormatEntryData GeneratePayload(PSObject so, int enumerationLimit)
		{
			FormatEntryData formatEntryData = new FormatEntryData();
			TableRowEntry tableRowEntry;
			if (this.dataBaseInfo.view != null)
			{
				tableRowEntry = this.GenerateTableRowEntryFromDataBaseInfo(so, enumerationLimit);
			}
			else
			{
				tableRowEntry = this.GenerateTableRowEntryFromFromProperties(so, enumerationLimit);
				tableRowEntry.multiLine = this.dataBaseInfo.db.defaultSettingsSection.MultilineTables;
			}
			formatEntryData.formatEntryInfo = tableRowEntry;
			if (this.parameters != null && this.parameters.shapeParameters != null)
			{
				TableSpecificParameters tableSpecificParameters = (TableSpecificParameters)this.parameters.shapeParameters;
				if (tableSpecificParameters != null && tableSpecificParameters.multiLine != null)
				{
					tableRowEntry.multiLine = tableSpecificParameters.multiLine.Value;
				}
			}
			return formatEntryData;
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x0012264C File Offset: 0x0012084C
		private List<TableRowItemDefinition> GetActiveTableRowDefinition(TableControlBody tableBody, PSObject so, out bool multiLine)
		{
			multiLine = tableBody.defaultDefinition.multiLine;
			if (tableBody.optionalDefinitionList.Count == 0)
			{
				return tableBody.defaultDefinition.rowItemDefinitionList;
			}
			TableRowDefinition tableRowDefinition = null;
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			TypeMatch typeMatch = new TypeMatch(this.expressionFactory, this.dataBaseInfo.db, internalTypeNames);
			foreach (TableRowDefinition tableRowDefinition2 in tableBody.optionalDefinitionList)
			{
				if (typeMatch.PerfectMatch(new TypeMatchItem(tableRowDefinition2, tableRowDefinition2.appliesTo)))
				{
					tableRowDefinition = tableRowDefinition2;
					break;
				}
			}
			if (tableRowDefinition == null)
			{
				tableRowDefinition = (typeMatch.BestMatch as TableRowDefinition);
			}
			if (tableRowDefinition == null)
			{
				Collection<string> collection = Deserializer.MaskDeserializationPrefix(internalTypeNames);
				if (collection != null)
				{
					typeMatch = new TypeMatch(this.expressionFactory, this.dataBaseInfo.db, collection);
					foreach (TableRowDefinition tableRowDefinition3 in tableBody.optionalDefinitionList)
					{
						if (typeMatch.PerfectMatch(new TypeMatchItem(tableRowDefinition3, tableRowDefinition3.appliesTo)))
						{
							tableRowDefinition = tableRowDefinition3;
							break;
						}
					}
					if (tableRowDefinition == null)
					{
						tableRowDefinition = (typeMatch.BestMatch as TableRowDefinition);
					}
				}
			}
			if (tableRowDefinition == null)
			{
				return tableBody.defaultDefinition.rowItemDefinitionList;
			}
			if (tableRowDefinition.multiLine)
			{
				multiLine = tableRowDefinition.multiLine;
			}
			List<TableRowItemDefinition> list = new List<TableRowItemDefinition>();
			int num = 0;
			foreach (TableRowItemDefinition tableRowItemDefinition in tableRowDefinition.rowItemDefinitionList)
			{
				if (tableRowItemDefinition.formatTokenList.Count == 0)
				{
					list.Add(tableBody.defaultDefinition.rowItemDefinitionList[num]);
				}
				else
				{
					list.Add(tableRowItemDefinition);
				}
				num++;
			}
			return list;
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x0012283C File Offset: 0x00120A3C
		private TableRowEntry GenerateTableRowEntryFromDataBaseInfo(PSObject so, int enumerationLimit)
		{
			TableRowEntry tableRowEntry = new TableRowEntry();
			List<TableRowItemDefinition> activeTableRowDefinition = this.GetActiveTableRowDefinition(this.tableBody, so, out tableRowEntry.multiLine);
			foreach (TableRowItemDefinition tableRowItemDefinition in activeTableRowDefinition)
			{
				FormatPropertyField formatPropertyField = base.GenerateFormatPropertyField(tableRowItemDefinition.formatTokenList, so, enumerationLimit);
				formatPropertyField.alignment = tableRowItemDefinition.alignment;
				tableRowEntry.formatPropertyFieldList.Add(formatPropertyField);
			}
			return tableRowEntry;
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x001228C8 File Offset: 0x00120AC8
		private TableRowEntry GenerateTableRowEntryFromFromProperties(PSObject so, int enumerationLimit)
		{
			TableRowEntry tableRowEntry = new TableRowEntry();
			for (int i = 0; i < this.activeAssociationList.Count; i++)
			{
				FormatPropertyField formatPropertyField = new FormatPropertyField();
				FieldFormattingDirective directive = null;
				if (this.activeAssociationList[i].OriginatingParameter != null)
				{
					directive = (this.activeAssociationList[i].OriginatingParameter.GetEntry("formatString") as FieldFormattingDirective);
				}
				formatPropertyField.propertyValue = base.GetExpressionDisplayValue(so, enumerationLimit, this.activeAssociationList[i].ResolvedExpression, directive);
				tableRowEntry.formatPropertyFieldList.Add(formatPropertyField);
			}
			return tableRowEntry;
		}

		// Token: 0x04001B42 RID: 6978
		private TableControlBody tableBody;
	}
}
