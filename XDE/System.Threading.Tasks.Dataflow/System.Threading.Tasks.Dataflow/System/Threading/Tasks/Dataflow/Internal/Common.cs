using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x0200006F RID: 111
	internal static class Common
	{
		// Token: 0x060003A5 RID: 933 RVA: 0x0000D09C File Offset: 0x0000B29C
		internal static bool TryKeepAliveUntil<TStateIn, TStateOut>(Common.KeepAlivePredicate<TStateIn, TStateOut> predicate, TStateIn stateIn, out TStateOut stateOut)
		{
			for (int i = 16; i > 0; i--)
			{
				if (!Thread.Yield() && predicate(stateIn, out stateOut))
				{
					return true;
				}
			}
			stateOut = default(TStateOut);
			return false;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
		internal static T UnwrapWeakReference<T>(object state) where T : class
		{
			WeakReference<T> weakReference = state as WeakReference<T>;
			T result;
			if (!weakReference.TryGetTarget(out result))
			{
				return default(T);
			}
			return result;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000D100 File Offset: 0x0000B300
		internal static int GetBlockId(IDataflowBlock block)
		{
			Task potentiallyNotSupportedCompletionTask = Common.GetPotentiallyNotSupportedCompletionTask(block);
			if (potentiallyNotSupportedCompletionTask == null)
			{
				return 0;
			}
			return potentiallyNotSupportedCompletionTask.Id;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000D120 File Offset: 0x0000B320
		internal static string GetNameForDebugger(IDataflowBlock block, DataflowBlockOptions options = null)
		{
			if (block == null)
			{
				return string.Empty;
			}
			string name = block.GetType().Name;
			if (options == null)
			{
				return name;
			}
			int blockId = Common.GetBlockId(block);
			string result;
			try
			{
				result = string.Format(options.NameFormat, name, blockId);
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000D180 File Offset: 0x0000B380
		internal static bool IsCooperativeCancellation(Exception exception)
		{
			return exception is OperationCanceledException;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000D18C File Offset: 0x0000B38C
		internal static void WireCancellationToComplete(CancellationToken cancellationToken, Task completionTask, Action<object> completeAction, object completeState)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				completeAction(completeState);
				return;
			}
			if (cancellationToken.CanBeCanceled)
			{
				CancellationTokenRegistration cancellationTokenRegistration = cancellationToken.Register(completeAction, completeState);
				completionTask.ContinueWith(delegate(Task completed, object state)
				{
					((CancellationTokenRegistration)state).Dispose();
				}, cancellationTokenRegistration, cancellationToken, Common.GetContinuationOptions(TaskContinuationOptions.None), TaskScheduler.Default);
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		internal static Exception InitializeStackTrace(Exception exception)
		{
			try
			{
				throw exception;
			}
			catch
			{
			}
			return exception;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000D220 File Offset: 0x0000B420
		internal static void StoreDataflowMessageValueIntoExceptionData<T>(Exception exc, T messageValue, bool targetInnerExceptions = false)
		{
			string text = messageValue as string;
			if (text == null && messageValue != null)
			{
				try
				{
					text = messageValue.ToString();
				}
				catch
				{
				}
			}
			if (text == null)
			{
				return;
			}
			Common.StoreStringIntoExceptionData(exc, "DataflowMessageValue", text);
			if (targetInnerExceptions)
			{
				AggregateException ex = exc as AggregateException;
				if (ex != null)
				{
					using (IEnumerator<Exception> enumerator = ex.InnerExceptions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Exception exception = enumerator.Current;
							Common.StoreStringIntoExceptionData(exception, "DataflowMessageValue", text);
						}
						return;
					}
				}
				if (exc.InnerException != null)
				{
					Common.StoreStringIntoExceptionData(exc.InnerException, "DataflowMessageValue", text);
				}
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000D2E0 File Offset: 0x0000B4E0
		private static void StoreStringIntoExceptionData(Exception exception, string key, string value)
		{
			try
			{
				IDictionary data = exception.Data;
				if (data != null && !data.IsFixedSize && !data.IsReadOnly && data[key] == null)
				{
					data[key] = value;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000D330 File Offset: 0x0000B530
		internal static void ThrowAsync(Exception error)
		{
			ExceptionDispatchInfo state2 = ExceptionDispatchInfo.Capture(error);
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				((ExceptionDispatchInfo)state).Throw();
			}, state2);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000D36C File Offset: 0x0000B56C
		internal static void AddException(ref List<Exception> list, Exception exception, bool unwrapInnerExceptions = false)
		{
			if (list == null)
			{
				list = new List<Exception>();
			}
			if (!unwrapInnerExceptions)
			{
				list.Add(exception);
				return;
			}
			AggregateException ex = exception as AggregateException;
			if (ex != null)
			{
				list.AddRange(ex.InnerExceptions);
				return;
			}
			list.Add(exception.InnerException);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000D3B8 File Offset: 0x0000B5B8
		private static Task<bool> CreateCachedBooleanTask(bool value)
		{
			AsyncTaskMethodBuilder<bool> asyncTaskMethodBuilder = AsyncTaskMethodBuilder<bool>.Create();
			asyncTaskMethodBuilder.SetResult(value);
			return asyncTaskMethodBuilder.Task;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000D3DC File Offset: 0x0000B5DC
		private static TaskCompletionSource<T> CreateCachedTaskCompletionSource<T>()
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetResult(default(T));
			return taskCompletionSource;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000D400 File Offset: 0x0000B600
		internal static Task<TResult> CreateTaskFromException<TResult>(Exception exception)
		{
			AsyncTaskMethodBuilder<TResult> asyncTaskMethodBuilder = AsyncTaskMethodBuilder<TResult>.Create();
			asyncTaskMethodBuilder.SetException(exception);
			return asyncTaskMethodBuilder.Task;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000D424 File Offset: 0x0000B624
		internal static Task<TResult> CreateTaskFromCancellation<TResult>(CancellationToken cancellationToken)
		{
			return new Task<TResult>(Common.CachedGenericDelegates<TResult>.DefaultTResultFunc, cancellationToken);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000D440 File Offset: 0x0000B640
		internal static Task GetPotentiallyNotSupportedCompletionTask(IDataflowBlock block)
		{
			try
			{
				return block.Completion;
			}
			catch (NotImplementedException)
			{
			}
			catch (NotSupportedException)
			{
			}
			return null;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000D47C File Offset: 0x0000B67C
		internal static IDisposable CreateUnlinker<TOutput>(object outgoingLock, TargetRegistry<TOutput> targetRegistry, ITargetBlock<TOutput> targetBlock)
		{
			return Disposables.Create<object, TargetRegistry<TOutput>, ITargetBlock<TOutput>>(Common.CachedGenericDelegates<TOutput>.CreateUnlinkerShimAction, outgoingLock, targetRegistry, targetBlock);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000D48C File Offset: 0x0000B68C
		internal static bool IsValidTimeout(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			return num >= -1L && num <= 2147483647L;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000D4B5 File Offset: 0x0000B6B5
		internal static TaskContinuationOptions GetContinuationOptions(TaskContinuationOptions toInclude = TaskContinuationOptions.None)
		{
			return toInclude | TaskContinuationOptions.DenyChildAttach;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000D4BC File Offset: 0x0000B6BC
		internal static TaskCreationOptions GetCreationOptionsForTask(bool isReplacementReplica = false)
		{
			TaskCreationOptions taskCreationOptions = TaskCreationOptions.DenyChildAttach;
			if (isReplacementReplica)
			{
				taskCreationOptions |= TaskCreationOptions.PreferFairness;
			}
			return taskCreationOptions;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000D4D3 File Offset: 0x0000B6D3
		internal static Exception StartTaskSafe(Task task, TaskScheduler scheduler)
		{
			if (scheduler == TaskScheduler.Default)
			{
				task.Start(scheduler);
				return null;
			}
			return Common.StartTaskSafeCore(task, scheduler);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000D4F0 File Offset: 0x0000B6F0
		private static Exception StartTaskSafeCore(Task task, TaskScheduler scheduler)
		{
			Exception result = null;
			try
			{
				task.Start(scheduler);
			}
			catch (Exception ex)
			{
				AggregateException exception = task.Exception;
				result = ex;
			}
			return result;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000D528 File Offset: 0x0000B728
		internal static void ReleaseAllPostponedMessages<T>(ITargetBlock<T> target, QueuedMap<ISourceBlock<T>, DataflowMessageHeader> postponedMessages, ref List<Exception> exceptions)
		{
			int count = postponedMessages.Count;
			int num = 0;
			KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> keyValuePair;
			while (postponedMessages.TryPop(out keyValuePair))
			{
				try
				{
					if (keyValuePair.Key.ReserveMessage(keyValuePair.Value, target))
					{
						keyValuePair.Key.ReleaseReservation(keyValuePair.Value, target);
					}
				}
				catch (Exception exception)
				{
					Common.AddException(ref exceptions, exception, false);
				}
				num++;
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000D598 File Offset: 0x0000B798
		internal static void PropagateCompletion(Task sourceCompletionTask, IDataflowBlock target, Action<Exception> exceptionHandler)
		{
			AggregateException ex = sourceCompletionTask.IsFaulted ? sourceCompletionTask.Exception : null;
			try
			{
				if (ex != null)
				{
					target.Fault(ex);
				}
				else
				{
					target.Complete();
				}
			}
			catch (Exception obj)
			{
				if (exceptionHandler == null)
				{
					throw;
				}
				exceptionHandler(obj);
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000D5EC File Offset: 0x0000B7EC
		private static void PropagateCompletionAsContinuation(Task sourceCompletionTask, IDataflowBlock target)
		{
			sourceCompletionTask.ContinueWith(delegate(Task task, object state)
			{
				Common.PropagateCompletion(task, (IDataflowBlock)state, Common.AsyncExceptionHandler);
			}, target, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None), TaskScheduler.Default);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000D625 File Offset: 0x0000B825
		internal static void PropagateCompletionOnceCompleted(Task sourceCompletionTask, IDataflowBlock target)
		{
			if (sourceCompletionTask.IsCompleted)
			{
				Common.PropagateCompletion(sourceCompletionTask, target, null);
				return;
			}
			Common.PropagateCompletionAsContinuation(sourceCompletionTask, target);
		}

		// Token: 0x0400016C RID: 364
		internal static readonly DataflowMessageHeader SingleMessageHeader = new DataflowMessageHeader(1L);

		// Token: 0x0400016D RID: 365
		internal static readonly Task<bool> CompletedTaskWithTrueResult = Common.CreateCachedBooleanTask(true);

		// Token: 0x0400016E RID: 366
		internal static readonly Task<bool> CompletedTaskWithFalseResult = Common.CreateCachedBooleanTask(false);

		// Token: 0x0400016F RID: 367
		internal static readonly TaskCompletionSource<VoidResult> CompletedVoidResultTaskCompletionSource = Common.CreateCachedTaskCompletionSource<VoidResult>();

		// Token: 0x04000170 RID: 368
		internal static readonly TimeSpan InfiniteTimeSpan = Timeout.InfiniteTimeSpan;

		// Token: 0x04000171 RID: 369
		internal static readonly Action<Exception> AsyncExceptionHandler = new Action<Exception>(Common.ThrowAsync);

		// Token: 0x02000070 RID: 112
		// (Invoke) Token: 0x060003C1 RID: 961
		internal delegate bool KeepAlivePredicate<TStateIn, TStateOut>(TStateIn stateIn, out TStateOut stateOut);

		// Token: 0x02000071 RID: 113
		private static class CachedGenericDelegates<T>
		{
			// Token: 0x04000172 RID: 370
			internal static readonly Func<T> DefaultTResultFunc = () => default(T);

			// Token: 0x04000173 RID: 371
			internal static readonly Action<object, TargetRegistry<T>, ITargetBlock<T>> CreateUnlinkerShimAction = delegate(object syncObj, TargetRegistry<T> registry, ITargetBlock<T> target)
			{
				lock (syncObj)
				{
					registry.Remove(target, false);
				}
			};
		}
	}
}
