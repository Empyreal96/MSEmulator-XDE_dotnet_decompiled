using System;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000085 RID: 133
	internal struct TypeDescriptor
	{
		// Token: 0x0600031D RID: 797 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
		private TypeDescriptor(TargetType targetType, Maybe<int> maxItems, Maybe<TypeDescriptor> nextValue = null)
		{
			this.targetType = targetType;
			this.maxItems = maxItems;
			this.nextValue = nextValue;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000C80B File Offset: 0x0000AA0B
		public TargetType TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0000C813 File Offset: 0x0000AA13
		public Maybe<int> MaxItems
		{
			get
			{
				return this.maxItems;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000C81B File Offset: 0x0000AA1B
		public Maybe<TypeDescriptor> NextValue
		{
			get
			{
				return this.nextValue;
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000C823 File Offset: 0x0000AA23
		public static TypeDescriptor Create(TargetType tag, Maybe<int> maximumItems, TypeDescriptor next = default(TypeDescriptor))
		{
			if (maximumItems == null)
			{
				throw new ArgumentNullException("maximumItems");
			}
			return new TypeDescriptor(tag, maximumItems, next.ToMaybe<TypeDescriptor>());
		}

		// Token: 0x040000E8 RID: 232
		private readonly TargetType targetType;

		// Token: 0x040000E9 RID: 233
		private readonly Maybe<int> maxItems;

		// Token: 0x040000EA RID: 234
		private readonly Maybe<TypeDescriptor> nextValue;
	}
}
