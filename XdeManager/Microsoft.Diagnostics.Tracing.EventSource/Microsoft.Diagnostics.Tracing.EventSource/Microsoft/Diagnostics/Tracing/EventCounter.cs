using System;
using System.Threading;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000013 RID: 19
	public class EventCounter
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x00008B64 File Offset: 0x00006D64
		public EventCounter(string name, EventSource eventSource)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (eventSource == null)
			{
				throw new ArgumentNullException("eventSource");
			}
			this.InitializeBuffer();
			this._name = name;
			EventCounterGroup.AddEventCounter(eventSource, this);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00008B9C File Offset: 0x00006D9C
		public void WriteMetric(float value)
		{
			this.Enqueue(value);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00008BA8 File Offset: 0x00006DA8
		private void InitializeBuffer()
		{
			this._bufferedValues = new float[10];
			checked
			{
				for (int i = 0; i < this._bufferedValues.Length; i++)
				{
					this._bufferedValues[i] = float.NegativeInfinity;
				}
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00008BE8 File Offset: 0x00006DE8
		private void Enqueue(float value)
		{
			int num = this._bufferedValuesIndex;
			checked
			{
				float num2;
				do
				{
					num2 = Interlocked.CompareExchange(ref this._bufferedValues[num], value, float.NegativeInfinity);
					num++;
					if (this._bufferedValues.Length <= num)
					{
						lock (this._bufferedValues)
						{
							this.Flush();
						}
						num = 0;
					}
				}
				while (num2 != float.NegativeInfinity);
				this._bufferedValuesIndex = num;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00008C74 File Offset: 0x00006E74
		private void Flush()
		{
			checked
			{
				for (int i = 0; i < this._bufferedValues.Length; i++)
				{
					float num = Interlocked.Exchange(ref this._bufferedValues[i], float.NegativeInfinity);
					if (num != float.NegativeInfinity)
					{
						this.OnMetricWritten(num);
					}
				}
				this._bufferedValuesIndex = 0;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00008CC8 File Offset: 0x00006EC8
		private void OnMetricWritten(float value)
		{
			this._sum += value;
			this._sumSquared += value * value;
			if (this._count == 0 || value > this._max)
			{
				this._max = value;
			}
			if (this._count == 0 || value < this._min)
			{
				this._min = value;
			}
			checked
			{
				this._count++;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00008D34 File Offset: 0x00006F34
		internal EventCounterPayload GetEventCounterPayload()
		{
			EventCounterPayload result;
			lock (this._bufferedValues)
			{
				this.Flush();
				EventCounterPayload eventCounterPayload = new EventCounterPayload();
				eventCounterPayload.Name = this._name;
				eventCounterPayload.Count = this._count;
				eventCounterPayload.Mean = this._sum / (float)this._count;
				eventCounterPayload.StandardDerivation = (float)Math.Sqrt((double)(this._sumSquared / (float)this._count - this._sum * this._sum / (float)this._count / (float)this._count));
				eventCounterPayload.Min = this._min;
				eventCounterPayload.Max = this._max;
				this.ResetStatistics();
				result = eventCounterPayload;
			}
			return result;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00008E04 File Offset: 0x00007004
		private void ResetStatistics()
		{
			this._count = 0;
			this._sum = 0f;
			this._sumSquared = 0f;
			this._min = 0f;
			this._max = 0f;
		}

		// Token: 0x04000066 RID: 102
		private const int BufferedSize = 10;

		// Token: 0x04000067 RID: 103
		private const float UnusedBufferSlotValue = float.NegativeInfinity;

		// Token: 0x04000068 RID: 104
		private const int UnsetIndex = -1;

		// Token: 0x04000069 RID: 105
		private readonly string _name;

		// Token: 0x0400006A RID: 106
		private volatile float[] _bufferedValues;

		// Token: 0x0400006B RID: 107
		private volatile int _bufferedValuesIndex;

		// Token: 0x0400006C RID: 108
		private int _count;

		// Token: 0x0400006D RID: 109
		private float _sum;

		// Token: 0x0400006E RID: 110
		private float _sumSquared;

		// Token: 0x0400006F RID: 111
		private float _min;

		// Token: 0x04000070 RID: 112
		private float _max;
	}
}
