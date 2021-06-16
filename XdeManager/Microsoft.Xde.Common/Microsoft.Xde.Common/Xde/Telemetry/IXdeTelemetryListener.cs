using System;

namespace Microsoft.Xde.Telemetry
{
	// Token: 0x02000018 RID: 24
	public interface IXdeTelemetryListener
	{
		// Token: 0x0600009F RID: 159
		void WriteEvent<T>(string eventName, Logger.Level level, bool isError, T data);
	}
}
