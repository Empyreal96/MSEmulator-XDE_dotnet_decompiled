using System;
using System.Collections.Generic;
using System.Reflection;
using DiscUtils.CoreCompat;

namespace DiscUtils.Setup
{
	// Token: 0x02000045 RID: 69
	public static class SetupHelper
	{
		// Token: 0x060002D4 RID: 724 RVA: 0x000063F6 File Offset: 0x000045F6
		static SetupHelper()
		{
			SetupHelper.RegisterAssembly(ReflectionHelper.GetAssembly(typeof(SetupHelper)));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00006418 File Offset: 0x00004618
		public static void RegisterAssembly(Assembly assembly)
		{
			HashSet<string> alreadyLoaded = SetupHelper._alreadyLoaded;
			lock (alreadyLoaded)
			{
				if (SetupHelper._alreadyLoaded.Add(assembly.FullName))
				{
					FileSystemManager.RegisterFileSystems(assembly);
					VirtualDiskManager.RegisterVirtualDiskTypes(assembly);
					VolumeManager.RegisterLogicalVolumeFactory(assembly);
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002D6 RID: 726 RVA: 0x00006478 File Offset: 0x00004678
		// (remove) Token: 0x060002D7 RID: 727 RVA: 0x000064AC File Offset: 0x000046AC
		public static event EventHandler<FileOpenEventArgs> OpeningFile;

		// Token: 0x060002D8 RID: 728 RVA: 0x000064DF File Offset: 0x000046DF
		internal static void OnOpeningFile(object sender, FileOpenEventArgs e)
		{
			EventHandler<FileOpenEventArgs> openingFile = SetupHelper.OpeningFile;
			if (openingFile == null)
			{
				return;
			}
			openingFile(sender, e);
		}

		// Token: 0x040000A0 RID: 160
		private static readonly HashSet<string> _alreadyLoaded = new HashSet<string>();
	}
}
