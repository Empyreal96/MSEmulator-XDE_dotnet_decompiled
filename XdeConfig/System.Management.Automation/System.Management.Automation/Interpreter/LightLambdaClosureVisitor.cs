using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006EF RID: 1775
	internal sealed class LightLambdaClosureVisitor : ExpressionVisitor
	{
		// Token: 0x0600498F RID: 18831 RVA: 0x00184A2A File Offset: 0x00182C2A
		private LightLambdaClosureVisitor(Dictionary<ParameterExpression, LocalVariable> closureVariables, ParameterExpression closureArray)
		{
			this._closureArray = closureArray;
			this._closureVars = closureVariables;
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x00184A4C File Offset: 0x00182C4C
		internal static Func<StrongBox<object>[], Delegate> BindLambda(LambdaExpression lambda, Dictionary<ParameterExpression, LocalVariable> closureVariables)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(StrongBox<object>[]), "closure");
			LightLambdaClosureVisitor lightLambdaClosureVisitor = new LightLambdaClosureVisitor(closureVariables, parameterExpression);
			lambda = (LambdaExpression)lightLambdaClosureVisitor.Visit(lambda);
			Expression<Func<StrongBox<object>[], Delegate>> expression = Expression.Lambda<Func<StrongBox<object>[], Delegate>>(lambda, new ParameterExpression[]
			{
				parameterExpression
			});
			return expression.Compile();
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x00184AA0 File Offset: 0x00182CA0
		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			this._shadowedVars.Push(new HashSet<ParameterExpression>(node.Parameters));
			Expression expression = this.Visit(node.Body);
			this._shadowedVars.Pop();
			if (expression == node.Body)
			{
				return node;
			}
			return Expression.Lambda<T>(expression, node.Name, node.TailCall, node.Parameters);
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x00184B00 File Offset: 0x00182D00
		protected override Expression VisitBlock(BlockExpression node)
		{
			if (node.Variables.Count > 0)
			{
				this._shadowedVars.Push(new HashSet<ParameterExpression>(node.Variables));
			}
			ReadOnlyCollection<Expression> readOnlyCollection = base.Visit(node.Expressions);
			if (node.Variables.Count > 0)
			{
				this._shadowedVars.Pop();
			}
			if (readOnlyCollection == node.Expressions)
			{
				return node;
			}
			return Expression.Block(node.Variables, readOnlyCollection);
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00184B70 File Offset: 0x00182D70
		protected override CatchBlock VisitCatchBlock(CatchBlock node)
		{
			if (node.Variable != null)
			{
				this._shadowedVars.Push(new HashSet<ParameterExpression>(new ParameterExpression[]
				{
					node.Variable
				}));
			}
			Expression expression = this.Visit(node.Body);
			Expression expression2 = this.Visit(node.Filter);
			if (node.Variable != null)
			{
				this._shadowedVars.Pop();
			}
			if (expression == node.Body && expression2 == node.Filter)
			{
				return node;
			}
			return Expression.MakeCatchBlock(node.Test, node.Variable, expression, expression2);
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00184BFC File Offset: 0x00182DFC
		protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
		{
			int count = node.Variables.Count;
			List<Expression> list = new List<Expression>();
			List<ParameterExpression> list2 = new List<ParameterExpression>();
			int[] array = new int[count];
			for (int i = 0; i < count; i++)
			{
				Expression closureItem = this.GetClosureItem(node.Variables[i], false);
				if (closureItem == null)
				{
					array[i] = list2.Count;
					list2.Add(node.Variables[i]);
				}
				else
				{
					array[i] = -1 - list.Count;
					list.Add(closureItem);
				}
			}
			if (list.Count == 0)
			{
				return node;
			}
			NewArrayExpression newArrayExpression = Expression.NewArrayInit(typeof(IStrongBox), list);
			if (list2.Count == 0)
			{
				return Expression.Invoke(Expression.Constant(new Func<IStrongBox[], IRuntimeVariables>(RuntimeVariables.Create)), new Expression[]
				{
					newArrayExpression
				});
			}
			Func<IRuntimeVariables, IRuntimeVariables, int[], IRuntimeVariables> value = new Func<IRuntimeVariables, IRuntimeVariables, int[], IRuntimeVariables>(LightLambdaClosureVisitor.MergedRuntimeVariables.Create);
			return Expression.Invoke(Utils.Constant(value), new Expression[]
			{
				Expression.RuntimeVariables(list2),
				newArrayExpression,
				Utils.Constant(array)
			});
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00184D10 File Offset: 0x00182F10
		protected override Expression VisitParameter(ParameterExpression node)
		{
			Expression closureItem = this.GetClosureItem(node, true);
			if (closureItem == null)
			{
				return node;
			}
			return Utils.Convert(closureItem, node.Type);
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00184D38 File Offset: 0x00182F38
		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (node.NodeType == ExpressionType.Assign && node.Left.NodeType == ExpressionType.Parameter)
			{
				ParameterExpression parameterExpression = (ParameterExpression)node.Left;
				Expression closureItem = this.GetClosureItem(parameterExpression, true);
				if (closureItem != null)
				{
					return Expression.Block(new ParameterExpression[]
					{
						parameterExpression
					}, new Expression[]
					{
						Expression.Assign(parameterExpression, this.Visit(node.Right)),
						Expression.Assign(closureItem, Utils.Convert(parameterExpression, typeof(object))),
						parameterExpression
					});
				}
			}
			return base.VisitBinary(node);
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00184DCC File Offset: 0x00182FCC
		private Expression GetClosureItem(ParameterExpression variable, bool unbox)
		{
			foreach (HashSet<ParameterExpression> hashSet in this._shadowedVars)
			{
				if (hashSet.Contains(variable))
				{
					return null;
				}
			}
			LocalVariable localVariable;
			if (!this._closureVars.TryGetValue(variable, out localVariable))
			{
				throw new InvalidOperationException("unbound variable: " + variable.Name);
			}
			Expression expression = localVariable.LoadFromArray(null, this._closureArray);
			if (!unbox)
			{
				return expression;
			}
			return LightCompiler.Unbox(expression);
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00184E68 File Offset: 0x00183068
		protected override Expression VisitExtension(Expression node)
		{
			return this.Visit(node.ReduceExtensions());
		}

		// Token: 0x040023C3 RID: 9155
		private readonly Dictionary<ParameterExpression, LocalVariable> _closureVars;

		// Token: 0x040023C4 RID: 9156
		private readonly ParameterExpression _closureArray;

		// Token: 0x040023C5 RID: 9157
		private readonly Stack<HashSet<ParameterExpression>> _shadowedVars = new Stack<HashSet<ParameterExpression>>();

		// Token: 0x020006F0 RID: 1776
		private sealed class MergedRuntimeVariables : IRuntimeVariables
		{
			// Token: 0x06004999 RID: 18841 RVA: 0x00184E76 File Offset: 0x00183076
			private MergedRuntimeVariables(IRuntimeVariables first, IRuntimeVariables second, int[] indexes)
			{
				this._first = first;
				this._second = second;
				this._indexes = indexes;
			}

			// Token: 0x0600499A RID: 18842 RVA: 0x00184E93 File Offset: 0x00183093
			internal static IRuntimeVariables Create(IRuntimeVariables first, IRuntimeVariables second, int[] indexes)
			{
				return new LightLambdaClosureVisitor.MergedRuntimeVariables(first, second, indexes);
			}

			// Token: 0x17000F75 RID: 3957
			// (get) Token: 0x0600499B RID: 18843 RVA: 0x00184E9D File Offset: 0x0018309D
			int IRuntimeVariables.Count
			{
				get
				{
					return this._indexes.Length;
				}
			}

			// Token: 0x17000F76 RID: 3958
			object IRuntimeVariables.this[int index]
			{
				get
				{
					index = this._indexes[index];
					if (index < 0)
					{
						return this._second[-1 - index];
					}
					return this._first[index];
				}
				set
				{
					index = this._indexes[index];
					if (index >= 0)
					{
						this._first[index] = value;
						return;
					}
					this._second[-1 - index] = value;
				}
			}

			// Token: 0x040023C6 RID: 9158
			private readonly IRuntimeVariables _first;

			// Token: 0x040023C7 RID: 9159
			private readonly IRuntimeVariables _second;

			// Token: 0x040023C8 RID: 9160
			private readonly int[] _indexes;
		}
	}
}
