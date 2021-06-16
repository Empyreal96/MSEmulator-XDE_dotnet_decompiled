using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000039 RID: 57
	internal static class AsyncUtils
	{
		// Token: 0x06000411 RID: 1041 RVA: 0x00010522 File Offset: 0x0000E722
		internal static Task<bool> ToAsync(this bool value)
		{
			if (!value)
			{
				return AsyncUtils.False;
			}
			return AsyncUtils.True;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00010532 File Offset: 0x0000E732
		public static Task CancelIfRequestedAsync(this CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return null;
			}
			return cancellationToken.FromCanceled();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00010545 File Offset: 0x0000E745
		public static Task<T> CancelIfRequestedAsync<T>(this CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return null;
			}
			return cancellationToken.FromCanceled<T>();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00010558 File Offset: 0x0000E758
		public static Task FromCanceled(this CancellationToken cancellationToken)
		{
			return new Task(delegate()
			{
			}, cancellationToken);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001057F File Offset: 0x0000E77F
		public static Task<T> FromCanceled<T>(this CancellationToken cancellationToken)
		{
			return new Task<T>(() => default(T), cancellationToken);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000105A6 File Offset: 0x0000E7A6
		public static Task WriteAsync(this TextWriter writer, char value, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return writer.WriteAsync(value);
			}
			return cancellationToken.FromCanceled();
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x000105BF File Offset: 0x0000E7BF
		public static Task WriteAsync(this TextWriter writer, string value, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return writer.WriteAsync(value);
			}
			return cancellationToken.FromCanceled();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000105D8 File Offset: 0x0000E7D8
		public static Task WriteAsync(this TextWriter writer, char[] value, int start, int count, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return writer.WriteAsync(value, start, count);
			}
			return cancellationToken.FromCanceled();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000105F4 File Offset: 0x0000E7F4
		public static Task<int> ReadAsync(this TextReader reader, char[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return reader.ReadAsync(buffer, index, count);
			}
			return cancellationToken.FromCanceled<int>();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00010610 File Offset: 0x0000E810
		public static bool IsCompletedSucessfully(this Task task)
		{
			return task.Status == TaskStatus.RanToCompletion;
		}

		// Token: 0x04000146 RID: 326
		public static readonly Task<bool> False = Task.FromResult<bool>(false);

		// Token: 0x04000147 RID: 327
		public static readonly Task<bool> True = Task.FromResult<bool>(true);

		// Token: 0x04000148 RID: 328
		internal static readonly Task CompletedTask = Task.Delay(0);
	}
}
