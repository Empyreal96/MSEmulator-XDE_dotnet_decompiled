using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A84 RID: 2692
	internal struct ExcepInfo
	{
		// Token: 0x06006AFD RID: 27389 RVA: 0x002189F8 File Offset: 0x00216BF8
		private static string ConvertAndFreeBstr(ref IntPtr bstr)
		{
			if (bstr == IntPtr.Zero)
			{
				return null;
			}
			string result = Marshal.PtrToStringBSTR(bstr);
			Marshal.FreeBSTR(bstr);
			bstr = IntPtr.Zero;
			return result;
		}

		// Token: 0x06006AFE RID: 27390 RVA: 0x00218A3C File Offset: 0x00216C3C
		internal void Dummy()
		{
			this.wCode = 0;
			this.wReserved = 0;
			this.wReserved += 1;
			this.bstrSource = IntPtr.Zero;
			this.bstrDescription = IntPtr.Zero;
			this.bstrHelpFile = IntPtr.Zero;
			this.dwHelpContext = 0;
			this.pfnDeferredFillIn = IntPtr.Zero;
			this.pvReserved = IntPtr.Zero;
			this.scode = 0;
			throw Error.MethodShouldNotBeCalled();
		}

		// Token: 0x06006AFF RID: 27391 RVA: 0x00218AB0 File Offset: 0x00216CB0
		internal Exception GetException()
		{
			int errorCode = (this.scode != 0) ? this.scode : ((int)this.wCode);
			Exception ex = Marshal.GetExceptionForHR(errorCode);
			string text = ExcepInfo.ConvertAndFreeBstr(ref this.bstrDescription);
			if (text != null)
			{
				if (ex is COMException)
				{
					ex = new COMException(text, errorCode);
				}
				else
				{
					Type type = ex.GetType();
					ConstructorInfo constructor = type.GetConstructor(new Type[]
					{
						typeof(string)
					});
					if (constructor != null)
					{
						ex = (Exception)constructor.Invoke(new object[]
						{
							text
						});
					}
				}
			}
			ex.Source = ExcepInfo.ConvertAndFreeBstr(ref this.bstrSource);
			string text2 = ExcepInfo.ConvertAndFreeBstr(ref this.bstrHelpFile);
			if (text2 != null && this.dwHelpContext != 0)
			{
				text2 = text2 + "#" + this.dwHelpContext;
			}
			ex.HelpLink = text2;
			return ex;
		}

		// Token: 0x0400332A RID: 13098
		private short wCode;

		// Token: 0x0400332B RID: 13099
		private short wReserved;

		// Token: 0x0400332C RID: 13100
		private IntPtr bstrSource;

		// Token: 0x0400332D RID: 13101
		private IntPtr bstrDescription;

		// Token: 0x0400332E RID: 13102
		private IntPtr bstrHelpFile;

		// Token: 0x0400332F RID: 13103
		private int dwHelpContext;

		// Token: 0x04003330 RID: 13104
		private IntPtr pvReserved;

		// Token: 0x04003331 RID: 13105
		private IntPtr pfnDeferredFillIn;

		// Token: 0x04003332 RID: 13106
		private int scode;
	}
}
