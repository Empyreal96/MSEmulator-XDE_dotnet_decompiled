using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009B RID: 155
	internal class TraceJsonReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x06000829 RID: 2089 RVA: 0x0002405C File Offset: 0x0002225C
		public TraceJsonReader(JsonReader innerReader)
		{
			this._innerReader = innerReader;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._sw.Write("Deserialized JSON: " + Environment.NewLine);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x000240BD File Offset: 0x000222BD
		public string GetDeserializedJsonMessage()
		{
			return this._sw.ToString();
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x000240CA File Offset: 0x000222CA
		public override bool Read()
		{
			bool result = this._innerReader.Read();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000240DD File Offset: 0x000222DD
		public override int? ReadAsInt32()
		{
			int? result = this._innerReader.ReadAsInt32();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000240F0 File Offset: 0x000222F0
		public override string ReadAsString()
		{
			string result = this._innerReader.ReadAsString();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00024103 File Offset: 0x00022303
		public override byte[] ReadAsBytes()
		{
			byte[] result = this._innerReader.ReadAsBytes();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00024116 File Offset: 0x00022316
		public override decimal? ReadAsDecimal()
		{
			decimal? result = this._innerReader.ReadAsDecimal();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00024129 File Offset: 0x00022329
		public override double? ReadAsDouble()
		{
			double? result = this._innerReader.ReadAsDouble();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0002413C File Offset: 0x0002233C
		public override bool? ReadAsBoolean()
		{
			bool? result = this._innerReader.ReadAsBoolean();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0002414F File Offset: 0x0002234F
		public override DateTime? ReadAsDateTime()
		{
			DateTime? result = this._innerReader.ReadAsDateTime();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00024162 File Offset: 0x00022362
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? result = this._innerReader.ReadAsDateTimeOffset();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00024175 File Offset: 0x00022375
		public void WriteCurrentToken()
		{
			this._textWriter.WriteToken(this._innerReader, false, false, true);
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0002418B File Offset: 0x0002238B
		public override int Depth
		{
			get
			{
				return this._innerReader.Depth;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00024198 File Offset: 0x00022398
		public override string Path
		{
			get
			{
				return this._innerReader.Path;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x000241A5 File Offset: 0x000223A5
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x000241B2 File Offset: 0x000223B2
		public override char QuoteChar
		{
			get
			{
				return this._innerReader.QuoteChar;
			}
			protected internal set
			{
				this._innerReader.QuoteChar = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x000241C0 File Offset: 0x000223C0
		public override JsonToken TokenType
		{
			get
			{
				return this._innerReader.TokenType;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x000241CD File Offset: 0x000223CD
		public override object Value
		{
			get
			{
				return this._innerReader.Value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x000241DA File Offset: 0x000223DA
		public override Type ValueType
		{
			get
			{
				return this._innerReader.ValueType;
			}
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000241E7 File Offset: 0x000223E7
		public override void Close()
		{
			this._innerReader.Close();
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x000241F4 File Offset: 0x000223F4
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo;
			return (jsonLineInfo = (this._innerReader as IJsonLineInfo)) != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00024218 File Offset: 0x00022418
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo;
				if ((jsonLineInfo = (this._innerReader as IJsonLineInfo)) == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0002423C File Offset: 0x0002243C
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo;
				if ((jsonLineInfo = (this._innerReader as IJsonLineInfo)) == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x040002C6 RID: 710
		private readonly JsonReader _innerReader;

		// Token: 0x040002C7 RID: 711
		private readonly JsonTextWriter _textWriter;

		// Token: 0x040002C8 RID: 712
		private readonly StringWriter _sw;
	}
}
