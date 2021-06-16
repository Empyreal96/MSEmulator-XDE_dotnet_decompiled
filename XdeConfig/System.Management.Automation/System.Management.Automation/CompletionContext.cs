using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x02000979 RID: 2425
	internal class CompletionContext
	{
		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x06005907 RID: 22791 RVA: 0x001D311A File Offset: 0x001D131A
		// (set) Token: 0x06005908 RID: 22792 RVA: 0x001D3122 File Offset: 0x001D1322
		internal List<Ast> RelatedAsts { get; set; }

		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x06005909 RID: 22793 RVA: 0x001D312B File Offset: 0x001D132B
		// (set) Token: 0x0600590A RID: 22794 RVA: 0x001D3133 File Offset: 0x001D1333
		internal Token TokenAtCursor { get; set; }

		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x0600590B RID: 22795 RVA: 0x001D313C File Offset: 0x001D133C
		// (set) Token: 0x0600590C RID: 22796 RVA: 0x001D3144 File Offset: 0x001D1344
		internal Token TokenBeforeCursor { get; set; }

		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x0600590D RID: 22797 RVA: 0x001D314D File Offset: 0x001D134D
		// (set) Token: 0x0600590E RID: 22798 RVA: 0x001D3155 File Offset: 0x001D1355
		internal IScriptPosition CursorPosition { get; set; }

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x0600590F RID: 22799 RVA: 0x001D315E File Offset: 0x001D135E
		// (set) Token: 0x06005910 RID: 22800 RVA: 0x001D3166 File Offset: 0x001D1366
		internal CompletionExecutionHelper Helper { get; set; }

		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x06005911 RID: 22801 RVA: 0x001D316F File Offset: 0x001D136F
		// (set) Token: 0x06005912 RID: 22802 RVA: 0x001D3177 File Offset: 0x001D1377
		internal Hashtable Options { get; set; }

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06005913 RID: 22803 RVA: 0x001D3180 File Offset: 0x001D1380
		// (set) Token: 0x06005914 RID: 22804 RVA: 0x001D3188 File Offset: 0x001D1388
		internal Dictionary<string, ScriptBlock> CustomArgumentCompleters { get; set; }

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x06005915 RID: 22805 RVA: 0x001D3191 File Offset: 0x001D1391
		// (set) Token: 0x06005916 RID: 22806 RVA: 0x001D3199 File Offset: 0x001D1399
		internal Dictionary<string, ScriptBlock> NativeArgumentCompleters { get; set; }

		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x06005917 RID: 22807 RVA: 0x001D31A2 File Offset: 0x001D13A2
		// (set) Token: 0x06005918 RID: 22808 RVA: 0x001D31AA File Offset: 0x001D13AA
		internal string WordToComplete { get; set; }

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x06005919 RID: 22809 RVA: 0x001D31B3 File Offset: 0x001D13B3
		// (set) Token: 0x0600591A RID: 22810 RVA: 0x001D31BB File Offset: 0x001D13BB
		internal int ReplacementIndex { get; set; }

		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x0600591B RID: 22811 RVA: 0x001D31C4 File Offset: 0x001D13C4
		// (set) Token: 0x0600591C RID: 22812 RVA: 0x001D31CC File Offset: 0x001D13CC
		internal int ReplacementLength { get; set; }

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x0600591D RID: 22813 RVA: 0x001D31D5 File Offset: 0x001D13D5
		// (set) Token: 0x0600591E RID: 22814 RVA: 0x001D31DD File Offset: 0x001D13DD
		internal ExecutionContext ExecutionContext { get; set; }

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x0600591F RID: 22815 RVA: 0x001D31E6 File Offset: 0x001D13E6
		// (set) Token: 0x06005920 RID: 22816 RVA: 0x001D31EE File Offset: 0x001D13EE
		internal PseudoBindingInfo PseudoBindingInfo { get; set; }

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x06005921 RID: 22817 RVA: 0x001D31F7 File Offset: 0x001D13F7
		// (set) Token: 0x06005922 RID: 22818 RVA: 0x001D31FF File Offset: 0x001D13FF
		internal TypeDefinitionAst CurrentTypeDefinitionAst { get; set; }

		// Token: 0x06005923 RID: 22819 RVA: 0x001D3208 File Offset: 0x001D1408
		internal bool GetOption(string option, bool @default)
		{
			if (this.Options == null || !this.Options.ContainsKey(option))
			{
				return @default;
			}
			return LanguagePrimitives.ConvertTo<bool>(this.Options[option]);
		}
	}
}
