using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200066C RID: 1644
	internal sealed class MethodInfoCallInstruction : CallInstruction
	{
		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x0600463A RID: 17978 RVA: 0x001787B8 File Offset: 0x001769B8
		public override MethodInfo Info
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x0600463B RID: 17979 RVA: 0x001787C0 File Offset: 0x001769C0
		public override int ArgumentCount
		{
			get
			{
				return this._argumentCount;
			}
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x001787C8 File Offset: 0x001769C8
		internal MethodInfoCallInstruction(MethodInfo target, int argumentCount)
		{
			this._target = target;
			this._argumentCount = argumentCount;
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x001787DE File Offset: 0x001769DE
		public override object Invoke(params object[] args)
		{
			return this.InvokeWorker(args);
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x001787E8 File Offset: 0x001769E8
		public override object InvokeInstance(object instance, params object[] args)
		{
			if (this._target.IsStatic)
			{
				try
				{
					return this._target.Invoke(null, args);
				}
				catch (TargetInvocationException ex)
				{
					throw ExceptionHelpers.UpdateForRethrow(ex.InnerException);
				}
			}
			object result;
			try
			{
				result = this._target.Invoke(instance, args);
			}
			catch (TargetInvocationException ex2)
			{
				throw ExceptionHelpers.UpdateForRethrow(ex2.InnerException);
			}
			return result;
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x0017885C File Offset: 0x00176A5C
		private object InvokeWorker(params object[] args)
		{
			if (this._target.IsStatic)
			{
				try
				{
					return this._target.Invoke(null, args);
				}
				catch (TargetInvocationException ex)
				{
					throw ExceptionHelpers.UpdateForRethrow(ex.InnerException);
				}
			}
			object result;
			try
			{
				result = this._target.Invoke(args[0], MethodInfoCallInstruction.GetNonStaticArgs(args));
			}
			catch (TargetInvocationException ex2)
			{
				throw ExceptionHelpers.UpdateForRethrow(ex2.InnerException);
			}
			return result;
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x001788D4 File Offset: 0x00176AD4
		private static object[] GetNonStaticArgs(object[] args)
		{
			object[] array = new object[args.Length - 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = args[i + 1];
			}
			return array;
		}

		// Token: 0x06004641 RID: 17985 RVA: 0x00178904 File Offset: 0x00176B04
		public sealed override int Run(InterpretedFrame frame)
		{
			int num = frame.StackIndex - this._argumentCount;
			object[] array = new object[this._argumentCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = frame.Data[num + i];
			}
			object obj = this.Invoke(array);
			if (this._target.ReturnType != typeof(void))
			{
				frame.Data[num] = obj;
				frame.StackIndex = num + 1;
			}
			else
			{
				frame.StackIndex = num;
			}
			return 1;
		}

		// Token: 0x06004642 RID: 17986 RVA: 0x00178986 File Offset: 0x00176B86
		public override object Invoke()
		{
			return this.InvokeWorker(new object[0]);
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x00178994 File Offset: 0x00176B94
		public override object Invoke(object arg0)
		{
			return this.InvokeWorker(new object[]
			{
				arg0
			});
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x001789B4 File Offset: 0x00176BB4
		public override object Invoke(object arg0, object arg1)
		{
			return this.InvokeWorker(new object[]
			{
				arg0,
				arg1
			});
		}

		// Token: 0x040022A6 RID: 8870
		private readonly MethodInfo _target;

		// Token: 0x040022A7 RID: 8871
		private readonly int _argumentCount;
	}
}
