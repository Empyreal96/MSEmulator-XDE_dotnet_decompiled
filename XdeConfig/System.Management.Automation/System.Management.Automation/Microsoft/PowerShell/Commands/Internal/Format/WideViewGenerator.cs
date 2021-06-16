using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004AE RID: 1198
	internal sealed class WideViewGenerator : ViewGenerator
	{
		// Token: 0x06003565 RID: 13669 RVA: 0x00122962 File Offset: 0x00120B62
		internal override void Initialize(TerminatingErrorContext errorContext, MshExpressionFactory expressionFactory, PSObject so, TypeInfoDataBase db, FormattingCommandLineParameters parameters)
		{
			base.Initialize(errorContext, expressionFactory, so, db, parameters);
			this.inputParameters = parameters;
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x0012297C File Offset: 0x00120B7C
		internal override FormatStartData GenerateStartData(PSObject so)
		{
			FormatStartData formatStartData = base.GenerateStartData(so);
			WideViewHeaderInfo wideViewHeaderInfo = new WideViewHeaderInfo();
			formatStartData.shapeInfo = wideViewHeaderInfo;
			if (!base.AutoSize)
			{
				wideViewHeaderInfo.columns = this.Columns;
			}
			else
			{
				wideViewHeaderInfo.columns = 0;
			}
			return formatStartData;
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06003567 RID: 13671 RVA: 0x001229BC File Offset: 0x00120BBC
		private int Columns
		{
			get
			{
				if (this.parameters != null && this.parameters.shapeParameters != null)
				{
					WideSpecificParameters wideSpecificParameters = (WideSpecificParameters)this.parameters.shapeParameters;
					if (wideSpecificParameters.columns != null)
					{
						return wideSpecificParameters.columns.Value;
					}
				}
				if (this.dataBaseInfo.view != null && this.dataBaseInfo.view.mainControl != null)
				{
					WideControlBody wideControlBody = (WideControlBody)this.dataBaseInfo.view.mainControl;
					return wideControlBody.columns;
				}
				return 0;
			}
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x00122A48 File Offset: 0x00120C48
		internal override FormatEntryData GeneratePayload(PSObject so, int enumerationLimit)
		{
			FormatEntryData formatEntryData = new FormatEntryData();
			if (this.dataBaseInfo.view != null)
			{
				formatEntryData.formatEntryInfo = this.GenerateWideViewEntryFromDataBaseInfo(so, enumerationLimit);
			}
			else
			{
				formatEntryData.formatEntryInfo = this.GenerateWideViewEntryFromProperties(so, enumerationLimit);
			}
			return formatEntryData;
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x00122A88 File Offset: 0x00120C88
		private WideViewEntry GenerateWideViewEntryFromDataBaseInfo(PSObject so, int enumerationLimit)
		{
			WideControlBody wideBody = (WideControlBody)this.dataBaseInfo.view.mainControl;
			WideControlEntryDefinition activeWideControlEntryDefinition = this.GetActiveWideControlEntryDefinition(wideBody, so);
			return new WideViewEntry
			{
				formatPropertyField = base.GenerateFormatPropertyField(activeWideControlEntryDefinition.formatTokenList, so, enumerationLimit)
			};
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x00122AD0 File Offset: 0x00120CD0
		private WideControlEntryDefinition GetActiveWideControlEntryDefinition(WideControlBody wideBody, PSObject so)
		{
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			TypeMatch typeMatch = new TypeMatch(this.expressionFactory, this.dataBaseInfo.db, internalTypeNames);
			foreach (WideControlEntryDefinition wideControlEntryDefinition in wideBody.optionalEntryList)
			{
				if (typeMatch.PerfectMatch(new TypeMatchItem(wideControlEntryDefinition, wideControlEntryDefinition.appliesTo)))
				{
					return wideControlEntryDefinition;
				}
			}
			if (typeMatch.BestMatch != null)
			{
				return typeMatch.BestMatch as WideControlEntryDefinition;
			}
			Collection<string> collection = Deserializer.MaskDeserializationPrefix(internalTypeNames);
			if (collection != null)
			{
				typeMatch = new TypeMatch(this.expressionFactory, this.dataBaseInfo.db, collection);
				foreach (WideControlEntryDefinition wideControlEntryDefinition2 in wideBody.optionalEntryList)
				{
					if (typeMatch.PerfectMatch(new TypeMatchItem(wideControlEntryDefinition2, wideControlEntryDefinition2.appliesTo)))
					{
						return wideControlEntryDefinition2;
					}
				}
				if (typeMatch.BestMatch != null)
				{
					return typeMatch.BestMatch as WideControlEntryDefinition;
				}
			}
			return wideBody.defaultEntryDefinition;
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x00122C04 File Offset: 0x00120E04
		private WideViewEntry GenerateWideViewEntryFromProperties(PSObject so, int enumerationLimit)
		{
			if (this.activeAssociationList == null)
			{
				this.SetUpActiveProperty(so);
			}
			WideViewEntry wideViewEntry = new WideViewEntry();
			FormatPropertyField formatPropertyField = new FormatPropertyField();
			wideViewEntry.formatPropertyField = formatPropertyField;
			if (this.activeAssociationList.Count > 0)
			{
				MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation = this.activeAssociationList[0];
				FieldFormattingDirective directive = null;
				if (mshResolvedExpressionParameterAssociation.OriginatingParameter != null)
				{
					directive = (mshResolvedExpressionParameterAssociation.OriginatingParameter.GetEntry("formatString") as FieldFormattingDirective);
				}
				formatPropertyField.propertyValue = base.GetExpressionDisplayValue(so, enumerationLimit, mshResolvedExpressionParameterAssociation.ResolvedExpression, directive);
			}
			this.activeAssociationList = null;
			return wideViewEntry;
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x00122C8C File Offset: 0x00120E8C
		private void SetUpActiveProperty(PSObject so)
		{
			List<MshParameter> list = null;
			if (this.inputParameters != null)
			{
				list = this.inputParameters.mshParameterList;
			}
			if (list != null && list.Count > 0)
			{
				this.activeAssociationList = AssociationManager.ExpandParameters(list, so);
				return;
			}
			MshExpression displayNameExpression = PSObjectHelper.GetDisplayNameExpression(so, this.expressionFactory);
			if (displayNameExpression != null)
			{
				this.activeAssociationList = new List<MshResolvedExpressionParameterAssociation>();
				this.activeAssociationList.Add(new MshResolvedExpressionParameterAssociation(null, displayNameExpression));
				return;
			}
			this.activeAssociationList = AssociationManager.ExpandDefaultPropertySet(so, this.expressionFactory);
			if (this.activeAssociationList.Count > 0)
			{
				return;
			}
			this.activeAssociationList = AssociationManager.ExpandAll(so);
		}
	}
}
