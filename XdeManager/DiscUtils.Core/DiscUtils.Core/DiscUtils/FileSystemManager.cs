using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DiscUtils.CoreCompat;
using DiscUtils.Vfs;

namespace DiscUtils
{
	// Token: 0x02000011 RID: 17
	public static class FileSystemManager
	{
		// Token: 0x060000BC RID: 188 RVA: 0x00002CBF File Offset: 0x00000EBF
		public static void RegisterFileSystems(VfsFileSystemFactory factory)
		{
			FileSystemManager._factories.Add(factory);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00002CCC File Offset: 0x00000ECC
		public static void RegisterFileSystems(Assembly assembly)
		{
			FileSystemManager._factories.AddRange(FileSystemManager.DetectFactories(assembly));
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public static FileSystemInfo[] DetectFileSystems(VolumeInfo volume)
		{
			FileSystemInfo[] result;
			using (Stream stream = volume.Open())
			{
				result = FileSystemManager.DoDetect(stream, volume);
			}
			return result;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00002D1C File Offset: 0x00000F1C
		public static FileSystemInfo[] DetectFileSystems(Stream stream)
		{
			return FileSystemManager.DoDetect(stream, null);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00002D25 File Offset: 0x00000F25
		private static IEnumerable<VfsFileSystemFactory> DetectFactories(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (ReflectionHelper.GetCustomAttribute(type, typeof(VfsFileSystemFactoryAttribute), false) != null)
				{
					yield return (VfsFileSystemFactory)Activator.CreateInstance(type);
				}
			}
			Type[] array = null;
			yield break;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00002D38 File Offset: 0x00000F38
		private static FileSystemInfo[] DoDetect(Stream stream, VolumeInfo volume)
		{
			BufferedStream stream2 = new BufferedStream(stream);
			List<FileSystemInfo> list = new List<FileSystemInfo>();
			foreach (VfsFileSystemFactory vfsFileSystemFactory in FileSystemManager._factories)
			{
				list.AddRange(vfsFileSystemFactory.Detect(stream2, volume));
			}
			return list.ToArray();
		}

		// Token: 0x0400001E RID: 30
		private static readonly List<VfsFileSystemFactory> _factories = new List<VfsFileSystemFactory>();
	}
}
