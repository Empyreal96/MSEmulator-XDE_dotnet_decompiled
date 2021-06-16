using System;
using System.Collections;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x0200045D RID: 1117
	[Serializable]
	public sealed class PSPrimitiveDictionary : Hashtable
	{
		// Token: 0x060030C3 RID: 12483 RVA: 0x0010ABD8 File Offset: 0x00108DD8
		public PSPrimitiveDictionary() : base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x0010ABE8 File Offset: 0x00108DE8
		public PSPrimitiveDictionary(Hashtable other) : base(StringComparer.OrdinalIgnoreCase)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (object obj in other)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				Hashtable hashtable = PSObject.Base(dictionaryEntry.Value) as Hashtable;
				if (hashtable != null)
				{
					this.Add(dictionaryEntry.Key, new PSPrimitiveDictionary(hashtable));
				}
				else
				{
					this.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x0010AC8C File Offset: 0x00108E8C
		private PSPrimitiveDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x0010AC98 File Offset: 0x00108E98
		private string VerifyKey(object key)
		{
			key = PSObject.Base(key);
			string text = key as string;
			if (text == null)
			{
				string message = StringUtil.Format(Serialization.PrimitiveHashtableInvalidKey, key.GetType().FullName);
				throw new ArgumentException(message);
			}
			return text;
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x0010ACD8 File Offset: 0x00108ED8
		private void VerifyValue(object value)
		{
			if (value == null)
			{
				return;
			}
			value = PSObject.Base(value);
			Type type = value.GetType();
			foreach (Type right in PSPrimitiveDictionary.handshakeFriendlyTypes)
			{
				if (type == right)
				{
					return;
				}
			}
			if (type.IsArray || type == typeof(ArrayList))
			{
				foreach (object value2 in ((IEnumerable)value))
				{
					this.VerifyValue(value2);
				}
				return;
			}
			string message = StringUtil.Format(Serialization.PrimitiveHashtableInvalidValue, value.GetType().FullName);
			throw new ArgumentException(message);
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x0010ADA8 File Offset: 0x00108FA8
		public override void Add(object key, object value)
		{
			string key2 = this.VerifyKey(key);
			this.VerifyValue(value);
			base.Add(key2, value);
		}

		// Token: 0x17000B2A RID: 2858
		public override object this[object key]
		{
			get
			{
				return base[key];
			}
			set
			{
				string key2 = this.VerifyKey(key);
				this.VerifyValue(value);
				base[key2] = value;
			}
		}

		// Token: 0x17000B2B RID: 2859
		public object this[string key]
		{
			get
			{
				return base[key];
			}
			set
			{
				this.VerifyValue(value);
				base[key] = value;
			}
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x0010AE16 File Offset: 0x00109016
		public override object Clone()
		{
			return new PSPrimitiveDictionary(this);
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x0010AE1E File Offset: 0x0010901E
		public void Add(string key, bool value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x0010AE2D File Offset: 0x0010902D
		public void Add(string key, bool[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x0010AE37 File Offset: 0x00109037
		public void Add(string key, byte value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x0010AE46 File Offset: 0x00109046
		public void Add(string key, byte[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x0010AE50 File Offset: 0x00109050
		public void Add(string key, char value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x0010AE5F File Offset: 0x0010905F
		public void Add(string key, char[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x0010AE69 File Offset: 0x00109069
		public void Add(string key, DateTime value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x0010AE78 File Offset: 0x00109078
		public void Add(string key, DateTime[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x0010AE82 File Offset: 0x00109082
		public void Add(string key, decimal value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x0010AE91 File Offset: 0x00109091
		public void Add(string key, decimal[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x0010AE9B File Offset: 0x0010909B
		public void Add(string key, double value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x0010AEAA File Offset: 0x001090AA
		public void Add(string key, double[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x0010AEB4 File Offset: 0x001090B4
		public void Add(string key, Guid value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x0010AEC3 File Offset: 0x001090C3
		public void Add(string key, Guid[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x0010AECD File Offset: 0x001090CD
		public void Add(string key, int value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x0010AEDC File Offset: 0x001090DC
		public void Add(string key, int[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x0010AEE6 File Offset: 0x001090E6
		public void Add(string key, long value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x0010AEF5 File Offset: 0x001090F5
		public void Add(string key, long[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x0010AEFF File Offset: 0x001090FF
		public void Add(string key, sbyte value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x0010AF0E File Offset: 0x0010910E
		public void Add(string key, sbyte[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x0010AF18 File Offset: 0x00109118
		public void Add(string key, float value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x0010AF27 File Offset: 0x00109127
		public void Add(string key, float[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x0010AF31 File Offset: 0x00109131
		public void Add(string key, string value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x0010AF3B File Offset: 0x0010913B
		public void Add(string key, string[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x0010AF45 File Offset: 0x00109145
		public void Add(string key, TimeSpan value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x0010AF54 File Offset: 0x00109154
		public void Add(string key, TimeSpan[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x0010AF5E File Offset: 0x0010915E
		public void Add(string key, ushort value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x0010AF6D File Offset: 0x0010916D
		public void Add(string key, ushort[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x0010AF77 File Offset: 0x00109177
		public void Add(string key, uint value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x0010AF86 File Offset: 0x00109186
		public void Add(string key, uint[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x0010AF90 File Offset: 0x00109190
		public void Add(string key, ulong value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x0010AF9F File Offset: 0x0010919F
		public void Add(string key, ulong[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x0010AFA9 File Offset: 0x001091A9
		public void Add(string key, Uri value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x0010AFB3 File Offset: 0x001091B3
		public void Add(string key, Uri[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x0010AFBD File Offset: 0x001091BD
		public void Add(string key, Version value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x0010AFC7 File Offset: 0x001091C7
		public void Add(string key, Version[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x0010AFD1 File Offset: 0x001091D1
		public void Add(string key, PSPrimitiveDictionary value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x0010AFDB File Offset: 0x001091DB
		public void Add(string key, PSPrimitiveDictionary[] value)
		{
			this.Add(key, value);
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x0010AFE8 File Offset: 0x001091E8
		internal static PSPrimitiveDictionary CloneAndAddPSVersionTable(PSPrimitiveDictionary originalHash)
		{
			if (originalHash != null && originalHash.ContainsKey("PSVersionTable"))
			{
				return (PSPrimitiveDictionary)originalHash.Clone();
			}
			PSPrimitiveDictionary psprimitiveDictionary;
			if (originalHash != null)
			{
				psprimitiveDictionary = (PSPrimitiveDictionary)originalHash.Clone();
			}
			else
			{
				psprimitiveDictionary = new PSPrimitiveDictionary();
			}
			PSPrimitiveDictionary value = new PSPrimitiveDictionary(PSVersionInfo.GetPSVersionTable());
			psprimitiveDictionary.Add("PSVersionTable", value);
			return psprimitiveDictionary;
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x0010B044 File Offset: 0x00109244
		internal static bool TryPathGet<T>(IDictionary data, out T result, params string[] keys)
		{
			if (data == null || !data.Contains(keys[0]))
			{
				result = default(T);
				return false;
			}
			if (keys.Length == 1)
			{
				return LanguagePrimitives.TryConvertTo<T>(data[keys[0]], out result);
			}
			IDictionary dictionary;
			if (LanguagePrimitives.TryConvertTo<IDictionary>(data[keys[0]], out dictionary) && dictionary != null)
			{
				string[] array = new string[keys.Length - 1];
				Array.Copy(keys, 1, array, 0, array.Length);
				return PSPrimitiveDictionary.TryPathGet<T>(dictionary, out result, array);
			}
			result = default(T);
			return false;
		}

		// Token: 0x04001A36 RID: 6710
		private static readonly Type[] handshakeFriendlyTypes = new Type[]
		{
			typeof(bool),
			typeof(byte),
			typeof(char),
			typeof(DateTime),
			typeof(decimal),
			typeof(double),
			typeof(Guid),
			typeof(int),
			typeof(long),
			typeof(sbyte),
			typeof(float),
			typeof(string),
			typeof(TimeSpan),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(Uri),
			typeof(byte[]),
			typeof(Version),
			typeof(ProgressRecord),
			typeof(XmlDocument),
			typeof(PSPrimitiveDictionary)
		};
	}
}
