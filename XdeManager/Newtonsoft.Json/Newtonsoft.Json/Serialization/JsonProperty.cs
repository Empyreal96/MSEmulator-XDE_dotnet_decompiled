using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008B RID: 139
	public class JsonProperty
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001D71C File Offset: 0x0001B91C
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0001D724 File Offset: 0x0001B924
		internal JsonContract PropertyContract { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0001D72D File Offset: 0x0001B92D
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0001D735 File Offset: 0x0001B935
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
			set
			{
				this._propertyName = value;
				this._skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(this._propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0001D757 File Offset: 0x0001B957
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0001D75F File Offset: 0x0001B95F
		public Type DeclaringType { get; set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001D768 File Offset: 0x0001B968
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0001D770 File Offset: 0x0001B970
		public int? Order { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001D779 File Offset: 0x0001B979
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0001D781 File Offset: 0x0001B981
		public string UnderlyingName { get; set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0001D78A File Offset: 0x0001B98A
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0001D792 File Offset: 0x0001B992
		public IValueProvider ValueProvider { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001D79B File Offset: 0x0001B99B
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0001D7A3 File Offset: 0x0001B9A3
		public IAttributeProvider AttributeProvider { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0001D7AC File Offset: 0x0001B9AC
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0001D7B4 File Offset: 0x0001B9B4
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
			set
			{
				if (this._propertyType != value)
				{
					this._propertyType = value;
					this._hasGeneratedDefaultValue = false;
				}
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0001D7D2 File Offset: 0x0001B9D2
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0001D7DA File Offset: 0x0001B9DA
		public JsonConverter Converter { get; set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0001D7E3 File Offset: 0x0001B9E3
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0001D7EB File Offset: 0x0001B9EB
		[Obsolete("MemberConverter is obsolete. Use Converter instead.")]
		public JsonConverter MemberConverter
		{
			get
			{
				return this.Converter;
			}
			set
			{
				this.Converter = value;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0001D7F4 File Offset: 0x0001B9F4
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0001D7FC File Offset: 0x0001B9FC
		public bool Ignored { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0001D805 File Offset: 0x0001BA05
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x0001D80D File Offset: 0x0001BA0D
		public bool Readable { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001D816 File Offset: 0x0001BA16
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x0001D81E File Offset: 0x0001BA1E
		public bool Writable { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0001D827 File Offset: 0x0001BA27
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0001D82F File Offset: 0x0001BA2F
		public bool HasMemberAttribute { get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0001D838 File Offset: 0x0001BA38
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x0001D84A File Offset: 0x0001BA4A
		public object DefaultValue
		{
			get
			{
				if (!this._hasExplicitDefaultValue)
				{
					return null;
				}
				return this._defaultValue;
			}
			set
			{
				this._hasExplicitDefaultValue = true;
				this._defaultValue = value;
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001D85A File Offset: 0x0001BA5A
		internal object GetResolvedDefaultValue()
		{
			if (this._propertyType == null)
			{
				return null;
			}
			if (!this._hasExplicitDefaultValue && !this._hasGeneratedDefaultValue)
			{
				this._defaultValue = ReflectionUtils.GetDefaultValue(this.PropertyType);
				this._hasGeneratedDefaultValue = true;
			}
			return this._defaultValue;
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001D89C File Offset: 0x0001BA9C
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001D8C2 File Offset: 0x0001BAC2
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

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001D8D0 File Offset: 0x0001BAD0
		public bool IsRequiredSpecified
		{
			get
			{
				return this._required != null;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001D8DD File Offset: 0x0001BADD
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0001D8E5 File Offset: 0x0001BAE5
		public bool? IsReference { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001D8EE File Offset: 0x0001BAEE
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0001D8F6 File Offset: 0x0001BAF6
		public NullValueHandling? NullValueHandling { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0001D8FF File Offset: 0x0001BAFF
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0001D907 File Offset: 0x0001BB07
		public DefaultValueHandling? DefaultValueHandling { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001D910 File Offset: 0x0001BB10
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x0001D918 File Offset: 0x0001BB18
		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0001D921 File Offset: 0x0001BB21
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0001D929 File Offset: 0x0001BB29
		public ObjectCreationHandling? ObjectCreationHandling { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0001D932 File Offset: 0x0001BB32
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0001D93A File Offset: 0x0001BB3A
		public TypeNameHandling? TypeNameHandling { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0001D943 File Offset: 0x0001BB43
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0001D94B File Offset: 0x0001BB4B
		public Predicate<object> ShouldSerialize { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0001D954 File Offset: 0x0001BB54
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0001D95C File Offset: 0x0001BB5C
		public Predicate<object> ShouldDeserialize { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0001D965 File Offset: 0x0001BB65
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x0001D96D File Offset: 0x0001BB6D
		public Predicate<object> GetIsSpecified { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0001D976 File Offset: 0x0001BB76
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x0001D97E File Offset: 0x0001BB7E
		public Action<object, object> SetIsSpecified { get; set; }

		// Token: 0x0600073D RID: 1853 RVA: 0x0001D987 File Offset: 0x0001BB87
		public override string ToString()
		{
			return this.PropertyName;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001D98F File Offset: 0x0001BB8F
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0001D997 File Offset: 0x0001BB97
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0001D9A0 File Offset: 0x0001BBA0
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x0001D9A8 File Offset: 0x0001BBA8
		public bool? ItemIsReference { get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0001D9B1 File Offset: 0x0001BBB1
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0001D9B9 File Offset: 0x0001BBB9
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0001D9C2 File Offset: 0x0001BBC2
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0001D9CA File Offset: 0x0001BBCA
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x06000746 RID: 1862 RVA: 0x0001D9D3 File Offset: 0x0001BBD3
		internal void WritePropertyName(JsonWriter writer)
		{
			if (this._skipPropertyNameEscape)
			{
				writer.WritePropertyName(this.PropertyName, false);
				return;
			}
			writer.WritePropertyName(this.PropertyName);
		}

		// Token: 0x04000283 RID: 643
		internal Required? _required;

		// Token: 0x04000284 RID: 644
		internal bool _hasExplicitDefaultValue;

		// Token: 0x04000285 RID: 645
		private object _defaultValue;

		// Token: 0x04000286 RID: 646
		private bool _hasGeneratedDefaultValue;

		// Token: 0x04000287 RID: 647
		private string _propertyName;

		// Token: 0x04000288 RID: 648
		internal bool _skipPropertyNameEscape;

		// Token: 0x04000289 RID: 649
		private Type _propertyType;
	}
}
