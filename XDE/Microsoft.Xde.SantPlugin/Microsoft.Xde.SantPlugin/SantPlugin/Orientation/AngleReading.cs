using System;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000023 RID: 35
	public struct AngleReading
	{
		// Token: 0x06000166 RID: 358 RVA: 0x000068DC File Offset: 0x00004ADC
		public override string ToString()
		{
			return string.Format("PanelGroup: {0}, Angle: {1}", this.PanelGroup, this.Angle);
		}

		// Token: 0x040000CC RID: 204
		public uint PanelGroup;

		// Token: 0x040000CD RID: 205
		public float Angle;
	}
}
