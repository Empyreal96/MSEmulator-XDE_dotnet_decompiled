using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000025 RID: 37
	public class JsonSerializerSettings
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00005F3C File Offset: 0x0000413C
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00005F62 File Offset: 0x00004162
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00005F70 File Offset: 0x00004170
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00005F96 File Offset: 0x00004196
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

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00005FA4 File Offset: 0x000041A4
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00005FCA File Offset: 0x000041CA
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00005FD8 File Offset: 0x000041D8
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00005FFE File Offset: 0x000041FE
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

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000600C File Offset: 0x0000420C
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00006032 File Offset: 0x00004232
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

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00006040 File Offset: 0x00004240
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00006048 File Offset: 0x00004248
		public IList<JsonConverter> Converters { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00006054 File Offset: 0x00004254
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000607A File Offset: 0x0000427A
		public PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				PreserveReferencesHandling? preserveReferencesHandling = this._preserveReferencesHandling;
				if (preserveReferencesHandling == null)
				{
					return PreserveReferencesHandling.None;
				}
				return preserveReferencesHandling.GetValueOrDefault();
			}
			set
			{
				this._preserveReferencesHandling = new PreserveReferencesHandling?(value);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00006088 File Offset: 0x00004288
		// (set) Token: 0x0600019C RID: 412 RVA: 0x000060AE File Offset: 0x000042AE
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

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000060BC File Offset: 0x000042BC
		// (set) Token: 0x0600019E RID: 414 RVA: 0x000060E2 File Offset: 0x000042E2
		public MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				MetadataPropertyHandling? metadataPropertyHandling = this._metadataPropertyHandling;
				if (metadataPropertyHandling == null)
				{
					return MetadataPropertyHandling.Default;
				}
				return metadataPropertyHandling.GetValueOrDefault();
			}
			set
			{
				this._metadataPropertyHandling = new MetadataPropertyHandling?(value);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000060F0 File Offset: 0x000042F0
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000060F8 File Offset: 0x000042F8
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return (FormatterAssemblyStyle)this.TypeNameAssemblyFormatHandling;
			}
			set
			{
				this.TypeNameAssemblyFormatHandling = (TypeNameAssemblyFormatHandling)value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00006104 File Offset: 0x00004304
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x0000612A File Offset: 0x0000432A
		public TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				TypeNameAssemblyFormatHandling? typeNameAssemblyFormatHandling = this._typeNameAssemblyFormatHandling;
				if (typeNameAssemblyFormatHandling == null)
				{
					return TypeNameAssemblyFormatHandling.Simple;
				}
				return typeNameAssemblyFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._typeNameAssemblyFormatHandling = new TypeNameAssemblyFormatHandling?(value);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00006138 File Offset: 0x00004338
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x0000615E File Offset: 0x0000435E
		public ConstructorHandling ConstructorHandling
		{
			get
			{
				ConstructorHandling? constructorHandling = this._constructorHandling;
				if (constructorHandling == null)
				{
					return ConstructorHandling.Default;
				}
				return constructorHandling.GetValueOrDefault();
			}
			set
			{
				this._constructorHandling = new ConstructorHandling?(value);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000616C File Offset: 0x0000436C
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00006174 File Offset: 0x00004374
		public IContractResolver ContractResolver { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000617D File Offset: 0x0000437D
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00006185 File Offset: 0x00004385
		public IEqualityComparer EqualityComparer { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000618E File Offset: 0x0000438E
		// (set) Token: 0x060001AA RID: 426 RVA: 0x000061A4 File Offset: 0x000043A4
		[Obsolete("ReferenceResolver property is obsolete. Use the ReferenceResolverProvider property to set the IReferenceResolver: settings.ReferenceResolverProvider = () => resolver")]
		public IReferenceResolver ReferenceResolver
		{
			get
			{
				Func<IReferenceResolver> referenceResolverProvider = this.ReferenceResolverProvider;
				if (referenceResolverProvider == null)
				{
					return null;
				}
				return referenceResolverProvider();
			}
			set
			{
				this.ReferenceResolverProvider = ((value != null) ? (() => value) : null);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000061DB File Offset: 0x000043DB
		// (set) Token: 0x060001AC RID: 428 RVA: 0x000061E3 File Offset: 0x000043E3
		public Func<IReferenceResolver> ReferenceResolverProvider { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000061EC File Offset: 0x000043EC
		// (set) Token: 0x060001AE RID: 430 RVA: 0x000061F4 File Offset: 0x000043F4
		public ITraceWriter TraceWriter { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00006200 File Offset: 0x00004400
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00006237 File Offset: 0x00004437
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public SerializationBinder Binder
		{
			get
			{
				if (this.SerializationBinder == null)
				{
					return null;
				}
				SerializationBinderAdapter serializationBinderAdapter;
				if ((serializationBinderAdapter = (this.SerializationBinder as SerializationBinderAdapter)) != null)
				{
					return serializationBinderAdapter.SerializationBinder;
				}
				throw new InvalidOperationException("Cannot get SerializationBinder because an ISerializationBinder was previously set.");
			}
			set
			{
				this.SerializationBinder = ((value == null) ? null : new SerializationBinderAdapter(value));
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000624B File Offset: 0x0000444B
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00006253 File Offset: 0x00004453
		public ISerializationBinder SerializationBinder { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000625C File Offset: 0x0000445C
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00006264 File Offset: 0x00004464
		public EventHandler<ErrorEventArgs> Error { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00006270 File Offset: 0x00004470
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x0000629A File Offset: 0x0000449A
		public StreamingContext Context
		{
			get
			{
				StreamingContext? context = this._context;
				if (context == null)
				{
					return JsonSerializerSettings.DefaultContext;
				}
				return context.GetValueOrDefault();
			}
			set
			{
				this._context = new StreamingContext?(value);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000062A8 File Offset: 0x000044A8
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x000062B9 File Offset: 0x000044B9
		public string DateFormatString
		{
			get
			{
				return this._dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
			}
			set
			{
				this._dateFormatString = value;
				this._dateFormatStringSet = true;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x000062C9 File Offset: 0x000044C9
		// (set) Token: 0x060001BA RID: 442 RVA: 0x000062D4 File Offset: 0x000044D4
		public int? MaxDepth
		{
			get
			{
				return this._maxDepth;
			}
			set
			{
				int? num = value;
				int num2 = 0;
				if (num.GetValueOrDefault() <= num2 & num != null)
				{
					throw new ArgumentException("Value must be positive.", "value");
				}
				this._maxDepth = value;
				this._maxDepthSet = true;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000631C File Offset: 0x0000451C
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00006342 File Offset: 0x00004542
		public Formatting Formatting
		{
			get
			{
				Formatting? formatting = this._formatting;
				if (formatting == null)
				{
					return Formatting.None;
				}
				return formatting.GetValueOrDefault();
			}
			set
			{
				this._formatting = new Formatting?(value);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00006350 File Offset: 0x00004550
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00006376 File Offset: 0x00004576
		public DateFormatHandling DateFormatHandling
		{
			get
			{
				DateFormatHandling? dateFormatHandling = this._dateFormatHandling;
				if (dateFormatHandling == null)
				{
					return DateFormatHandling.IsoDateFormat;
				}
				return dateFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._dateFormatHandling = new DateFormatHandling?(value);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00006384 File Offset: 0x00004584
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x000063AA File Offset: 0x000045AA
		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				DateTimeZoneHandling? dateTimeZoneHandling = this._dateTimeZoneHandling;
				if (dateTimeZoneHandling == null)
				{
					return DateTimeZoneHandling.RoundtripKind;
				}
				return dateTimeZoneHandling.GetValueOrDefault();
			}
			set
			{
				this._dateTimeZoneHandling = new DateTimeZoneHandling?(value);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000063B8 File Offset: 0x000045B8
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x000063DE File Offset: 0x000045DE
		public DateParseHandling DateParseHandling
		{
			get
			{
				DateParseHandling? dateParseHandling = this._dateParseHandling;
				if (dateParseHandling == null)
				{
					return DateParseHandling.DateTime;
				}
				return dateParseHandling.GetValueOrDefault();
			}
			set
			{
				this._dateParseHandling = new DateParseHandling?(value);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000063EC File Offset: 0x000045EC
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00006412 File Offset: 0x00004612
		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				FloatFormatHandling? floatFormatHandling = this._floatFormatHandling;
				if (floatFormatHandling == null)
				{
					return FloatFormatHandling.String;
				}
				return floatFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._floatFormatHandling = new FloatFormatHandling?(value);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00006420 File Offset: 0x00004620
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00006446 File Offset: 0x00004646
		public FloatParseHandling FloatParseHandling
		{
			get
			{
				FloatParseHandling? floatParseHandling = this._floatParseHandling;
				if (floatParseHandling == null)
				{
					return FloatParseHandling.Double;
				}
				return floatParseHandling.GetValueOrDefault();
			}
			set
			{
				this._floatParseHandling = new FloatParseHandling?(value);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00006454 File Offset: 0x00004654
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x0000647A File Offset: 0x0000467A
		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				StringEscapeHandling? stringEscapeHandling = this._stringEscapeHandling;
				if (stringEscapeHandling == null)
				{
					return StringEscapeHandling.Default;
				}
				return stringEscapeHandling.GetValueOrDefault();
			}
			set
			{
				this._stringEscapeHandling = new StringEscapeHandling?(value);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00006488 File Offset: 0x00004688
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00006499 File Offset: 0x00004699
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? JsonSerializerSettings.DefaultCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000064A4 File Offset: 0x000046A4
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000064CA File Offset: 0x000046CA
		public bool CheckAdditionalContent
		{
			get
			{
				return this._checkAdditionalContent ?? false;
			}
			set
			{
				this._checkAdditionalContent = new bool?(value);
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000064EF File Offset: 0x000046EF
		[DebuggerStepThrough]
		public JsonSerializerSettings()
		{
			this.Converters = new List<JsonConverter>();
		}

		// Token: 0x0400008E RID: 142
		internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;

		// Token: 0x0400008F RID: 143
		internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;

		// Token: 0x04000090 RID: 144
		internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;

		// Token: 0x04000091 RID: 145
		internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;

		// Token: 0x04000092 RID: 146
		internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;

		// Token: 0x04000093 RID: 147
		internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;

		// Token: 0x04000094 RID: 148
		internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;

		// Token: 0x04000095 RID: 149
		internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;

		// Token: 0x04000096 RID: 150
		internal const MetadataPropertyHandling DefaultMetadataPropertyHandling = MetadataPropertyHandling.Default;

		// Token: 0x04000097 RID: 151
		internal static readonly StreamingContext DefaultContext = default(StreamingContext);

		// Token: 0x04000098 RID: 152
		internal const Formatting DefaultFormatting = Formatting.None;

		// Token: 0x04000099 RID: 153
		internal const DateFormatHandling DefaultDateFormatHandling = DateFormatHandling.IsoDateFormat;

		// Token: 0x0400009A RID: 154
		internal const DateTimeZoneHandling DefaultDateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;

		// Token: 0x0400009B RID: 155
		internal const DateParseHandling DefaultDateParseHandling = DateParseHandling.DateTime;

		// Token: 0x0400009C RID: 156
		internal const FloatParseHandling DefaultFloatParseHandling = FloatParseHandling.Double;

		// Token: 0x0400009D RID: 157
		internal const FloatFormatHandling DefaultFloatFormatHandling = FloatFormatHandling.String;

		// Token: 0x0400009E RID: 158
		internal const StringEscapeHandling DefaultStringEscapeHandling = StringEscapeHandling.Default;

		// Token: 0x0400009F RID: 159
		internal const TypeNameAssemblyFormatHandling DefaultTypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;

		// Token: 0x040000A0 RID: 160
		internal static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;

		// Token: 0x040000A1 RID: 161
		internal const bool DefaultCheckAdditionalContent = false;

		// Token: 0x040000A2 RID: 162
		internal const string DefaultDateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		// Token: 0x040000A3 RID: 163
		internal Formatting? _formatting;

		// Token: 0x040000A4 RID: 164
		internal DateFormatHandling? _dateFormatHandling;

		// Token: 0x040000A5 RID: 165
		internal DateTimeZoneHandling? _dateTimeZoneHandling;

		// Token: 0x040000A6 RID: 166
		internal DateParseHandling? _dateParseHandling;

		// Token: 0x040000A7 RID: 167
		internal FloatFormatHandling? _floatFormatHandling;

		// Token: 0x040000A8 RID: 168
		internal FloatParseHandling? _floatParseHandling;

		// Token: 0x040000A9 RID: 169
		internal StringEscapeHandling? _stringEscapeHandling;

		// Token: 0x040000AA RID: 170
		internal CultureInfo _culture;

		// Token: 0x040000AB RID: 171
		internal bool? _checkAdditionalContent;

		// Token: 0x040000AC RID: 172
		internal int? _maxDepth;

		// Token: 0x040000AD RID: 173
		internal bool _maxDepthSet;

		// Token: 0x040000AE RID: 174
		internal string _dateFormatString;

		// Token: 0x040000AF RID: 175
		internal bool _dateFormatStringSet;

		// Token: 0x040000B0 RID: 176
		internal TypeNameAssemblyFormatHandling? _typeNameAssemblyFormatHandling;

		// Token: 0x040000B1 RID: 177
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x040000B2 RID: 178
		internal PreserveReferencesHandling? _preserveReferencesHandling;

		// Token: 0x040000B3 RID: 179
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x040000B4 RID: 180
		internal ObjectCreationHandling? _objectCreationHandling;

		// Token: 0x040000B5 RID: 181
		internal MissingMemberHandling? _missingMemberHandling;

		// Token: 0x040000B6 RID: 182
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x040000B7 RID: 183
		internal StreamingContext? _context;

		// Token: 0x040000B8 RID: 184
		internal ConstructorHandling? _constructorHandling;

		// Token: 0x040000B9 RID: 185
		internal TypeNameHandling? _typeNameHandling;

		// Token: 0x040000BA RID: 186
		internal MetadataPropertyHandling? _metadataPropertyHandling;
	}
}
