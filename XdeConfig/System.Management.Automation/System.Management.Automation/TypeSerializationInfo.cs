using System;

namespace System.Management.Automation
{
	// Token: 0x02000458 RID: 1112
	internal class TypeSerializationInfo
	{
		// Token: 0x0600309A RID: 12442 RVA: 0x00109C63 File Offset: 0x00107E63
		internal TypeSerializationInfo(Type type, string itemTag, string propertyTag, TypeSerializerDelegate serializer, TypeDeserializerDelegate deserializer)
		{
			this._type = type;
			this._serializer = serializer;
			this._deserializer = deserializer;
			this._itemTag = itemTag;
			this._propertyTag = propertyTag;
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x0600309B RID: 12443 RVA: 0x00109C90 File Offset: 0x00107E90
		internal Type Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x0600309C RID: 12444 RVA: 0x00109C98 File Offset: 0x00107E98
		internal string ItemTag
		{
			get
			{
				return this._itemTag;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x00109CA0 File Offset: 0x00107EA0
		internal string PropertyTag
		{
			get
			{
				return this._propertyTag;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x00109CA8 File Offset: 0x00107EA8
		internal TypeSerializerDelegate Serializer
		{
			get
			{
				return this._serializer;
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x0600309F RID: 12447 RVA: 0x00109CB0 File Offset: 0x00107EB0
		internal TypeDeserializerDelegate Deserializer
		{
			get
			{
				return this._deserializer;
			}
		}

		// Token: 0x04001A29 RID: 6697
		private readonly Type _type;

		// Token: 0x04001A2A RID: 6698
		private readonly string _itemTag;

		// Token: 0x04001A2B RID: 6699
		private readonly string _propertyTag;

		// Token: 0x04001A2C RID: 6700
		private readonly TypeSerializerDelegate _serializer;

		// Token: 0x04001A2D RID: 6701
		private readonly TypeDeserializerDelegate _deserializer;
	}
}
