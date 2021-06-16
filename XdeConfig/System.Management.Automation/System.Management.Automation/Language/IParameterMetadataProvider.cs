using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Language
{
	// Token: 0x02000536 RID: 1334
	internal interface IParameterMetadataProvider
	{
		// Token: 0x06003771 RID: 14193
		RuntimeDefinedParameterDictionary GetParameterMetadata(bool automaticPositions, ref bool usesCmdletBinding);

		// Token: 0x06003772 RID: 14194
		IEnumerable<Attribute> GetScriptBlockAttributes();

		// Token: 0x06003773 RID: 14195
		bool UsesCmdletBinding();

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06003774 RID: 14196
		ReadOnlyCollection<ParameterAst> Parameters { get; }

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06003775 RID: 14197
		ScriptBlockAst Body { get; }

		// Token: 0x06003776 RID: 14198
		PowerShell GetPowerShell(ExecutionContext context, Dictionary<string, object> variables, bool isTrustedInput, bool filterNonUsingVariables, bool? createLocalScope, params object[] args);

		// Token: 0x06003777 RID: 14199
		string GetWithInputHandlingForInvokeCommand();

		// Token: 0x06003778 RID: 14200
		Tuple<string, string> GetWithInputHandlingForInvokeCommandWithUsingExpression(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple);
	}
}
