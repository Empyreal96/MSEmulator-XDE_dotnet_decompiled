using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B6 RID: 182
	public class JProperty : JContainer
	{
		// Token: 0x06000A57 RID: 2647 RVA: 0x0002A4F0 File Offset: 0x000286F0
		public override Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			Task task = writer.WritePropertyNameAsync(this._name, cancellationToken);
			if (task.IsCompletedSucessfully())
			{
				return this.WriteValueAsync(writer, cancellationToken, converters);
			}
			return this.WriteToAsync(task, writer, cancellationToken, converters);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0002A528 File Offset: 0x00028728
		private async Task WriteToAsync(Task task, JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await task.ConfigureAwait(false);
			await this.WriteValueAsync(writer, cancellationToken, converters).ConfigureAwait(false);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0002A590 File Offset: 0x00028790
		private Task WriteValueAsync(JsonWriter writer, CancellationToken cancellationToken, JsonConverter[] converters)
		{
			JToken value = this.Value;
			if (value == null)
			{
				return writer.WriteNullAsync(cancellationToken);
			}
			return value.WriteToAsync(writer, cancellationToken, converters);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0002A5B8 File Offset: 0x000287B8
		public new static Task<JProperty> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JProperty.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0002A5C4 File Offset: 0x000287C4
		public new static async Task<JProperty> LoadAsync(JsonReader reader, JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
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
					throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty p = new JProperty((string)reader.Value);
			p.SetLineInfo(reader as IJsonLineInfo, settings);
			await p.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return p;
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0002A619 File Offset: 0x00028819
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0002A621 File Offset: 0x00028821
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0002A629 File Offset: 0x00028829
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x0002A638 File Offset: 0x00028838
		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return this._content._token;
			}
			set
			{
				base.CheckReentrancy();
				JToken item = value ?? JValue.CreateNull();
				if (this._content._token == null)
				{
					this.InsertItem(0, item, false);
					return;
				}
				this.SetItem(0, item);
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0002A675 File Offset: 0x00028875
		public JProperty(JProperty other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0002A695 File Offset: 0x00028895
		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0002A6A8 File Offset: 0x000288A8
		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (JContainer.IsTokenUnchanged(this.Value, item))
			{
				return;
			}
			JObject jobject = (JObject)base.Parent;
			if (jobject != null)
			{
				jobject.InternalPropertyChanging(this);
			}
			base.SetItem(0, item);
			JObject jobject2 = (JObject)base.Parent;
			if (jobject2 == null)
			{
				return;
			}
			jobject2.InternalPropertyChanged(this);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0002A702 File Offset: 0x00028902
		internal override bool RemoveItem(JToken item)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0002A722 File Offset: 0x00028922
		internal override void RemoveItemAt(int index)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0002A742 File Offset: 0x00028942
		internal override int IndexOfItem(JToken item)
		{
			return this._content.IndexOf(item);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0002A750 File Offset: 0x00028950
		internal override void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return;
			}
			if (this.Value != null)
			{
				throw new JsonException("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
			}
			base.InsertItem(0, item, false);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0002A78F File Offset: 0x0002898F
		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0002A79C File Offset: 0x0002899C
		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			JProperty jproperty = content as JProperty;
			JToken jtoken = (jproperty != null) ? jproperty.Value : null;
			if (jtoken != null && jtoken.Type != JTokenType.Null)
			{
				this.Value = jtoken;
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0002A7D0 File Offset: 0x000289D0
		internal override void ClearItems()
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0002A7F0 File Offset: 0x000289F0
		internal override bool DeepEquals(JToken node)
		{
			JProperty jproperty;
			return (jproperty = (node as JProperty)) != null && this._name == jproperty.Name && base.ContentsEqual(jproperty);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0002A823 File Offset: 0x00028A23
		internal override JToken CloneToken()
		{
			return new JProperty(this);
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0002A82B File Offset: 0x00028A2B
		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0002A82E File Offset: 0x00028A2E
		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0002A853 File Offset: 0x00028A53
		public JProperty(string name, params object[] content) : this(name, content)
		{
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0002A860 File Offset: 0x00028A60
		public JProperty(string name, object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			this.Value = (base.IsMultiContent(content) ? new JArray(content) : JContainer.CreateFromContent(content));
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0002A8B0 File Offset: 0x00028AB0
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			JToken value = this.Value;
			if (value != null)
			{
				value.WriteTo(writer, converters);
				return;
			}
			writer.WriteNull();
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0002A8E2 File Offset: 0x00028AE2
		internal override int GetDeepHashCode()
		{
			int hashCode = this._name.GetHashCode();
			JToken value = this.Value;
			return hashCode ^ ((value != null) ? value.GetDeepHashCode() : 0);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0002A902 File Offset: 0x00028B02
		public new static JProperty Load(JsonReader reader)
		{
			return JProperty.Load(reader, null);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0002A90C File Offset: 0x00028B0C
		public new static JProperty Load(JsonReader reader, JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jproperty = new JProperty((string)reader.Value);
			jproperty.SetLineInfo(reader as IJsonLineInfo, settings);
			jproperty.ReadTokenFrom(reader, settings);
			return jproperty;
		}

		// Token: 0x04000366 RID: 870
		private readonly JProperty.JPropertyList _content = new JProperty.JPropertyList();

		// Token: 0x04000367 RID: 871
		private readonly string _name;

		// Token: 0x020001BE RID: 446
		private class JPropertyList : IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
		{
			// Token: 0x06000F91 RID: 3985 RVA: 0x00044D2E File Offset: 0x00042F2E
			public IEnumerator<JToken> GetEnumerator()
			{
				if (this._token != null)
				{
					yield return this._token;
				}
				yield break;
			}

			// Token: 0x06000F92 RID: 3986 RVA: 0x00044D3D File Offset: 0x00042F3D
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000F93 RID: 3987 RVA: 0x00044D45 File Offset: 0x00042F45
			public void Add(JToken item)
			{
				this._token = item;
			}

			// Token: 0x06000F94 RID: 3988 RVA: 0x00044D4E File Offset: 0x00042F4E
			public void Clear()
			{
				this._token = null;
			}

			// Token: 0x06000F95 RID: 3989 RVA: 0x00044D57 File Offset: 0x00042F57
			public bool Contains(JToken item)
			{
				return this._token == item;
			}

			// Token: 0x06000F96 RID: 3990 RVA: 0x00044D62 File Offset: 0x00042F62
			public void CopyTo(JToken[] array, int arrayIndex)
			{
				if (this._token != null)
				{
					array[arrayIndex] = this._token;
				}
			}

			// Token: 0x06000F97 RID: 3991 RVA: 0x00044D75 File Offset: 0x00042F75
			public bool Remove(JToken item)
			{
				if (this._token == item)
				{
					this._token = null;
					return true;
				}
				return false;
			}

			// Token: 0x170002A3 RID: 675
			// (get) Token: 0x06000F98 RID: 3992 RVA: 0x00044D8A File Offset: 0x00042F8A
			public int Count
			{
				get
				{
					if (this._token == null)
					{
						return 0;
					}
					return 1;
				}
			}

			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x06000F99 RID: 3993 RVA: 0x00044D97 File Offset: 0x00042F97
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000F9A RID: 3994 RVA: 0x00044D9A File Offset: 0x00042F9A
			public int IndexOf(JToken item)
			{
				if (this._token != item)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x06000F9B RID: 3995 RVA: 0x00044DA8 File Offset: 0x00042FA8
			public void Insert(int index, JToken item)
			{
				if (index == 0)
				{
					this._token = item;
				}
			}

			// Token: 0x06000F9C RID: 3996 RVA: 0x00044DB4 File Offset: 0x00042FB4
			public void RemoveAt(int index)
			{
				if (index == 0)
				{
					this._token = null;
				}
			}

			// Token: 0x170002A5 RID: 677
			public JToken this[int index]
			{
				get
				{
					if (index != 0)
					{
						return null;
					}
					return this._token;
				}
				set
				{
					if (index == 0)
					{
						this._token = value;
					}
				}
			}

			// Token: 0x0400079F RID: 1951
			internal JToken _token;
		}
	}
}
