using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004A5 RID: 1189
	internal sealed class GroupingInfoManager
	{
		// Token: 0x06003512 RID: 13586 RVA: 0x00120224 File Offset: 0x0011E424
		internal void Initialize(MshExpression groupingExpression, string displayLabel)
		{
			this.groupingKeyExpression = groupingExpression;
			this.label = displayLabel;
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06003513 RID: 13587 RVA: 0x00120234 File Offset: 0x0011E434
		internal object CurrentGroupingKeyPropertyValue
		{
			get
			{
				return this.currentGroupingKeyPropertyValue;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06003514 RID: 13588 RVA: 0x0012023C File Offset: 0x0011E43C
		internal string GroupingKeyDisplayName
		{
			get
			{
				if (this.label != null)
				{
					return this.label;
				}
				return this.groupingKeyDisplayName;
			}
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x00120254 File Offset: 0x0011E454
		internal bool UpdateGroupingKeyValue(PSObject so)
		{
			if (this.groupingKeyExpression == null)
			{
				return false;
			}
			List<MshExpressionResult> values = this.groupingKeyExpression.GetValues(so);
			if (values.Count > 0 && values[0].Exception == null)
			{
				object result = values[0].Result;
				object obj = this.currentGroupingKeyPropertyValue;
				this.currentGroupingKeyPropertyValue = result;
				bool flag = !GroupingInfoManager.IsEqual(this.currentGroupingKeyPropertyValue, obj) && !GroupingInfoManager.IsEqual(obj, this.currentGroupingKeyPropertyValue);
				if (flag && this.label == null)
				{
					this.groupingKeyDisplayName = values[0].ResolvedExpression.ToString();
				}
				return flag;
			}
			return false;
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x001202F0 File Offset: 0x0011E4F0
		private static bool IsEqual(object first, object second)
		{
			try
			{
				return LanguagePrimitives.Compare(first, second, true, CultureInfo.CurrentCulture) == 0;
			}
			catch (InvalidCastException)
			{
			}
			catch (ArgumentException)
			{
			}
			string strA = PSObject.AsPSObject(first).ToString();
			string strB = PSObject.AsPSObject(second).ToString();
			return string.Compare(strA, strB, StringComparison.CurrentCultureIgnoreCase) == 0;
		}

		// Token: 0x04001B23 RID: 6947
		private string label;

		// Token: 0x04001B24 RID: 6948
		private string groupingKeyDisplayName;

		// Token: 0x04001B25 RID: 6949
		private MshExpression groupingKeyExpression;

		// Token: 0x04001B26 RID: 6950
		private object currentGroupingKeyPropertyValue = AutomationNull.Value;
	}
}
