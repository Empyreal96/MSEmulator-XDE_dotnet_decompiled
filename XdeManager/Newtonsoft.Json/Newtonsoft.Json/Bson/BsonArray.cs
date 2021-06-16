using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000100 RID: 256
	internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
	{
		// Token: 0x06000D83 RID: 3459 RVA: 0x000369A5 File Offset: 0x00034BA5
		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x000369BA File Offset: 0x00034BBA
		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x000369BD File Offset: 0x00034BBD
		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x000369CF File Offset: 0x00034BCF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000418 RID: 1048
		private readonly List<BsonToken> _children = new List<BsonToken>();
	}
}
