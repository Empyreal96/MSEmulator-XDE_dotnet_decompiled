using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BA RID: 186
	public class JsonLoadSettings
	{
		// Token: 0x06000A98 RID: 2712 RVA: 0x0002AE70 File Offset: 0x00029070
		public JsonLoadSettings()
		{
			this._lineInfoHandling = LineInfoHandling.Load;
			this._commentHandling = CommentHandling.Ignore;
			this._duplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace;
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0002AE8D File Offset: 0x0002908D
		// (set) Token: 0x06000A9A RID: 2714 RVA: 0x0002AE95 File Offset: 0x00029095
		public CommentHandling CommentHandling
		{
			get
			{
				return this._commentHandling;
			}
			set
			{
				if (value < CommentHandling.Ignore || value > CommentHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._commentHandling = value;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x0002AEB1 File Offset: 0x000290B1
		// (set) Token: 0x06000A9C RID: 2716 RVA: 0x0002AEB9 File Offset: 0x000290B9
		public LineInfoHandling LineInfoHandling
		{
			get
			{
				return this._lineInfoHandling;
			}
			set
			{
				if (value < LineInfoHandling.Ignore || value > LineInfoHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lineInfoHandling = value;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0002AED5 File Offset: 0x000290D5
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x0002AEDD File Offset: 0x000290DD
		public DuplicatePropertyNameHandling DuplicatePropertyNameHandling
		{
			get
			{
				return this._duplicatePropertyNameHandling;
			}
			set
			{
				if (value < DuplicatePropertyNameHandling.Replace || value > DuplicatePropertyNameHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._duplicatePropertyNameHandling = value;
			}
		}

		// Token: 0x0400036A RID: 874
		private CommentHandling _commentHandling;

		// Token: 0x0400036B RID: 875
		private LineInfoHandling _lineInfoHandling;

		// Token: 0x0400036C RID: 876
		private DuplicatePropertyNameHandling _duplicatePropertyNameHandling;
	}
}
