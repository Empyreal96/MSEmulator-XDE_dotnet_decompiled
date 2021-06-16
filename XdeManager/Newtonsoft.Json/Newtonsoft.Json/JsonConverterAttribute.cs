using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000015 RID: 21
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonConverterAttribute : Attribute
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00002F2D File Offset: 0x0000112D
		public Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002F35 File Offset: 0x00001135
		public object[] ConverterParameters { get; }

		// Token: 0x06000085 RID: 133 RVA: 0x00002F3D File Offset: 0x0000113D
		public JsonConverterAttribute(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this._converterType = converterType;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002F60 File Offset: 0x00001160
		public JsonConverterAttribute(Type converterType, params object[] converterParameters) : this(converterType)
		{
			this.ConverterParameters = converterParameters;
		}

		// Token: 0x04000038 RID: 56
		private readonly Type _converterType;
	}
}
