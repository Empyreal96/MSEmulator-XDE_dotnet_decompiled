using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200001F RID: 31
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonPropertyAttribute : Attribute
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000338A File Offset: 0x0000158A
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003392 File Offset: 0x00001592
		public Type ItemConverterType { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000339B File Offset: 0x0000159B
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000033A3 File Offset: 0x000015A3
		public object[] ItemConverterParameters { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000033AC File Offset: 0x000015AC
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000033B4 File Offset: 0x000015B4
		public Type NamingStrategyType { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000033BD File Offset: 0x000015BD
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000033C5 File Offset: 0x000015C5
		public object[] NamingStrategyParameters { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000033D0 File Offset: 0x000015D0
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x000033F6 File Offset: 0x000015F6
		public NullValueHandling NullValueHandling
		{
			get
			{
				NullValueHandling? nullValueHandling = this._nullValueHandling;
				if (nullValueHandling == null)
				{
					return NullValueHandling.Include;
				}
				return nullValueHandling.GetValueOrDefault();
			}
			set
			{
				this._nullValueHandling = new NullValueHandling?(value);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003404 File Offset: 0x00001604
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x0000342A File Offset: 0x0000162A
		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				DefaultValueHandling? defaultValueHandling = this._defaultValueHandling;
				if (defaultValueHandling == null)
				{
					return DefaultValueHandling.Include;
				}
				return defaultValueHandling.GetValueOrDefault();
			}
			set
			{
				this._defaultValueHandling = new DefaultValueHandling?(value);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003438 File Offset: 0x00001638
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x0000345E File Offset: 0x0000165E
		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				ReferenceLoopHandling? referenceLoopHandling = this._referenceLoopHandling;
				if (referenceLoopHandling == null)
				{
					return ReferenceLoopHandling.Error;
				}
				return referenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._referenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000346C File Offset: 0x0000166C
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003492 File Offset: 0x00001692
		public ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				ObjectCreationHandling? objectCreationHandling = this._objectCreationHandling;
				if (objectCreationHandling == null)
				{
					return ObjectCreationHandling.Auto;
				}
				return objectCreationHandling.GetValueOrDefault();
			}
			set
			{
				this._objectCreationHandling = new ObjectCreationHandling?(value);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000034A0 File Offset: 0x000016A0
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000034C6 File Offset: 0x000016C6
		public TypeNameHandling TypeNameHandling
		{
			get
			{
				TypeNameHandling? typeNameHandling = this._typeNameHandling;
				if (typeNameHandling == null)
				{
					return TypeNameHandling.None;
				}
				return typeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._typeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000034D4 File Offset: 0x000016D4
		// (set) Token: 0x060000BC RID: 188 RVA: 0x000034FA File Offset: 0x000016FA
		public bool IsReference
		{
			get
			{
				return this._isReference ?? false;
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003508 File Offset: 0x00001708
		// (set) Token: 0x060000BE RID: 190 RVA: 0x0000352E File Offset: 0x0000172E
		public int Order
		{
			get
			{
				int? order = this._order;
				if (order == null)
				{
					return 0;
				}
				return order.GetValueOrDefault();
			}
			set
			{
				this._order = new int?(value);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BF RID: 191 RVA: 0x0000353C File Offset: 0x0000173C
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00003562 File Offset: 0x00001762
		public Required Required
		{
			get
			{
				Required? required = this._required;
				if (required == null)
				{
					return Required.Default;
				}
				return required.GetValueOrDefault();
			}
			set
			{
				this._required = new Required?(value);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003570 File Offset: 0x00001770
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00003578 File Offset: 0x00001778
		public string PropertyName { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003584 File Offset: 0x00001784
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x000035AA File Offset: 0x000017AA
		public ReferenceLoopHandling ItemReferenceLoopHandling
		{
			get
			{
				ReferenceLoopHandling? itemReferenceLoopHandling = this._itemReferenceLoopHandling;
				if (itemReferenceLoopHandling == null)
				{
					return ReferenceLoopHandling.Error;
				}
				return itemReferenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000035B8 File Offset: 0x000017B8
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x000035DE File Offset: 0x000017DE
		public TypeNameHandling ItemTypeNameHandling
		{
			get
			{
				TypeNameHandling? itemTypeNameHandling = this._itemTypeNameHandling;
				if (itemTypeNameHandling == null)
				{
					return TypeNameHandling.None;
				}
				return itemTypeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._itemTypeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x000035EC File Offset: 0x000017EC
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00003612 File Offset: 0x00001812
		public bool ItemIsReference
		{
			get
			{
				return this._itemIsReference ?? false;
			}
			set
			{
				this._itemIsReference = new bool?(value);
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003620 File Offset: 0x00001820
		public JsonPropertyAttribute()
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003628 File Offset: 0x00001828
		public JsonPropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x0400004A RID: 74
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x0400004B RID: 75
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x0400004C RID: 76
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x0400004D RID: 77
		internal ObjectCreationHandling? _objectCreationHandling;

		// Token: 0x0400004E RID: 78
		internal TypeNameHandling? _typeNameHandling;

		// Token: 0x0400004F RID: 79
		internal bool? _isReference;

		// Token: 0x04000050 RID: 80
		internal int? _order;

		// Token: 0x04000051 RID: 81
		internal Required? _required;

		// Token: 0x04000052 RID: 82
		internal bool? _itemIsReference;

		// Token: 0x04000053 RID: 83
		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		// Token: 0x04000054 RID: 84
		internal TypeNameHandling? _itemTypeNameHandling;
	}
}
