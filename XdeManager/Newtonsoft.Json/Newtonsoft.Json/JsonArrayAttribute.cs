using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200000F RID: 15
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonArrayAttribute : JsonContainerAttribute
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022E2 File Offset: 0x000004E2
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000022EA File Offset: 0x000004EA
		public bool AllowNullItems
		{
			get
			{
				return this._allowNullItems;
			}
			set
			{
				this._allowNullItems = value;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022F3 File Offset: 0x000004F3
		public JsonArrayAttribute()
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022FB File Offset: 0x000004FB
		public JsonArrayAttribute(bool allowNullItems)
		{
			this._allowNullItems = allowNullItems;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000230A File Offset: 0x0000050A
		public JsonArrayAttribute(string id) : base(id)
		{
		}

		// Token: 0x04000023 RID: 35
		private bool _allowNullItems;
	}
}
