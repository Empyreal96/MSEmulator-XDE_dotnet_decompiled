using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006EE RID: 1774
	internal class LightLambda
	{
		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06004943 RID: 18755 RVA: 0x001824B8 File Offset: 0x001806B8
		// (remove) Token: 0x06004944 RID: 18756 RVA: 0x001824F0 File Offset: 0x001806F0
		public event EventHandler<LightLambdaCompileEventArgs> Compile;

		// Token: 0x06004945 RID: 18757 RVA: 0x00182525 File Offset: 0x00180725
		internal LightLambda(LightDelegateCreator delegateCreator, StrongBox<object>[] closure, int compilationThreshold)
		{
			this._delegateCreator = delegateCreator;
			this._closure = closure;
			this._interpreter = delegateCreator.Interpreter;
			this._compilationThreshold = compilationThreshold;
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x00182550 File Offset: 0x00180750
		private static Func<LightLambda, Delegate> GetRunDelegateCtor(Type delegateType)
		{
			Func<LightLambda, Delegate> result;
			lock (LightLambda._runCache)
			{
				Func<LightLambda, Delegate> func;
				if (LightLambda._runCache.TryGetValue(delegateType, out func))
				{
					result = func;
				}
				else
				{
					result = LightLambda.MakeRunDelegateCtor(delegateType);
				}
			}
			return result;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x001825C0 File Offset: 0x001807C0
		private static Func<LightLambda, Delegate> MakeRunDelegateCtor(Type delegateType)
		{
			MethodInfo method = delegateType.GetMethod("Invoke");
			ParameterInfo[] parameters = method.GetParameters();
			string text = "Run";
			if (parameters.Length >= 16)
			{
				return null;
			}
			Type[] array;
			if (method.ReturnType == typeof(void))
			{
				text += "Void";
				array = new Type[parameters.Length];
			}
			else
			{
				array = new Type[parameters.Length + 1];
				array[array.Length - 1] = method.ReturnType;
			}
			MethodInfo method2;
			if (method.ReturnType == typeof(void) && array.Length == 2 && parameters[0].ParameterType.IsByRef && parameters[1].ParameterType.IsByRef)
			{
				method2 = typeof(LightLambda).GetMethod("RunVoidRef2", BindingFlags.Instance | BindingFlags.NonPublic);
				array[0] = parameters[0].ParameterType.GetElementType();
				array[1] = parameters[1].ParameterType.GetElementType();
			}
			else if (method.ReturnType == typeof(void) && array.Length == 0)
			{
				method2 = typeof(LightLambda).GetMethod("RunVoid0", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			else
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i] = parameters[i].ParameterType;
					if (array[i].IsByRef)
					{
						return null;
					}
				}
				if (DelegateHelpers.MakeDelegate(array) == delegateType)
				{
					text = "Make" + text + parameters.Length;
					MethodInfo methodInfo = typeof(LightLambda).GetMethod(text, BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(array);
					return LightLambda._runCache[delegateType] = (Func<LightLambda, Delegate>)methodInfo.CreateDelegate(typeof(Func<LightLambda, Delegate>));
				}
				method2 = typeof(LightLambda).GetMethod(text + parameters.Length, BindingFlags.Instance | BindingFlags.NonPublic);
			}
			try
			{
				DynamicMethod dynamicMethod = new DynamicMethod("FastCtor", typeof(Delegate), new Type[]
				{
					typeof(LightLambda)
				}, typeof(LightLambda), true);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldftn, method2.IsGenericMethodDefinition ? method2.MakeGenericMethod(array) : method2);
				ilgenerator.Emit(OpCodes.Newobj, delegateType.GetConstructor(new Type[]
				{
					typeof(object),
					typeof(IntPtr)
				}));
				ilgenerator.Emit(OpCodes.Ret);
				return LightLambda._runCache[delegateType] = (Func<LightLambda, Delegate>)dynamicMethod.CreateDelegate(typeof(Func<LightLambda, Delegate>));
			}
			catch (SecurityException)
			{
			}
			MethodInfo targetMethod = method2.IsGenericMethodDefinition ? method2.MakeGenericMethod(array) : method2;
			return LightLambda._runCache[delegateType] = ((LightLambda lambda) => targetMethod.CreateDelegate(delegateType, lambda));
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x001828F8 File Offset: 0x00180AF8
		private Delegate CreateCustomDelegate(Type delegateType)
		{
			MethodInfo method = delegateType.GetMethod("Invoke");
			ParameterInfo[] parameters = method.GetParameters();
			ParameterExpression[] array = new ParameterExpression[parameters.Length];
			Expression[] array2 = new Expression[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterExpression parameterExpression = Expression.Parameter(parameters[i].ParameterType, parameters[i].Name);
				array[i] = parameterExpression;
				array2[i] = Expression.Convert(parameterExpression, typeof(object));
			}
			NewArrayExpression newArrayExpression = Expression.NewArrayInit(typeof(object), array2);
			Expression instance = Utils.Constant(this);
			MethodInfo method2 = typeof(LightLambda).GetMethod("Run");
			UnaryExpression body = Expression.Convert(Expression.Call(instance, method2, new Expression[]
			{
				newArrayExpression
			}), method.ReturnType);
			LambdaExpression lambdaExpression = Expression.Lambda(delegateType, body, array);
			return lambdaExpression.Compile();
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x001829D8 File Offset: 0x00180BD8
		internal Delegate MakeDelegate(Type delegateType)
		{
			Func<LightLambda, Delegate> runDelegateCtor = LightLambda.GetRunDelegateCtor(delegateType);
			if (runDelegateCtor != null)
			{
				return runDelegateCtor(this);
			}
			return this.CreateCustomDelegate(delegateType);
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x00182A00 File Offset: 0x00180C00
		private bool TryGetCompiled()
		{
			if (this._delegateCreator.HasCompiled)
			{
				this._compiled = this._delegateCreator.CreateCompiledDelegate(this._closure);
				EventHandler<LightLambdaCompileEventArgs> compile = this.Compile;
				if (compile != null && this._delegateCreator.SameDelegateType)
				{
					compile(this, new LightLambdaCompileEventArgs(this._compiled));
				}
				return true;
			}
			if (this._compilationThreshold-- == 0)
			{
				if (this._interpreter.CompileSynchronously)
				{
					this._delegateCreator.Compile(null);
					return this.TryGetCompiled();
				}
				ThreadPool.QueueUserWorkItem(new WaitCallback(this._delegateCreator.Compile), null);
			}
			return false;
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x00182AA8 File Offset: 0x00180CA8
		private InterpretedFrame MakeFrame()
		{
			return new InterpretedFrame(this._interpreter, this._closure);
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x00182ABC File Offset: 0x00180CBC
		internal void RunVoidRef2<T0, T1>(ref T0 arg0, ref T1 arg1)
		{
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
				arg0 = (T0)((object)interpretedFrame.Data[0]);
				arg1 = (T1)((object)interpretedFrame.Data[1]);
			}
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x00182B4C File Offset: 0x00180D4C
		public object Run(params object[] arguments)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return this._compiled.DynamicInvoke(arguments);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			for (int i = 0; i < arguments.Length; i++)
			{
				interpretedFrame.Data[i] = arguments[i];
			}
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return interpretedFrame.Pop();
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x00182BCC File Offset: 0x00180DCC
		internal TRet Run0<TRet>()
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<TRet>)this._compiled)();
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x00182C3C File Offset: 0x00180E3C
		internal void RunVoid0()
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action)this._compiled)();
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x00182CA0 File Offset: 0x00180EA0
		internal static Delegate MakeRun0<TRet>(LightLambda lambda)
		{
			return new Func<TRet>(lambda.Run0<TRet>);
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x00182CAE File Offset: 0x00180EAE
		internal static Delegate MakeRunVoid0(LightLambda lambda)
		{
			return new Action(lambda.RunVoid0);
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x00182CBC File Offset: 0x00180EBC
		internal TRet Run1<T0, TRet>(T0 arg0)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, TRet>)this._compiled)(arg0);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x00182D38 File Offset: 0x00180F38
		internal void RunVoid1<T0>(T0 arg0)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0>)this._compiled)(arg0);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x00182DAC File Offset: 0x00180FAC
		internal static Delegate MakeRun1<T0, TRet>(LightLambda lambda)
		{
			return new Func<T0, TRet>(lambda.Run1<T0, TRet>);
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x00182DBA File Offset: 0x00180FBA
		internal static Delegate MakeRunVoid1<T0>(LightLambda lambda)
		{
			return new Action<T0>(lambda.RunVoid1<T0>);
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x00182DC8 File Offset: 0x00180FC8
		internal TRet Run2<T0, T1, TRet>(T0 arg0, T1 arg1)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, TRet>)this._compiled)(arg0, arg1);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x00182E54 File Offset: 0x00181054
		internal void RunVoid2<T0, T1>(T0 arg0, T1 arg1)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1>)this._compiled)(arg0, arg1);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x00182ED4 File Offset: 0x001810D4
		internal static Delegate MakeRun2<T0, T1, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, TRet>(lambda.Run2<T0, T1, TRet>);
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x00182EE2 File Offset: 0x001810E2
		internal static Delegate MakeRunVoid2<T0, T1>(LightLambda lambda)
		{
			return new Action<T0, T1>(lambda.RunVoid2<T0, T1>);
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x00182EF0 File Offset: 0x001810F0
		internal TRet Run3<T0, T1, T2, TRet>(T0 arg0, T1 arg1, T2 arg2)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, TRet>)this._compiled)(arg0, arg1, arg2);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x00182F8C File Offset: 0x0018118C
		internal void RunVoid3<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2>)this._compiled)(arg0, arg1, arg2);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x0018301C File Offset: 0x0018121C
		internal static Delegate MakeRun3<T0, T1, T2, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, TRet>(lambda.Run3<T0, T1, T2, TRet>);
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x0018302A File Offset: 0x0018122A
		internal static Delegate MakeRunVoid3<T0, T1, T2>(LightLambda lambda)
		{
			return new Action<T0, T1, T2>(lambda.RunVoid3<T0, T1, T2>);
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x00183038 File Offset: 0x00181238
		internal TRet Run4<T0, T1, T2, T3, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, TRet>)this._compiled)(arg0, arg1, arg2, arg3);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x001830E4 File Offset: 0x001812E4
		internal void RunVoid4<T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3>)this._compiled)(arg0, arg1, arg2, arg3);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x00183184 File Offset: 0x00181384
		internal static Delegate MakeRun4<T0, T1, T2, T3, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, TRet>(lambda.Run4<T0, T1, T2, T3, TRet>);
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x00183192 File Offset: 0x00181392
		internal static Delegate MakeRunVoid4<T0, T1, T2, T3>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3>(lambda.RunVoid4<T0, T1, T2, T3>);
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x001831A0 File Offset: 0x001813A0
		internal TRet Run5<T0, T1, T2, T3, T4, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x0018325C File Offset: 0x0018145C
		internal void RunVoid5<T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4>)this._compiled)(arg0, arg1, arg2, arg3, arg4);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x00183310 File Offset: 0x00181510
		internal static Delegate MakeRun5<T0, T1, T2, T3, T4, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, TRet>(lambda.Run5<T0, T1, T2, T3, T4, TRet>);
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x0018331E File Offset: 0x0018151E
		internal static Delegate MakeRunVoid5<T0, T1, T2, T3, T4>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4>(lambda.RunVoid5<T0, T1, T2, T3, T4>);
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x0018332C File Offset: 0x0018152C
		internal TRet Run6<T0, T1, T2, T3, T4, T5, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x001833FC File Offset: 0x001815FC
		internal void RunVoid6<T0, T1, T2, T3, T4, T5>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x001834C0 File Offset: 0x001816C0
		internal static Delegate MakeRun6<T0, T1, T2, T3, T4, T5, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, TRet>(lambda.Run6<T0, T1, T2, T3, T4, T5, TRet>);
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x001834CE File Offset: 0x001816CE
		internal static Delegate MakeRunVoid6<T0, T1, T2, T3, T4, T5>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5>(lambda.RunVoid6<T0, T1, T2, T3, T4, T5>);
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x001834DC File Offset: 0x001816DC
		internal TRet Run7<T0, T1, T2, T3, T4, T5, T6, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, T6, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x001835BC File Offset: 0x001817BC
		internal void RunVoid7<T0, T1, T2, T3, T4, T5, T6>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5, T6>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x00183690 File Offset: 0x00181890
		internal static Delegate MakeRun7<T0, T1, T2, T3, T4, T5, T6, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, T6, TRet>(lambda.Run7<T0, T1, T2, T3, T4, T5, T6, TRet>);
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x0018369E File Offset: 0x0018189E
		internal static Delegate MakeRunVoid7<T0, T1, T2, T3, T4, T5, T6>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5, T6>(lambda.RunVoid7<T0, T1, T2, T3, T4, T5, T6>);
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x001836AC File Offset: 0x001818AC
		internal TRet Run8<T0, T1, T2, T3, T4, T5, T6, T7, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, T6, T7, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x0018379C File Offset: 0x0018199C
		internal void RunVoid8<T0, T1, T2, T3, T4, T5, T6, T7>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5, T6, T7>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x00183880 File Offset: 0x00181A80
		internal static Delegate MakeRun8<T0, T1, T2, T3, T4, T5, T6, T7, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, T6, T7, TRet>(lambda.Run8<T0, T1, T2, T3, T4, T5, T6, T7, TRet>);
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x0018388E File Offset: 0x00181A8E
		internal static Delegate MakeRunVoid8<T0, T1, T2, T3, T4, T5, T6, T7>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5, T6, T7>(lambda.RunVoid8<T0, T1, T2, T3, T4, T5, T6, T7>);
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x0018389C File Offset: 0x00181A9C
		internal TRet Run9<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x0018399C File Offset: 0x00181B9C
		internal void RunVoid9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5, T6, T7, T8>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x00183A94 File Offset: 0x00181C94
		internal static Delegate MakeRun9<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(lambda.Run9<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>);
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x00183AA2 File Offset: 0x00181CA2
		internal static Delegate MakeRunVoid9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5, T6, T7, T8>(lambda.RunVoid9<T0, T1, T2, T3, T4, T5, T6, T7, T8>);
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x00183AB0 File Offset: 0x00181CB0
		internal TRet Run10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x00183BC4 File Offset: 0x00181DC4
		internal void RunVoid10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x00183CCC File Offset: 0x00181ECC
		internal static Delegate MakeRun10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(lambda.Run10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>);
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x00183CDA File Offset: 0x00181EDA
		internal static Delegate MakeRunVoid10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(lambda.RunVoid10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>);
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00183CE8 File Offset: 0x00181EE8
		internal TRet Run11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x00183E0C File Offset: 0x0018200C
		internal void RunVoid11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00183F28 File Offset: 0x00182128
		internal static Delegate MakeRun11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(lambda.Run11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>);
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00183F36 File Offset: 0x00182136
		internal static Delegate MakeRunVoid11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(lambda.RunVoid11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>);
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00183F44 File Offset: 0x00182144
		internal TRet Run12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			interpretedFrame.Data[11] = arg11;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x0018407C File Offset: 0x0018227C
		internal void RunVoid12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			interpretedFrame.Data[11] = arg11;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x001841A8 File Offset: 0x001823A8
		internal static Delegate MakeRun12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>(lambda.Run12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>);
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x001841B6 File Offset: 0x001823B6
		internal static Delegate MakeRunVoid12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(lambda.RunVoid12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>);
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x001841C4 File Offset: 0x001823C4
		internal TRet Run13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			interpretedFrame.Data[11] = arg11;
			interpretedFrame.Data[12] = arg12;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x0018430C File Offset: 0x0018250C
		internal void RunVoid13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			interpretedFrame.Data[11] = arg11;
			interpretedFrame.Data[12] = arg12;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x0018444C File Offset: 0x0018264C
		internal static Delegate MakeRun13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>(lambda.Run13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>);
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x0018445A File Offset: 0x0018265A
		internal static Delegate MakeRunVoid13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(lambda.RunVoid13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>);
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x00184468 File Offset: 0x00182668
		internal TRet Run14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			interpretedFrame.Data[11] = arg11;
			interpretedFrame.Data[12] = arg12;
			interpretedFrame.Data[13] = arg13;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x001845C4 File Offset: 0x001827C4
		internal void RunVoid14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			interpretedFrame.Data[11] = arg11;
			interpretedFrame.Data[12] = arg12;
			interpretedFrame.Data[13] = arg13;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x00184714 File Offset: 0x00182914
		internal static Delegate MakeRun14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>(lambda.Run14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>);
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x00184722 File Offset: 0x00182922
		internal static Delegate MakeRunVoid14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(lambda.RunVoid14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>);
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x00184730 File Offset: 0x00182930
		internal TRet Run15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				return ((Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			interpretedFrame.Data[11] = arg11;
			interpretedFrame.Data[12] = arg12;
			interpretedFrame.Data[13] = arg13;
			interpretedFrame.Data[14] = arg14;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
			return (TRet)((object)interpretedFrame.Pop());
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x0018489C File Offset: 0x00182A9C
		internal void RunVoid15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
		{
			if (this._compiled != null || this.TryGetCompiled())
			{
				((Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)this._compiled)(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
				return;
			}
			InterpretedFrame interpretedFrame = this.MakeFrame();
			interpretedFrame.Data[0] = arg0;
			interpretedFrame.Data[1] = arg1;
			interpretedFrame.Data[2] = arg2;
			interpretedFrame.Data[3] = arg3;
			interpretedFrame.Data[4] = arg4;
			interpretedFrame.Data[5] = arg5;
			interpretedFrame.Data[6] = arg6;
			interpretedFrame.Data[7] = arg7;
			interpretedFrame.Data[8] = arg8;
			interpretedFrame.Data[9] = arg9;
			interpretedFrame.Data[10] = arg10;
			interpretedFrame.Data[11] = arg11;
			interpretedFrame.Data[12] = arg12;
			interpretedFrame.Data[13] = arg13;
			interpretedFrame.Data[14] = arg14;
			ThreadLocal<InterpretedFrame>.StorageInfo currentFrame = interpretedFrame.Enter();
			try
			{
				this._interpreter.Run(interpretedFrame);
			}
			finally
			{
				interpretedFrame.Leave(currentFrame);
			}
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x00184A00 File Offset: 0x00182C00
		internal static Delegate MakeRun15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>(LightLambda lambda)
		{
			return new Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>(lambda.Run15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>);
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00184A0E File Offset: 0x00182C0E
		internal static Delegate MakeRunVoid15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(LightLambda lambda)
		{
			return new Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(lambda.RunVoid15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>);
		}

		// Token: 0x040023BB RID: 9147
		internal const int MaxParameters = 16;

		// Token: 0x040023BC RID: 9148
		private readonly StrongBox<object>[] _closure;

		// Token: 0x040023BD RID: 9149
		private readonly Interpreter _interpreter;

		// Token: 0x040023BE RID: 9150
		private static readonly CacheDict<Type, Func<LightLambda, Delegate>> _runCache = new CacheDict<Type, Func<LightLambda, Delegate>>(100);

		// Token: 0x040023BF RID: 9151
		private readonly LightDelegateCreator _delegateCreator;

		// Token: 0x040023C0 RID: 9152
		private Delegate _compiled;

		// Token: 0x040023C1 RID: 9153
		private int _compilationThreshold;
	}
}
