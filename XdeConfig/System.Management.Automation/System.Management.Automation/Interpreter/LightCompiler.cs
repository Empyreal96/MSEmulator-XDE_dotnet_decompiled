using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006EB RID: 1771
	internal sealed class LightCompiler
	{
		// Token: 0x060048E2 RID: 18658 RVA: 0x0017FB20 File Offset: 0x0017DD20
		public LightCompiler(int compilationThreshold)
		{
			this._instructions = new InstructionList();
			this._compilationThreshold = ((compilationThreshold < 0) ? 32 : compilationThreshold);
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x0017FB86 File Offset: 0x0017DD86
		private LightCompiler(LightCompiler parent) : this(parent._compilationThreshold)
		{
			this._parent = parent;
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x060048E4 RID: 18660 RVA: 0x0017FB9B File Offset: 0x0017DD9B
		public InstructionList Instructions
		{
			get
			{
				return this._instructions;
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x060048E5 RID: 18661 RVA: 0x0017FBA3 File Offset: 0x0017DDA3
		public LocalVariables Locals
		{
			get
			{
				return this._locals;
			}
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x0017FBAB File Offset: 0x0017DDAB
		internal static Expression Unbox(Expression strongBoxExpression)
		{
			return Expression.Field(strongBoxExpression, typeof(StrongBox<object>).GetField("Value"));
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x0017FBC8 File Offset: 0x0017DDC8
		public LightDelegateCreator CompileTop(LambdaExpression node)
		{
			for (int i = 0; i < node.Parameters.Count; i++)
			{
				ParameterExpression variable = node.Parameters[i];
				LocalDefinition localDefinition = this._locals.DefineLocal(variable, 0);
				this._instructions.EmitInitializeParameter(localDefinition.Index);
			}
			this.Compile(node.Body);
			if (node.Body.Type != typeof(void) && node.ReturnType == typeof(void))
			{
				this._instructions.EmitPop();
			}
			return new LightDelegateCreator(this.MakeInterpreter(node.Name), node);
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x0017FC74 File Offset: 0x0017DE74
		private Interpreter MakeInterpreter(string lambdaName)
		{
			if (this._forceCompile)
			{
				return null;
			}
			DebugInfo[] debugInfos = this._debugInfos.ToArray();
			return new Interpreter(lambdaName, this._locals, this.GetBranchMapping(), this._instructions.ToArray(), debugInfos, this._compilationThreshold);
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x0017FCBC File Offset: 0x0017DEBC
		private void CompileConstantExpression(Expression expr)
		{
			ConstantExpression constantExpression = (ConstantExpression)expr;
			this._instructions.EmitLoad(constantExpression.Value, constantExpression.Type);
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x0017FCE7 File Offset: 0x0017DEE7
		private void CompileDefaultExpression(Expression expr)
		{
			this.CompileDefaultExpression(expr.Type);
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x0017FCF8 File Offset: 0x0017DEF8
		private void CompileDefaultExpression(Type type)
		{
			if (type != typeof(void))
			{
				if (type.GetTypeInfo().IsValueType)
				{
					object primitiveDefaultValue = ScriptingRuntimeHelpers.GetPrimitiveDefaultValue(type);
					if (primitiveDefaultValue != null)
					{
						this._instructions.EmitLoad(primitiveDefaultValue);
						return;
					}
					this._instructions.EmitDefaultValue(type);
					return;
				}
				else
				{
					this._instructions.EmitLoad(null);
				}
			}
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x0017FD54 File Offset: 0x0017DF54
		private LocalVariable EnsureAvailableForClosure(ParameterExpression expr)
		{
			LocalVariable localVariable;
			if (this._locals.TryGetLocalOrClosure(expr, out localVariable))
			{
				if (!localVariable.InClosure && !localVariable.IsBoxed)
				{
					this._locals.Box(expr, this._instructions);
				}
				return localVariable;
			}
			if (this._parent != null)
			{
				this._parent.EnsureAvailableForClosure(expr);
				return this._locals.AddClosureVariable(expr);
			}
			throw new InvalidOperationException("unbound variable: " + expr);
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x0017FDC8 File Offset: 0x0017DFC8
		private LocalVariable ResolveLocal(ParameterExpression variable)
		{
			LocalVariable result;
			if (!this._locals.TryGetLocalOrClosure(variable, out result))
			{
				result = this.EnsureAvailableForClosure(variable);
			}
			return result;
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x0017FDF0 File Offset: 0x0017DFF0
		public void CompileGetVariable(ParameterExpression variable)
		{
			LocalVariable localVariable = this.ResolveLocal(variable);
			if (localVariable.InClosure)
			{
				this._instructions.EmitLoadLocalFromClosure(localVariable.Index);
				return;
			}
			if (localVariable.IsBoxed)
			{
				this._instructions.EmitLoadLocalBoxed(localVariable.Index);
				return;
			}
			this._instructions.EmitLoadLocal(localVariable.Index);
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x0017FE4C File Offset: 0x0017E04C
		public void CompileGetBoxedVariable(ParameterExpression variable)
		{
			LocalVariable localVariable = this.ResolveLocal(variable);
			if (localVariable.InClosure)
			{
				this._instructions.EmitLoadLocalFromClosureBoxed(localVariable.Index);
				return;
			}
			this._instructions.EmitLoadLocal(localVariable.Index);
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x0017FE8C File Offset: 0x0017E08C
		public void CompileSetVariable(ParameterExpression variable, bool isVoid)
		{
			LocalVariable localVariable = this.ResolveLocal(variable);
			if (localVariable.InClosure)
			{
				if (isVoid)
				{
					this._instructions.EmitStoreLocalToClosure(localVariable.Index);
					return;
				}
				this._instructions.EmitAssignLocalToClosure(localVariable.Index);
				return;
			}
			else if (localVariable.IsBoxed)
			{
				if (isVoid)
				{
					this._instructions.EmitStoreLocalBoxed(localVariable.Index);
					return;
				}
				this._instructions.EmitAssignLocalBoxed(localVariable.Index);
				return;
			}
			else
			{
				if (isVoid)
				{
					this._instructions.EmitStoreLocal(localVariable.Index);
					return;
				}
				this._instructions.EmitAssignLocal(localVariable.Index);
				return;
			}
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x0017FF28 File Offset: 0x0017E128
		public void CompileParameterExpression(Expression expr)
		{
			ParameterExpression variable = (ParameterExpression)expr;
			this.CompileGetVariable(variable);
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x0017FF44 File Offset: 0x0017E144
		private void CompileBlockExpression(Expression expr, bool asVoid)
		{
			BlockExpression blockExpression = (BlockExpression)expr;
			LocalDefinition[] locals = this.CompileBlockStart(blockExpression);
			Expression expr2 = blockExpression.Expressions[blockExpression.Expressions.Count - 1];
			this.Compile(expr2, asVoid);
			this.CompileBlockEnd(locals);
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x0017FF88 File Offset: 0x0017E188
		private LocalDefinition[] CompileBlockStart(BlockExpression node)
		{
			int count = this._instructions.Count;
			ReadOnlyCollection<ParameterExpression> variables = node.Variables;
			LocalDefinition[] array;
			if (variables.Count != 0)
			{
				array = new LocalDefinition[variables.Count];
				int num = 0;
				for (int i = 0; i < variables.Count; i++)
				{
					ParameterExpression parameterExpression = variables[i];
					LocalDefinition localDefinition = this._locals.DefineLocal(parameterExpression, count);
					array[num++] = localDefinition;
					this._instructions.EmitInitializeLocal(localDefinition.Index, parameterExpression.Type);
				}
			}
			else
			{
				array = LightCompiler.EmptyLocals;
			}
			for (int j = 0; j < node.Expressions.Count - 1; j++)
			{
				this.CompileAsVoid(node.Expressions[j]);
			}
			return array;
		}

		// Token: 0x060048F4 RID: 18676 RVA: 0x00180050 File Offset: 0x0017E250
		private void CompileBlockEnd(LocalDefinition[] locals)
		{
			foreach (LocalDefinition definition in locals)
			{
				this._locals.UndefineLocal(definition, this._instructions.Count);
			}
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x00180090 File Offset: 0x0017E290
		private void CompileIndexExpression(Expression expr)
		{
			IndexExpression indexExpression = (IndexExpression)expr;
			if (indexExpression.Object != null)
			{
				this.Compile(indexExpression.Object);
			}
			for (int i = 0; i < indexExpression.Arguments.Count; i++)
			{
				Expression expr2 = indexExpression.Arguments[i];
				this.Compile(expr2);
			}
			if (indexExpression.Indexer != null)
			{
				this._instructions.EmitCall(indexExpression.Indexer.GetMethod);
				return;
			}
			if (indexExpression.Arguments.Count != 1)
			{
				this._instructions.EmitCall(indexExpression.Object.Type.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public));
				return;
			}
			this._instructions.EmitGetArrayItem(indexExpression.Object.Type);
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x00180150 File Offset: 0x0017E350
		private void CompileIndexAssignment(BinaryExpression node, bool asVoid)
		{
			IndexExpression indexExpression = (IndexExpression)node.Left;
			if (!asVoid)
			{
				throw new NotImplementedException();
			}
			if (indexExpression.Object != null)
			{
				this.Compile(indexExpression.Object);
			}
			for (int i = 0; i < indexExpression.Arguments.Count; i++)
			{
				Expression expr = indexExpression.Arguments[i];
				this.Compile(expr);
			}
			this.Compile(node.Right);
			if (indexExpression.Indexer != null)
			{
				this._instructions.EmitCall(indexExpression.Indexer.SetMethod);
				return;
			}
			if (indexExpression.Arguments.Count != 1)
			{
				this._instructions.EmitCall(indexExpression.Object.Type.GetMethod("Set", BindingFlags.Instance | BindingFlags.Public));
				return;
			}
			this._instructions.EmitSetArrayItem(indexExpression.Object.Type);
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x00180228 File Offset: 0x0017E428
		private void CompileMemberAssignment(BinaryExpression node, bool asVoid)
		{
			MemberExpression memberExpression = (MemberExpression)node.Left;
			PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
			if (propertyInfo != null)
			{
				MethodInfo setMethod = propertyInfo.SetMethod;
				if (memberExpression.Expression != null)
				{
					this.Compile(memberExpression.Expression);
				}
				this.Compile(node.Right);
				int count = this._instructions.Count;
				if (!asVoid)
				{
					LocalDefinition definition = this._locals.DefineLocal(Expression.Parameter(node.Right.Type), count);
					this._instructions.EmitAssignLocal(definition.Index);
					this._instructions.EmitCall(setMethod);
					this._instructions.EmitLoadLocal(definition.Index);
					this._locals.UndefineLocal(definition, this._instructions.Count);
					return;
				}
				this._instructions.EmitCall(setMethod);
				return;
			}
			else
			{
				FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
				if (!(fieldInfo != null))
				{
					throw new NotImplementedException();
				}
				if (memberExpression.Expression != null)
				{
					this.Compile(memberExpression.Expression);
				}
				this.Compile(node.Right);
				int count2 = this._instructions.Count;
				if (!asVoid)
				{
					LocalDefinition definition2 = this._locals.DefineLocal(Expression.Parameter(node.Right.Type), count2);
					this._instructions.EmitAssignLocal(definition2.Index);
					this._instructions.EmitStoreField(fieldInfo);
					this._instructions.EmitLoadLocal(definition2.Index);
					this._locals.UndefineLocal(definition2, this._instructions.Count);
					return;
				}
				this._instructions.EmitStoreField(fieldInfo);
				return;
			}
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x001803CC File Offset: 0x0017E5CC
		private void CompileVariableAssignment(BinaryExpression node, bool asVoid)
		{
			this.Compile(node.Right);
			ParameterExpression variable = (ParameterExpression)node.Left;
			this.CompileSetVariable(variable, asVoid);
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x001803FC File Offset: 0x0017E5FC
		private void CompileAssignBinaryExpression(Expression expr, bool asVoid)
		{
			BinaryExpression binaryExpression = (BinaryExpression)expr;
			ExpressionType nodeType = binaryExpression.Left.NodeType;
			if (nodeType <= ExpressionType.Parameter)
			{
				if (nodeType == ExpressionType.MemberAccess)
				{
					this.CompileMemberAssignment(binaryExpression, asVoid);
					return;
				}
				if (nodeType != ExpressionType.Parameter)
				{
					goto IL_49;
				}
			}
			else if (nodeType != ExpressionType.Extension)
			{
				if (nodeType == ExpressionType.Index)
				{
					this.CompileIndexAssignment(binaryExpression, asVoid);
					return;
				}
				goto IL_49;
			}
			this.CompileVariableAssignment(binaryExpression, asVoid);
			return;
			IL_49:
			throw new InvalidOperationException("Invalid lvalue for assignment: " + binaryExpression.Left.NodeType);
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x00180474 File Offset: 0x0017E674
		private void CompileBinaryExpression(Expression expr)
		{
			BinaryExpression binaryExpression = (BinaryExpression)expr;
			if (binaryExpression.Method != null)
			{
				this.Compile(binaryExpression.Left);
				this.Compile(binaryExpression.Right);
				this._instructions.EmitCall(binaryExpression.Method);
				return;
			}
			ExpressionType nodeType = binaryExpression.NodeType;
			if (nodeType <= ExpressionType.LessThanOrEqual)
			{
				switch (nodeType)
				{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
					break;
				default:
					if (nodeType == ExpressionType.ArrayIndex)
					{
						this.Compile(binaryExpression.Left);
						this.Compile(binaryExpression.Right);
						this._instructions.EmitGetArrayItem(binaryExpression.Left.Type);
						return;
					}
					switch (nodeType)
					{
					case ExpressionType.Divide:
						break;
					case ExpressionType.Equal:
						this.CompileEqual(binaryExpression.Left, binaryExpression.Right);
						return;
					case ExpressionType.ExclusiveOr:
					case ExpressionType.Invoke:
					case ExpressionType.Lambda:
					case ExpressionType.LeftShift:
						goto IL_146;
					case ExpressionType.GreaterThan:
					case ExpressionType.GreaterThanOrEqual:
					case ExpressionType.LessThan:
					case ExpressionType.LessThanOrEqual:
						this.CompileComparison(binaryExpression.NodeType, binaryExpression.Left, binaryExpression.Right);
						return;
					default:
						goto IL_146;
					}
					break;
				}
			}
			else
			{
				switch (nodeType)
				{
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
					break;
				default:
					if (nodeType == ExpressionType.NotEqual)
					{
						this.CompileNotEqual(binaryExpression.Left, binaryExpression.Right);
						return;
					}
					switch (nodeType)
					{
					case ExpressionType.Subtract:
					case ExpressionType.SubtractChecked:
						break;
					default:
						goto IL_146;
					}
					break;
				}
			}
			this.CompileArithmetic(binaryExpression.NodeType, binaryExpression.Left, binaryExpression.Right);
			return;
			IL_146:
			throw new NotImplementedException(binaryExpression.NodeType.ToString());
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x001805DC File Offset: 0x0017E7DC
		private void CompileEqual(Expression left, Expression right)
		{
			this.Compile(left);
			this.Compile(right);
			this._instructions.EmitEqual(left.Type);
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x001805FD File Offset: 0x0017E7FD
		private void CompileNotEqual(Expression left, Expression right)
		{
			this.Compile(left);
			this.Compile(right);
			this._instructions.EmitNotEqual(left.Type);
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x00180620 File Offset: 0x0017E820
		private void CompileComparison(ExpressionType nodeType, Expression left, Expression right)
		{
			this.Compile(left);
			this.Compile(right);
			switch (nodeType)
			{
			case ExpressionType.GreaterThan:
				this._instructions.EmitGreaterThan(left.Type);
				return;
			case ExpressionType.GreaterThanOrEqual:
				this._instructions.EmitGreaterThanOrEqual(left.Type);
				return;
			case ExpressionType.LessThan:
				this._instructions.EmitLessThan(left.Type);
				return;
			case ExpressionType.LessThanOrEqual:
				this._instructions.EmitLessThanOrEqual(left.Type);
				return;
			}
			throw Assert.Unreachable;
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x001806B4 File Offset: 0x0017E8B4
		private void CompileArithmetic(ExpressionType nodeType, Expression left, Expression right)
		{
			this.Compile(left);
			this.Compile(right);
			if (nodeType <= ExpressionType.Divide)
			{
				switch (nodeType)
				{
				case ExpressionType.Add:
					this._instructions.EmitAdd(left.Type, false);
					return;
				case ExpressionType.AddChecked:
					this._instructions.EmitAdd(left.Type, true);
					return;
				default:
					if (nodeType == ExpressionType.Divide)
					{
						this._instructions.EmitDiv(left.Type);
						return;
					}
					break;
				}
			}
			else
			{
				switch (nodeType)
				{
				case ExpressionType.Multiply:
					this._instructions.EmitMul(left.Type, false);
					return;
				case ExpressionType.MultiplyChecked:
					this._instructions.EmitMul(left.Type, true);
					return;
				default:
					switch (nodeType)
					{
					case ExpressionType.Subtract:
						this._instructions.EmitSub(left.Type, false);
						return;
					case ExpressionType.SubtractChecked:
						this._instructions.EmitSub(left.Type, true);
						return;
					}
					break;
				}
			}
			throw Assert.Unreachable;
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x001807A4 File Offset: 0x0017E9A4
		private void CompileConvertUnaryExpression(Expression expr)
		{
			UnaryExpression unaryExpression = (UnaryExpression)expr;
			if (unaryExpression.Method != null)
			{
				this.Compile(unaryExpression.Operand);
				if (unaryExpression.Method != ScriptingRuntimeHelpers.Int32ToObjectMethod)
				{
					this._instructions.EmitCall(unaryExpression.Method);
					return;
				}
			}
			else
			{
				if (unaryExpression.Type == typeof(void))
				{
					this.CompileAsVoid(unaryExpression.Operand);
					return;
				}
				this.Compile(unaryExpression.Operand);
				this.CompileConvertToType(unaryExpression.Operand.Type, unaryExpression.Type, unaryExpression.NodeType == ExpressionType.ConvertChecked);
			}
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x00180848 File Offset: 0x0017EA48
		private void CompileConvertToType(Type typeFrom, Type typeTo, bool isChecked)
		{
			if (typeTo == typeFrom)
			{
				return;
			}
			TypeCode typeCode = typeFrom.GetTypeCode();
			TypeCode typeCode2 = typeTo.GetTypeCode();
			if (TypeUtils.IsNumeric(typeCode) && TypeUtils.IsNumeric(typeCode2))
			{
				if (isChecked)
				{
					this._instructions.EmitNumericConvertChecked(typeCode, typeCode2);
					return;
				}
				this._instructions.EmitNumericConvertUnchecked(typeCode, typeCode2);
			}
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x0018089B File Offset: 0x0017EA9B
		private void CompileNotExpression(UnaryExpression node)
		{
			if (node.Operand.Type == typeof(bool))
			{
				this.Compile(node.Operand);
				this._instructions.EmitNot();
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x001808D8 File Offset: 0x0017EAD8
		private void CompileUnaryExpression(Expression expr)
		{
			UnaryExpression unaryExpression = (UnaryExpression)expr;
			if (unaryExpression.Method != null)
			{
				this.Compile(unaryExpression.Operand);
				this._instructions.EmitCall(unaryExpression.Method);
				return;
			}
			ExpressionType nodeType = unaryExpression.NodeType;
			if (nodeType == ExpressionType.Not)
			{
				this.CompileNotExpression(unaryExpression);
				return;
			}
			if (nodeType != ExpressionType.TypeAs)
			{
				throw new NotImplementedException(unaryExpression.NodeType.ToString());
			}
			this.CompileTypeAsExpression(unaryExpression);
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x00180950 File Offset: 0x0017EB50
		private void CompileAndAlsoBinaryExpression(Expression expr)
		{
			this.CompileLogicalBinaryExpression(expr, true);
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x0018095A File Offset: 0x0017EB5A
		private void CompileOrElseBinaryExpression(Expression expr)
		{
			this.CompileLogicalBinaryExpression(expr, false);
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x00180964 File Offset: 0x0017EB64
		private void CompileLogicalBinaryExpression(Expression expr, bool andAlso)
		{
			BinaryExpression binaryExpression = (BinaryExpression)expr;
			if (binaryExpression.Method != null)
			{
				throw new NotImplementedException();
			}
			if (binaryExpression.Left.Type == typeof(bool))
			{
				BranchLabel branchLabel = this._instructions.MakeLabel();
				BranchLabel label = this._instructions.MakeLabel();
				this.Compile(binaryExpression.Left);
				if (andAlso)
				{
					this._instructions.EmitBranchFalse(branchLabel);
				}
				else
				{
					this._instructions.EmitBranchTrue(branchLabel);
				}
				this.Compile(binaryExpression.Right);
				this._instructions.EmitBranch(label, false, true);
				this._instructions.MarkLabel(branchLabel);
				this._instructions.EmitLoad(!andAlso);
				this._instructions.MarkLabel(label);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x00180A34 File Offset: 0x0017EC34
		private void CompileConditionalExpression(Expression expr, bool asVoid)
		{
			ConditionalExpression conditionalExpression = (ConditionalExpression)expr;
			this.Compile(conditionalExpression.Test);
			if (conditionalExpression.IfTrue == Utils.Empty())
			{
				BranchLabel branchLabel = this._instructions.MakeLabel();
				this._instructions.EmitBranchTrue(branchLabel);
				this.Compile(conditionalExpression.IfFalse, asVoid);
				this._instructions.MarkLabel(branchLabel);
				return;
			}
			BranchLabel branchLabel2 = this._instructions.MakeLabel();
			this._instructions.EmitBranchFalse(branchLabel2);
			this.Compile(conditionalExpression.IfTrue, asVoid);
			if (conditionalExpression.IfFalse != Utils.Empty())
			{
				BranchLabel label = this._instructions.MakeLabel();
				this._instructions.EmitBranch(label, false, !asVoid);
				this._instructions.MarkLabel(branchLabel2);
				this.Compile(conditionalExpression.IfFalse, asVoid);
				this._instructions.MarkLabel(label);
				return;
			}
			this._instructions.MarkLabel(branchLabel2);
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x00180B14 File Offset: 0x0017ED14
		private void CompileLoopExpression(Expression expr)
		{
		}

		// Token: 0x06004908 RID: 18696 RVA: 0x00180B4C File Offset: 0x0017ED4C
		private void CompileSwitchExpression(Expression expr)
		{
			SwitchExpression switchExpression = (SwitchExpression)expr;
			if (switchExpression.SwitchValue.Type != typeof(int) || switchExpression.Comparison != null)
			{
				throw new NotImplementedException();
			}
			if (!switchExpression.Cases.All((SwitchCase c) => c.TestValues.All((Expression t) => t is ConstantExpression)))
			{
				throw new NotImplementedException();
			}
			LabelInfo labelInfo = this.DefineLabel(null);
			bool hasValue = switchExpression.Type != typeof(void);
			this.Compile(switchExpression.SwitchValue);
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			int count = this._instructions.Count;
			this._instructions.EmitSwitch(dictionary);
			if (switchExpression.DefaultBody != null)
			{
				this.Compile(switchExpression.DefaultBody);
			}
			this._instructions.EmitBranch(labelInfo.GetLabel(this), false, hasValue);
			for (int i = 0; i < switchExpression.Cases.Count; i++)
			{
				SwitchCase switchCase = switchExpression.Cases[i];
				int value = this._instructions.Count - count;
				for (int j = 0; j < switchCase.TestValues.Count; j++)
				{
					ConstantExpression constantExpression = (ConstantExpression)switchCase.TestValues[j];
					dictionary[(int)constantExpression.Value] = value;
				}
				this.Compile(switchCase.Body);
				if (i < switchExpression.Cases.Count - 1)
				{
					this._instructions.EmitBranch(labelInfo.GetLabel(this), false, hasValue);
				}
			}
			this._instructions.MarkLabel(labelInfo.GetLabel(this));
		}

		// Token: 0x06004909 RID: 18697 RVA: 0x00180CF8 File Offset: 0x0017EEF8
		private void CompileLabelExpression(Expression expr)
		{
			LabelExpression labelExpression = (LabelExpression)expr;
			LabelInfo labelInfo = null;
			if (this._labelBlock.Kind == LabelScopeKind.Block)
			{
				this._labelBlock.TryGetLabelInfo(labelExpression.Target, out labelInfo);
				if (labelInfo == null && this._labelBlock.Parent.Kind == LabelScopeKind.Switch)
				{
					this._labelBlock.Parent.TryGetLabelInfo(labelExpression.Target, out labelInfo);
				}
			}
			if (labelInfo == null)
			{
				labelInfo = this.DefineLabel(labelExpression.Target);
			}
			if (labelExpression.DefaultValue != null)
			{
				if (labelExpression.Target.Type == typeof(void))
				{
					this.CompileAsVoid(labelExpression.DefaultValue);
				}
				else
				{
					this.Compile(labelExpression.DefaultValue);
				}
			}
			this._instructions.MarkLabel(labelInfo.GetLabel(this));
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x00180DC0 File Offset: 0x0017EFC0
		private void CompileGotoExpression(Expression expr)
		{
			GotoExpression gotoExpression = (GotoExpression)expr;
			LabelInfo labelInfo = this.ReferenceLabel(gotoExpression.Target);
			if (gotoExpression.Value != null)
			{
				this.Compile(gotoExpression.Value);
			}
			this._instructions.EmitGoto(labelInfo.GetLabel(this), gotoExpression.Type != typeof(void), gotoExpression.Value != null && gotoExpression.Value.Type != typeof(void));
		}

		// Token: 0x0600490B RID: 18699 RVA: 0x00180E41 File Offset: 0x0017F041
		public BranchLabel GetBranchLabel(LabelTarget target)
		{
			return this.ReferenceLabel(target).GetLabel(this);
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x00180E50 File Offset: 0x0017F050
		public void PushLabelBlock(LabelScopeKind type)
		{
			this._labelBlock = new LabelScopeInfo(this._labelBlock, type);
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x00180E64 File Offset: 0x0017F064
		public void PopLabelBlock(LabelScopeKind kind)
		{
			this._labelBlock = this._labelBlock.Parent;
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x00180E78 File Offset: 0x0017F078
		private LabelInfo EnsureLabel(LabelTarget node)
		{
			LabelInfo result;
			if (!this._treeLabels.TryGetValue(node, out result))
			{
				result = (this._treeLabels[node] = new LabelInfo(node));
			}
			return result;
		}

		// Token: 0x0600490F RID: 18703 RVA: 0x00180EAC File Offset: 0x0017F0AC
		private LabelInfo ReferenceLabel(LabelTarget node)
		{
			LabelInfo labelInfo = this.EnsureLabel(node);
			labelInfo.Reference(this._labelBlock);
			return labelInfo;
		}

		// Token: 0x06004910 RID: 18704 RVA: 0x00180ED0 File Offset: 0x0017F0D0
		internal LabelInfo DefineLabel(LabelTarget node)
		{
			if (node == null)
			{
				return new LabelInfo(null);
			}
			LabelInfo labelInfo = this.EnsureLabel(node);
			labelInfo.Define(this._labelBlock);
			return labelInfo;
		}

		// Token: 0x06004911 RID: 18705 RVA: 0x00180EFC File Offset: 0x0017F0FC
		private bool TryPushLabelBlock(Expression node)
		{
			ExpressionType nodeType = node.NodeType;
			switch (nodeType)
			{
			case ExpressionType.Conditional:
				goto IL_151;
			case ExpressionType.Constant:
				break;
			case ExpressionType.Convert:
				if (!(node.Type != typeof(void)))
				{
					this.PushLabelBlock(LabelScopeKind.Statement);
					return true;
				}
				break;
			default:
				if (nodeType == ExpressionType.Block)
				{
					this.PushLabelBlock(LabelScopeKind.Block);
					if (this._labelBlock.Parent.Kind != LabelScopeKind.Switch)
					{
						this.DefineBlockLabels(node);
					}
					return true;
				}
				switch (nodeType)
				{
				case ExpressionType.Goto:
				case ExpressionType.Loop:
					goto IL_151;
				case ExpressionType.Label:
					if (this._labelBlock.Kind == LabelScopeKind.Block)
					{
						LabelTarget target = ((LabelExpression)node).Target;
						if (this._labelBlock.ContainsTarget(target))
						{
							return false;
						}
						if (this._labelBlock.Parent.Kind == LabelScopeKind.Switch && this._labelBlock.Parent.ContainsTarget(target))
						{
							return false;
						}
					}
					this.PushLabelBlock(LabelScopeKind.Statement);
					return true;
				case ExpressionType.Switch:
				{
					this.PushLabelBlock(LabelScopeKind.Switch);
					SwitchExpression switchExpression = (SwitchExpression)node;
					for (int i = 0; i < switchExpression.Cases.Count; i++)
					{
						SwitchCase switchCase = switchExpression.Cases[i];
						this.DefineBlockLabels(switchCase.Body);
					}
					this.DefineBlockLabels(switchExpression.DefaultBody);
					return true;
				}
				}
				break;
			}
			if (this._labelBlock.Kind != LabelScopeKind.Expression)
			{
				this.PushLabelBlock(LabelScopeKind.Expression);
				return true;
			}
			return false;
			IL_151:
			this.PushLabelBlock(LabelScopeKind.Statement);
			return true;
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x00181064 File Offset: 0x0017F264
		private void DefineBlockLabels(Expression node)
		{
			BlockExpression blockExpression = node as BlockExpression;
			if (blockExpression == null)
			{
				return;
			}
			int i = 0;
			int count = blockExpression.Expressions.Count;
			while (i < count)
			{
				Expression expression = blockExpression.Expressions[i];
				LabelExpression labelExpression = expression as LabelExpression;
				if (labelExpression != null)
				{
					this.DefineLabel(labelExpression.Target);
				}
				i++;
			}
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x001810BC File Offset: 0x0017F2BC
		private HybridReferenceDictionary<LabelTarget, BranchLabel> GetBranchMapping()
		{
			HybridReferenceDictionary<LabelTarget, BranchLabel> hybridReferenceDictionary = new HybridReferenceDictionary<LabelTarget, BranchLabel>(this._treeLabels.Count);
			foreach (KeyValuePair<LabelTarget, LabelInfo> keyValuePair in this._treeLabels)
			{
				hybridReferenceDictionary[keyValuePair.Key] = keyValuePair.Value.GetLabel(this);
			}
			return hybridReferenceDictionary;
		}

		// Token: 0x06004914 RID: 18708 RVA: 0x00181130 File Offset: 0x0017F330
		private void CompileThrowUnaryExpression(Expression expr, bool asVoid)
		{
			UnaryExpression unaryExpression = (UnaryExpression)expr;
			if (unaryExpression.Operand == null)
			{
				this.CompileParameterExpression(this._exceptionForRethrowStack.Peek());
				if (asVoid)
				{
					this._instructions.EmitRethrowVoid();
					return;
				}
				this._instructions.EmitRethrow();
				return;
			}
			else
			{
				this.Compile(unaryExpression.Operand);
				if (asVoid)
				{
					this._instructions.EmitThrowVoid();
					return;
				}
				this._instructions.EmitThrow();
				return;
			}
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x001811A0 File Offset: 0x0017F3A0
		private bool EndsWithRethrow(Expression expr)
		{
			if (expr.NodeType == ExpressionType.Throw)
			{
				UnaryExpression unaryExpression = (UnaryExpression)expr;
				return unaryExpression.Operand == null;
			}
			BlockExpression blockExpression = expr as BlockExpression;
			return blockExpression != null && this.EndsWithRethrow(blockExpression.Expressions[blockExpression.Expressions.Count - 1]);
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x001811F4 File Offset: 0x0017F3F4
		private void CompileAsVoidRemoveRethrow(Expression expr)
		{
			int currentStackDepth = this._instructions.CurrentStackDepth;
			if (expr.NodeType == ExpressionType.Throw)
			{
				return;
			}
			BlockExpression blockExpression = (BlockExpression)expr;
			LocalDefinition[] locals = this.CompileBlockStart(blockExpression);
			this.CompileAsVoidRemoveRethrow(blockExpression.Expressions[blockExpression.Expressions.Count - 1]);
			this.CompileBlockEnd(locals);
		}

		// Token: 0x06004917 RID: 18711 RVA: 0x0018124C File Offset: 0x0017F44C
		private void CompileTryExpression(Expression expr)
		{
			TryExpression tryExpression = (TryExpression)expr;
			BranchLabel label = this._instructions.MakeLabel();
			BranchLabel branchLabel = this._instructions.MakeLabel();
			int count = this._instructions.Count;
			BranchLabel branchLabel2 = null;
			if (tryExpression.Finally != null)
			{
				branchLabel2 = this._instructions.MakeLabel();
				this._instructions.EmitEnterTryFinally(branchLabel2);
			}
			else
			{
				this._instructions.EmitEnterTryCatch();
			}
			List<ExceptionHandler> list = null;
			EnterTryCatchFinallyInstruction enterTryCatchFinallyInstruction = this._instructions.GetInstruction(count) as EnterTryCatchFinallyInstruction;
			this.PushLabelBlock(LabelScopeKind.Try);
			this.Compile(tryExpression.Body);
			bool flag = tryExpression.Body.Type != typeof(void);
			int count2 = this._instructions.Count;
			this._instructions.MarkLabel(branchLabel);
			this._instructions.EmitGoto(label, flag, flag);
			if (tryExpression.Handlers.Count > 0)
			{
				list = new List<ExceptionHandler>();
				if (tryExpression.Finally == null && tryExpression.Handlers.Count == 1)
				{
					CatchBlock catchBlock = tryExpression.Handlers[0];
					if (catchBlock.Filter == null && catchBlock.Test == typeof(Exception) && catchBlock.Variable == null && this.EndsWithRethrow(catchBlock.Body))
					{
						if (flag)
						{
							this._instructions.EmitEnterExceptionHandlerNonVoid();
						}
						else
						{
							this._instructions.EmitEnterExceptionHandlerVoid();
						}
						int labelIndex = this._instructions.MarkRuntimeLabel();
						int count3 = this._instructions.Count;
						this.CompileAsVoidRemoveRethrow(catchBlock.Body);
						this._instructions.EmitLeaveFault(flag);
						this._instructions.MarkLabel(label);
						list.Add(new ExceptionHandler(count, count2, labelIndex, count3, this._instructions.Count, null));
						enterTryCatchFinallyInstruction.SetTryHandler(new TryCatchFinallyHandler(count, count2, branchLabel.TargetIndex, list.ToArray()));
						this.PopLabelBlock(LabelScopeKind.Try);
						return;
					}
				}
				for (int i = 0; i < tryExpression.Handlers.Count; i++)
				{
					CatchBlock catchBlock2 = tryExpression.Handlers[i];
					this.PushLabelBlock(LabelScopeKind.Catch);
					if (catchBlock2.Filter != null)
					{
						throw new NotImplementedException();
					}
					ParameterExpression parameterExpression = catchBlock2.Variable ?? Expression.Parameter(catchBlock2.Test);
					LocalDefinition definition = this._locals.DefineLocal(parameterExpression, this._instructions.Count);
					this._exceptionForRethrowStack.Push(parameterExpression);
					if (flag)
					{
						this._instructions.EmitEnterExceptionHandlerNonVoid();
					}
					else
					{
						this._instructions.EmitEnterExceptionHandlerVoid();
					}
					int labelIndex2 = this._instructions.MarkRuntimeLabel();
					int count4 = this._instructions.Count;
					this.CompileSetVariable(parameterExpression, true);
					this.Compile(catchBlock2.Body);
					this._exceptionForRethrowStack.Pop();
					this._instructions.EmitLeaveExceptionHandler(flag, branchLabel);
					list.Add(new ExceptionHandler(count, count2, labelIndex2, count4, this._instructions.Count, catchBlock2.Test));
					this.PopLabelBlock(LabelScopeKind.Catch);
					this._locals.UndefineLocal(definition, this._instructions.Count);
				}
				if (tryExpression.Fault != null)
				{
					throw new NotImplementedException();
				}
			}
			if (tryExpression.Finally != null)
			{
				this.PushLabelBlock(LabelScopeKind.Finally);
				this._instructions.MarkLabel(branchLabel2);
				this._instructions.EmitEnterFinally(branchLabel2);
				this.CompileAsVoid(tryExpression.Finally);
				this._instructions.EmitLeaveFinally();
				enterTryCatchFinallyInstruction.SetTryHandler(new TryCatchFinallyHandler(count, count2, branchLabel.TargetIndex, branchLabel2.TargetIndex, this._instructions.Count, (list != null) ? list.ToArray() : null));
				this.PopLabelBlock(LabelScopeKind.Finally);
			}
			else
			{
				enterTryCatchFinallyInstruction.SetTryHandler(new TryCatchFinallyHandler(count, count2, branchLabel.TargetIndex, list.ToArray()));
			}
			this._instructions.MarkLabel(label);
			this.PopLabelBlock(LabelScopeKind.Try);
		}

		// Token: 0x06004918 RID: 18712 RVA: 0x00181634 File Offset: 0x0017F834
		private void CompileDynamicExpression(Expression expr)
		{
			DynamicExpression dynamicExpression = (DynamicExpression)expr;
			for (int i = 0; i < dynamicExpression.Arguments.Count; i++)
			{
				Expression expr2 = dynamicExpression.Arguments[i];
				this.Compile(expr2);
			}
			this._instructions.EmitDynamic(dynamicExpression.DelegateType, dynamicExpression.Binder);
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x0018169C File Offset: 0x0017F89C
		private void CompileMethodCallExpression(Expression expr)
		{
			MethodCallExpression methodCallExpression = (MethodCallExpression)expr;
			ParameterInfo[] parameters = methodCallExpression.Method.GetParameters();
			TypeInfo typeInfo = methodCallExpression.Method.DeclaringType.GetTypeInfo();
			if (!parameters.TrueForAll((ParameterInfo p) => !p.ParameterType.IsByRef) || (!methodCallExpression.Method.IsStatic && typeInfo.IsValueType && !typeInfo.IsPrimitive))
			{
				this._forceCompile = true;
			}
			if (!methodCallExpression.Method.IsStatic)
			{
				this.Compile(methodCallExpression.Object);
			}
			for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
			{
				Expression expr2 = methodCallExpression.Arguments[i];
				this.Compile(expr2);
			}
			this._instructions.EmitCall(methodCallExpression.Method, parameters);
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x00181780 File Offset: 0x0017F980
		private void CompileNewExpression(Expression expr)
		{
			NewExpression newExpression = (NewExpression)expr;
			if (newExpression.Constructor != null)
			{
				ParameterInfo[] parameters = newExpression.Constructor.GetParameters();
				if (!parameters.TrueForAll((ParameterInfo p) => !p.ParameterType.IsByRef))
				{
					this._forceCompile = true;
				}
			}
			if (newExpression.Constructor != null)
			{
				for (int i = 0; i < newExpression.Arguments.Count; i++)
				{
					Expression expr2 = newExpression.Arguments[i];
					this.Compile(expr2);
				}
				this._instructions.EmitNew(newExpression.Constructor);
				return;
			}
			this._instructions.EmitDefaultValue(newExpression.Type);
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x00181838 File Offset: 0x0017FA38
		private void CompileMemberExpression(Expression expr)
		{
			MemberExpression memberExpression = (MemberExpression)expr;
			MemberInfo member = memberExpression.Member;
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				if (fieldInfo.IsLiteral)
				{
					this._instructions.EmitLoad(fieldInfo.GetValue(null), fieldInfo.FieldType);
					return;
				}
				if (!fieldInfo.IsStatic)
				{
					this.Compile(memberExpression.Expression);
					this._instructions.EmitLoadField(fieldInfo);
					return;
				}
				if (fieldInfo.IsInitOnly)
				{
					this._instructions.EmitLoad(fieldInfo.GetValue(null), fieldInfo.FieldType);
					return;
				}
				this._instructions.EmitLoadField(fieldInfo);
				return;
			}
			else
			{
				PropertyInfo propertyInfo = member as PropertyInfo;
				if (propertyInfo != null)
				{
					MethodInfo getMethod = propertyInfo.GetMethod;
					if (memberExpression.Expression != null)
					{
						this.Compile(memberExpression.Expression);
					}
					this._instructions.EmitCall(getMethod);
					return;
				}
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x00181914 File Offset: 0x0017FB14
		private void CompileNewArrayExpression(Expression expr)
		{
			NewArrayExpression newArrayExpression = (NewArrayExpression)expr;
			for (int i = 0; i < newArrayExpression.Expressions.Count; i++)
			{
				Expression expr2 = newArrayExpression.Expressions[i];
				this.Compile(expr2);
			}
			Type elementType = newArrayExpression.Type.GetElementType();
			int count = newArrayExpression.Expressions.Count;
			if (newArrayExpression.NodeType == ExpressionType.NewArrayInit)
			{
				this._instructions.EmitNewArrayInit(elementType, count);
				return;
			}
			if (newArrayExpression.NodeType != ExpressionType.NewArrayBounds)
			{
				throw new NotImplementedException();
			}
			if (count == 1)
			{
				this._instructions.EmitNewArray(elementType);
				return;
			}
			this._instructions.EmitNewArrayBounds(elementType, count);
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x001819B4 File Offset: 0x0017FBB4
		private void CompileExtensionExpression(Expression expr)
		{
			IInstructionProvider instructionProvider = expr as IInstructionProvider;
			if (instructionProvider != null)
			{
				instructionProvider.AddInstructions(this);
				return;
			}
			if (expr.CanReduce)
			{
				this.Compile(expr.Reduce());
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x001819F0 File Offset: 0x0017FBF0
		private void CompileDebugInfoExpression(Expression expr)
		{
			DebugInfoExpression debugInfoExpression = (DebugInfoExpression)expr;
			int count = this._instructions.Count;
			DebugInfo item = new DebugInfo
			{
				Index = count,
				FileName = debugInfoExpression.Document.FileName,
				StartLine = debugInfoExpression.StartLine,
				EndLine = debugInfoExpression.EndLine,
				IsClear = debugInfoExpression.IsClear
			};
			this._debugInfos.Add(item);
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x00181A60 File Offset: 0x0017FC60
		private void CompileRuntimeVariablesExpression(Expression expr)
		{
			RuntimeVariablesExpression runtimeVariablesExpression = (RuntimeVariablesExpression)expr;
			for (int i = 0; i < runtimeVariablesExpression.Variables.Count; i++)
			{
				ParameterExpression parameterExpression = runtimeVariablesExpression.Variables[i];
				this.EnsureAvailableForClosure(parameterExpression);
				this.CompileGetBoxedVariable(parameterExpression);
			}
			this._instructions.EmitNewRuntimeVariables(runtimeVariablesExpression.Variables.Count);
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x00181ABC File Offset: 0x0017FCBC
		private void CompileLambdaExpression(Expression expr)
		{
			LambdaExpression node = (LambdaExpression)expr;
			LightCompiler lightCompiler = new LightCompiler(this);
			LightDelegateCreator creator = lightCompiler.CompileTop(node);
			if (lightCompiler._locals.ClosureVariables != null)
			{
				foreach (ParameterExpression variable in lightCompiler._locals.ClosureVariables.Keys)
				{
					this.CompileGetBoxedVariable(variable);
				}
			}
			this._instructions.EmitCreateDelegate(creator);
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x00181B4C File Offset: 0x0017FD4C
		private void CompileCoalesceBinaryExpression(Expression expr)
		{
			BinaryExpression binaryExpression = (BinaryExpression)expr;
			if (TypeUtils.IsNullableType(binaryExpression.Left.Type))
			{
				throw new NotImplementedException();
			}
			if (binaryExpression.Conversion != null)
			{
				throw new NotImplementedException();
			}
			BranchLabel branchLabel = this._instructions.MakeLabel();
			this.Compile(binaryExpression.Left);
			this._instructions.EmitCoalescingBranch(branchLabel);
			this._instructions.EmitPop();
			this.Compile(binaryExpression.Right);
			this._instructions.MarkLabel(branchLabel);
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x00181BD0 File Offset: 0x0017FDD0
		private void CompileInvocationExpression(Expression expr)
		{
			InvocationExpression invocationExpression = (InvocationExpression)expr;
			if (typeof(LambdaExpression).IsAssignableFrom(invocationExpression.Expression.Type))
			{
				throw new NotImplementedException();
			}
			this.CompileMethodCallExpression(Expression.Call(invocationExpression.Expression, invocationExpression.Expression.Type.GetMethod("Invoke"), invocationExpression.Arguments));
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x00181C32 File Offset: 0x0017FE32
		private void CompileListInitExpression(Expression expr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x00181C39 File Offset: 0x0017FE39
		private void CompileMemberInitExpression(Expression expr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x00181C40 File Offset: 0x0017FE40
		private void CompileQuoteUnaryExpression(Expression expr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x00181C48 File Offset: 0x0017FE48
		private void CompileUnboxUnaryExpression(Expression expr)
		{
			UnaryExpression unaryExpression = (UnaryExpression)expr;
			this.Compile(unaryExpression.Operand);
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x00181C68 File Offset: 0x0017FE68
		private void CompileTypeEqualExpression(Expression expr)
		{
			TypeBinaryExpression typeBinaryExpression = (TypeBinaryExpression)expr;
			this.Compile(typeBinaryExpression.Expression);
			this._instructions.EmitLoad(typeBinaryExpression.TypeOperand);
			this._instructions.EmitTypeEquals();
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x00181CA4 File Offset: 0x0017FEA4
		private void CompileTypeAsExpression(UnaryExpression node)
		{
			this.Compile(node.Operand);
			this._instructions.EmitTypeAs(node.Type);
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x00181CC4 File Offset: 0x0017FEC4
		private void CompileTypeIsExpression(Expression expr)
		{
			TypeBinaryExpression typeBinaryExpression = (TypeBinaryExpression)expr;
			this.Compile(typeBinaryExpression.Expression);
			if (typeBinaryExpression.TypeOperand.GetTypeInfo().IsSealed)
			{
				this._instructions.EmitLoad(typeBinaryExpression.TypeOperand);
				this._instructions.EmitTypeEquals();
				return;
			}
			this._instructions.EmitTypeIs(typeBinaryExpression.TypeOperand);
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x00181D24 File Offset: 0x0017FF24
		private void CompileReducibleExpression(Expression expr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x00181D2B File Offset: 0x0017FF2B
		internal void Compile(Expression expr, bool asVoid)
		{
			if (asVoid)
			{
				this.CompileAsVoid(expr);
				return;
			}
			this.Compile(expr);
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x00181D40 File Offset: 0x0017FF40
		internal void CompileAsVoid(Expression expr)
		{
			bool flag = this.TryPushLabelBlock(expr);
			int currentStackDepth = this._instructions.CurrentStackDepth;
			ExpressionType nodeType = expr.NodeType;
			if (nodeType <= ExpressionType.Parameter)
			{
				if (nodeType == ExpressionType.Constant || nodeType == ExpressionType.Parameter)
				{
					goto IL_90;
				}
			}
			else
			{
				switch (nodeType)
				{
				case ExpressionType.Assign:
					this.CompileAssignBinaryExpression(expr, true);
					goto IL_90;
				case ExpressionType.Block:
					this.CompileBlockExpression(expr, true);
					goto IL_90;
				default:
					if (nodeType == ExpressionType.Default)
					{
						goto IL_90;
					}
					if (nodeType == ExpressionType.Throw)
					{
						this.CompileThrowUnaryExpression(expr, true);
						goto IL_90;
					}
					break;
				}
			}
			this.CompileNoLabelPush(expr);
			if (expr.Type != typeof(void))
			{
				this._instructions.EmitPop();
			}
			IL_90:
			if (flag)
			{
				this.PopLabelBlock(this._labelBlock.Kind);
			}
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x00181DF4 File Offset: 0x0017FFF4
		private void CompileNoLabelPush(Expression expr)
		{
			int currentStackDepth = this._instructions.CurrentStackDepth;
			switch (expr.NodeType)
			{
			case ExpressionType.Add:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.AddChecked:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.And:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.AndAlso:
				this.CompileAndAlsoBinaryExpression(expr);
				return;
			case ExpressionType.ArrayLength:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.ArrayIndex:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.Call:
				this.CompileMethodCallExpression(expr);
				return;
			case ExpressionType.Coalesce:
				this.CompileCoalesceBinaryExpression(expr);
				return;
			case ExpressionType.Conditional:
				this.CompileConditionalExpression(expr, expr.Type == typeof(void));
				return;
			case ExpressionType.Constant:
				this.CompileConstantExpression(expr);
				return;
			case ExpressionType.Convert:
				this.CompileConvertUnaryExpression(expr);
				return;
			case ExpressionType.ConvertChecked:
				this.CompileConvertUnaryExpression(expr);
				return;
			case ExpressionType.Divide:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.Equal:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.ExclusiveOr:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.GreaterThan:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.GreaterThanOrEqual:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.Invoke:
				this.CompileInvocationExpression(expr);
				return;
			case ExpressionType.Lambda:
				this.CompileLambdaExpression(expr);
				return;
			case ExpressionType.LeftShift:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.LessThan:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.LessThanOrEqual:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.ListInit:
				this.CompileListInitExpression(expr);
				return;
			case ExpressionType.MemberAccess:
				this.CompileMemberExpression(expr);
				return;
			case ExpressionType.MemberInit:
				this.CompileMemberInitExpression(expr);
				return;
			case ExpressionType.Modulo:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.Multiply:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.MultiplyChecked:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.Negate:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.UnaryPlus:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.NegateChecked:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.New:
				this.CompileNewExpression(expr);
				return;
			case ExpressionType.NewArrayInit:
				this.CompileNewArrayExpression(expr);
				return;
			case ExpressionType.NewArrayBounds:
				this.CompileNewArrayExpression(expr);
				return;
			case ExpressionType.Not:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.NotEqual:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.Or:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.OrElse:
				this.CompileOrElseBinaryExpression(expr);
				return;
			case ExpressionType.Parameter:
				this.CompileParameterExpression(expr);
				return;
			case ExpressionType.Power:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.Quote:
				this.CompileQuoteUnaryExpression(expr);
				return;
			case ExpressionType.RightShift:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.Subtract:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.SubtractChecked:
				this.CompileBinaryExpression(expr);
				return;
			case ExpressionType.TypeAs:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.TypeIs:
				this.CompileTypeIsExpression(expr);
				return;
			case ExpressionType.Assign:
				this.CompileAssignBinaryExpression(expr, expr.Type == typeof(void));
				return;
			case ExpressionType.Block:
				this.CompileBlockExpression(expr, expr.Type == typeof(void));
				return;
			case ExpressionType.DebugInfo:
				this.CompileDebugInfoExpression(expr);
				return;
			case ExpressionType.Decrement:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.Dynamic:
				this.CompileDynamicExpression(expr);
				return;
			case ExpressionType.Default:
				this.CompileDefaultExpression(expr);
				return;
			case ExpressionType.Extension:
				this.CompileExtensionExpression(expr);
				return;
			case ExpressionType.Goto:
				this.CompileGotoExpression(expr);
				return;
			case ExpressionType.Increment:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.Index:
				this.CompileIndexExpression(expr);
				return;
			case ExpressionType.Label:
				this.CompileLabelExpression(expr);
				return;
			case ExpressionType.RuntimeVariables:
				this.CompileRuntimeVariablesExpression(expr);
				return;
			case ExpressionType.Loop:
				this.CompileLoopExpression(expr);
				return;
			case ExpressionType.Switch:
				this.CompileSwitchExpression(expr);
				return;
			case ExpressionType.Throw:
				this.CompileThrowUnaryExpression(expr, expr.Type == typeof(void));
				return;
			case ExpressionType.Try:
				this.CompileTryExpression(expr);
				return;
			case ExpressionType.Unbox:
				this.CompileUnboxUnaryExpression(expr);
				return;
			case ExpressionType.AddAssign:
			case ExpressionType.AndAssign:
			case ExpressionType.DivideAssign:
			case ExpressionType.ExclusiveOrAssign:
			case ExpressionType.LeftShiftAssign:
			case ExpressionType.ModuloAssign:
			case ExpressionType.MultiplyAssign:
			case ExpressionType.OrAssign:
			case ExpressionType.PowerAssign:
			case ExpressionType.RightShiftAssign:
			case ExpressionType.SubtractAssign:
			case ExpressionType.AddAssignChecked:
			case ExpressionType.MultiplyAssignChecked:
			case ExpressionType.SubtractAssignChecked:
			case ExpressionType.PreIncrementAssign:
			case ExpressionType.PreDecrementAssign:
			case ExpressionType.PostIncrementAssign:
			case ExpressionType.PostDecrementAssign:
				this.CompileReducibleExpression(expr);
				return;
			case ExpressionType.TypeEqual:
				this.CompileTypeEqualExpression(expr);
				return;
			case ExpressionType.OnesComplement:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.IsTrue:
				this.CompileUnaryExpression(expr);
				return;
			case ExpressionType.IsFalse:
				this.CompileUnaryExpression(expr);
				return;
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x001821EC File Offset: 0x001803EC
		public void Compile(Expression expr)
		{
			bool flag = this.TryPushLabelBlock(expr);
			this.CompileNoLabelPush(expr);
			if (flag)
			{
				this.PopLabelBlock(this._labelBlock.Kind);
			}
		}

		// Token: 0x040023A5 RID: 9125
		internal const int DefaultCompilationThreshold = 32;

		// Token: 0x040023A6 RID: 9126
		private readonly int _compilationThreshold;

		// Token: 0x040023A7 RID: 9127
		private readonly InstructionList _instructions;

		// Token: 0x040023A8 RID: 9128
		private readonly LocalVariables _locals = new LocalVariables();

		// Token: 0x040023A9 RID: 9129
		private readonly List<DebugInfo> _debugInfos = new List<DebugInfo>();

		// Token: 0x040023AA RID: 9130
		private readonly HybridReferenceDictionary<LabelTarget, LabelInfo> _treeLabels = new HybridReferenceDictionary<LabelTarget, LabelInfo>();

		// Token: 0x040023AB RID: 9131
		private LabelScopeInfo _labelBlock = new LabelScopeInfo(null, LabelScopeKind.Lambda);

		// Token: 0x040023AC RID: 9132
		private readonly Stack<ParameterExpression> _exceptionForRethrowStack = new Stack<ParameterExpression>();

		// Token: 0x040023AD RID: 9133
		private bool _forceCompile;

		// Token: 0x040023AE RID: 9134
		private readonly LightCompiler _parent;

		// Token: 0x040023AF RID: 9135
		private static LocalDefinition[] EmptyLocals = new LocalDefinition[0];
	}
}
