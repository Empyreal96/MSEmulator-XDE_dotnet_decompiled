using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000089 RID: 137
	public class JsonObjectContract : JsonContainerContract
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001D37E File Offset: 0x0001B57E
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0001D386 File Offset: 0x0001B586
		public MemberSerialization MemberSerialization { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001D38F File Offset: 0x0001B58F
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0001D397 File Offset: 0x0001B597
		public MissingMemberHandling? MissingMemberHandling { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001D3A0 File Offset: 0x0001B5A0
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0001D3A8 File Offset: 0x0001B5A8
		public Required? ItemRequired { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0001D3B1 File Offset: 0x0001B5B1
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0001D3B9 File Offset: 0x0001B5B9
		public NullValueHandling? ItemNullValueHandling { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0001D3C2 File Offset: 0x0001B5C2
		public JsonPropertyCollection Properties { get; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001D3CA File Offset: 0x0001B5CA
		public JsonPropertyCollection CreatorParameters
		{
			get
			{
				if (this._creatorParameters == null)
				{
					this._creatorParameters = new JsonPropertyCollection(base.UnderlyingType);
				}
				return this._creatorParameters;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0001D3EB File Offset: 0x0001B5EB
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x0001D3F3 File Offset: 0x0001B5F3
		public ObjectConstructor<object> OverrideCreator
		{
			get
			{
				return this._overrideCreator;
			}
			set
			{
				this._overrideCreator = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0001D3FC File Offset: 0x0001B5FC
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0001D404 File Offset: 0x0001B604
		internal ObjectConstructor<object> ParameterizedCreator
		{
			get
			{
				return this._parameterizedCreator;
			}
			set
			{
				this._parameterizedCreator = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0001D40D File Offset: 0x0001B60D
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0001D415 File Offset: 0x0001B615
		public ExtensionDataSetter ExtensionDataSetter { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001D41E File Offset: 0x0001B61E
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x0001D426 File Offset: 0x0001B626
		public ExtensionDataGetter ExtensionDataGetter { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0001D42F File Offset: 0x0001B62F
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0001D437 File Offset: 0x0001B637
		public Type ExtensionDataValueType
		{
			get
			{
				return this._extensionDataValueType;
			}
			set
			{
				this._extensionDataValueType = value;
				this.ExtensionDataIsJToken = (value != null && typeof(JToken).IsAssignableFrom(value));
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001D462 File Offset: 0x0001B662
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0001D46A File Offset: 0x0001B66A
		public Func<string, string> ExtensionDataNameResolver { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001D474 File Offset: 0x0001B674
		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				if (this._hasRequiredOrDefaultValueProperties == null)
				{
					this._hasRequiredOrDefaultValueProperties = new bool?(false);
					if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
					{
						this._hasRequiredOrDefaultValueProperties = new bool?(true);
					}
					else
					{
						foreach (JsonProperty jsonProperty in this.Properties)
						{
							if (jsonProperty.Required == Required.Default)
							{
								DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling & DefaultValueHandling.Populate;
								DefaultValueHandling defaultValueHandling2 = DefaultValueHandling.Populate;
								if (!(defaultValueHandling.GetValueOrDefault() == defaultValueHandling2 & defaultValueHandling != null))
								{
									continue;
								}
							}
							this._hasRequiredOrDefaultValueProperties = new bool?(true);
							break;
						}
					}
				}
				return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001D560 File Offset: 0x0001B760
		public JsonObjectContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Object;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001D581 File Offset: 0x0001B781
		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, this.NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
		}

		// Token: 0x0400027B RID: 635
		internal bool ExtensionDataIsJToken;

		// Token: 0x0400027C RID: 636
		private bool? _hasRequiredOrDefaultValueProperties;

		// Token: 0x0400027D RID: 637
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x0400027E RID: 638
		private ObjectConstructor<object> _parameterizedCreator;

		// Token: 0x0400027F RID: 639
		private JsonPropertyCollection _creatorParameters;

		// Token: 0x04000280 RID: 640
		private Type _extensionDataValueType;
	}
}
