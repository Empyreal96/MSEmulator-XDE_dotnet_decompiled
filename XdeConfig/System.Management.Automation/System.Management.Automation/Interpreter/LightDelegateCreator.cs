using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006EC RID: 1772
	internal sealed class LightDelegateCreator
	{
		// Token: 0x06004934 RID: 18740 RVA: 0x00182229 File Offset: 0x00180429
		internal LightDelegateCreator(Interpreter interpreter, LambdaExpression lambda)
		{
			this._interpreter = interpreter;
			this._lambda = lambda;
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06004935 RID: 18741 RVA: 0x0018224A File Offset: 0x0018044A
		internal Interpreter Interpreter
		{
			get
			{
				return this._interpreter;
			}
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06004936 RID: 18742 RVA: 0x00182252 File Offset: 0x00180452
		private bool HasClosure
		{
			get
			{
				return this._interpreter != null && this._interpreter.ClosureSize > 0;
			}
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06004937 RID: 18743 RVA: 0x0018226C File Offset: 0x0018046C
		internal bool HasCompiled
		{
			get
			{
				return this._compiled != null;
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06004938 RID: 18744 RVA: 0x0018227A File Offset: 0x0018047A
		internal bool SameDelegateType
		{
			get
			{
				return this._compiledDelegateType == this.DelegateType;
			}
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x0018228D File Offset: 0x0018048D
		public Delegate CreateDelegate()
		{
			return this.CreateDelegate(null);
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00182298 File Offset: 0x00180498
		internal Delegate CreateDelegate(StrongBox<object>[] closure)
		{
			if (this._compiled != null && this.SameDelegateType)
			{
				return this.CreateCompiledDelegate(closure);
			}
			if (this._interpreter == null)
			{
				this.Compile(null);
				return this.CreateCompiledDelegate(closure);
			}
			return new LightLambda(this, closure, this._interpreter._compilationThreshold).MakeDelegate(this.DelegateType);
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x0600493B RID: 18747 RVA: 0x001822F4 File Offset: 0x001804F4
		private Type DelegateType
		{
			get
			{
				LambdaExpression lambdaExpression = this._lambda as LambdaExpression;
				if (lambdaExpression != null)
				{
					return lambdaExpression.Type;
				}
				return null;
			}
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x00182318 File Offset: 0x00180518
		internal Delegate CreateCompiledDelegate(StrongBox<object>[] closure)
		{
			if (this.HasClosure)
			{
				Func<StrongBox<object>[], Delegate> func = (Func<StrongBox<object>[], Delegate>)this._compiled;
				return func(closure);
			}
			return this._compiled;
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x00182348 File Offset: 0x00180548
		internal void Compile(object state)
		{
			if (this._compiled != null)
			{
				return;
			}
			lock (this._compileLock)
			{
				if (this._compiled == null)
				{
					LambdaExpression lambdaExpression = this._lambda as LambdaExpression;
					if (this._interpreter != null)
					{
						this._compiledDelegateType = LightDelegateCreator.GetFuncOrAction(lambdaExpression);
						lambdaExpression = Expression.Lambda(this._compiledDelegateType, lambdaExpression.Body, lambdaExpression.Name, lambdaExpression.Parameters);
					}
					if (this.HasClosure)
					{
						this._compiled = LightLambdaClosureVisitor.BindLambda(lambdaExpression, this._interpreter.ClosureVariables);
					}
					else
					{
						this._compiled = lambdaExpression.Compile();
					}
				}
			}
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x0018241C File Offset: 0x0018061C
		private static Type GetFuncOrAction(LambdaExpression lambda)
		{
			bool flag = lambda.ReturnType == typeof(void);
			Type[] array = lambda.Parameters.Map(delegate(ParameterExpression p)
			{
				if (!p.IsByRef)
				{
					return p.Type;
				}
				return p.Type.MakeByRefType();
			});
			if (flag)
			{
				Type result;
				if (Expression.TryGetActionType(array, out result))
				{
					return result;
				}
			}
			else
			{
				array = array.AddLast(lambda.ReturnType);
				Type result;
				if (Expression.TryGetFuncType(array, out result))
				{
					return result;
				}
			}
			return lambda.Type;
		}

		// Token: 0x040023B4 RID: 9140
		private readonly Interpreter _interpreter;

		// Token: 0x040023B5 RID: 9141
		private readonly Expression _lambda;

		// Token: 0x040023B6 RID: 9142
		private Type _compiledDelegateType;

		// Token: 0x040023B7 RID: 9143
		private Delegate _compiled;

		// Token: 0x040023B8 RID: 9144
		private readonly object _compileLock = new object();
	}
}
