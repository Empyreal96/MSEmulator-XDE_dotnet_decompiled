using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000036 RID: 54
	public interface IXdeView : IWin32Window
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000132 RID: 306
		// (remove) Token: 0x06000133 RID: 307
		event EventHandler Shown;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000134 RID: 308
		// (remove) Token: 0x06000135 RID: 309
		event CancelEventHandler Closing;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000136 RID: 310
		// (remove) Token: 0x06000137 RID: 311
		event EventHandler Load;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000138 RID: 312
		// (remove) Token: 0x06000139 RID: 313
		event EventHandler<ResolutionChangedEventArgs> PhysicalResolutionChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600013A RID: 314
		// (remove) Token: 0x0600013B RID: 315
		event EventHandler RdpDisconnected;

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600013C RID: 316
		IXdeToolbar Toolbar { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600013D RID: 317
		// (set) Token: 0x0600013E RID: 318
		Point Location { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600013F RID: 319
		// (set) Token: 0x06000140 RID: 320
		Point ScreenLocation { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000141 RID: 321
		double Dpi { get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000142 RID: 322
		Rectangle DesktopBounds { get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000143 RID: 323
		Rectangle DisplayDesktopBounds { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000144 RID: 324
		Size PhysicalResolution { get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000145 RID: 325
		Rectangle CurrentVirtualMachineDisplayBounds { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000146 RID: 326
		IWin32Window TopWindow { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000147 RID: 327
		// (set) Token: 0x06000148 RID: 328
		string Text { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000149 RID: 329
		// (set) Token: 0x0600014A RID: 330
		bool ShowDisplayName { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600014B RID: 331
		// (set) Token: 0x0600014C RID: 332
		Icon Icon { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600014D RID: 333
		string ScreenText { get; }

		// Token: 0x0600014E RID: 334
		void Run();

		// Token: 0x0600014F RID: 335
		void Close();

		// Token: 0x06000150 RID: 336
		void AsynchronousClose();

		// Token: 0x06000151 RID: 337
		void Minimize();

		// Token: 0x06000152 RID: 338
		void FitToScreen();

		// Token: 0x06000153 RID: 339
		void CenterToScreen();

		// Token: 0x06000154 RID: 340
		void BringAppToFront();

		// Token: 0x06000155 RID: 341
		void ConnectToVirtualMachineGuid(string virtualMachineName, string guid);

		// Token: 0x06000156 RID: 342
		void IndicateGuestVideoDisconnected();

		// Token: 0x06000157 RID: 343
		void IndicateShutdown();

		// Token: 0x06000158 RID: 344
		void IndicateFullBoot();

		// Token: 0x06000159 RID: 345
		void IndicateRestoring();

		// Token: 0x0600015A RID: 346
		object Invoke(Delegate method);

		// Token: 0x0600015B RID: 347
		void BeginInvoke(Delegate method);
	}
}
