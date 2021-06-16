using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x0200059F RID: 1439
	internal static class CachedReflectionInfo
	{
		// Token: 0x04001DBC RID: 7612
		internal const BindingFlags instanceFlags = BindingFlags.Instance | BindingFlags.NonPublic;

		// Token: 0x04001DBD RID: 7613
		internal const BindingFlags staticFlags = BindingFlags.Static | BindingFlags.NonPublic;

		// Token: 0x04001DBE RID: 7614
		internal const BindingFlags staticPublicFlags = BindingFlags.Static | BindingFlags.Public;

		// Token: 0x04001DBF RID: 7615
		internal const BindingFlags instancePublicFlags = BindingFlags.Instance | BindingFlags.Public;

		// Token: 0x04001DC0 RID: 7616
		internal static readonly ConstructorInfo ObjectList_ctor = typeof(List<object>).GetConstructor(PSTypeExtensions.EmptyTypes);

		// Token: 0x04001DC1 RID: 7617
		internal static readonly MethodInfo ObjectList_ToArray = typeof(List<object>).GetMethod("ToArray", PSTypeExtensions.EmptyTypes);

		// Token: 0x04001DC2 RID: 7618
		internal static readonly MethodInfo ArrayOps_GetMDArrayValue = typeof(ArrayOps).GetMethod("GetMDArrayValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DC3 RID: 7619
		internal static readonly MethodInfo ArrayOps_GetMDArrayValueOrSlice = typeof(ArrayOps).GetMethod("GetMDArrayValueOrSlice", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DC4 RID: 7620
		internal static readonly MethodInfo ArrayOps_GetNonIndexable = typeof(ArrayOps).GetMethod("GetNonIndexable", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DC5 RID: 7621
		internal static readonly MethodInfo ArrayOps_IndexStringMessage = typeof(ArrayOps).GetMethod("IndexStringMessage", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DC6 RID: 7622
		internal static readonly MethodInfo ArrayOps_Multiply = typeof(ArrayOps).GetMethod("Multiply", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DC7 RID: 7623
		internal static readonly MethodInfo ArrayOps_SetMDArrayValue = typeof(ArrayOps).GetMethod("SetMDArrayValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DC8 RID: 7624
		internal static readonly MethodInfo ArrayOps_SlicingIndex = typeof(ArrayOps).GetMethod("SlicingIndex", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DC9 RID: 7625
		internal static readonly ConstructorInfo BreakException_ctor = typeof(BreakException).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Standard, new Type[]
		{
			typeof(string)
		}, null);

		// Token: 0x04001DCA RID: 7626
		internal static readonly MethodInfo CharOps_CompareIeq = typeof(CharOps).GetMethod("CompareIeq", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DCB RID: 7627
		internal static readonly MethodInfo CharOps_CompareIne = typeof(CharOps).GetMethod("CompareIne", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DCC RID: 7628
		internal static readonly MethodInfo CharOps_CompareStringIeq = typeof(CharOps).GetMethod("CompareStringIeq", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DCD RID: 7629
		internal static readonly MethodInfo CharOps_CompareStringIne = typeof(CharOps).GetMethod("CompareStringIne", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DCE RID: 7630
		internal static readonly MethodInfo CommandParameterInternal_CreateArgument = typeof(CommandParameterInternal).GetMethod("CreateArgument", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DCF RID: 7631
		internal static readonly MethodInfo CommandParameterInternal_CreateParameter = typeof(CommandParameterInternal).GetMethod("CreateParameter", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DD0 RID: 7632
		internal static readonly MethodInfo CommandParameterInternal_CreateParameterWithArgument = typeof(CommandParameterInternal).GetMethod("CreateParameterWithArgument", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DD1 RID: 7633
		internal static readonly MethodInfo CommandProcessorBase_CheckForSevereException = typeof(CommandProcessorBase).GetMethod("CheckForSevereException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DD2 RID: 7634
		internal static readonly MethodInfo CommandRedirection_UnbindForExpression = typeof(CommandRedirection).GetMethod("UnbindForExpression", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DD3 RID: 7635
		internal static readonly ConstructorInfo ContinueException_ctor = typeof(ContinueException).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Standard, new Type[]
		{
			typeof(string)
		}, null);

		// Token: 0x04001DD4 RID: 7636
		internal static readonly MethodInfo Convert_ChangeType = typeof(Convert).GetMethod("ChangeType", new Type[]
		{
			typeof(object),
			typeof(Type)
		});

		// Token: 0x04001DD5 RID: 7637
		internal static readonly MethodInfo Debugger_EnterScriptFunction = typeof(ScriptDebugger).GetMethod("EnterScriptFunction", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DD6 RID: 7638
		internal static readonly MethodInfo Debugger_ExitScriptFunction = typeof(ScriptDebugger).GetMethod("ExitScriptFunction", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DD7 RID: 7639
		internal static readonly MethodInfo Debugger_OnSequencePointHit = typeof(ScriptDebugger).GetMethod("OnSequencePointHit", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DD8 RID: 7640
		internal static readonly MethodInfo EnumerableOps_AddEnumerable = typeof(EnumerableOps).GetMethod("AddEnumerable", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DD9 RID: 7641
		internal static readonly MethodInfo EnumerableOps_AddObject = typeof(EnumerableOps).GetMethod("AddObject", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DDA RID: 7642
		internal static readonly MethodInfo EnumerableOps_Compare = typeof(EnumerableOps).GetMethod("Compare", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DDB RID: 7643
		internal static readonly MethodInfo EnumerableOps_Current = typeof(EnumerableOps).GetMethod("Current", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DDC RID: 7644
		internal static readonly MethodInfo EnumerableOps_GetEnumerator = typeof(EnumerableOps).GetMethod("GetEnumerator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DDD RID: 7645
		internal static readonly MethodInfo EnumerableOps_GetCOMEnumerator = typeof(EnumerableOps).GetMethod("GetCOMEnumerator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DDE RID: 7646
		internal static readonly MethodInfo EnumerableOps_GetGenericEnumerator = typeof(EnumerableOps).GetMethod("GetGenericEnumerator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DDF RID: 7647
		internal static readonly MethodInfo EnumerableOps_GetSlice = typeof(EnumerableOps).GetMethod("GetSlice", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE0 RID: 7648
		internal static readonly MethodInfo EnumerableOps_MethodInvoker = typeof(EnumerableOps).GetMethod("MethodInvoker", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE1 RID: 7649
		internal static readonly MethodInfo EnumerableOps_Where = typeof(EnumerableOps).GetMethod("Where", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE2 RID: 7650
		internal static readonly MethodInfo EnumerableOps_ForEach = typeof(EnumerableOps).GetMethod("ForEach", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE3 RID: 7651
		internal static readonly MethodInfo EnumerableOps_MoveNext = typeof(EnumerableOps).GetMethod("MoveNext", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE4 RID: 7652
		internal static readonly MethodInfo EnumerableOps_Multiply = typeof(EnumerableOps).GetMethod("Multiply", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE5 RID: 7653
		internal static readonly MethodInfo EnumerableOps_PropertyGetter = typeof(EnumerableOps).GetMethod("PropertyGetter", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE6 RID: 7654
		internal static readonly MethodInfo EnumerableOps_SlicingIndex = typeof(EnumerableOps).GetMethod("SlicingIndex", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE7 RID: 7655
		internal static readonly MethodInfo EnumerableOps_ToArray = typeof(EnumerableOps).GetMethod("ToArray", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE8 RID: 7656
		internal static readonly MethodInfo EnumerableOps_WriteEnumerableToPipe = typeof(EnumerableOps).GetMethod("WriteEnumerableToPipe", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DE9 RID: 7657
		internal static readonly ConstructorInfo ErrorRecord__ctor = typeof(ErrorRecord).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Standard, new Type[]
		{
			typeof(ErrorRecord),
			typeof(RuntimeException)
		}, null);

		// Token: 0x04001DEA RID: 7658
		internal static readonly PropertyInfo Exception_Message = typeof(Exception).GetProperty("Message");

		// Token: 0x04001DEB RID: 7659
		internal static readonly MethodInfo ExceptionHandlingOps_CheckActionPreference = typeof(ExceptionHandlingOps).GetMethod("CheckActionPreference", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DEC RID: 7660
		internal static readonly MethodInfo ExceptionHandlingOps_ConvertToArgumentConversionException = typeof(ExceptionHandlingOps).GetMethod("ConvertToArgumentConversionException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DED RID: 7661
		internal static readonly MethodInfo ExceptionHandlingOps_ConvertToException = typeof(ExceptionHandlingOps).GetMethod("ConvertToException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DEE RID: 7662
		internal static readonly MethodInfo ExceptionHandlingOps_ConvertToMethodInvocationException = typeof(ExceptionHandlingOps).GetMethod("ConvertToMethodInvocationException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DEF RID: 7663
		internal static readonly MethodInfo ExceptionHandlingOps_ConvertToRuntimeException = typeof(ExceptionHandlingOps).GetMethod("ConvertToRuntimeException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DF0 RID: 7664
		internal static readonly MethodInfo ExceptionHandlingOps_FindMatchingHandler = typeof(ExceptionHandlingOps).GetMethod("FindMatchingHandler", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DF1 RID: 7665
		internal static readonly MethodInfo ExceptionHandlingOps_RestoreStoppingPipeline = typeof(ExceptionHandlingOps).GetMethod("RestoreStoppingPipeline", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DF2 RID: 7666
		internal static readonly MethodInfo ExceptionHandlingOps_SuspendStoppingPipeline = typeof(ExceptionHandlingOps).GetMethod("SuspendStoppingPipeline", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DF3 RID: 7667
		internal static readonly PropertyInfo ExecutionContext_CurrentExceptionBeingHandled = typeof(ExecutionContext).GetProperty("CurrentExceptionBeingHandled", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DF4 RID: 7668
		internal static readonly FieldInfo ExecutionContext_Debugger = typeof(ExecutionContext).GetField("_debugger", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DF5 RID: 7669
		internal static readonly FieldInfo ExecutionContext_DebuggingMode = typeof(ExecutionContext).GetField("_debuggingMode", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DF6 RID: 7670
		internal static readonly PropertyInfo ExecutionContext_ExceptionHandlerInEnclosingStatementBlock = typeof(ExecutionContext).GetProperty("PropagateExceptionsToEnclosingStatementBlock", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DF7 RID: 7671
		internal static readonly MethodInfo ExecutionContext_IsStrictVersion = typeof(ExecutionContext).GetMethod("IsStrictVersion", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001DF8 RID: 7672
		internal static readonly PropertyInfo ExecutionContext_QuestionMarkVariableValue = typeof(ExecutionContext).GetProperty("QuestionMarkVariableValue", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DF9 RID: 7673
		internal static readonly PropertyInfo ExecutionContext_LanguageMode = typeof(ExecutionContext).GetProperty("LanguageMode", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DFA RID: 7674
		internal static readonly PropertyInfo ExecutionContext_EngineIntrinsics = typeof(ExecutionContext).GetProperty("EngineIntrinsics", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DFB RID: 7675
		internal static readonly PropertyInfo ExecutionContext_ShellFunctionErrorOutputPipe = typeof(ExecutionContext).GetProperty("ShellFunctionErrorOutputPipe", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DFC RID: 7676
		internal static readonly PropertyInfo ExecutionContext_TypeTable = typeof(ExecutionContext).GetProperty("TypeTable", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DFD RID: 7677
		internal static readonly PropertyInfo ExecutionContext_ExpressionWarningOutputPipe = typeof(ExecutionContext).GetProperty("ExpressionWarningOutputPipe", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DFE RID: 7678
		internal static readonly PropertyInfo ExecutionContext_ExpressionVerboseOutputPipe = typeof(ExecutionContext).GetProperty("ExpressionVerboseOutputPipe", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001DFF RID: 7679
		internal static readonly PropertyInfo ExecutionContext_ExpressionDebugOutputPipe = typeof(ExecutionContext).GetProperty("ExpressionDebugOutputPipe", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E00 RID: 7680
		internal static readonly PropertyInfo ExecutionContext_ExpressionInformationOutputPipe = typeof(ExecutionContext).GetProperty("ExpressionInformationOutputPipe", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E01 RID: 7681
		internal static readonly MethodInfo FileRedirection_BindForExpression = typeof(FileRedirection).GetMethod("BindForExpression", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E02 RID: 7682
		internal static readonly ConstructorInfo FileRedirection_ctor = typeof(FileRedirection).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Standard, new Type[]
		{
			typeof(RedirectionStream),
			typeof(bool),
			typeof(string)
		}, null);

		// Token: 0x04001E03 RID: 7683
		internal static readonly MethodInfo FileRedirection_Dispose = typeof(FileRedirection).GetMethod("Dispose");

		// Token: 0x04001E04 RID: 7684
		internal static readonly FieldInfo FunctionContext__currentSequencePointIndex = typeof(FunctionContext).GetField("_currentSequencePointIndex", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E05 RID: 7685
		internal static readonly FieldInfo FunctionContext__executionContext = typeof(FunctionContext).GetField("_executionContext", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E06 RID: 7686
		internal static readonly FieldInfo FunctionContext__functionName = typeof(FunctionContext).GetField("_functionName", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E07 RID: 7687
		internal static readonly FieldInfo FunctionContext__localsTuple = typeof(FunctionContext).GetField("_localsTuple", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E08 RID: 7688
		internal static readonly FieldInfo FunctionContext__outputPipe = typeof(FunctionContext).GetField("_outputPipe", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E09 RID: 7689
		internal static readonly FieldInfo FunctionContext__traps = typeof(FunctionContext).GetField("_traps", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E0A RID: 7690
		internal static readonly MethodInfo FunctionContext_PopTrapHandlers = typeof(FunctionContext).GetMethod("PopTrapHandlers", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E0B RID: 7691
		internal static readonly MethodInfo FunctionContext_PushTrapHandlers = typeof(FunctionContext).GetMethod("PushTrapHandlers", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E0C RID: 7692
		internal static readonly MethodInfo FunctionOps_DefineFunction = typeof(FunctionOps).GetMethod("DefineFunction", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E0D RID: 7693
		internal static readonly MethodInfo FunctionOps_DefineWorkflows = typeof(FunctionOps).GetMethod("DefineWorkflows", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E0E RID: 7694
		internal static readonly ConstructorInfo Hashtable_ctor = typeof(Hashtable).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.Standard, new Type[]
		{
			typeof(int),
			typeof(IEqualityComparer)
		}, null);

		// Token: 0x04001E0F RID: 7695
		internal static readonly MethodInfo HashtableOps_Add = typeof(HashtableOps).GetMethod("Add", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E10 RID: 7696
		internal static readonly MethodInfo HashtableOps_AddKeyValuePair = typeof(HashtableOps).GetMethod("AddKeyValuePair", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E11 RID: 7697
		internal static readonly PropertyInfo ICollection_Count = typeof(ICollection).GetProperty("Count");

		// Token: 0x04001E12 RID: 7698
		internal static readonly MethodInfo IComparable_CompareTo = typeof(IComparable).GetMethod("CompareTo");

		// Token: 0x04001E13 RID: 7699
		internal static readonly MethodInfo IDictionary_set_Item = typeof(IDictionary).GetMethod("set_Item");

		// Token: 0x04001E14 RID: 7700
		internal static readonly MethodInfo IDisposable_Dispose = typeof(IDisposable).GetMethod("Dispose");

		// Token: 0x04001E15 RID: 7701
		internal static readonly MethodInfo IEnumerable_GetEnumerator = typeof(IEnumerable).GetMethod("GetEnumerator");

		// Token: 0x04001E16 RID: 7702
		internal static readonly PropertyInfo IEnumerator_Current = typeof(IEnumerator).GetProperty("Current");

		// Token: 0x04001E17 RID: 7703
		internal static readonly MethodInfo IEnumerator_MoveNext = typeof(IEnumerator).GetMethod("MoveNext");

		// Token: 0x04001E18 RID: 7704
		internal static readonly MethodInfo IList_get_Item = typeof(IList).GetMethod("get_Item");

		// Token: 0x04001E19 RID: 7705
		internal static readonly MethodInfo InterpreterError_NewInterpreterException = typeof(InterpreterError).GetMethod("NewInterpreterException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E1A RID: 7706
		internal static readonly MethodInfo InterpreterError_NewInterpreterExceptionWithInnerException = typeof(InterpreterError).GetMethod("NewInterpreterExceptionWithInnerException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E1B RID: 7707
		internal static readonly MethodInfo IntOps_Range = typeof(IntOps).GetMethod("Range", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E1C RID: 7708
		internal static readonly MethodInfo LanguagePrimitives_GetInvalidCastMessages = typeof(LanguagePrimitives).GetMethod("GetInvalidCastMessages", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E1D RID: 7709
		internal static readonly MethodInfo LanguagePrimitives_ThrowInvalidCastException = typeof(LanguagePrimitives).GetMethod("ThrowInvalidCastException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E1E RID: 7710
		internal static readonly MethodInfo LocalPipeline_GetExecutionContextFromTLS = typeof(LocalPipeline).GetMethod("GetExecutionContextFromTLS", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E1F RID: 7711
		internal static readonly MethodInfo LoopFlowException_MatchLabel = typeof(LoopFlowException).GetMethod("MatchLabel", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E20 RID: 7712
		internal static readonly MethodInfo MergingRedirection_BindForExpression = typeof(MergingRedirection).GetMethod("BindForExpression", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E21 RID: 7713
		internal static readonly ConstructorInfo MethodException_ctor = typeof(MethodException).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(string),
			typeof(Exception),
			typeof(string),
			typeof(object[])
		}, null);

		// Token: 0x04001E22 RID: 7714
		internal static readonly MethodInfo MutableTuple_IsValueSet = typeof(MutableTuple).GetMethod("IsValueSet", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E23 RID: 7715
		internal static readonly MethodInfo Object_Equals = typeof(object).GetMethod("Equals", new Type[]
		{
			typeof(object)
		});

		// Token: 0x04001E24 RID: 7716
		internal static readonly ConstructorInfo OrderedDictionary_ctor = typeof(OrderedDictionary).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.Standard, new Type[]
		{
			typeof(int),
			typeof(IEqualityComparer)
		}, null);

		// Token: 0x04001E25 RID: 7717
		internal static readonly MethodInfo Parser_ScanNumber = typeof(Parser).GetMethod("ScanNumber", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E26 RID: 7718
		internal static readonly MethodInfo ParserOps_ContainsOperatorCompiled = typeof(ParserOps).GetMethod("ContainsOperatorCompiled", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E27 RID: 7719
		internal static readonly MethodInfo ParserOps_ImplicitOp = typeof(ParserOps).GetMethod("ImplicitOp", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E28 RID: 7720
		internal static readonly MethodInfo ParserOps_JoinOperator = typeof(ParserOps).GetMethod("JoinOperator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E29 RID: 7721
		internal static readonly MethodInfo ParserOps_LikeOperator = typeof(ParserOps).GetMethod("LikeOperator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E2A RID: 7722
		internal static readonly MethodInfo ParserOps_MatchOperator = typeof(ParserOps).GetMethod("MatchOperator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E2B RID: 7723
		internal static readonly MethodInfo ParserOps_ReplaceOperator = typeof(ParserOps).GetMethod("ReplaceOperator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E2C RID: 7724
		internal static readonly MethodInfo ParserOps_SplitOperator = typeof(ParserOps).GetMethod("SplitOperator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E2D RID: 7725
		internal static readonly MethodInfo ParserOps_UnaryJoinOperator = typeof(ParserOps).GetMethod("UnaryJoinOperator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E2E RID: 7726
		internal static readonly MethodInfo ParserOps_UnarySplitOperator = typeof(ParserOps).GetMethod("UnarySplitOperator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E2F RID: 7727
		internal static readonly ConstructorInfo Pipe_ctor = typeof(Pipe).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Standard, new Type[]
		{
			typeof(List<object>)
		}, null);

		// Token: 0x04001E30 RID: 7728
		internal static readonly MethodInfo Pipe_Add = typeof(Pipe).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E31 RID: 7729
		internal static readonly MethodInfo Pipe_SetVariableListForTemporaryPipe = typeof(Pipe).GetMethod("SetVariableListForTemporaryPipe", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E32 RID: 7730
		internal static readonly MethodInfo PipelineOps_CheckAutomationNullInCommandArgument = typeof(PipelineOps).GetMethod("CheckAutomationNullInCommandArgument", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E33 RID: 7731
		internal static readonly MethodInfo PipelineOps_CheckAutomationNullInCommandArgumentArray = typeof(PipelineOps).GetMethod("CheckAutomationNullInCommandArgumentArray", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E34 RID: 7732
		internal static readonly MethodInfo PipelineOps_CheckForInterrupts = typeof(PipelineOps).GetMethod("CheckForInterrupts", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E35 RID: 7733
		internal static readonly MethodInfo PipelineOps_GetExitException = typeof(PipelineOps).GetMethod("GetExitException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E36 RID: 7734
		internal static readonly MethodInfo PipelineOps_FlushPipe = typeof(PipelineOps).GetMethod("FlushPipe", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E37 RID: 7735
		internal static readonly MethodInfo PipelineOps_InvokePipeline = typeof(PipelineOps).GetMethod("InvokePipeline", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E38 RID: 7736
		internal static readonly MethodInfo PipelineOps_Nop = typeof(PipelineOps).GetMethod("Nop", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E39 RID: 7737
		internal static readonly MethodInfo PipelineOps_PipelineResult = typeof(PipelineOps).GetMethod("PipelineResult", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E3A RID: 7738
		internal static readonly MethodInfo PipelineOps_ClearPipe = typeof(PipelineOps).GetMethod("ClearPipe", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E3B RID: 7739
		internal static readonly MethodInfo PSGetDynamicMemberBinder_GetIDictionaryMember = typeof(PSGetDynamicMemberBinder).GetMethod("GetIDictionaryMember", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E3C RID: 7740
		internal static readonly MethodInfo PSGetMemberBinder_CloneMemberInfo = typeof(PSGetMemberBinder).GetMethod("CloneMemberInfo", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E3D RID: 7741
		internal static readonly MethodInfo PSGetMemberBinder_GetAdaptedValue = typeof(PSGetMemberBinder).GetMethod("GetAdaptedValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E3E RID: 7742
		internal static readonly MethodInfo PSGetMemberBinder_GetTypeTableFromTLS = typeof(PSGetMemberBinder).GetMethod("GetTypeTableFromTLS", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E3F RID: 7743
		internal static readonly MethodInfo PSGetMemberBinder_IsTypeNameSame = typeof(PSGetMemberBinder).GetMethod("IsTypeNameSame", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E40 RID: 7744
		internal static readonly MethodInfo PSGetMemberBinder_TryGetGenericDictionaryValue = typeof(PSGetMemberBinder).GetMethod("TryGetGenericDictionaryValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E41 RID: 7745
		internal static readonly MethodInfo PSGetMemberBinder_TryGetInstanceMember = typeof(PSGetMemberBinder).GetMethod("TryGetInstanceMember", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E42 RID: 7746
		internal static readonly MethodInfo PSGetMemberBinder_TryGetIDictionaryValue = typeof(PSGetMemberBinder).GetMethod("TryGetIDictionaryValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E43 RID: 7747
		internal static readonly MethodInfo PSInvokeMemberBinder_InvokeAdaptedMember = typeof(PSInvokeMemberBinder).GetMethod("InvokeAdaptedMember", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E44 RID: 7748
		internal static readonly MethodInfo PSInvokeMemberBinder_InvokeAdaptedSetMember = typeof(PSInvokeMemberBinder).GetMethod("InvokeAdaptedSetMember", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E45 RID: 7749
		internal static readonly MethodInfo PSInvokeMemberBinder_IsHeterogeneousArray = typeof(PSInvokeMemberBinder).GetMethod("IsHeterogeneousArray", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E46 RID: 7750
		internal static readonly MethodInfo PSInvokeMemberBinder_IsHomogenousArray = typeof(PSInvokeMemberBinder).GetMethod("IsHomogenousArray", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E47 RID: 7751
		internal static readonly MethodInfo PSInvokeMemberBinder_TryGetInstanceMethod = typeof(PSInvokeMemberBinder).GetMethod("TryGetInstanceMethod", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E48 RID: 7752
		internal static readonly MethodInfo PSMethodInfo_Invoke = typeof(PSMethodInfo).GetMethod("Invoke");

		// Token: 0x04001E49 RID: 7753
		internal static readonly PropertyInfo PSNoteProperty_Value = typeof(PSNoteProperty).GetProperty("Value");

		// Token: 0x04001E4A RID: 7754
		internal static readonly MethodInfo PSObject_Base = typeof(PSObject).GetMethod("Base", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E4B RID: 7755
		internal static readonly PropertyInfo PSObject_BaseObject = typeof(PSObject).GetProperty("BaseObject");

		// Token: 0x04001E4C RID: 7756
		internal static readonly FieldInfo PSObject_isDeserialized = typeof(PSObject).GetField("isDeserialized", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E4D RID: 7757
		internal static readonly MethodInfo PSObject_ToStringParser = typeof(PSObject).GetMethod("ToStringParser", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E4E RID: 7758
		internal static readonly PropertyInfo PSReference_Value = typeof(PSReference).GetProperty("Value");

		// Token: 0x04001E4F RID: 7759
		internal static readonly MethodInfo PSScriptMethod_InvokeScript = typeof(PSScriptMethod).GetMethod("InvokeScript", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E50 RID: 7760
		internal static readonly MethodInfo PSScriptProperty_InvokeGetter = typeof(PSScriptProperty).GetMethod("InvokeGetter", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E51 RID: 7761
		internal static readonly MethodInfo PSScriptProperty_InvokeSetter = typeof(PSScriptProperty).GetMethod("InvokeSetter", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E52 RID: 7762
		internal static readonly MethodInfo PSSetMemberBinder_SetAdaptedValue = typeof(PSSetMemberBinder).GetMethod("SetAdaptedValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E53 RID: 7763
		internal static readonly MethodInfo PSVariableAssignmentBinder_CopyInstanceMembersOfValueType = typeof(PSVariableAssignmentBinder).GetMethod("CopyInstanceMembersOfValueType", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E54 RID: 7764
		internal static readonly FieldInfo PSVariableAssignmentBinder__mutableValueWithInstanceMemberVersion = typeof(PSVariableAssignmentBinder).GetField("_mutableValueWithInstanceMemberVersion", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E55 RID: 7765
		internal static readonly MethodInfo PSCreateInstanceBinder_IsTargetTypeNonPublic = typeof(PSCreateInstanceBinder).GetMethod("IsTargetTypeNonPublic", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E56 RID: 7766
		internal static readonly MethodInfo PSCreateInstanceBinder_GetTargetTypeName = typeof(PSCreateInstanceBinder).GetMethod("GetTargetTypeName", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E57 RID: 7767
		internal static readonly ConstructorInfo RangeEnumerator_ctor = typeof(RangeEnumerator).GetConstructor(new Type[]
		{
			typeof(int),
			typeof(int)
		});

		// Token: 0x04001E58 RID: 7768
		internal static readonly MethodInfo ReservedNameMembers_GeneratePSAdaptedMemberSet = typeof(ReservedNameMembers).GetMethod("GeneratePSAdaptedMemberSet", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E59 RID: 7769
		internal static readonly MethodInfo ReservedNameMembers_GeneratePSBaseMemberSet = typeof(ReservedNameMembers).GetMethod("GeneratePSBaseMemberSet", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E5A RID: 7770
		internal static readonly MethodInfo ReservedNameMembers_GeneratePSExtendedMemberSet = typeof(ReservedNameMembers).GetMethod("GeneratePSExtendedMemberSet", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E5B RID: 7771
		internal static readonly MethodInfo ReservedNameMembers_GeneratePSObjectMemberSet = typeof(ReservedNameMembers).GetMethod("GeneratePSObjectMemberSet", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E5C RID: 7772
		internal static readonly MethodInfo ReservedNameMembers_PSTypeNames = typeof(ReservedNameMembers).GetMethod("PSTypeNames");

		// Token: 0x04001E5D RID: 7773
		internal static readonly MethodInfo RestrictedLanguageChecker_CheckDataStatementLanguageModeAtRuntime = typeof(RestrictedLanguageChecker).GetMethod("CheckDataStatementLanguageModeAtRuntime", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E5E RID: 7774
		internal static readonly MethodInfo RestrictedLanguageChecker_CheckDataStatementAstAtRuntime = typeof(RestrictedLanguageChecker).GetMethod("CheckDataStatementAstAtRuntime", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E5F RID: 7775
		internal static readonly MethodInfo RestrictedLanguageChecker_EnsureUtilityModuleLoaded = typeof(RestrictedLanguageChecker).GetMethod("EnsureUtilityModuleLoaded", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E60 RID: 7776
		internal static readonly ConstructorInfo ReturnException_ctor = typeof(ReturnException).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Standard, new Type[]
		{
			typeof(object)
		}, null);

		// Token: 0x04001E61 RID: 7777
		internal static readonly PropertyInfo RuntimeException_ErrorRecord = typeof(RuntimeException).GetProperty("ErrorRecord");

		// Token: 0x04001E62 RID: 7778
		internal static readonly MethodInfo ScriptBlock_DoInvokeReturnAsIs = typeof(ScriptBlock).GetMethod("DoInvokeReturnAsIs", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E63 RID: 7779
		internal static readonly MethodInfo ScriptBlock_InvokeAsDelegateHelper = typeof(ScriptBlock).GetMethod("InvokeAsDelegateHelper", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E64 RID: 7780
		internal static readonly MethodInfo ScriptBlock_InvokeAsMemberFunction = typeof(ScriptBlock).GetMethod("InvokeAsMemberFunction", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E65 RID: 7781
		internal static readonly MethodInfo ScriptBlockExpressionWrapper_GetScriptBlock = typeof(ScriptBlockExpressionWrapper).GetMethod("GetScriptBlock", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001E66 RID: 7782
		internal static readonly ConstructorInfo SetValueException_ctor = typeof(SetValueException).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(string),
			typeof(Exception),
			typeof(string),
			typeof(object[])
		}, null);

		// Token: 0x04001E67 RID: 7783
		internal static readonly ConstructorInfo GetValueException_ctor = typeof(GetValueException).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(string),
			typeof(Exception),
			typeof(string),
			typeof(object[])
		}, null);

		// Token: 0x04001E68 RID: 7784
		internal static readonly ConstructorInfo StreamReader_ctor = typeof(StreamReader).GetConstructor(new Type[]
		{
			typeof(string)
		});

		// Token: 0x04001E69 RID: 7785
		internal static readonly MethodInfo StreamReader_ReadLine = typeof(StreamReader).GetMethod("ReadLine");

		// Token: 0x04001E6A RID: 7786
		internal static readonly ConstructorInfo String_ctor_char_int = typeof(string).GetConstructor(new Type[]
		{
			typeof(char),
			typeof(int)
		});

		// Token: 0x04001E6B RID: 7787
		internal static readonly MethodInfo String_Concat_String = typeof(string).GetMethod("Concat", BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Standard, new Type[]
		{
			typeof(string),
			typeof(string)
		}, null);

		// Token: 0x04001E6C RID: 7788
		internal static readonly MethodInfo String_Equals = typeof(string).GetMethod("Equals", BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Standard, new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(StringComparison)
		}, null);

		// Token: 0x04001E6D RID: 7789
		internal static readonly MethodInfo String_get_Chars = typeof(string).GetMethod("get_Chars");

		// Token: 0x04001E6E RID: 7790
		internal static readonly PropertyInfo String_Length = typeof(string).GetProperty("Length");

		// Token: 0x04001E6F RID: 7791
		internal static readonly MethodInfo StringOps_Compare = typeof(StringOps).GetMethod("Compare", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E70 RID: 7792
		internal static readonly MethodInfo StringOps_Equals = typeof(StringOps).GetMethod("Equals", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E71 RID: 7793
		internal static readonly MethodInfo StringOps_FormatOperator = typeof(StringOps).GetMethod("FormatOperator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E72 RID: 7794
		internal static readonly MethodInfo StringOps_Multiply = typeof(StringOps).GetMethod("Multiply", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E73 RID: 7795
		internal static readonly MethodInfo SwitchOps_ConditionSatisfiedRegex = typeof(SwitchOps).GetMethod("ConditionSatisfiedRegex", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E74 RID: 7796
		internal static readonly MethodInfo SwitchOps_ConditionSatisfiedWildcard = typeof(SwitchOps).GetMethod("ConditionSatisfiedWildcard", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E75 RID: 7797
		internal static readonly MethodInfo SwitchOps_ResolveFilePath = typeof(SwitchOps).GetMethod("ResolveFilePath", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E76 RID: 7798
		internal static readonly MethodInfo TypeOps_AsOperator = typeof(TypeOps).GetMethod("AsOperator", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E77 RID: 7799
		internal static readonly MethodInfo TypeOps_AddPowerShellTypesToTheScope = typeof(TypeOps).GetMethod("AddPowerShellTypesToTheScope", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E78 RID: 7800
		internal static readonly MethodInfo TypeOps_SetCurrentTypeResolutionState = typeof(TypeOps).GetMethod("SetCurrentTypeResolutionState", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E79 RID: 7801
		internal static readonly MethodInfo TypeOps_SetAssemblyDefiningPSTypes = typeof(TypeOps).GetMethod("SetAssemblyDefiningPSTypes", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E7A RID: 7802
		internal static readonly MethodInfo TypeOps_IsInstance = typeof(TypeOps).GetMethod("IsInstance", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E7B RID: 7803
		internal static readonly MethodInfo TypeOps_ResolveTypeName = typeof(TypeOps).GetMethod("ResolveTypeName", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E7C RID: 7804
		internal static readonly MethodInfo VariableOps_GetUsingValue = typeof(VariableOps).GetMethod("GetUsingValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E7D RID: 7805
		internal static readonly MethodInfo VariableOps_GetVariableAsRef = typeof(VariableOps).GetMethod("GetVariableAsRef", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E7E RID: 7806
		internal static readonly MethodInfo VariableOps_GetVariableValue = typeof(VariableOps).GetMethod("GetVariableValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E7F RID: 7807
		internal static readonly MethodInfo VariableOps_GetAutomaticVariableValue = typeof(VariableOps).GetMethod("GetAutomaticVariableValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E80 RID: 7808
		internal static readonly MethodInfo VariableOps_SetVariableValue = typeof(VariableOps).GetMethod("SetVariableValue", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001E81 RID: 7809
		internal static readonly MethodInfo ClassOps_ValidateSetProperty = typeof(ClassOps).GetMethod("ValidateSetProperty", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001E82 RID: 7810
		internal static readonly MethodInfo ClassOps_CallBaseCtor = typeof(ClassOps).GetMethod("CallBaseCtor", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001E83 RID: 7811
		internal static readonly MethodInfo ClassOps_CallMethodNonVirtually = typeof(ClassOps).GetMethod("CallMethodNonVirtually", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001E84 RID: 7812
		internal static readonly MethodInfo ClassOps_CallVoidMethodNonVirtually = typeof(ClassOps).GetMethod("CallVoidMethodNonVirtually", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001E85 RID: 7813
		internal static readonly MethodInfo ArgumentTransformationAttribute_Transform = typeof(ArgumentTransformationAttribute).GetMethod("Transform", BindingFlags.Instance | BindingFlags.Public);
	}
}
