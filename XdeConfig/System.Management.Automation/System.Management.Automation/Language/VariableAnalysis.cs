using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x020005ED RID: 1517
	internal class VariableAnalysis : ICustomAstVisitor
	{
		// Token: 0x060040F3 RID: 16627 RVA: 0x00158B4D File Offset: 0x00156D4D
		internal static string GetUnaliasedVariableName(string varName)
		{
			if (!varName.Equals("PSItem", StringComparison.OrdinalIgnoreCase))
			{
				return varName;
			}
			return "_";
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x00158B64 File Offset: 0x00156D64
		internal static string GetUnaliasedVariableName(VariablePath varPath)
		{
			return VariableAnalysis.GetUnaliasedVariableName(varPath.UnqualifiedPath);
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x00158B71 File Offset: 0x00156D71
		internal static void NoteAllScopeVariable(string variableName)
		{
			VariableAnalysis._allScopeVariables.GetOrAdd(variableName, true);
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x00158B93 File Offset: 0x00156D93
		internal static bool AnyVariablesCouldBeAllScope(Dictionary<string, int> variableNames)
		{
			return variableNames.Any((KeyValuePair<string, int> keyValuePair) => VariableAnalysis._allScopeVariables.ContainsKey(keyValuePair.Key));
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x00158BB8 File Offset: 0x00156DB8
		internal static Tuple<Type, Dictionary<string, int>> AnalyzeExpression(ExpressionAst exprAst)
		{
			return new VariableAnalysis().AnalyzeImpl(exprAst);
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00158BC8 File Offset: 0x00156DC8
		private Tuple<Type, Dictionary<string, int>> AnalyzeImpl(ExpressionAst exprAst)
		{
			this._variables = FindAllVariablesVisitor.Visit(exprAst);
			this._disableOptimizations = true;
			this.Init();
			this._localsAllocated = SpecialVariables.AutomaticVariables.Length;
			this._currentBlock = this._entryBlock;
			exprAst.Accept(this);
			this._currentBlock.FlowsTo(this._exitBlock);
			return this.FinishAnalysis(false);
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x00158C27 File Offset: 0x00156E27
		internal static Tuple<Type, Dictionary<string, int>> AnalyzeTrap(TrapStatementAst trap)
		{
			return new VariableAnalysis().AnalyzeImpl(trap);
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x00158C34 File Offset: 0x00156E34
		private Tuple<Type, Dictionary<string, int>> AnalyzeImpl(TrapStatementAst trap)
		{
			this._variables = FindAllVariablesVisitor.Visit(trap);
			this._disableOptimizations = true;
			this.Init();
			this._localsAllocated = SpecialVariables.AutomaticVariables.Length;
			this._currentBlock = this._entryBlock;
			trap.Body.Accept(this);
			this._currentBlock.FlowsTo(this._exitBlock);
			return this.FinishAnalysis(false);
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x00158C98 File Offset: 0x00156E98
		private void Init()
		{
			this._entryBlock = VariableAnalysis.Block.NewEntryBlock();
			this._exitBlock = new VariableAnalysis.Block();
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x00158CB0 File Offset: 0x00156EB0
		internal static Tuple<Type, Dictionary<string, int>> Analyze(IParameterMetadataProvider ast, bool disableOptimizations, bool scriptCmdlet)
		{
			return new VariableAnalysis().AnalyzeImpl(ast, disableOptimizations, scriptCmdlet);
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x00158CDC File Offset: 0x00156EDC
		internal static bool AnalyzeMemberFunction(FunctionMemberAst ast)
		{
			VariableAnalysis variableAnalysis = new VariableAnalysis();
			variableAnalysis.AnalyzeImpl(ast, false, false);
			return variableAnalysis._exitBlock._predecessors.All((VariableAnalysis.Block b) => b._returns || b._throws || b._unreachable);
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x00158D28 File Offset: 0x00156F28
		private Tuple<Type, Dictionary<string, int>> AnalyzeImpl(IParameterMetadataProvider ast, bool disableOptimizations, bool scriptCmdlet)
		{
			this._variables = FindAllVariablesVisitor.Visit(ast, disableOptimizations, scriptCmdlet, out this._localsAllocated, out this._disableOptimizations);
			this.Init();
			if (ast.Parameters != null)
			{
				foreach (ParameterAst parameterAst in ast.Parameters)
				{
					VariablePath variablePath = parameterAst.Name.VariablePath;
					if (variablePath.IsAnyLocal())
					{
						bool flag = false;
						int num = -1;
						Type type = null;
						bool flag2 = false;
						foreach (AttributeBaseAst attributeBaseAst in parameterAst.Attributes)
						{
							if (attributeBaseAst is TypeConstraintAst)
							{
								num++;
								if (type == null)
								{
									type = attributeBaseAst.TypeName.GetReflectionType();
									if (type == null)
									{
										flag2 = true;
									}
								}
							}
							else
							{
								Type reflectionAttributeType = attributeBaseAst.TypeName.GetReflectionAttributeType();
								if (reflectionAttributeType == null)
								{
									flag2 = true;
								}
								else if (typeof(ValidateArgumentsAttribute).IsAssignableFrom(reflectionAttributeType) || typeof(ArgumentTransformationAttribute).IsAssignableFrom(reflectionAttributeType))
								{
									flag = true;
								}
							}
						}
						string unaliasedVariableName = VariableAnalysis.GetUnaliasedVariableName(variablePath);
						VariableAnalysisDetails variableAnalysisDetails = this._variables[unaliasedVariableName];
						variableAnalysisDetails.Assigned = true;
						Type type2;
						if ((type2 = type) == null)
						{
							type2 = (variableAnalysisDetails.Type ?? typeof(object));
						}
						type = type2;
						if ((flag || flag2 || num > 0 || typeof(PSReference).IsAssignableFrom(type) || VariableAnalysis.MustBeBoxed(type)) && !variableAnalysisDetails.Automatic && !variableAnalysisDetails.PreferenceVariable)
						{
							variableAnalysisDetails.LocalTupleIndex = -2;
						}
						this._entryBlock.AddAst(new VariableAnalysis.AssignmentTarget(unaliasedVariableName, type));
					}
				}
			}
			ast.Body.Accept(this);
			return this.FinishAnalysis(scriptCmdlet);
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x00158F60 File Offset: 0x00157160
		private Tuple<Type, Dictionary<string, int>> FinishAnalysis(bool scriptCmdlet = false)
		{
			List<VariableAnalysis.Block> list = VariableAnalysis.Block.GenerateReverseDepthFirstOrder(this._entryBlock);
			BitArray bitArray = new BitArray(this._variables.Count);
			list[0]._visitData = bitArray;
			this.AnalyzeBlock(bitArray, list[0]);
			for (int i = 1; i < list.Count; i++)
			{
				VariableAnalysis.Block block = list[i];
				bitArray = new BitArray(this._variables.Count);
				bitArray.SetAll(true);
				block._visitData = bitArray;
				int num = 0;
				foreach (VariableAnalysis.Block block2 in block._predecessors)
				{
					if (block2._visitData != null)
					{
						num++;
						bitArray.And((BitArray)block2._visitData);
					}
				}
				this.AnalyzeBlock(bitArray, block);
			}
			foreach (VariableAnalysisDetails variableAnalysisDetails in this._variables.Values)
			{
				if (variableAnalysisDetails.LocalTupleIndex == -2)
				{
					foreach (Ast ast in variableAnalysisDetails.AssociatedAsts)
					{
						VariableAnalysis.FixTupleIndex(ast, -2);
						VariableAnalysis.FixAssigned(ast, variableAnalysisDetails);
					}
				}
			}
			VariableAnalysisDetails[] array = (from details in this._variables.Values
			where details.LocalTupleIndex >= 0
			orderby details.LocalTupleIndex
			select details).ToArray<VariableAnalysisDetails>();
			Dictionary<string, int> dictionary = new Dictionary<string, int>(0, StringComparer.OrdinalIgnoreCase);
			for (int j = 0; j < array.Length; j++)
			{
				VariableAnalysisDetails variableAnalysisDetails2 = array[j];
				string name = variableAnalysisDetails2.Name;
				dictionary.Add(name, j);
				if (variableAnalysisDetails2.LocalTupleIndex != j)
				{
					foreach (Ast ast2 in variableAnalysisDetails2.AssociatedAsts)
					{
						VariableAnalysis.FixTupleIndex(ast2, j);
					}
				}
			}
			Type item = MutableTuple.MakeTupleType((from l in array
			select l.Type).ToArray<Type>());
			return Tuple.Create<Type, Dictionary<string, int>>(item, dictionary);
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x0015920C File Offset: 0x0015740C
		private static bool MustBeBoxed(Type type)
		{
			return type.GetTypeInfo().IsValueType && PSVariableAssignmentBinder.IsValueTypeMutable(type) && typeof(SwitchParameter) != type;
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x00159238 File Offset: 0x00157438
		private static void FixTupleIndex(Ast ast, int newIndex)
		{
			VariableExpressionAst variableExpressionAst = ast as VariableExpressionAst;
			if (variableExpressionAst != null)
			{
				if (variableExpressionAst.TupleIndex != -2)
				{
					variableExpressionAst.TupleIndex = newIndex;
					return;
				}
			}
			else
			{
				DataStatementAst dataStatementAst = ast as DataStatementAst;
				if (dataStatementAst != null && dataStatementAst.TupleIndex != -2)
				{
					dataStatementAst.TupleIndex = newIndex;
				}
			}
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x0015927C File Offset: 0x0015747C
		private static void FixAssigned(Ast ast, VariableAnalysisDetails details)
		{
			VariableExpressionAst variableExpressionAst = ast as VariableExpressionAst;
			if (variableExpressionAst != null && details.Assigned)
			{
				variableExpressionAst.Assigned = true;
			}
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x001592A4 File Offset: 0x001574A4
		private void AnalyzeBlock(BitArray assignedBitArray, VariableAnalysis.Block block)
		{
			foreach (Ast ast in block._asts)
			{
				VariableExpressionAst variableExpressionAst = ast as VariableExpressionAst;
				if (variableExpressionAst != null)
				{
					VariablePath variablePath = variableExpressionAst.VariablePath;
					if (variablePath.IsAnyLocal())
					{
						string unaliasedVariableName = VariableAnalysis.GetUnaliasedVariableName(variablePath);
						VariableAnalysisDetails variableAnalysisDetails = this._variables[unaliasedVariableName];
						if (variableAnalysisDetails.Automatic)
						{
							variableExpressionAst.TupleIndex = variableAnalysisDetails.LocalTupleIndex;
							variableExpressionAst.Automatic = true;
						}
						else
						{
							variableExpressionAst.TupleIndex = ((assignedBitArray[variableAnalysisDetails.BitIndex] && !variableAnalysisDetails.PreferenceVariable) ? variableAnalysisDetails.LocalTupleIndex : -2);
						}
					}
				}
				else
				{
					VariableAnalysis.AssignmentTarget assignmentTarget = ast as VariableAnalysis.AssignmentTarget;
					if (assignmentTarget != null)
					{
						if (assignmentTarget._targetAst != null)
						{
							this.CheckLHSAssign(assignmentTarget._targetAst, assignedBitArray);
						}
						else
						{
							this.CheckLHSAssignVar(assignmentTarget._variableName, assignedBitArray, assignmentTarget._type);
						}
					}
					else
					{
						DataStatementAst dataStatementAst = ast as DataStatementAst;
						if (dataStatementAst != null)
						{
							VariableAnalysisDetails variableAnalysisDetails2 = this.CheckLHSAssignVar(dataStatementAst.Variable, assignedBitArray, typeof(object));
							dataStatementAst.TupleIndex = variableAnalysisDetails2.LocalTupleIndex;
							variableAnalysisDetails2.AssociatedAsts.Add(dataStatementAst);
						}
					}
				}
			}
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x00159400 File Offset: 0x00157600
		private void CheckLHSAssign(ExpressionAst lhs, BitArray assignedBitArray)
		{
			ConvertExpressionAst convertExpressionAst = lhs as ConvertExpressionAst;
			Type type = null;
			if (convertExpressionAst != null)
			{
				lhs = convertExpressionAst.Child;
				type = convertExpressionAst.StaticType;
			}
			VariableExpressionAst variableExpressionAst = lhs as VariableExpressionAst;
			VariablePath variablePath = variableExpressionAst.VariablePath;
			if (variablePath.IsAnyLocal())
			{
				string unaliasedVariableName = VariableAnalysis.GetUnaliasedVariableName(variablePath);
				if (type == null && (unaliasedVariableName.Equals("foreach", StringComparison.OrdinalIgnoreCase) || unaliasedVariableName.Equals("switch", StringComparison.OrdinalIgnoreCase)))
				{
					type = typeof(object);
				}
				VariableAnalysisDetails variableAnalysisDetails = this.CheckLHSAssignVar(unaliasedVariableName, assignedBitArray, type);
				variableAnalysisDetails.AssociatedAsts.Add(variableExpressionAst);
				variableAnalysisDetails.Assigned = true;
				variableExpressionAst.TupleIndex = variableAnalysisDetails.LocalTupleIndex;
				variableExpressionAst.Automatic = variableAnalysisDetails.Automatic;
				return;
			}
			variableExpressionAst.TupleIndex = -2;
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x001594BC File Offset: 0x001576BC
		private VariableAnalysisDetails CheckLHSAssignVar(string variableName, BitArray assignedBitArray, Type convertType)
		{
			VariableAnalysisDetails variableAnalysisDetails = this._variables[variableName];
			if (variableAnalysisDetails.LocalTupleIndex == -1)
			{
				variableAnalysisDetails.LocalTupleIndex = ((this._disableOptimizations || VariableAnalysis._allScopeVariables.ContainsKey(variableName)) ? -2 : this._localsAllocated++);
			}
			if (convertType != null && VariableAnalysis.MustBeBoxed(convertType))
			{
				variableAnalysisDetails.LocalTupleIndex = -2;
			}
			Type type = variableAnalysisDetails.Type;
			if (type == null)
			{
				variableAnalysisDetails.Type = (convertType ?? typeof(object));
			}
			else
			{
				if (!assignedBitArray[variableAnalysisDetails.BitIndex] && convertType == null)
				{
					convertType = typeof(object);
				}
				if (convertType != null && !convertType.Equals(type))
				{
					if (variableAnalysisDetails.Automatic || variableAnalysisDetails.PreferenceVariable)
					{
						variableAnalysisDetails.Type = typeof(object);
					}
					else
					{
						variableAnalysisDetails.LocalTupleIndex = -2;
					}
				}
			}
			assignedBitArray.Set(variableAnalysisDetails.BitIndex, true);
			return variableAnalysisDetails;
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x001595BC File Offset: 0x001577BC
		public object VisitErrorStatement(ErrorStatementAst errorStatementAst)
		{
			return null;
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x001595BF File Offset: 0x001577BF
		public object VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
		{
			return null;
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x001595C4 File Offset: 0x001577C4
		public object VisitScriptBlock(ScriptBlockAst scriptBlockAst)
		{
			this._currentBlock = this._entryBlock;
			if (scriptBlockAst.DynamicParamBlock != null)
			{
				scriptBlockAst.DynamicParamBlock.Accept(this);
			}
			if (scriptBlockAst.BeginBlock != null)
			{
				scriptBlockAst.BeginBlock.Accept(this);
			}
			if (scriptBlockAst.ProcessBlock != null)
			{
				scriptBlockAst.ProcessBlock.Accept(this);
			}
			if (scriptBlockAst.EndBlock != null)
			{
				scriptBlockAst.EndBlock.Accept(this);
			}
			this._currentBlock.FlowsTo(this._exitBlock);
			return null;
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x00159643 File Offset: 0x00157843
		public object VisitParamBlock(ParamBlockAst paramBlockAst)
		{
			return null;
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x00159646 File Offset: 0x00157846
		public object VisitNamedBlock(NamedBlockAst namedBlockAst)
		{
			return this.VisitStatementBlock(namedBlockAst.Statements);
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x00159654 File Offset: 0x00157854
		public object VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			return null;
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x00159657 File Offset: 0x00157857
		public object VisitAttribute(AttributeAst attributeAst)
		{
			return null;
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x0015965A File Offset: 0x0015785A
		public object VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
		{
			return null;
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x0015965D File Offset: 0x0015785D
		public object VisitParameter(ParameterAst parameterAst)
		{
			return null;
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x00159660 File Offset: 0x00157860
		public object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			return null;
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x00159663 File Offset: 0x00157863
		public object VisitStatementBlock(StatementBlockAst statementBlockAst)
		{
			return this.VisitStatementBlock(statementBlockAst.Statements);
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x00159674 File Offset: 0x00157874
		private object VisitStatementBlock(ReadOnlyCollection<StatementAst> statements)
		{
			foreach (StatementAst statementAst in statements)
			{
				statementAst.Accept(this);
			}
			return null;
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x001596C0 File Offset: 0x001578C0
		public object VisitIfStatement(IfStatementAst ifStmtAst)
		{
			VariableAnalysis.Block block = new VariableAnalysis.Block();
			if (ifStmtAst.ElseClause == null)
			{
				this._currentBlock.FlowsTo(block);
			}
			int count = ifStmtAst.Clauses.Count;
			for (int i = 0; i < count; i++)
			{
				Tuple<PipelineBaseAst, StatementBlockAst> tuple = ifStmtAst.Clauses[i];
				bool flag = i == count - 1 && ifStmtAst.ElseClause == null;
				VariableAnalysis.Block block2 = new VariableAnalysis.Block();
				VariableAnalysis.Block block3 = flag ? block : new VariableAnalysis.Block();
				tuple.Item1.Accept(this);
				this._currentBlock.FlowsTo(block2);
				this._currentBlock.FlowsTo(block3);
				this._currentBlock = block2;
				tuple.Item2.Accept(this);
				this._currentBlock.FlowsTo(block);
				this._currentBlock = block3;
			}
			if (ifStmtAst.ElseClause != null)
			{
				ifStmtAst.ElseClause.Accept(this);
				this._currentBlock.FlowsTo(block);
			}
			this._currentBlock = block;
			return null;
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x001597B4 File Offset: 0x001579B4
		public object VisitTrap(TrapStatementAst trapStatementAst)
		{
			trapStatementAst.Body.Accept(this);
			return null;
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x00159950 File Offset: 0x00157B50
		public object VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			VariableAnalysisDetails variableAnalysisDetails = this._variables["switch"];
			if (variableAnalysisDetails.LocalTupleIndex == -1 && !this._disableOptimizations)
			{
				variableAnalysisDetails.LocalTupleIndex = this._localsAllocated++;
			}
			Action generateCondition = delegate()
			{
				switchStatementAst.Condition.Accept(this);
				this._currentBlock.AddAst(new VariableAnalysis.AssignmentTarget("switch", typeof(IEnumerator)));
			};
			Action generateLoopBody = delegate()
			{
				bool flag = switchStatementAst.Default != null;
				VariableAnalysis.Block block = new VariableAnalysis.Block();
				int count = switchStatementAst.Clauses.Count;
				for (int i = 0; i < count; i++)
				{
					Tuple<ExpressionAst, StatementBlockAst> tuple = switchStatementAst.Clauses[i];
					VariableAnalysis.Block block2 = new VariableAnalysis.Block();
					bool flag2 = i == count - 1 && !flag;
					VariableAnalysis.Block block3 = flag2 ? block : new VariableAnalysis.Block();
					tuple.Item1.Accept(this);
					this._currentBlock.FlowsTo(block3);
					this._currentBlock.FlowsTo(block2);
					this._currentBlock = block2;
					tuple.Item2.Accept(this);
					if (!flag2)
					{
						this._currentBlock.FlowsTo(block3);
						this._currentBlock = block3;
					}
				}
				if (flag)
				{
					this._currentBlock.FlowsTo(block);
					switchStatementAst.Default.Accept(this);
				}
				this._currentBlock.FlowsTo(block);
				this._currentBlock = block;
			};
			this.GenerateWhileLoop(switchStatementAst.Label, generateCondition, generateLoopBody, null);
			return null;
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x001599DB File Offset: 0x00157BDB
		public object VisitDataStatement(DataStatementAst dataStatementAst)
		{
			dataStatementAst.Body.Accept(this);
			if (dataStatementAst.Variable != null)
			{
				this._currentBlock.AddAst(dataStatementAst);
			}
			return null;
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x00159A00 File Offset: 0x00157C00
		private void GenerateWhileLoop(string loopLabel, Action generateCondition, Action generateLoopBody, Ast continueAction = null)
		{
			VariableAnalysis.Block block = new VariableAnalysis.Block();
			if (continueAction != null)
			{
				VariableAnalysis.Block block2 = new VariableAnalysis.Block();
				this._currentBlock.FlowsTo(block2);
				this._currentBlock = block;
				continueAction.Accept(this);
				this._currentBlock.FlowsTo(block2);
				this._currentBlock = block2;
			}
			else
			{
				this._currentBlock.FlowsTo(block);
				this._currentBlock = block;
			}
			VariableAnalysis.Block block3 = new VariableAnalysis.Block();
			VariableAnalysis.Block block4 = new VariableAnalysis.Block();
			if (generateCondition != null)
			{
				generateCondition();
				this._currentBlock.FlowsTo(block4);
			}
			this._loopTargets.Add(new VariableAnalysis.LoopGotoTargets(loopLabel ?? "", block4, block));
			this._currentBlock.FlowsTo(block3);
			this._currentBlock = block3;
			generateLoopBody();
			this._currentBlock.FlowsTo(block);
			this._currentBlock = block4;
			this._loopTargets.RemoveAt(this._loopTargets.Count - 1);
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x00159AE4 File Offset: 0x00157CE4
		private void GenerateDoLoop(LoopStatementAst loopStatement)
		{
			VariableAnalysis.Block block = new VariableAnalysis.Block();
			VariableAnalysis.Block block2 = new VariableAnalysis.Block();
			VariableAnalysis.Block block3 = new VariableAnalysis.Block();
			VariableAnalysis.Block block4 = new VariableAnalysis.Block();
			this._loopTargets.Add(new VariableAnalysis.LoopGotoTargets(loopStatement.Label ?? "", block3, block));
			this._currentBlock.FlowsTo(block2);
			this._currentBlock = block2;
			loopStatement.Body.Accept(this);
			this._currentBlock.FlowsTo(block);
			this._currentBlock = block;
			loopStatement.Condition.Accept(this);
			this._currentBlock.FlowsTo(block3);
			this._currentBlock.FlowsTo(block4);
			this._currentBlock = block4;
			this._currentBlock.FlowsTo(block2);
			this._currentBlock = block3;
			this._loopTargets.RemoveAt(this._loopTargets.Count - 1);
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x00159C54 File Offset: 0x00157E54
		public object VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			VariableAnalysisDetails variableAnalysisDetails = this._variables["foreach"];
			if (variableAnalysisDetails.LocalTupleIndex == -1 && !this._disableOptimizations)
			{
				variableAnalysisDetails.LocalTupleIndex = this._localsAllocated++;
			}
			VariableAnalysis.Block afterFor = new VariableAnalysis.Block();
			Action generateCondition = delegate()
			{
				forEachStatementAst.Condition.Accept(this);
				this._currentBlock.FlowsTo(afterFor);
				this._currentBlock.AddAst(new VariableAnalysis.AssignmentTarget("foreach", typeof(IEnumerator)));
				this._currentBlock.AddAst(new VariableAnalysis.AssignmentTarget(forEachStatementAst.Variable));
			};
			this.GenerateWhileLoop(forEachStatementAst.Label, generateCondition, delegate
			{
				forEachStatementAst.Body.Accept(this);
			}, null);
			this._currentBlock.FlowsTo(afterFor);
			this._currentBlock = afterFor;
			return null;
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x00159D03 File Offset: 0x00157F03
		public object VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
		{
			this.GenerateDoLoop(doWhileStatementAst);
			return null;
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x00159D0D File Offset: 0x00157F0D
		public object VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
		{
			this.GenerateDoLoop(doUntilStatementAst);
			return null;
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x00159D54 File Offset: 0x00157F54
		public object VisitForStatement(ForStatementAst forStatementAst)
		{
			if (forStatementAst.Initializer != null)
			{
				forStatementAst.Initializer.Accept(this);
			}
			Action generateCondition = (forStatementAst.Condition != null) ? delegate()
			{
				forStatementAst.Condition.Accept(this);
			} : null;
			this.GenerateWhileLoop(forStatementAst.Label, generateCondition, delegate
			{
				forStatementAst.Body.Accept(this);
			}, forStatementAst.Iterator);
			return null;
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x00159E18 File Offset: 0x00158018
		public object VisitWhileStatement(WhileStatementAst whileStatementAst)
		{
			this.GenerateWhileLoop(whileStatementAst.Label, delegate
			{
				whileStatementAst.Condition.Accept(this);
			}, delegate
			{
				whileStatementAst.Body.Accept(this);
			}, null);
			return null;
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x00159E64 File Offset: 0x00158064
		public object VisitCatchClause(CatchClauseAst catchClauseAst)
		{
			catchClauseAst.Body.Accept(this);
			return null;
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x00159E74 File Offset: 0x00158074
		public object VisitTryStatement(TryStatementAst tryStatementAst)
		{
			VariableAnalysis.Block currentBlock = this._currentBlock;
			this._currentBlock = new VariableAnalysis.Block();
			currentBlock.FlowsTo(this._currentBlock);
			tryStatementAst.Body.Accept(this);
			VariableAnalysis.Block currentBlock2 = this._currentBlock;
			VariableAnalysis.Block block = (tryStatementAst.Finally == null) ? null : new VariableAnalysis.Block();
			VariableAnalysis.Block block2 = new VariableAnalysis.Block();
			bool flag = false;
			foreach (CatchClauseAst catchClauseAst in tryStatementAst.CatchClauses)
			{
				if (catchClauseAst.IsCatchAll)
				{
					flag = true;
				}
				this._currentBlock = new VariableAnalysis.Block();
				currentBlock.FlowsTo(this._currentBlock);
				catchClauseAst.Accept(this);
				this._currentBlock.FlowsTo(block ?? block2);
			}
			if (block != null)
			{
				currentBlock2.FlowsTo(block);
				this._currentBlock = block;
				tryStatementAst.Finally.Accept(this);
				this._currentBlock.FlowsTo(block2);
				VariableAnalysis.Block currentBlock3 = this._currentBlock;
				if (!flag)
				{
					currentBlock.FlowsTo(block);
					VariableAnalysis.Block block3 = new VariableAnalysis.Block();
					currentBlock3.FlowsTo(block3);
					block3._throws = true;
					block3.FlowsTo(this._exitBlock);
				}
				currentBlock3.FlowsTo(block2);
			}
			else
			{
				currentBlock2.FlowsTo(block2);
			}
			this._currentBlock = block2;
			return null;
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x0015A004 File Offset: 0x00158204
		private void BreakOrContinue(ExpressionAst label, Func<VariableAnalysis.LoopGotoTargets, VariableAnalysis.Block> fieldSelector)
		{
			VariableAnalysis.Block block = null;
			if (label != null)
			{
				label.Accept(this);
				if (this._loopTargets.Any<VariableAnalysis.LoopGotoTargets>())
				{
					StringConstantExpressionAst labelStrAst = label as StringConstantExpressionAst;
					if (labelStrAst != null)
					{
						block = (from t in this._loopTargets
						where t.Label.Equals(labelStrAst.Value, StringComparison.OrdinalIgnoreCase)
						select fieldSelector(t)).LastOrDefault<VariableAnalysis.Block>();
					}
				}
			}
			else if (this._loopTargets.Count > 0)
			{
				block = fieldSelector(this._loopTargets.Last<VariableAnalysis.LoopGotoTargets>());
			}
			if (block == null)
			{
				this._currentBlock.FlowsTo(this._exitBlock);
				this._currentBlock._throws = true;
			}
			else
			{
				this._currentBlock.FlowsTo(block);
			}
			this._currentBlock = new VariableAnalysis.Block();
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x0015A105 File Offset: 0x00158305
		public object VisitBreakStatement(BreakStatementAst breakStatementAst)
		{
			this.BreakOrContinue(breakStatementAst.Label, (VariableAnalysis.LoopGotoTargets t) => t.BreakTarget);
			return null;
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x0015A139 File Offset: 0x00158339
		public object VisitContinueStatement(ContinueStatementAst continueStatementAst)
		{
			this.BreakOrContinue(continueStatementAst.Label, (VariableAnalysis.LoopGotoTargets t) => t.ContinueTarget);
			return null;
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x0015A168 File Offset: 0x00158368
		private VariableAnalysis.Block ControlFlowStatement(PipelineBaseAst pipelineAst)
		{
			if (pipelineAst != null)
			{
				pipelineAst.Accept(this);
			}
			this._currentBlock.FlowsTo(this._exitBlock);
			VariableAnalysis.Block currentBlock = this._currentBlock;
			this._currentBlock = new VariableAnalysis.Block();
			return currentBlock;
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x0015A1A4 File Offset: 0x001583A4
		public object VisitReturnStatement(ReturnStatementAst returnStatementAst)
		{
			this.ControlFlowStatement(returnStatementAst.Pipeline)._returns = true;
			return null;
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x0015A1B9 File Offset: 0x001583B9
		public object VisitExitStatement(ExitStatementAst exitStatementAst)
		{
			this.ControlFlowStatement(exitStatementAst.Pipeline)._throws = true;
			return null;
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0015A1CE File Offset: 0x001583CE
		public object VisitThrowStatement(ThrowStatementAst throwStatementAst)
		{
			this.ControlFlowStatement(throwStatementAst.Pipeline)._throws = true;
			return null;
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x0015A4A8 File Offset: 0x001586A8
		private static IEnumerable<ExpressionAst> GetAssignmentTargets(ExpressionAst expressionAst)
		{
			ParenExpressionAst parenExpr = expressionAst as ParenExpressionAst;
			if (parenExpr != null)
			{
				foreach (ExpressionAst e in VariableAnalysis.GetAssignmentTargets(parenExpr.Pipeline.GetPureExpression()))
				{
					yield return e;
				}
			}
			else
			{
				ArrayLiteralAst arrayLiteral = expressionAst as ArrayLiteralAst;
				if (arrayLiteral != null)
				{
					foreach (ExpressionAst e2 in arrayLiteral.Elements.SelectMany(new Func<ExpressionAst, IEnumerable<ExpressionAst>>(VariableAnalysis.GetAssignmentTargets)))
					{
						yield return e2;
					}
				}
				else
				{
					yield return expressionAst;
				}
			}
			yield break;
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x0015A4C8 File Offset: 0x001586C8
		public object VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			assignmentStatementAst.Right.Accept(this);
			foreach (ExpressionAst expressionAst in VariableAnalysis.GetAssignmentTargets(assignmentStatementAst.Left))
			{
				bool flag = false;
				int num = 0;
				ConvertExpressionAst convertExpressionAst = null;
				ExpressionAst expressionAst2 = expressionAst;
				while (expressionAst2 is AttributedExpressionAst)
				{
					num++;
					convertExpressionAst = (expressionAst2 as ConvertExpressionAst);
					if (convertExpressionAst == null)
					{
						flag = true;
					}
					expressionAst2 = ((AttributedExpressionAst)expressionAst2).Child;
				}
				if (expressionAst2 is VariableExpressionAst)
				{
					if (flag || num > 1 || (convertExpressionAst != null && convertExpressionAst.Type.TypeName.GetReflectionType() == null))
					{
						VariablePath variablePath = ((VariableExpressionAst)expressionAst2).VariablePath;
						if (variablePath.IsAnyLocal())
						{
							VariableAnalysisDetails variableAnalysisDetails = this._variables[VariableAnalysis.GetUnaliasedVariableName(variablePath)];
							variableAnalysisDetails.LocalTupleIndex = -2;
						}
					}
					if (!flag && num <= 1)
					{
						this._currentBlock.AddAst(new VariableAnalysis.AssignmentTarget(expressionAst));
					}
				}
				else
				{
					expressionAst.Accept(this);
				}
			}
			return null;
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x0015A5E0 File Offset: 0x001587E0
		public object VisitPipeline(PipelineAst pipelineAst)
		{
			bool flag = false;
			foreach (CommandBaseAst commandBaseAst in pipelineAst.PipelineElements)
			{
				commandBaseAst.Accept(this);
				if (commandBaseAst is CommandAst)
				{
					flag = true;
				}
				foreach (RedirectionAst redirectionAst in commandBaseAst.Redirections)
				{
					redirectionAst.Accept(this);
				}
			}
			if (flag && this._loopTargets.Any<VariableAnalysis.LoopGotoTargets>())
			{
				foreach (VariableAnalysis.LoopGotoTargets loopGotoTargets in this._loopTargets)
				{
					this._currentBlock.FlowsTo(loopGotoTargets.BreakTarget);
					this._currentBlock.FlowsTo(loopGotoTargets.ContinueTarget);
				}
				VariableAnalysis.Block block = new VariableAnalysis.Block();
				this._currentBlock.FlowsTo(block);
				this._currentBlock = block;
			}
			return null;
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x0015A70C File Offset: 0x0015890C
		public object VisitCommand(CommandAst commandAst)
		{
			foreach (CommandElementAst commandElementAst in commandAst.CommandElements)
			{
				commandElementAst.Accept(this);
			}
			return null;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x0015A75C File Offset: 0x0015895C
		public object VisitCommandExpression(CommandExpressionAst commandExpressionAst)
		{
			commandExpressionAst.Expression.Accept(this);
			return null;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x0015A76C File Offset: 0x0015896C
		public object VisitCommandParameter(CommandParameterAst commandParameterAst)
		{
			if (commandParameterAst.Argument != null)
			{
				commandParameterAst.Argument.Accept(this);
			}
			return null;
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x0015A784 File Offset: 0x00158984
		public object VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
		{
			fileRedirectionAst.Location.Accept(this);
			return null;
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x0015A794 File Offset: 0x00158994
		public object VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
		{
			return null;
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x0015A798 File Offset: 0x00158998
		public object VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
		{
			if (binaryExpressionAst.Operator == TokenKind.And || binaryExpressionAst.Operator == TokenKind.Or)
			{
				binaryExpressionAst.Left.Accept(this);
				VariableAnalysis.Block block = new VariableAnalysis.Block();
				VariableAnalysis.Block block2 = new VariableAnalysis.Block();
				this._currentBlock.FlowsTo(block);
				this._currentBlock.FlowsTo(block2);
				this._currentBlock = block2;
				binaryExpressionAst.Right.Accept(this);
				this._currentBlock.FlowsTo(block);
				this._currentBlock = block;
			}
			else
			{
				binaryExpressionAst.Left.Accept(this);
				binaryExpressionAst.Right.Accept(this);
			}
			return null;
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0015A82E File Offset: 0x00158A2E
		public object VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
		{
			unaryExpressionAst.Child.Accept(this);
			return null;
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x0015A83E File Offset: 0x00158A3E
		public object VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
		{
			convertExpressionAst.Child.Accept(this);
			return null;
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x0015A84E File Offset: 0x00158A4E
		public object VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
		{
			return null;
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x0015A851 File Offset: 0x00158A51
		public object VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
		{
			return null;
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x0015A854 File Offset: 0x00158A54
		public object VisitSubExpression(SubExpressionAst subExpressionAst)
		{
			subExpressionAst.SubExpression.Accept(this);
			return null;
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x0015A864 File Offset: 0x00158A64
		public object VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			return null;
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x0015A868 File Offset: 0x00158A68
		public object VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			VariablePath variablePath = variableExpressionAst.VariablePath;
			if (variablePath.IsAnyLocal())
			{
				VariableAnalysisDetails variableAnalysisDetails = this._variables[VariableAnalysis.GetUnaliasedVariableName(variablePath)];
				if (variableAnalysisDetails.LocalTupleIndex != -1)
				{
					variableExpressionAst.TupleIndex = (variableAnalysisDetails.PreferenceVariable ? -2 : variableAnalysisDetails.LocalTupleIndex);
					variableExpressionAst.Automatic = variableAnalysisDetails.Automatic;
				}
				else
				{
					this._currentBlock.AddAst(variableExpressionAst);
				}
				variableAnalysisDetails.AssociatedAsts.Add(variableExpressionAst);
			}
			else
			{
				variableExpressionAst.TupleIndex = -2;
			}
			return null;
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x0015A8E8 File Offset: 0x00158AE8
		public object VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			return null;
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x0015A8EB File Offset: 0x00158AEB
		public object VisitMemberExpression(MemberExpressionAst memberExpressionAst)
		{
			memberExpressionAst.Expression.Accept(this);
			memberExpressionAst.Member.Accept(this);
			return null;
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x0015A908 File Offset: 0x00158B08
		public object VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
		{
			invokeMemberExpressionAst.Expression.Accept(this);
			invokeMemberExpressionAst.Member.Accept(this);
			if (invokeMemberExpressionAst.Arguments != null)
			{
				foreach (ExpressionAst expressionAst in invokeMemberExpressionAst.Arguments)
				{
					expressionAst.Accept(this);
				}
			}
			return null;
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x0015A97C File Offset: 0x00158B7C
		public object VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
		{
			arrayExpressionAst.SubExpression.Accept(this);
			return null;
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x0015A98C File Offset: 0x00158B8C
		public object VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
		{
			foreach (ExpressionAst expressionAst in arrayLiteralAst.Elements)
			{
				expressionAst.Accept(this);
			}
			return null;
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x0015A9DC File Offset: 0x00158BDC
		public object VisitHashtable(HashtableAst hashtableAst)
		{
			foreach (Tuple<ExpressionAst, StatementAst> tuple in hashtableAst.KeyValuePairs)
			{
				tuple.Item1.Accept(this);
				tuple.Item2.Accept(this);
			}
			return null;
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x0015AA40 File Offset: 0x00158C40
		public object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return null;
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x0015AA43 File Offset: 0x00158C43
		public object VisitParenExpression(ParenExpressionAst parenExpressionAst)
		{
			parenExpressionAst.Pipeline.Accept(this);
			return null;
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x0015AA54 File Offset: 0x00158C54
		public object VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
		{
			foreach (ExpressionAst expressionAst in expandableStringExpressionAst.NestedExpressions)
			{
				expressionAst.Accept(this);
			}
			return null;
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x0015AAA4 File Offset: 0x00158CA4
		public object VisitIndexExpression(IndexExpressionAst indexExpressionAst)
		{
			indexExpressionAst.Target.Accept(this);
			indexExpressionAst.Index.Accept(this);
			return null;
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x0015AAC1 File Offset: 0x00158CC1
		public object VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
		{
			attributedExpressionAst.Child.Accept(this);
			return null;
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x0015AAD1 File Offset: 0x00158CD1
		public object VisitBlockStatement(BlockStatementAst blockStatementAst)
		{
			blockStatementAst.Body.Accept(this);
			return null;
		}

		// Token: 0x0400209D RID: 8349
		internal const int Unanalyzed = -1;

		// Token: 0x0400209E RID: 8350
		internal const int ForceDynamic = -2;

		// Token: 0x0400209F RID: 8351
		private static readonly ConcurrentDictionary<string, bool> _allScopeVariables = new ConcurrentDictionary<string, bool>(1, 16, StringComparer.OrdinalIgnoreCase);

		// Token: 0x040020A0 RID: 8352
		private Dictionary<string, VariableAnalysisDetails> _variables;

		// Token: 0x040020A1 RID: 8353
		private VariableAnalysis.Block _entryBlock;

		// Token: 0x040020A2 RID: 8354
		private VariableAnalysis.Block _exitBlock;

		// Token: 0x040020A3 RID: 8355
		private VariableAnalysis.Block _currentBlock;

		// Token: 0x040020A4 RID: 8356
		private bool _disableOptimizations;

		// Token: 0x040020A5 RID: 8357
		private readonly List<VariableAnalysis.LoopGotoTargets> _loopTargets = new List<VariableAnalysis.LoopGotoTargets>();

		// Token: 0x040020A6 RID: 8358
		private int _localsAllocated;

		// Token: 0x020005EE RID: 1518
		private class LoopGotoTargets
		{
			// Token: 0x0600414B RID: 16715 RVA: 0x0015AB08 File Offset: 0x00158D08
			internal LoopGotoTargets(string label, VariableAnalysis.Block breakTarget, VariableAnalysis.Block continueTarget)
			{
				this.Label = label;
				this.BreakTarget = breakTarget;
				this.ContinueTarget = continueTarget;
			}

			// Token: 0x17000DF7 RID: 3575
			// (get) Token: 0x0600414C RID: 16716 RVA: 0x0015AB25 File Offset: 0x00158D25
			// (set) Token: 0x0600414D RID: 16717 RVA: 0x0015AB2D File Offset: 0x00158D2D
			internal string Label { get; private set; }

			// Token: 0x17000DF8 RID: 3576
			// (get) Token: 0x0600414E RID: 16718 RVA: 0x0015AB36 File Offset: 0x00158D36
			// (set) Token: 0x0600414F RID: 16719 RVA: 0x0015AB3E File Offset: 0x00158D3E
			internal VariableAnalysis.Block BreakTarget { get; private set; }

			// Token: 0x17000DF9 RID: 3577
			// (get) Token: 0x06004150 RID: 16720 RVA: 0x0015AB47 File Offset: 0x00158D47
			// (set) Token: 0x06004151 RID: 16721 RVA: 0x0015AB4F File Offset: 0x00158D4F
			internal VariableAnalysis.Block ContinueTarget { get; private set; }
		}

		// Token: 0x020005EF RID: 1519
		private class Block
		{
			// Token: 0x17000DFA RID: 3578
			// (get) Token: 0x06004152 RID: 16722 RVA: 0x0015AB58 File Offset: 0x00158D58
			// (set) Token: 0x06004153 RID: 16723 RVA: 0x0015AB60 File Offset: 0x00158D60
			internal bool _unreachable { get; private set; }

			// Token: 0x06004154 RID: 16724 RVA: 0x0015AB69 File Offset: 0x00158D69
			public Block()
			{
				this._unreachable = true;
			}

			// Token: 0x06004155 RID: 16725 RVA: 0x0015AB99 File Offset: 0x00158D99
			public static VariableAnalysis.Block NewEntryBlock()
			{
				return new VariableAnalysis.Block(false);
			}

			// Token: 0x06004156 RID: 16726 RVA: 0x0015ABA1 File Offset: 0x00158DA1
			private Block(bool unreachable)
			{
				this._unreachable = unreachable;
			}

			// Token: 0x06004157 RID: 16727 RVA: 0x0015ABD1 File Offset: 0x00158DD1
			internal void FlowsTo(VariableAnalysis.Block next)
			{
				if (this._successors.IndexOf(next) < 0)
				{
					if (!this._unreachable)
					{
						next._unreachable = false;
					}
					this._successors.Add(next);
					next._predecessors.Add(this);
				}
			}

			// Token: 0x06004158 RID: 16728 RVA: 0x0015AC09 File Offset: 0x00158E09
			internal void AddAst(Ast ast)
			{
				this._asts.Add(ast);
			}

			// Token: 0x06004159 RID: 16729 RVA: 0x0015AC18 File Offset: 0x00158E18
			internal static List<VariableAnalysis.Block> GenerateReverseDepthFirstOrder(VariableAnalysis.Block block)
			{
				List<VariableAnalysis.Block> list = new List<VariableAnalysis.Block>();
				VariableAnalysis.Block.VisitDepthFirstOrder(block, list);
				list.Reverse();
				for (int i = 0; i < list.Count; i++)
				{
					list[i]._visitData = null;
				}
				return list;
			}

			// Token: 0x0600415A RID: 16730 RVA: 0x0015AC58 File Offset: 0x00158E58
			private static void VisitDepthFirstOrder(VariableAnalysis.Block block, List<VariableAnalysis.Block> visitData)
			{
				if (object.ReferenceEquals(block._visitData, visitData))
				{
					return;
				}
				block._visitData = visitData;
				foreach (VariableAnalysis.Block block2 in block._successors)
				{
					VariableAnalysis.Block.VisitDepthFirstOrder(block2, visitData);
				}
				visitData.Add(block);
			}

			// Token: 0x040020B1 RID: 8369
			internal readonly List<Ast> _asts = new List<Ast>();

			// Token: 0x040020B2 RID: 8370
			private readonly List<VariableAnalysis.Block> _successors = new List<VariableAnalysis.Block>();

			// Token: 0x040020B3 RID: 8371
			internal readonly List<VariableAnalysis.Block> _predecessors = new List<VariableAnalysis.Block>();

			// Token: 0x040020B4 RID: 8372
			internal object _visitData;

			// Token: 0x040020B5 RID: 8373
			internal bool _throws;

			// Token: 0x040020B6 RID: 8374
			internal bool _returns;
		}

		// Token: 0x020005F0 RID: 1520
		private class AssignmentTarget : Ast
		{
			// Token: 0x0600415B RID: 16731 RVA: 0x0015ACC8 File Offset: 0x00158EC8
			public AssignmentTarget(ExpressionAst targetExpressionAst) : base(PositionUtilities.EmptyExtent)
			{
				this._targetAst = targetExpressionAst;
			}

			// Token: 0x0600415C RID: 16732 RVA: 0x0015ACDC File Offset: 0x00158EDC
			public AssignmentTarget(string variableName, Type type) : base(PositionUtilities.EmptyExtent)
			{
				this._variableName = variableName;
				this._type = type;
			}

			// Token: 0x0600415D RID: 16733 RVA: 0x0015ACF7 File Offset: 0x00158EF7
			public override Ast Copy()
			{
				return null;
			}

			// Token: 0x0600415E RID: 16734 RVA: 0x0015ACFA File Offset: 0x00158EFA
			internal override object Accept(ICustomAstVisitor visitor)
			{
				return null;
			}

			// Token: 0x0600415F RID: 16735 RVA: 0x0015ACFD File Offset: 0x00158EFD
			internal override AstVisitAction InternalVisit(AstVisitor visitor)
			{
				return AstVisitAction.Continue;
			}

			// Token: 0x06004160 RID: 16736 RVA: 0x0015AD00 File Offset: 0x00158F00
			internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
			{
				return Ast.EmptyPSTypeNameArray;
			}

			// Token: 0x040020B8 RID: 8376
			internal readonly ExpressionAst _targetAst;

			// Token: 0x040020B9 RID: 8377
			internal readonly string _variableName;

			// Token: 0x040020BA RID: 8378
			internal readonly Type _type;
		}
	}
}
