using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200006F RID: 111
	internal class DefaultReferenceResolver : IReferenceResolver
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x0001BAAC File Offset: 0x00019CAC
		private BidirectionalDictionary<string, object> GetMappings(object context)
		{
			JsonSerializerInternalBase jsonSerializerInternalBase;
			if ((jsonSerializerInternalBase = (context as JsonSerializerInternalBase)) == null)
			{
				JsonSerializerProxy jsonSerializerProxy;
				if ((jsonSerializerProxy = (context as JsonSerializerProxy)) == null)
				{
					throw new JsonException("The DefaultReferenceResolver can only be used internally.");
				}
				jsonSerializerInternalBase = jsonSerializerProxy.GetInternalSerializer();
			}
			return jsonSerializerInternalBase.DefaultReferenceMappings;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001BAE8 File Offset: 0x00019CE8
		public object ResolveReference(object context, string reference)
		{
			object result;
			this.GetMappings(context).TryGetByFirst(reference, out result);
			return result;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001BB08 File Offset: 0x00019D08
		public string GetReference(object context, object value)
		{
			BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
			string text;
			if (!mappings.TryGetBySecond(value, out text))
			{
				this._referenceCount++;
				text = this._referenceCount.ToString(CultureInfo.InvariantCulture);
				mappings.Set(text, value);
			}
			return text;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001BB50 File Offset: 0x00019D50
		public void AddReference(object context, string reference, object value)
		{
			this.GetMappings(context).Set(reference, value);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001BB60 File Offset: 0x00019D60
		public bool IsReferenced(object context, object value)
		{
			string text;
			return this.GetMappings(context).TryGetBySecond(value, out text);
		}

		// Token: 0x0400021A RID: 538
		private int _referenceCount;
	}
}
