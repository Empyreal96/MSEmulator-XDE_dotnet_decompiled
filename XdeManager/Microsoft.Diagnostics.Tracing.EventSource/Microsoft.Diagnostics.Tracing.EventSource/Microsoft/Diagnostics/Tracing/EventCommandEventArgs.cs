using System;
using System.Collections.Generic;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000028 RID: 40
	public class EventCommandEventArgs : EventArgs
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000AE2E File Offset: 0x0000902E
		// (set) Token: 0x0600016C RID: 364 RVA: 0x0000AE36 File Offset: 0x00009036
		public EventCommand Command { get; internal set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000AE3F File Offset: 0x0000903F
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000AE47 File Offset: 0x00009047
		public IDictionary<string, string> Arguments { get; internal set; }

		// Token: 0x0600016F RID: 367 RVA: 0x0000AE50 File Offset: 0x00009050
		public bool EnableEvent(int eventId)
		{
			if (this.Command != EventCommand.Enable && this.Command != EventCommand.Disable)
			{
				throw new InvalidOperationException();
			}
			return this.eventSource.EnableEventForDispatcher(this.dispatcher, eventId, true);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000AE7F File Offset: 0x0000907F
		public bool DisableEvent(int eventId)
		{
			if (this.Command != EventCommand.Enable && this.Command != EventCommand.Disable)
			{
				throw new InvalidOperationException();
			}
			return this.eventSource.EnableEventForDispatcher(this.dispatcher, eventId, false);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000AEB0 File Offset: 0x000090B0
		internal EventCommandEventArgs(EventCommand command, IDictionary<string, string> arguments, EventSource eventSource, EventListener listener, int perEventSourceSessionId, int etwSessionId, bool enable, EventLevel level, EventKeywords matchAnyKeyword)
		{
			this.Command = command;
			this.Arguments = arguments;
			this.eventSource = eventSource;
			this.listener = listener;
			this.perEventSourceSessionId = perEventSourceSessionId;
			this.etwSessionId = etwSessionId;
			this.enable = enable;
			this.level = level;
			this.matchAnyKeyword = matchAnyKeyword;
		}

		// Token: 0x040000C0 RID: 192
		internal EventSource eventSource;

		// Token: 0x040000C1 RID: 193
		internal EventDispatcher dispatcher;

		// Token: 0x040000C2 RID: 194
		internal EventListener listener;

		// Token: 0x040000C3 RID: 195
		internal int perEventSourceSessionId;

		// Token: 0x040000C4 RID: 196
		internal int etwSessionId;

		// Token: 0x040000C5 RID: 197
		internal bool enable;

		// Token: 0x040000C6 RID: 198
		internal EventLevel level;

		// Token: 0x040000C7 RID: 199
		internal EventKeywords matchAnyKeyword;

		// Token: 0x040000C8 RID: 200
		internal EventCommandEventArgs nextCommand;
	}
}
