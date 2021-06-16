using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004AC RID: 1196
	internal sealed class ListViewGenerator : ViewGenerator
	{
		// Token: 0x0600354D RID: 13645 RVA: 0x00121964 File Offset: 0x0011FB64
		internal override void Initialize(TerminatingErrorContext terminatingErrorContext, MshExpressionFactory mshExpressionFactory, TypeInfoDataBase db, ViewDefinition view, FormattingCommandLineParameters formatParameters)
		{
			base.Initialize(terminatingErrorContext, mshExpressionFactory, db, view, formatParameters);
			if (this.dataBaseInfo != null && this.dataBaseInfo.view != null)
			{
				this.listBody = (ListControlBody)this.dataBaseInfo.view.mainControl;
			}
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x001219A4 File Offset: 0x0011FBA4
		internal override void Initialize(TerminatingErrorContext errorContext, MshExpressionFactory expressionFactory, PSObject so, TypeInfoDataBase db, FormattingCommandLineParameters parameters)
		{
			base.Initialize(errorContext, expressionFactory, so, db, parameters);
			if (this.dataBaseInfo != null && this.dataBaseInfo.view != null)
			{
				this.listBody = (ListControlBody)this.dataBaseInfo.view.mainControl;
			}
			this.inputParameters = parameters;
			this.SetUpActiveProperties(so);
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x00121A00 File Offset: 0x0011FC00
		internal override void PrepareForRemoteObjects(PSObject so)
		{
			PSPropertyInfo pspropertyInfo = so.Properties[RemotingConstants.ComputerNameNoteProperty];
			if (this.dataBaseInfo != null && this.dataBaseInfo.view != null && this.dataBaseInfo.view.mainControl != null)
			{
				this.listBody = (ListControlBody)this.dataBaseInfo.view.mainControl.Copy();
				ListControlItemDefinition listControlItemDefinition = new ListControlItemDefinition();
				listControlItemDefinition.label = new TextToken();
				listControlItemDefinition.label.text = RemotingConstants.ComputerNameNoteProperty;
				FieldPropertyToken fieldPropertyToken = new FieldPropertyToken();
				fieldPropertyToken.expression = new ExpressionToken(RemotingConstants.ComputerNameNoteProperty, false);
				listControlItemDefinition.formatTokenList.Add(fieldPropertyToken);
				this.listBody.defaultEntryDefinition.itemDefinitionList.Add(listControlItemDefinition);
			}
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x00121AC8 File Offset: 0x0011FCC8
		internal override FormatStartData GenerateStartData(PSObject so)
		{
			FormatStartData formatStartData = base.GenerateStartData(so);
			formatStartData.shapeInfo = new ListViewHeaderInfo();
			return formatStartData;
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x00121AEC File Offset: 0x0011FCEC
		internal override FormatEntryData GeneratePayload(PSObject so, int enumerationLimit)
		{
			FormatEntryData formatEntryData = new FormatEntryData();
			if (this.dataBaseInfo.view != null)
			{
				formatEntryData.formatEntryInfo = this.GenerateListViewEntryFromDataBaseInfo(so, enumerationLimit);
			}
			else
			{
				formatEntryData.formatEntryInfo = this.GenerateListViewEntryFromProperties(so, enumerationLimit);
			}
			return formatEntryData;
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x00121B2C File Offset: 0x0011FD2C
		private ListViewEntry GenerateListViewEntryFromDataBaseInfo(PSObject so, int enumerationLimit)
		{
			ListViewEntry listViewEntry = new ListViewEntry();
			ListControlEntryDefinition activeListControlEntryDefinition = this.GetActiveListControlEntryDefinition(this.listBody, so);
			foreach (ListControlItemDefinition listControlItemDefinition in activeListControlEntryDefinition.itemDefinitionList)
			{
				if (base.EvaluateDisplayCondition(so, listControlItemDefinition.conditionToken))
				{
					ListViewField listViewField = new ListViewField();
					MshExpressionResult mshExpressionResult;
					listViewField.formatPropertyField = base.GenerateFormatPropertyField(listControlItemDefinition.formatTokenList, so, enumerationLimit, out mshExpressionResult);
					if (listControlItemDefinition.label != null)
					{
						listViewField.label = this.dataBaseInfo.db.displayResourceManagerCache.GetTextTokenString(listControlItemDefinition.label);
					}
					else if (mshExpressionResult != null)
					{
						listViewField.label = mshExpressionResult.ResolvedExpression.ToString();
					}
					else
					{
						FormatToken formatToken = listControlItemDefinition.formatTokenList[0];
						FieldPropertyToken fieldPropertyToken = formatToken as FieldPropertyToken;
						if (fieldPropertyToken != null)
						{
							MshExpression mshExpression = this.expressionFactory.CreateFromExpressionToken(fieldPropertyToken.expression, this.dataBaseInfo.view.loadingInfo);
							listViewField.label = mshExpression.ToString();
						}
						else
						{
							TextToken textToken = formatToken as TextToken;
							if (textToken != null)
							{
								listViewField.label = this.dataBaseInfo.db.displayResourceManagerCache.GetTextTokenString(textToken);
							}
						}
					}
					listViewEntry.listViewFieldList.Add(listViewField);
				}
			}
			return listViewEntry;
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x00121C98 File Offset: 0x0011FE98
		private ListControlEntryDefinition GetActiveListControlEntryDefinition(ListControlBody listBody, PSObject so)
		{
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			TypeMatch typeMatch = new TypeMatch(this.expressionFactory, this.dataBaseInfo.db, internalTypeNames);
			foreach (ListControlEntryDefinition listControlEntryDefinition in listBody.optionalEntryList)
			{
				if (typeMatch.PerfectMatch(new TypeMatchItem(listControlEntryDefinition, listControlEntryDefinition.appliesTo, so)))
				{
					return listControlEntryDefinition;
				}
			}
			if (typeMatch.BestMatch != null)
			{
				return typeMatch.BestMatch as ListControlEntryDefinition;
			}
			Collection<string> collection = Deserializer.MaskDeserializationPrefix(internalTypeNames);
			if (collection != null)
			{
				typeMatch = new TypeMatch(this.expressionFactory, this.dataBaseInfo.db, collection);
				foreach (ListControlEntryDefinition listControlEntryDefinition2 in listBody.optionalEntryList)
				{
					if (typeMatch.PerfectMatch(new TypeMatchItem(listControlEntryDefinition2, listControlEntryDefinition2.appliesTo)))
					{
						return listControlEntryDefinition2;
					}
				}
				if (typeMatch.BestMatch != null)
				{
					return typeMatch.BestMatch as ListControlEntryDefinition;
				}
			}
			return listBody.defaultEntryDefinition;
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x00121DD0 File Offset: 0x0011FFD0
		private ListViewEntry GenerateListViewEntryFromProperties(PSObject so, int enumerationLimit)
		{
			if (this.activeAssociationList == null)
			{
				this.SetUpActiveProperties(so);
			}
			ListViewEntry listViewEntry = new ListViewEntry();
			for (int i = 0; i < this.activeAssociationList.Count; i++)
			{
				MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation = this.activeAssociationList[i];
				ListViewField listViewField = new ListViewField();
				if (mshResolvedExpressionParameterAssociation.OriginatingParameter != null)
				{
					object entry = mshResolvedExpressionParameterAssociation.OriginatingParameter.GetEntry("label");
					if (entry != AutomationNull.Value)
					{
						listViewField.propertyName = (string)entry;
					}
					else
					{
						listViewField.propertyName = mshResolvedExpressionParameterAssociation.ResolvedExpression.ToString();
					}
				}
				else
				{
					listViewField.propertyName = mshResolvedExpressionParameterAssociation.ResolvedExpression.ToString();
				}
				FieldFormattingDirective directive = null;
				if (mshResolvedExpressionParameterAssociation.OriginatingParameter != null)
				{
					directive = (mshResolvedExpressionParameterAssociation.OriginatingParameter.GetEntry("formatString") as FieldFormattingDirective);
				}
				listViewField.formatPropertyField.propertyValue = base.GetExpressionDisplayValue(so, enumerationLimit, mshResolvedExpressionParameterAssociation.ResolvedExpression, directive);
				listViewEntry.listViewFieldList.Add(listViewField);
			}
			this.activeAssociationList = null;
			return listViewEntry;
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x00121EC8 File Offset: 0x001200C8
		private void SetUpActiveProperties(PSObject so)
		{
			List<MshParameter> rawMshParameterList = null;
			if (this.inputParameters != null)
			{
				rawMshParameterList = this.inputParameters.mshParameterList;
			}
			this.activeAssociationList = AssociationManager.SetupActiveProperties(rawMshParameterList, so, this.expressionFactory);
		}

		// Token: 0x04001B41 RID: 6977
		private ListControlBody listBody;
	}
}
