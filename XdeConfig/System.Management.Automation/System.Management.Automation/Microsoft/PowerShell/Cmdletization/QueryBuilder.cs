using System;
using System.Collections;

namespace Microsoft.PowerShell.Cmdletization
{
	// Token: 0x020009A3 RID: 2467
	public abstract class QueryBuilder
	{
		// Token: 0x06005AD6 RID: 23254 RVA: 0x001E8AC4 File Offset: 0x001E6CC4
		public virtual void FilterByProperty(string propertyName, IEnumerable allowedPropertyValues, bool wildcardsEnabled, BehaviorOnNoMatch behaviorOnNoMatch)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005AD7 RID: 23255 RVA: 0x001E8ACB File Offset: 0x001E6CCB
		public virtual void ExcludeByProperty(string propertyName, IEnumerable excludedPropertyValues, bool wildcardsEnabled, BehaviorOnNoMatch behaviorOnNoMatch)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005AD8 RID: 23256 RVA: 0x001E8AD2 File Offset: 0x001E6CD2
		public virtual void FilterByMinPropertyValue(string propertyName, object minPropertyValue, BehaviorOnNoMatch behaviorOnNoMatch)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005AD9 RID: 23257 RVA: 0x001E8AD9 File Offset: 0x001E6CD9
		public virtual void FilterByMaxPropertyValue(string propertyName, object maxPropertyValue, BehaviorOnNoMatch behaviorOnNoMatch)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005ADA RID: 23258 RVA: 0x001E8AE0 File Offset: 0x001E6CE0
		public virtual void FilterByAssociatedInstance(object associatedInstance, string associationName, string sourceRole, string resultRole, BehaviorOnNoMatch behaviorOnNoMatch)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005ADB RID: 23259 RVA: 0x001E8AE7 File Offset: 0x001E6CE7
		public virtual void AddQueryOption(string optionName, object optionValue)
		{
			throw new NotImplementedException();
		}
	}
}
