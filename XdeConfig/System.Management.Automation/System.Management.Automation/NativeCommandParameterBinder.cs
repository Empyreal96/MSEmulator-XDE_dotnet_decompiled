using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200008E RID: 142
	internal class NativeCommandParameterBinder : ParameterBinderBase
	{
		// Token: 0x0600072E RID: 1838 RVA: 0x00022A82 File Offset: 0x00020C82
		internal NativeCommandParameterBinder(NativeCommand command) : base(command.MyInvocation, command.Context, command)
		{
			this.nativeCommand = command;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00022AA9 File Offset: 0x00020CA9
		internal override void BindParameter(string name, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00022AB0 File Offset: 0x00020CB0
		internal override object GetDefaultParameterValue(string name)
		{
			return null;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00022AB4 File Offset: 0x00020CB4
		internal void BindParameters(Collection<CommandParameterInternal> parameters)
		{
			bool sawVerbatimArgumentMarker = false;
			bool flag = true;
			foreach (CommandParameterInternal commandParameterInternal in parameters)
			{
				if (!flag)
				{
					this.arguments.Append(' ');
				}
				flag = false;
				if (commandParameterInternal.ParameterNameSpecified)
				{
					this.arguments.Append(commandParameterInternal.ParameterText);
					if (commandParameterInternal.SpaceAfterParameter)
					{
						this.arguments.Append(' ');
					}
				}
				if (commandParameterInternal.ArgumentSpecified)
				{
					object argumentValue = commandParameterInternal.ArgumentValue;
					if (string.Equals("--%", argumentValue as string, StringComparison.OrdinalIgnoreCase))
					{
						sawVerbatimArgumentMarker = true;
					}
					else if (argumentValue != AutomationNull.Value && argumentValue != UnboundParameter.Value)
					{
						this.appendOneNativeArgument(base.Context, argumentValue, commandParameterInternal.ArrayIsSingleArgumentForNativeCommand ? ',' : ' ', sawVerbatimArgumentMarker);
					}
				}
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x00022B9C File Offset: 0x00020D9C
		internal string Arguments
		{
			get
			{
				NativeCommandParameterBinder.tracer.WriteLine("Raw argument string: " + this.arguments.ToString(), new object[0]);
				string[] array = CommandLineParameterBinderNativeMethods.PreParseCommandLine(this.arguments.ToString());
				for (int i = 0; i < array.Length; i++)
				{
					NativeCommandParameterBinder.tracer.WriteLine("Argument {0}: {1}", new object[]
					{
						i,
						array[i]
					});
				}
				return this.arguments.ToString();
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00022C20 File Offset: 0x00020E20
		private void appendOneNativeArgument(ExecutionContext context, object obj, char separator, bool sawVerbatimArgumentMarker)
		{
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(obj);
			bool flag = false;
			for (;;)
			{
				string text;
				if (enumerator == null)
				{
					text = PSObject.ToStringParser(context, obj);
				}
				else
				{
					if (!ParserOps.MoveNext(context, null, enumerator))
					{
						break;
					}
					text = PSObject.ToStringParser(context, ParserOps.Current(null, enumerator));
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (flag)
					{
						this.arguments.Append(separator);
					}
					else
					{
						flag = true;
					}
					if (sawVerbatimArgumentMarker)
					{
						text = Environment.ExpandEnvironmentVariables(text);
						this.arguments.Append(text);
					}
					else
					{
						bool flag2 = false;
						int num = 0;
						for (int i = 0; i < text.Length; i++)
						{
							if (text[i] == '"')
							{
								num++;
							}
							else if (char.IsWhiteSpace(text[i]) && num % 2 == 0)
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							this.arguments.Append('"');
							this.arguments.Append(text);
							this.arguments.Append('"');
						}
						else
						{
							this.arguments.Append(text);
						}
					}
				}
				if (enumerator == null)
				{
					return;
				}
			}
		}

		// Token: 0x04000313 RID: 787
		[TraceSource("NativeCommandParameterBinder", "The parameter binder for native commands")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("NativeCommandParameterBinder", "The parameter binder for native commands");

		// Token: 0x04000314 RID: 788
		private readonly StringBuilder arguments = new StringBuilder();

		// Token: 0x04000315 RID: 789
		private NativeCommand nativeCommand;
	}
}
