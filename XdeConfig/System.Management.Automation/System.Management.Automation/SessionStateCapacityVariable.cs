using System;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x02000838 RID: 2104
	internal class SessionStateCapacityVariable : PSVariable
	{
		// Token: 0x06005107 RID: 20743 RVA: 0x001B00FC File Offset: 0x001AE2FC
		internal SessionStateCapacityVariable(string name, int defaultCapacity, int maxCapacity, int minCapacity, ScopedItemOptions options) : base(name, defaultCapacity, options)
		{
			ValidateRangeAttribute item = new ValidateRangeAttribute(minCapacity, maxCapacity);
			this.minCapacity = minCapacity;
			this.maxCapacity = maxCapacity;
			base.Attributes.Add(item);
			this._fastValue = defaultCapacity;
		}

		// Token: 0x06005108 RID: 20744 RVA: 0x001B0158 File Offset: 0x001AE358
		public SessionStateCapacityVariable(string name, SessionStateCapacityVariable sharedCapacityVariable, ScopedItemOptions options) : base(name, sharedCapacityVariable.Value, options)
		{
			ValidateRangeAttribute item = new ValidateRangeAttribute(0, int.MaxValue);
			base.Attributes.Add(item);
			this.sharedCapacityVariable = sharedCapacityVariable;
			this.Description = sharedCapacityVariable.Description;
			this._fastValue = (int)sharedCapacityVariable.Value;
		}

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x06005109 RID: 20745 RVA: 0x001B01C4 File Offset: 0x001AE3C4
		// (set) Token: 0x0600510A RID: 20746 RVA: 0x001B01EF File Offset: 0x001AE3EF
		public override object Value
		{
			get
			{
				object value;
				if (this.sharedCapacityVariable != null)
				{
					value = this.sharedCapacityVariable.Value;
				}
				else
				{
					value = base.Value;
				}
				return value;
			}
			set
			{
				this.sharedCapacityVariable = null;
				base.Value = LanguagePrimitives.ConvertTo(value, typeof(int), CultureInfo.InvariantCulture);
				this._fastValue = (int)base.Value;
			}
		}

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x0600510B RID: 20747 RVA: 0x001B0224 File Offset: 0x001AE424
		internal int FastValue
		{
			get
			{
				return this._fastValue;
			}
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x001B022C File Offset: 0x001AE42C
		public override bool IsValidValue(object value)
		{
			int num = (int)value;
			return (num >= this.minCapacity && num <= this.maxCapacity) || base.IsValidValue(value);
		}

		// Token: 0x0400296D RID: 10605
		private int _fastValue;

		// Token: 0x0400296E RID: 10606
		private int minCapacity;

		// Token: 0x0400296F RID: 10607
		private int maxCapacity = int.MaxValue;

		// Token: 0x04002970 RID: 10608
		private SessionStateCapacityVariable sharedCapacityVariable;
	}
}
