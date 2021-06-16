using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000019 RID: 25
	internal static class Utilities
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00007960 File Offset: 0x00005B60
		public static Uri BuildEndpoint(Uri baseUri, string path, string payload = null)
		{
			string relativeUri = (!string.IsNullOrWhiteSpace(payload)) ? string.Format("{0}?{1}", path, payload) : path;
			return new Uri(baseUri, relativeUri);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000798C File Offset: 0x00005B8C
		public static string BuildQueryString(Dictionary<string, string> payload)
		{
			string text = string.Empty;
			foreach (KeyValuePair<string, string> keyValuePair in payload)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair.Key,
					"=",
					keyValuePair.Value,
					"&"
				});
			}
			text = text.Trim(new char[]
			{
				'&'
			});
			return text;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007A20 File Offset: 0x00005C20
		public static string Hex64Encode(string str)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007A34 File Offset: 0x00005C34
		public static bool IsHoloLens(DevicePortal.DevicePortalPlatforms platform, string deviceFamily)
		{
			bool result = false;
			if (platform == DevicePortal.DevicePortalPlatforms.HoloLens || (platform == DevicePortal.DevicePortalPlatforms.VirtualMachine && deviceFamily == "Windows.Holographic"))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007A5C File Offset: 0x00005C5C
		public static void ModifyEndpointForFilename(ref string endpoint)
		{
			foreach (char oldChar in Path.GetInvalidFileNameChars())
			{
				endpoint = endpoint.Replace(oldChar, '_');
			}
			endpoint = endpoint.Replace('-', '_');
			endpoint = endpoint.Replace('.', '_');
			endpoint = endpoint.Replace('=', '_');
			endpoint = endpoint.Replace('&', '_');
		}
	}
}
