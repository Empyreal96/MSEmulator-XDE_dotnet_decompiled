using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Provider;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000765 RID: 1893
	internal class FileSystemContentReaderWriter : IContentReader, IContentWriter, IDisposable
	{
		// Token: 0x06004BA9 RID: 19369 RVA: 0x0018BFB4 File Offset: 0x0018A1B4
		public FileSystemContentReaderWriter(string path, FileMode mode, FileAccess access, FileShare share, Encoding encoding, bool usingByteEncoding, bool waitForChanges, CmdletProvider provider, bool isRawStream) : this(path, null, mode, access, share, encoding, usingByteEncoding, waitForChanges, provider, isRawStream)
		{
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x0018BFD8 File Offset: 0x0018A1D8
		public FileSystemContentReaderWriter(string path, string streamName, FileMode mode, FileAccess access, FileShare share, Encoding encoding, bool usingByteEncoding, bool waitForChanges, CmdletProvider provider, bool isRawStream)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			FileSystemContentReaderWriter.tracer.WriteLine("path = {0}", new object[]
			{
				path
			});
			FileSystemContentReaderWriter.tracer.WriteLine("mode = {0}", new object[]
			{
				mode
			});
			FileSystemContentReaderWriter.tracer.WriteLine("access = {0}", new object[]
			{
				access
			});
			this.path = path;
			this.streamName = streamName;
			this.mode = mode;
			this.access = access;
			this.share = share;
			this.encoding = encoding;
			this.usingByteEncoding = usingByteEncoding;
			this.waitForChanges = waitForChanges;
			this.provider = provider;
			this.isRawStream = isRawStream;
			this.CreateStreams(path, streamName, mode, access, share, encoding);
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x0018C0C4 File Offset: 0x0018A2C4
		public FileSystemContentReaderWriter(string path, string streamName, FileMode mode, FileAccess access, FileShare share, Encoding encoding, bool usingByteEncoding, bool waitForChanges, CmdletProvider provider, bool isRawStream, bool suppressNewline) : this(path, streamName, mode, access, share, encoding, usingByteEncoding, waitForChanges, provider, isRawStream)
		{
			this.suppressNewline = suppressNewline;
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x0018C0F0 File Offset: 0x0018A2F0
		public FileSystemContentReaderWriter(string path, FileMode mode, FileAccess access, FileShare share, string delimiter, Encoding encoding, bool waitForChanges, CmdletProvider provider, bool isRawStream) : this(path, null, mode, access, share, encoding, false, waitForChanges, provider, isRawStream)
		{
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x0018C114 File Offset: 0x0018A314
		public FileSystemContentReaderWriter(string path, string streamName, FileMode mode, FileAccess access, FileShare share, string delimiter, Encoding encoding, bool waitForChanges, CmdletProvider provider, bool isRawStream) : this(path, streamName, mode, access, share, encoding, false, waitForChanges, provider, isRawStream)
		{
			this.delimiter = delimiter;
			this.usingDelimiter = true;
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x0018C148 File Offset: 0x0018A348
		public IList Read(long readCount)
		{
			if (this.isRawStream && this.waitForChanges)
			{
				throw PSTraceSource.NewInvalidOperationException(FileSystemProviderStrings.RawAndWaitCannotCoexist, new object[0]);
			}
			bool flag = this.waitForChanges;
			FileSystemContentReaderWriter.tracer.WriteLine("blocks requested = {0}", new object[]
			{
				readCount
			});
			ArrayList arrayList = new ArrayList();
			bool flag2 = readCount <= 0L;
			if (this._alreadyDetectEncoding && this.reader.BaseStream.Position == 0L)
			{
				Encoding currentEncoding = this.reader.CurrentEncoding;
				this.stream.Dispose();
				this.CreateStreams(this.path, null, this.mode, this.access, this.share, currentEncoding);
				this._alreadyDetectEncoding = false;
			}
			try
			{
				long num = 0L;
				while (num < readCount || flag2)
				{
					if (flag && this.provider.Stopping)
					{
						flag = false;
					}
					if (this.usingByteEncoding)
					{
						if (!this.ReadByteEncoded(flag, arrayList, false))
						{
							break;
						}
					}
					else if (this.usingDelimiter || this.isRawStream)
					{
						if (!this.ReadDelimited(flag, arrayList, false, this.delimiter))
						{
							break;
						}
					}
					else if (!this.ReadByLine(flag, arrayList, false))
					{
						break;
					}
					num += 1L;
				}
				FileSystemContentReaderWriter.tracer.WriteLine("blocks read = {0}", new object[]
				{
					arrayList.Count
				});
			}
			catch (Exception ex)
			{
				if (ex is IOException || ex is ArgumentException || ex is SecurityException || ex is UnauthorizedAccessException || ex is ArgumentNullException)
				{
					this.provider.WriteError(new ErrorRecord(ex, "GetContentReaderIOError", ErrorCategory.ReadError, this.path));
					return null;
				}
				throw;
			}
			return arrayList.ToArray();
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x0018C30C File Offset: 0x0018A50C
		internal IList ReadWithoutWaitingChanges(long readCount)
		{
			bool flag = this.waitForChanges;
			this.waitForChanges = false;
			IList result;
			try
			{
				result = this.Read(readCount);
			}
			finally
			{
				this.waitForChanges = flag;
			}
			return result;
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x0018C34C File Offset: 0x0018A54C
		internal void SeekItemsBackward(int backCount)
		{
			if (backCount < 0)
			{
				throw PSTraceSource.NewArgumentException("backCount");
			}
			if (this.isRawStream && this.waitForChanges)
			{
				throw PSTraceSource.NewInvalidOperationException(FileSystemProviderStrings.RawAndWaitCannotCoexist, new object[0]);
			}
			FileSystemContentReaderWriter.tracer.WriteLine("blocks seek backwards = {0}", new object[]
			{
				backCount
			});
			ArrayList arrayList = new ArrayList();
			if (this.reader != null)
			{
				this.Seek(0L, SeekOrigin.Begin);
				this.reader.Peek();
				this._alreadyDetectEncoding = true;
			}
			this.Seek(0L, SeekOrigin.End);
			if (backCount == 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char value in this.delimiter)
			{
				stringBuilder.Insert(0, value);
			}
			string text2 = stringBuilder.ToString();
			long num = 0L;
			string text3 = null;
			try
			{
				if (this.isRawStream)
				{
					this.Seek(0L, SeekOrigin.Begin);
				}
				else
				{
					while (num < (long)backCount)
					{
						if (this.usingByteEncoding)
						{
							if (!this.ReadByteEncoded(false, arrayList, true))
							{
								break;
							}
						}
						else if (this.usingDelimiter)
						{
							if (!this.ReadDelimited(false, arrayList, true, text2))
							{
								break;
							}
							text3 = (string)arrayList[0];
							if (num == 0L && text3.Equals(text2, StringComparison.Ordinal))
							{
								backCount++;
							}
						}
						else if (!this.ReadByLine(false, arrayList, true))
						{
							break;
						}
						arrayList.Clear();
						num += 1L;
					}
					if (!this.usingByteEncoding)
					{
						long num2 = this._backReader.GetCurrentPosition();
						if (this.usingDelimiter && num == (long)backCount && text3.EndsWith(text2, StringComparison.Ordinal))
						{
							num2 += (long)this._backReader.GetByteCount(this.delimiter);
						}
						this.Seek(num2, SeekOrigin.Begin);
					}
					FileSystemContentReaderWriter.tracer.WriteLine("blocks seek position = {0}", new object[]
					{
						this.stream.Position
					});
				}
			}
			catch (Exception ex)
			{
				if (!(ex is IOException) && !(ex is ArgumentException) && !(ex is SecurityException) && !(ex is UnauthorizedAccessException) && !(ex is ArgumentNullException))
				{
					throw;
				}
				this.provider.WriteError(new ErrorRecord(ex, "GetContentReaderIOError", ErrorCategory.ReadError, this.path));
			}
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x0018C598 File Offset: 0x0018A798
		private bool ReadByLine(bool waitChanges, ArrayList blocks, bool readBackward)
		{
			string text = readBackward ? this._backReader.ReadLine() : this.reader.ReadLine();
			if (text == null && waitChanges)
			{
				do
				{
					this.WaitForChanges(this.path, this.mode, this.access, this.share, this.reader.CurrentEncoding);
					text = this.reader.ReadLine();
				}
				while (text == null && !this.provider.Stopping);
			}
			if (text != null)
			{
				blocks.Add(text);
			}
			int num = readBackward ? this._backReader.Peek() : this.reader.Peek();
			return num != -1;
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x0018C63C File Offset: 0x0018A83C
		private bool ReadDelimited(bool waitChanges, ArrayList blocks, bool readBackward, string actualDelimiter)
		{
			char[] array = new char[actualDelimiter.Length];
			int num = actualDelimiter.Length;
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<char, int> dictionary = new Dictionary<char, int>();
			foreach (char c in actualDelimiter)
			{
				dictionary[c] = actualDelimiter.Length - actualDelimiter.LastIndexOf(c) - 1;
			}
			int num2;
			do
			{
				if (this.isRawStream)
				{
					string text = this.reader.ReadToEnd();
					num2 = text.Length;
					stringBuilder.Append(text);
				}
				else
				{
					array = new char[num];
					num2 = (readBackward ? this._backReader.Read(array, 0, num) : this.reader.Read(array, 0, num));
					if (num2 == 0 && waitChanges)
					{
						while (num2 < num && !this.provider.Stopping)
						{
							this.WaitForChanges(this.path, this.mode, this.access, this.share, this.reader.CurrentEncoding);
							num2 += this.reader.Read(array, 0, num - num2);
						}
					}
					if (num2 > 0)
					{
						stringBuilder.Append(array, 0, num2);
						if (dictionary.ContainsKey(stringBuilder[stringBuilder.Length - 1]))
						{
							num = dictionary[stringBuilder[stringBuilder.Length - 1]];
						}
						else
						{
							num = actualDelimiter.Length;
						}
						if (num == 0)
						{
							num = 1;
						}
					}
				}
			}
			while ((this.isRawStream && num2 != 0) || (stringBuilder.ToString().IndexOf(actualDelimiter, StringComparison.Ordinal) < 0 && num2 != 0));
			if (stringBuilder.Length > 0)
			{
				blocks.Add(stringBuilder.ToString());
			}
			int num3 = readBackward ? this._backReader.Peek() : this.reader.Peek();
			return num3 != -1 || (readBackward && stringBuilder.Length > 0);
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x0018C810 File Offset: 0x0018AA10
		private bool ReadByteEncoded(bool waitChanges, ArrayList blocks, bool readBack)
		{
			if (this.isRawStream)
			{
				byte[] array = new byte[this.stream.Length];
				int i = (int)this.stream.Length;
				int num = 0;
				while (i > 0)
				{
					int num2 = this.stream.Read(array, num, i);
					if (num2 == 0)
					{
						break;
					}
					num += num2;
					i -= num2;
				}
				if (num == 0)
				{
					return false;
				}
				blocks.Add(array);
				return true;
			}
			else if (readBack)
			{
				if (this.stream.Position == 0L)
				{
					return false;
				}
				this.stream.Position -= 1L;
				blocks.Add((byte)this.stream.ReadByte());
				this.stream.Position -= 1L;
				return true;
			}
			else
			{
				int num3 = this.stream.ReadByte();
				if (num3 == -1 && waitChanges)
				{
					this.WaitForChanges(this.path, this.mode, this.access, this.share, ClrFacade.GetDefaultEncoding());
					num3 = this.stream.ReadByte();
				}
				if (num3 != -1)
				{
					blocks.Add((byte)num3);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x0018C928 File Offset: 0x0018AB28
		private void CreateStreams(string filePath, string streamName, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, Encoding fileEncoding)
		{
			if (File.Exists(filePath) && this.provider.Force)
			{
				this.oldAttributes = File.GetAttributes(filePath);
				this.haveOldAttributes = true;
				FileAttributes fileAttributes = FileAttributes.Hidden;
				if ((fileAccess & FileAccess.Write) != (FileAccess)0)
				{
					fileAttributes |= FileAttributes.ReadOnly;
				}
				File.SetAttributes(this.path, File.GetAttributes(filePath) & ~fileAttributes);
			}
			FileAccess fileAccess2 = fileAccess;
			if ((fileAccess & FileAccess.Write) != (FileAccess)0)
			{
				fileAccess = FileAccess.ReadWrite;
			}
			try
			{
				if (!string.IsNullOrEmpty(streamName))
				{
					this.stream = AlternateDataStreamUtilities.CreateFileStream(filePath, streamName, fileMode, fileAccess, fileShare);
				}
				else
				{
					this.stream = new FileStream(filePath, fileMode, fileAccess, fileShare);
				}
			}
			catch (IOException)
			{
				if (!string.IsNullOrEmpty(streamName))
				{
					this.stream = AlternateDataStreamUtilities.CreateFileStream(filePath, streamName, fileMode, fileAccess2, fileShare);
				}
				else
				{
					this.stream = new FileStream(filePath, fileMode, fileAccess2, fileShare);
				}
			}
			if (!this.usingByteEncoding)
			{
				if ((fileAccess & FileAccess.Read) != (FileAccess)0)
				{
					this.reader = new StreamReader(this.stream, fileEncoding);
					this._backReader = new FileStreamBackReader(this.stream, fileEncoding);
				}
				if ((fileAccess & FileAccess.Write) != (FileAccess)0)
				{
					if (this.reader != null && (fileAccess & FileAccess.Read) != (FileAccess)0)
					{
						this.reader.Peek();
						fileEncoding = this.reader.CurrentEncoding;
					}
					this.writer = new StreamWriter(this.stream, fileEncoding);
				}
			}
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x0018CADC File Offset: 0x0018ACDC
		private void WaitForChanges(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, Encoding fileEncoding)
		{
			if (this.stream != null)
			{
				this.fileOffset = this.stream.Position;
				this.stream.Dispose();
			}
			FileInfo watchFile = new FileInfo(filePath);
			long length = watchFile.Length;
			using (FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(watchFile.DirectoryName, watchFile.Name))
			{
				ErrorEventArgs errorEventArgs = null;
				TaskCompletionSource<FileSystemEventArgs> tcs = new TaskCompletionSource<FileSystemEventArgs>();
				FileSystemEventHandler value = delegate(object source, FileSystemEventArgs e)
				{
					tcs.TrySetResult(e);
				};
				RenamedEventHandler value2 = delegate(object source, RenamedEventArgs e)
				{
					tcs.TrySetResult(e);
				};
				ErrorEventHandler value3 = delegate(object source, ErrorEventArgs e)
				{
					errorEventArgs = e;
					tcs.TrySetResult(new FileSystemEventArgs(WatcherChangeTypes.All, watchFile.DirectoryName, watchFile.Name));
				};
				fileSystemWatcher.Changed += value;
				fileSystemWatcher.Created += value;
				fileSystemWatcher.Deleted += value;
				fileSystemWatcher.Renamed += value2;
				fileSystemWatcher.Error += value3;
				try
				{
					fileSystemWatcher.EnableRaisingEvents = true;
					while (!this.provider.Stopping)
					{
						bool flag = tcs.Task.Wait(500);
						if (errorEventArgs != null)
						{
							throw errorEventArgs.GetException();
						}
						if (flag)
						{
							break;
						}
						watchFile.Refresh();
						if (length != watchFile.Length)
						{
							break;
						}
					}
				}
				finally
				{
					fileSystemWatcher.EnableRaisingEvents = false;
					fileSystemWatcher.Changed -= value;
					fileSystemWatcher.Created -= value;
					fileSystemWatcher.Deleted -= value;
					fileSystemWatcher.Renamed -= value2;
					fileSystemWatcher.Error -= value3;
				}
			}
			Thread.Sleep(100);
			this.CreateStreams(filePath, null, fileMode, fileAccess, fileShare, fileEncoding);
			if (this.fileOffset > this.stream.Length)
			{
				this.fileOffset = 0L;
			}
			this.stream.Seek(this.fileOffset, SeekOrigin.Begin);
			if (this.reader != null)
			{
				this.reader.DiscardBufferedData();
			}
			if (this._backReader != null)
			{
				this._backReader.DiscardBufferedData();
			}
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x0018CCFC File Offset: 0x0018AEFC
		public void Seek(long offset, SeekOrigin origin)
		{
			if (this.writer != null)
			{
				this.writer.Flush();
			}
			this.stream.Seek(offset, origin);
			if (this.writer != null)
			{
				this.writer.Flush();
			}
			if (this.reader != null)
			{
				this.reader.DiscardBufferedData();
			}
			if (this._backReader != null)
			{
				this._backReader.DiscardBufferedData();
			}
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x0018CD64 File Offset: 0x0018AF64
		public void Close()
		{
			bool flag = false;
			if (this.writer != null)
			{
				try
				{
					this.writer.Flush();
					this.writer.Dispose();
				}
				finally
				{
					flag = true;
				}
			}
			if (this.reader != null)
			{
				this.reader.Dispose();
				flag = true;
			}
			if (this._backReader != null)
			{
				this._backReader.Dispose();
				flag = true;
			}
			if (!flag)
			{
				this.stream.Flush();
				this.stream.Dispose();
			}
			if (this.haveOldAttributes && this.provider.Force)
			{
				File.SetAttributes(this.path, this.oldAttributes);
			}
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x0018CE14 File Offset: 0x0018B014
		public IList Write(IList content)
		{
			foreach (object obj in content)
			{
				object[] array = obj as object[];
				if (array != null)
				{
					foreach (object content2 in array)
					{
						this.WriteObject(content2);
					}
				}
				else
				{
					this.WriteObject(obj);
				}
			}
			return content;
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x0018CE98 File Offset: 0x0018B098
		private void WriteObject(object content)
		{
			if (content == null)
			{
				return;
			}
			if (this.usingByteEncoding)
			{
				try
				{
					byte value = (byte)content;
					this.stream.WriteByte(value);
					return;
				}
				catch (InvalidCastException)
				{
					throw PSTraceSource.NewArgumentException("content", FileSystemProviderStrings.ByteEncodingError, new object[0]);
				}
			}
			if (this.suppressNewline)
			{
				this.writer.Write(content.ToString());
				return;
			}
			this.writer.WriteLine(content.ToString());
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x0018CF18 File Offset: 0x0018B118
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x0018CF28 File Offset: 0x0018B128
		internal void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				if (this.stream != null)
				{
					this.stream.Dispose();
				}
				if (this.reader != null)
				{
					this.reader.Dispose();
				}
				if (this._backReader != null)
				{
					this._backReader.Dispose();
				}
				if (this.writer != null)
				{
					this.writer.Dispose();
				}
			}
		}

		// Token: 0x04002481 RID: 9345
		[TraceSource("FileSystemContentStream", "The provider content reader and writer for the file system")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("FileSystemContentStream", "The provider content reader and writer for the file system");

		// Token: 0x04002482 RID: 9346
		private string path;

		// Token: 0x04002483 RID: 9347
		private string streamName;

		// Token: 0x04002484 RID: 9348
		private FileMode mode;

		// Token: 0x04002485 RID: 9349
		private FileAccess access;

		// Token: 0x04002486 RID: 9350
		private FileShare share;

		// Token: 0x04002487 RID: 9351
		private Encoding encoding;

		// Token: 0x04002488 RID: 9352
		private CmdletProvider provider;

		// Token: 0x04002489 RID: 9353
		private FileStream stream;

		// Token: 0x0400248A RID: 9354
		private StreamReader reader;

		// Token: 0x0400248B RID: 9355
		private StreamWriter writer;

		// Token: 0x0400248C RID: 9356
		private bool usingByteEncoding;

		// Token: 0x0400248D RID: 9357
		private string delimiter = "\n";

		// Token: 0x0400248E RID: 9358
		private bool usingDelimiter;

		// Token: 0x0400248F RID: 9359
		private bool waitForChanges;

		// Token: 0x04002490 RID: 9360
		private bool isRawStream;

		// Token: 0x04002491 RID: 9361
		private long fileOffset;

		// Token: 0x04002492 RID: 9362
		private FileAttributes oldAttributes;

		// Token: 0x04002493 RID: 9363
		private bool haveOldAttributes;

		// Token: 0x04002494 RID: 9364
		private FileStreamBackReader _backReader;

		// Token: 0x04002495 RID: 9365
		private bool _alreadyDetectEncoding;

		// Token: 0x04002496 RID: 9366
		private bool suppressNewline;
	}
}
