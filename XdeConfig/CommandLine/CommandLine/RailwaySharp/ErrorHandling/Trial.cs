using System;
using System.Collections.Generic;
using System.Linq;

namespace RailwaySharp.ErrorHandling
{
	// Token: 0x0200001D RID: 29
	internal static class Trial
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00003EE4 File Offset: 0x000020E4
		public static Result<TSuccess, TMessage> Ok<TSuccess, TMessage>(TSuccess value)
		{
			return new Ok<TSuccess, TMessage>(value, Enumerable.Empty<TMessage>());
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003EF1 File Offset: 0x000020F1
		public static Result<TSuccess, TMessage> Pass<TSuccess, TMessage>(TSuccess value)
		{
			return new Ok<TSuccess, TMessage>(value, Enumerable.Empty<TMessage>());
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003EFE File Offset: 0x000020FE
		public static Result<TSuccess, TMessage> Warn<TSuccess, TMessage>(TMessage message, TSuccess value)
		{
			return new Ok<TSuccess, TMessage>(value, new TMessage[]
			{
				message
			});
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003F14 File Offset: 0x00002114
		public static Result<TSuccess, TMessage> Fail<TSuccess, TMessage>(TMessage message)
		{
			return new Bad<TSuccess, TMessage>(new TMessage[]
			{
				message
			});
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003F29 File Offset: 0x00002129
		public static bool Failed<TSuccess, TMessage>(Result<TSuccess, TMessage> result)
		{
			return result.Tag == ResultType.Bad;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003F34 File Offset: 0x00002134
		public static TResult Either<TSuccess, TMessage, TResult>(Func<TSuccess, IEnumerable<TMessage>, TResult> successFunc, Func<IEnumerable<TMessage>, TResult> failureFunc, Result<TSuccess, TMessage> trialResult)
		{
			Ok<TSuccess, TMessage> ok = trialResult as Ok<TSuccess, TMessage>;
			if (ok != null)
			{
				return successFunc(ok.Success, ok.Messages);
			}
			Bad<TSuccess, TMessage> bad = (Bad<TSuccess, TMessage>)trialResult;
			return failureFunc(bad.Messages);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003F74 File Offset: 0x00002174
		public static TSuccess ReturnOrFail<TSuccess, TMessage>(Result<TSuccess, TMessage> result)
		{
			Func<IEnumerable<TMessage>, TSuccess> failureFunc = delegate(IEnumerable<TMessage> msgs)
			{
				throw new Exception(string.Join(Environment.NewLine, from m in msgs
				select m.ToString()));
			};
			return Trial.Either<TSuccess, TMessage, TSuccess>((TSuccess succ, IEnumerable<TMessage> _) => succ, failureFunc, result);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003FC8 File Offset: 0x000021C8
		public static Result<TSuccess, TMessage> MergeMessages<TSuccess, TMessage>(IEnumerable<TMessage> messages, Result<TSuccess, TMessage> result)
		{
			Func<TSuccess, IEnumerable<TMessage>, Result<TSuccess, TMessage>> successFunc = (TSuccess succ, IEnumerable<TMessage> msgs) => new Ok<TSuccess, TMessage>(succ, messages.Concat(msgs));
			Func<IEnumerable<TMessage>, Result<TSuccess, TMessage>> failureFunc = (IEnumerable<TMessage> errors) => new Bad<TSuccess, TMessage>(errors.Concat(messages));
			return Trial.Either<TSuccess, TMessage, Result<TSuccess, TMessage>>(successFunc, failureFunc, result);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004004 File Offset: 0x00002204
		public static Result<TSuccess, TMessage> Bind<TValue, TSuccess, TMessage>(Func<TValue, Result<TSuccess, TMessage>> func, Result<TValue, TMessage> result)
		{
			Func<TValue, IEnumerable<TMessage>, Result<TSuccess, TMessage>> successFunc = (TValue succ, IEnumerable<TMessage> msgs) => Trial.MergeMessages<TSuccess, TMessage>(msgs, func(succ));
			Func<IEnumerable<TMessage>, Result<TSuccess, TMessage>> failureFunc = (IEnumerable<TMessage> messages) => new Bad<TSuccess, TMessage>(messages);
			return Trial.Either<TValue, TMessage, Result<TSuccess, TMessage>>(successFunc, failureFunc, result);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000404F File Offset: 0x0000224F
		public static Result<TSuccess, TMessage> Flatten<TSuccess, TMessage>(Result<Result<TSuccess, TMessage>, TMessage> result)
		{
			return Trial.Bind<Result<TSuccess, TMessage>, TSuccess, TMessage>((Result<TSuccess, TMessage> x) => x, result);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004078 File Offset: 0x00002278
		public static Result<TSuccess, TMessage> Apply<TValue, TSuccess, TMessage>(Result<Func<TValue, TSuccess>, TMessage> wrappedFunction, Result<TValue, TMessage> result)
		{
			if (wrappedFunction.Tag == ResultType.Ok && result.Tag == ResultType.Ok)
			{
				Ok<Func<TValue, TSuccess>, TMessage> ok = (Ok<Func<TValue, TSuccess>, TMessage>)wrappedFunction;
				Ok<TValue, TMessage> ok2 = (Ok<TValue, TMessage>)result;
				return new Ok<TSuccess, TMessage>(ok.Success(ok2.Success), ok.Messages.Concat(ok2.Messages));
			}
			if (wrappedFunction.Tag == ResultType.Bad && result.Tag == ResultType.Ok)
			{
				return new Bad<TSuccess, TMessage>(((Bad<TValue, TMessage>)result).Messages);
			}
			if (wrappedFunction.Tag == ResultType.Ok && result.Tag == ResultType.Bad)
			{
				return new Bad<TSuccess, TMessage>(((Bad<TValue, TMessage>)result).Messages);
			}
			Bad<Func<TValue, TSuccess>, TMessage> bad = (Bad<Func<TValue, TSuccess>, TMessage>)wrappedFunction;
			Bad<TValue, TMessage> bad2 = (Bad<TValue, TMessage>)result;
			return new Bad<TSuccess, TMessage>(bad.Messages.Concat(bad2.Messages));
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004131 File Offset: 0x00002331
		public static Result<TSuccess, TMessage> Lift<TValue, TSuccess, TMessage>(Func<TValue, TSuccess> func, Result<TValue, TMessage> result)
		{
			return Trial.Apply<TValue, TSuccess, TMessage>(Trial.Ok<Func<TValue, TSuccess>, TMessage>(func), result);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000413F File Offset: 0x0000233F
		public static Result<TSuccess1, TMessage1> Lift2<TSuccess, TMessage, TSuccess1, TMessage1>(Func<TSuccess, Func<TMessage, TSuccess1>> func, Result<TSuccess, TMessage1> a, Result<TMessage, TMessage1> b)
		{
			return Trial.Apply<TMessage, TSuccess1, TMessage1>(Trial.Lift<TSuccess, Func<TMessage, TSuccess1>, TMessage1>(func, a), b);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004150 File Offset: 0x00002350
		public static Result<IEnumerable<TSuccess>, TMessage> Collect<TSuccess, TMessage>(IEnumerable<Result<TSuccess, TMessage>> xs)
		{
			return Trial.Lift<IEnumerable<TSuccess>, IEnumerable<TSuccess>, TMessage>(new Func<IEnumerable<TSuccess>, IEnumerable<TSuccess>>(Enumerable.Reverse<TSuccess>), xs.Aggregate(null, delegate(Result<IEnumerable<TSuccess>, TMessage> result, Result<TSuccess, TMessage> next)
			{
				if (result.Tag == ResultType.Ok && next.Tag == ResultType.Ok)
				{
					Ok<IEnumerable<TSuccess>, TMessage> ok = (Ok<IEnumerable<TSuccess>, TMessage>)result;
					Ok<TSuccess, TMessage> ok2 = (Ok<TSuccess, TMessage>)next;
					return new Ok<IEnumerable<TSuccess>, TMessage>(Enumerable.Empty<TSuccess>().Concat(new TSuccess[]
					{
						ok2.Success
					}).Concat(ok.Success), ok.Messages.Concat(ok2.Messages));
				}
				if ((result.Tag == ResultType.Ok && next.Tag == ResultType.Bad) || (result.Tag == ResultType.Bad && next.Tag == ResultType.Ok))
				{
					IEnumerable<TMessage> first = (result.Tag == ResultType.Ok) ? ((Ok<IEnumerable<TSuccess>, TMessage>)result).Messages : ((Bad<TSuccess, TMessage>)next).Messages;
					IEnumerable<TMessage> second = (result.Tag == ResultType.Bad) ? ((Bad<IEnumerable<TSuccess>, TMessage>)result).Messages : ((Ok<TSuccess, TMessage>)next).Messages;
					return new Bad<IEnumerable<TSuccess>, TMessage>(first.Concat(second));
				}
				Bad<IEnumerable<TSuccess>, TMessage> bad = (Bad<IEnumerable<TSuccess>, TMessage>)result;
				Bad<TSuccess, TMessage> bad2 = (Bad<TSuccess, TMessage>)next;
				return new Bad<IEnumerable<TSuccess>, TMessage>(bad.Messages.Concat(bad2.Messages));
			}, (Result<IEnumerable<TSuccess>, TMessage> x) => x));
		}
	}
}
