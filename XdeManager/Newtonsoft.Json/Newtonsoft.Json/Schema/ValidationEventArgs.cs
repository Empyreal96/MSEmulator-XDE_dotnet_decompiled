using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AB RID: 171
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x06000952 RID: 2386 RVA: 0x00027DC3 File Offset: 0x00025FC3
		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00027DDD File Offset: 0x00025FDD
		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00027DE5 File Offset: 0x00025FE5
		public string Path
		{
			get
			{
				return this._ex.Path;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x00027DF2 File Offset: 0x00025FF2
		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		// Token: 0x04000351 RID: 849
		private readonly JsonSchemaException _ex;
	}
}
