using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000C8 RID: 200
	internal class FieldFilter : PathFilter
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0002F2DA File Offset: 0x0002D4DA
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x0002F2E2 File Offset: 0x0002D4E2
		public string Name { get; set; }

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0002F2EB File Offset: 0x0002D4EB
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken jtoken in current)
			{
				JObject jobject;
				if ((jobject = (jtoken as JObject)) != null)
				{
					if (this.Name != null)
					{
						JToken jtoken2 = jobject[this.Name];
						if (jtoken2 != null)
						{
							yield return jtoken2;
						}
						else if (errorWhenNoMatch)
						{
							throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, this.Name));
						}
					}
					else
					{
						foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
						{
							yield return keyValuePair.Value;
						}
						IEnumerator<KeyValuePair<string, JToken>> enumerator2 = null;
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Property '{0}' not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, this.Name ?? "*", jtoken.GetType().Name));
				}
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}
	}
}
