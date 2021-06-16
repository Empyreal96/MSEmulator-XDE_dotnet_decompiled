using System;
using System.Globalization;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x02000099 RID: 153
	internal class ProcessStreamReader
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x000244AC File Offset: 0x000226AC
		internal ProcessStreamReader(StreamReader streamReader, string processPath, bool isOutput, PipelineWriter writer, ProcessOutputReader processOutputReader)
		{
			this.streamReader = streamReader;
			this.processPath = processPath;
			this.isOutput = isOutput;
			this.writer = writer;
			this.processOutputReader = processOutputReader;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000244DC File Offset: 0x000226DC
		internal void Start()
		{
			this.thread = new Thread(new ThreadStart(this.ReaderStartProc));
			if (this.isOutput)
			{
				this.thread.Name = string.Format(CultureInfo.InvariantCulture, "{0} :Output Reader", new object[]
				{
					this.processPath
				});
			}
			else
			{
				this.thread.Name = string.Format(CultureInfo.InvariantCulture, "{0} :Error Reader", new object[]
				{
					this.processPath
				});
			}
			this.thread.Start();
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002456B File Offset: 0x0002276B
		internal void Done()
		{
			if (this.thread != null)
			{
				this.thread.Join();
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00024580 File Offset: 0x00022780
		private void ReaderStartProc()
		{
			try
			{
				this.ReaderStartProcHelper();
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			finally
			{
				this.processOutputReader.ReaderDone(this.isOutput);
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000245D0 File Offset: 0x000227D0
		private void ReaderStartProcHelper()
		{
			string text = this.streamReader.ReadLine();
			if (text == null)
			{
				return;
			}
			if (!text.Equals("#< CLIXML", StringComparison.Ordinal))
			{
				this.ReadText(text);
				return;
			}
			this.ReadXml();
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0002460C File Offset: 0x0002280C
		private void ReadText(string line)
		{
			if (this.isOutput)
			{
				while (line != null)
				{
					this.AddObjectToWriter(line, MinishellStream.Output);
					line = this.streamReader.ReadLine();
				}
				return;
			}
			ErrorRecord data = new ErrorRecord(new RemoteException(line), "NativeCommandError", ErrorCategory.NotSpecified, line);
			this.AddObjectToWriter(data, MinishellStream.Error);
			char[] array = new char[4096];
			int charCount;
			while ((charCount = this.streamReader.Read(array, 0, array.Length)) != 0)
			{
				StringBuilder stringBuilder = new StringBuilder().Append(array, 0, charCount);
				this.AddObjectToWriter(new ErrorRecord(new RemoteException(stringBuilder.ToString()), "NativeCommandErrorMessage", ErrorCategory.NotSpecified, null), MinishellStream.Error);
			}
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000246A8 File Offset: 0x000228A8
		private void ReadXml()
		{
			try
			{
				XmlReader reader = XmlReader.Create(this.streamReader, InternalDeserializer.XmlReaderSettingsForCliXml);
				Deserializer deserializer = new Deserializer(reader);
				while (!deserializer.Done())
				{
					string text;
					object obj = deserializer.Deserialize(out text);
					MinishellStream minishellStream = MinishellStream.Unknown;
					if (text != null)
					{
						minishellStream = StringToMinishellStreamConverter.ToMinishellStream(text);
					}
					if (minishellStream == MinishellStream.Unknown)
					{
						minishellStream = (this.isOutput ? MinishellStream.Output : MinishellStream.Error);
					}
					if (minishellStream == MinishellStream.Output || obj != null)
					{
						if (minishellStream == MinishellStream.Error)
						{
							if (obj is PSObject)
							{
								obj = ErrorRecord.FromPSObjectForRemoting(PSObject.AsPSObject(obj));
							}
							else
							{
								string text2 = null;
								try
								{
									text2 = (string)LanguagePrimitives.ConvertTo(obj, typeof(string), CultureInfo.InvariantCulture);
								}
								catch (PSInvalidCastException)
								{
									continue;
								}
								obj = new ErrorRecord(new RemoteException(text2), "NativeCommandError", ErrorCategory.NotSpecified, text2);
							}
						}
						else if (minishellStream == MinishellStream.Information)
						{
							if (obj is PSObject)
							{
								obj = InformationRecord.FromPSObjectForRemoting(PSObject.AsPSObject(obj));
							}
							else
							{
								string messageData = null;
								try
								{
									messageData = (string)LanguagePrimitives.ConvertTo(obj, typeof(string), CultureInfo.InvariantCulture);
								}
								catch (PSInvalidCastException)
								{
									continue;
								}
								obj = new InformationRecord(messageData, null);
							}
						}
						else
						{
							if (minishellStream != MinishellStream.Debug && minishellStream != MinishellStream.Verbose)
							{
								if (minishellStream != MinishellStream.Warning)
								{
									goto IL_121;
								}
							}
							try
							{
								obj = LanguagePrimitives.ConvertTo(obj, typeof(string), CultureInfo.InvariantCulture);
							}
							catch (PSInvalidCastException)
							{
								continue;
							}
						}
						IL_121:
						this.AddObjectToWriter(obj, minishellStream);
					}
				}
			}
			catch (XmlException ex)
			{
				string cliXmlError = NativeCP.CliXmlError;
				string message = string.Format(null, cliXmlError, new object[]
				{
					this.isOutput ? MinishellStream.Output : MinishellStream.Error,
					this.processPath,
					ex.Message
				});
				XmlException exception = new XmlException(message, ex);
				ErrorRecord data = new ErrorRecord(exception, "ProcessStreamReader_CliXmlError", ErrorCategory.SyntaxError, this.processPath);
				this.AddObjectToWriter(data, MinishellStream.Error);
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x000248C4 File Offset: 0x00022AC4
		private void AddObjectToWriter(object data, MinishellStream stream)
		{
			try
			{
				ProcessOutputObject obj = new ProcessOutputObject(data, stream);
				lock (this.writer)
				{
					this.writer.Write(obj);
				}
			}
			catch (PipelineClosedException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x04000355 RID: 853
		private StreamReader streamReader;

		// Token: 0x04000356 RID: 854
		private bool isOutput;

		// Token: 0x04000357 RID: 855
		private PipelineWriter writer;

		// Token: 0x04000358 RID: 856
		private string processPath;

		// Token: 0x04000359 RID: 857
		private ProcessOutputReader processOutputReader;

		// Token: 0x0400035A RID: 858
		private Thread thread;
	}
}
