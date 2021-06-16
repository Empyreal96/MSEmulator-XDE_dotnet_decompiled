using System;
using System.Collections;
using System.Linq;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Language
{
	// Token: 0x020005C1 RID: 1473
	internal class GetSafeValueVisitor : ICustomAstVisitor
	{
		// Token: 0x06003EE0 RID: 16096 RVA: 0x0014C3B8 File Offset: 0x0014A5B8
		private GetSafeValueVisitor()
		{
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0014C3C0 File Offset: 0x0014A5C0
		public static object GetSafeValue(Ast ast, ExecutionContext context, bool isForGetPowerShell)
		{
			GetSafeValueVisitor._context = context;
			if (IsSafeValueVisitor.IsAstSafe(ast, isForGetPowerShell))
			{
				return ast.Accept(new GetSafeValueVisitor());
			}
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x0014C3E7 File Offset: 0x0014A5E7
		public object VisitErrorStatement(ErrorStatementAst errorStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x0014C3F3 File Offset: 0x0014A5F3
		public object VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x0014C3FF File Offset: 0x0014A5FF
		public object VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0014C40B File Offset: 0x0014A60B
		public object VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0014C417 File Offset: 0x0014A617
		public object VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0014C423 File Offset: 0x0014A623
		public object VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x0014C42F File Offset: 0x0014A62F
		public object VisitAttribute(AttributeAst attributeAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x0014C43B File Offset: 0x0014A63B
		public object VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x0014C447 File Offset: 0x0014A647
		public object VisitParameter(ParameterAst parameterAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x0014C453 File Offset: 0x0014A653
		public object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x0014C45F File Offset: 0x0014A65F
		public object VisitIfStatement(IfStatementAst ifStmtAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x0014C46B File Offset: 0x0014A66B
		public object VisitTrap(TrapStatementAst trapStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x0014C477 File Offset: 0x0014A677
		public object VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x0014C483 File Offset: 0x0014A683
		public object VisitDataStatement(DataStatementAst dataStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x0014C48F File Offset: 0x0014A68F
		public object VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0014C49B File Offset: 0x0014A69B
		public object VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x0014C4A7 File Offset: 0x0014A6A7
		public object VisitForStatement(ForStatementAst forStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x0014C4B3 File Offset: 0x0014A6B3
		public object VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0014C4BF File Offset: 0x0014A6BF
		public object VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x0014C4CB File Offset: 0x0014A6CB
		public object VisitTryStatement(TryStatementAst tryStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x0014C4D7 File Offset: 0x0014A6D7
		public object VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x0014C4E3 File Offset: 0x0014A6E3
		public object VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x0014C4EF File Offset: 0x0014A6EF
		public object VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x0014C4FB File Offset: 0x0014A6FB
		public object VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x0014C507 File Offset: 0x0014A707
		public object VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x0014C513 File Offset: 0x0014A713
		public object VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x0014C51F File Offset: 0x0014A71F
		public object VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x0014C52B File Offset: 0x0014A72B
		public object VisitCommand(CommandAst commandAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x0014C537 File Offset: 0x0014A737
		public object VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x0014C543 File Offset: 0x0014A743
		public object VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x0014C54F File Offset: 0x0014A74F
		public object VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x0014C55B File Offset: 0x0014A75B
		public object VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x0014C567 File Offset: 0x0014A767
		public object VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x0014C573 File Offset: 0x0014A773
		public object VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x0014C57F File Offset: 0x0014A77F
		public object VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x0014C58C File Offset: 0x0014A78C
		private object GetSingleValueFromTarget(object target, object index)
		{
			string text = target as string;
			if (text != null)
			{
				int num = (int)index;
				if (Math.Abs(num) >= text.Length)
				{
					return null;
				}
				return (num >= 0) ? text[num] : text[text.Length + num];
			}
			else
			{
				object[] array = target as object[];
				if (array != null)
				{
					int num2 = (int)index;
					if (Math.Abs(num2) >= array.Length)
					{
						return null;
					}
					if (num2 < 0)
					{
						return array[array.Length + num2];
					}
					return array[num2];
				}
				else
				{
					Hashtable hashtable = target as Hashtable;
					if (hashtable != null)
					{
						return hashtable[index];
					}
					throw new Exception();
				}
			}
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x0014C640 File Offset: 0x0014A840
		private object GetIndexedValueFromTarget(object target, object index)
		{
			object[] array = index as object[];
			if (array == null)
			{
				return this.GetSingleValueFromTarget(target, index);
			}
			return (from i in array
			select this.GetSingleValueFromTarget(target, i)).ToArray<object>();
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0014C690 File Offset: 0x0014A890
		public object VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			object obj = indexExpressionAst.Index.Accept(this);
			object obj2 = indexExpressionAst.Target.Accept(this);
			if (obj == null || obj2 == null)
			{
				throw new ArgumentNullException("indexExpressionAst");
			}
			return this.GetIndexedValueFromTarget(obj2, obj);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x0014C6D0 File Offset: 0x0014A8D0
		public object VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			object[] array = new object[expandableStringExpressionAst.NestedExpressions.Count];
			string text = null;
			if (GetSafeValueVisitor._context != null)
			{
				text = (GetSafeValueVisitor._context.SessionState.PSVariable.GetValue("OFS") as string);
			}
			if (text == null)
			{
				text = " ";
			}
			for (int i = 0; i < array.Length; i++)
			{
				object obj = expandableStringExpressionAst.NestedExpressions[i].Accept(this);
				object[] array2 = obj as object[];
				if (array2 != null)
				{
					object[] array3 = new object[array2.Length];
					for (int j = 0; j < array3.Length; j++)
					{
						object[] array4 = array2[j] as object[];
						if (array4 != null)
						{
							array3[j] = string.Join(text, array4);
						}
						else
						{
							array3[j] = array2[j];
						}
					}
					array[i] = string.Join(text, array3);
				}
				else
				{
					array[i] = obj;
				}
			}
			return StringUtil.Format(expandableStringExpressionAst.FormatExpression, array);
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x0014C7B4 File Offset: 0x0014A9B4
		public object VisitStatementBlock(StatementBlockAst statementBlockAst)
		{
			ArrayList arrayList = new ArrayList();
			foreach (StatementAst statementAst in statementBlockAst.Statements)
			{
				if (statementAst == null)
				{
					throw PSTraceSource.NewArgumentException("ast");
				}
				arrayList.Add(statementAst.Accept(this));
			}
			return arrayList.ToArray();
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x0014C824 File Offset: 0x0014AA24
		public object VisitPipeline(PipelineAst pipelineAst)
		{
			ExpressionAst pureExpression = pipelineAst.GetPureExpression();
			if (pureExpression != null)
			{
				return pureExpression.Accept(this);
			}
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x0014C84D File Offset: 0x0014AA4D
		public object VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x0014C859 File Offset: 0x0014AA59
		public object VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			if (GetSafeValueVisitor._context != null)
			{
				return Compiler.GetExpressionValue(unaryExpressionAst, true, GetSafeValueVisitor._context, null);
			}
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x0014C87A File Offset: 0x0014AA7A
		public object VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			if (GetSafeValueVisitor._context != null)
			{
				return Compiler.GetExpressionValue(convertExpressionAst, true, GetSafeValueVisitor._context, null);
			}
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x0014C89B File Offset: 0x0014AA9B
		public object VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			return constantExpressionAst.Value;
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x0014C8A3 File Offset: 0x0014AAA3
		public object VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			return stringConstantExpressionAst.Value;
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x0014C8AB File Offset: 0x0014AAAB
		public object VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			return subExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x0014C8B9 File Offset: 0x0014AAB9
		public object VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			return usingExpressionAst.SubExpression.Accept(this);
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x0014C8C7 File Offset: 0x0014AAC7
		public object VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			if (GetSafeValueVisitor._context != null)
			{
				return VariableOps.GetVariableValue(variableExpressionAst.VariablePath, GetSafeValueVisitor._context, variableExpressionAst);
			}
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x0014C8EC File Offset: 0x0014AAEC
		public object VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x0014C8F8 File Offset: 0x0014AAF8
		public object VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			throw PSTraceSource.NewArgumentException("ast");
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x0014C904 File Offset: 0x0014AB04
		public object VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			object obj = arrayExpressionAst.SubExpression.Accept(this);
			if (!(obj is object[]))
			{
				return new object[]
				{
					obj
				};
			}
			return obj;
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x0014C934 File Offset: 0x0014AB34
		public object VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			ArrayList arrayList = new ArrayList();
			foreach (ExpressionAst expressionAst in arrayLiteralAst.Elements)
			{
				arrayList.Add(expressionAst.Accept(this));
			}
			return arrayList.ToArray();
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x0014C994 File Offset: 0x0014AB94
		public object VisitHashtable(HashtableAst hashtableAst)
		{
			Hashtable hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
			foreach (Tuple<ExpressionAst, StatementAst> tuple in hashtableAst.KeyValuePairs)
			{
				object key = tuple.Item1.Accept(this);
				object value = tuple.Item2.Accept(this);
				hashtable.Add(key, value);
			}
			return hashtable;
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x0014CA10 File Offset: 0x0014AC10
		public object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return ScriptBlock.Create(scriptBlockExpressionAst.Extent.Text);
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x0014CA22 File Offset: 0x0014AC22
		public object VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			return parenExpressionAst.Pipeline.Accept(this);
		}

		// Token: 0x04001F49 RID: 8009
		private static ExecutionContext _context;
	}
}
