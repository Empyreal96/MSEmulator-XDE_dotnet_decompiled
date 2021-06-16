using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007D RID: 125
	public class JsonContainerContract : JsonContract
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0001C801 File Offset: 0x0001AA01
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x0001C809 File Offset: 0x0001AA09
		internal JsonContract ItemContract
		{
			get
			{
				return this._itemContract;
			}
			set
			{
				this._itemContract = value;
				if (this._itemContract != null)
				{
					this._finalItemContract = (this._itemContract.UnderlyingType.IsSealed() ? this._itemContract : null);
					return;
				}
				this._finalItemContract = null;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001C843 File Offset: 0x0001AA43
		internal JsonContract FinalItemContract
		{
			get
			{
				return this._finalItemContract;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x0001C84B File Offset: 0x0001AA4B
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x0001C853 File Offset: 0x0001AA53
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001C85C File Offset: 0x0001AA5C
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x0001C864 File Offset: 0x0001AA64
		public bool? ItemIsReference { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0001C86D File Offset: 0x0001AA6D
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x0001C875 File Offset: 0x0001AA75
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x0001C87E File Offset: 0x0001AA7E
		// (set) Token: 0x0600068F RID: 1679 RVA: 0x0001C886 File Offset: 0x0001AA86
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x06000690 RID: 1680 RVA: 0x0001C890 File Offset: 0x0001AA90
		internal JsonContainerContract(Type underlyingType) : base(underlyingType)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);
			if (cachedAttribute != null)
			{
				if (cachedAttribute.ItemConverterType != null)
				{
					this.ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
				}
				this.ItemIsReference = cachedAttribute._itemIsReference;
				this.ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
				this.ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
			}
		}

		// Token: 0x04000239 RID: 569
		private JsonContract _itemContract;

		// Token: 0x0400023A RID: 570
		private JsonContract _finalItemContract;
	}
}
