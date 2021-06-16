using System;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000011 RID: 17
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public abstract class JsonContainerAttribute : Attribute
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000231B File Offset: 0x0000051B
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002323 File Offset: 0x00000523
		public string Id { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000232C File Offset: 0x0000052C
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002334 File Offset: 0x00000534
		public string Title { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000233D File Offset: 0x0000053D
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002345 File Offset: 0x00000545
		public string Description { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000234E File Offset: 0x0000054E
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002356 File Offset: 0x00000556
		public Type ItemConverterType { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000235F File Offset: 0x0000055F
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002367 File Offset: 0x00000567
		public object[] ItemConverterParameters { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002370 File Offset: 0x00000570
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002378 File Offset: 0x00000578
		public Type NamingStrategyType
		{
			get
			{
				return this._namingStrategyType;
			}
			set
			{
				this._namingStrategyType = value;
				this.NamingStrategyInstance = null;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002388 File Offset: 0x00000588
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002390 File Offset: 0x00000590
		public object[] NamingStrategyParameters
		{
			get
			{
				return this._namingStrategyParameters;
			}
			set
			{
				this._namingStrategyParameters = value;
				this.NamingStrategyInstance = null;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000023A0 File Offset: 0x000005A0
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000023A8 File Offset: 0x000005A8
		internal NamingStrategy NamingStrategyInstance { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000023B4 File Offset: 0x000005B4
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000023DA File Offset: 0x000005DA
		public bool IsReference
		{
			get
			{
				return this._isReference ?? false;
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000023E8 File Offset: 0x000005E8
		// (set) Token: 0x06000028 RID: 40 RVA: 0x0000240E File Offset: 0x0000060E
		public bool ItemIsReference
		{
			get
			{
				return this._itemIsReference ?? false;
			}
			set
			{
				this._itemIsReference = new bool?(value);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000241C File Offset: 0x0000061C
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002442 File Offset: 0x00000642
		public ReferenceLoopHandling ItemReferenceLoopHandling
		{
			get
			{
				ReferenceLoopHandling? itemReferenceLoopHandling = this._itemReferenceLoopHandling;
				if (itemReferenceLoopHandling == null)
				{
					return ReferenceLoopHandling.Error;
				}
				return itemReferenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002450 File Offset: 0x00000650
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002476 File Offset: 0x00000676
		public TypeNameHandling ItemTypeNameHandling
		{
			get
			{
				TypeNameHandling? itemTypeNameHandling = this._itemTypeNameHandling;
				if (itemTypeNameHandling == null)
				{
					return TypeNameHandling.None;
				}
				return itemTypeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._itemTypeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002484 File Offset: 0x00000684
		protected JsonContainerAttribute()
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000248C File Offset: 0x0000068C
		protected JsonContainerAttribute(string id)
		{
			this.Id = id;
		}

		// Token: 0x0400002A RID: 42
		internal bool? _isReference;

		// Token: 0x0400002B RID: 43
		internal bool? _itemIsReference;

		// Token: 0x0400002C RID: 44
		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		// Token: 0x0400002D RID: 45
		internal TypeNameHandling? _itemTypeNameHandling;

		// Token: 0x0400002E RID: 46
		private Type _namingStrategyType;

		// Token: 0x0400002F RID: 47
		private object[] _namingStrategyParameters;
	}
}
