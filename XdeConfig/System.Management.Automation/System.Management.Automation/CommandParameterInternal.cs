using System;
using System.Diagnostics;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x02000010 RID: 16
	[DebuggerDisplay("{ParameterName}")]
	internal sealed class CommandParameterInternal
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000420B File Offset: 0x0000240B
		internal bool SpaceAfterParameter
		{
			get
			{
				return this._spaceAfterParameter;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004213 File Offset: 0x00002413
		internal bool ParameterNameSpecified
		{
			get
			{
				return this._parameter != null;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004221 File Offset: 0x00002421
		internal bool ArgumentSpecified
		{
			get
			{
				return this._argument != null;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000422F File Offset: 0x0000242F
		internal bool ParameterAndArgumentSpecified
		{
			get
			{
				return this.ParameterNameSpecified && this.ArgumentSpecified;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004241 File Offset: 0x00002441
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000424E File Offset: 0x0000244E
		internal string ParameterName
		{
			get
			{
				return this._parameter.parameterName;
			}
			set
			{
				this._parameter.parameterName = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000425C File Offset: 0x0000245C
		internal string ParameterText
		{
			get
			{
				return this._parameter.parameterText;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004269 File Offset: 0x00002469
		internal IScriptExtent ParameterExtent
		{
			get
			{
				if (this._parameter == null)
				{
					return PositionUtilities.EmptyExtent;
				}
				return this._parameter.extent;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004284 File Offset: 0x00002484
		internal IScriptExtent ArgumentExtent
		{
			get
			{
				if (this._argument == null)
				{
					return PositionUtilities.EmptyExtent;
				}
				return this._argument.extent;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000429F File Offset: 0x0000249F
		internal object ArgumentValue
		{
			get
			{
				if (this._argument == null)
				{
					return UnboundParameter.Value;
				}
				return this._argument.value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000042BA File Offset: 0x000024BA
		internal bool ArgumentSplatted
		{
			get
			{
				return this._argument != null && this._argument.splatted;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000042D1 File Offset: 0x000024D1
		internal bool ArrayIsSingleArgumentForNativeCommand
		{
			get
			{
				return this._argument != null && this._argument.arrayIsSingleArgumentForNativeCommand;
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000042E8 File Offset: 0x000024E8
		internal void SetArgumentValue(IScriptExtent extent, object value)
		{
			if (this._argument == null)
			{
				this._argument = new CommandParameterInternal.Argument();
			}
			this._argument.value = value;
			this._argument.extent = extent;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004318 File Offset: 0x00002518
		internal IScriptExtent ErrorExtent
		{
			get
			{
				if (this._argument != null && this._argument.extent != PositionUtilities.EmptyExtent)
				{
					return this._argument.extent;
				}
				if (this._parameter == null)
				{
					return PositionUtilities.EmptyExtent;
				}
				return this._parameter.extent;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004364 File Offset: 0x00002564
		internal static CommandParameterInternal CreateParameter(IScriptExtent extent, string parameterName, string parameterText)
		{
			return new CommandParameterInternal
			{
				_parameter = new CommandParameterInternal.Parameter
				{
					extent = extent,
					parameterName = parameterName,
					parameterText = parameterText
				}
			};
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000439C File Offset: 0x0000259C
		internal static CommandParameterInternal CreateArgument(IScriptExtent extent, object value, bool splatted = false, bool arrayIsSingleArgumentForNativeCommand = false)
		{
			return new CommandParameterInternal
			{
				_argument = new CommandParameterInternal.Argument
				{
					extent = extent,
					value = value,
					splatted = splatted,
					arrayIsSingleArgumentForNativeCommand = arrayIsSingleArgumentForNativeCommand
				}
			};
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000043DC File Offset: 0x000025DC
		internal static CommandParameterInternal CreateParameterWithArgument(IScriptExtent parameterExtent, string parameterName, string parameterText, IScriptExtent argumentExtent, object value, bool spaceAfterParameter, bool arrayIsSingleArgumentForNativeCommand = false)
		{
			return new CommandParameterInternal
			{
				_parameter = new CommandParameterInternal.Parameter
				{
					extent = parameterExtent,
					parameterName = parameterName,
					parameterText = parameterText
				},
				_argument = new CommandParameterInternal.Argument
				{
					extent = argumentExtent,
					value = value,
					arrayIsSingleArgumentForNativeCommand = arrayIsSingleArgumentForNativeCommand
				},
				_spaceAfterParameter = spaceAfterParameter
			};
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000443E File Offset: 0x0000263E
		internal bool IsDashQuestion()
		{
			return this.ParameterNameSpecified && this.ParameterName.Equals("?", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x04000039 RID: 57
		private CommandParameterInternal.Parameter _parameter;

		// Token: 0x0400003A RID: 58
		private CommandParameterInternal.Argument _argument;

		// Token: 0x0400003B RID: 59
		private bool _spaceAfterParameter;

		// Token: 0x02000011 RID: 17
		private class Parameter
		{
			// Token: 0x0400003C RID: 60
			internal IScriptExtent extent;

			// Token: 0x0400003D RID: 61
			internal string parameterName;

			// Token: 0x0400003E RID: 62
			internal string parameterText;
		}

		// Token: 0x02000012 RID: 18
		private class Argument
		{
			// Token: 0x0400003F RID: 63
			internal IScriptExtent extent;

			// Token: 0x04000040 RID: 64
			internal object value;

			// Token: 0x04000041 RID: 65
			internal bool splatted;

			// Token: 0x04000042 RID: 66
			internal bool arrayIsSingleArgumentForNativeCommand;
		}
	}
}
