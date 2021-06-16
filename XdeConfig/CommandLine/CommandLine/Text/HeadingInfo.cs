using System;
using System.IO;
using System.Reflection;
using System.Text;
using CommandLine.Infrastructure;
using CSharpx;

namespace CommandLine.Text
{
	// Token: 0x02000057 RID: 87
	public class HeadingInfo
	{
		// Token: 0x060001FC RID: 508 RVA: 0x00008894 File Offset: 0x00006A94
		public HeadingInfo(string programName, string version = null)
		{
			if (string.IsNullOrWhiteSpace("programName"))
			{
				throw new ArgumentException("programName");
			}
			this.programName = programName;
			this.version = version;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000088C1 File Offset: 0x00006AC1
		public static HeadingInfo Empty
		{
			get
			{
				return new HeadingInfo("", null);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001FE RID: 510 RVA: 0x000088D0 File Offset: 0x00006AD0
		public static HeadingInfo Default
		{
			get
			{
				string text = ReflectionHelper.GetAttribute<AssemblyTitleAttribute>().MapValueOrDefault((AssemblyTitleAttribute titleAttribute) => titleAttribute.Title, ReflectionHelper.GetAssemblyName());
				string text2 = ReflectionHelper.GetAttribute<AssemblyInformationalVersionAttribute>().MapValueOrDefault((AssemblyInformationalVersionAttribute versionAttribute) => versionAttribute.InformationalVersion, ReflectionHelper.GetAssemblyVersion());
				return new HeadingInfo(text, text2);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00008940 File Offset: 0x00006B40
		public static implicit operator string(HeadingInfo info)
		{
			return info.ToString();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00008948 File Offset: 0x00006B48
		public override string ToString()
		{
			bool flag = string.IsNullOrEmpty(this.version);
			return new StringBuilder(this.programName.Length + ((!flag) ? (this.version.Length + 1) : 0)).Append(this.programName).AppendWhen(!flag, new string[]
			{
				" ",
				this.version
			}).ToString();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000089B8 File Offset: 0x00006BB8
		public void WriteMessage(string message, TextWriter writer)
		{
			if (string.IsNullOrWhiteSpace("message"))
			{
				throw new ArgumentException("message");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.WriteLine(new StringBuilder(this.programName.Length + message.Length + 2).Append(this.programName).Append(": ").Append(message).ToString());
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00008A29 File Offset: 0x00006C29
		public void WriteMessage(string message)
		{
			this.WriteMessage(message, Console.Out);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00008A37 File Offset: 0x00006C37
		public void WriteError(string message)
		{
			this.WriteMessage(message, Console.Error);
		}

		// Token: 0x0400009B RID: 155
		private readonly string programName;

		// Token: 0x0400009C RID: 156
		private readonly string version;
	}
}
