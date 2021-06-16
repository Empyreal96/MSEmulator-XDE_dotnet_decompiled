using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200062D RID: 1581
	internal static class FunctionOps
	{
		// Token: 0x060044AF RID: 17583 RVA: 0x0016FB6C File Offset: 0x0016DD6C
		internal static void DefineFunction(ExecutionContext context, FunctionDefinitionAst functionDefinitionAst, ScriptBlockExpressionWrapper scriptBlockExpressionWrapper)
		{
			try
			{
				ScriptBlock scriptBlock = scriptBlockExpressionWrapper.GetScriptBlock(context, functionDefinitionAst.IsFilter);
				context.EngineSessionState.SetFunctionRaw(functionDefinitionAst.Name, scriptBlock, context.EngineSessionState.CurrentScope.ScopeOrigin);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				RuntimeException ex2 = ex as RuntimeException;
				if (ex2 == null)
				{
					throw ExceptionHandlingOps.ConvertToRuntimeException(ex, functionDefinitionAst.Extent);
				}
				InterpreterError.UpdateExceptionErrorRecordPosition(ex2, functionDefinitionAst.Extent);
				throw;
			}
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x0016FBE8 File Offset: 0x0016DDE8
		internal static void DefineWorkflows(ExecutionContext context, ScriptBlockAst scriptBlockAst)
		{
			ParseException ex = null;
			try
			{
				IAstToWorkflowConverter astToWorkflowConverterAndEnsureWorkflowModuleLoaded = Utils.GetAstToWorkflowConverterAndEnsureWorkflowModuleLoaded(context);
				List<WorkflowInfo> list = astToWorkflowConverterAndEnsureWorkflowModuleLoaded.CompileWorkflows(scriptBlockAst, context.EngineSessionState.Module, null, out ex);
				foreach (WorkflowInfo workflowInfo in list)
				{
					context.EngineSessionState.SetWorkflowRaw(workflowInfo, context.EngineSessionState.CurrentScope.ScopeOrigin);
				}
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				RuntimeException ex3 = ex2 as RuntimeException;
				if (ex3 == null)
				{
					throw ExceptionHandlingOps.ConvertToRuntimeException(ex2, scriptBlockAst.Extent);
				}
				InterpreterError.UpdateExceptionErrorRecordPosition(ex3, scriptBlockAst.Extent);
				throw;
			}
			if (ex != null && ex.Errors != null)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				InterpreterError.UpdateExceptionErrorRecordPosition(ex, scriptBlockAst.Extent);
				throw ex;
			}
		}
	}
}
