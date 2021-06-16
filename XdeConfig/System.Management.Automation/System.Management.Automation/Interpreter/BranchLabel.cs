using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200066A RID: 1642
	internal sealed class BranchLabel
	{
		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x001772F2 File Offset: 0x001754F2
		// (set) Token: 0x060045FC RID: 17916 RVA: 0x001772FA File Offset: 0x001754FA
		internal int LabelIndex
		{
			get
			{
				return this._labelIndex;
			}
			set
			{
				this._labelIndex = value;
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x00177303 File Offset: 0x00175503
		internal bool HasRuntimeLabel
		{
			get
			{
				return this._labelIndex != int.MinValue;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x060045FE RID: 17918 RVA: 0x00177315 File Offset: 0x00175515
		internal int TargetIndex
		{
			get
			{
				return this._targetIndex;
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x060045FF RID: 17919 RVA: 0x0017731D File Offset: 0x0017551D
		internal int StackDepth
		{
			get
			{
				return this._stackDepth;
			}
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x00177325 File Offset: 0x00175525
		internal RuntimeLabel ToRuntimeLabel()
		{
			return new RuntimeLabel(this._targetIndex, this._continuationStackDepth, this._stackDepth);
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x00177340 File Offset: 0x00175540
		internal void Mark(InstructionList instructions)
		{
			this._stackDepth = instructions.CurrentStackDepth;
			this._continuationStackDepth = instructions.CurrentContinuationsDepth;
			this._targetIndex = instructions.Count;
			if (this._forwardBranchFixups != null)
			{
				foreach (int branchIndex in this._forwardBranchFixups)
				{
					this.FixupBranch(instructions, branchIndex);
				}
				this._forwardBranchFixups = null;
			}
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x001773C8 File Offset: 0x001755C8
		internal void AddBranch(InstructionList instructions, int branchIndex)
		{
			if (this._targetIndex == -2147483648)
			{
				if (this._forwardBranchFixups == null)
				{
					this._forwardBranchFixups = new List<int>();
				}
				this._forwardBranchFixups.Add(branchIndex);
				return;
			}
			this.FixupBranch(instructions, branchIndex);
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x001773FF File Offset: 0x001755FF
		internal void FixupBranch(InstructionList instructions, int branchIndex)
		{
			instructions.FixupBranch(branchIndex, this._targetIndex - branchIndex);
		}

		// Token: 0x0400229C RID: 8860
		internal const int UnknownIndex = -2147483648;

		// Token: 0x0400229D RID: 8861
		internal const int UnknownDepth = -2147483648;

		// Token: 0x0400229E RID: 8862
		internal int _labelIndex = int.MinValue;

		// Token: 0x0400229F RID: 8863
		internal int _targetIndex = int.MinValue;

		// Token: 0x040022A0 RID: 8864
		internal int _stackDepth = int.MinValue;

		// Token: 0x040022A1 RID: 8865
		internal int _continuationStackDepth = int.MinValue;

		// Token: 0x040022A2 RID: 8866
		private List<int> _forwardBranchFixups;
	}
}
