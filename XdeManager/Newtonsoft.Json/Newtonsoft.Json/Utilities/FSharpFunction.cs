using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000052 RID: 82
	internal class FSharpFunction
	{
		// Token: 0x06000517 RID: 1303 RVA: 0x000165F1 File Offset: 0x000147F1
		public FSharpFunction(object instance, MethodCall<object, object> invoker)
		{
			this._instance = instance;
			this._invoker = invoker;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00016607 File Offset: 0x00014807
		public object Invoke(params object[] args)
		{
			return this._invoker(this._instance, args);
		}

		// Token: 0x040001C6 RID: 454
		private readonly object _instance;

		// Token: 0x040001C7 RID: 455
		private readonly MethodCall<object, object> _invoker;
	}
}
