using System;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x0200006B RID: 107
	internal sealed class Disposables
	{
		// Token: 0x0600039A RID: 922 RVA: 0x0000CF9F File Offset: 0x0000B19F
		internal static IDisposable Create<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			return new Disposables.Disposable<T1, T2>(action, arg1, arg2);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000CFA9 File Offset: 0x0000B1A9
		internal static IDisposable Create<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			return new Disposables.Disposable<T1, T2, T3>(action, arg1, arg2, arg3);
		}

		// Token: 0x04000164 RID: 356
		internal static readonly IDisposable Nop = new Disposables.NopDisposable();

		// Token: 0x0200006C RID: 108
		[DebuggerDisplay("Disposed = true")]
		private sealed class NopDisposable : IDisposable
		{
			// Token: 0x0600039D RID: 925 RVA: 0x00002927 File Offset: 0x00000B27
			void IDisposable.Dispose()
			{
			}
		}

		// Token: 0x0200006D RID: 109
		[DebuggerDisplay("Disposed = {Disposed}")]
		private sealed class Disposable<T1, T2> : IDisposable
		{
			// Token: 0x0600039F RID: 927 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
			internal Disposable(Action<T1, T2> action, T1 arg1, T2 arg2)
			{
				this._action = action;
				this._arg1 = arg1;
				this._arg2 = arg2;
			}

			// Token: 0x17000147 RID: 327
			// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000CFDD File Offset: 0x0000B1DD
			private bool Disposed
			{
				get
				{
					return this._action == null;
				}
			}

			// Token: 0x060003A1 RID: 929 RVA: 0x0000CFE8 File Offset: 0x0000B1E8
			void IDisposable.Dispose()
			{
				Action<T1, T2> action = this._action;
				if (action != null && Interlocked.CompareExchange<Action<T1, T2>>(ref this._action, null, action) == action)
				{
					action(this._arg1, this._arg2);
				}
			}

			// Token: 0x04000165 RID: 357
			private readonly T1 _arg1;

			// Token: 0x04000166 RID: 358
			private readonly T2 _arg2;

			// Token: 0x04000167 RID: 359
			private Action<T1, T2> _action;
		}

		// Token: 0x0200006E RID: 110
		[DebuggerDisplay("Disposed = {Disposed}")]
		private sealed class Disposable<T1, T2, T3> : IDisposable
		{
			// Token: 0x060003A2 RID: 930 RVA: 0x0000D026 File Offset: 0x0000B226
			internal Disposable(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
			{
				this._action = action;
				this._arg1 = arg1;
				this._arg2 = arg2;
				this._arg3 = arg3;
			}

			// Token: 0x17000148 RID: 328
			// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000D04B File Offset: 0x0000B24B
			private bool Disposed
			{
				get
				{
					return this._action == null;
				}
			}

			// Token: 0x060003A4 RID: 932 RVA: 0x0000D058 File Offset: 0x0000B258
			void IDisposable.Dispose()
			{
				Action<T1, T2, T3> action = this._action;
				if (action != null && Interlocked.CompareExchange<Action<T1, T2, T3>>(ref this._action, null, action) == action)
				{
					action(this._arg1, this._arg2, this._arg3);
				}
			}

			// Token: 0x04000168 RID: 360
			private readonly T1 _arg1;

			// Token: 0x04000169 RID: 361
			private readonly T2 _arg2;

			// Token: 0x0400016A RID: 362
			private readonly T3 _arg3;

			// Token: 0x0400016B RID: 363
			private Action<T1, T2, T3> _action;
		}
	}
}
