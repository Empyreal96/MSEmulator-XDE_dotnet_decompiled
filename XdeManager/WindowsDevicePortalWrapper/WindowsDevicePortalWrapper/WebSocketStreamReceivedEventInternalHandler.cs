using System;
using System.IO;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000017 RID: 23
	// (Invoke) Token: 0x0600016C RID: 364
	internal delegate void WebSocketStreamReceivedEventInternalHandler<T>(WebSocket<T> sender, WebSocketMessageReceivedEventArgs<Stream> args);
}
