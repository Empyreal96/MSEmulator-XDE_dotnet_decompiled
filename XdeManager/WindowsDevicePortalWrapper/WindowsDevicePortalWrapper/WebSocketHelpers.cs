using System;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000006 RID: 6
	internal static class WebSocketHelpers
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00006FA4 File Offset: 0x000051A4
		internal static string GetSecWebSocketAcceptString(string secWebSocketKey)
		{
			string result;
			using (SHA1 sha = SHA1.Create())
			{
				string s = secWebSocketKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				result = Convert.ToBase64String(sha.ComputeHash(bytes));
			}
			return result;
		}

		// Token: 0x040000EC RID: 236
		internal const string SecWebSocketKeyGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

		// Token: 0x040000ED RID: 237
		internal const string WebSocketUpgradeToken = "websocket";

		// Token: 0x040000EE RID: 238
		internal const int DefaultReceiveBufferSize = 16384;

		// Token: 0x040000EF RID: 239
		internal const int DefaultClientSendBufferSize = 16384;

		// Token: 0x040000F0 RID: 240
		internal const int MaxControlFramePayloadLength = 123;
	}
}
