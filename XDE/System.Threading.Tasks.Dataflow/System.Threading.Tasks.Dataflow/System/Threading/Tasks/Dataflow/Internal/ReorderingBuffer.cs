using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000083 RID: 131
	[DebuggerDisplay("Count={CountForDebugging}")]
	[DebuggerTypeProxy(typeof(ReorderingBuffer<>.DebugView))]
	internal sealed class ReorderingBuffer<TOutput> : IReorderingBuffer
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000DDCA File Offset: 0x0000BFCA
		private object ValueLock
		{
			get
			{
				return this._reorderingBuffer;
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000DDD2 File Offset: 0x0000BFD2
		internal ReorderingBuffer(object owningSource, Action<object, TOutput> outputAction)
		{
			this._owningSource = owningSource;
			this._outputAction = outputAction;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000DDF4 File Offset: 0x0000BFF4
		internal void AddItem(long id, TOutput item, bool itemIsValid)
		{
			object valueLock = this.ValueLock;
			lock (valueLock)
			{
				if (this._nextReorderedIdToOutput == id)
				{
					this.OutputNextItem(item, itemIsValid);
				}
				else
				{
					this._reorderingBuffer.Add(id, new KeyValuePair<bool, TOutput>(itemIsValid, item));
				}
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000DE54 File Offset: 0x0000C054
		internal bool? AddItemIfNextAndTrusted(long id, TOutput item, bool isTrusted)
		{
			object valueLock = this.ValueLock;
			bool? flag2;
			lock (valueLock)
			{
				if (this._nextReorderedIdToOutput == id)
				{
					if (isTrusted)
					{
						this.OutputNextItem(item, true);
						flag2 = null;
						flag2 = flag2;
					}
					else
					{
						flag2 = new bool?(true);
					}
				}
				else
				{
					flag2 = new bool?(false);
				}
			}
			return flag2;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000DEC0 File Offset: 0x0000C0C0
		public void IgnoreItem(long id)
		{
			this.AddItem(id, default(TOutput), false);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000DEE0 File Offset: 0x0000C0E0
		private void OutputNextItem(TOutput theNextItem, bool itemIsValid)
		{
			this._nextReorderedIdToOutput += 1L;
			if (itemIsValid)
			{
				this._outputAction(this._owningSource, theNextItem);
			}
			KeyValuePair<bool, TOutput> keyValuePair;
			while (this._reorderingBuffer.TryGetValue(this._nextReorderedIdToOutput, out keyValuePair))
			{
				this._reorderingBuffer.Remove(this._nextReorderedIdToOutput);
				this._nextReorderedIdToOutput += 1L;
				if (keyValuePair.Key)
				{
					this._outputAction(this._owningSource, keyValuePair.Value);
				}
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000DF6A File Offset: 0x0000C16A
		private int CountForDebugging
		{
			get
			{
				return this._reorderingBuffer.Count;
			}
		}

		// Token: 0x04000192 RID: 402
		private readonly object _owningSource;

		// Token: 0x04000193 RID: 403
		private readonly Dictionary<long, KeyValuePair<bool, TOutput>> _reorderingBuffer = new Dictionary<long, KeyValuePair<bool, TOutput>>();

		// Token: 0x04000194 RID: 404
		private readonly Action<object, TOutput> _outputAction;

		// Token: 0x04000195 RID: 405
		private long _nextReorderedIdToOutput;

		// Token: 0x02000084 RID: 132
		private sealed class DebugView
		{
			// Token: 0x06000402 RID: 1026 RVA: 0x0000DF77 File Offset: 0x0000C177
			public DebugView(ReorderingBuffer<TOutput> buffer)
			{
				this._buffer = buffer;
			}

			// Token: 0x17000153 RID: 339
			// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000DF86 File Offset: 0x0000C186
			public Dictionary<long, KeyValuePair<bool, TOutput>> ItemsBuffered
			{
				get
				{
					return this._buffer._reorderingBuffer;
				}
			}

			// Token: 0x17000154 RID: 340
			// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000DF93 File Offset: 0x0000C193
			public long NextIdRequired
			{
				get
				{
					return this._buffer._nextReorderedIdToOutput;
				}
			}

			// Token: 0x04000196 RID: 406
			private readonly ReorderingBuffer<TOutput> _buffer;
		}
	}
}
