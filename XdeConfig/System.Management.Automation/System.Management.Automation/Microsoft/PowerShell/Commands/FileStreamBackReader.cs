using System;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000766 RID: 1894
	internal sealed class FileStreamBackReader : StreamReader
	{
		// Token: 0x06004BBD RID: 19389 RVA: 0x0018CF9C File Offset: 0x0018B19C
		internal FileStreamBackReader(FileStream fileStream, Encoding encoding) : base(fileStream, encoding)
		{
			this._stream = fileStream;
			if (this._stream.Length > 0L)
			{
				long position = this._stream.Position;
				this._stream.Seek(0L, SeekOrigin.Begin);
				base.Peek();
				this._stream.Position = position;
				this._currentEncoding = base.CurrentEncoding;
				this._currentPosition = this._stream.Position;
				this._oemEncoding = EncodingConversion.Convert(null, "oem");
				this._defaultAnsiEncoding = EncodingConversion.Convert(null, "default");
			}
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x0018D060 File Offset: 0x0018B260
		private bool IsSingleByteCharacterSet()
		{
			if (this._singleByteCharSet != null)
			{
				return this._singleByteCharSet.Value;
			}
			FileStreamBackReader.NativeMethods.CPINFO cpinfo;
			if ((this._currentEncoding.Equals(this._oemEncoding) || this._currentEncoding.Equals(this._defaultAnsiEncoding)) && FileStreamBackReader.NativeMethods.GetCPInfo((uint)this._currentEncoding.CodePage, out cpinfo) && cpinfo.MaxCharSize == 1)
			{
				this._singleByteCharSet = new bool?(true);
				return true;
			}
			this._singleByteCharSet = new bool?(false);
			return false;
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x0018D0E5 File Offset: 0x0018B2E5
		public override int ReadBlock(char[] buffer, int index, int count)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x0018D0EC File Offset: 0x0018B2EC
		public override string ReadToEnd()
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x0018D0F3 File Offset: 0x0018B2F3
		internal new void DiscardBufferedData()
		{
			base.DiscardBufferedData();
			this._currentPosition = this._stream.Position;
			this._charCount = 0;
			this._byteCount = 0;
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x0018D11C File Offset: 0x0018B31C
		internal long GetCurrentPosition()
		{
			if (this._charCount == 0)
			{
				return this._currentPosition;
			}
			int byteCount = this._currentEncoding.GetByteCount(this._charBuff, 0, this._charCount);
			return this._currentPosition + (long)byteCount;
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x0018D15C File Offset: 0x0018B35C
		internal int GetByteCount(string delimiter)
		{
			char[] array = delimiter.ToCharArray();
			return this._currentEncoding.GetByteCount(array, 0, array.Length);
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x0018D180 File Offset: 0x0018B380
		public override int Peek()
		{
			if (this._charCount == 0 && this.RefillCharBuffer() == -1)
			{
				return -1;
			}
			return (int)this._charBuff[this._charCount - 1];
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x0018D1A4 File Offset: 0x0018B3A4
		public override int Read()
		{
			if (this._charCount == 0 && this.RefillCharBuffer() == -1)
			{
				return -1;
			}
			this._charCount--;
			return (int)this._charBuff[this._charCount];
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x0018D1D4 File Offset: 0x0018B3D4
		public override int Read(char[] buffer, int index, int count)
		{
			int num = 0;
			while (this._charCount != 0 || this.RefillCharBuffer() != -1)
			{
				int i = (this._charCount > count) ? count : this._charCount;
				while (i > 0)
				{
					buffer[index++] = this._charBuff[--this._charCount];
					i--;
					count--;
					num++;
				}
				if (count <= 0)
				{
					return num;
				}
			}
			return num;
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x0018D244 File Offset: 0x0018B444
		public override string ReadLine()
		{
			if (this._charCount == 0 && this.RefillCharBuffer() == -1)
			{
				return null;
			}
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			if (this._charBuff[this._charCount - 1] == '\r' || this._charBuff[this._charCount - 1] == '\n')
			{
				num++;
				stringBuilder.Insert(0, this._charBuff[--this._charCount]);
				if (this._charBuff[this._charCount] == '\n')
				{
					if (this._charCount == 0 && this.RefillCharBuffer() == -1)
					{
						return string.Empty;
					}
					if (this._charCount > 0 && this._charBuff[this._charCount - 1] == '\r')
					{
						num++;
						stringBuilder.Insert(0, this._charBuff[--this._charCount]);
					}
				}
			}
			for (;;)
			{
				if (this._charCount <= 0)
				{
					if (this.RefillCharBuffer() == -1)
					{
						goto Block_11;
					}
				}
				else
				{
					if (this._charBuff[this._charCount - 1] == '\r' || this._charBuff[this._charCount - 1] == '\n')
					{
						break;
					}
					stringBuilder.Insert(0, this._charBuff[--this._charCount]);
				}
			}
			stringBuilder.Remove(stringBuilder.Length - num, num);
			return stringBuilder.ToString();
			Block_11:
			stringBuilder.Remove(stringBuilder.Length - num, num);
			return stringBuilder.ToString();
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x0018D3B3 File Offset: 0x0018B5B3
		private int RefillCharBuffer()
		{
			if (this.RefillByteBuff() == -1)
			{
				return -1;
			}
			this._charCount = this._currentEncoding.GetChars(this._byteBuff, 0, this._byteCount, this._charBuff, 0);
			return this._charCount;
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x0018D3EC File Offset: 0x0018B5EC
		private int RefillByteBuff()
		{
			long position = this._stream.Position;
			if (position == 0L)
			{
				return -1;
			}
			int num = (position > 4096L) ? 4096 : ((int)position);
			this._stream.Seek((long)(-(long)num), SeekOrigin.Current);
			if (this._currentEncoding.Equals(Encoding.UTF8))
			{
				byte b;
				for (;;)
				{
					this._currentPosition = this._stream.Position;
					b = (byte)this._stream.ReadByte();
					if ((b & 192) == 192 || (b & 128) == 0)
					{
						break;
					}
					if (position <= this._stream.Position)
					{
						goto IL_A0;
					}
				}
				this._byteBuff[0] = b;
				this._byteCount = 1;
				IL_A0:
				if (position == this._stream.Position)
				{
					this._stream.Seek((long)(-(long)num), SeekOrigin.Current);
					this._byteCount = 0;
				}
				this._byteCount += this._stream.Read(this._byteBuff, this._byteCount, (int)(position - this._stream.Position));
				this._stream.Position = this._currentPosition;
			}
			else
			{
				if (!this._currentEncoding.Equals(Encoding.Unicode) && !this._currentEncoding.Equals(Encoding.BigEndianUnicode) && !this._currentEncoding.Equals(Encoding.UTF32) && !this._currentEncoding.Equals(Encoding.ASCII) && !this.IsSingleByteCharacterSet())
				{
					string message = StringUtil.Format(FileSystemProviderStrings.ReadBackward_Encoding_NotSupport, this._currentEncoding.EncodingName);
					throw new BackReaderEncodingNotSupportedException(message, this._currentEncoding.EncodingName);
				}
				this._currentPosition = this._stream.Position;
				this._byteCount = this._stream.Read(this._byteBuff, 0, num);
				this._stream.Position = this._currentPosition;
			}
			return this._byteCount;
		}

		// Token: 0x04002497 RID: 9367
		private const int BuffSize = 4096;

		// Token: 0x04002498 RID: 9368
		private const byte BothTopBitsSet = 192;

		// Token: 0x04002499 RID: 9369
		private const byte TopBitUnset = 128;

		// Token: 0x0400249A RID: 9370
		private readonly FileStream _stream;

		// Token: 0x0400249B RID: 9371
		private readonly Encoding _currentEncoding;

		// Token: 0x0400249C RID: 9372
		private readonly Encoding _oemEncoding;

		// Token: 0x0400249D RID: 9373
		private readonly Encoding _defaultAnsiEncoding;

		// Token: 0x0400249E RID: 9374
		private readonly byte[] _byteBuff = new byte[4096];

		// Token: 0x0400249F RID: 9375
		private readonly char[] _charBuff = new char[4096];

		// Token: 0x040024A0 RID: 9376
		private int _byteCount;

		// Token: 0x040024A1 RID: 9377
		private int _charCount;

		// Token: 0x040024A2 RID: 9378
		private long _currentPosition;

		// Token: 0x040024A3 RID: 9379
		private bool? _singleByteCharSet = null;

		// Token: 0x02000767 RID: 1895
		private static class NativeMethods
		{
			// Token: 0x06004BCA RID: 19402
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern bool GetCPInfo(uint codePage, out FileStreamBackReader.NativeMethods.CPINFO lpCpInfo);

			// Token: 0x040024A4 RID: 9380
			private const int MAX_DEFAULTCHAR = 2;

			// Token: 0x040024A5 RID: 9381
			private const int MAX_LEADBYTES = 12;

			// Token: 0x02000768 RID: 1896
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct CPINFO
			{
				// Token: 0x040024A6 RID: 9382
				[MarshalAs(UnmanagedType.U4)]
				internal int MaxCharSize;

				// Token: 0x040024A7 RID: 9383
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
				public byte[] DefaultChar;

				// Token: 0x040024A8 RID: 9384
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
				public byte[] LeadBytes;
			}
		}
	}
}
