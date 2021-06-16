using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000C7 RID: 199
	internal class ArraySliceFilter : PathFilter
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0002F273 File Offset: 0x0002D473
		// (set) Token: 0x06000BC6 RID: 3014 RVA: 0x0002F27B File Offset: 0x0002D47B
		public int? Start { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x0002F284 File Offset: 0x0002D484
		// (set) Token: 0x06000BC8 RID: 3016 RVA: 0x0002F28C File Offset: 0x0002D48C
		public int? End { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x0002F295 File Offset: 0x0002D495
		// (set) Token: 0x06000BCA RID: 3018 RVA: 0x0002F29D File Offset: 0x0002D49D
		public int? Step { get; set; }

		// Token: 0x06000BCB RID: 3019 RVA: 0x0002F2A6 File Offset: 0x0002D4A6
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			int? num = this.Step;
			int num2 = 0;
			if (num.GetValueOrDefault() == num2 & num != null)
			{
				throw new JsonException("Step cannot be zero.");
			}
			foreach (JToken jtoken in current)
			{
				JArray a;
				if ((a = (jtoken as JArray)) != null)
				{
					int stepCount = this.Step ?? 1;
					int num3 = this.Start ?? ((stepCount > 0) ? 0 : (a.Count - 1));
					int stopIndex = this.End ?? ((stepCount > 0) ? a.Count : -1);
					num = this.Start;
					num2 = 0;
					if (num.GetValueOrDefault() < num2 & num != null)
					{
						num3 = a.Count + num3;
					}
					num = this.End;
					num2 = 0;
					if (num.GetValueOrDefault() < num2 & num != null)
					{
						stopIndex = a.Count + stopIndex;
					}
					num3 = Math.Max(num3, (stepCount > 0) ? 0 : int.MinValue);
					num3 = Math.Min(num3, (stepCount > 0) ? a.Count : (a.Count - 1));
					stopIndex = Math.Max(stopIndex, -1);
					stopIndex = Math.Min(stopIndex, a.Count);
					bool positiveStep = stepCount > 0;
					if (this.IsValid(num3, stopIndex, positiveStep))
					{
						int i = num3;
						while (this.IsValid(i, stopIndex, positiveStep))
						{
							yield return a[i];
							i += stepCount;
						}
					}
					else if (errorWhenNoMatch)
					{
						throw new JsonException("Array slice of {0} to {1} returned no results.".FormatWith(CultureInfo.InvariantCulture, (this.Start != null) ? this.Start.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) : "*", (this.End != null) ? this.End.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) : "*"));
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Array slice is not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, jtoken.GetType().Name));
				}
				a = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0002F2C4 File Offset: 0x0002D4C4
		private bool IsValid(int index, int stopIndex, bool positiveStep)
		{
			if (positiveStep)
			{
				return index < stopIndex;
			}
			return index > stopIndex;
		}
	}
}
