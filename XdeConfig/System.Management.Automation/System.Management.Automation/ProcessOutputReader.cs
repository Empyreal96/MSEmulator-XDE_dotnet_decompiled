using System;
using System.Diagnostics;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000098 RID: 152
	internal class ProcessOutputReader
	{
		// Token: 0x06000765 RID: 1893 RVA: 0x0002429B File Offset: 0x0002249B
		internal ProcessOutputReader(Process process, string processPath, bool redirectOutput, bool redirectError)
		{
			this.process = process;
			this.processPath = processPath;
			this.redirectOutput = redirectOutput;
			this.redirectError = redirectError;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000242CC File Offset: 0x000224CC
		internal void Start()
		{
			this.processOutput = new ObjectStream(128);
			lock (this.readerLock)
			{
				if (this.redirectOutput)
				{
					this.readerCount++;
					this.outputReader = new ProcessStreamReader(this.process.StandardOutput, this.processPath, true, this.processOutput.ObjectWriter, this);
					this.outputReader.Start();
				}
				if (this.redirectError)
				{
					this.readerCount++;
					this.errorReader = new ProcessStreamReader(this.process.StandardError, this.processPath, false, this.processOutput.ObjectWriter, this);
					this.errorReader.Start();
				}
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x000243AC File Offset: 0x000225AC
		internal void Stop()
		{
			if (this.processOutput != null)
			{
				try
				{
					this.processOutput.ObjectReader.Close();
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
				try
				{
					this.processOutput.Close();
				}
				catch (Exception e2)
				{
					CommandProcessorBase.CheckForSevereException(e2);
				}
			}
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00024410 File Offset: 0x00022610
		internal void Done()
		{
			if (this.outputReader != null)
			{
				this.outputReader.Done();
			}
			if (this.errorReader != null)
			{
				this.errorReader.Done();
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00024438 File Offset: 0x00022638
		internal object Read()
		{
			return this.processOutput.ObjectReader.Read();
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002444C File Offset: 0x0002264C
		internal void ReaderDone(bool isOutput)
		{
			int num;
			lock (this.readerLock)
			{
				num = --this.readerCount;
			}
			if (num == 0)
			{
				this.processOutput.ObjectWriter.Close();
			}
		}

		// Token: 0x0400034C RID: 844
		private Process process;

		// Token: 0x0400034D RID: 845
		private string processPath;

		// Token: 0x0400034E RID: 846
		private bool redirectOutput;

		// Token: 0x0400034F RID: 847
		private bool redirectError;

		// Token: 0x04000350 RID: 848
		private ProcessStreamReader outputReader;

		// Token: 0x04000351 RID: 849
		private ProcessStreamReader errorReader;

		// Token: 0x04000352 RID: 850
		private ObjectStream processOutput;

		// Token: 0x04000353 RID: 851
		private object readerLock = new object();

		// Token: 0x04000354 RID: 852
		private int readerCount;
	}
}
