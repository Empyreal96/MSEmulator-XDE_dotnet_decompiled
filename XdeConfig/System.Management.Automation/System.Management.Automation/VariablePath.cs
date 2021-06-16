using System;

namespace System.Management.Automation
{
	// Token: 0x02000847 RID: 2119
	public class VariablePath
	{
		// Token: 0x06005180 RID: 20864 RVA: 0x001B2472 File Offset: 0x001B0672
		private VariablePath()
		{
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x001B247A File Offset: 0x001B067A
		public VariablePath(string path) : this(path, VariablePathFlags.None)
		{
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x001B2484 File Offset: 0x001B0684
		internal VariablePath(string path, VariablePathFlags knownFlags)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			this._userPath = path;
			this._flags = knownFlags;
			string text = null;
			string text2 = null;
			VariablePathFlags variablePathFlags = VariablePathFlags.Unqualified;
			int num = 0;
			int num2 = -1;
			for (;;)
			{
				char c = path[0];
				if (c <= 'V')
				{
					if (c <= 'L')
					{
						if (c == 'G')
						{
							goto IL_98;
						}
						if (c == 'L')
						{
							goto IL_A8;
						}
					}
					else
					{
						if (c == 'P')
						{
							goto IL_B8;
						}
						if (c == 'S')
						{
							goto IL_C8;
						}
						if (c == 'V')
						{
							goto IL_D8;
						}
					}
				}
				else if (c <= 'l')
				{
					if (c == 'g')
					{
						goto IL_98;
					}
					if (c == 'l')
					{
						goto IL_A8;
					}
				}
				else
				{
					if (c == 'p')
					{
						goto IL_B8;
					}
					if (c == 's')
					{
						goto IL_C8;
					}
					if (c == 'v')
					{
						goto IL_D8;
					}
				}
				IL_EA:
				if (text == null)
				{
					break;
				}
				num++;
				int num3 = 0;
				while (num < path.Length && num3 < text.Length && (path[num] == text[num3] || path[num] == text2[num3]))
				{
					num3++;
					num++;
				}
				if (num3 != text.Length || num >= path.Length || path[num] != ':')
				{
					break;
				}
				if (this._flags == VariablePathFlags.None)
				{
					this._flags = VariablePathFlags.Variable;
				}
				this._flags |= variablePathFlags;
				num2 = num;
				num++;
				if (variablePathFlags == VariablePathFlags.Variable)
				{
					knownFlags = VariablePathFlags.Variable;
					text2 = (text = null);
					variablePathFlags = VariablePathFlags.None;
					continue;
				}
				break;
				IL_98:
				text = "lobal";
				text2 = "LOBAL";
				variablePathFlags = VariablePathFlags.Global;
				goto IL_EA;
				IL_A8:
				text = "ocal";
				text2 = "OCAL";
				variablePathFlags = VariablePathFlags.Local;
				goto IL_EA;
				IL_B8:
				text = "rivate";
				text2 = "RIVATE";
				variablePathFlags = VariablePathFlags.Private;
				goto IL_EA;
				IL_C8:
				text = "cript";
				text2 = "CRIPT";
				variablePathFlags = VariablePathFlags.Script;
				goto IL_EA;
				IL_D8:
				if (knownFlags == VariablePathFlags.None)
				{
					text = "ariable";
					text2 = "ARIABLE";
					variablePathFlags = VariablePathFlags.Variable;
					goto IL_EA;
				}
				goto IL_EA;
			}
			if (this._flags == VariablePathFlags.None)
			{
				num2 = path.IndexOf(':', num);
				if (num2 > 0)
				{
					this._flags = VariablePathFlags.DriveQualified;
				}
			}
			if (num2 == -1)
			{
				this._unqualifiedPath = this._userPath;
			}
			else
			{
				this._unqualifiedPath = this._userPath.Substring(num2 + 1);
			}
			if (this._flags == VariablePathFlags.None)
			{
				this._flags = (VariablePathFlags.Variable | VariablePathFlags.Unqualified);
			}
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x001B267C File Offset: 0x001B087C
		internal VariablePath CloneAndSetLocal()
		{
			return new VariablePath
			{
				_userPath = this._userPath,
				_unqualifiedPath = this._unqualifiedPath,
				_flags = (VariablePathFlags.Local | VariablePathFlags.Variable)
			};
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06005184 RID: 20868 RVA: 0x001B26B0 File Offset: 0x001B08B0
		public string UserPath
		{
			get
			{
				return this._userPath;
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x06005185 RID: 20869 RVA: 0x001B26B8 File Offset: 0x001B08B8
		public bool IsGlobal
		{
			get
			{
				return VariablePathFlags.None != (this._flags & VariablePathFlags.Global);
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x06005186 RID: 20870 RVA: 0x001B26C8 File Offset: 0x001B08C8
		public bool IsLocal
		{
			get
			{
				return VariablePathFlags.None != (this._flags & VariablePathFlags.Local);
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06005187 RID: 20871 RVA: 0x001B26D8 File Offset: 0x001B08D8
		public bool IsPrivate
		{
			get
			{
				return VariablePathFlags.None != (this._flags & VariablePathFlags.Private);
			}
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06005188 RID: 20872 RVA: 0x001B26E8 File Offset: 0x001B08E8
		public bool IsScript
		{
			get
			{
				return VariablePathFlags.None != (this._flags & VariablePathFlags.Script);
			}
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06005189 RID: 20873 RVA: 0x001B26F8 File Offset: 0x001B08F8
		public bool IsUnqualified
		{
			get
			{
				return VariablePathFlags.None != (this._flags & VariablePathFlags.Unqualified);
			}
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x0600518A RID: 20874 RVA: 0x001B270C File Offset: 0x001B090C
		public bool IsUnscopedVariable
		{
			get
			{
				return VariablePathFlags.None == (this._flags & VariablePathFlags.UnscopedVariableMask);
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x0600518B RID: 20875 RVA: 0x001B271A File Offset: 0x001B091A
		public bool IsVariable
		{
			get
			{
				return VariablePathFlags.None != (this._flags & VariablePathFlags.Variable);
			}
		}

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x0600518C RID: 20876 RVA: 0x001B272B File Offset: 0x001B092B
		internal bool IsFunction
		{
			get
			{
				return VariablePathFlags.None != (this._flags & VariablePathFlags.Function);
			}
		}

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x0600518D RID: 20877 RVA: 0x001B273C File Offset: 0x001B093C
		public bool IsDriveQualified
		{
			get
			{
				return VariablePathFlags.None != (this._flags & VariablePathFlags.DriveQualified);
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x0600518E RID: 20878 RVA: 0x001B274D File Offset: 0x001B094D
		public string DriveName
		{
			get
			{
				if (!this.IsDriveQualified)
				{
					return null;
				}
				return this._userPath.Substring(0, this._userPath.IndexOf(':'));
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x0600518F RID: 20879 RVA: 0x001B2772 File Offset: 0x001B0972
		internal string UnqualifiedPath
		{
			get
			{
				return this._unqualifiedPath;
			}
		}

		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06005190 RID: 20880 RVA: 0x001B277A File Offset: 0x001B097A
		internal string QualifiedName
		{
			get
			{
				if (!this.IsDriveQualified)
				{
					return this._unqualifiedPath;
				}
				return this._userPath;
			}
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x001B2791 File Offset: 0x001B0991
		public override string ToString()
		{
			return this._userPath;
		}

		// Token: 0x040029D6 RID: 10710
		private string _userPath;

		// Token: 0x040029D7 RID: 10711
		private string _unqualifiedPath;

		// Token: 0x040029D8 RID: 10712
		private VariablePathFlags _flags;
	}
}
