using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200047E RID: 1150
	public abstract class LoopFlowException : FlowControlException
	{
		// Token: 0x0600333C RID: 13116 RVA: 0x001181E5 File Offset: 0x001163E5
		internal LoopFlowException(string label)
		{
			this.Label = (label ?? "");
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x001181FD File Offset: 0x001163FD
		internal LoopFlowException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x00118207 File Offset: 0x00116407
		internal LoopFlowException()
		{
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x0011820F File Offset: 0x0011640F
		// (set) Token: 0x06003340 RID: 13120 RVA: 0x00118217 File Offset: 0x00116417
		public string Label { get; internal set; }

		// Token: 0x06003341 RID: 13121 RVA: 0x00118220 File Offset: 0x00116420
		internal bool MatchLabel(string loopLabel)
		{
			return LoopFlowException.MatchLoopLabel(this.Label, loopLabel);
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x0011822E File Offset: 0x0011642E
		internal static bool MatchLoopLabel(string flowLabel, string loopLabel)
		{
			return string.IsNullOrEmpty(flowLabel) || flowLabel.Equals(loopLabel, StringComparison.OrdinalIgnoreCase);
		}
	}
}
