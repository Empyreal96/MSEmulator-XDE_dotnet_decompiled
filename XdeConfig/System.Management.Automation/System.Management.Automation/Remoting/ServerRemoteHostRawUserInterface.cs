using System;
using System.Management.Automation.Host;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000309 RID: 777
	internal class ServerRemoteHostRawUserInterface : PSHostRawUserInterface
	{
		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060024C1 RID: 9409 RVA: 0x000CDE67 File Offset: 0x000CC067
		private HostDefaultData HostDefaultData
		{
			get
			{
				return this._remoteHostUserInterface.ServerRemoteHost.HostInfo.HostDefaultData;
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000CDE7E File Offset: 0x000CC07E
		internal ServerRemoteHostRawUserInterface(ServerRemoteHostUserInterface remoteHostUserInterface)
		{
			this._remoteHostUserInterface = remoteHostUserInterface;
			this._serverMethodExecutor = remoteHostUserInterface.ServerRemoteHost.ServerMethodExecutor;
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060024C3 RID: 9411 RVA: 0x000CDE9E File Offset: 0x000CC09E
		// (set) Token: 0x060024C4 RID: 9412 RVA: 0x000CDEC8 File Offset: 0x000CC0C8
		public override ConsoleColor ForegroundColor
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.ForegroundColor))
				{
					return (ConsoleColor)this.HostDefaultData.GetValue(HostDefaultDataId.ForegroundColor);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetForegroundColor);
			}
			set
			{
				this.HostDefaultData.SetValue(HostDefaultDataId.ForegroundColor, value);
				this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetForegroundColor, new object[]
				{
					value
				});
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060024C5 RID: 9413 RVA: 0x000CDF05 File Offset: 0x000CC105
		// (set) Token: 0x060024C6 RID: 9414 RVA: 0x000CDF30 File Offset: 0x000CC130
		public override ConsoleColor BackgroundColor
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.BackgroundColor))
				{
					return (ConsoleColor)this.HostDefaultData.GetValue(HostDefaultDataId.BackgroundColor);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetBackgroundColor);
			}
			set
			{
				this.HostDefaultData.SetValue(HostDefaultDataId.BackgroundColor, value);
				this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetBackgroundColor, new object[]
				{
					value
				});
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060024C7 RID: 9415 RVA: 0x000CDF6D File Offset: 0x000CC16D
		// (set) Token: 0x060024C8 RID: 9416 RVA: 0x000CDF98 File Offset: 0x000CC198
		public override Coordinates CursorPosition
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.CursorPosition))
				{
					return (Coordinates)this.HostDefaultData.GetValue(HostDefaultDataId.CursorPosition);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetCursorPosition);
			}
			set
			{
				this.HostDefaultData.SetValue(HostDefaultDataId.CursorPosition, value);
				this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetCursorPosition, new object[]
				{
					value
				});
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060024C9 RID: 9417 RVA: 0x000CDFD5 File Offset: 0x000CC1D5
		// (set) Token: 0x060024CA RID: 9418 RVA: 0x000CE000 File Offset: 0x000CC200
		public override Coordinates WindowPosition
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.WindowPosition))
				{
					return (Coordinates)this.HostDefaultData.GetValue(HostDefaultDataId.WindowPosition);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetWindowPosition);
			}
			set
			{
				this.HostDefaultData.SetValue(HostDefaultDataId.WindowPosition, value);
				this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetWindowPosition, new object[]
				{
					value
				});
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x000CE03D File Offset: 0x000CC23D
		// (set) Token: 0x060024CC RID: 9420 RVA: 0x000CE068 File Offset: 0x000CC268
		public override int CursorSize
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.CursorSize))
				{
					return (int)this.HostDefaultData.GetValue(HostDefaultDataId.CursorSize);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetCursorSize);
			}
			set
			{
				this.HostDefaultData.SetValue(HostDefaultDataId.CursorSize, value);
				this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetCursorSize, new object[]
				{
					value
				});
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060024CD RID: 9421 RVA: 0x000CE0A5 File Offset: 0x000CC2A5
		// (set) Token: 0x060024CE RID: 9422 RVA: 0x000CE0D0 File Offset: 0x000CC2D0
		public override Size BufferSize
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.BufferSize))
				{
					return (Size)this.HostDefaultData.GetValue(HostDefaultDataId.BufferSize);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetBufferSize);
			}
			set
			{
				this.HostDefaultData.SetValue(HostDefaultDataId.BufferSize, value);
				this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetBufferSize, new object[]
				{
					value
				});
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060024CF RID: 9423 RVA: 0x000CE10D File Offset: 0x000CC30D
		// (set) Token: 0x060024D0 RID: 9424 RVA: 0x000CE138 File Offset: 0x000CC338
		public override Size WindowSize
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.WindowSize))
				{
					return (Size)this.HostDefaultData.GetValue(HostDefaultDataId.WindowSize);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetWindowSize);
			}
			set
			{
				this.HostDefaultData.SetValue(HostDefaultDataId.WindowSize, value);
				this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetWindowSize, new object[]
				{
					value
				});
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060024D1 RID: 9425 RVA: 0x000CE175 File Offset: 0x000CC375
		// (set) Token: 0x060024D2 RID: 9426 RVA: 0x000CE1A0 File Offset: 0x000CC3A0
		public override string WindowTitle
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.WindowTitle))
				{
					return (string)this.HostDefaultData.GetValue(HostDefaultDataId.WindowTitle);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetWindowTitle);
			}
			set
			{
				this.HostDefaultData.SetValue(HostDefaultDataId.WindowTitle, value);
				this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetWindowTitle, new object[]
				{
					value
				});
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060024D3 RID: 9427 RVA: 0x000CE1D4 File Offset: 0x000CC3D4
		public override Size MaxWindowSize
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.MaxWindowSize))
				{
					return (Size)this.HostDefaultData.GetValue(HostDefaultDataId.MaxWindowSize);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetMaxWindowSize);
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x000CE1FD File Offset: 0x000CC3FD
		public override Size MaxPhysicalWindowSize
		{
			get
			{
				if (this.HostDefaultData.HasValue(HostDefaultDataId.MaxPhysicalWindowSize))
				{
					return (Size)this.HostDefaultData.GetValue(HostDefaultDataId.MaxPhysicalWindowSize);
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetMaxPhysicalWindowSize);
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060024D5 RID: 9429 RVA: 0x000CE226 File Offset: 0x000CC426
		public override bool KeyAvailable
		{
			get
			{
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetKeyAvailable);
			}
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x000CE230 File Offset: 0x000CC430
		public override KeyInfo ReadKey(ReadKeyOptions options)
		{
			return this._serverMethodExecutor.ExecuteMethod<KeyInfo>(RemoteHostMethodId.ReadKey, new object[]
			{
				options
			});
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x000CE25B File Offset: 0x000CC45B
		public override void FlushInputBuffer()
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.FlushInputBuffer);
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x000CE26C File Offset: 0x000CC46C
		public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.ScrollBufferContents, new object[]
			{
				source,
				destination,
				clip,
				fill
			});
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000CE2B4 File Offset: 0x000CC4B4
		public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetBufferContents1, new object[]
			{
				rectangle,
				fill
			});
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x000CE2E8 File Offset: 0x000CC4E8
		public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetBufferContents2, new object[]
			{
				origin,
				contents
			});
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x000CE317 File Offset: 0x000CC517
		public override BufferCell[,] GetBufferContents(Rectangle rectangle)
		{
			throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetBufferContents);
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000CE320 File Offset: 0x000CC520
		public override int LengthInBufferCells(string source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return source.Length;
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000CE336 File Offset: 0x000CC536
		public override int LengthInBufferCells(string source, int offset)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return source.Length - offset;
		}

		// Token: 0x0400120C RID: 4620
		private ServerRemoteHostUserInterface _remoteHostUserInterface;

		// Token: 0x0400120D RID: 4621
		private ServerMethodExecutor _serverMethodExecutor;
	}
}
