using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004B1 RID: 1201
	internal static class OutOfBandFormatViewManager
	{
		// Token: 0x06003579 RID: 13689 RVA: 0x00123710 File Offset: 0x00121910
		internal static bool IsPropertyLessObject(PSObject so)
		{
			List<MshResolvedExpressionParameterAssociation> list = AssociationManager.ExpandAll(so);
			if (list.Count == 0)
			{
				return true;
			}
			if (list.Count == 3)
			{
				foreach (MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation in list)
				{
					if (!mshResolvedExpressionParameterAssociation.ResolvedExpression.ToString().Equals(RemotingConstants.ComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase) && !mshResolvedExpressionParameterAssociation.ResolvedExpression.ToString().Equals(RemotingConstants.ShowComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase) && !mshResolvedExpressionParameterAssociation.ResolvedExpression.ToString().Equals(RemotingConstants.RunspaceIdNoteProperty, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
				return true;
			}
			if (list.Count == 4)
			{
				foreach (MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation2 in list)
				{
					if (!mshResolvedExpressionParameterAssociation2.ResolvedExpression.ToString().Equals(RemotingConstants.ComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase) && !mshResolvedExpressionParameterAssociation2.ResolvedExpression.ToString().Equals(RemotingConstants.ShowComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase) && !mshResolvedExpressionParameterAssociation2.ResolvedExpression.ToString().Equals(RemotingConstants.RunspaceIdNoteProperty, StringComparison.OrdinalIgnoreCase) && !mshResolvedExpressionParameterAssociation2.ResolvedExpression.ToString().Equals(RemotingConstants.SourceJobInstanceId, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
				return true;
			}
			if (list.Count == 5)
			{
				foreach (MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation3 in list)
				{
					if (!mshResolvedExpressionParameterAssociation3.ResolvedExpression.ToString().Equals(RemotingConstants.ComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase) && !mshResolvedExpressionParameterAssociation3.ResolvedExpression.ToString().Equals(RemotingConstants.ShowComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase) && !mshResolvedExpressionParameterAssociation3.ResolvedExpression.ToString().Equals(RemotingConstants.RunspaceIdNoteProperty, StringComparison.OrdinalIgnoreCase) && !mshResolvedExpressionParameterAssociation3.ResolvedExpression.ToString().Equals(RemotingConstants.SourceJobInstanceId, StringComparison.OrdinalIgnoreCase) && !mshResolvedExpressionParameterAssociation3.ResolvedExpression.ToString().Equals(RemotingConstants.SourceLength, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x00123940 File Offset: 0x00121B40
		internal static FormatEntryData GenerateOutOfBandData(TerminatingErrorContext errorContext, MshExpressionFactory expressionFactory, TypeInfoDataBase db, PSObject so, int enumerationLimit, bool useToStringFallback, out List<ErrorRecord> errors)
		{
			errors = null;
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			ViewDefinition outOfBandView = DisplayDataQuery.GetOutOfBandView(expressionFactory, db, internalTypeNames);
			ViewGenerator viewGenerator;
			if (outOfBandView != null)
			{
				if (outOfBandView.mainControl is ComplexControlBody)
				{
					viewGenerator = new ComplexViewGenerator();
				}
				else
				{
					viewGenerator = new ListViewGenerator();
				}
				viewGenerator.Initialize(errorContext, expressionFactory, db, outOfBandView, null);
			}
			else
			{
				if (DefaultScalarTypes.IsTypeInList(internalTypeNames) || OutOfBandFormatViewManager.IsPropertyLessObject(so))
				{
					return OutOfBandFormatViewManager.GenerateOutOfBandObjectAsToString(so);
				}
				if (!useToStringFallback)
				{
					return null;
				}
				if (new MshExpression("*").ResolveNames(so).Count <= 0)
				{
					return null;
				}
				viewGenerator = new ListViewGenerator();
				viewGenerator.Initialize(errorContext, expressionFactory, so, db, null);
			}
			FormatEntryData formatEntryData = viewGenerator.GeneratePayload(so, enumerationLimit);
			formatEntryData.outOfBand = true;
			formatEntryData.SetStreamTypeFromPSObject(so);
			errors = viewGenerator.ErrorManager.DrainFailedResultList();
			return formatEntryData;
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x001239FC File Offset: 0x00121BFC
		internal static FormatEntryData GenerateOutOfBandObjectAsToString(PSObject so)
		{
			return new FormatEntryData
			{
				outOfBand = true,
				formatEntryInfo = new RawTextFormatEntry
				{
					text = so.ToString()
				}
			};
		}
	}
}
