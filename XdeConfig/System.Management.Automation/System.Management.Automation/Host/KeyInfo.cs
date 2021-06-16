using System;
using System.Globalization;

namespace System.Management.Automation.Host
{
	// Token: 0x02000222 RID: 546
	public struct KeyInfo
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x0009A912 File Offset: 0x00098B12
		// (set) Token: 0x060019BC RID: 6588 RVA: 0x0009A91A File Offset: 0x00098B1A
		public int VirtualKeyCode
		{
			get
			{
				return this.virtualKeyCode;
			}
			set
			{
				this.virtualKeyCode = value;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060019BD RID: 6589 RVA: 0x0009A923 File Offset: 0x00098B23
		// (set) Token: 0x060019BE RID: 6590 RVA: 0x0009A92B File Offset: 0x00098B2B
		public char Character
		{
			get
			{
				return this.character;
			}
			set
			{
				this.character = value;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060019BF RID: 6591 RVA: 0x0009A934 File Offset: 0x00098B34
		// (set) Token: 0x060019C0 RID: 6592 RVA: 0x0009A93C File Offset: 0x00098B3C
		public ControlKeyStates ControlKeyState
		{
			get
			{
				return this.controlKeyState;
			}
			set
			{
				this.controlKeyState = value;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x0009A945 File Offset: 0x00098B45
		// (set) Token: 0x060019C2 RID: 6594 RVA: 0x0009A94D File Offset: 0x00098B4D
		public bool KeyDown
		{
			get
			{
				return this.keyDown;
			}
			set
			{
				this.keyDown = value;
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0009A956 File Offset: 0x00098B56
		public KeyInfo(int virtualKeyCode, char ch, ControlKeyStates controlKeyState, bool keyDown)
		{
			this.virtualKeyCode = virtualKeyCode;
			this.character = ch;
			this.controlKeyState = controlKeyState;
			this.keyDown = keyDown;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0009A978 File Offset: 0x00098B78
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", new object[]
			{
				this.VirtualKeyCode,
				this.Character,
				this.ControlKeyState,
				this.KeyDown
			});
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0009A9D4 File Offset: 0x00098BD4
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is KeyInfo)
			{
				result = (this == (KeyInfo)obj);
			}
			return result;
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0009AA00 File Offset: 0x00098C00
		public override int GetHashCode()
		{
			uint num = this.KeyDown ? 268435456U : 0U;
			num |= (uint)((uint)this.ControlKeyState << 16);
			return (num | (uint)this.VirtualKeyCode).GetHashCode();
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x0009AA3C File Offset: 0x00098C3C
		public static bool operator ==(KeyInfo first, KeyInfo second)
		{
			return first.Character == second.Character && first.ControlKeyState == second.ControlKeyState && first.KeyDown == second.KeyDown && first.VirtualKeyCode == second.VirtualKeyCode;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x0009AA8E File Offset: 0x00098C8E
		public static bool operator !=(KeyInfo first, KeyInfo second)
		{
			return !(first == second);
		}

		// Token: 0x04000A97 RID: 2711
		private int virtualKeyCode;

		// Token: 0x04000A98 RID: 2712
		private char character;

		// Token: 0x04000A99 RID: 2713
		private ControlKeyStates controlKeyState;

		// Token: 0x04000A9A RID: 2714
		private bool keyDown;
	}
}
