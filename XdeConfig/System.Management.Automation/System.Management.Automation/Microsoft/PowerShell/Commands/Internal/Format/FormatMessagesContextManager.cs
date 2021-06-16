using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004F8 RID: 1272
	internal class FormatMessagesContextManager
	{
		// Token: 0x06003673 RID: 13939 RVA: 0x001271F8 File Offset: 0x001253F8
		internal void Process(object o)
		{
			PacketInfoData packetInfoData = o as PacketInfoData;
			FormatEntryData formatEntryData = packetInfoData as FormatEntryData;
			if (formatEntryData != null)
			{
				FormatMessagesContextManager.OutputContext c = null;
				if (!formatEntryData.outOfBand)
				{
					c = this.stack.Peek();
				}
				this.payload(formatEntryData, c);
				return;
			}
			bool flag = packetInfoData is FormatStartData;
			bool flag2 = packetInfoData is GroupStartData;
			if (flag || flag2)
			{
				FormatMessagesContextManager.OutputContext outputContext = this.contextCreation(this.ActiveOutputContext, packetInfoData);
				this.stack.Push(outputContext);
				if (flag)
				{
					this.fs(outputContext);
					return;
				}
				if (flag2)
				{
					this.gs(outputContext);
					return;
				}
			}
			else
			{
				GroupEndData groupEndData = packetInfoData as GroupEndData;
				FormatEndData formatEndData = packetInfoData as FormatEndData;
				if (groupEndData != null || formatEndData != null)
				{
					FormatMessagesContextManager.OutputContext c2 = this.stack.Peek();
					if (formatEndData != null)
					{
						this.fe(formatEndData, c2);
					}
					else if (groupEndData != null)
					{
						this.ge(groupEndData, c2);
					}
					this.stack.Pop();
				}
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06003674 RID: 13940 RVA: 0x001272F3 File Offset: 0x001254F3
		internal FormatMessagesContextManager.OutputContext ActiveOutputContext
		{
			get
			{
				if (this.stack.Count <= 0)
				{
					return null;
				}
				return this.stack.Peek();
			}
		}

		// Token: 0x04001BEA RID: 7146
		internal FormatMessagesContextManager.FormatContextCreationCallback contextCreation;

		// Token: 0x04001BEB RID: 7147
		internal FormatMessagesContextManager.FormatStartCallback fs;

		// Token: 0x04001BEC RID: 7148
		internal FormatMessagesContextManager.FormatEndCallback fe;

		// Token: 0x04001BED RID: 7149
		internal FormatMessagesContextManager.GroupStartCallback gs;

		// Token: 0x04001BEE RID: 7150
		internal FormatMessagesContextManager.GroupEndCallback ge;

		// Token: 0x04001BEF RID: 7151
		internal FormatMessagesContextManager.PayloadCallback payload;

		// Token: 0x04001BF0 RID: 7152
		private Stack<FormatMessagesContextManager.OutputContext> stack = new Stack<FormatMessagesContextManager.OutputContext>();

		// Token: 0x020004F9 RID: 1273
		// (Invoke) Token: 0x06003677 RID: 13943
		internal delegate FormatMessagesContextManager.OutputContext FormatContextCreationCallback(FormatMessagesContextManager.OutputContext parentContext, FormatInfoData formatData);

		// Token: 0x020004FA RID: 1274
		// (Invoke) Token: 0x0600367B RID: 13947
		internal delegate void FormatStartCallback(FormatMessagesContextManager.OutputContext c);

		// Token: 0x020004FB RID: 1275
		// (Invoke) Token: 0x0600367F RID: 13951
		internal delegate void FormatEndCallback(FormatEndData fe, FormatMessagesContextManager.OutputContext c);

		// Token: 0x020004FC RID: 1276
		// (Invoke) Token: 0x06003683 RID: 13955
		internal delegate void GroupStartCallback(FormatMessagesContextManager.OutputContext c);

		// Token: 0x020004FD RID: 1277
		// (Invoke) Token: 0x06003687 RID: 13959
		internal delegate void GroupEndCallback(GroupEndData fe, FormatMessagesContextManager.OutputContext c);

		// Token: 0x020004FE RID: 1278
		// (Invoke) Token: 0x0600368B RID: 13963
		internal delegate void PayloadCallback(FormatEntryData formatEntryData, FormatMessagesContextManager.OutputContext c);

		// Token: 0x020004FF RID: 1279
		internal abstract class OutputContext
		{
			// Token: 0x0600368E RID: 13966 RVA: 0x00127323 File Offset: 0x00125523
			internal OutputContext(FormatMessagesContextManager.OutputContext parentContextInStack)
			{
				this.parentContext = parentContextInStack;
			}

			// Token: 0x17000C17 RID: 3095
			// (get) Token: 0x0600368F RID: 13967 RVA: 0x00127332 File Offset: 0x00125532
			internal FormatMessagesContextManager.OutputContext ParentContext
			{
				get
				{
					return this.parentContext;
				}
			}

			// Token: 0x04001BF1 RID: 7153
			private FormatMessagesContextManager.OutputContext parentContext;
		}
	}
}
