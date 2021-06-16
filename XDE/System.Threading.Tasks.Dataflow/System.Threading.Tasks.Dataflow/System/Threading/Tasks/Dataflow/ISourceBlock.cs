using System;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Represents a dataflow block that is a source of data.</summary>
	/// <typeparam name="TOutput">Specifies the type of data supplied by the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" />.This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam>
	// Token: 0x02000031 RID: 49
	public interface ISourceBlock<out TOutput> : IDataflowBlock
	{
		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>An IDisposable that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect this source.</param>
		/// <param name="linkOptions">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowLinkOptions" /> instance that configures the link.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null (Nothing in Visual Basic) or <paramref name="linkOptions" /> is null (Nothing in Visual Basic).</exception>
		// Token: 0x06000124 RID: 292
		IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions);

		/// <summary>Called by a linked <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to accept and consume a <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> previously offered by this <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" />.</summary>
		/// <returns>The value of the consumed message. This may correspond to a different <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance than was previously reserved and passed as the <paramref name="messageHeader" /> to <see cref="M:System.Threading.Tasks.Dataflow.ISourceBlock`1.ConsumeMessage()" />. The consuming <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> must use the returned value instead of the value passed as <paramref name="messageValue" /> through <see cref="M:System.Threading.Tasks.Dataflow.ITargetBlock`1.OfferMessage()" />.If the message requested is not available, the return value will be null.</returns>
		/// <param name="messageHeader">The <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> of the message being consumed.</param>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> consuming the message.</param>
		/// <param name="messageConsumed">true if the message was successfully consumed; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="messageHeader" /> is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> is null.</exception>
		// Token: 0x06000125 RID: 293
		TOutput ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed);

		/// <summary>Called by a linked <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to reserve a previously offered <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> by this <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" />.</summary>
		/// <returns>true if the message was successfully reserved; otherwise, false.</returns>
		/// <param name="messageHeader">The <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> of the message being reserved.</param>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> reserving the message.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="messageHeader" /> is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> is null.</exception>
		// Token: 0x06000126 RID: 294
		bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target);

		/// <summary>Called by a linked <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to release a previously reserved <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> by this <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" />.</summary>
		/// <param name="messageHeader">The <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> of the reserved message being released.</param>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> releasing the message it previously reserved.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="messageHeader" /> is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="target" /> did not have the message reserved.</exception>
		// Token: 0x06000127 RID: 295
		void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target);
	}
}
