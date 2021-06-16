using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B9 RID: 185
	public class JRaw : JValue
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x0002AD9C File Offset: 0x00028F9C
		public static async Task<JRaw> CreateAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			JRaw result;
			using (StringWriter sw = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
				{
					await jsonWriter.WriteTokenSyncReadingAsync(reader, cancellationToken).ConfigureAwait(false);
					result = new JRaw(sw.ToString());
				}
			}
			return result;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0002ADE9 File Offset: 0x00028FE9
		public JRaw(JRaw other) : base(other)
		{
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0002ADF2 File Offset: 0x00028FF2
		public JRaw(object rawJson) : base(rawJson, JTokenType.Raw)
		{
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0002AE00 File Offset: 0x00029000
		public static JRaw Create(JsonReader reader)
		{
			JRaw result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					result = new JRaw(stringWriter.ToString());
				}
			}
			return result;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0002AE68 File Offset: 0x00029068
		internal override JToken CloneToken()
		{
			return new JRaw(this);
		}
	}
}
