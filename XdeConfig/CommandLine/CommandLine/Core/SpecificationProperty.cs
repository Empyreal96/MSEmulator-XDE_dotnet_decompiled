using System;
using System.Reflection;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000079 RID: 121
	internal class SpecificationProperty
	{
		// Token: 0x060002EC RID: 748 RVA: 0x0000BD43 File Offset: 0x00009F43
		private SpecificationProperty(Specification specification, PropertyInfo property, Maybe<object> value)
		{
			this.property = property;
			this.specification = specification;
			this.value = value;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000BD60 File Offset: 0x00009F60
		public static SpecificationProperty Create(Specification specification, PropertyInfo property, Maybe<object> value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return new SpecificationProperty(specification, property, value);
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000BD78 File Offset: 0x00009F78
		public Specification Specification
		{
			get
			{
				return this.specification;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000BD80 File Offset: 0x00009F80
		public PropertyInfo Property
		{
			get
			{
				return this.property;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000BD88 File Offset: 0x00009F88
		public Maybe<object> Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x040000DF RID: 223
		private readonly Specification specification;

		// Token: 0x040000E0 RID: 224
		private readonly PropertyInfo property;

		// Token: 0x040000E1 RID: 225
		private readonly Maybe<object> value;
	}
}
