using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000015 RID: 21
	internal class CookedDataRuns
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00004427 File Offset: 0x00002627
		public CookedDataRuns()
		{
			this._runs = new List<CookedDataRun>();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004445 File Offset: 0x00002645
		public CookedDataRuns(IEnumerable<DataRun> rawRuns, NonResidentAttributeRecord attributeExtent)
		{
			this._runs = new List<CookedDataRun>();
			this.Append(rawRuns, attributeExtent);
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000446B File Offset: 0x0000266B
		public int Count
		{
			get
			{
				return this._runs.Count;
			}
		}

		// Token: 0x17000021 RID: 33
		public CookedDataRun this[int index]
		{
			get
			{
				return this._runs[index];
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00004486 File Offset: 0x00002686
		public CookedDataRun Last
		{
			get
			{
				if (this._runs.Count == 0)
				{
					return null;
				}
				return this._runs[this._runs.Count - 1];
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000044B0 File Offset: 0x000026B0
		public long NextVirtualCluster
		{
			get
			{
				if (this._runs.Count == 0)
				{
					return 0L;
				}
				int index = this._runs.Count - 1;
				return this._runs[index].StartVcn + this._runs[index].Length;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004500 File Offset: 0x00002700
		public int FindDataRun(long vcn, int startIdx)
		{
			int count = this._runs.Count;
			if (count > 0)
			{
				CookedDataRun cookedDataRun = this._runs[count - 1];
				if (vcn >= cookedDataRun.StartVcn)
				{
					if (cookedDataRun.StartVcn + cookedDataRun.Length > vcn)
					{
						return count - 1;
					}
					throw new IOException("Looking for VCN outside of data runs");
				}
				else
				{
					for (int i = startIdx; i < count; i++)
					{
						cookedDataRun = this._runs[i];
						if (cookedDataRun.StartVcn + cookedDataRun.Length > vcn)
						{
							return i;
						}
					}
				}
			}
			throw new IOException("Looking for VCN outside of data runs");
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000458C File Offset: 0x0000278C
		public void Append(DataRun rawRun, NonResidentAttributeRecord attributeExtent)
		{
			CookedDataRun last = this.Last;
			this._runs.Add(new CookedDataRun(rawRun, this.NextVirtualCluster, (last == null) ? 0L : last.StartLcn, attributeExtent));
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000045C8 File Offset: 0x000027C8
		public void Append(IEnumerable<DataRun> rawRuns, NonResidentAttributeRecord attributeExtent)
		{
			long num = this.NextVirtualCluster;
			long num2 = 0L;
			foreach (DataRun dataRun in rawRuns)
			{
				this._runs.Add(new CookedDataRun(dataRun, num, num2, attributeExtent));
				num += dataRun.RunLength;
				num2 += dataRun.RunOffset;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000463C File Offset: 0x0000283C
		public void MakeSparse(int index)
		{
			if (index < this._firstDirty)
			{
				this._firstDirty = index;
			}
			if (index > this._lastDirty)
			{
				this._lastDirty = index;
			}
			long num = (index == 0) ? 0L : this._runs[index - 1].StartLcn;
			CookedDataRun cookedDataRun = this._runs[index];
			if (cookedDataRun.IsSparse)
			{
				throw new ArgumentException("Run is already sparse", "index");
			}
			this._runs[index] = new CookedDataRun(new DataRun(0L, cookedDataRun.Length, true), cookedDataRun.StartVcn, num, cookedDataRun.AttributeExtent);
			cookedDataRun.AttributeExtent.ReplaceRun(cookedDataRun.DataRun, this._runs[index].DataRun);
			for (int i = index + 1; i < this._runs.Count; i++)
			{
				if (!this._runs[i].IsSparse)
				{
					this._runs[i].DataRun.RunOffset += cookedDataRun.StartLcn - num;
					return;
				}
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004748 File Offset: 0x00002948
		public void MakeNonSparse(int index, IEnumerable<DataRun> rawRuns)
		{
			if (index < this._firstDirty)
			{
				this._firstDirty = index;
			}
			if (index > this._lastDirty)
			{
				this._lastDirty = index;
			}
			long num = (index == 0) ? 0L : this._runs[index - 1].StartLcn;
			CookedDataRun cookedDataRun = this._runs[index];
			if (!cookedDataRun.IsSparse)
			{
				throw new ArgumentException("Run is already non-sparse", "index");
			}
			this._runs.RemoveAt(index);
			int num2 = cookedDataRun.AttributeExtent.RemoveRun(cookedDataRun.DataRun);
			CookedDataRun cookedDataRun2 = null;
			long num3 = num;
			long num4 = cookedDataRun.StartVcn;
			foreach (DataRun dataRun in rawRuns)
			{
				CookedDataRun cookedDataRun3 = new CookedDataRun(dataRun, num4, num3, cookedDataRun.AttributeExtent);
				this._runs.Insert(index, cookedDataRun3);
				cookedDataRun.AttributeExtent.InsertRun(num2, dataRun);
				num4 += dataRun.RunLength;
				num3 += dataRun.RunOffset;
				cookedDataRun2 = cookedDataRun3;
				num2++;
				index++;
			}
			for (int i = index; i < this._runs.Count; i++)
			{
				if (!this._runs[i].IsSparse)
				{
					this._runs[i].DataRun.RunOffset = this._runs[i].StartLcn - cookedDataRun2.StartLcn;
					return;
				}
				this._runs[i].StartLcn = cookedDataRun2.StartLcn;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000048E0 File Offset: 0x00002AE0
		public void SplitRun(int runIdx, long vcn)
		{
			if (runIdx < this._firstDirty)
			{
				this._firstDirty = runIdx;
			}
			if (runIdx > this._lastDirty)
			{
				this._lastDirty = runIdx;
			}
			CookedDataRun cookedDataRun = this._runs[runIdx];
			if (cookedDataRun.StartVcn >= vcn || cookedDataRun.StartVcn + cookedDataRun.Length <= vcn)
			{
				throw new ArgumentException("Attempt to split run outside of it's range", "vcn");
			}
			long num = vcn - cookedDataRun.StartVcn;
			long num2 = cookedDataRun.IsSparse ? 0L : num;
			CookedDataRun cookedDataRun2 = new CookedDataRun(new DataRun(num2, cookedDataRun.Length - num, cookedDataRun.IsSparse), vcn, cookedDataRun.StartLcn, cookedDataRun.AttributeExtent);
			cookedDataRun.Length = num;
			this._runs.Insert(runIdx + 1, cookedDataRun2);
			cookedDataRun.AttributeExtent.InsertRun(cookedDataRun.DataRun, cookedDataRun2.DataRun);
			for (int i = runIdx + 2; i < this._runs.Count; i++)
			{
				if (!this._runs[i].IsSparse)
				{
					this._runs[i].DataRun.RunOffset -= num2;
					return;
				}
				this._runs[i].StartLcn += num2;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004A1C File Offset: 0x00002C1C
		public void TruncateAt(int index)
		{
			while (index < this._runs.Count)
			{
				this._runs[index].AttributeExtent.RemoveRun(this._runs[index].DataRun);
				this._runs.RemoveAt(index);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004A70 File Offset: 0x00002C70
		internal void CollapseRuns()
		{
			int num = (this._firstDirty > 1) ? (this._firstDirty - 1) : 0;
			while (num < this._runs.Count - 1 && num <= this._lastDirty + 1)
			{
				if (this._runs[num].IsSparse && this._runs[num + 1].IsSparse)
				{
					this._runs[num].Length += this._runs[num + 1].Length;
					this._runs[num + 1].AttributeExtent.RemoveRun(this._runs[num + 1].DataRun);
					this._runs.RemoveAt(num + 1);
				}
				else if (!this._runs[num].IsSparse && !this._runs[num].IsSparse && this._runs[num].StartLcn + this._runs[num].Length == this._runs[num + 1].StartLcn)
				{
					this._runs[num].Length += this._runs[num + 1].Length;
					this._runs[num + 1].AttributeExtent.RemoveRun(this._runs[num + 1].DataRun);
					this._runs.RemoveAt(num + 1);
					for (int i = num + 1; i < this._runs.Count; i++)
					{
						if (!this._runs[i].IsSparse)
						{
							this._runs[i].DataRun.RunOffset = this._runs[i].StartLcn - this._runs[num].StartLcn;
							break;
						}
						this._runs[i].StartLcn = this._runs[num].StartLcn;
					}
				}
				else
				{
					num++;
				}
			}
			this._firstDirty = int.MaxValue;
			this._lastDirty = 0;
		}

		// Token: 0x0400006D RID: 109
		private int _firstDirty = int.MaxValue;

		// Token: 0x0400006E RID: 110
		private int _lastDirty;

		// Token: 0x0400006F RID: 111
		private readonly List<CookedDataRun> _runs;
	}
}
