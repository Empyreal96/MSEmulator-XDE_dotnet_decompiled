using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B2 RID: 178
	public class JConstructor : JContainer
	{
		// Token: 0x06000990 RID: 2448 RVA: 0x00028500 File Offset: 0x00026700
		public override async Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await writer.WriteStartConstructorAsync(this._name, cancellationToken).ConfigureAwait(false);
			for (int i = 0; i < this._values.Count; i++)
			{
				await this._values[i].WriteToAsync(writer, cancellationToken, converters).ConfigureAwait(false);
			}
			await writer.WriteEndConstructorAsync(cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0002855D File Offset: 0x0002675D
		public new static Task<JConstructor> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JConstructor.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00028568 File Offset: 0x00026768
		public new static async Task<JConstructor> LoadAsync(JsonReader reader, JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
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
					throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor c = new JConstructor((string)reader.Value);
			c.SetLineInfo(reader as IJsonLineInfo, settings);
			await c.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return c;
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x000285BD File Offset: 0x000267BD
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x000285C5 File Offset: 0x000267C5
		internal override int IndexOfItem(JToken item)
		{
			return this._values.IndexOfReference(item);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x000285D4 File Offset: 0x000267D4
		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			JConstructor jconstructor;
			if ((jconstructor = (content as JConstructor)) == null)
			{
				return;
			}
			if (jconstructor.Name != null)
			{
				this.Name = jconstructor.Name;
			}
			JContainer.MergeEnumerableContent(this, jconstructor, settings);
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x00028608 File Offset: 0x00026808
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x00028610 File Offset: 0x00026810
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x00028619 File Offset: 0x00026819
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Constructor;
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002861C File Offset: 0x0002681C
		public JConstructor()
		{
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0002862F File Offset: 0x0002682F
		public JConstructor(JConstructor other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002864F File Offset: 0x0002684F
		public JConstructor(string name, params object[] content) : this(name, content)
		{
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00028659 File Offset: 0x00026859
		public JConstructor(string name, object content) : this(name)
		{
			this.Add(content);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00028669 File Offset: 0x00026869
		public JConstructor(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Constructor name cannot be empty.", "name");
			}
			this._name = name;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x000286AC File Offset: 0x000268AC
		internal override bool DeepEquals(JToken node)
		{
			JConstructor jconstructor;
			return (jconstructor = (node as JConstructor)) != null && this._name == jconstructor.Name && base.ContentsEqual(jconstructor);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x000286DF File Offset: 0x000268DF
		internal override JToken CloneToken()
		{
			return new JConstructor(this);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x000286E8 File Offset: 0x000268E8
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(this._name);
			int count = this._values.Count;
			for (int i = 0; i < count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		// Token: 0x170001BF RID: 447
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (key is int)
				{
					int index = (int)key;
					return this.GetItem(index);
				}
				throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (key is int)
				{
					int index = (int)key;
					this.SetItem(index, value);
					return;
				}
				throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
			}
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000287D2 File Offset: 0x000269D2
		internal override int GetDeepHashCode()
		{
			return this._name.GetHashCode() ^ base.ContentsHashCode();
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000287E6 File Offset: 0x000269E6
		public new static JConstructor Load(JsonReader reader)
		{
			return JConstructor.Load(reader, null);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x000287F0 File Offset: 0x000269F0
		public new static JConstructor Load(JsonReader reader, JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor jconstructor = new JConstructor((string)reader.Value);
			jconstructor.SetLineInfo(reader as IJsonLineInfo, settings);
			jconstructor.ReadTokenFrom(reader, settings);
			return jconstructor;
		}

		// Token: 0x0400035A RID: 858
		private string _name;

		// Token: 0x0400035B RID: 859
		private readonly List<JToken> _values = new List<JToken>();
	}
}
