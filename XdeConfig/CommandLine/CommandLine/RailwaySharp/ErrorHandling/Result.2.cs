using System;
using System.Collections.Generic;
using System.Linq;

namespace RailwaySharp.ErrorHandling
{
	// Token: 0x0200001C RID: 28
	internal static class Result
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00003E57 File Offset: 0x00002057
		public static Result<TSuccess, TMessage> FailWith<TSuccess, TMessage>(IEnumerable<TMessage> messages)
		{
			return new Bad<TSuccess, TMessage>(messages);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003E5F File Offset: 0x0000205F
		public static Result<TSuccess, TMessage> FailWith<TSuccess, TMessage>(TMessage message)
		{
			return new Bad<TSuccess, TMessage>(new TMessage[]
			{
				message
			});
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003E74 File Offset: 0x00002074
		public static Result<TSuccess, TMessage> Succeed<TSuccess, TMessage>(TSuccess value)
		{
			return new Ok<TSuccess, TMessage>(value, Enumerable.Empty<TMessage>());
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003E81 File Offset: 0x00002081
		public static Result<TSuccess, TMessage> Succeed<TSuccess, TMessage>(TSuccess value, TMessage message)
		{
			return new Ok<TSuccess, TMessage>(value, new TMessage[]
			{
				message
			});
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003E97 File Offset: 0x00002097
		public static Result<TSuccess, TMessage> Succeed<TSuccess, TMessage>(TSuccess value, IEnumerable<TMessage> messages)
		{
			return new Ok<TSuccess, TMessage>(value, messages);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003EA0 File Offset: 0x000020A0
		public static Result<TSuccess, Exception> Try<TSuccess>(Func<TSuccess> func)
		{
			Result<TSuccess, Exception> result;
			try
			{
				result = new Ok<TSuccess, Exception>(func(), Enumerable.Empty<Exception>());
			}
			catch (Exception ex)
			{
				result = new Bad<TSuccess, Exception>(new Exception[]
				{
					ex
				});
			}
			return result;
		}
	}
}
