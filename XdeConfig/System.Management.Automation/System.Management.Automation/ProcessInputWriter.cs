using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Management.Automation.Internal;
using System.Text;
using System.Threading;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x02000097 RID: 151
	internal class ProcessInputWriter
	{
		// Token: 0x0600075B RID: 1883 RVA: 0x0002400C File Offset: 0x0002220C
		internal ProcessInputWriter(InternalCommand command)
		{
			this.command = command;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00024026 File Offset: 0x00022226
		internal void Add(object input)
		{
			this.inputList.Add(input);
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x00024035 File Offset: 0x00022235
		internal int Count
		{
			get
			{
				return this.inputList.Count;
			}
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00024044 File Offset: 0x00022244
		internal void Start(Process process, NativeCommandIOFormat inputFormat)
		{
			Encoding encoding = this.command.Context.GetVariableValue(SpecialVariables.OutputEncodingVarPath) as Encoding;
			if (encoding == null)
			{
				encoding = Encoding.ASCII;
			}
			this.streamWriter = new StreamWriter(process.StandardInput.BaseStream, encoding);
			this.inputFormat = inputFormat;
			if (inputFormat == NativeCommandIOFormat.Text)
			{
				this.ConvertToString();
			}
			this.inputThread = new Thread(new ThreadStart(this.WriterThreadProc));
			this.inputThread.Start();
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x000240BE File Offset: 0x000222BE
		internal void Stop()
		{
			this.stopping = true;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x000240C7 File Offset: 0x000222C7
		internal void Done()
		{
			if (this.inputThread != null)
			{
				this.inputThread.Join();
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x000240DC File Offset: 0x000222DC
		private void WriterThreadProc()
		{
			try
			{
				if (this.inputFormat == NativeCommandIOFormat.Text)
				{
					this.WriteTextInput();
				}
				else
				{
					this.WriteXmlInput();
				}
			}
			catch (IOException)
			{
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00024114 File Offset: 0x00022314
		private void WriteTextInput()
		{
			try
			{
				foreach (object obj in this.inputList)
				{
					if (this.stopping)
					{
						break;
					}
					string value = PSObject.ToStringParser(this.command.Context, obj);
					this.streamWriter.Write(value);
				}
			}
			finally
			{
				this.streamWriter.Dispose();
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000241A4 File Offset: 0x000223A4
		private void WriteXmlInput()
		{
			try
			{
				this.streamWriter.WriteLine("#< CLIXML");
				XmlWriter writer = XmlWriter.Create(this.streamWriter);
				Serializer serializer = new Serializer(writer);
				foreach (object source in this.inputList)
				{
					if (this.stopping)
					{
						return;
					}
					serializer.Serialize(source);
				}
				serializer.Done();
			}
			finally
			{
				this.streamWriter.Dispose();
			}
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00024248 File Offset: 0x00022448
		private void ConvertToString()
		{
			PipelineProcessor pipelineProcessor = new PipelineProcessor();
			pipelineProcessor.Add(this.command.Context.CreateCommand("out-string", false));
			object[] c = (object[])pipelineProcessor.Execute(this.inputList.ToArray());
			this.inputList = new ArrayList(c);
		}

		// Token: 0x04000346 RID: 838
		private InternalCommand command;

		// Token: 0x04000347 RID: 839
		private ArrayList inputList = new ArrayList();

		// Token: 0x04000348 RID: 840
		private StreamWriter streamWriter;

		// Token: 0x04000349 RID: 841
		private NativeCommandIOFormat inputFormat;

		// Token: 0x0400034A RID: 842
		private Thread inputThread;

		// Token: 0x0400034B RID: 843
		private bool stopping;
	}
}
