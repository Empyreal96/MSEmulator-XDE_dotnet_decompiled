using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004A8 RID: 1192
	internal sealed class ComplexViewGenerator : ViewGenerator
	{
		// Token: 0x0600352E RID: 13614 RVA: 0x00120B1A File Offset: 0x0011ED1A
		internal override void Initialize(TerminatingErrorContext errorContext, MshExpressionFactory expressionFactory, PSObject so, TypeInfoDataBase db, FormattingCommandLineParameters parameters)
		{
			base.Initialize(errorContext, expressionFactory, so, db, parameters);
			this.inputParameters = parameters;
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x00120B34 File Offset: 0x0011ED34
		internal override FormatStartData GenerateStartData(PSObject so)
		{
			FormatStartData formatStartData = base.GenerateStartData(so);
			formatStartData.shapeInfo = new ComplexViewHeaderInfo();
			return formatStartData;
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x00120B58 File Offset: 0x0011ED58
		internal override FormatEntryData GeneratePayload(PSObject so, int enumerationLimit)
		{
			FormatEntryData formatEntryData = new FormatEntryData();
			if (this.dataBaseInfo.view != null)
			{
				formatEntryData.formatEntryInfo = this.GenerateComplexViewEntryFromDataBaseInfo(so, enumerationLimit);
			}
			else
			{
				formatEntryData.formatEntryInfo = this.GenerateComplexViewEntryFromProperties(so, enumerationLimit);
			}
			return formatEntryData;
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x00120B98 File Offset: 0x0011ED98
		private ComplexViewEntry GenerateComplexViewEntryFromProperties(PSObject so, int enumerationLimit)
		{
			ComplexViewObjectBrowser complexViewObjectBrowser = new ComplexViewObjectBrowser(base.ErrorManager, this.expressionFactory, enumerationLimit);
			return complexViewObjectBrowser.GenerateView(so, this.inputParameters);
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x00120BC8 File Offset: 0x0011EDC8
		private ComplexViewEntry GenerateComplexViewEntryFromDataBaseInfo(PSObject so, int enumerationLimit)
		{
			ComplexViewEntry complexViewEntry = new ComplexViewEntry();
			ComplexControlGenerator complexControlGenerator = new ComplexControlGenerator(this.dataBaseInfo.db, this.dataBaseInfo.view.loadingInfo, this.expressionFactory, this.dataBaseInfo.view.formatControlDefinitionHolder.controlDefinitionList, base.ErrorManager, enumerationLimit, this.errorContext);
			complexControlGenerator.GenerateFormatEntries(50, this.dataBaseInfo.view.mainControl, so, complexViewEntry.formatValueList);
			return complexViewEntry;
		}
	}
}
