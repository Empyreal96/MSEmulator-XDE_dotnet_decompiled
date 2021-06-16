using System;
using System.Diagnostics;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007A RID: 122
	public interface ITraceWriter
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000671 RID: 1649
		TraceLevel LevelFilter { get; }

		// Token: 0x06000672 RID: 1650
		void Trace(TraceLevel level, string message, Exception ex);
	}
}
