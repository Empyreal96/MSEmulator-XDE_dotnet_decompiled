using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000090 RID: 144
	internal class JsonSerializerProxy : JsonSerializer
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060007AA RID: 1962 RVA: 0x00023259 File Offset: 0x00021459
		// (remove) Token: 0x060007AB RID: 1963 RVA: 0x00023267 File Offset: 0x00021467
		public override event EventHandler<ErrorEventArgs> Error
		{
			add
			{
				this._serializer.Error += value;
			}
			remove
			{
				this._serializer.Error -= value;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00023275 File Offset: 0x00021475
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x00023282 File Offset: 0x00021482
		public override IReferenceResolver ReferenceResolver
		{
			get
			{
				return this._serializer.ReferenceResolver;
			}
			set
			{
				this._serializer.ReferenceResolver = value;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x00023290 File Offset: 0x00021490
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x0002329D File Offset: 0x0002149D
		public override ITraceWriter TraceWriter
		{
			get
			{
				return this._serializer.TraceWriter;
			}
			set
			{
				this._serializer.TraceWriter = value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000232AB File Offset: 0x000214AB
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x000232B8 File Offset: 0x000214B8
		public override IEqualityComparer EqualityComparer
		{
			get
			{
				return this._serializer.EqualityComparer;
			}
			set
			{
				this._serializer.EqualityComparer = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x000232C6 File Offset: 0x000214C6
		public override JsonConverterCollection Converters
		{
			get
			{
				return this._serializer.Converters;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x000232D3 File Offset: 0x000214D3
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x000232E0 File Offset: 0x000214E0
		public override DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._serializer.DefaultValueHandling;
			}
			set
			{
				this._serializer.DefaultValueHandling = value;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x000232EE File Offset: 0x000214EE
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x000232FB File Offset: 0x000214FB
		public override IContractResolver ContractResolver
		{
			get
			{
				return this._serializer.ContractResolver;
			}
			set
			{
				this._serializer.ContractResolver = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00023309 File Offset: 0x00021509
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x00023316 File Offset: 0x00021516
		public override MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._serializer.MissingMemberHandling;
			}
			set
			{
				this._serializer.MissingMemberHandling = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00023324 File Offset: 0x00021524
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x00023331 File Offset: 0x00021531
		public override NullValueHandling NullValueHandling
		{
			get
			{
				return this._serializer.NullValueHandling;
			}
			set
			{
				this._serializer.NullValueHandling = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0002333F File Offset: 0x0002153F
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x0002334C File Offset: 0x0002154C
		public override ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._serializer.ObjectCreationHandling;
			}
			set
			{
				this._serializer.ObjectCreationHandling = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0002335A File Offset: 0x0002155A
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x00023367 File Offset: 0x00021567
		public override ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._serializer.ReferenceLoopHandling;
			}
			set
			{
				this._serializer.ReferenceLoopHandling = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00023375 File Offset: 0x00021575
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x00023382 File Offset: 0x00021582
		public override PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._serializer.PreserveReferencesHandling;
			}
			set
			{
				this._serializer.PreserveReferencesHandling = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00023390 File Offset: 0x00021590
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x0002339D File Offset: 0x0002159D
		public override TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._serializer.TypeNameHandling;
			}
			set
			{
				this._serializer.TypeNameHandling = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x000233AB File Offset: 0x000215AB
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x000233B8 File Offset: 0x000215B8
		public override MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._serializer.MetadataPropertyHandling;
			}
			set
			{
				this._serializer.MetadataPropertyHandling = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x000233C6 File Offset: 0x000215C6
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x000233D3 File Offset: 0x000215D3
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public override FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormat;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormat = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x000233E1 File Offset: 0x000215E1
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x000233EE File Offset: 0x000215EE
		public override TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormatHandling;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormatHandling = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x000233FC File Offset: 0x000215FC
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x00023409 File Offset: 0x00021609
		public override ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._serializer.ConstructorHandling;
			}
			set
			{
				this._serializer.ConstructorHandling = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00023417 File Offset: 0x00021617
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x00023424 File Offset: 0x00021624
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public override SerializationBinder Binder
		{
			get
			{
				return this._serializer.Binder;
			}
			set
			{
				this._serializer.Binder = value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00023432 File Offset: 0x00021632
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x0002343F File Offset: 0x0002163F
		public override ISerializationBinder SerializationBinder
		{
			get
			{
				return this._serializer.SerializationBinder;
			}
			set
			{
				this._serializer.SerializationBinder = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x0002344D File Offset: 0x0002164D
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0002345A File Offset: 0x0002165A
		public override StreamingContext Context
		{
			get
			{
				return this._serializer.Context;
			}
			set
			{
				this._serializer.Context = value;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00023468 File Offset: 0x00021668
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x00023475 File Offset: 0x00021675
		public override Formatting Formatting
		{
			get
			{
				return this._serializer.Formatting;
			}
			set
			{
				this._serializer.Formatting = value;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00023483 File Offset: 0x00021683
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x00023490 File Offset: 0x00021690
		public override DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._serializer.DateFormatHandling;
			}
			set
			{
				this._serializer.DateFormatHandling = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0002349E File Offset: 0x0002169E
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x000234AB File Offset: 0x000216AB
		public override DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._serializer.DateTimeZoneHandling;
			}
			set
			{
				this._serializer.DateTimeZoneHandling = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x000234B9 File Offset: 0x000216B9
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x000234C6 File Offset: 0x000216C6
		public override DateParseHandling DateParseHandling
		{
			get
			{
				return this._serializer.DateParseHandling;
			}
			set
			{
				this._serializer.DateParseHandling = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x000234D4 File Offset: 0x000216D4
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x000234E1 File Offset: 0x000216E1
		public override FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._serializer.FloatFormatHandling;
			}
			set
			{
				this._serializer.FloatFormatHandling = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x000234EF File Offset: 0x000216EF
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x000234FC File Offset: 0x000216FC
		public override FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._serializer.FloatParseHandling;
			}
			set
			{
				this._serializer.FloatParseHandling = value;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002350A File Offset: 0x0002170A
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x00023517 File Offset: 0x00021717
		public override StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._serializer.StringEscapeHandling;
			}
			set
			{
				this._serializer.StringEscapeHandling = value;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00023525 File Offset: 0x00021725
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x00023532 File Offset: 0x00021732
		public override string DateFormatString
		{
			get
			{
				return this._serializer.DateFormatString;
			}
			set
			{
				this._serializer.DateFormatString = value;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00023540 File Offset: 0x00021740
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0002354D File Offset: 0x0002174D
		public override CultureInfo Culture
		{
			get
			{
				return this._serializer.Culture;
			}
			set
			{
				this._serializer.Culture = value;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0002355B File Offset: 0x0002175B
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x00023568 File Offset: 0x00021768
		public override int? MaxDepth
		{
			get
			{
				return this._serializer.MaxDepth;
			}
			set
			{
				this._serializer.MaxDepth = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00023576 File Offset: 0x00021776
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x00023583 File Offset: 0x00021783
		public override bool CheckAdditionalContent
		{
			get
			{
				return this._serializer.CheckAdditionalContent;
			}
			set
			{
				this._serializer.CheckAdditionalContent = value;
			}
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00023591 File Offset: 0x00021791
		internal JsonSerializerInternalBase GetInternalSerializer()
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader;
			}
			return this._serializerWriter;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000235A8 File Offset: 0x000217A8
		public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
		{
			ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
			this._serializerReader = serializerReader;
			this._serializer = serializerReader.Serializer;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x000235CE File Offset: 0x000217CE
		public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
		{
			ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
			this._serializerWriter = serializerWriter;
			this._serializer = serializerWriter.Serializer;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000235F4 File Offset: 0x000217F4
		internal override object DeserializeInternal(JsonReader reader, Type objectType)
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader.Deserialize(reader, objectType, false);
			}
			return this._serializer.Deserialize(reader, objectType);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002361A File Offset: 0x0002181A
		internal override void PopulateInternal(JsonReader reader, object target)
		{
			if (this._serializerReader != null)
			{
				this._serializerReader.Populate(reader, target);
				return;
			}
			this._serializer.Populate(reader, target);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0002363F File Offset: 0x0002183F
		internal override void SerializeInternal(JsonWriter jsonWriter, object value, Type rootType)
		{
			if (this._serializerWriter != null)
			{
				this._serializerWriter.Serialize(jsonWriter, value, rootType);
				return;
			}
			this._serializer.Serialize(jsonWriter, value);
		}

		// Token: 0x040002AD RID: 685
		private readonly JsonSerializerInternalReader _serializerReader;

		// Token: 0x040002AE RID: 686
		private readonly JsonSerializerInternalWriter _serializerWriter;

		// Token: 0x040002AF RID: 687
		private readonly JsonSerializer _serializer;
	}
}
