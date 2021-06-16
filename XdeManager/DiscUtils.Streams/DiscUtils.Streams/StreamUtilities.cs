using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000037 RID: 55
	public static class StreamUtilities
	{
		// Token: 0x060001FA RID: 506 RVA: 0x0000704C File Offset: 0x0000524C
		public static void AssertBufferParameters(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", offset, "Offset is negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "Count is negative");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentException("buffer is too small", "buffer");
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000070B4 File Offset: 0x000052B4
		public static void ReadExact(Stream stream, byte[] buffer, int offset, int count)
		{
			int num = count;
			while (count > 0)
			{
				int num2 = stream.Read(buffer, offset, count);
				if (num2 == 0)
				{
					throw new EndOfStreamException("Unable to complete read of " + num + " bytes");
				}
				offset += num2;
				count -= num2;
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000070FC File Offset: 0x000052FC
		public static byte[] ReadExact(Stream stream, int count)
		{
			byte[] array = new byte[count];
			StreamUtilities.ReadExact(stream, array, 0, count);
			return array;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000711C File Offset: 0x0000531C
		public static void ReadExact(IBuffer buffer, long pos, byte[] data, int offset, int count)
		{
			int num = count;
			while (count > 0)
			{
				int num2 = buffer.Read(pos, data, offset, count);
				if (num2 == 0)
				{
					throw new EndOfStreamException("Unable to complete read of " + num + " bytes");
				}
				pos += (long)num2;
				offset += num2;
				count -= num2;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00007170 File Offset: 0x00005370
		public static byte[] ReadExact(IBuffer buffer, long pos, int count)
		{
			byte[] array = new byte[count];
			StreamUtilities.ReadExact(buffer, pos, array, 0, count);
			return array;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00007190 File Offset: 0x00005390
		public static int ReadMaximum(Stream stream, byte[] buffer, int offset, int count)
		{
			int num = 0;
			while (count > 0)
			{
				int num2 = stream.Read(buffer, offset, count);
				if (num2 == 0)
				{
					return num;
				}
				offset += num2;
				count -= num2;
				num += num2;
			}
			return num;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000071C4 File Offset: 0x000053C4
		public static int ReadMaximum(IBuffer buffer, long pos, byte[] data, int offset, int count)
		{
			int num = 0;
			while (count > 0)
			{
				int num2 = buffer.Read(pos, data, offset, count);
				if (num2 == 0)
				{
					return num;
				}
				pos += (long)num2;
				offset += num2;
				count -= num2;
				num += num2;
			}
			return num;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00007201 File Offset: 0x00005401
		public static byte[] ReadAll(IBuffer buffer)
		{
			return StreamUtilities.ReadExact(buffer, 0L, (int)buffer.Capacity);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00007212 File Offset: 0x00005412
		public static byte[] ReadSector(Stream stream)
		{
			return StreamUtilities.ReadExact(stream, 512);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00007220 File Offset: 0x00005420
		public static T ReadStruct<T>(Stream stream) where T : IByteArraySerializable, new()
		{
			T result = Activator.CreateInstance<T>();
			byte[] buffer = StreamUtilities.ReadExact(stream, result.Size);
			result.ReadFrom(buffer, 0);
			return result;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007258 File Offset: 0x00005458
		public static T ReadStruct<T>(Stream stream, int length) where T : IByteArraySerializable, new()
		{
			T result = Activator.CreateInstance<T>();
			byte[] buffer = StreamUtilities.ReadExact(stream, length);
			result.ReadFrom(buffer, 0);
			return result;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00007284 File Offset: 0x00005484
		public static void WriteStruct<T>(Stream stream, T obj) where T : IByteArraySerializable
		{
			byte[] array = new byte[obj.Size];
			obj.WriteTo(array, 0);
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000072C0 File Offset: 0x000054C0
		public static void PumpStreams(Stream source, Stream dest)
		{
			byte[] array = new byte[8192];
			int count;
			while ((count = source.Read(array, 0, array.Length)) > 0)
			{
				dest.Write(array, 0, count);
			}
		}
	}
}
