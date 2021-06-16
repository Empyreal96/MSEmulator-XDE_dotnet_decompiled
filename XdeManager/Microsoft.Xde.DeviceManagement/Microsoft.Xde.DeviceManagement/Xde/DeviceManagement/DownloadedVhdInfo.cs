using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x02000008 RID: 8
	[JsonObject(MemberSerialization.OptIn)]
	public class DownloadedVhdInfo
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00002A94 File Offset: 0x00000C94
		public static void RecordDownloadedVhdInfo(string source, string dest)
		{
			DownloadedVhdInfo value = new DownloadedVhdInfo
			{
				Source = source
			};
			string directoryName = Path.GetDirectoryName(dest);
			string path = Path.GetFileName(dest) + ".json";
			using (StreamWriter streamWriter = File.CreateText(Path.Combine(directoryName, path)))
			{
				new JsonSerializer().Serialize(streamWriter, value);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002AFC File Offset: 0x00000CFC
		public static DownloadedVhdInfo LoadInfoForDownloadedVhd(string vhd)
		{
			string path = vhd + ".json";
			if (!File.Exists(path))
			{
				return null;
			}
			DownloadedVhdInfo result;
			using (StreamReader streamReader = File.OpenText(path))
			{
				DownloadedVhdInfo downloadedVhdInfo = (DownloadedVhdInfo)new JsonSerializer().Deserialize(streamReader, typeof(DownloadedVhdInfo));
				if (string.IsNullOrEmpty(downloadedVhdInfo.UapVersion) && !string.IsNullOrEmpty(downloadedVhdInfo.BuildVersion))
				{
					string[] array = downloadedVhdInfo.BuildVersion.Split(new char[]
					{
						'.'
					});
					if (array.Length != 0)
					{
						string str = array[0];
						downloadedVhdInfo.UapVersion = "10.0." + str + ".0";
					}
				}
				if (string.IsNullOrEmpty(downloadedVhdInfo.UapVersion))
				{
					downloadedVhdInfo.UapVersion = "10.0.18362.0";
				}
				result = downloadedVhdInfo;
			}
			return result;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002BCC File Offset: 0x00000DCC
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002BD4 File Offset: 0x00000DD4
		[JsonProperty]
		public string Source
		{
			get
			{
				return this.source;
			}
			private set
			{
				if (value == this.source)
				{
					return;
				}
				this.source = value;
				Match match = DownloadedVhdInfo.PathRegex.Match(value);
				if (match.Success && match.Groups.Count == 4)
				{
					this.Branch = match.Groups[1].Value;
					this.BuildVersion = match.Groups[2].Value;
					this.BuildRelativePath = match.Groups[3].Value;
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002C5E File Offset: 0x00000E5E
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002C66 File Offset: 0x00000E66
		[JsonProperty]
		public string UapVersion { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002C6F File Offset: 0x00000E6F
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002C77 File Offset: 0x00000E77
		public string Branch { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002C80 File Offset: 0x00000E80
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002C88 File Offset: 0x00000E88
		public string BuildVersion { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002C91 File Offset: 0x00000E91
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002C99 File Offset: 0x00000E99
		public string BuildRelativePath { get; private set; }

		// Token: 0x0600005F RID: 95 RVA: 0x00002CA2 File Offset: 0x00000EA2
		public static IEnumerable<string> GetLatestVhdFileNamesFromBranch(string branch, string buildRelativePath, int max)
		{
			return DownloadedVhdInfo.GetLatestVhdFileNamesFromBranch(branch, buildRelativePath, max, CancellationToken.None);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002CB1 File Offset: 0x00000EB1
		public static IEnumerable<string> GetLatestVhdFileNamesFromBranch(string branch, string buildRelativePath, int max, CancellationToken cancelToken)
		{
			string path = "\\\\winbuilds\\release\\" + branch;
			int count = 0;
			if (!Directory.Exists(path))
			{
				yield break;
			}
			string[] array = (from s in Directory.GetDirectories(path)
			orderby s descending
			select s).Take(20).ToArray<string>();
			if (cancelToken.IsCancellationRequested)
			{
				yield break;
			}
			foreach (string path2 in array)
			{
				if (cancelToken.IsCancellationRequested)
				{
					yield break;
				}
				string text = Path.Combine(path2, buildRelativePath);
				if (File.Exists(text))
				{
					yield return text;
					int num = count;
					count = num + 1;
					if (count >= max)
					{
						break;
					}
				}
				else
				{
					string text2 = buildRelativePath.Replace("\\images\\", "\\wcos_images\\");
					if (text2 == buildRelativePath)
					{
						text2 = buildRelativePath.Replace("\\wcos_images\\", "\\images\\");
					}
					if (text2 != buildRelativePath)
					{
						text = Path.Combine(path2, text2);
						if (File.Exists(text))
						{
							yield return text;
							int num = count;
							count = num + 1;
							if (count >= max)
							{
								break;
							}
						}
					}
				}
			}
			string[] array2 = null;
			yield break;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002CD6 File Offset: 0x00000ED6
		public IEnumerable<string> GetLatestVhdFileNamesFromSource(int max)
		{
			return DownloadedVhdInfo.GetLatestVhdFileNamesFromBranch(this.Branch, this.BuildRelativePath, max);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002CEA File Offset: 0x00000EEA
		public string GetLatestVhdFileNameFromSource()
		{
			return this.GetLatestVhdFileNamesFromSource(1).FirstOrDefault<string>();
		}

		// Token: 0x0400001F RID: 31
		public const string BuildVer19H1 = "10.0.18362.0";

		// Token: 0x04000020 RID: 32
		private static readonly Regex PathRegex = new Regex("\\\\\\\\winbuilds\\\\release\\\\([a-zA-Z_0-9]+)\\\\([0-9\\.-]+)\\\\(.+)");

		// Token: 0x04000021 RID: 33
		private string source;
	}
}
