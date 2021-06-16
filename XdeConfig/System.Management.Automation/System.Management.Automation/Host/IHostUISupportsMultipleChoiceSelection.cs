using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Host
{
	// Token: 0x0200020E RID: 526
	public interface IHostUISupportsMultipleChoiceSelection
	{
		// Token: 0x0600189A RID: 6298
		Collection<int> PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, IEnumerable<int> defaultChoices);
	}
}
