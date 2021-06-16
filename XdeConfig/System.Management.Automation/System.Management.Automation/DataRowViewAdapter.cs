using System;
using System.Data;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000185 RID: 389
	internal class DataRowViewAdapter : PropertyOnlyAdapter
	{
		// Token: 0x06001303 RID: 4867 RVA: 0x00076274 File Offset: 0x00074474
		protected override void DoAddAllProperties<T>(object obj, PSMemberInfoInternalCollection<T> members)
		{
			DataRowView dataRowView = (DataRowView)obj;
			if (dataRowView.Row == null || dataRowView.Row.Table == null || dataRowView.Row.Table.Columns == null)
			{
				return;
			}
			foreach (object obj2 in dataRowView.Row.Table.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj2;
				members.Add(new PSProperty(dataColumn.ColumnName, this, obj, dataColumn.ColumnName) as T);
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x00076324 File Offset: 0x00074524
		protected override PSProperty DoGetProperty(object obj, string propertyName)
		{
			DataRowView dataRowView = (DataRowView)obj;
			if (!dataRowView.Row.Table.Columns.Contains(propertyName))
			{
				return null;
			}
			string columnName = dataRowView.Row.Table.Columns[propertyName].ColumnName;
			return new PSProperty(columnName, this, obj, columnName);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00076378 File Offset: 0x00074578
		protected override string PropertyType(PSProperty property, bool forDisplay)
		{
			string name = (string)property.adapterData;
			DataRowView dataRowView = (DataRowView)property.baseObject;
			Type dataType = dataRowView.Row.Table.Columns[name].DataType;
			if (!forDisplay)
			{
				return dataType.FullName;
			}
			return ToStringCodeMethods.Type(dataType, false);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x000763CC File Offset: 0x000745CC
		protected override bool PropertyIsSettable(PSProperty property)
		{
			string name = (string)property.adapterData;
			DataRowView dataRowView = (DataRowView)property.baseObject;
			return !dataRowView.Row.Table.Columns[name].ReadOnly;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0007640F File Offset: 0x0007460F
		protected override bool PropertyIsGettable(PSProperty property)
		{
			return true;
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00076414 File Offset: 0x00074614
		protected override object PropertyGet(PSProperty property)
		{
			DataRowView dataRowView = (DataRowView)property.baseObject;
			return dataRowView[(string)property.adapterData];
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00076440 File Offset: 0x00074640
		protected override void PropertySet(PSProperty property, object setValue, bool convertIfPossible)
		{
			DataRowView dataRowView = (DataRowView)property.baseObject;
			dataRowView[(string)property.adapterData] = setValue;
		}
	}
}
