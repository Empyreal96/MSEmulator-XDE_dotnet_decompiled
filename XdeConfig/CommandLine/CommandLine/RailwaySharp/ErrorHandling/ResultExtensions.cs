using System;
using System.Collections.Generic;
using System.Linq;

namespace RailwaySharp.ErrorHandling
{
	// Token: 0x0200001E RID: 30
	internal static class ResultExtensions
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x000041B4 File Offset: 0x000023B4
		public static void Match<TSuccess, TMessage>(this Result<TSuccess, TMessage> result, Action<TSuccess, IEnumerable<TMessage>> ifSuccess, Action<IEnumerable<TMessage>> ifFailure)
		{
			Ok<TSuccess, TMessage> ok = result as Ok<TSuccess, TMessage>;
			if (ok != null)
			{
				ifSuccess(ok.Success, ok.Messages);
				return;
			}
			Bad<TSuccess, TMessage> bad = (Bad<TSuccess, TMessage>)result;
			ifFailure(bad.Messages);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000041F4 File Offset: 0x000023F4
		public static TResult Either<TSuccess, TMessage, TResult>(this Result<TSuccess, TMessage> result, Func<TSuccess, IEnumerable<TMessage>, TResult> ifSuccess, Func<IEnumerable<TMessage>, TResult> ifFailure)
		{
			Ok<TSuccess, TMessage> ok = result as Ok<TSuccess, TMessage>;
			if (ok != null)
			{
				return ifSuccess(ok.Success, ok.Messages);
			}
			Bad<TSuccess, TMessage> bad = (Bad<TSuccess, TMessage>)result;
			return ifFailure(bad.Messages);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004231 File Offset: 0x00002431
		public static Result<TResult, TMessage> Map<TSuccess, TMessage, TResult>(this Result<TSuccess, TMessage> result, Func<TSuccess, TResult> func)
		{
			return Trial.Lift<TSuccess, TResult, TMessage>(func, result);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000423A File Offset: 0x0000243A
		public static Result<IEnumerable<TSuccess>, TMessage> Collect<TSuccess, TMessage>(this IEnumerable<Result<TSuccess, TMessage>> values)
		{
			return Trial.Collect<TSuccess, TMessage>(values);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004244 File Offset: 0x00002444
		public static Result<IEnumerable<TSuccess>, TMessage> Flatten<TSuccess, TMessage>(this Result<IEnumerable<Result<TSuccess, TMessage>>, TMessage> result)
		{
			if (result.Tag != ResultType.Ok)
			{
				return new Bad<IEnumerable<TSuccess>, TMessage>(((Bad<IEnumerable<Result<TSuccess, TMessage>>, TMessage>)result).Messages);
			}
			Result<IEnumerable<TSuccess>, TMessage> result2 = ((Ok<IEnumerable<Result<TSuccess, TMessage>>, TMessage>)result).Success.Collect<TSuccess, TMessage>();
			if (result2.Tag == ResultType.Ok)
			{
				Ok<IEnumerable<TSuccess>, TMessage> ok = (Ok<IEnumerable<TSuccess>, TMessage>)result2;
				return new Ok<IEnumerable<TSuccess>, TMessage>(ok.Success, ok.Messages);
			}
			return new Bad<IEnumerable<TSuccess>, TMessage>(((Bad<IEnumerable<TSuccess>, TMessage>)result2).Messages);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000042AC File Offset: 0x000024AC
		public static Result<TResult, TMessage> SelectMany<TSuccess, TMessage, TResult>(this Result<TSuccess, TMessage> result, Func<TSuccess, Result<TResult, TMessage>> func)
		{
			return Trial.Bind<TSuccess, TResult, TMessage>(func, result);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000042B8 File Offset: 0x000024B8
		public static Result<TResult, TMessage> SelectMany<TSuccess, TMessage, TValue, TResult>(this Result<TSuccess, TMessage> result, Func<TSuccess, Result<TValue, TMessage>> func, Func<TSuccess, TValue, TResult> mapperFunc)
		{
			Func<TSuccess, Func<TValue, TResult>> curriedMapper = (TSuccess suc) => (TValue val) => mapperFunc(suc, val);
			Func<Result<TSuccess, TMessage>, Result<TValue, TMessage>, Result<TResult, TMessage>> func2 = (Result<TSuccess, TMessage> a, Result<TValue, TMessage> b) => Trial.Lift2<TSuccess, TValue, TResult, TMessage>(curriedMapper, a, b);
			Result<TValue, TMessage> arg = Trial.Bind<TSuccess, TValue, TMessage>(func, result);
			return func2(result, arg);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000042FD File Offset: 0x000024FD
		public static Result<TResult, TMessage> Select<TSuccess, TMessage, TResult>(this Result<TSuccess, TMessage> result, Func<TSuccess, TResult> func)
		{
			return Trial.Lift<TSuccess, TResult, TMessage>(func, result);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004308 File Offset: 0x00002508
		public static IEnumerable<TMessage> FailedWith<TSuccess, TMessage>(this Result<TSuccess, TMessage> result)
		{
			if (result.Tag == ResultType.Ok)
			{
				Ok<TSuccess, TMessage> ok = (Ok<TSuccess, TMessage>)result;
				throw new Exception(string.Format("Result was a success: {0} - {1}", ok.Success, string.Join(Environment.NewLine, from m in ok.Messages
				select m.ToString())));
			}
			return ((Bad<TSuccess, TMessage>)result).Messages;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004380 File Offset: 0x00002580
		public static TSuccess SucceededWith<TSuccess, TMessage>(this Result<TSuccess, TMessage> result)
		{
			if (result.Tag == ResultType.Ok)
			{
				return ((Ok<TSuccess, TMessage>)result).Success;
			}
			Bad<TSuccess, TMessage> bad = (Bad<TSuccess, TMessage>)result;
			throw new Exception(string.Format("Result was an error: {0}", string.Join(Environment.NewLine, from m in bad.Messages
			select m.ToString())));
		}
	}
}
