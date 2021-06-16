using System;
using System.Reflection;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200001D RID: 29
	internal static class EnumHelper<UnderlyingType>
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00009CE5 File Offset: 0x00007EE5
		public static UnderlyingType Cast<ValueType>(ValueType value)
		{
			return EnumHelper<UnderlyingType>.Caster<ValueType>.Instance(value);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00009CF2 File Offset: 0x00007EF2
		internal static UnderlyingType Identity(UnderlyingType value)
		{
			return value;
		}

		// Token: 0x04000095 RID: 149
		private static readonly MethodInfo IdentityInfo = Statics.GetDeclaredStaticMethod(typeof(EnumHelper<UnderlyingType>), "Identity");

		// Token: 0x0200001E RID: 30
		// (Invoke) Token: 0x0600011C RID: 284
		private delegate UnderlyingType Transformer<ValueType>(ValueType value);

		// Token: 0x0200001F RID: 31
		private static class Caster<ValueType>
		{
			// Token: 0x04000096 RID: 150
			public static readonly EnumHelper<UnderlyingType>.Transformer<ValueType> Instance = (EnumHelper<UnderlyingType>.Transformer<ValueType>)Statics.CreateDelegate(typeof(EnumHelper<UnderlyingType>.Transformer<ValueType>), EnumHelper<UnderlyingType>.IdentityInfo);
		}
	}
}
