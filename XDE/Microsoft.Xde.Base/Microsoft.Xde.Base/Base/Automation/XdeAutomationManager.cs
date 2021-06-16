using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Xde.Common;
using Microsoft.Xde.Interface;

namespace Microsoft.Xde.Base.Automation
{
	// Token: 0x0200000F RID: 15
	[Export(typeof(IXdeAutomationServices))]
	public class XdeAutomationManager : IXdeAutomationServices, IDisposable
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00005B2A File Offset: 0x00003D2A
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00005B32 File Offset: 0x00003D32
		[Import]
		public IXdeArgsProcessor Args { get; set; }

		// Token: 0x06000114 RID: 276 RVA: 0x00005B3B File Offset: 0x00003D3B
		public void RegisterAutomationFeature<T>(T implementation) where T : class
		{
			if (this.Args.AutomateFeatures)
			{
				this.interfaceToImpl[typeof(T)] = XdeFeatureServerSession<T>.ServiceHostFactory(this.Args.VirtualMachineName, implementation);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005B70 File Offset: 0x00003D70
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			foreach (IDisposable disposable in this.interfaceToImpl.Values)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x04000066 RID: 102
		private Dictionary<Type, IDisposable> interfaceToImpl = new Dictionary<Type, IDisposable>();
	}
}
