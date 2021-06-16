using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000090 RID: 144
	[DebuggerDisplay("Count={Count}")]
	[DebuggerTypeProxy(typeof(TargetRegistry<>.DebugView))]
	internal sealed class TargetRegistry<T>
	{
		// Token: 0x06000478 RID: 1144 RVA: 0x0001032A File Offset: 0x0000E52A
		internal TargetRegistry(ISourceBlock<T> owningSource)
		{
			this._owningSource = owningSource;
			this._targetInformation = new Dictionary<ITargetBlock<T>, TargetRegistry<T>.LinkedTargetInfo>();
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00010344 File Offset: 0x0000E544
		internal void Add(ref ITargetBlock<T> target, DataflowLinkOptions linkOptions)
		{
			TargetRegistry<T>.LinkedTargetInfo linkedTargetInfo;
			if (this._targetInformation.TryGetValue(target, out linkedTargetInfo))
			{
				target = new TargetRegistry<T>.NopLinkPropagator(this._owningSource, target);
			}
			TargetRegistry<T>.LinkedTargetInfo linkedTargetInfo2 = new TargetRegistry<T>.LinkedTargetInfo(target, linkOptions);
			this.AddToList(linkedTargetInfo2, linkOptions.Append);
			this._targetInformation.Add(target, linkedTargetInfo2);
			if (linkedTargetInfo2.RemainingMessages > 0)
			{
				this._linksWithRemainingMessages++;
			}
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockLinking<T>(this._owningSource, target);
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000103C8 File Offset: 0x0000E5C8
		internal void Remove(ITargetBlock<T> target, bool onlyIfReachedMaxMessages = false)
		{
			if (onlyIfReachedMaxMessages && this._linksWithRemainingMessages == 0)
			{
				return;
			}
			this.Remove_Slow(target, onlyIfReachedMaxMessages);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000103E0 File Offset: 0x0000E5E0
		private void Remove_Slow(ITargetBlock<T> target, bool onlyIfReachedMaxMessages)
		{
			TargetRegistry<T>.LinkedTargetInfo linkedTargetInfo;
			if (this._targetInformation.TryGetValue(target, out linkedTargetInfo))
			{
				if (!onlyIfReachedMaxMessages || linkedTargetInfo.RemainingMessages == 1)
				{
					this.RemoveFromList(linkedTargetInfo);
					this._targetInformation.Remove(target);
					if (linkedTargetInfo.RemainingMessages == 0)
					{
						this._linksWithRemainingMessages--;
					}
					DataflowEtwProvider log = DataflowEtwProvider.Log;
					if (log.IsEnabled())
					{
						log.DataflowBlockUnlinking<T>(this._owningSource, target);
						return;
					}
				}
				else if (linkedTargetInfo.RemainingMessages > 0)
				{
					linkedTargetInfo.RemainingMessages--;
				}
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00010468 File Offset: 0x0000E668
		internal TargetRegistry<T>.LinkedTargetInfo ClearEntryPoints()
		{
			TargetRegistry<T>.LinkedTargetInfo firstTarget = this._firstTarget;
			this._firstTarget = (this._lastTarget = null);
			this._targetInformation.Clear();
			this._linksWithRemainingMessages = 0;
			return firstTarget;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000104A0 File Offset: 0x0000E6A0
		internal void PropagateCompletion(TargetRegistry<T>.LinkedTargetInfo firstTarget)
		{
			Task completion = this._owningSource.Completion;
			for (TargetRegistry<T>.LinkedTargetInfo linkedTargetInfo = firstTarget; linkedTargetInfo != null; linkedTargetInfo = linkedTargetInfo.Next)
			{
				if (linkedTargetInfo.PropagateCompletion)
				{
					Common.PropagateCompletion(completion, linkedTargetInfo.Target, Common.AsyncExceptionHandler);
				}
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x000104E0 File Offset: 0x0000E6E0
		internal TargetRegistry<T>.LinkedTargetInfo FirstTargetNode
		{
			get
			{
				return this._firstTarget;
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000104E8 File Offset: 0x0000E6E8
		internal void AddToList(TargetRegistry<T>.LinkedTargetInfo node, bool append)
		{
			if (this._firstTarget == null && this._lastTarget == null)
			{
				this._lastTarget = node;
				this._firstTarget = node;
				return;
			}
			if (append)
			{
				node.Previous = this._lastTarget;
				this._lastTarget.Next = node;
				this._lastTarget = node;
				return;
			}
			node.Next = this._firstTarget;
			this._firstTarget.Previous = node;
			this._firstTarget = node;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00010558 File Offset: 0x0000E758
		internal void RemoveFromList(TargetRegistry<T>.LinkedTargetInfo node)
		{
			TargetRegistry<T>.LinkedTargetInfo previous = node.Previous;
			TargetRegistry<T>.LinkedTargetInfo next = node.Next;
			if (node.Previous != null)
			{
				node.Previous.Next = next;
				node.Previous = null;
			}
			if (node.Next != null)
			{
				node.Next.Previous = previous;
				node.Next = null;
			}
			if (this._firstTarget == node)
			{
				this._firstTarget = next;
			}
			if (this._lastTarget == node)
			{
				this._lastTarget = previous;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x000105C9 File Offset: 0x0000E7C9
		private int Count
		{
			get
			{
				return this._targetInformation.Count;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x000105D8 File Offset: 0x0000E7D8
		private ITargetBlock<T>[] TargetsForDebugger
		{
			get
			{
				ITargetBlock<T>[] array = new ITargetBlock<!0>[this.Count];
				int num = 0;
				for (TargetRegistry<T>.LinkedTargetInfo linkedTargetInfo = this._firstTarget; linkedTargetInfo != null; linkedTargetInfo = linkedTargetInfo.Next)
				{
					array[num++] = linkedTargetInfo.Target;
				}
				return array;
			}
		}

		// Token: 0x040001D4 RID: 468
		private readonly ISourceBlock<T> _owningSource;

		// Token: 0x040001D5 RID: 469
		private readonly Dictionary<ITargetBlock<T>, TargetRegistry<T>.LinkedTargetInfo> _targetInformation;

		// Token: 0x040001D6 RID: 470
		private TargetRegistry<T>.LinkedTargetInfo _firstTarget;

		// Token: 0x040001D7 RID: 471
		private TargetRegistry<T>.LinkedTargetInfo _lastTarget;

		// Token: 0x040001D8 RID: 472
		private int _linksWithRemainingMessages;

		// Token: 0x02000091 RID: 145
		internal sealed class LinkedTargetInfo
		{
			// Token: 0x06000483 RID: 1155 RVA: 0x00010614 File Offset: 0x0000E814
			internal LinkedTargetInfo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
			{
				this.Target = target;
				this.PropagateCompletion = linkOptions.PropagateCompletion;
				this.RemainingMessages = linkOptions.MaxMessages;
			}

			// Token: 0x040001D9 RID: 473
			internal readonly ITargetBlock<T> Target;

			// Token: 0x040001DA RID: 474
			internal readonly bool PropagateCompletion;

			// Token: 0x040001DB RID: 475
			internal int RemainingMessages;

			// Token: 0x040001DC RID: 476
			internal TargetRegistry<T>.LinkedTargetInfo Previous;

			// Token: 0x040001DD RID: 477
			internal TargetRegistry<T>.LinkedTargetInfo Next;
		}

		// Token: 0x02000092 RID: 146
		[DebuggerTypeProxy(typeof(TargetRegistry<>.NopLinkPropagator.DebugView))]
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class NopLinkPropagator : IPropagatorBlock<T, T>, ITargetBlock<!0>, IDataflowBlock, ISourceBlock<!0>, IDebuggerDisplay
		{
			// Token: 0x06000484 RID: 1156 RVA: 0x0001063B File Offset: 0x0000E83B
			internal NopLinkPropagator(ISourceBlock<T> owningSource, ITargetBlock<T> target)
			{
				this._owningSource = owningSource;
				this._target = target;
			}

			// Token: 0x06000485 RID: 1157 RVA: 0x00010651 File Offset: 0x0000E851
			DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
			{
				return this._target.OfferMessage(messageHeader, messageValue, this, consumeToAccept);
			}

			// Token: 0x06000486 RID: 1158 RVA: 0x00010663 File Offset: 0x0000E863
			T ISourceBlock<!0>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
			{
				return this._owningSource.ConsumeMessage(messageHeader, this, out messageConsumed);
			}

			// Token: 0x06000487 RID: 1159 RVA: 0x00010673 File Offset: 0x0000E873
			bool ISourceBlock<!0>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
			{
				return this._owningSource.ReserveMessage(messageHeader, this);
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x00010682 File Offset: 0x0000E882
			void ISourceBlock<!0>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
			{
				this._owningSource.ReleaseReservation(messageHeader, this);
			}

			// Token: 0x17000182 RID: 386
			// (get) Token: 0x06000489 RID: 1161 RVA: 0x00010691 File Offset: 0x0000E891
			Task IDataflowBlock.Completion
			{
				get
				{
					return this._owningSource.Completion;
				}
			}

			// Token: 0x0600048A RID: 1162 RVA: 0x0001069E File Offset: 0x0000E89E
			void IDataflowBlock.Complete()
			{
				this._target.Complete();
			}

			// Token: 0x0600048B RID: 1163 RVA: 0x000106AB File Offset: 0x0000E8AB
			void IDataflowBlock.Fault(Exception exception)
			{
				this._target.Fault(exception);
			}

			// Token: 0x0600048C RID: 1164 RVA: 0x000033CB File Offset: 0x000015CB
			IDisposable ISourceBlock<!0>.LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
			{
				throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
			}

			// Token: 0x17000183 RID: 387
			// (get) Token: 0x0600048D RID: 1165 RVA: 0x000106BC File Offset: 0x0000E8BC
			private object DebuggerDisplayContent
			{
				get
				{
					IDebuggerDisplay debuggerDisplay = this._owningSource as IDebuggerDisplay;
					IDebuggerDisplay debuggerDisplay2 = this._target as IDebuggerDisplay;
					return string.Format("{0} Source=\"{1}\", Target=\"{2}\"", Common.GetNameForDebugger(this, null), (debuggerDisplay != null) ? debuggerDisplay.Content : this._owningSource, (debuggerDisplay2 != null) ? debuggerDisplay2.Content : this._target);
				}
			}

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x0600048E RID: 1166 RVA: 0x00010714 File Offset: 0x0000E914
			object IDebuggerDisplay.Content
			{
				get
				{
					return this.DebuggerDisplayContent;
				}
			}

			// Token: 0x040001DE RID: 478
			private readonly ISourceBlock<T> _owningSource;

			// Token: 0x040001DF RID: 479
			private readonly ITargetBlock<T> _target;

			// Token: 0x02000093 RID: 147
			private sealed class DebugView
			{
				// Token: 0x0600048F RID: 1167 RVA: 0x0001071C File Offset: 0x0000E91C
				public DebugView(TargetRegistry<T>.NopLinkPropagator passthrough)
				{
					this._passthrough = passthrough;
				}

				// Token: 0x17000185 RID: 389
				// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001072B File Offset: 0x0000E92B
				public ITargetBlock<T> LinkedTarget
				{
					get
					{
						return this._passthrough._target;
					}
				}

				// Token: 0x040001E0 RID: 480
				private readonly TargetRegistry<T>.NopLinkPropagator _passthrough;
			}
		}

		// Token: 0x02000094 RID: 148
		private sealed class DebugView
		{
			// Token: 0x06000491 RID: 1169 RVA: 0x00010738 File Offset: 0x0000E938
			public DebugView(TargetRegistry<T> registry)
			{
				this._registry = registry;
			}

			// Token: 0x17000186 RID: 390
			// (get) Token: 0x06000492 RID: 1170 RVA: 0x00010747 File Offset: 0x0000E947
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public ITargetBlock<T>[] Targets
			{
				get
				{
					return this._registry.TargetsForDebugger;
				}
			}

			// Token: 0x040001E1 RID: 481
			private readonly TargetRegistry<T> _registry;
		}
	}
}
