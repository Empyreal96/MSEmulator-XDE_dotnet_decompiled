using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a set of static (Shared in Visual Basic) methods for working with dataflow blocks.</summary>
	// Token: 0x0200000E RID: 14
	public static class DataflowBlock
	{
		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>An <see cref="T:System.IDisposable" /> that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="source">The source from which to link.</param>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect the source.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.-or-The <paramref name="target" /> is null.</exception>
		// Token: 0x06000036 RID: 54 RVA: 0x00002A28 File Offset: 0x00000C28
		public static IDisposable LinkTo<TOutput>(this ISourceBlock<TOutput> source, ITargetBlock<TOutput> target)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return source.LinkTo(target, DataflowLinkOptions.Default);
		}

		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> using the specified filter.</summary>
		/// <returns>An <see cref="T:System.IDisposable" /> that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="source">The source from which to link.</param>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect the source.</param>
		/// <param name="predicate">The filter a message must pass in order for it to propagate from the source to the target.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.-or-The <paramref name="target" /> is null.-or-The <paramref name="predicate" /> is null.</exception>
		// Token: 0x06000037 RID: 55 RVA: 0x00002A52 File Offset: 0x00000C52
		public static IDisposable LinkTo<TOutput>(this ISourceBlock<TOutput> source, ITargetBlock<TOutput> target, Predicate<TOutput> predicate)
		{
			return source.LinkTo(target, DataflowLinkOptions.Default, predicate);
		}

		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> using the specified filter.</summary>
		/// <returns>An <see cref="T:System.IDisposable" /> that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="source">The source from which to link.</param>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect the source.</param>
		/// <param name="linkOptions">One of the enumeration values that specifies how to configure a link between dataflow blocks.</param>
		/// <param name="predicate">The filter a message must pass in order for it to propagate from the source to the target.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null (Nothing in Visual Basic).-or-The <paramref name="target" /> is null (Nothing in Visual Basic).-or-The <paramref name="linkOptions" /> is null (Nothing in Visual Basic).-or-The <paramref name="predicate" /> is null (Nothing in Visual Basic).</exception>
		// Token: 0x06000038 RID: 56 RVA: 0x00002A64 File Offset: 0x00000C64
		public static IDisposable LinkTo<TOutput>(this ISourceBlock<TOutput> source, ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions, Predicate<TOutput> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (linkOptions == null)
			{
				throw new ArgumentNullException("linkOptions");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			DataflowBlock.FilteredLinkPropagator<TOutput> target2 = new DataflowBlock.FilteredLinkPropagator<TOutput>(source, target, predicate);
			return source.LinkTo(target2, linkOptions);
		}

		/// <summary>Posts an item to the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>true if the item was accepted by the target block; otherwise, false.</returns>
		/// <param name="target">The target block.</param>
		/// <param name="item">The item being offered to the target.</param>
		/// <typeparam name="TInput">Specifies the type of data accepted by the target block.</typeparam>
		// Token: 0x06000039 RID: 57 RVA: 0x00002ABA File Offset: 0x00000CBA
		public static bool Post<TInput>(this ITargetBlock<TInput> target, TInput item)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return target.OfferMessage(Common.SingleMessageHeader, item, null, false) == DataflowMessageStatus.Accepted;
		}

		/// <summary>Asynchronously offers a message to the target message block, allowing for postponement.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous send. If the target accepts and consumes the offered element during the call to <see cref="M:System.Threading.Tasks.Dataflow.DataflowBlock.SendAsync``1(System.Threading.Tasks.Dataflow.ITargetBlock{``0},``0)" />, upon return from the call the resulting <see cref="T:System.Threading.Tasks.Task`1" /> will be completed and its <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will return true. If the target declines the offered element during the call, upon return from the call the resulting <see cref="T:System.Threading.Tasks.Task`1" /> will be completed and its <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will return false. If the target postpones the offered element, the element will be buffered until such time that the target consumes or releases it, at which point the task will complete, with its <see cref="P:System.Threading.Tasks.Task`1.Result" /> indicating whether the message was consumed. If the target never attempts to consume or release the message, the returned task will never complete.</returns>
		/// <param name="target">The target to which to post the data.</param>
		/// <param name="item">The item being offered to the target.</param>
		/// <typeparam name="TInput">Specifies the type of the data to post to the target.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> is null.</exception>
		// Token: 0x0600003A RID: 58 RVA: 0x00002ADB File Offset: 0x00000CDB
		public static Task<bool> SendAsync<TInput>(this ITargetBlock<TInput> target, TInput item)
		{
			return target.SendAsync(item, CancellationToken.None);
		}

		/// <summary>Asynchronously offers a message to the target message block, allowing for postponement.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task{Boolean}" /> that represents the asynchronous send.  If the target accepts and consumes the offered element during the call to SendAsync, upon return from the call the resulting <see cref="T:System.Threading.Tasks.Task{Boolean}" /> will be completed and its Result property will return true.  If the target declines the offered element during the call, upon return from the call the resulting <see cref="T:System.Threading.Tasks.Task{Boolean}" /> will be completed and its Result property will return false. If the target postpones the offered element, the element will be buffered until such time that the target consumes or releases it, at which point the Task will complete, with its Result indicating whether the message was consumed. If the target never attempts to consume or release the message, the returned task will never complete.If cancellation is requested before the target has successfully consumed the sent data, the returned task will complete in the Canceled state and the data will no longer be available to the target.</returns>
		/// <param name="target">The target to which to post the data.</param>
		/// <param name="item">The item being offered to the target.</param>
		/// <param name="cancellationToken">The cancellation token with which to request cancellation of the send operation.</param>
		/// <typeparam name="TInput">Specifies the type of the data to post to the target.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> is null (Nothing in Visual Basic).</exception>
		// Token: 0x0600003B RID: 59 RVA: 0x00002AEC File Offset: 0x00000CEC
		public static Task<bool> SendAsync<TInput>(this ITargetBlock<TInput> target, TInput item, CancellationToken cancellationToken)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Common.CreateTaskFromCancellation<bool>(cancellationToken);
			}
			DataflowBlock.SendAsyncSource<TInput> sendAsyncSource;
			try
			{
				DataflowMessageStatus dataflowMessageStatus = target.OfferMessage(Common.SingleMessageHeader, item, null, false);
				if (dataflowMessageStatus == DataflowMessageStatus.Accepted)
				{
					return Common.CompletedTaskWithTrueResult;
				}
				if (dataflowMessageStatus == DataflowMessageStatus.DecliningPermanently)
				{
					return Common.CompletedTaskWithFalseResult;
				}
				sendAsyncSource = new DataflowBlock.SendAsyncSource<TInput>(target, item, cancellationToken);
			}
			catch (Exception ex)
			{
				Common.StoreDataflowMessageValueIntoExceptionData<TInput>(ex, item, false);
				return Common.CreateTaskFromException<bool>(ex);
			}
			sendAsyncSource.OfferToTarget();
			return sendAsyncSource.Task;
		}

		/// <summary>Attempts to synchronously receive an item from the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" />.</summary>
		/// <returns>true if an item could be received; otherwise, false.</returns>
		/// <param name="source">The source from which to receive.</param>
		/// <param name="item">The item received from the source.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		// Token: 0x0600003C RID: 60 RVA: 0x00002B7C File Offset: 0x00000D7C
		public static bool TryReceive<TOutput>(this IReceivableSourceBlock<TOutput> source, out TOutput item)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return source.TryReceive(null, out item);
		}

		/// <summary>Asynchronously receives a value from the specified source.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous receive operation. When an item is successfully received from the source, the returned task will be completed and its <see cref="P:System.Threading.Tasks.Task`1.Result" /> will return the received item. If an item cannot be retrieved, because the source is empty and completed, the returned task will be canceled.</returns>
		/// <param name="source">The source from which to asynchronously receive.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
		// Token: 0x0600003D RID: 61 RVA: 0x00002B94 File Offset: 0x00000D94
		public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source)
		{
			return source.ReceiveAsync(Common.InfiniteTimeSpan, CancellationToken.None);
		}

		/// <summary>Asynchronously receives a value from the specified source.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous receive operation. When an item is successfully received from the source, the returned task will be completed and its <see cref="P:System.Threading.Tasks.Task`1.Result" />will return the received item. If an item cannot be retrieved, either because cancellation is requested or the source is empty and completed, the returned task will be canceled.</returns>
		/// <param name="source">The source from which to asynchronously receive.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> which may be used to cancel the receive operation.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
		// Token: 0x0600003E RID: 62 RVA: 0x00002BA6 File Offset: 0x00000DA6
		public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source, CancellationToken cancellationToken)
		{
			return source.ReceiveAsync(Common.InfiniteTimeSpan, cancellationToken);
		}

		/// <summary>Asynchronously receives a value from the specified source.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous receive operation. When an item is successfully received from the source, the returned task will be completed and its <see cref="P:System.Threading.Tasks.Task`1.Result" /> will return the received item. If an item cannot be retrieved, either because the timeout expires or the source is empty and completed, the returned task will be canceled.</returns>
		/// <param name="source">The source from which to asynchronously receive.</param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out-or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x0600003F RID: 63 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout)
		{
			return source.ReceiveAsync(timeout, CancellationToken.None);
		}

		/// <summary>Asynchronously receives a value from the specified source.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous receive operation. When an item is successfully received from the source, the returned task will be completed and its <see cref="P:System.Threading.Tasks.Task`1.Result" /> will return the received item. If an item cannot be retrieved, either because the timeout expires, cancellation is requested, or the source is empty and completed, the returned task will be canceled.</returns>
		/// <param name="source">The source from which to asynchronously receive.</param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> which may be used to cancel the receive operation.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out-or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x06000040 RID: 64 RVA: 0x00002BC2 File Offset: 0x00000DC2
		public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout, CancellationToken cancellationToken)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (!Common.IsValidTimeout(timeout))
			{
				throw new ArgumentOutOfRangeException("timeout", SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
			}
			return source.ReceiveCore(true, timeout, cancellationToken);
		}

		/// <summary>Synchronously receives an item from the source.</summary>
		/// <returns>The received item.</returns>
		/// <param name="source">The source from which to receive.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No item could be received from the source.</exception>
		// Token: 0x06000041 RID: 65 RVA: 0x00002BF3 File Offset: 0x00000DF3
		public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source)
		{
			return source.Receive(Common.InfiniteTimeSpan, CancellationToken.None);
		}

		/// <summary>Synchronously receives an item from the source.</summary>
		/// <returns>The received item.</returns>
		/// <param name="source">The source from which to receive.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> which may be used to cancel the receive operation.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No item could be received from the source.</exception>
		/// <exception cref="T:System.OperationCanceledException">The operation was canceled before an item was received from the source.</exception>
		// Token: 0x06000042 RID: 66 RVA: 0x00002C05 File Offset: 0x00000E05
		public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source, CancellationToken cancellationToken)
		{
			return source.Receive(Common.InfiniteTimeSpan, cancellationToken);
		}

		/// <summary>Synchronously receives an item from the source.</summary>
		/// <returns>The received item.</returns>
		/// <param name="source">The source from which to receive.</param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out-or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No item could be received from the source.</exception>
		/// <exception cref="T:System.TimeoutException">The specified timeout expired before an item was received from the source.</exception>
		// Token: 0x06000043 RID: 67 RVA: 0x00002C13 File Offset: 0x00000E13
		public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout)
		{
			return source.Receive(timeout, CancellationToken.None);
		}

		/// <summary>Synchronously receives an item from the source.</summary>
		/// <returns>The received item.</returns>
		/// <param name="source">The source from which to receive.</param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> which may be used to cancel the receive operation.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out-or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">No item could be received from the source.</exception>
		/// <exception cref="T:System.TimeoutException">The specified timeout expired before an item was received from the source.</exception>
		/// <exception cref="T:System.OperationCanceledException">The operation was canceled before an item was received from the source.</exception>
		// Token: 0x06000044 RID: 68 RVA: 0x00002C24 File Offset: 0x00000E24
		public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout, CancellationToken cancellationToken)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (!Common.IsValidTimeout(timeout))
			{
				throw new ArgumentOutOfRangeException("timeout", SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
			}
			cancellationToken.ThrowIfCancellationRequested();
			IReceivableSourceBlock<TOutput> receivableSourceBlock = source as IReceivableSourceBlock<TOutput>;
			TOutput result;
			if (receivableSourceBlock != null && receivableSourceBlock.TryReceive(null, out result))
			{
				return result;
			}
			Task<TOutput> task = source.ReceiveCore(false, timeout, cancellationToken);
			TOutput result2;
			try
			{
				result2 = task.GetAwaiter().GetResult();
			}
			catch
			{
				if (task.IsCanceled)
				{
					cancellationToken.ThrowIfCancellationRequested();
				}
				throw;
			}
			return result2;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002CB8 File Offset: 0x00000EB8
		private static Task<TOutput> ReceiveCore<TOutput>(this ISourceBlock<TOutput> source, bool attemptTryReceive, TimeSpan timeout, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Common.CreateTaskFromCancellation<TOutput>(cancellationToken);
			}
			if (attemptTryReceive)
			{
				IReceivableSourceBlock<TOutput> receivableSourceBlock = source as IReceivableSourceBlock<TOutput>;
				if (receivableSourceBlock != null)
				{
					try
					{
						TOutput result;
						if (receivableSourceBlock.TryReceive(null, out result))
						{
							return Task.FromResult<TOutput>(result);
						}
					}
					catch (Exception exception)
					{
						return Common.CreateTaskFromException<TOutput>(exception);
					}
				}
			}
			int num = (int)timeout.TotalMilliseconds;
			if (num == 0)
			{
				return Common.CreateTaskFromException<TOutput>(DataflowBlock.ReceiveTarget<TOutput>.CreateExceptionForTimeout());
			}
			return DataflowBlock.ReceiveCoreByLinking<TOutput>(source, num, cancellationToken);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002D38 File Offset: 0x00000F38
		private static Task<TOutput> ReceiveCoreByLinking<TOutput>(ISourceBlock<TOutput> source, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			DataflowBlock.ReceiveTarget<TOutput> receiveTarget = new DataflowBlock.ReceiveTarget<TOutput>();
			try
			{
				if (cancellationToken.CanBeCanceled)
				{
					receiveTarget._externalCancellationToken = cancellationToken;
					receiveTarget._regFromExternalCancellationToken = cancellationToken.Register(DataflowBlock._cancelCts, receiveTarget._cts);
				}
				if (millisecondsTimeout > 0)
				{
					receiveTarget._timer = new Timer(DataflowBlock.ReceiveTarget<TOutput>.CachedLinkingTimerCallback, receiveTarget, millisecondsTimeout, -1);
				}
				if (receiveTarget._cts.Token.CanBeCanceled)
				{
					receiveTarget._cts.Token.Register(DataflowBlock.ReceiveTarget<TOutput>.CachedLinkingCancellationCallback, receiveTarget);
				}
				IDisposable disposable = source.LinkTo(receiveTarget, DataflowLinkOptions.UnlinkAfterOneAndPropagateCompletion);
				receiveTarget._unlink = disposable;
				if (Volatile.Read(ref receiveTarget._cleanupReserved))
				{
					IDisposable disposable2 = Interlocked.CompareExchange<IDisposable>(ref receiveTarget._unlink, null, disposable);
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
				}
			}
			catch (Exception receivedException)
			{
				receiveTarget._receivedException = receivedException;
				receiveTarget.TryCleanupAndComplete(DataflowBlock.ReceiveCoreByLinkingCleanupReason.SourceProtocolError);
			}
			return receiveTarget.Task;
		}

		/// <summary>Provides a <see cref="T:System.Threading.Tasks.Task`1" /> that asynchronously monitors the source for available output.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that informs of whether and when more output is available. If, when the task completes, its <see cref="P:System.Threading.Tasks.Task`1.Result" /> is true, more output is available in the source (though another consumer of the source may retrieve the data).  If it returns false, more output is not and will never be available, due to the source completing prior to output being available.</returns>
		/// <param name="source">The source to monitor.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		// Token: 0x06000047 RID: 71 RVA: 0x00002E20 File Offset: 0x00001020
		public static Task<bool> OutputAvailableAsync<TOutput>(this ISourceBlock<TOutput> source)
		{
			return source.OutputAvailableAsync(CancellationToken.None);
		}

		/// <summary>Provides a <see cref="T:System.Threading.Tasks.Task`1" /> that asynchronously monitors the source for available output.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that informs of whether and when more output is available. If, when the task completes, its <see cref="P:System.Threading.Tasks.Task`1.Result" /> is true, more output is available in the source (though another consumer of the source may retrieve the data). If it returns false, more output is not and will never be available, due to the source completing prior to output being available. If it returns false, more output is not and will never be available, due to the source completing prior to output being available.</returns>
		/// <param name="source">The source to monitor.</param>
		/// <param name="cancellationToken">The cancellation token with which to cancel the asynchronous operation.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		// Token: 0x06000048 RID: 72 RVA: 0x00002E30 File Offset: 0x00001030
		public static Task<bool> OutputAvailableAsync<TOutput>(this ISourceBlock<TOutput> source, CancellationToken cancellationToken)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Common.CreateTaskFromCancellation<bool>(cancellationToken);
			}
			DataflowBlock.OutputAvailableAsyncTarget<TOutput> outputAvailableAsyncTarget = new DataflowBlock.OutputAvailableAsyncTarget<TOutput>();
			Task<bool> result;
			try
			{
				outputAvailableAsyncTarget._unlinker = source.LinkTo(outputAvailableAsyncTarget, DataflowLinkOptions.UnlinkAfterOneAndPropagateCompletion);
				if (outputAvailableAsyncTarget.Task.IsCompleted)
				{
					result = outputAvailableAsyncTarget.Task;
				}
				else
				{
					if (cancellationToken.CanBeCanceled)
					{
						outputAvailableAsyncTarget._ctr = cancellationToken.Register(DataflowBlock.OutputAvailableAsyncTarget<TOutput>.s_cancelAndUnlink, outputAvailableAsyncTarget);
					}
					result = outputAvailableAsyncTarget.Task.ContinueWith<bool>(DataflowBlock.OutputAvailableAsyncTarget<TOutput>.s_handleCompletion, outputAvailableAsyncTarget, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None) | TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
				}
			}
			catch (Exception exception)
			{
				outputAvailableAsyncTarget.TrySetException(exception);
				outputAvailableAsyncTarget.AttemptThreadSafeUnlink();
				result = outputAvailableAsyncTarget.Task;
			}
			return result;
		}

		/// <summary>Encapsulates a target and a source into a single propagator.</summary>
		/// <returns>The encapsulated target and source.</returns>
		/// <param name="target">The target to encapsulate.</param>
		/// <param name="source">The source to encapsulate.</param>
		/// <typeparam name="TInput">Specifies the type of input expected by the target.</typeparam>
		/// <typeparam name="TOutput">Specifies the type of output produced by the source.</typeparam>
		// Token: 0x06000049 RID: 73 RVA: 0x00002EF8 File Offset: 0x000010F8
		public static IPropagatorBlock<TInput, TOutput> Encapsulate<TInput, TOutput>(ITargetBlock<TInput> target, ISourceBlock<TOutput> source)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new DataflowBlock.EncapsulatingPropagator<TInput, TOutput>(target, source);
		}

		/// <summary>Monitors two dataflow sources, invoking the provided handler for whichever source makes data available first.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous choice. If both sources are completed prior to the choice completing, the resulting task will be canceled. When one of the sources has data available and successfully propagates it to the choice, the resulting task will complete when the handler completes; if the handler throws an exception, the task will end in the <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state and will contain the unhandled exception. Otherwise, the task will end with its <see cref="P:System.Threading.Tasks.Task`1.Result" /> set to either 0 or 1 to represent the first or second source, respectively.This method will only consume an element from one of the two data sources, never both.</returns>
		/// <param name="source1">The first source.</param>
		/// <param name="action1">The handler to execute on data from the first source.</param>
		/// <param name="source2">The second source.</param>
		/// <param name="action2">The handler to execute on data from the second source.</param>
		/// <typeparam name="T1">Specifies type of data contained in the first source.</typeparam>
		/// <typeparam name="T2">Specifies type of data contained in the second source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source1" /> is null.-or-The <paramref name="action1" /> is null.-or-The <paramref name="source2" /> is null.-or-The <paramref name="action2" /> is null.</exception>
		// Token: 0x0600004A RID: 74 RVA: 0x00002F1D File Offset: 0x0000111D
		public static Task<int> Choose<T1, T2>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2)
		{
			return DataflowBlock.Choose<T1, T2>(source1, action1, source2, action2, DataflowBlockOptions.Default);
		}

		/// <summary>Monitors two dataflow sources, invoking the provided handler for whichever source makes data available first.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous choice. If both sources are completed prior to the choice completing, or if the <see cref="T:System.Threading.CancellationToken" /> provided as part of <paramref name="dataflowBlockOptions" /> is canceled prior to the choice completing, the resulting task will be canceled. When one of the sources has data available and successfully propagates it to the choice, the resulting task will complete when the handler completes; if the handler throws an exception, the task will end in the <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state and will contain the unhandled exception. Otherwise, the task will end with its <see cref="P:System.Threading.Tasks.Task`1.Result" /> set to either 0 or 1 to represent the first or second source, respectively.This method will only consume an element from one of the two data sources, never both. If cancellation is requested after an element has been received, the cancellation request will be ignored, and the relevant handler will be allowed to execute. </returns>
		/// <param name="source1">The first source.</param>
		/// <param name="action1">The handler to execute on data from the first source.</param>
		/// <param name="source2">The second source.</param>
		/// <param name="action2">The handler to execute on data from the second source.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this choice.</param>
		/// <typeparam name="T1">Specifies type of data contained in the first source.</typeparam>
		/// <typeparam name="T2">Specifies type of data contained in the second source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source1" /> is null.-or-The <paramref name="action1" /> is null.-or-The <paramref name="source2" /> is null.-or-The <paramref name="action2" /> is null.-or-The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x0600004B RID: 75 RVA: 0x00002F30 File Offset: 0x00001130
		public static Task<int> Choose<T1, T2>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, DataflowBlockOptions dataflowBlockOptions)
		{
			if (source1 == null)
			{
				throw new ArgumentNullException("source1");
			}
			if (action1 == null)
			{
				throw new ArgumentNullException("action1");
			}
			if (source2 == null)
			{
				throw new ArgumentNullException("source2");
			}
			if (action2 == null)
			{
				throw new ArgumentNullException("action2");
			}
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			return DataflowBlock.ChooseCore<T1, T2, VoidResult>(source1, action1, source2, action2, null, null, dataflowBlockOptions);
		}

		/// <summary>Monitors three dataflow sources, invoking the provided handler for whichever source makes data available first.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous choice. If all sources are completed prior to the choice completing, the resulting task will be canceled. When one of the sources has data available and successfully propagates it to the choice, the resulting task will complete when the handler completes; if the handler throws an exception, the task will end in the <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state and will contain the unhandled exception. Otherwise, the task will end with its <see cref="P:System.Threading.Tasks.Task`1.Result" /> set to the 0-based index of the source.This method will only consume an element from one of the data sources, never more than one.</returns>
		/// <param name="source1">The first source.</param>
		/// <param name="action1">The handler to execute on data from the first source.</param>
		/// <param name="source2">The second source.</param>
		/// <param name="action2">The handler to execute on data from the second source.</param>
		/// <param name="source3">The third source.</param>
		/// <param name="action3">The handler to execute on data from the third source.</param>
		/// <typeparam name="T1">Specifies type of data contained in the first source.</typeparam>
		/// <typeparam name="T2">Specifies type of data contained in the second source.</typeparam>
		/// <typeparam name="T3">Specifies type of data contained in the third source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source1" /> is null.-or-The <paramref name="action1" /> is null.-or-The <paramref name="source2" /> is null.-or-The <paramref name="action2" /> is null.-or-The <paramref name="source3" /> is null.-or-The <paramref name="action3" /> is null.</exception>
		// Token: 0x0600004C RID: 76 RVA: 0x00002F91 File Offset: 0x00001191
		public static Task<int> Choose<T1, T2, T3>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, ISourceBlock<T3> source3, Action<T3> action3)
		{
			return DataflowBlock.Choose<T1, T2, T3>(source1, action1, source2, action2, source3, action3, DataflowBlockOptions.Default);
		}

		/// <summary>Monitors three dataflow sources, invoking the provided handler for whichever source makes data available first.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous choice. If all sources are completed prior to the choice completing, or if the <see cref="T:System.Threading.CancellationToken" /> provided as part of <paramref name="dataflowBlockOptions" /> is canceled prior to the choice completing, the resulting task will be canceled. When one of the sources has data available and successfully propagates it to the choice, the resulting task will complete when the handler completes; if the handler throws an exception, the task will end in the <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state and will contain the unhandled exception. Otherwise, the task will end with its <see cref="P:System.Threading.Tasks.Task`1.Result" /> set to the 0-based index of the source.This method will only consume an element from one of the data sources, never more than one. If cancellation is requested after an element has been received, the cancellation request will be ignored, and the relevant handler will be allowed to execute. </returns>
		/// <param name="source1">The first source.</param>
		/// <param name="action1">The handler to execute on data from the first source.</param>
		/// <param name="source2">The second source.</param>
		/// <param name="action2">The handler to execute on data from the second source.</param>
		/// <param name="source3">The third source.</param>
		/// <param name="action3">The handler to execute on data from the third source.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this choice.</param>
		/// <typeparam name="T1">Specifies type of data contained in the first source.</typeparam>
		/// <typeparam name="T2">Specifies type of data contained in the second source.</typeparam>
		/// <typeparam name="T3">Specifies type of data contained in the third source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source1" /> is null.-or-The <paramref name="action1" /> is null.-or-The <paramref name="source2" /> is null.-or-The <paramref name="action2" /> is null.-or-The <paramref name="source3" /> is null.-or-The <paramref name="action3" /> is null.-or-The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x0600004D RID: 77 RVA: 0x00002FA8 File Offset: 0x000011A8
		public static Task<int> Choose<T1, T2, T3>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, ISourceBlock<T3> source3, Action<T3> action3, DataflowBlockOptions dataflowBlockOptions)
		{
			if (source1 == null)
			{
				throw new ArgumentNullException("source1");
			}
			if (action1 == null)
			{
				throw new ArgumentNullException("action1");
			}
			if (source2 == null)
			{
				throw new ArgumentNullException("source2");
			}
			if (action2 == null)
			{
				throw new ArgumentNullException("action2");
			}
			if (source3 == null)
			{
				throw new ArgumentNullException("source3");
			}
			if (action3 == null)
			{
				throw new ArgumentNullException("action3");
			}
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			return DataflowBlock.ChooseCore<T1, T2, T3>(source1, action1, source2, action2, source3, action3, dataflowBlockOptions);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000302C File Offset: 0x0000122C
		private static Task<int> ChooseCore<T1, T2, T3>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, ISourceBlock<T3> source3, Action<T3> action3, DataflowBlockOptions dataflowBlockOptions)
		{
			bool flag = source3 != null;
			if (dataflowBlockOptions.CancellationToken.IsCancellationRequested)
			{
				return Common.CreateTaskFromCancellation<int>(dataflowBlockOptions.CancellationToken);
			}
			try
			{
				TaskScheduler taskScheduler = dataflowBlockOptions.TaskScheduler;
				Task<int> result;
				if (DataflowBlock.TryChooseFromSource<T1>(source1, action1, 0, taskScheduler, out result) || DataflowBlock.TryChooseFromSource<T2>(source2, action2, 1, taskScheduler, out result) || (flag && DataflowBlock.TryChooseFromSource<T3>(source3, action3, 2, taskScheduler, out result)))
				{
					return result;
				}
			}
			catch (Exception exception)
			{
				return Common.CreateTaskFromException<int>(exception);
			}
			return DataflowBlock.ChooseCoreByLinking<T1, T2, T3>(source1, action1, source2, action2, source3, action3, dataflowBlockOptions);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000030C8 File Offset: 0x000012C8
		private static bool TryChooseFromSource<T>(ISourceBlock<T> source, Action<T> action, int branchId, TaskScheduler scheduler, out Task<int> task)
		{
			IReceivableSourceBlock<T> receivableSourceBlock = source as IReceivableSourceBlock<T>;
			T item;
			if (receivableSourceBlock == null || !receivableSourceBlock.TryReceive(out item))
			{
				task = null;
				return false;
			}
			task = Task.Factory.StartNew<int>(DataflowBlock.ChooseTarget<T>.s_processBranchFunction, Tuple.Create<Action<T>, T, int>(action, item, branchId), CancellationToken.None, Common.GetCreationOptionsForTask(false), scheduler);
			return true;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003118 File Offset: 0x00001318
		private static Task<int> ChooseCoreByLinking<T1, T2, T3>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, ISourceBlock<T3> source3, Action<T3> action3, DataflowBlockOptions dataflowBlockOptions)
		{
			bool flag = source3 != null;
			StrongBox<Task> boxedCompleted = new StrongBox<Task>();
			CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(dataflowBlockOptions.CancellationToken, CancellationToken.None);
			TaskScheduler taskScheduler = dataflowBlockOptions.TaskScheduler;
			Task<int>[] array = new Task<int>[flag ? 3 : 2];
			array[0] = DataflowBlock.CreateChooseBranch<T1>(boxedCompleted, cts, taskScheduler, 0, source1, action1);
			array[1] = DataflowBlock.CreateChooseBranch<T2>(boxedCompleted, cts, taskScheduler, 1, source2, action2);
			if (flag)
			{
				array[2] = DataflowBlock.CreateChooseBranch<T3>(boxedCompleted, cts, taskScheduler, 2, source3, action3);
			}
			TaskCompletionSource<int> result = new TaskCompletionSource<int>();
			Task.Factory.ContinueWhenAll<int>(array, delegate(Task<int>[] tasks)
			{
				List<Exception> list = null;
				int num = -1;
				int result;
				foreach (Task<int> task in tasks)
				{
					TaskStatus status = task.Status;
					if (status != TaskStatus.RanToCompletion)
					{
						if (status == TaskStatus.Faulted)
						{
							Common.AddException(ref list, task.Exception, true);
						}
					}
					else
					{
						result = task.Result;
						if (result >= 0)
						{
							num = result;
						}
					}
				}
				if (list != null)
				{
					result.TrySetException(list);
				}
				else if (num >= 0)
				{
					result.TrySetResult(num);
				}
				else
				{
					result.TrySetCanceled();
				}
				cts.Dispose();
			}, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None), TaskScheduler.Default);
			return result.Task;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000031E4 File Offset: 0x000013E4
		private static Task<int> CreateChooseBranch<T>(StrongBox<Task> boxedCompleted, CancellationTokenSource cts, TaskScheduler scheduler, int branchId, ISourceBlock<T> source, Action<T> action)
		{
			if (cts.IsCancellationRequested)
			{
				return Common.CreateTaskFromCancellation<int>(cts.Token);
			}
			DataflowBlock.ChooseTarget<T> chooseTarget = new DataflowBlock.ChooseTarget<T>(boxedCompleted, cts.Token);
			IDisposable unlink;
			try
			{
				unlink = source.LinkTo(chooseTarget, DataflowLinkOptions.UnlinkAfterOneAndPropagateCompletion);
			}
			catch (Exception exception)
			{
				cts.Cancel();
				return Common.CreateTaskFromException<int>(exception);
			}
			return chooseTarget.Task.ContinueWith<int>(delegate(Task<T> completed)
			{
				int result;
				try
				{
					if (completed.Status == TaskStatus.RanToCompletion)
					{
						cts.Cancel();
						action(completed.Result);
						result = branchId;
					}
					else
					{
						result = -1;
					}
				}
				finally
				{
					unlink.Dispose();
				}
				return result;
			}, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None), scheduler);
		}

		/// <summary>Creates a new <see cref="T:System.IObservable`1" /> abstraction over the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" />.</summary>
		/// <returns>An <see cref="T:System.IObservable`1" /> that enables observers to be subscribed to the source.</returns>
		/// <param name="source">The source to wrap.</param>
		/// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
		// Token: 0x06000052 RID: 82 RVA: 0x0000329C File Offset: 0x0000149C
		public static IObservable<TOutput> AsObservable<TOutput>(this ISourceBlock<TOutput> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return DataflowBlock.SourceObservable<TOutput>.From(source);
		}

		/// <summary>Creates a new <see cref="T:System.IObserver`1" /> abstraction over the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>An observer that wraps the target block.</returns>
		/// <param name="target">The target to wrap.</param>
		/// <typeparam name="TInput">Specifies the type of input accepted by the target block.</typeparam>
		// Token: 0x06000053 RID: 83 RVA: 0x000032B2 File Offset: 0x000014B2
		public static IObserver<TInput> AsObserver<TInput>(this ITargetBlock<TInput> target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return new DataflowBlock.TargetObserver<TInput>(target);
		}

		/// <summary>Gets a target block that synchronously accepts all messages offered to it and drops them.</summary>
		/// <returns>A <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> that accepts and subsequently drops all offered messages.</returns>
		/// <typeparam name="TInput">The type of the messages this block can accept.</typeparam>
		// Token: 0x06000054 RID: 84 RVA: 0x000032C8 File Offset: 0x000014C8
		public static ITargetBlock<TInput> NullTarget<TInput>()
		{
			return new DataflowBlock.NullTargetBlock<TInput>();
		}

		// Token: 0x04000014 RID: 20
		private static readonly Action<object> _cancelCts = delegate(object state)
		{
			((CancellationTokenSource)state).Cancel();
		};

		// Token: 0x04000015 RID: 21
		private static readonly ExecutionDataflowBlockOptions _nonGreedyExecutionOptions = new ExecutionDataflowBlockOptions
		{
			BoundedCapacity = 1
		};

		// Token: 0x0200000F RID: 15
		[DebuggerTypeProxy(typeof(DataflowBlock.FilteredLinkPropagator<>.DebugView))]
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class FilteredLinkPropagator<T> : IPropagatorBlock<T, T>, ITargetBlock<T>, IDataflowBlock, ISourceBlock<T>, IDebuggerDisplay
		{
			// Token: 0x06000056 RID: 86 RVA: 0x000032F7 File Offset: 0x000014F7
			internal FilteredLinkPropagator(ISourceBlock<T> source, ITargetBlock<T> target, Predicate<T> predicate)
			{
				this._source = source;
				this._target = target;
				this._userProvidedPredicate = predicate;
			}

			// Token: 0x06000057 RID: 87 RVA: 0x00003314 File Offset: 0x00001514
			private bool RunPredicate(T item)
			{
				return this._userProvidedPredicate(item);
			}

			// Token: 0x06000058 RID: 88 RVA: 0x00003324 File Offset: 0x00001524
			DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (source == null)
				{
					throw new ArgumentNullException("source");
				}
				bool flag = this.RunPredicate(messageValue);
				if (flag)
				{
					return this._target.OfferMessage(messageHeader, messageValue, this, consumeToAccept);
				}
				return DataflowMessageStatus.Declined;
			}

			// Token: 0x06000059 RID: 89 RVA: 0x00003375 File Offset: 0x00001575
			T ISourceBlock<!0>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
			{
				return this._source.ConsumeMessage(messageHeader, this, out messageConsumed);
			}

			// Token: 0x0600005A RID: 90 RVA: 0x00003385 File Offset: 0x00001585
			bool ISourceBlock<!0>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
			{
				return this._source.ReserveMessage(messageHeader, this);
			}

			// Token: 0x0600005B RID: 91 RVA: 0x00003394 File Offset: 0x00001594
			void ISourceBlock<!0>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
			{
				this._source.ReleaseReservation(messageHeader, this);
			}

			// Token: 0x17000018 RID: 24
			// (get) Token: 0x0600005C RID: 92 RVA: 0x000033A3 File Offset: 0x000015A3
			Task IDataflowBlock.Completion
			{
				get
				{
					return this._source.Completion;
				}
			}

			// Token: 0x0600005D RID: 93 RVA: 0x000033B0 File Offset: 0x000015B0
			void IDataflowBlock.Complete()
			{
				this._target.Complete();
			}

			// Token: 0x0600005E RID: 94 RVA: 0x000033BD File Offset: 0x000015BD
			void IDataflowBlock.Fault(Exception exception)
			{
				this._target.Fault(exception);
			}

			// Token: 0x0600005F RID: 95 RVA: 0x000033CB File Offset: 0x000015CB
			IDisposable ISourceBlock<!0>.LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
			{
				throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
			}

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x06000060 RID: 96 RVA: 0x000033D8 File Offset: 0x000015D8
			private object DebuggerDisplayContent
			{
				get
				{
					IDebuggerDisplay debuggerDisplay = this._source as IDebuggerDisplay;
					IDebuggerDisplay debuggerDisplay2 = this._target as IDebuggerDisplay;
					return string.Format("{0} Source=\"{1}\", Target=\"{2}\"", Common.GetNameForDebugger(this, null), (debuggerDisplay != null) ? debuggerDisplay.Content : this._source, (debuggerDisplay2 != null) ? debuggerDisplay2.Content : this._target);
				}
			}

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x06000061 RID: 97 RVA: 0x00003430 File Offset: 0x00001630
			object IDebuggerDisplay.Content
			{
				get
				{
					return this.DebuggerDisplayContent;
				}
			}

			// Token: 0x04000016 RID: 22
			private readonly ISourceBlock<T> _source;

			// Token: 0x04000017 RID: 23
			private readonly ITargetBlock<T> _target;

			// Token: 0x04000018 RID: 24
			private readonly Predicate<T> _userProvidedPredicate;

			// Token: 0x02000010 RID: 16
			private sealed class DebugView
			{
				// Token: 0x06000062 RID: 98 RVA: 0x00003438 File Offset: 0x00001638
				public DebugView(DataflowBlock.FilteredLinkPropagator<T> filter)
				{
					this._filter = filter;
				}

				// Token: 0x1700001B RID: 27
				// (get) Token: 0x06000063 RID: 99 RVA: 0x00003447 File Offset: 0x00001647
				public ITargetBlock<T> LinkedTarget
				{
					get
					{
						return this._filter._target;
					}
				}

				// Token: 0x04000019 RID: 25
				private readonly DataflowBlock.FilteredLinkPropagator<T> _filter;
			}
		}

		// Token: 0x02000011 RID: 17
		[DebuggerTypeProxy(typeof(DataflowBlock.SendAsyncSource<>.DebugView))]
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class SendAsyncSource<TOutput> : TaskCompletionSource<bool>, ISourceBlock<TOutput>, IDataflowBlock, IDebuggerDisplay
		{
			// Token: 0x06000064 RID: 100 RVA: 0x00003454 File Offset: 0x00001654
			internal SendAsyncSource(ITargetBlock<TOutput> target, TOutput messageValue, CancellationToken cancellationToken)
			{
				this._target = target;
				this._messageValue = messageValue;
				if (cancellationToken.CanBeCanceled)
				{
					this._cancellationToken = cancellationToken;
					this._cancellationState = 1;
					try
					{
						this._cancellationRegistration = cancellationToken.Register(DataflowBlock.SendAsyncSource<TOutput>._cancellationCallback, new WeakReference<DataflowBlock.SendAsyncSource<TOutput>>(this));
					}
					catch
					{
						GC.SuppressFinalize(this);
						throw;
					}
				}
			}

			// Token: 0x06000065 RID: 101 RVA: 0x000034C0 File Offset: 0x000016C0
			~SendAsyncSource()
			{
				if (!Environment.HasShutdownStarted)
				{
					this.CompleteAsDeclined(true);
				}
			}

			// Token: 0x06000066 RID: 102 RVA: 0x000034F4 File Offset: 0x000016F4
			private void CompleteAsAccepted(bool runAsync)
			{
				this.RunCompletionAction(delegate(object state)
				{
					try
					{
						((DataflowBlock.SendAsyncSource<TOutput>)state).TrySetResult(true);
					}
					catch (ObjectDisposedException)
					{
					}
				}, this, runAsync);
			}

			// Token: 0x06000067 RID: 103 RVA: 0x0000351D File Offset: 0x0000171D
			private void CompleteAsDeclined(bool runAsync)
			{
				this.RunCompletionAction(delegate(object state)
				{
					try
					{
						((DataflowBlock.SendAsyncSource<TOutput>)state).TrySetResult(false);
					}
					catch (ObjectDisposedException)
					{
					}
				}, this, runAsync);
			}

			// Token: 0x06000068 RID: 104 RVA: 0x00003546 File Offset: 0x00001746
			private void CompleteAsFaulted(Exception exception, bool runAsync)
			{
				this.RunCompletionAction(delegate(object state)
				{
					Tuple<DataflowBlock.SendAsyncSource<TOutput>, Exception> tuple = (Tuple<DataflowBlock.SendAsyncSource<TOutput>, Exception>)state;
					try
					{
						tuple.Item1.TrySetException(tuple.Item2);
					}
					catch (ObjectDisposedException)
					{
					}
				}, Tuple.Create<DataflowBlock.SendAsyncSource<TOutput>, Exception>(this, exception), runAsync);
			}

			// Token: 0x06000069 RID: 105 RVA: 0x00003575 File Offset: 0x00001775
			private void CompleteAsCanceled(bool runAsync)
			{
				this.RunCompletionAction(delegate(object state)
				{
					try
					{
						((DataflowBlock.SendAsyncSource<TOutput>)state).TrySetCanceled();
					}
					catch (ObjectDisposedException)
					{
					}
				}, this, runAsync);
			}

			// Token: 0x0600006A RID: 106 RVA: 0x000035A0 File Offset: 0x000017A0
			private void RunCompletionAction(Action<object> completionAction, object completionActionState, bool runAsync)
			{
				GC.SuppressFinalize(this);
				if (this._cancellationState != 0)
				{
					this._cancellationRegistration.Dispose();
				}
				if (runAsync)
				{
					System.Threading.Tasks.Task.Factory.StartNew(completionAction, completionActionState, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
					return;
				}
				completionAction(completionActionState);
			}

			// Token: 0x0600006B RID: 107 RVA: 0x000035EE File Offset: 0x000017EE
			private void OfferToTargetAsync()
			{
				System.Threading.Tasks.Task.Factory.StartNew(delegate(object state)
				{
					((DataflowBlock.SendAsyncSource<TOutput>)state).OfferToTarget();
				}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
			}

			// Token: 0x0600006C RID: 108 RVA: 0x0000362C File Offset: 0x0000182C
			private static void CancellationHandler(object state)
			{
				DataflowBlock.SendAsyncSource<TOutput> sendAsyncSource = Common.UnwrapWeakReference<DataflowBlock.SendAsyncSource<TOutput>>(state);
				if (sendAsyncSource != null && sendAsyncSource._cancellationState == 1 && Interlocked.CompareExchange(ref sendAsyncSource._cancellationState, 3, 1) == 1)
				{
					sendAsyncSource.CompleteAsCanceled(true);
				}
			}

			// Token: 0x0600006D RID: 109 RVA: 0x00003664 File Offset: 0x00001864
			internal void OfferToTarget()
			{
				try
				{
					bool flag = this._cancellationState != 0;
					switch (this._target.OfferMessage(Common.SingleMessageHeader, this._messageValue, this, flag))
					{
					case DataflowMessageStatus.Accepted:
						if (!flag)
						{
							this.CompleteAsAccepted(false);
						}
						break;
					case DataflowMessageStatus.Declined:
					case DataflowMessageStatus.DecliningPermanently:
						this.CompleteAsDeclined(false);
						break;
					}
				}
				catch (Exception ex)
				{
					Common.StoreDataflowMessageValueIntoExceptionData<TOutput>(ex, this._messageValue, false);
					this.CompleteAsFaulted(ex, false);
				}
			}

			// Token: 0x0600006E RID: 110 RVA: 0x000036F0 File Offset: 0x000018F0
			TOutput ISourceBlock<!0>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (target == null)
				{
					throw new ArgumentNullException("target");
				}
				if (base.Task.IsCompleted)
				{
					messageConsumed = false;
					return default(TOutput);
				}
				bool flag = messageHeader.Id == 1L;
				if (flag)
				{
					int cancellationState = this._cancellationState;
					if (cancellationState == 0 || (cancellationState != 3 && Interlocked.CompareExchange(ref this._cancellationState, 3, cancellationState) == cancellationState))
					{
						this.CompleteAsAccepted(true);
						messageConsumed = true;
						return this._messageValue;
					}
				}
				messageConsumed = false;
				return default(TOutput);
			}

			// Token: 0x0600006F RID: 111 RVA: 0x00003788 File Offset: 0x00001988
			bool ISourceBlock<!0>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (target == null)
				{
					throw new ArgumentNullException("target");
				}
				if (base.Task.IsCompleted)
				{
					return false;
				}
				bool flag = messageHeader.Id == 1L;
				return flag && (this._cancellationState == 0 || Interlocked.CompareExchange(ref this._cancellationState, 2, 1) == 1);
			}

			// Token: 0x06000070 RID: 112 RVA: 0x000037F8 File Offset: 0x000019F8
			void ISourceBlock<!0>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (target == null)
				{
					throw new ArgumentNullException("target");
				}
				if (messageHeader.Id != 1L)
				{
					throw new InvalidOperationException(SR.InvalidOperation_MessageNotReservedByTarget);
				}
				if (base.Task.IsCompleted)
				{
					return;
				}
				if (this._cancellationState != 0)
				{
					if (Interlocked.CompareExchange(ref this._cancellationState, 1, 2) != 2)
					{
						throw new InvalidOperationException(SR.InvalidOperation_MessageNotReservedByTarget);
					}
					if (this._cancellationToken.IsCancellationRequested)
					{
						DataflowBlock.SendAsyncSource<TOutput>.CancellationHandler(new WeakReference<DataflowBlock.SendAsyncSource<TOutput>>(this));
					}
				}
				this.OfferToTargetAsync();
			}

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x06000071 RID: 113 RVA: 0x00003891 File Offset: 0x00001A91
			Task IDataflowBlock.Completion
			{
				get
				{
					return base.Task;
				}
			}

			// Token: 0x06000072 RID: 114 RVA: 0x000033CB File Offset: 0x000015CB
			IDisposable ISourceBlock<!0>.LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions)
			{
				throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
			}

			// Token: 0x06000073 RID: 115 RVA: 0x000033CB File Offset: 0x000015CB
			void IDataflowBlock.Complete()
			{
				throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
			}

			// Token: 0x06000074 RID: 116 RVA: 0x000033CB File Offset: 0x000015CB
			void IDataflowBlock.Fault(Exception exception)
			{
				throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
			}

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x06000075 RID: 117 RVA: 0x0000389C File Offset: 0x00001A9C
			private object DebuggerDisplayContent
			{
				get
				{
					IDebuggerDisplay debuggerDisplay = this._target as IDebuggerDisplay;
					return string.Format("{0} Message={1}, Target=\"{2}\"", Common.GetNameForDebugger(this, null), this._messageValue, (debuggerDisplay != null) ? debuggerDisplay.Content : this._target);
				}
			}

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x06000076 RID: 118 RVA: 0x000038E2 File Offset: 0x00001AE2
			object IDebuggerDisplay.Content
			{
				get
				{
					return this.DebuggerDisplayContent;
				}
			}

			// Token: 0x0400001A RID: 26
			private readonly ITargetBlock<TOutput> _target;

			// Token: 0x0400001B RID: 27
			private readonly TOutput _messageValue;

			// Token: 0x0400001C RID: 28
			private CancellationToken _cancellationToken;

			// Token: 0x0400001D RID: 29
			private CancellationTokenRegistration _cancellationRegistration;

			// Token: 0x0400001E RID: 30
			private int _cancellationState;

			// Token: 0x0400001F RID: 31
			private static readonly Action<object> _cancellationCallback = new Action<object>(DataflowBlock.SendAsyncSource<TOutput>.CancellationHandler);

			// Token: 0x02000012 RID: 18
			private sealed class DebugView
			{
				// Token: 0x06000078 RID: 120 RVA: 0x000038FD File Offset: 0x00001AFD
				public DebugView(DataflowBlock.SendAsyncSource<TOutput> source)
				{
					this._source = source;
				}

				// Token: 0x1700001F RID: 31
				// (get) Token: 0x06000079 RID: 121 RVA: 0x0000390C File Offset: 0x00001B0C
				public ITargetBlock<TOutput> Target
				{
					get
					{
						return this._source._target;
					}
				}

				// Token: 0x17000020 RID: 32
				// (get) Token: 0x0600007A RID: 122 RVA: 0x00003919 File Offset: 0x00001B19
				public TOutput Message
				{
					get
					{
						return this._source._messageValue;
					}
				}

				// Token: 0x17000021 RID: 33
				// (get) Token: 0x0600007B RID: 123 RVA: 0x00003926 File Offset: 0x00001B26
				public Task<bool> Completion
				{
					get
					{
						return this._source.Task;
					}
				}

				// Token: 0x04000020 RID: 32
				private readonly DataflowBlock.SendAsyncSource<TOutput> _source;
			}
		}

		// Token: 0x02000014 RID: 20
		private enum ReceiveCoreByLinkingCleanupReason
		{
			// Token: 0x04000028 RID: 40
			Success,
			// Token: 0x04000029 RID: 41
			Timer,
			// Token: 0x0400002A RID: 42
			Cancellation,
			// Token: 0x0400002B RID: 43
			SourceCompletion,
			// Token: 0x0400002C RID: 44
			SourceProtocolError,
			// Token: 0x0400002D RID: 45
			ErrorDuringCleanup
		}

		// Token: 0x02000015 RID: 21
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class ReceiveTarget<T> : TaskCompletionSource<T>, ITargetBlock<!0>, IDataflowBlock, IDebuggerDisplay
		{
			// Token: 0x17000022 RID: 34
			// (get) Token: 0x06000083 RID: 131 RVA: 0x00003A21 File Offset: 0x00001C21
			internal object IncomingLock
			{
				get
				{
					return this._cts;
				}
			}

			// Token: 0x06000084 RID: 132 RVA: 0x00003A29 File Offset: 0x00001C29
			internal ReceiveTarget()
			{
			}

			// Token: 0x06000085 RID: 133 RVA: 0x00003A3C File Offset: 0x00001C3C
			DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (source == null && consumeToAccept)
				{
					throw new ArgumentException(SR.Argument_CantConsumeFromANullSource, "consumeToAccept");
				}
				DataflowMessageStatus dataflowMessageStatus = DataflowMessageStatus.NotAvailable;
				if (Volatile.Read(ref this._cleanupReserved))
				{
					return DataflowMessageStatus.DecliningPermanently;
				}
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					if (this._cleanupReserved)
					{
						return DataflowMessageStatus.DecliningPermanently;
					}
					try
					{
						bool flag2 = true;
						T receivedValue = consumeToAccept ? source.ConsumeMessage(messageHeader, this, out flag2) : messageValue;
						if (flag2)
						{
							dataflowMessageStatus = DataflowMessageStatus.Accepted;
							this._receivedValue = receivedValue;
							this._cleanupReserved = true;
						}
					}
					catch (Exception ex)
					{
						dataflowMessageStatus = DataflowMessageStatus.DecliningPermanently;
						Common.StoreDataflowMessageValueIntoExceptionData<T>(ex, messageValue, false);
						this._receivedException = ex;
						this._cleanupReserved = true;
					}
				}
				if (dataflowMessageStatus == DataflowMessageStatus.Accepted)
				{
					this.CleanupAndComplete(DataflowBlock.ReceiveCoreByLinkingCleanupReason.Success);
				}
				else if (dataflowMessageStatus == DataflowMessageStatus.DecliningPermanently)
				{
					this.CleanupAndComplete(DataflowBlock.ReceiveCoreByLinkingCleanupReason.SourceProtocolError);
				}
				return dataflowMessageStatus;
			}

			// Token: 0x06000086 RID: 134 RVA: 0x00003B38 File Offset: 0x00001D38
			internal bool TryCleanupAndComplete(DataflowBlock.ReceiveCoreByLinkingCleanupReason reason)
			{
				if (Volatile.Read(ref this._cleanupReserved))
				{
					return false;
				}
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					if (this._cleanupReserved)
					{
						return false;
					}
					this._cleanupReserved = true;
				}
				this.CleanupAndComplete(reason);
				return true;
			}

			// Token: 0x06000087 RID: 135 RVA: 0x00003BA0 File Offset: 0x00001DA0
			private void CleanupAndComplete(DataflowBlock.ReceiveCoreByLinkingCleanupReason reason)
			{
				IDisposable unlink = this._unlink;
				if (reason != DataflowBlock.ReceiveCoreByLinkingCleanupReason.SourceCompletion && unlink != null)
				{
					IDisposable disposable = Interlocked.CompareExchange<IDisposable>(ref this._unlink, null, unlink);
					if (disposable != null)
					{
						try
						{
							disposable.Dispose();
						}
						catch (Exception receivedException)
						{
							this._receivedException = receivedException;
							reason = DataflowBlock.ReceiveCoreByLinkingCleanupReason.SourceProtocolError;
						}
					}
				}
				if (this._timer != null)
				{
					this._timer.Dispose();
				}
				if (reason != DataflowBlock.ReceiveCoreByLinkingCleanupReason.Cancellation)
				{
					if (reason == DataflowBlock.ReceiveCoreByLinkingCleanupReason.SourceCompletion && (this._externalCancellationToken.IsCancellationRequested || this._cts.IsCancellationRequested))
					{
						reason = DataflowBlock.ReceiveCoreByLinkingCleanupReason.Cancellation;
					}
					this._cts.Cancel();
				}
				this._regFromExternalCancellationToken.Dispose();
				switch (reason)
				{
				case DataflowBlock.ReceiveCoreByLinkingCleanupReason.Success:
					System.Threading.Tasks.Task.Factory.StartNew(delegate(object state)
					{
						DataflowBlock.ReceiveTarget<T> receiveTarget = (DataflowBlock.ReceiveTarget<T>)state;
						try
						{
							receiveTarget.TrySetResult(receiveTarget._receivedValue);
						}
						catch (ObjectDisposedException)
						{
						}
					}, this, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
					return;
				case DataflowBlock.ReceiveCoreByLinkingCleanupReason.Timer:
					if (this._receivedException == null)
					{
						this._receivedException = DataflowBlock.ReceiveTarget<T>.CreateExceptionForTimeout();
						goto IL_138;
					}
					goto IL_138;
				case DataflowBlock.ReceiveCoreByLinkingCleanupReason.SourceCompletion:
					if (this._receivedException == null)
					{
						this._receivedException = DataflowBlock.ReceiveTarget<T>.CreateExceptionForSourceCompletion();
						goto IL_138;
					}
					goto IL_138;
				case DataflowBlock.ReceiveCoreByLinkingCleanupReason.SourceProtocolError:
				case DataflowBlock.ReceiveCoreByLinkingCleanupReason.ErrorDuringCleanup:
					goto IL_138;
				}
				System.Threading.Tasks.Task.Factory.StartNew(delegate(object state)
				{
					DataflowBlock.ReceiveTarget<T> receiveTarget = (DataflowBlock.ReceiveTarget<T>)state;
					try
					{
						receiveTarget.TrySetCanceled();
					}
					catch (ObjectDisposedException)
					{
					}
				}, this, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
				return;
				IL_138:
				System.Threading.Tasks.Task.Factory.StartNew(delegate(object state)
				{
					DataflowBlock.ReceiveTarget<T> receiveTarget = (DataflowBlock.ReceiveTarget<T>)state;
					try
					{
						receiveTarget.TrySetException(receiveTarget._receivedException ?? new InvalidOperationException(SR.InvalidOperation_ErrorDuringCleanup));
					}
					catch (ObjectDisposedException)
					{
					}
				}, this, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
			}

			// Token: 0x06000088 RID: 136 RVA: 0x00003D2C File Offset: 0x00001F2C
			internal static Exception CreateExceptionForSourceCompletion()
			{
				return Common.InitializeStackTrace(new InvalidOperationException(SR.InvalidOperation_DataNotAvailableForReceive));
			}

			// Token: 0x06000089 RID: 137 RVA: 0x00003D3D File Offset: 0x00001F3D
			internal static Exception CreateExceptionForTimeout()
			{
				return Common.InitializeStackTrace(new TimeoutException());
			}

			// Token: 0x0600008A RID: 138 RVA: 0x00003D49 File Offset: 0x00001F49
			void IDataflowBlock.Complete()
			{
				this.TryCleanupAndComplete(DataflowBlock.ReceiveCoreByLinkingCleanupReason.SourceCompletion);
			}

			// Token: 0x0600008B RID: 139 RVA: 0x00003D53 File Offset: 0x00001F53
			void IDataflowBlock.Fault(Exception exception)
			{
				((IDataflowBlock)this).Complete();
			}

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x0600008C RID: 140 RVA: 0x000033CB File Offset: 0x000015CB
			Task IDataflowBlock.Completion
			{
				get
				{
					throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
				}
			}

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x0600008D RID: 141 RVA: 0x00003D5B File Offset: 0x00001F5B
			private object DebuggerDisplayContent
			{
				get
				{
					return string.Format("{0} IsCompleted={1}", Common.GetNameForDebugger(this, null), base.Task.IsCompleted);
				}
			}

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x0600008E RID: 142 RVA: 0x00003D7E File Offset: 0x00001F7E
			object IDebuggerDisplay.Content
			{
				get
				{
					return this.DebuggerDisplayContent;
				}
			}

			// Token: 0x0400002E RID: 46
			internal static readonly TimerCallback CachedLinkingTimerCallback = delegate(object state)
			{
				DataflowBlock.ReceiveTarget<T> receiveTarget = (DataflowBlock.ReceiveTarget<T>)state;
				receiveTarget.TryCleanupAndComplete(DataflowBlock.ReceiveCoreByLinkingCleanupReason.Timer);
			};

			// Token: 0x0400002F RID: 47
			internal static readonly Action<object> CachedLinkingCancellationCallback = delegate(object state)
			{
				DataflowBlock.ReceiveTarget<T> receiveTarget = (DataflowBlock.ReceiveTarget<T>)state;
				receiveTarget.TryCleanupAndComplete(DataflowBlock.ReceiveCoreByLinkingCleanupReason.Cancellation);
			};

			// Token: 0x04000030 RID: 48
			private T _receivedValue;

			// Token: 0x04000031 RID: 49
			internal readonly CancellationTokenSource _cts = new CancellationTokenSource();

			// Token: 0x04000032 RID: 50
			internal bool _cleanupReserved;

			// Token: 0x04000033 RID: 51
			internal CancellationToken _externalCancellationToken;

			// Token: 0x04000034 RID: 52
			internal CancellationTokenRegistration _regFromExternalCancellationToken;

			// Token: 0x04000035 RID: 53
			internal Timer _timer;

			// Token: 0x04000036 RID: 54
			internal IDisposable _unlink;

			// Token: 0x04000037 RID: 55
			internal Exception _receivedException;
		}

		// Token: 0x02000017 RID: 23
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class OutputAvailableAsyncTarget<T> : TaskCompletionSource<bool>, ITargetBlock<!0>, IDataflowBlock, IDebuggerDisplay
		{
			// Token: 0x06000097 RID: 151 RVA: 0x00003EA4 File Offset: 0x000020A4
			private static void CancelAndUnlink(object state)
			{
				DataflowBlock.OutputAvailableAsyncTarget<T> state2 = state as DataflowBlock.OutputAvailableAsyncTarget<T>;
				System.Threading.Tasks.Task.Factory.StartNew(delegate(object tgt)
				{
					DataflowBlock.OutputAvailableAsyncTarget<T> outputAvailableAsyncTarget = (DataflowBlock.OutputAvailableAsyncTarget<T>)tgt;
					outputAvailableAsyncTarget.TrySetCanceled();
					outputAvailableAsyncTarget.AttemptThreadSafeUnlink();
				}, state2, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
			}

			// Token: 0x06000098 RID: 152 RVA: 0x00003EF4 File Offset: 0x000020F4
			internal void AttemptThreadSafeUnlink()
			{
				IDisposable unlinker = this._unlinker;
				if (unlinker != null && Interlocked.CompareExchange<IDisposable>(ref this._unlinker, null, unlinker) == unlinker)
				{
					unlinker.Dispose();
				}
			}

			// Token: 0x06000099 RID: 153 RVA: 0x00003F21 File Offset: 0x00002121
			DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (source == null)
				{
					throw new ArgumentNullException("source");
				}
				base.TrySetResult(true);
				return DataflowMessageStatus.DecliningPermanently;
			}

			// Token: 0x0600009A RID: 154 RVA: 0x00003F53 File Offset: 0x00002153
			void IDataflowBlock.Complete()
			{
				base.TrySetResult(false);
			}

			// Token: 0x0600009B RID: 155 RVA: 0x00003F5D File Offset: 0x0000215D
			void IDataflowBlock.Fault(Exception exception)
			{
				if (exception == null)
				{
					throw new ArgumentNullException("exception");
				}
				base.TrySetResult(false);
			}

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x0600009C RID: 156 RVA: 0x000033CB File Offset: 0x000015CB
			Task IDataflowBlock.Completion
			{
				get
				{
					throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
				}
			}

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x0600009D RID: 157 RVA: 0x00003F75 File Offset: 0x00002175
			private object DebuggerDisplayContent
			{
				get
				{
					return string.Format("{0} IsCompleted={1}", Common.GetNameForDebugger(this, null), base.Task.IsCompleted);
				}
			}

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x0600009E RID: 158 RVA: 0x00003F98 File Offset: 0x00002198
			object IDebuggerDisplay.Content
			{
				get
				{
					return this.DebuggerDisplayContent;
				}
			}

			// Token: 0x0400003C RID: 60
			internal static readonly Func<Task<bool>, object, bool> s_handleCompletion = delegate(Task<bool> antecedent, object state)
			{
				DataflowBlock.OutputAvailableAsyncTarget<T> outputAvailableAsyncTarget = state as DataflowBlock.OutputAvailableAsyncTarget<T>;
				outputAvailableAsyncTarget._ctr.Dispose();
				return antecedent.GetAwaiter().GetResult();
			};

			// Token: 0x0400003D RID: 61
			internal static readonly Action<object> s_cancelAndUnlink = new Action<object>(DataflowBlock.OutputAvailableAsyncTarget<T>.CancelAndUnlink);

			// Token: 0x0400003E RID: 62
			internal IDisposable _unlinker;

			// Token: 0x0400003F RID: 63
			internal CancellationTokenRegistration _ctr;
		}

		// Token: 0x02000019 RID: 25
		[DebuggerTypeProxy(typeof(DataflowBlock.EncapsulatingPropagator<, >.DebugView))]
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class EncapsulatingPropagator<TInput, TOutput> : IPropagatorBlock<TInput, TOutput>, ITargetBlock<!0>, IDataflowBlock, ISourceBlock<!1>, IReceivableSourceBlock<TOutput>, IDebuggerDisplay
		{
			// Token: 0x060000A5 RID: 165 RVA: 0x0000402D File Offset: 0x0000222D
			public EncapsulatingPropagator(ITargetBlock<TInput> target, ISourceBlock<TOutput> source)
			{
				this._target = target;
				this._source = source;
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x00004043 File Offset: 0x00002243
			public void Complete()
			{
				this._target.Complete();
			}

			// Token: 0x060000A7 RID: 167 RVA: 0x00004050 File Offset: 0x00002250
			void IDataflowBlock.Fault(Exception exception)
			{
				if (exception == null)
				{
					throw new ArgumentNullException("exception");
				}
				this._target.Fault(exception);
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x0000406C File Offset: 0x0000226C
			public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
			{
				return this._target.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
			}

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000407E File Offset: 0x0000227E
			public Task Completion
			{
				get
				{
					return this._source.Completion;
				}
			}

			// Token: 0x060000AA RID: 170 RVA: 0x0000408B File Offset: 0x0000228B
			public IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions)
			{
				return this._source.LinkTo(target, linkOptions);
			}

			// Token: 0x060000AB RID: 171 RVA: 0x0000409C File Offset: 0x0000229C
			public bool TryReceive(Predicate<TOutput> filter, out TOutput item)
			{
				IReceivableSourceBlock<TOutput> receivableSourceBlock = this._source as IReceivableSourceBlock<TOutput>;
				if (receivableSourceBlock != null)
				{
					return receivableSourceBlock.TryReceive(filter, out item);
				}
				item = default(TOutput);
				return false;
			}

			// Token: 0x060000AC RID: 172 RVA: 0x000040CC File Offset: 0x000022CC
			public bool TryReceiveAll(out IList<TOutput> items)
			{
				IReceivableSourceBlock<TOutput> receivableSourceBlock = this._source as IReceivableSourceBlock<TOutput>;
				if (receivableSourceBlock != null)
				{
					return receivableSourceBlock.TryReceiveAll(out items);
				}
				items = null;
				return false;
			}

			// Token: 0x060000AD RID: 173 RVA: 0x000040F4 File Offset: 0x000022F4
			public TOutput ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
			{
				return this._source.ConsumeMessage(messageHeader, target, out messageConsumed);
			}

			// Token: 0x060000AE RID: 174 RVA: 0x00004104 File Offset: 0x00002304
			public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
			{
				return this._source.ReserveMessage(messageHeader, target);
			}

			// Token: 0x060000AF RID: 175 RVA: 0x00004113 File Offset: 0x00002313
			public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
			{
				this._source.ReleaseReservation(messageHeader, target);
			}

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004124 File Offset: 0x00002324
			private object DebuggerDisplayContent
			{
				get
				{
					IDebuggerDisplay debuggerDisplay = this._target as IDebuggerDisplay;
					IDebuggerDisplay debuggerDisplay2 = this._source as IDebuggerDisplay;
					return string.Format("{0} Target=\"{1}\", Source=\"{2}\"", Common.GetNameForDebugger(this, null), (debuggerDisplay != null) ? debuggerDisplay.Content : this._target, (debuggerDisplay2 != null) ? debuggerDisplay2.Content : this._source);
				}
			}

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000417C File Offset: 0x0000237C
			object IDebuggerDisplay.Content
			{
				get
				{
					return this.DebuggerDisplayContent;
				}
			}

			// Token: 0x04000042 RID: 66
			private ITargetBlock<TInput> _target;

			// Token: 0x04000043 RID: 67
			private ISourceBlock<TOutput> _source;

			// Token: 0x0200001A RID: 26
			private sealed class DebugView
			{
				// Token: 0x060000B2 RID: 178 RVA: 0x00004184 File Offset: 0x00002384
				public DebugView(DataflowBlock.EncapsulatingPropagator<TInput, TOutput> propagator)
				{
					this._propagator = propagator;
				}

				// Token: 0x1700002C RID: 44
				// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004193 File Offset: 0x00002393
				public ITargetBlock<TInput> Target
				{
					get
					{
						return this._propagator._target;
					}
				}

				// Token: 0x1700002D RID: 45
				// (get) Token: 0x060000B4 RID: 180 RVA: 0x000041A0 File Offset: 0x000023A0
				public ISourceBlock<TOutput> Source
				{
					get
					{
						return this._propagator._source;
					}
				}

				// Token: 0x04000044 RID: 68
				private readonly DataflowBlock.EncapsulatingPropagator<TInput, TOutput> _propagator;
			}
		}

		// Token: 0x0200001B RID: 27
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class ChooseTarget<T> : TaskCompletionSource<T>, ITargetBlock<!0>, IDataflowBlock, IDebuggerDisplay
		{
			// Token: 0x060000B5 RID: 181 RVA: 0x000041AD File Offset: 0x000023AD
			internal ChooseTarget(StrongBox<Task> completed, CancellationToken cancellationToken)
			{
				this._completed = completed;
				Common.WireCancellationToComplete(cancellationToken, base.Task, delegate(object state)
				{
					DataflowBlock.ChooseTarget<T> chooseTarget = (DataflowBlock.ChooseTarget<T>)state;
					StrongBox<Task> completed2 = chooseTarget._completed;
					lock (completed2)
					{
						chooseTarget.TrySetCanceled();
					}
				}, this);
			}

			// Token: 0x060000B6 RID: 182 RVA: 0x000041E8 File Offset: 0x000023E8
			public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (source == null && consumeToAccept)
				{
					throw new ArgumentException(SR.Argument_CantConsumeFromANullSource, "consumeToAccept");
				}
				StrongBox<Task> completed = this._completed;
				DataflowMessageStatus result;
				lock (completed)
				{
					if (this._completed.Value != null || base.Task.IsCompleted)
					{
						result = DataflowMessageStatus.DecliningPermanently;
					}
					else
					{
						if (consumeToAccept)
						{
							bool flag2;
							messageValue = source.ConsumeMessage(messageHeader, this, out flag2);
							if (!flag2)
							{
								return DataflowMessageStatus.NotAvailable;
							}
						}
						base.TrySetResult(messageValue);
						this._completed.Value = base.Task;
						result = DataflowMessageStatus.Accepted;
					}
				}
				return result;
			}

			// Token: 0x060000B7 RID: 183 RVA: 0x000042A8 File Offset: 0x000024A8
			void IDataflowBlock.Complete()
			{
				StrongBox<Task> completed = this._completed;
				lock (completed)
				{
					base.TrySetCanceled();
				}
			}

			// Token: 0x060000B8 RID: 184 RVA: 0x00003D53 File Offset: 0x00001F53
			void IDataflowBlock.Fault(Exception exception)
			{
				((IDataflowBlock)this).Complete();
			}

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000B9 RID: 185 RVA: 0x000033CB File Offset: 0x000015CB
			Task IDataflowBlock.Completion
			{
				get
				{
					throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
				}
			}

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x060000BA RID: 186 RVA: 0x00003D5B File Offset: 0x00001F5B
			private object DebuggerDisplayContent
			{
				get
				{
					return string.Format("{0} IsCompleted={1}", Common.GetNameForDebugger(this, null), base.Task.IsCompleted);
				}
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x060000BB RID: 187 RVA: 0x000042EC File Offset: 0x000024EC
			object IDebuggerDisplay.Content
			{
				get
				{
					return this.DebuggerDisplayContent;
				}
			}

			// Token: 0x04000045 RID: 69
			internal static readonly Func<object, int> s_processBranchFunction = delegate(object state)
			{
				Tuple<Action<T>, T, int> tuple = (Tuple<Action<T>, T, int>)state;
				tuple.Item1(tuple.Item2);
				return tuple.Item3;
			};

			// Token: 0x04000046 RID: 70
			private StrongBox<Task> _completed;
		}

		// Token: 0x0200001D RID: 29
		[DebuggerTypeProxy(typeof(DataflowBlock.SourceObservable<>.DebugView))]
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class SourceObservable<TOutput> : IObservable<TOutput>, IDebuggerDisplay
		{
			// Token: 0x060000C1 RID: 193 RVA: 0x0000438B File Offset: 0x0000258B
			internal static IObservable<TOutput> From(ISourceBlock<TOutput> source)
			{
				return DataflowBlock.SourceObservable<TOutput>._table.GetValue(source, (ISourceBlock<TOutput> s) => new DataflowBlock.SourceObservable<TOutput>(s));
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x000043B7 File Offset: 0x000025B7
			internal SourceObservable(ISourceBlock<TOutput> source)
			{
				this._source = source;
				this._observersState = new DataflowBlock.SourceObservable<TOutput>.ObserversState(this);
			}

			// Token: 0x060000C3 RID: 195 RVA: 0x000043E0 File Offset: 0x000025E0
			private AggregateException GetCompletionError()
			{
				Task potentiallyNotSupportedCompletionTask = Common.GetPotentiallyNotSupportedCompletionTask(this._source);
				if (potentiallyNotSupportedCompletionTask == null || !potentiallyNotSupportedCompletionTask.IsFaulted)
				{
					return null;
				}
				return potentiallyNotSupportedCompletionTask.Exception;
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x0000440C File Offset: 0x0000260C
			IDisposable IObservable<!0>.Subscribe(IObserver<TOutput> observer)
			{
				if (observer == null)
				{
					throw new ArgumentNullException("observer");
				}
				Task potentiallyNotSupportedCompletionTask = Common.GetPotentiallyNotSupportedCompletionTask(this._source);
				Exception ex = null;
				object subscriptionLock = this._SubscriptionLock;
				lock (subscriptionLock)
				{
					if (potentiallyNotSupportedCompletionTask == null || !potentiallyNotSupportedCompletionTask.IsCompleted || !this._observersState.Target.Completion.IsCompleted)
					{
						this._observersState.Observers = this._observersState.Observers.Add(observer);
						if (this._observersState.Observers.Count == 1)
						{
							this._observersState.Unlinker = this._source.LinkTo(this._observersState.Target);
							if (this._observersState.Unlinker == null)
							{
								this._observersState.Observers = ImmutableArray<IObserver<TOutput>>.Empty;
								return null;
							}
						}
						return Disposables.Create<DataflowBlock.SourceObservable<TOutput>, IObserver<TOutput>>(delegate(DataflowBlock.SourceObservable<TOutput> s, IObserver<TOutput> o)
						{
							s.Unsubscribe(o);
						}, this, observer);
					}
					ex = this.GetCompletionError();
				}
				if (ex != null)
				{
					observer.OnError(ex);
				}
				else
				{
					observer.OnCompleted();
				}
				return Disposables.Nop;
			}

			// Token: 0x060000C5 RID: 197 RVA: 0x00004544 File Offset: 0x00002744
			private void Unsubscribe(IObserver<TOutput> observer)
			{
				object subscriptionLock = this._SubscriptionLock;
				lock (subscriptionLock)
				{
					DataflowBlock.SourceObservable<TOutput>.ObserversState observersState = this._observersState;
					if (observersState.Observers.Contains(observer))
					{
						if (observersState.Observers.Count == 1)
						{
							this.ResetObserverState();
						}
						else
						{
							observersState.Observers = observersState.Observers.Remove(observer);
						}
					}
				}
			}

			// Token: 0x060000C6 RID: 198 RVA: 0x000045C0 File Offset: 0x000027C0
			private ImmutableArray<IObserver<TOutput>> ResetObserverState()
			{
				DataflowBlock.SourceObservable<TOutput>.ObserversState observersState = this._observersState;
				ImmutableArray<IObserver<TOutput>> observers = observersState.Observers;
				this._observersState = new DataflowBlock.SourceObservable<TOutput>.ObserversState(this);
				observersState.Unlinker.Dispose();
				observersState.Canceler.Cancel();
				return observers;
			}

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004600 File Offset: 0x00002800
			private object DebuggerDisplayContent
			{
				get
				{
					IDebuggerDisplay debuggerDisplay = this._source as IDebuggerDisplay;
					return string.Format("Observers={0}, Block=\"{1}\"", this._observersState.Observers.Count, (debuggerDisplay != null) ? debuggerDisplay.Content : this._source);
				}
			}

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004649 File Offset: 0x00002849
			object IDebuggerDisplay.Content
			{
				get
				{
					return this.DebuggerDisplayContent;
				}
			}

			// Token: 0x04000049 RID: 73
			private static readonly ConditionalWeakTable<ISourceBlock<TOutput>, DataflowBlock.SourceObservable<TOutput>> _table = new ConditionalWeakTable<ISourceBlock<TOutput>, DataflowBlock.SourceObservable<TOutput>>();

			// Token: 0x0400004A RID: 74
			private readonly object _SubscriptionLock = new object();

			// Token: 0x0400004B RID: 75
			private readonly ISourceBlock<TOutput> _source;

			// Token: 0x0400004C RID: 76
			private DataflowBlock.SourceObservable<TOutput>.ObserversState _observersState;

			// Token: 0x0200001E RID: 30
			private sealed class DebugView
			{
				// Token: 0x060000CA RID: 202 RVA: 0x0000465D File Offset: 0x0000285D
				public DebugView(DataflowBlock.SourceObservable<TOutput> observable)
				{
					this._observable = observable;
				}

				// Token: 0x17000033 RID: 51
				// (get) Token: 0x060000CB RID: 203 RVA: 0x0000466C File Offset: 0x0000286C
				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public IObserver<TOutput>[] Observers
				{
					get
					{
						return this._observable._observersState.Observers.ToArray();
					}
				}

				// Token: 0x0400004D RID: 77
				private readonly DataflowBlock.SourceObservable<TOutput> _observable;
			}

			// Token: 0x0200001F RID: 31
			private sealed class ObserversState
			{
				// Token: 0x060000CC RID: 204 RVA: 0x00004684 File Offset: 0x00002884
				internal ObserversState(DataflowBlock.SourceObservable<TOutput> observable)
				{
					this.Observable = observable;
					this.Target = new ActionBlock<TOutput>(new Func<TOutput, Task>(this.ProcessItemAsync), DataflowBlock._nonGreedyExecutionOptions);
					this.Target.Completion.ContinueWith(delegate(Task t, object state)
					{
						((DataflowBlock.SourceObservable<TOutput>.ObserversState)state).NotifyObserversOfCompletion(t.Exception);
					}, this, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously), TaskScheduler.Default);
					Task potentiallyNotSupportedCompletionTask = Common.GetPotentiallyNotSupportedCompletionTask(this.Observable._source);
					if (potentiallyNotSupportedCompletionTask != null)
					{
						potentiallyNotSupportedCompletionTask.ContinueWith(delegate(Task _1, object state1)
						{
							DataflowBlock.SourceObservable<TOutput>.ObserversState observersState = (DataflowBlock.SourceObservable<TOutput>.ObserversState)state1;
							observersState.Target.Complete();
							observersState.Target.Completion.ContinueWith(delegate(Task _2, object state2)
							{
								((DataflowBlock.SourceObservable<TOutput>.ObserversState)state2).NotifyObserversOfCompletion(null);
							}, state1, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.ExecuteSynchronously), TaskScheduler.Default);
						}, this, this.Canceler.Token, Common.GetContinuationOptions(TaskContinuationOptions.ExecuteSynchronously), TaskScheduler.Default);
					}
				}

				// Token: 0x060000CD RID: 205 RVA: 0x0000476C File Offset: 0x0000296C
				private Task ProcessItemAsync(TOutput item)
				{
					object subscriptionLock = this.Observable._SubscriptionLock;
					ImmutableArray<IObserver<TOutput>> observers;
					lock (subscriptionLock)
					{
						observers = this.Observers;
					}
					try
					{
						foreach (IObserver<TOutput> observer in observers)
						{
							DataflowBlock.TargetObserver<TOutput> targetObserver = observer as DataflowBlock.TargetObserver<TOutput>;
							if (targetObserver != null)
							{
								Task<bool> task = targetObserver.SendAsyncToTarget(item);
								if (task.Status != TaskStatus.RanToCompletion)
								{
									if (this._tempSendAsyncTaskList == null)
									{
										this._tempSendAsyncTaskList = new List<Task<bool>>();
									}
									this._tempSendAsyncTaskList.Add(task);
								}
							}
							else
							{
								observer.OnNext(item);
							}
						}
						if (this._tempSendAsyncTaskList != null && this._tempSendAsyncTaskList.Count > 0)
						{
							Task<bool[]> result = Task.WhenAll<bool>(this._tempSendAsyncTaskList);
							this._tempSendAsyncTaskList.Clear();
							return result;
						}
					}
					catch (Exception exception)
					{
						return Common.CreateTaskFromException<VoidResult>(exception);
					}
					return Common.CompletedTaskWithTrueResult;
				}

				// Token: 0x060000CE RID: 206 RVA: 0x00004888 File Offset: 0x00002A88
				private void NotifyObserversOfCompletion(Exception targetException = null)
				{
					object subscriptionLock = this.Observable._SubscriptionLock;
					ImmutableArray<IObserver<TOutput>> observers;
					lock (subscriptionLock)
					{
						observers = this.Observers;
						if (targetException != null)
						{
							this.Observable.ResetObserverState();
						}
						this.Observers = ImmutableArray<IObserver<TOutput>>.Empty;
					}
					if (observers.Count > 0)
					{
						Exception ex = targetException ?? this.Observable.GetCompletionError();
						try
						{
							if (ex != null)
							{
								using (IEnumerator<IObserver<TOutput>> enumerator = observers.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										IObserver<TOutput> observer = enumerator.Current;
										observer.OnError(ex);
									}
									goto IL_C9;
								}
							}
							foreach (IObserver<TOutput> observer2 in observers)
							{
								observer2.OnCompleted();
							}
							IL_C9:;
						}
						catch (Exception error)
						{
							Common.ThrowAsync(error);
						}
					}
				}

				// Token: 0x0400004E RID: 78
				internal readonly DataflowBlock.SourceObservable<TOutput> Observable;

				// Token: 0x0400004F RID: 79
				internal readonly ActionBlock<TOutput> Target;

				// Token: 0x04000050 RID: 80
				internal readonly CancellationTokenSource Canceler = new CancellationTokenSource();

				// Token: 0x04000051 RID: 81
				internal ImmutableArray<IObserver<TOutput>> Observers = ImmutableArray<IObserver<TOutput>>.Empty;

				// Token: 0x04000052 RID: 82
				internal IDisposable Unlinker;

				// Token: 0x04000053 RID: 83
				private List<Task<bool>> _tempSendAsyncTaskList;
			}
		}

		// Token: 0x02000022 RID: 34
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class TargetObserver<TInput> : IObserver<TInput>, IDebuggerDisplay
		{
			// Token: 0x060000D8 RID: 216 RVA: 0x00004A4F File Offset: 0x00002C4F
			internal TargetObserver(ITargetBlock<TInput> target)
			{
				this._target = target;
			}

			// Token: 0x060000D9 RID: 217 RVA: 0x00004A60 File Offset: 0x00002C60
			void IObserver<!0>.OnNext(TInput value)
			{
				Task<bool> task = this.SendAsyncToTarget(value);
				task.GetAwaiter().GetResult();
			}

			// Token: 0x060000DA RID: 218 RVA: 0x00004A84 File Offset: 0x00002C84
			void IObserver<!0>.OnCompleted()
			{
				this._target.Complete();
			}

			// Token: 0x060000DB RID: 219 RVA: 0x00004A91 File Offset: 0x00002C91
			void IObserver<!0>.OnError(Exception error)
			{
				this._target.Fault(error);
			}

			// Token: 0x060000DC RID: 220 RVA: 0x00004A9F File Offset: 0x00002C9F
			internal Task<bool> SendAsyncToTarget(TInput value)
			{
				return this._target.SendAsync(value);
			}

			// Token: 0x17000034 RID: 52
			// (get) Token: 0x060000DD RID: 221 RVA: 0x00004AB0 File Offset: 0x00002CB0
			private object DebuggerDisplayContent
			{
				get
				{
					IDebuggerDisplay debuggerDisplay = this._target as IDebuggerDisplay;
					return string.Format("Block=\"{0}\"", (debuggerDisplay != null) ? debuggerDisplay.Content : this._target);
				}
			}

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x060000DE RID: 222 RVA: 0x00004AE4 File Offset: 0x00002CE4
			object IDebuggerDisplay.Content
			{
				get
				{
					return this.DebuggerDisplayContent;
				}
			}

			// Token: 0x0400005B RID: 91
			private readonly ITargetBlock<TInput> _target;
		}

		// Token: 0x02000023 RID: 35
		private class NullTargetBlock<TInput> : ITargetBlock<!0>, IDataflowBlock
		{
			// Token: 0x060000DF RID: 223 RVA: 0x00004AEC File Offset: 0x00002CEC
			DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (consumeToAccept)
				{
					if (source == null)
					{
						throw new ArgumentException(SR.Argument_CantConsumeFromANullSource, "consumeToAccept");
					}
					bool flag;
					source.ConsumeMessage(messageHeader, this, out flag);
					if (!flag)
					{
						return DataflowMessageStatus.NotAvailable;
					}
				}
				return DataflowMessageStatus.Accepted;
			}

			// Token: 0x060000E0 RID: 224 RVA: 0x00002927 File Offset: 0x00000B27
			void IDataflowBlock.Complete()
			{
			}

			// Token: 0x060000E1 RID: 225 RVA: 0x00002927 File Offset: 0x00000B27
			void IDataflowBlock.Fault(Exception exception)
			{
			}

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004B3A File Offset: 0x00002D3A
			Task IDataflowBlock.Completion
			{
				get
				{
					return LazyInitializer.EnsureInitialized<Task>(ref this._completion, () => new TaskCompletionSource<VoidResult>().Task);
				}
			}

			// Token: 0x0400005C RID: 92
			private Task _completion;
		}
	}
}
