using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x0200047A RID: 1146
	[ComVisible(true)]
	internal static class Buffer
	{
		// Token: 0x06003329 RID: 13097 RVA: 0x00117EB4 File Offset: 0x001160B4
		internal unsafe static int IndexOfByte(byte* src, byte value, int index, int count)
		{
			byte* ptr = src + index;
			while ((ptr & 3) != 0)
			{
				if (count == 0)
				{
					return -1;
				}
				if (*ptr == value)
				{
					return (int)((long)(ptr - src));
				}
				count--;
				ptr++;
			}
			uint num = (uint)(((int)value << 8) + (int)value);
			num = (num << 16) + num;
			while (count > 3)
			{
				uint num2 = *(uint*)ptr;
				num2 ^= num;
				uint num3 = 2130640639U + num2;
				num2 ^= uint.MaxValue;
				num2 ^= num3;
				num2 &= 2164326656U;
				if (num2 != 0U)
				{
					int num4 = (int)((long)(ptr - src));
					if (*ptr == value)
					{
						return num4;
					}
					if (ptr[1] == value)
					{
						return num4 + 1;
					}
					if (ptr[2] == value)
					{
						return num4 + 2;
					}
					if (ptr[3] == value)
					{
						return num4 + 3;
					}
				}
				count -= 4;
				ptr += 4;
			}
			while (count > 0)
			{
				if (*ptr == value)
				{
					return (int)((long)(ptr - src));
				}
				count--;
				ptr++;
			}
			return -1;
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x00117F7E File Offset: 0x0011617E
		internal unsafe static void ZeroMemory(byte* src, long len)
		{
			for (;;)
			{
				long num = len;
				len = num - 1L;
				if (num <= 0L)
				{
					break;
				}
				src[len] = 0;
			}
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x00117F94 File Offset: 0x00116194
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal unsafe static void memcpy(byte* src, int srcIndex, byte[] dest, int destIndex, int len)
		{
			if (len == 0)
			{
				return;
			}
			fixed (byte* ptr = dest)
			{
				Buffer.memcpyimpl(src + srcIndex, ptr + destIndex, len);
			}
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x00117FD0 File Offset: 0x001161D0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal unsafe static void memcpy(byte[] src, int srcIndex, byte* pDest, int destIndex, int len)
		{
			if (len == 0)
			{
				return;
			}
			fixed (byte* ptr = src)
			{
				Buffer.memcpyimpl(ptr + srcIndex, pDest + destIndex, len);
			}
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x0011800A File Offset: 0x0011620A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal unsafe static void memcpy(char* pSrc, int srcIndex, char* pDest, int destIndex, int len)
		{
			if (len == 0)
			{
				return;
			}
			Buffer.memcpyimpl((byte*)(pSrc + srcIndex), (byte*)(pDest + destIndex), len * 2);
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x00118028 File Offset: 0x00116228
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal unsafe static void memcpyimpl(byte* src, byte* dest, int len)
		{
			if (len >= 16)
			{
				do
				{
					*(int*)dest = *(int*)src;
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(int*)(dest + 12) = *(int*)(src + 12);
					dest += 16;
					src += 16;
				}
				while ((len -= 16) >= 16);
			}
			if (len > 0)
			{
				if ((len & 8) != 0)
				{
					*(int*)dest = *(int*)src;
					*(int*)(dest + 4) = *(int*)(src + 4);
					dest += 8;
					src += 8;
				}
				if ((len & 4) != 0)
				{
					*(int*)dest = *(int*)src;
					dest += 4;
					src += 4;
				}
				if ((len & 2) != 0)
				{
					*(short*)dest = *(short*)src;
					dest += 2;
					src += 2;
				}
				if ((len & 1) != 0)
				{
					*(dest++) = *(src++);
				}
			}
		}
	}
}
