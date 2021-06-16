using System;
using System.IO;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x020002F6 RID: 758
	internal sealed class OutOfProcessMediator : OutOfProcessMediatorBase
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x000C9BFC File Offset: 0x000C7DFC
		private OutOfProcessMediator() : base(true)
		{
			this.originalStdIn = Console.In;
			Console.SetIn(TextReader.Null);
			this.originalStdOut = new OutOfProcessTextWriter(Console.Out);
			Console.SetOut(TextWriter.Null);
			this.originalStdErr = new OutOfProcessTextWriter(Console.Error);
			Console.SetError(TextWriter.Null);
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000C9C5C File Offset: 0x000C7E5C
		internal static void Run(string initialCommand)
		{
			lock (OutOfProcessMediatorBase.SyncObject)
			{
				if (OutOfProcessMediator.SingletonInstance != null)
				{
					return;
				}
				OutOfProcessMediator.SingletonInstance = new OutOfProcessMediator();
			}
			AppDomain.CurrentDomain.UnhandledException += OutOfProcessMediatorBase.AppDomainUnhandledException;
			OutOfProcessMediator.SingletonInstance.Start(initialCommand);
		}

		// Token: 0x040011B0 RID: 4528
		private static OutOfProcessMediator SingletonInstance;
	}
}
