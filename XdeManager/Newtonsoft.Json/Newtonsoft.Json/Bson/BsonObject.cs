using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x020000FF RID: 255
	internal class BsonObject : BsonToken, IEnumerable<BsonProperty>, IEnumerable
	{
		// Token: 0x06000D7E RID: 3454 RVA: 0x00036948 File Offset: 0x00034B48
		public void Add(string name, BsonToken token)
		{
			this._children.Add(new BsonProperty
			{
				Name = new BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x00036975 File Offset: 0x00034B75
		public override BsonType Type
		{
			get
			{
				return BsonType.Object;
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00036978 File Offset: 0x00034B78
		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0003698A File Offset: 0x00034B8A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000417 RID: 1047
		private readonly List<BsonProperty> _children = new List<BsonProperty>();
	}
}
