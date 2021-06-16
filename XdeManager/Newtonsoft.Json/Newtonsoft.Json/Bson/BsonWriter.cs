using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000109 RID: 265
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonWriter : JsonWriter
	{
		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00036B25 File Offset: 0x00034D25
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x00036B32 File Offset: 0x00034D32
		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._writer.DateTimeKindHandling;
			}
			set
			{
				this._writer.DateTimeKindHandling = value;
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00036B40 File Offset: 0x00034D40
		public BsonWriter(Stream stream)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._writer = new BsonBinaryWriter(new BinaryWriter(stream));
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00036B64 File Offset: 0x00034D64
		public BsonWriter(BinaryWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = new BsonBinaryWriter(writer);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00036B83 File Offset: 0x00034D83
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00036B90 File Offset: 0x00034D90
		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			this.RemoveParent();
			if (base.Top == 0)
			{
				this._writer.WriteToken(this._root);
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00036BB8 File Offset: 0x00034DB8
		public override void WriteComment(string text)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON comment as BSON.", null);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00036BC6 File Offset: 0x00034DC6
		public override void WriteStartConstructor(string name)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON constructor as BSON.", null);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00036BD4 File Offset: 0x00034DD4
		public override void WriteRaw(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00036BE2 File Offset: 0x00034DE2
		public override void WriteRawValue(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00036BF0 File Offset: 0x00034DF0
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new BsonArray());
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00036C03 File Offset: 0x00034E03
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new BsonObject());
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00036C16 File Offset: 0x00034E16
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this._propertyName = name;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00036C26 File Offset: 0x00034E26
		public override void Close()
		{
			base.Close();
			if (base.CloseOutput)
			{
				BsonBinaryWriter writer = this._writer;
				if (writer == null)
				{
					return;
				}
				writer.Close();
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00036C46 File Offset: 0x00034E46
		private void AddParent(BsonToken container)
		{
			this.AddToken(container);
			this._parent = container;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00036C56 File Offset: 0x00034E56
		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00036C69 File Offset: 0x00034E69
		private void AddValue(object value, BsonType type)
		{
			this.AddToken(new BsonValue(value, type));
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00036C78 File Offset: 0x00034E78
		internal void AddToken(BsonToken token)
		{
			if (this._parent != null)
			{
				BsonObject bsonObject;
				if ((bsonObject = (this._parent as BsonObject)) != null)
				{
					bsonObject.Add(this._propertyName, token);
					this._propertyName = null;
					return;
				}
				((BsonArray)this._parent).Add(token);
				return;
			}
			else
			{
				if (token.Type != BsonType.Object && token.Type != BsonType.Array)
				{
					throw JsonWriterException.Create(this, "Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, token.Type), null);
				}
				this._parent = token;
				this._root = token;
				return;
			}
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00036D08 File Offset: 0x00034F08
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value;
				base.SetWriteState(JsonToken.Integer, null);
				this.AddToken(new BsonBinary(bigInteger.ToByteArray(), BsonBinaryType.Binary));
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00036D49 File Offset: 0x00034F49
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddToken(BsonEmpty.Null);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00036D5C File Offset: 0x00034F5C
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddToken(BsonEmpty.Undefined);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00036D6F File Offset: 0x00034F6F
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddToken((value == null) ? BsonEmpty.Null : new BsonString(value, true));
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00036D8F File Offset: 0x00034F8F
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00036DA6 File Offset: 0x00034FA6
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			if (value > 2147483647U)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00036DD2 File Offset: 0x00034FD2
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00036DE9 File Offset: 0x00034FE9
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00036E19 File Offset: 0x00035019
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00036E2F File Offset: 0x0003502F
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00036E45 File Offset: 0x00035045
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddToken(value ? BsonBoolean.True : BsonBoolean.False);
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00036E63 File Offset: 0x00035063
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00036E7A File Offset: 0x0003507A
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00036E94 File Offset: 0x00035094
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string value2 = value.ToString(CultureInfo.InvariantCulture);
			this.AddToken(new BsonString(value2, true));
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00036EC4 File Offset: 0x000350C4
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00036EDB File Offset: 0x000350DB
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00036EF2 File Offset: 0x000350F2
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00036F08 File Offset: 0x00035108
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00036F2D File Offset: 0x0003512D
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00036F44 File Offset: 0x00035144
		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value, BsonBinaryType.Binary));
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00036F64 File Offset: 0x00035164
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value.ToByteArray(), BsonBinaryType.Uuid));
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00036F80 File Offset: 0x00035180
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00036FA2 File Offset: 0x000351A2
		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00036FCD File Offset: 0x000351CD
		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw JsonWriterException.Create(this, "An object id must be 12 bytes", null);
			}
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddValue(value, BsonType.Oid);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00036FFF File Offset: 0x000351FF
		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddToken(new BsonRegex(pattern, options));
		}

		// Token: 0x0400043C RID: 1084
		private readonly BsonBinaryWriter _writer;

		// Token: 0x0400043D RID: 1085
		private BsonToken _root;

		// Token: 0x0400043E RID: 1086
		private BsonToken _parent;

		// Token: 0x0400043F RID: 1087
		private string _propertyName;
	}
}
