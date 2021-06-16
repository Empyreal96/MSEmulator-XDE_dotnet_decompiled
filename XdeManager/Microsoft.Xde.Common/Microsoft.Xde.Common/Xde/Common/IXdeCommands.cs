using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000027 RID: 39
	public interface IXdeCommands
	{
		// Token: 0x060000FD RID: 253
		void Close();

		// Token: 0x060000FE RID: 254
		void Minimize();

		// Token: 0x060000FF RID: 255
		void RotateClockwise();

		// Token: 0x06000100 RID: 256
		void RotateCounterClockwise();

		// Token: 0x06000101 RID: 257
		void LaunchXdeTools();

		// Token: 0x06000102 RID: 258
		void FitToScreen();

		// Token: 0x06000103 RID: 259
		void ShowZoomUi();

		// Token: 0x06000104 RID: 260
		void DisplayHelp();
	}
}
