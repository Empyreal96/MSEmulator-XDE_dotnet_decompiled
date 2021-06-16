using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation.Language;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x0200008C RID: 140
	internal class MinishellParameterBinderController : NativeCommandParameterBinderController
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x000223C2 File Offset: 0x000205C2
		internal MinishellParameterBinderController(NativeCommand command) : base(command)
		{
			this.InputFormat = NativeCommandIOFormat.Xml;
			this.OutputFormat = NativeCommandIOFormat.Text;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x000223D9 File Offset: 0x000205D9
		internal override Collection<CommandParameterInternal> BindParameters(Collection<CommandParameterInternal> parameters)
		{
			return null;
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x000223DC File Offset: 0x000205DC
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x000223E4 File Offset: 0x000205E4
		internal NativeCommandIOFormat InputFormat { get; private set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x000223ED File Offset: 0x000205ED
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x000223F5 File Offset: 0x000205F5
		internal NativeCommandIOFormat OutputFormat { get; private set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x000223FE File Offset: 0x000205FE
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x00022406 File Offset: 0x00020606
		internal bool NonInteractive { get; private set; }

		// Token: 0x06000727 RID: 1831 RVA: 0x00022410 File Offset: 0x00020610
		internal Collection<CommandParameterInternal> BindParameters(Collection<CommandParameterInternal> parameters, bool outputRedirected, string hostName)
		{
			MinishellParameterBinderController.MinishellParameters minishellParameters = (MinishellParameterBinderController.MinishellParameters)0;
			string text = null;
			string text2 = null;
			for (int i = 0; i < parameters.Count; i++)
			{
				CommandParameterInternal commandParameterInternal = parameters[i];
				if (commandParameterInternal.ParameterNameSpecified)
				{
					string parameterName = commandParameterInternal.ParameterName;
					if ("command".StartsWith(parameterName, StringComparison.OrdinalIgnoreCase))
					{
						this.HandleSeenParameter(ref minishellParameters, MinishellParameterBinderController.MinishellParameters.Command, "command");
						if (i + 1 >= parameters.Count)
						{
							throw this.NewParameterBindingException(null, ErrorCategory.InvalidArgument, "command", typeof(ScriptBlock), null, NativeCP.NoValueForCommandParameter, "NoValueForCommandParameter", new object[0]);
						}
						i++;
						CommandParameterInternal commandParameterInternal2 = parameters[i];
						object obj = PSObject.Base(commandParameterInternal2.ArgumentValue);
						if (!commandParameterInternal2.ArgumentSpecified || !(obj is ScriptBlock))
						{
							throw this.NewParameterBindingException(null, ErrorCategory.InvalidArgument, "command", typeof(ScriptBlock), obj.GetType(), NativeCP.IncorrectValueForCommandParameter, "IncorrectValueForCommandParameter", new object[0]);
						}
						parameters[i - 1] = CommandParameterInternal.CreateParameter(commandParameterInternal.ParameterExtent, "encodedCommand", "-encodedCommand");
						string value = StringToBase64Converter.StringToBase64String(obj.ToString());
						parameters[i] = CommandParameterInternal.CreateArgument(commandParameterInternal2.ArgumentExtent, value, false, false);
					}
					else if ("inputFormat".StartsWith(parameterName, StringComparison.OrdinalIgnoreCase))
					{
						this.HandleSeenParameter(ref minishellParameters, MinishellParameterBinderController.MinishellParameters.InputFormat, "inputFormat");
						if (i + 1 >= parameters.Count)
						{
							throw this.NewParameterBindingException(null, ErrorCategory.InvalidArgument, "inputFormat", typeof(string), null, NativeCP.NoValueForInputFormatParameter, "NoValueForInputFormatParameter", new object[0]);
						}
						i++;
						CommandParameterInternal commandParameterInternal3 = parameters[i];
						text = this.ProcessFormatParameterValue("inputFormat", commandParameterInternal3.ArgumentValue);
						parameters[i - 1] = CommandParameterInternal.CreateParameter(commandParameterInternal.ParameterExtent, "inputFormat", "-inputFormat");
						parameters[i] = CommandParameterInternal.CreateArgument(commandParameterInternal3.ArgumentExtent, text, false, false);
					}
					else if ("outputFormat".StartsWith(parameterName, StringComparison.OrdinalIgnoreCase))
					{
						this.HandleSeenParameter(ref minishellParameters, MinishellParameterBinderController.MinishellParameters.OutputFormat, "outputFormat");
						if (i + 1 >= parameters.Count)
						{
							throw this.NewParameterBindingException(null, ErrorCategory.InvalidArgument, "outputFormat", typeof(string), null, NativeCP.NoValueForOutputFormatParameter, "NoValueForInputFormatParameter", new object[0]);
						}
						i++;
						CommandParameterInternal commandParameterInternal4 = parameters[i];
						text2 = this.ProcessFormatParameterValue("outputFormat", commandParameterInternal4.ArgumentValue);
						parameters[i - 1] = CommandParameterInternal.CreateParameter(commandParameterInternal.ParameterExtent, "outputFormat", "-outputFormat");
						parameters[i] = CommandParameterInternal.CreateArgument(commandParameterInternal4.ArgumentExtent, text2, false, false);
					}
					else if ("args".StartsWith(parameterName, StringComparison.OrdinalIgnoreCase))
					{
						this.HandleSeenParameter(ref minishellParameters, MinishellParameterBinderController.MinishellParameters.Arguments, "args");
						if (i + 1 >= parameters.Count)
						{
							throw this.NewParameterBindingException(null, ErrorCategory.InvalidArgument, "args", typeof(string), null, NativeCP.NoValuesSpecifiedForArgs, "NoValuesSpecifiedForArgs", new object[0]);
						}
						i++;
						CommandParameterInternal commandParameterInternal5 = parameters[i];
						string value2 = MinishellParameterBinderController.ConvertArgsValueToEncodedString(commandParameterInternal5.ArgumentValue);
						parameters[i - 1] = CommandParameterInternal.CreateParameter(commandParameterInternal.ParameterExtent, "encodedarguments", "-encodedarguments");
						parameters[i] = CommandParameterInternal.CreateArgument(commandParameterInternal5.ArgumentExtent, value2, false, false);
					}
				}
				else
				{
					CommandParameterInternal commandParameterInternal6 = parameters[i];
					object obj2 = PSObject.Base(commandParameterInternal6.ArgumentValue);
					if (obj2 is ScriptBlock)
					{
						this.HandleSeenParameter(ref minishellParameters, MinishellParameterBinderController.MinishellParameters.Command, "command");
						string value3 = StringToBase64Converter.StringToBase64String(obj2.ToString());
						parameters[i] = CommandParameterInternal.CreateParameterWithArgument(commandParameterInternal.ArgumentExtent, "encodedCommand", "-encodedCommand", commandParameterInternal.ArgumentExtent, value3, true, false);
					}
				}
			}
			if (text == null)
			{
				parameters.Add(CommandParameterInternal.CreateParameter(PositionUtilities.EmptyExtent, "inputFormat", "-inputFormat"));
				parameters.Add(CommandParameterInternal.CreateArgument(PositionUtilities.EmptyExtent, "xml", false, false));
				text = "xml";
			}
			if (text2 == null)
			{
				text2 = (outputRedirected ? "xml" : "text");
				parameters.Add(CommandParameterInternal.CreateParameter(PositionUtilities.EmptyExtent, "outputFormat", "-outputFormat"));
				parameters.Add(CommandParameterInternal.CreateArgument(PositionUtilities.EmptyExtent, text2, false, false));
			}
			this.InputFormat = ("xml".StartsWith(text, StringComparison.OrdinalIgnoreCase) ? NativeCommandIOFormat.Xml : NativeCommandIOFormat.Text);
			this.OutputFormat = ("xml".StartsWith(text2, StringComparison.OrdinalIgnoreCase) ? NativeCommandIOFormat.Xml : NativeCommandIOFormat.Text);
			if (string.IsNullOrEmpty(hostName) || !hostName.Equals("ConsoleHost", StringComparison.OrdinalIgnoreCase))
			{
				this.NonInteractive = true;
				parameters.Insert(0, CommandParameterInternal.CreateParameter(PositionUtilities.EmptyExtent, "noninteractive", "-noninteractive"));
			}
			((NativeCommandParameterBinder)base.DefaultParameterBinder).BindParameters(parameters);
			return MinishellParameterBinderController.emptyReturnCollection;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000228B0 File Offset: 0x00020AB0
		private void HandleSeenParameter(ref MinishellParameterBinderController.MinishellParameters seen, MinishellParameterBinderController.MinishellParameters parameter, string parameterName)
		{
			if ((seen & parameter) == parameter)
			{
				throw this.NewParameterBindingException(null, ErrorCategory.InvalidArgument, "-" + parameterName, null, null, NativeCP.ParameterSpecifiedAlready, "ParameterSpecifiedAlready", new object[]
				{
					parameterName
				});
			}
			seen |= parameter;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000228F8 File Offset: 0x00020AF8
		private string ProcessFormatParameterValue(string parameterName, object value)
		{
			string text;
			try
			{
				text = (string)LanguagePrimitives.ConvertTo(value, typeof(string), CultureInfo.InvariantCulture);
			}
			catch (PSInvalidCastException innerException)
			{
				throw this.NewParameterBindingException(innerException, ErrorCategory.InvalidArgument, parameterName, typeof(string), value.GetType(), NativeCP.StringValueExpectedForFormatParameter, "StringValueExpectedForFormatParameter", new object[]
				{
					parameterName
				});
			}
			if ("xml".StartsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				return "xml";
			}
			if ("text".StartsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				return "text";
			}
			throw this.NewParameterBindingException(null, ErrorCategory.InvalidArgument, parameterName, typeof(string), value.GetType(), NativeCP.IncorrectValueForFormatParameter, "IncorrectValueForFormatParameter", new object[]
			{
				text,
				parameterName
			});
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x000229C0 File Offset: 0x00020BC0
		private static string ConvertArgsValueToEncodedString(object value)
		{
			ArrayList source = MinishellParameterBinderController.ConvertArgsValueToArrayList(value);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlWriter xmlWriter = XmlWriter.Create(stringWriter);
			Serializer serializer = new Serializer(xmlWriter);
			serializer.Serialize(source);
			serializer.Done();
			xmlWriter.Flush();
			string input = stringWriter.ToString();
			return StringToBase64Converter.StringToBase64String(input);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00022A10 File Offset: 0x00020C10
		private static ArrayList ConvertArgsValueToArrayList(object value)
		{
			ArrayList arrayList = new ArrayList();
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(value);
			if (enumerator == null)
			{
				arrayList.Add(value);
			}
			else
			{
				while (enumerator.MoveNext())
				{
					object value2 = enumerator.Current;
					arrayList.Add(value2);
				}
			}
			return arrayList;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00022A50 File Offset: 0x00020C50
		private ParameterBindingException NewParameterBindingException(Exception innerException, ErrorCategory errorCategory, string parameterName, Type parameterType, Type typeSpecified, string resourceString, string errorId, params object[] args)
		{
			return new ParameterBindingException(innerException, errorCategory, base.InvocationInfo, null, parameterName, parameterType, typeSpecified, resourceString, errorId, args);
		}

		// Token: 0x04000301 RID: 769
		internal const string CommandParameter = "command";

		// Token: 0x04000302 RID: 770
		internal const string EncodedCommandParameter = "encodedCommand";

		// Token: 0x04000303 RID: 771
		internal const string ArgsParameter = "args";

		// Token: 0x04000304 RID: 772
		internal const string EncodedArgsParameter = "encodedarguments";

		// Token: 0x04000305 RID: 773
		internal const string InputFormatParameter = "inputFormat";

		// Token: 0x04000306 RID: 774
		internal const string OutputFormatParameter = "outputFormat";

		// Token: 0x04000307 RID: 775
		internal const string XmlFormatValue = "xml";

		// Token: 0x04000308 RID: 776
		internal const string TextFormatValue = "text";

		// Token: 0x04000309 RID: 777
		internal const string NonInteractiveParameter = "noninteractive";

		// Token: 0x0400030A RID: 778
		private static readonly Collection<CommandParameterInternal> emptyReturnCollection = new Collection<CommandParameterInternal>();

		// Token: 0x0200008D RID: 141
		[Flags]
		private enum MinishellParameters
		{
			// Token: 0x0400030F RID: 783
			Command = 1,
			// Token: 0x04000310 RID: 784
			Arguments = 2,
			// Token: 0x04000311 RID: 785
			InputFormat = 4,
			// Token: 0x04000312 RID: 786
			OutputFormat = 8
		}
	}
}
