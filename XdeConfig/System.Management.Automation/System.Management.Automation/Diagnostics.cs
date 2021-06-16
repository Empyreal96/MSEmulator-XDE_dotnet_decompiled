using System;
using System.Diagnostics;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000867 RID: 2151
	internal sealed class Diagnostics
	{
		// Token: 0x0600529E RID: 21150 RVA: 0x001B8F4C File Offset: 0x001B714C
		internal static string StackTrace(int framesToSkip)
		{
			StackTrace stackTrace = new StackTrace(true);
			StackFrame[] frames = stackTrace.GetFrames();
			StringBuilder stringBuilder = new StringBuilder();
			int num = 10;
			num += framesToSkip;
			int num2 = framesToSkip;
			while (num2 < frames.Length && num2 < num)
			{
				StackFrame stackFrame = frames[num2];
				stringBuilder.Append(stackFrame.ToString());
				num2++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x0600529F RID: 21151 RVA: 0x001B8FA8 File Offset: 0x001B71A8
		// (set) Token: 0x060052A0 RID: 21152 RVA: 0x001B8FE8 File Offset: 0x001B71E8
		internal static bool ThrowInsteadOfAssert
		{
			get
			{
				bool result;
				lock (Diagnostics.throwInsteadOfAssertLock)
				{
					result = Diagnostics.throwInsteadOfAssert;
				}
				return result;
			}
			set
			{
				lock (Diagnostics.throwInsteadOfAssertLock)
				{
					Diagnostics.throwInsteadOfAssert = value;
				}
			}
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x001B9028 File Offset: 0x001B7228
		private Diagnostics()
		{
		}

		// Token: 0x060052A2 RID: 21154 RVA: 0x001B9030 File Offset: 0x001B7230
		[Conditional("ASSERTIONS_TRACE")]
		[Conditional("DEBUG")]
		internal static void Assert(bool condition, string whyThisShouldNeverHappen)
		{
			Diagnostics.Assert(condition, whyThisShouldNeverHappen, string.Empty);
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x001B9040 File Offset: 0x001B7240
		[Conditional("ASSERTIONS_TRACE")]
		[Conditional("DEBUG")]
		internal static void Assert(bool condition, string whyThisShouldNeverHappen, string detailMessage)
		{
			if (condition)
			{
				return;
			}
			if (Diagnostics.ThrowInsteadOfAssert)
			{
				string message = string.Concat(new string[]
				{
					"ASSERT: ",
					whyThisShouldNeverHappen,
					"  ",
					detailMessage,
					" "
				});
				throw new AssertException(message);
			}
			Debug.Fail(whyThisShouldNeverHappen, detailMessage);
		}

		// Token: 0x04002A81 RID: 10881
		private static object throwInsteadOfAssertLock = 1;

		// Token: 0x04002A82 RID: 10882
		private static bool throwInsteadOfAssert = false;
	}
}
