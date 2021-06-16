using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001FF RID: 511
	internal class History
	{
		// Token: 0x060017BA RID: 6074 RVA: 0x0009276C File Offset: 0x0009096C
		internal History(System.Management.Automation.ExecutionContext context)
		{
			Collection<Attribute> collection = new Collection<Attribute>();
			collection.Add(new ValidateRangeAttribute(1, 32767));
			PSVariable psvariable = new PSVariable("MaximumHistoryCount", 4096, ScopedItemOptions.None, collection);
			psvariable.Description = SessionStateStrings.MaxHistoryCountDescription;
			context.EngineSessionState.SetVariable(psvariable, false, CommandOrigin.Internal);
			this._capacity = 4096;
			this._buffer = new HistoryInfo[this._capacity];
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x000927F8 File Offset: 0x000909F8
		internal long AddEntry(long pipelineId, string cmdline, PipelineState status, DateTime startTime, DateTime endTime, bool skipIfLocked)
		{
			if (!Monitor.TryEnter(this._syncRoot, skipIfLocked ? 0 : -1))
			{
				return -1L;
			}
			long result;
			try
			{
				this.ReallocateBufferIfNeeded();
				HistoryInfo entry = new HistoryInfo(pipelineId, cmdline, status, startTime, endTime);
				result = this.Add(entry);
			}
			finally
			{
				Monitor.Exit(this._syncRoot);
			}
			return result;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00092858 File Offset: 0x00090A58
		internal void UpdateEntry(long id, PipelineState status, DateTime endTime, bool skipIfLocked)
		{
			if (!Monitor.TryEnter(this._syncRoot, skipIfLocked ? 0 : -1))
			{
				return;
			}
			try
			{
				HistoryInfo historyInfo = this.CoreGetEntry(id);
				if (historyInfo != null)
				{
					historyInfo.SetStatus(status);
					historyInfo.SetEndTime(endTime);
				}
			}
			finally
			{
				Monitor.Exit(this._syncRoot);
			}
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x000928B4 File Offset: 0x00090AB4
		internal HistoryInfo GetEntry(long id)
		{
			HistoryInfo result;
			lock (this._syncRoot)
			{
				this.ReallocateBufferIfNeeded();
				HistoryInfo historyInfo = this.CoreGetEntry(id);
				if (historyInfo != null && !historyInfo.Cleared)
				{
					result = historyInfo.Clone();
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00092914 File Offset: 0x00090B14
		internal HistoryInfo[] GetEntries(long id, long count, SwitchParameter newest)
		{
			this.ReallocateBufferIfNeeded();
			if (count < -1L)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("count", count);
			}
			if (newest.ToString() == null)
			{
				throw PSTraceSource.NewArgumentNullException("newest");
			}
			if (count == -1L || count > this._countEntriesAdded || count > (long)this._countEntriesInBuffer)
			{
				count = (long)this._countEntriesInBuffer;
			}
			if (count == 0L || this._countEntriesInBuffer == 0)
			{
				return new HistoryInfo[0];
			}
			HistoryInfo[] result;
			lock (this._syncRoot)
			{
				List<HistoryInfo> list = new List<HistoryInfo>();
				if (id > 0L)
				{
					if (!newest.IsPresent)
					{
						long num = id - count + 1L;
						if (num < 1L)
						{
							num = 1L;
						}
						long num2 = id;
						while (num2 >= num && num > 1L)
						{
							if (this._buffer[this.GetIndexFromId(num2)] != null && this._buffer[this.GetIndexFromId(num2)].Cleared)
							{
								num -= 1L;
							}
							num2 -= 1L;
						}
						for (long num3 = num; num3 <= id; num3 += 1L)
						{
							if (this._buffer[this.GetIndexFromId(num3)] != null && !this._buffer[this.GetIndexFromId(num3)].Cleared)
							{
								list.Add(this._buffer[this.GetIndexFromId(num3)].Clone());
							}
						}
					}
					else
					{
						long num = id + count - 1L;
						if (num >= this._countEntriesAdded)
						{
							num = this._countEntriesAdded;
						}
						long num4 = id;
						while (num4 <= num && num < this._countEntriesAdded)
						{
							if (this._buffer[this.GetIndexFromId(num4)] != null && this._buffer[this.GetIndexFromId(num4)].Cleared)
							{
								num += 1L;
							}
							num4 += 1L;
						}
						for (long num5 = num; num5 >= id; num5 -= 1L)
						{
							if (this._buffer[this.GetIndexFromId(num5)] != null && !this._buffer[this.GetIndexFromId(num5)].Cleared)
							{
								list.Add(this._buffer[this.GetIndexFromId(num5)].Clone());
							}
						}
					}
				}
				else
				{
					long num6 = 0L;
					if (this._capacity != 4096)
					{
						num6 = this.SmallestIDinBuffer();
					}
					if (!newest.IsPresent)
					{
						long num7 = 1L;
						if (this._capacity != 4096 && this._countEntriesAdded > (long)this._capacity)
						{
							num7 = num6;
						}
						long num8 = count - 1L;
						while (num8 >= 0L)
						{
							if (num7 > this._countEntriesAdded)
							{
								break;
							}
							if (num7 <= 0L || this.GetIndexFromId(num7) >= this._buffer.Length || this._buffer[this.GetIndexFromId(num7)].Cleared)
							{
								num7 += 1L;
							}
							else
							{
								list.Add(this._buffer[this.GetIndexFromId(num7)].Clone());
								num8 -= 1L;
								num7 += 1L;
							}
						}
					}
					else
					{
						long num7 = this._countEntriesAdded;
						long num9 = count - 1L;
						while (num9 >= 0L && (this._capacity == 4096 || this._countEntriesAdded <= (long)this._capacity || num7 >= num6) && num7 >= 1L)
						{
							if (num7 <= 0L || this.GetIndexFromId(num7) >= this._buffer.Length || this._buffer[this.GetIndexFromId(num7)].Cleared)
							{
								num7 -= 1L;
							}
							else
							{
								list.Add(this._buffer[this.GetIndexFromId(num7)].Clone());
								num9 -= 1L;
								num7 -= 1L;
							}
						}
					}
				}
				HistoryInfo[] array = new HistoryInfo[list.Count];
				list.CopyTo(array);
				result = array;
			}
			return result;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00092CC4 File Offset: 0x00090EC4
		internal HistoryInfo[] GetEntries(WildcardPattern wildcardpattern, long count, SwitchParameter newest)
		{
			HistoryInfo[] result;
			lock (this._syncRoot)
			{
				if (count < -1L)
				{
					throw PSTraceSource.NewArgumentOutOfRangeException("count", count);
				}
				if (newest.ToString() == null)
				{
					throw PSTraceSource.NewArgumentNullException("newest");
				}
				if (count > this._countEntriesAdded || count == -1L)
				{
					count = (long)this._countEntriesInBuffer;
				}
				List<HistoryInfo> list = new List<HistoryInfo>();
				long num = 1L;
				if (this._capacity != 4096)
				{
					num = this.SmallestIDinBuffer();
				}
				if (count != 0L)
				{
					if (!newest.IsPresent)
					{
						long num2 = 1L;
						if (this._capacity != 4096 && this._countEntriesAdded > (long)this._capacity)
						{
							num2 = num;
						}
						long num3 = 0L;
						while (num3 <= count - 1L)
						{
							if (num2 > this._countEntriesAdded)
							{
								break;
							}
							if (!this._buffer[this.GetIndexFromId(num2)].Cleared && wildcardpattern.IsMatch(this._buffer[this.GetIndexFromId(num2)].CommandLine.Trim()))
							{
								list.Add(this._buffer[this.GetIndexFromId(num2)].Clone());
								num3 += 1L;
							}
							num2 += 1L;
						}
					}
					else
					{
						long num4 = this._countEntriesAdded;
						long num5 = 0L;
						while (num5 <= count - 1L)
						{
							if ((this._capacity != 4096 && this._countEntriesAdded > (long)this._capacity && num4 < num) || num4 < 1L)
							{
								break;
							}
							if (!this._buffer[this.GetIndexFromId(num4)].Cleared && wildcardpattern.IsMatch(this._buffer[this.GetIndexFromId(num4)].CommandLine.Trim()))
							{
								list.Add(this._buffer[this.GetIndexFromId(num4)].Clone());
								num5 += 1L;
							}
							num4 -= 1L;
						}
					}
				}
				else
				{
					for (long num6 = 1L; num6 <= this._countEntriesAdded; num6 += 1L)
					{
						if (!this._buffer[this.GetIndexFromId(num6)].Cleared && wildcardpattern.IsMatch(this._buffer[this.GetIndexFromId(num6)].CommandLine.Trim()))
						{
							list.Add(this._buffer[this.GetIndexFromId(num6)].Clone());
						}
					}
				}
				HistoryInfo[] array = new HistoryInfo[list.Count];
				list.CopyTo(array);
				result = array;
			}
			return result;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00092F4C File Offset: 0x0009114C
		internal void ClearEntry(long id)
		{
			lock (this._syncRoot)
			{
				if (id < 0L)
				{
					throw PSTraceSource.NewArgumentOutOfRangeException("id", id);
				}
				if (this._countEntriesInBuffer != 0)
				{
					if (id <= this._countEntriesAdded)
					{
						HistoryInfo historyInfo = this.CoreGetEntry(id);
						if (historyInfo != null)
						{
							historyInfo.Cleared = true;
							this._countEntriesInBuffer--;
						}
					}
				}
			}
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x00092FD4 File Offset: 0x000911D4
		internal int Buffercapacity()
		{
			return this._capacity;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x00092FDC File Offset: 0x000911DC
		private long Add(HistoryInfo entry)
		{
			if (entry == null)
			{
				throw PSTraceSource.NewArgumentNullException("entry");
			}
			this._buffer[this.GetIndexForNewEntry()] = entry;
			this._countEntriesAdded += 1L;
			entry.SetId(this._countEntriesAdded);
			this.IncrementCountOfEntriesInBuffer();
			return this._countEntriesAdded;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0009302C File Offset: 0x0009122C
		private HistoryInfo CoreGetEntry(long id)
		{
			if (id <= 0L)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("id", id);
			}
			if (this._countEntriesInBuffer == 0)
			{
				return null;
			}
			if (id > this._countEntriesAdded)
			{
				return null;
			}
			return this._buffer[this.GetIndexFromId(id)];
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00093068 File Offset: 0x00091268
		private long SmallestIDinBuffer()
		{
			long num = 0L;
			if (this._buffer == null)
			{
				return num;
			}
			for (int i = 0; i < this._buffer.Length; i++)
			{
				if (this._buffer[i] != null && !this._buffer[i].Cleared)
				{
					num = this._buffer[i].Id;
					break;
				}
			}
			for (int j = 0; j < this._buffer.Length; j++)
			{
				if (this._buffer[j] != null && !this._buffer[j].Cleared && num > this._buffer[j].Id)
				{
					num = this._buffer[j].Id;
				}
			}
			return num;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0009310C File Offset: 0x0009130C
		private void ReallocateBufferIfNeeded()
		{
			int historySize = this.GetHistorySize();
			if (historySize == this._capacity)
			{
				return;
			}
			HistoryInfo[] array = new HistoryInfo[historySize];
			int num = this._countEntriesInBuffer;
			if ((long)num < this._countEntriesAdded)
			{
				num = (int)this._countEntriesAdded;
			}
			if (this._countEntriesInBuffer > historySize)
			{
				num = historySize;
			}
			for (int i = num; i > 0; i--)
			{
				long id = this._countEntriesAdded - (long)i + 1L;
				array[History.GetIndexFromId(id, historySize)] = this._buffer[this.GetIndexFromId(id)];
			}
			this._countEntriesInBuffer = num;
			this._capacity = historySize;
			this._buffer = array;
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x0009319D File Offset: 0x0009139D
		private int GetIndexForNewEntry()
		{
			return (int)(this._countEntriesAdded % (long)this._capacity);
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x000931AE File Offset: 0x000913AE
		private int GetIndexFromId(long id)
		{
			return (int)((id - 1L) % (long)this._capacity);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000931BD File Offset: 0x000913BD
		private static int GetIndexFromId(long id, int capacity)
		{
			return (int)((id - 1L) % (long)capacity);
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000931C7 File Offset: 0x000913C7
		private void IncrementCountOfEntriesInBuffer()
		{
			if (this._countEntriesInBuffer < this._capacity)
			{
				this._countEntriesInBuffer++;
			}
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000931E8 File Offset: 0x000913E8
		private int GetHistorySize()
		{
			int num = 0;
			System.Management.Automation.ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			object obj = (executionContextFromTLS != null) ? executionContextFromTLS.GetVariableValue(SpecialVariables.HistorySizeVarPath) : null;
			if (obj != null)
			{
				try
				{
					num = (int)LanguagePrimitives.ConvertTo(obj, typeof(int), CultureInfo.InvariantCulture);
				}
				catch (InvalidCastException)
				{
				}
			}
			if (num <= 0)
			{
				num = 4096;
			}
			return num;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00093250 File Offset: 0x00091450
		internal long GetNextHistoryId()
		{
			return this._countEntriesAdded + 1L;
		}

		// Token: 0x04000A05 RID: 2565
		internal const int DefaultHistorySize = 4096;

		// Token: 0x04000A06 RID: 2566
		private HistoryInfo[] _buffer;

		// Token: 0x04000A07 RID: 2567
		private int _capacity;

		// Token: 0x04000A08 RID: 2568
		private int _countEntriesInBuffer;

		// Token: 0x04000A09 RID: 2569
		private long _countEntriesAdded;

		// Token: 0x04000A0A RID: 2570
		private object _syncRoot = new object();
	}
}
