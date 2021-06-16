using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Management.Automation.Language
{
	// Token: 0x02000592 RID: 1426
	public sealed class CommentHelpInfo
	{
		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06003B1F RID: 15135 RVA: 0x0013718F File Offset: 0x0013538F
		// (set) Token: 0x06003B20 RID: 15136 RVA: 0x00137197 File Offset: 0x00135397
		public string Synopsis { get; internal set; }

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06003B21 RID: 15137 RVA: 0x001371A0 File Offset: 0x001353A0
		// (set) Token: 0x06003B22 RID: 15138 RVA: 0x001371A8 File Offset: 0x001353A8
		public string Description { get; internal set; }

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x001371B1 File Offset: 0x001353B1
		// (set) Token: 0x06003B24 RID: 15140 RVA: 0x001371B9 File Offset: 0x001353B9
		public string Notes { get; internal set; }

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06003B25 RID: 15141 RVA: 0x001371C2 File Offset: 0x001353C2
		// (set) Token: 0x06003B26 RID: 15142 RVA: 0x001371CA File Offset: 0x001353CA
		public IDictionary<string, string> Parameters { get; internal set; }

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06003B27 RID: 15143 RVA: 0x001371D3 File Offset: 0x001353D3
		// (set) Token: 0x06003B28 RID: 15144 RVA: 0x001371DB File Offset: 0x001353DB
		public ReadOnlyCollection<string> Links { get; internal set; }

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06003B29 RID: 15145 RVA: 0x001371E4 File Offset: 0x001353E4
		// (set) Token: 0x06003B2A RID: 15146 RVA: 0x001371EC File Offset: 0x001353EC
		public ReadOnlyCollection<string> Examples { get; internal set; }

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06003B2B RID: 15147 RVA: 0x001371F5 File Offset: 0x001353F5
		// (set) Token: 0x06003B2C RID: 15148 RVA: 0x001371FD File Offset: 0x001353FD
		public ReadOnlyCollection<string> Inputs { get; internal set; }

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06003B2D RID: 15149 RVA: 0x00137206 File Offset: 0x00135406
		// (set) Token: 0x06003B2E RID: 15150 RVA: 0x0013720E File Offset: 0x0013540E
		public ReadOnlyCollection<string> Outputs { get; internal set; }

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06003B2F RID: 15151 RVA: 0x00137217 File Offset: 0x00135417
		// (set) Token: 0x06003B30 RID: 15152 RVA: 0x0013721F File Offset: 0x0013541F
		public string Component { get; internal set; }

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06003B31 RID: 15153 RVA: 0x00137228 File Offset: 0x00135428
		// (set) Token: 0x06003B32 RID: 15154 RVA: 0x00137230 File Offset: 0x00135430
		public string Role { get; internal set; }

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06003B33 RID: 15155 RVA: 0x00137239 File Offset: 0x00135439
		// (set) Token: 0x06003B34 RID: 15156 RVA: 0x00137241 File Offset: 0x00135441
		public string Functionality { get; internal set; }

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06003B35 RID: 15157 RVA: 0x0013724A File Offset: 0x0013544A
		// (set) Token: 0x06003B36 RID: 15158 RVA: 0x00137252 File Offset: 0x00135452
		public string ForwardHelpTargetName { get; internal set; }

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06003B37 RID: 15159 RVA: 0x0013725B File Offset: 0x0013545B
		// (set) Token: 0x06003B38 RID: 15160 RVA: 0x00137263 File Offset: 0x00135463
		public string ForwardHelpCategory { get; internal set; }

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06003B39 RID: 15161 RVA: 0x0013726C File Offset: 0x0013546C
		// (set) Token: 0x06003B3A RID: 15162 RVA: 0x00137274 File Offset: 0x00135474
		public string RemoteHelpRunspace { get; internal set; }

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06003B3B RID: 15163 RVA: 0x0013727D File Offset: 0x0013547D
		// (set) Token: 0x06003B3C RID: 15164 RVA: 0x00137285 File Offset: 0x00135485
		public string MamlHelpFile { get; internal set; }

		// Token: 0x06003B3D RID: 15165 RVA: 0x00137290 File Offset: 0x00135490
		public string GetCommentBlock()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<#");
			if (!string.IsNullOrEmpty(this.Synopsis))
			{
				stringBuilder.AppendLine(".SYNOPSIS");
				stringBuilder.AppendLine(this.Synopsis);
			}
			if (!string.IsNullOrEmpty(this.Description))
			{
				stringBuilder.AppendLine(".DESCRIPTION");
				stringBuilder.AppendLine(this.Description);
			}
			foreach (KeyValuePair<string, string> keyValuePair in this.Parameters)
			{
				stringBuilder.Append(".PARAMETER ");
				stringBuilder.AppendLine(keyValuePair.Key);
				stringBuilder.AppendLine(keyValuePair.Value);
			}
			for (int i = 0; i < this.Inputs.Count; i++)
			{
				string value = this.Inputs[i];
				stringBuilder.AppendLine(".INPUTS");
				stringBuilder.AppendLine(value);
			}
			for (int j = 0; j < this.Outputs.Count; j++)
			{
				string value2 = this.Outputs[j];
				stringBuilder.AppendLine(".OUTPUTS");
				stringBuilder.AppendLine(value2);
			}
			if (!string.IsNullOrEmpty(this.Notes))
			{
				stringBuilder.AppendLine(".NOTES");
				stringBuilder.AppendLine(this.Notes);
			}
			for (int k = 0; k < this.Examples.Count; k++)
			{
				string value3 = this.Examples[k];
				stringBuilder.AppendLine(".EXAMPLE");
				stringBuilder.AppendLine(value3);
			}
			for (int l = 0; l < this.Links.Count; l++)
			{
				string value4 = this.Links[l];
				stringBuilder.AppendLine(".LINK");
				stringBuilder.AppendLine(value4);
			}
			if (!string.IsNullOrEmpty(this.ForwardHelpTargetName))
			{
				stringBuilder.Append(".FORWARDHELPTARGETNAME ");
				stringBuilder.AppendLine(this.ForwardHelpTargetName);
			}
			if (!string.IsNullOrEmpty(this.ForwardHelpCategory))
			{
				stringBuilder.Append(".FORWARDHELPCATEGORY ");
				stringBuilder.AppendLine(this.ForwardHelpCategory);
			}
			if (!string.IsNullOrEmpty(this.RemoteHelpRunspace))
			{
				stringBuilder.Append(".REMOTEHELPRUNSPACE ");
				stringBuilder.AppendLine(this.RemoteHelpRunspace);
			}
			if (!string.IsNullOrEmpty(this.Component))
			{
				stringBuilder.AppendLine(".COMPONENT");
				stringBuilder.AppendLine(this.Component);
			}
			if (!string.IsNullOrEmpty(this.Role))
			{
				stringBuilder.AppendLine(".ROLE");
				stringBuilder.AppendLine(this.Role);
			}
			if (!string.IsNullOrEmpty(this.Functionality))
			{
				stringBuilder.AppendLine(".FUNCTIONALITY");
				stringBuilder.AppendLine(this.Functionality);
			}
			if (!string.IsNullOrEmpty(this.MamlHelpFile))
			{
				stringBuilder.Append(".EXTERNALHELP ");
				stringBuilder.AppendLine(this.MamlHelpFile);
			}
			stringBuilder.AppendLine("#>");
			return stringBuilder.ToString();
		}
	}
}
