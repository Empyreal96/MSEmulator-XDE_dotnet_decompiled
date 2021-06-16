using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Management.Automation.Language;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000709 RID: 1801
	internal sealed class LoopCompiler : ExpressionVisitor
	{
		// Token: 0x060049FC RID: 18940 RVA: 0x00185990 File Offset: 0x00183B90
		internal LoopCompiler(PowerShellLoopExpression loop, HybridReferenceDictionary<LabelTarget, BranchLabel> labelMapping, Dictionary<ParameterExpression, LocalVariable> locals, Dictionary<ParameterExpression, LocalVariable> closureVariables, int loopStartInstructionIndex, int loopEndInstructionIndex)
		{
			this._loop = loop;
			this._outerVariables = locals;
			this._closureVariables = closureVariables;
			this._frameDataVar = Expression.Parameter(typeof(object[]));
			this._frameClosureVar = Expression.Parameter(typeof(StrongBox<object>[]));
			this._frameVar = Expression.Parameter(typeof(InterpretedFrame));
			this._loopVariables = new Dictionary<ParameterExpression, LoopCompiler.LoopVariable>();
			this._returnLabel = Expression.Label(typeof(int));
			this._labelMapping = labelMapping;
			this._loopStartInstructionIndex = loopStartInstructionIndex;
			this._loopEndInstructionIndex = loopEndInstructionIndex;
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x00185A30 File Offset: 0x00183C30
		internal Func<object[], StrongBox<object>[], InterpretedFrame, int> CreateDelegate()
		{
			Expression expression = this.Visit(this._loop);
			List<Expression> list = new List<Expression>();
			List<Expression> list2 = new List<Expression>();
			foreach (KeyValuePair<ParameterExpression, LoopCompiler.LoopVariable> keyValuePair in this._loopVariables)
			{
				LocalVariable localVariable;
				if (!this._outerVariables.TryGetValue(keyValuePair.Key, out localVariable))
				{
					localVariable = this._closureVariables[keyValuePair.Key];
				}
				Expression expression2 = localVariable.LoadFromArray(this._frameDataVar, this._frameClosureVar);
				if (localVariable.InClosureOrBoxed)
				{
					ParameterExpression boxStorage = keyValuePair.Value.BoxStorage;
					list.Add(Expression.Assign(boxStorage, expression2));
					this.AddTemp(boxStorage);
				}
				else
				{
					list.Add(Expression.Assign(keyValuePair.Key, Utils.Convert(expression2, keyValuePair.Key.Type)));
					if ((keyValuePair.Value.Access & ExpressionAccess.Write) != ExpressionAccess.None)
					{
						list2.Add(Expression.Assign(expression2, Utils.Box(keyValuePair.Key)));
					}
					this.AddTemp(keyValuePair.Key);
				}
			}
			if (list2.Count > 0)
			{
				list.Add(Expression.TryFinally(expression, Expression.Block(list2)));
			}
			else
			{
				list.Add(expression);
			}
			list.Add(Expression.Label(this._returnLabel, Expression.Constant(this._loopEndInstructionIndex - this._loopStartInstructionIndex)));
			Expression<Func<object[], StrongBox<object>[], InterpretedFrame, int>> expression3 = Expression.Lambda<Func<object[], StrongBox<object>[], InterpretedFrame, int>>((this._temps != null) ? Expression.Block(this._temps, list) : Expression.Block(list), new ParameterExpression[]
			{
				this._frameDataVar,
				this._frameClosureVar,
				this._frameVar
			});
			return expression3.Compile();
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x00185C08 File Offset: 0x00183E08
		protected override Expression VisitExtension(Expression node)
		{
			if (node.CanReduce)
			{
				return this.Visit(node.Reduce());
			}
			return base.VisitExtension(node);
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x00185C28 File Offset: 0x00183E28
		protected override Expression VisitGoto(GotoExpression node)
		{
			LabelTarget target = node.Target;
			Expression expression = this.Visit(node.Value);
			BranchLabel branchLabel;
			if (!this._labelMapping.TryGetValue(target, out branchLabel))
			{
				return node.Update(target, expression);
			}
			if (branchLabel.TargetIndex >= this._loopStartInstructionIndex && branchLabel.TargetIndex < this._loopEndInstructionIndex)
			{
				return node.Update(target, expression);
			}
			return Expression.Return(this._returnLabel, (expression != null && expression.Type != typeof(void)) ? Expression.Call(this._frameVar, InterpretedFrame.GotoMethod, Expression.Constant(branchLabel.LabelIndex), Utils.Box(expression)) : Expression.Call(this._frameVar, InterpretedFrame.VoidGotoMethod, new Expression[]
			{
				Expression.Constant(branchLabel.LabelIndex)
			}), node.Type);
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x00185D08 File Offset: 0x00183F08
		protected override Expression VisitBlock(BlockExpression node)
		{
			ReadOnlyCollection<ParameterExpression> variables = node.Variables;
			HashSet<ParameterExpression> prevLocals = this.EnterVariableScope(variables);
			Expression result = base.VisitBlock(node);
			this.ExitVariableScope(prevLocals);
			return result;
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x00185D34 File Offset: 0x00183F34
		private HashSet<ParameterExpression> EnterVariableScope(ICollection<ParameterExpression> variables)
		{
			if (this._loopLocals == null)
			{
				this._loopLocals = new HashSet<ParameterExpression>(variables);
				return null;
			}
			HashSet<ParameterExpression> result = new HashSet<ParameterExpression>(this._loopLocals);
			this._loopLocals.UnionWith(variables);
			return result;
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x00185D70 File Offset: 0x00183F70
		protected override CatchBlock VisitCatchBlock(CatchBlock node)
		{
			if (node.Variable != null)
			{
				HashSet<ParameterExpression> prevLocals = this.EnterVariableScope(new ParameterExpression[]
				{
					node.Variable
				});
				CatchBlock result = base.VisitCatchBlock(node);
				this.ExitVariableScope(prevLocals);
				return result;
			}
			return base.VisitCatchBlock(node);
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x00185DB8 File Offset: 0x00183FB8
		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			HashSet<ParameterExpression> prevLocals = this.EnterVariableScope(node.Parameters);
			Expression result;
			try
			{
				result = base.VisitLambda<T>(node);
			}
			finally
			{
				this.ExitVariableScope(prevLocals);
			}
			return result;
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x00185DF8 File Offset: 0x00183FF8
		private void ExitVariableScope(HashSet<ParameterExpression> prevLocals)
		{
			this._loopLocals = prevLocals;
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x00185E04 File Offset: 0x00184004
		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (node.CanReduce)
			{
				return this.Visit(node.Reduce());
			}
			ParameterExpression parameterExpression = node.Left as ParameterExpression;
			if (parameterExpression == null || node.NodeType != ExpressionType.Assign)
			{
				return base.VisitBinary(node);
			}
			Expression expression = this.VisitVariable(parameterExpression, ExpressionAccess.Write);
			Expression expression2 = this.Visit(node.Right);
			if (expression.Type != parameterExpression.Type)
			{
				Expression expression3;
				if (expression2.NodeType != ExpressionType.Parameter)
				{
					expression3 = this.AddTemp(Expression.Parameter(expression2.Type));
					expression2 = Expression.Assign(expression3, expression2);
				}
				else
				{
					expression3 = expression2;
				}
				return Expression.Block(node.Update(expression, null, Expression.Convert(expression2, expression.Type)), expression3);
			}
			return node.Update(expression, null, expression2);
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x00185EBF File Offset: 0x001840BF
		protected override Expression VisitUnary(UnaryExpression node)
		{
			if (node.CanReduce)
			{
				return this.Visit(node.Reduce());
			}
			return base.VisitUnary(node);
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x00185EDD File Offset: 0x001840DD
		protected override Expression VisitParameter(ParameterExpression node)
		{
			return this.VisitVariable(node, ExpressionAccess.Read);
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x00185EE8 File Offset: 0x001840E8
		private Expression VisitVariable(ParameterExpression node, ExpressionAccess access)
		{
			if (this._loopLocals.Contains(node))
			{
				return node;
			}
			LoopCompiler.LoopVariable loopVariable;
			ParameterExpression parameterExpression;
			if (this._loopVariables.TryGetValue(node, out loopVariable))
			{
				parameterExpression = loopVariable.BoxStorage;
				this._loopVariables[node] = new LoopCompiler.LoopVariable(loopVariable.Access | access, parameterExpression);
			}
			else
			{
				LocalVariable localVariable;
				if (!this._outerVariables.TryGetValue(node, out localVariable) && (this._closureVariables == null || !this._closureVariables.TryGetValue(node, out localVariable)))
				{
					return node;
				}
				parameterExpression = (localVariable.InClosureOrBoxed ? Expression.Parameter(typeof(StrongBox<object>), node.Name) : null);
				this._loopVariables[node] = new LoopCompiler.LoopVariable(access, parameterExpression);
			}
			if (parameterExpression == null)
			{
				return node;
			}
			if ((access & ExpressionAccess.Write) != ExpressionAccess.None)
			{
				return LightCompiler.Unbox(parameterExpression);
			}
			return Expression.Convert(LightCompiler.Unbox(parameterExpression), node.Type);
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x00185FBC File Offset: 0x001841BC
		private ParameterExpression AddTemp(ParameterExpression variable)
		{
			if (this._temps == null)
			{
				this._temps = new List<ParameterExpression>();
			}
			this._temps.Add(variable);
			return variable;
		}

		// Token: 0x040023DE RID: 9182
		private readonly ParameterExpression _frameDataVar;

		// Token: 0x040023DF RID: 9183
		private readonly ParameterExpression _frameClosureVar;

		// Token: 0x040023E0 RID: 9184
		private readonly ParameterExpression _frameVar;

		// Token: 0x040023E1 RID: 9185
		private readonly LabelTarget _returnLabel;

		// Token: 0x040023E2 RID: 9186
		private readonly Dictionary<ParameterExpression, LocalVariable> _outerVariables;

		// Token: 0x040023E3 RID: 9187
		private readonly Dictionary<ParameterExpression, LocalVariable> _closureVariables;

		// Token: 0x040023E4 RID: 9188
		private readonly PowerShellLoopExpression _loop;

		// Token: 0x040023E5 RID: 9189
		private List<ParameterExpression> _temps;

		// Token: 0x040023E6 RID: 9190
		private readonly Dictionary<ParameterExpression, LoopCompiler.LoopVariable> _loopVariables;

		// Token: 0x040023E7 RID: 9191
		private HashSet<ParameterExpression> _loopLocals;

		// Token: 0x040023E8 RID: 9192
		private readonly HybridReferenceDictionary<LabelTarget, BranchLabel> _labelMapping;

		// Token: 0x040023E9 RID: 9193
		private readonly int _loopStartInstructionIndex;

		// Token: 0x040023EA RID: 9194
		private readonly int _loopEndInstructionIndex;

		// Token: 0x0200070A RID: 1802
		private struct LoopVariable
		{
			// Token: 0x06004A0A RID: 18954 RVA: 0x00185FDE File Offset: 0x001841DE
			public LoopVariable(ExpressionAccess access, ParameterExpression box)
			{
				this.Access = access;
				this.BoxStorage = box;
			}

			// Token: 0x06004A0B RID: 18955 RVA: 0x00185FEE File Offset: 0x001841EE
			public override string ToString()
			{
				return this.Access.ToString() + " " + this.BoxStorage;
			}

			// Token: 0x040023EB RID: 9195
			public ExpressionAccess Access;

			// Token: 0x040023EC RID: 9196
			public ParameterExpression BoxStorage;
		}
	}
}
