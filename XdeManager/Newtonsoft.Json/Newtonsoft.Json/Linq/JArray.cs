using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B1 RID: 177
	public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
	{
		// Token: 0x0600096D RID: 2413 RVA: 0x00028118 File Offset: 0x00026318
		public override async Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await writer.WriteStartArrayAsync(cancellationToken).ConfigureAwait(false);
			for (int i = 0; i < this._values.Count; i++)
			{
				await this._values[i].WriteToAsync(writer, cancellationToken, converters).ConfigureAwait(false);
			}
			await writer.WriteEndArrayAsync(cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00028175 File Offset: 0x00026375
		public new static Task<JArray> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JArray.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00028180 File Offset: 0x00026380
		public new static async Task<JArray> LoadAsync(JsonReader reader, JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (reader.TokenType == JsonToken.None)
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = reader.ReadAsync(cancellationToken).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray a = new JArray();
			a.SetLineInfo(reader as IJsonLineInfo, settings);
			await a.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return a;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x000281D5 File Offset: 0x000263D5
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x000281DD File Offset: 0x000263DD
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Array;
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x000281E0 File Offset: 0x000263E0
		public JArray()
		{
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x000281F3 File Offset: 0x000263F3
		public JArray(JArray other) : base(other)
		{
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00028207 File Offset: 0x00026407
		public JArray(params object[] content) : this(content)
		{
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00028210 File Offset: 0x00026410
		public JArray(object content)
		{
			this.Add(content);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0002822C File Offset: 0x0002642C
		internal override bool DeepEquals(JToken node)
		{
			JArray container;
			return (container = (node as JArray)) != null && base.ContentsEqual(container);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0002824C File Offset: 0x0002644C
		internal override JToken CloneToken()
		{
			return new JArray(this);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00028254 File Offset: 0x00026454
		public new static JArray Load(JsonReader reader)
		{
			return JArray.Load(reader, null);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00028260 File Offset: 0x00026460
		public new static JArray Load(JsonReader reader, JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray jarray = new JArray();
			jarray.SetLineInfo(reader as IJsonLineInfo, settings);
			jarray.ReadTokenFrom(reader, settings);
			return jarray;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x000282D4 File Offset: 0x000264D4
		public new static JArray Parse(string json)
		{
			return JArray.Parse(json, null);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x000282E0 File Offset: 0x000264E0
		public new static JArray Parse(string json, JsonLoadSettings settings)
		{
			JArray result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JArray jarray = JArray.Load(jsonReader, settings);
				while (jsonReader.Read())
				{
				}
				result = jarray;
			}
			return result;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00028328 File Offset: 0x00026528
		public new static JArray FromObject(object o)
		{
			return JArray.FromObject(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00028338 File Offset: 0x00026538
		public new static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, jtoken.Type));
			}
			return (JArray)jtoken;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0002837C File Offset: 0x0002657C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			for (int i = 0; i < this._values.Count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		// Token: 0x170001B9 RID: 441
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this.GetItem((int)key);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x170001BA RID: 442
		public JToken this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0002844A File Offset: 0x0002664A
		internal override int IndexOfItem(JToken item)
		{
			return this._values.IndexOfReference(item);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00028458 File Offset: 0x00026658
		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			IEnumerable enumerable = (base.IsMultiContent(content) || content is JArray) ? ((IEnumerable)content) : null;
			if (enumerable == null)
			{
				return;
			}
			JContainer.MergeEnumerableContent(this, enumerable, settings);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0002848C File Offset: 0x0002668C
		public int IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00028495 File Offset: 0x00026695
		public void Insert(int index, JToken item)
		{
			this.InsertItem(index, item, false);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x000284A0 File Offset: 0x000266A0
		public void RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x000284AC File Offset: 0x000266AC
		public IEnumerator<JToken> GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x000284C7 File Offset: 0x000266C7
		public void Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x000284D0 File Offset: 0x000266D0
		public void Clear()
		{
			this.ClearItems();
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000284D8 File Offset: 0x000266D8
		public bool Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x000284E1 File Offset: 0x000266E1
		public void CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x000284EB File Offset: 0x000266EB
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x000284EE File Offset: 0x000266EE
		public bool Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x000284F7 File Offset: 0x000266F7
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x04000359 RID: 857
		private readonly List<JToken> _values = new List<JToken>();
	}
}
