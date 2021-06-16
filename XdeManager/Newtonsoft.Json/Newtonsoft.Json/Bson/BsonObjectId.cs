using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x020000FC RID: 252
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonObjectId
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x00035CD9 File Offset: 0x00033ED9
		public byte[] Value { get; }

		// Token: 0x06000D55 RID: 3413 RVA: 0x00035CE1 File Offset: 0x00033EE1
		public BsonObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new ArgumentException("An ObjectId must be 12 bytes", "value");
			}
			this.Value = value;
		}
	}
}
