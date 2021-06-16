using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;
using RailwaySharp.ErrorHandling;

namespace CommandLine.Infrastructure
{
	// Token: 0x02000064 RID: 100
	internal static class ResultExtensions
	{
		// Token: 0x06000286 RID: 646 RVA: 0x0000A524 File Offset: 0x00008724
		public static IEnumerable<TMessage> SuccessfulMessages<TSuccess, TMessage>(this Result<TSuccess, TMessage> result)
		{
			if (result.Tag == ResultType.Ok)
			{
				return ((Ok<TSuccess, TMessage>)result).Messages;
			}
			return Enumerable.Empty<TMessage>();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000A53F File Offset: 0x0000873F
		public static Maybe<TSuccess> ToMaybe<TSuccess, TMessage>(this Result<TSuccess, TMessage> result)
		{
			if (result.Tag == ResultType.Ok)
			{
				return Maybe.Just<TSuccess>(((Ok<TSuccess, TMessage>)result).Success);
			}
			return Maybe.Nothing<TSuccess>();
		}
	}
}
