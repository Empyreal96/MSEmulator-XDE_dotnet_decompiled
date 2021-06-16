using System;
using System.Globalization;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C0 RID: 192
	public class JTokenWriter : JsonWriter
	{
		// Token: 0x06000B55 RID: 2901 RVA: 0x0002D9FD File Offset: 0x0002BBFD
		internal override Task WriteTokenAsync(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments, CancellationToken cancellationToken)
		{
			if (reader is JTokenReader)
			{
				this.WriteToken(reader, writeChildren, writeDateConstructorAsDate, writeComments);
				return AsyncUtils.CompletedTask;
			}
			return base.WriteTokenSyncReadingAsync(reader, cancellationToken);
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0002DA21 File Offset: 0x0002BC21
		public JToken CurrentToken
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0002DA29 File Offset: 0x0002BC29
		public JToken Token
		{
			get
			{
				if (this._token != null)
				{
					return this._token;
				}
				return this._value;
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002DA40 File Offset: 0x0002BC40
		public JTokenWriter(JContainer container)
		{
			ValidationUtils.ArgumentNotNull(container, "container");
			this._token = container;
			this._parent = container;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0002DA61 File Offset: 0x0002BC61
		public JTokenWriter()
		{
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0002DA69 File Offset: 0x0002BC69
		public override void Flush()
		{
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0002DA6B File Offset: 0x0002BC6B
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0002DA73 File Offset: 0x0002BC73
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new JObject());
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0002DA86 File Offset: 0x0002BC86
		private void AddParent(JContainer container)
		{
			if (this._parent == null)
			{
				this._token = container;
			}
			else
			{
				this._parent.AddAndSkipParentCheck(container);
			}
			this._parent = container;
			this._current = container;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0002DAB4 File Offset: 0x0002BCB4
		private void RemoveParent()
		{
			this._current = this._parent;
			this._parent = this._parent.Parent;
			if (this._parent != null && this._parent.Type == JTokenType.Property)
			{
				this._parent = this._parent.Parent;
			}
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0002DB05 File Offset: 0x0002BD05
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new JArray());
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0002DB18 File Offset: 0x0002BD18
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this.AddParent(new JConstructor(name));
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0002DB2D File Offset: 0x0002BD2D
		protected override void WriteEnd(JsonToken token)
		{
			this.RemoveParent();
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0002DB35 File Offset: 0x0002BD35
		public override void WritePropertyName(string name)
		{
			JObject jobject = this._parent as JObject;
			if (jobject != null)
			{
				jobject.Remove(name);
			}
			this.AddParent(new JProperty(name));
			base.WritePropertyName(name);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0002DB62 File Offset: 0x0002BD62
		private void AddValue(object value, JsonToken token)
		{
			this.AddValue(new JValue(value), token);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0002DB74 File Offset: 0x0002BD74
		internal void AddValue(JValue value, JsonToken token)
		{
			if (this._parent != null)
			{
				this._parent.Add(value);
				this._current = this._parent.Last;
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					return;
				}
			}
			else
			{
				this._value = (value ?? JValue.CreateNull());
				this._current = this._value;
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0002DBE2 File Offset: 0x0002BDE2
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				base.InternalWriteValue(JsonToken.Integer);
				this.AddValue(value, JsonToken.Integer);
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0002DC03 File Offset: 0x0002BE03
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddValue(null, JsonToken.Null);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0002DC14 File Offset: 0x0002BE14
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddValue(null, JsonToken.Undefined);
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0002DC25 File Offset: 0x0002BE25
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this.AddValue(new JRaw(json), JsonToken.Raw);
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002DC3B File Offset: 0x0002BE3B
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			this.AddValue(JValue.CreateComment(text), JsonToken.Comment);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0002DC51 File Offset: 0x0002BE51
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0002DC63 File Offset: 0x0002BE63
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002DC79 File Offset: 0x0002BE79
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0002DC8F File Offset: 0x0002BE8F
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0002DCA5 File Offset: 0x0002BEA5
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0002DCBB File Offset: 0x0002BEBB
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002DCD1 File Offset: 0x0002BED1
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0002DCE7 File Offset: 0x0002BEE7
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Boolean);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0002DCFE File Offset: 0x0002BEFE
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002DD14 File Offset: 0x0002BF14
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002DD2C File Offset: 0x0002BF2C
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string value2 = value.ToString(CultureInfo.InvariantCulture);
			this.AddValue(value2, JsonToken.String);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002DD56 File Offset: 0x0002BF56
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0002DD6C File Offset: 0x0002BF6C
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0002DD82 File Offset: 0x0002BF82
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0002DD98 File Offset: 0x0002BF98
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002DDBD File Offset: 0x0002BFBD
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0002DDD4 File Offset: 0x0002BFD4
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Bytes);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0002DDE6 File Offset: 0x0002BFE6
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0002DDFD File Offset: 0x0002BFFD
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002DE14 File Offset: 0x0002C014
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002DE28 File Offset: 0x0002C028
		internal override void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			JTokenReader jtokenReader;
			if ((jtokenReader = (reader as JTokenReader)) == null || !writeChildren || !writeDateConstructorAsDate || !writeComments)
			{
				base.WriteToken(reader, writeChildren, writeDateConstructorAsDate, writeComments);
				return;
			}
			if (jtokenReader.TokenType == JsonToken.None && !jtokenReader.Read())
			{
				return;
			}
			JToken jtoken = jtokenReader.CurrentToken.CloneToken();
			if (this._parent != null)
			{
				this._parent.Add(jtoken);
				this._current = this._parent.Last;
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					base.InternalWriteValue(JsonToken.Null);
				}
			}
			else
			{
				this._current = jtoken;
				if (this._token == null && this._value == null)
				{
					this._token = (jtoken as JContainer);
					this._value = (jtoken as JValue);
				}
			}
			jtokenReader.Skip();
		}

		// Token: 0x04000396 RID: 918
		private JContainer _token;

		// Token: 0x04000397 RID: 919
		private JContainer _parent;

		// Token: 0x04000398 RID: 920
		private JValue _value;

		// Token: 0x04000399 RID: 921
		private JToken _current;
	}
}
