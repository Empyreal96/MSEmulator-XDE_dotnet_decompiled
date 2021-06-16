using System;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Represents a dataflow block that is a target for data.</summary>
	/// <typeparam name="TInput">Specifies the type of data accepted by the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam>
	// Token: 0x02000032 RID: 50
	public interface ITargetBlock<in TInput> : IDataflowBlock
	{
		/// <summary>Offers a message to the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />, giving the target the opportunity to consume or postpone the message.</summary>
		/// <returns>The status of the offered message. If the message was accepted by the target, <see cref="F:System.Threading.Tasks.Dataflow.DataflowMessageStatus.Accepted" /> is returned, and the source should no longer use the offered message, because it is now owned by the target. If the message was postponed by the target, <see cref="F:System.Threading.Tasks.Dataflow.DataflowMessageStatus.Postponed" /> is returned as a notification that the target may later attempt to consume or reserve the message; in the meantime, the source still owns the message and may offer it to other blocks.If the target would have otherwise postponed message, but <paramref name="source" /> was null, <see cref="F:System.Threading.Tasks.Dataflow.DataflowMessageStatus.Declined" /> is instead returned. If the target tried to accept the message but missed it due to the source delivering the message to another target or simply discarding it, <see cref="F:System.Threading.Tasks.Dataflow.DataflowMessageStatus.NotAvailable" /> is returned. If the target chose not to accept the message, <see cref="F:System.Threading.Tasks.Dataflow.DataflowMessageStatus.Declined" /> is returned. If the target chose not to accept the message and will never accept another message from this source, <see cref="F:System.Threading.Tasks.Dataflow.DataflowMessageStatus.DecliningPermanently" /> is returned.</returns>
		/// <param name="messageHeader">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance that represents the header of the message being offered.</param>
		/// <param name="messageValue">The value of the message being offered.</param>
		/// <param name="source">The <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> offering the message. This may be null.</param>
		/// <param name="consumeToAccept">Set to true to instruct the target to call <see cref="M:System.Threading.Tasks.Dataflow.ISourceBlock`1.ConsumeMessage()" /> synchronously during the call to <see cref="M:System.Threading.Tasks.Dataflow.ITargetBlock`1.OfferMessage()" />, prior to returning <see cref="F:System.Threading.Tasks.Dataflow.DataflowMessageStatus.Accepted" />, in order to consume the message.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="messageHeader" /> is not valid.-or-<paramref name="consumeToAccept" /> may only be true if provided with a non-null <paramref name="source" />.</exception>
		// Token: 0x06000128 RID: 296
		DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept);
	}
}
