using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200006F RID: 111
	public interface IArgumentCompleter
	{
		// Token: 0x06000610 RID: 1552
		IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters);
	}
}
