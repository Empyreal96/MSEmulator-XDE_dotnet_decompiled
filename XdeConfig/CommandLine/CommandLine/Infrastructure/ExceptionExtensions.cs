using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Infrastructure
{
	// Token: 0x0200005F RID: 95
	internal static class ExceptionExtensions
	{
		// Token: 0x06000272 RID: 626 RVA: 0x0000A21A File Offset: 0x0000841A
		public static void RethrowWhenAbsentIn(this Exception exception, IEnumerable<Type> validExceptions)
		{
			if (!validExceptions.Contains(exception.GetType()))
			{
				throw exception;
			}
		}
	}
}
