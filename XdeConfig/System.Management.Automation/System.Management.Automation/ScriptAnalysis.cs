using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x020000C0 RID: 192
	[Serializable]
	internal class ScriptAnalysis
	{
		// Token: 0x06000A5C RID: 2652 RVA: 0x0003F1D8 File Offset: 0x0003D3D8
		internal ScriptAnalysis(string path, ExecutionContext context)
		{
			ModuleIntrinsics.Tracer.WriteLine("Analyzing path: " + path, new object[0]);
			try
			{
				bool isUnc = new Uri(path).IsUnc;
				if (isUnc && context.CurrentCommandProcessor.CommandRuntime != null)
				{
					ProgressRecord progressRecord = new ProgressRecord(0, Modules.ScriptAnalysisPreparing, string.Format(CultureInfo.InvariantCulture, Modules.ScriptAnalysisModule, new object[]
					{
						path
					}));
					progressRecord.RecordType = ProgressRecordType.Processing;
					context.CurrentCommandProcessor.CommandRuntime.WriteProgress((long)base.GetType().FullName.GetHashCode(), progressRecord);
				}
			}
			catch (UriFormatException)
			{
			}
			catch (InvalidOperationException)
			{
			}
			string input = File.ReadAllText(path);
			Token[] array = null;
			ParseError[] array2 = null;
			Ast ast = Parser.ParseInput(input, out array, out array2);
			ExportVisitor exportVisitor = new ExportVisitor();
			ast.Visit(exportVisitor);
			this.DiscoveredExports = exportVisitor.DiscoveredExports;
			this.DiscoveredAliases = new Dictionary<string, string>();
			this.DiscoveredModules = exportVisitor.DiscoveredModules;
			this.DiscoveredCommandFilters = exportVisitor.DiscoveredCommandFilters;
			if (this.DiscoveredCommandFilters.Count == 0)
			{
				this.DiscoveredCommandFilters.Add("*");
			}
			else
			{
				List<WildcardPattern> list = new List<WildcardPattern>();
				foreach (string pattern in this.DiscoveredCommandFilters)
				{
					list.Add(new WildcardPattern(pattern));
				}
				foreach (string text in exportVisitor.DiscoveredAliases.Keys)
				{
					if (SessionStateUtilities.MatchesAnyWildcardPattern(text, list, false))
					{
						this.DiscoveredAliases[text] = exportVisitor.DiscoveredAliases[text];
					}
				}
			}
			this.AddsSelfToPath = exportVisitor.AddsSelfToPath;
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0003F3E0 File Offset: 0x0003D5E0
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x0003F3E8 File Offset: 0x0003D5E8
		internal List<string> DiscoveredExports { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0003F3F1 File Offset: 0x0003D5F1
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x0003F3F9 File Offset: 0x0003D5F9
		internal Dictionary<string, string> DiscoveredAliases { get; set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0003F402 File Offset: 0x0003D602
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0003F40A File Offset: 0x0003D60A
		internal List<RequiredModuleInfo> DiscoveredModules { get; set; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0003F413 File Offset: 0x0003D613
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x0003F41B File Offset: 0x0003D61B
		internal List<string> DiscoveredCommandFilters { get; set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0003F424 File Offset: 0x0003D624
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x0003F42C File Offset: 0x0003D62C
		internal bool AddsSelfToPath { get; set; }
	}
}
