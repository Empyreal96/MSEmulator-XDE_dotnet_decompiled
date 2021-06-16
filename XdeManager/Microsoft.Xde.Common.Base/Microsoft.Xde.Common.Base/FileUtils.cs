using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000005 RID: 5
	public static class FileUtils
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000024D8 File Offset: 0x000006D8
		public static bool AreFilesSame(string file1, string file2)
		{
			if (!File.Exists(file1) || !File.Exists(file2))
			{
				return false;
			}
			FileInfo fileInfo = new FileInfo(file1);
			FileInfo fileInfo2 = new FileInfo(file2);
			return fileInfo.Length == fileInfo2.Length && fileInfo.LastWriteTimeUtc == fileInfo2.LastWriteTimeUtc;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002528 File Offset: 0x00000728
		public static int CopyFileWithConsoleOutput(string source, string destDir)
		{
			string directoryName = Path.GetDirectoryName(source);
			string fileName = Path.GetFileName(source);
			int result;
			using (Process process = new Process())
			{
				int x = 0;
				int y = -1;
				Regex percentRegex = new Regex("^\\s*(\\d+(\\.\\d+)?)\\%");
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.FileName = "robocopy";
				process.StartInfo.Arguments = string.Concat(new string[]
				{
					"\"",
					directoryName,
					"\" \"",
					destDir,
					"\" ",
					fileName
				});
				process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
				{
					if (e.Data != null)
					{
						Match match = percentRegex.Match(e.Data);
						if (match.Success)
						{
							if (y == -1)
							{
								try
								{
									y = Console.CursorTop;
								}
								catch (IOException)
								{
								}
							}
							float percent = float.Parse(match.Groups[1].Value);
							FileUtils.DrawConsoleProgressBar(x, y, percent);
							return;
						}
						Console.WriteLine(e.Data);
					}
				};
				process.Start();
				ChildProcessTracker.AddProcess(process);
				process.BeginOutputReadLine();
				try
				{
					Console.CursorVisible = false;
				}
				catch (IOException)
				{
				}
				string text = process.StandardError.ReadToEnd();
				process.WaitForExit();
				if (y != -1)
				{
					FileUtils.ClearProgressBarArea(x, y);
				}
				try
				{
					Console.CursorVisible = true;
				}
				catch (IOException)
				{
				}
				if (!string.IsNullOrEmpty(text))
				{
					Console.WriteLine("Copy failed:\r\n" + text);
				}
				result = ((process.ExitCode == 0 || process.ExitCode == 1) ? 0 : 1);
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000026D0 File Offset: 0x000008D0
		public static void GrantHyperVRightsForFile(string fileName)
		{
			FileSecurity accessControl = File.GetAccessControl(fileName);
			accessControl.AddAccessRule(FileUtils.GetFileSystemAccessRule(false));
			File.SetAccessControl(fileName, accessControl);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000026F8 File Offset: 0x000008F8
		public static void GrantHyperVRightsForDirectory(string path, bool inherit)
		{
			DirectorySecurity accessControl = Directory.GetAccessControl(path);
			accessControl.AddAccessRule(FileUtils.GetFileSystemAccessRule(inherit));
			Directory.SetAccessControl(path, accessControl);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000271F File Offset: 0x0000091F
		public static void GrantHyperVRightsForDirectory(string path)
		{
			FileUtils.GrantHyperVRightsForDirectory(path, false);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002728 File Offset: 0x00000928
		private static FileSystemAccessRule GetFileSystemAccessRule(bool inherit)
		{
			return new FileSystemAccessRule(new SecurityIdentifier("S-1-5-83-0"), FileSystemRights.FullControl, inherit ? (InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit) : InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow);
		}

		// Token: 0x06000020 RID: 32
		[DllImport("cabinet.dll", SetLastError = true)]
		private static extern bool CreateDecompressor(uint algorithm, IntPtr allocationRoutines, out IntPtr decompressorHandle);

		// Token: 0x06000021 RID: 33
		[DllImport("cabinet.dll", SetLastError = true)]
		private static extern bool CreateCompressor(uint algorithm, IntPtr allocationRoutines, out IntPtr compressorHandle);

		// Token: 0x06000022 RID: 34
		[DllImport("cabinet.dll", SetLastError = true)]
		private static extern bool Decompress(IntPtr decompressorHandle, byte[] compressedData, ulong compressedDataSize, byte[] uncompressedBuffer, ulong uncompressedBufferSize, out ulong uncompressedDataSize);

		// Token: 0x06000023 RID: 35
		[DllImport("cabinet.dll", SetLastError = true)]
		private static extern bool Compress(IntPtr compressorHandle, byte[] uncompressedData, ulong uncompressedDataSize, byte[] compressedBuffer, ulong compressedBufferSize, out ulong compressedDataSize);

		// Token: 0x06000024 RID: 36
		[DllImport("cabinet.dll", SetLastError = true)]
		private static extern bool Decompress(IntPtr decompressorHandle, byte[] compressedData, ulong compressedDataSize, byte[] uncompressedBuffer, ulong uncompressedBufferSize, IntPtr uncompressedDataSize);

		// Token: 0x06000025 RID: 37
		[DllImport("cabinet.dll", SetLastError = true)]
		private static extern bool CloseDecompressor(IntPtr decompressorHandle);

		// Token: 0x06000026 RID: 38
		[DllImport("cabinet.dll", SetLastError = true)]
		private static extern bool CloseCompressor(IntPtr compressorHandle);

		// Token: 0x06000027 RID: 39 RVA: 0x00002747 File Offset: 0x00000947
		private static uint GetChunkCountForDecompressedSize(uint uncompressedSize)
		{
			return uncompressedSize / 8192U + ((uncompressedSize % 8192U != 0U) ? 1U : 0U);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002760 File Offset: 0x00000960
		public static void WofCompress(Stream input, Stream output)
		{
			byte[] array = new byte[8192];
			byte[] array2 = new byte[8192];
			uint uncompressedSize = (uint)input.Length;
			uint num = 8192U;
			uint chunkCountForDecompressedSize = FileUtils.GetChunkCountForDecompressedSize(uncompressedSize);
			IntPtr compressorHandle;
			if (!FileUtils.CreateCompressor(536870916U, IntPtr.Zero, out compressorHandle))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			try
			{
				uint num2 = 4U * (chunkCountForDecompressedSize - 1U);
				uint num3 = 0U;
				for (uint num4 = 0U; num4 < chunkCountForDecompressedSize; num4 += 1U)
				{
					if (num4 == chunkCountForDecompressedSize - 1U)
					{
						num = (uint)input.Length - 8192U * (chunkCountForDecompressedSize - 1U);
					}
					input.Read(array, 0, (int)num);
					ulong num5;
					if (FileUtils.Compress(compressorHandle, array, (ulong)num, array2, (ulong)((long)array2.Length), out num5))
					{
						if (num4 != 0U)
						{
							byte[] bytes = BitConverter.GetBytes(num3);
							output.Seek((long)((ulong)((num4 - 1U) * 4U)), SeekOrigin.Begin);
							output.Write(bytes, 0, 4);
						}
						output.Seek((long)((ulong)(num2 + num3)), SeekOrigin.Begin);
						output.Write(array2, 0, (int)num5);
						num3 += (uint)num5;
					}
				}
			}
			finally
			{
				FileUtils.CloseCompressor(compressorHandle);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002870 File Offset: 0x00000A70
		public static void WofDecompress(Stream input, Stream output, uint uncompressedSize)
		{
			IntPtr zero = IntPtr.Zero;
			if (!FileUtils.CreateDecompressor(536870916U, IntPtr.Zero, out zero))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			try
			{
				uint chunkCountForDecompressedSize = FileUtils.GetChunkCountForDecompressedSize(uncompressedSize);
				byte[] array = new byte[4U * (chunkCountForDecompressedSize - 1U)];
				byte[] array2 = new byte[8192];
				byte[] array3 = new byte[8192];
				input.Read(array, 0, array.Length);
				int num = (int)input.Length - array.Length;
				uint num2 = 0U;
				int num3 = 0;
				while ((long)num3 < (long)((ulong)chunkCountForDecompressedSize))
				{
					uint num4 = ((long)num3 < (long)((ulong)(chunkCountForDecompressedSize - 1U))) ? BitConverter.ToUInt32(array, 4 * num3) : ((uint)num);
					uint num5 = num4 - num2;
					input.Read(array3, 0, (int)num5);
					num2 = num4;
					if (num5 == 8192U)
					{
						output.Write(array3, 0, (int)num5);
					}
					else
					{
						ulong num6 = (ulong)((long)array2.Length);
						if ((long)num3 == (long)((ulong)(chunkCountForDecompressedSize - 1U)))
						{
							num6 = (ulong)(uncompressedSize - (chunkCountForDecompressedSize - 1U) * 8192U);
						}
						if (!FileUtils.Decompress(zero, array3, (ulong)num5, array2, num6, IntPtr.Zero))
						{
							throw new Win32Exception(Marshal.GetLastWin32Error());
						}
						output.Write(array2, 0, (int)num6);
					}
					num3++;
				}
			}
			finally
			{
				FileUtils.CloseDecompressor(zero);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000029A4 File Offset: 0x00000BA4
		private static string GetProgressBarText(float percent)
		{
			int num = (int)Math.Round((double)(50f * (percent / 100f)), 1);
			string text = string.Format("{0,5:###.0}", percent);
			return string.Concat(new string[]
			{
				"[",
				new string('*', num),
				new string(' ', 50 - num),
				"] ",
				text,
				"%"
			});
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A18 File Offset: 0x00000C18
		private static void ClearProgressBarArea(int x, int y)
		{
			string text = new string(' ', FileUtils.GetProgressBarText(100f).Length);
			FileUtils.WriteTextToProgressBar(x, y, text);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A44 File Offset: 0x00000C44
		private static void WriteTextToProgressBar(int x, int y, string text)
		{
			try
			{
				int cursorLeft = Console.CursorLeft;
				int cursorTop = Console.CursorTop;
				Console.SetCursorPosition(x, y);
				Console.Write(text);
				Console.SetCursorPosition(cursorLeft, cursorTop);
			}
			catch (IOException)
			{
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002A84 File Offset: 0x00000C84
		private static void DrawConsoleProgressBar(int x, int y, float percent)
		{
			string progressBarText = FileUtils.GetProgressBarText(percent);
			FileUtils.WriteTextToProgressBar(x, y, progressBarText);
		}

		// Token: 0x04000011 RID: 17
		private const int ChunkSize = 8192;

		// Token: 0x04000012 RID: 18
		private const uint COMPRESS_RAW = 536870912U;

		// Token: 0x04000013 RID: 19
		private const uint COMPRESS_ALGORITHM_XPRESS_HUFF = 4U;
	}
}
