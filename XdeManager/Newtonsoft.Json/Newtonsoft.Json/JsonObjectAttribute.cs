using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200001C RID: 28
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonObjectAttribute : JsonContainerAttribute
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003008 File Offset: 0x00001208
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00003010 File Offset: 0x00001210
		public MemberSerialization MemberSerialization
		{
			get
			{
				return this._memberSerialization;
			}
			set
			{
				this._memberSerialization = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000301C File Offset: 0x0000121C
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00003042 File Offset: 0x00001242
		public MissingMemberHandling MissingMemberHandling
		{
			get
			{
				MissingMemberHandling? missingMemberHandling = this._missingMemberHandling;
				if (missingMemberHandling == null)
				{
					return MissingMemberHandling.Ignore;
				}
				return missingMemberHandling.GetValueOrDefault();
			}
			set
			{
				this._missingMemberHandling = new MissingMemberHandling?(value);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00003050 File Offset: 0x00001250
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003076 File Offset: 0x00001276
		public NullValueHandling ItemNullValueHandling
		{
			get
			{
				NullValueHandling? itemNullValueHandling = this._itemNullValueHandling;
				if (itemNullValueHandling == null)
				{
					return NullValueHandling.Include;
				}
				return itemNullValueHandling.GetValueOrDefault();
			}
			set
			{
				this._itemNullValueHandling = new NullValueHandling?(value);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003084 File Offset: 0x00001284
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000030AA File Offset: 0x000012AA
		public Required ItemRequired
		{
			get
			{
				Required? itemRequired = this._itemRequired;
				if (itemRequired == null)
				{
					return Required.Default;
				}
				return itemRequired.GetValueOrDefault();
			}
			set
			{
				this._itemRequired = new Required?(value);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000030B8 File Offset: 0x000012B8
		public JsonObjectAttribute()
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000030C0 File Offset: 0x000012C0
		public JsonObjectAttribute(MemberSerialization memberSerialization)
		{
			this.MemberSerialization = memberSerialization;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000030CF File Offset: 0x000012CF
		public JsonObjectAttribute(string id) : base(id)
		{
		}

		// Token: 0x0400003C RID: 60
		private MemberSerialization _memberSerialization;

		// Token: 0x0400003D RID: 61
		internal MissingMemberHandling? _missingMemberHandling;

		// Token: 0x0400003E RID: 62
		internal Required? _itemRequired;

		// Token: 0x0400003F RID: 63
		internal NullValueHandling? _itemNullValueHandling;
	}
}
