using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000093 RID: 147
	public class MemoryTraceWriter : ITraceWriter
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00023BEA File Offset: 0x00021DEA
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x00023BF2 File Offset: 0x00021DF2
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06000805 RID: 2053 RVA: 0x00023BFB File Offset: 0x00021DFB
		public MemoryTraceWriter()
		{
			this.LevelFilter = TraceLevel.Verbose;
			this._traceMessages = new Queue<string>();
			this._lock = new object();
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00023C20 File Offset: 0x00021E20
		public void Trace(TraceLevel level, string message, Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", CultureInfo.InvariantCulture));
			stringBuilder.Append(" ");
			stringBuilder.Append(level.ToString("g"));
			stringBuilder.Append(" ");
			stringBuilder.Append(message);
			string item = stringBuilder.ToString();
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._traceMessages.Count >= 1000)
				{
					this._traceMessages.Dequeue();
				}
				this._traceMessages.Enqueue(item);
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00023CE4 File Offset: 0x00021EE4
		public IEnumerable<string> GetTraceMessages()
		{
			return this._traceMessages;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00023CEC File Offset: 0x00021EEC
		public override string ToString()
		{
			object @lock = this._lock;
			string result;
			lock (@lock)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string value in this._traceMessages)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(value);
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x040002BD RID: 701
		private readonly Queue<string> _traceMessages;

		// Token: 0x040002BE RID: 702
		private readonly object _lock;
	}
}
