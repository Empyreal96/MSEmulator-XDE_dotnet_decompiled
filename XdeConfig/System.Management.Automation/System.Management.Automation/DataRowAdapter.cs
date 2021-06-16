using System;
using System.Data;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000184 RID: 388
	internal class DataRowAdapter : PropertyOnlyAdapter
	{
		// Token: 0x060012FB RID: 4859 RVA: 0x0007609C File Offset: 0x0007429C
		protected override void DoAddAllProperties<T>(object obj, PSMemberInfoInternalCollection<T> members)
		{
			DataRow dataRow = (DataRow)obj;
			if (dataRow.Table == null || dataRow.Table.Columns == null)
			{
				return;
			}
			foreach (object obj2 in dataRow.Table.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj2;
				members.Add(new PSProperty(dataColumn.ColumnName, this, obj, dataColumn.ColumnName) as T);
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00076134 File Offset: 0x00074334
		protected override PSProperty DoGetProperty(object obj, string propertyName)
		{
			DataRow dataRow = (DataRow)obj;
			if (!dataRow.Table.Columns.Contains(propertyName))
			{
				return null;
			}
			string columnName = dataRow.Table.Columns[propertyName].ColumnName;
			return new PSProperty(columnName, this, obj, columnName);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00076180 File Offset: 0x00074380
		protected override string PropertyType(PSProperty property, bool forDisplay)
		{
			string name = (string)property.adapterData;
			DataRow dataRow = (DataRow)property.baseObject;
			Type dataType = dataRow.Table.Columns[name].DataType;
			if (!forDisplay)
			{
				return dataType.FullName;
			}
			return ToStringCodeMethods.Type(dataType, false);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x000761D0 File Offset: 0x000743D0
		protected override bool PropertyIsSettable(PSProperty property)
		{
			string name = (string)property.adapterData;
			DataRow dataRow = (DataRow)property.baseObject;
			return !dataRow.Table.Columns[name].ReadOnly;
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0007620E File Offset: 0x0007440E
		protected override bool PropertyIsGettable(PSProperty property)
		{
			return true;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00076214 File Offset: 0x00074414
		protected override object PropertyGet(PSProperty property)
		{
			DataRow dataRow = (DataRow)property.baseObject;
			return dataRow[(string)property.adapterData];
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00076240 File Offset: 0x00074440
		protected override void PropertySet(PSProperty property, object setValue, bool convertIfPossible)
		{
			DataRow dataRow = (DataRow)property.baseObject;
			dataRow[(string)property.adapterData] = setValue;
		}
	}
}
