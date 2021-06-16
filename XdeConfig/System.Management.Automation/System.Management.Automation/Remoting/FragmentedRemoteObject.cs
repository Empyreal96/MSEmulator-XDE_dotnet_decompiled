using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002B1 RID: 689
	internal class FragmentedRemoteObject
	{
		// Token: 0x0600214C RID: 8524 RVA: 0x000BFD4B File Offset: 0x000BDF4B
		internal FragmentedRemoteObject()
		{
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000BFD54 File Offset: 0x000BDF54
		internal FragmentedRemoteObject(byte[] blob, long objectId, long fragmentId, bool isEndFragment)
		{
			this._objectId = objectId;
			this._fragmentId = fragmentId;
			this._isStartFragment = (fragmentId == 0L);
			this._isEndFragment = isEndFragment;
			this._blob = blob;
			this._blobLength = this._blob.Length;
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x0600214E RID: 8526 RVA: 0x000BFDA1 File Offset: 0x000BDFA1
		// (set) Token: 0x0600214F RID: 8527 RVA: 0x000BFDA9 File Offset: 0x000BDFA9
		internal long ObjectId
		{
			get
			{
				return this._objectId;
			}
			set
			{
				this._objectId = value;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x000BFDB2 File Offset: 0x000BDFB2
		// (set) Token: 0x06002151 RID: 8529 RVA: 0x000BFDBA File Offset: 0x000BDFBA
		internal long FragmentId
		{
			get
			{
				return this._fragmentId;
			}
			set
			{
				this._fragmentId = value;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002152 RID: 8530 RVA: 0x000BFDC3 File Offset: 0x000BDFC3
		// (set) Token: 0x06002153 RID: 8531 RVA: 0x000BFDCB File Offset: 0x000BDFCB
		internal bool IsStartFragment
		{
			get
			{
				return this._isStartFragment;
			}
			set
			{
				this._isStartFragment = value;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002154 RID: 8532 RVA: 0x000BFDD4 File Offset: 0x000BDFD4
		// (set) Token: 0x06002155 RID: 8533 RVA: 0x000BFDDC File Offset: 0x000BDFDC
		internal bool IsEndFragment
		{
			get
			{
				return this._isEndFragment;
			}
			set
			{
				this._isEndFragment = value;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002156 RID: 8534 RVA: 0x000BFDE5 File Offset: 0x000BDFE5
		// (set) Token: 0x06002157 RID: 8535 RVA: 0x000BFDED File Offset: 0x000BDFED
		internal int BlobLength
		{
			get
			{
				return this._blobLength;
			}
			set
			{
				this._blobLength = value;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002158 RID: 8536 RVA: 0x000BFDF6 File Offset: 0x000BDFF6
		// (set) Token: 0x06002159 RID: 8537 RVA: 0x000BFDFE File Offset: 0x000BDFFE
		internal byte[] Blob
		{
			get
			{
				return this._blob;
			}
			set
			{
				this._blob = value;
			}
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000BFE08 File Offset: 0x000BE008
		internal byte[] GetBytes()
		{
			int num = 8;
			int num2 = 8;
			int num3 = 1;
			int num4 = 4;
			int num5 = num + num2 + num3 + num4 + this.BlobLength;
			byte[] array = new byte[num5];
			int num6 = 0;
			array[num6++] = (byte)(this.ObjectId >> 56 & 127L);
			array[num6++] = (byte)(this.ObjectId >> 48 & 255L);
			array[num6++] = (byte)(this.ObjectId >> 40 & 255L);
			array[num6++] = (byte)(this.ObjectId >> 32 & 255L);
			array[num6++] = (byte)(this.ObjectId >> 24 & 255L);
			array[num6++] = (byte)(this.ObjectId >> 16 & 255L);
			array[num6++] = (byte)(this.ObjectId >> 8 & 255L);
			array[num6++] = (byte)(this.ObjectId & 255L);
			num6 = 8;
			array[num6++] = (byte)(this.FragmentId >> 56 & 127L);
			array[num6++] = (byte)(this.FragmentId >> 48 & 255L);
			array[num6++] = (byte)(this.FragmentId >> 40 & 255L);
			array[num6++] = (byte)(this.FragmentId >> 32 & 255L);
			array[num6++] = (byte)(this.FragmentId >> 24 & 255L);
			array[num6++] = (byte)(this.FragmentId >> 16 & 255L);
			array[num6++] = (byte)(this.FragmentId >> 8 & 255L);
			array[num6++] = (byte)(this.FragmentId & 255L);
			num6 = 16;
			byte b = this.IsStartFragment ? 1 : 0;
			byte b2 = this.IsEndFragment ? 2 : 0;
			array[num6++] = (b | b2);
			num6 = 17;
			array[num6++] = (byte)(this.BlobLength >> 24 & 255);
			array[num6++] = (byte)(this.BlobLength >> 16 & 255);
			array[num6++] = (byte)(this.BlobLength >> 8 & 255);
			array[num6++] = (byte)(this.BlobLength & 255);
			Array.Copy(this._blob, 0, array, 21, this.BlobLength);
			return array;
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000C0094 File Offset: 0x000BE294
		internal static long GetObjectId(byte[] fragmentBytes, int startIndex)
		{
			int num = startIndex + 1;
			long num2 = (long)((ulong)fragmentBytes[startIndex] << 56 & 9151314442816847872UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 48 & 71776119061217280UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 40 & 280375465082880UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 32 & 1095216660480UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 24 & (ulong)-16777216);
			num2 += (long)((ulong)fragmentBytes[num++] << 16 & 16711680UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 8 & 65280UL);
			return num2 + (long)((ulong)fragmentBytes[num++] & 255UL);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000C0158 File Offset: 0x000BE358
		internal static long GetFragmentId(byte[] fragmentBytes, int startIndex)
		{
			int num = startIndex + 8;
			long num2 = (long)((ulong)fragmentBytes[num++] << 56 & 9151314442816847872UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 48 & 71776119061217280UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 40 & 280375465082880UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 32 & 1095216660480UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 24 & (ulong)-16777216);
			num2 += (long)((ulong)fragmentBytes[num++] << 16 & 16711680UL);
			num2 += (long)((ulong)fragmentBytes[num++] << 8 & 65280UL);
			return num2 + (long)((ulong)fragmentBytes[num++] & 255UL);
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000C021B File Offset: 0x000BE41B
		internal static bool GetIsStartFragment(byte[] fragmentBytes, int startIndex)
		{
			return (fragmentBytes[startIndex + 16] & 1) != 0;
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000C022A File Offset: 0x000BE42A
		internal static bool GetIsEndFragment(byte[] fragmentBytes, int startIndex)
		{
			return (fragmentBytes[startIndex + 16] & 2) != 0;
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000C023C File Offset: 0x000BE43C
		internal static int GetBlobLength(byte[] fragmentBytes, int startIndex)
		{
			int num = 0;
			int num2 = startIndex + 17;
			num += ((int)fragmentBytes[num2++] << 24 & 2130706432);
			num += ((int)fragmentBytes[num2++] << 16 & 16711680);
			num += ((int)fragmentBytes[num2++] << 8 & 65280);
			return num + (int)(fragmentBytes[num2++] & byte.MaxValue);
		}

		// Token: 0x04000EBC RID: 3772
		internal const byte SFlag = 1;

		// Token: 0x04000EBD RID: 3773
		internal const byte EFlag = 2;

		// Token: 0x04000EBE RID: 3774
		internal const int HeaderLength = 21;

		// Token: 0x04000EBF RID: 3775
		private const int _objectIdOffset = 0;

		// Token: 0x04000EC0 RID: 3776
		private const int _fragmentIdOffset = 8;

		// Token: 0x04000EC1 RID: 3777
		private const int _flagsOffset = 16;

		// Token: 0x04000EC2 RID: 3778
		private const int _blobLengthOffset = 17;

		// Token: 0x04000EC3 RID: 3779
		private const int _blobOffset = 21;

		// Token: 0x04000EC4 RID: 3780
		private long _objectId;

		// Token: 0x04000EC5 RID: 3781
		private long _fragmentId;

		// Token: 0x04000EC6 RID: 3782
		private bool _isStartFragment;

		// Token: 0x04000EC7 RID: 3783
		private bool _isEndFragment;

		// Token: 0x04000EC8 RID: 3784
		private byte[] _blob;

		// Token: 0x04000EC9 RID: 3785
		private int _blobLength;
	}
}
