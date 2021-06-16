using System;
using System.Collections.Generic;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000593 RID: 1427
	public interface IAstToWorkflowConverter
	{
		// Token: 0x06003B3F RID: 15167
		List<ParseError> ValidateAst(FunctionDefinitionAst ast);

		// Token: 0x06003B40 RID: 15168
		List<WorkflowInfo> CompileWorkflows(ScriptBlockAst ast, PSModuleInfo definingModule);

		// Token: 0x06003B41 RID: 15169
		List<WorkflowInfo> CompileWorkflows(ScriptBlockAst ast, PSModuleInfo definingModule, InitialSessionState initialSessionState, out ParseException parsingErrors);

		// Token: 0x06003B42 RID: 15170
		List<WorkflowInfo> CompileWorkflows(ScriptBlockAst ast, PSModuleInfo definingModule, string rootWorkflowName);

		// Token: 0x06003B43 RID: 15171
		List<WorkflowInfo> CompileWorkflows(ScriptBlockAst ast, PSModuleInfo definingModule, InitialSessionState initialSessionState, out ParseException parsingErrors, string rootWorkflowName);

		// Token: 0x06003B44 RID: 15172
		WorkflowInfo CompileWorkflow(string name, string definition, InitialSessionState initialSessionState);
	}
}
