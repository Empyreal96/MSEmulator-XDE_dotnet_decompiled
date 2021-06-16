using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008D7 RID: 2263
	internal class GraphicalHostReflectionWrapper
	{
		// Token: 0x06005574 RID: 21876 RVA: 0x001C206D File Offset: 0x001C026D
		private GraphicalHostReflectionWrapper()
		{
		}

		// Token: 0x06005575 RID: 21877 RVA: 0x001C2075 File Offset: 0x001C0275
		internal static GraphicalHostReflectionWrapper GetGraphicalHostReflectionWrapper(PSCmdlet parentCmdlet, string graphicalHostHelperTypeName)
		{
			return GraphicalHostReflectionWrapper.GetGraphicalHostReflectionWrapper(parentCmdlet, graphicalHostHelperTypeName, parentCmdlet.CommandInfo.Name);
		}

		// Token: 0x06005576 RID: 21878 RVA: 0x001C2098 File Offset: 0x001C0298
		internal static GraphicalHostReflectionWrapper GetGraphicalHostReflectionWrapper(PSCmdlet parentCmdlet, string graphicalHostHelperTypeName, string featureName)
		{
			GraphicalHostReflectionWrapper graphicalHostReflectionWrapper = new GraphicalHostReflectionWrapper();
			if (GraphicalHostReflectionWrapper.IsInputFromRemoting(parentCmdlet))
			{
				ErrorRecord errorRecord = new ErrorRecord(new NotSupportedException(StringUtil.Format(HelpErrors.RemotingNotSupportedForFeature, featureName)), "RemotingNotSupported", ErrorCategory.InvalidOperation, parentCmdlet);
				parentCmdlet.ThrowTerminatingError(errorRecord);
			}
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = "Microsoft.PowerShell.GraphicalHost";
			assemblyName.Version = new Version(3, 0, 0, 0);
			assemblyName.CultureInfo = new CultureInfo(string.Empty);
			assemblyName.SetPublicKeyToken(new byte[]
			{
				49,
				191,
				56,
				86,
				173,
				54,
				78,
				53
			});
			try
			{
				graphicalHostReflectionWrapper.graphicalHostAssembly = Assembly.Load(assemblyName);
			}
			catch (FileNotFoundException ex)
			{
				string message = StringUtil.Format(HelpErrors.GraphicalHostAssemblyIsNotFound, featureName, ex.Message);
				parentCmdlet.ThrowTerminatingError(new ErrorRecord(new NotSupportedException(message, ex), "ErrorLoadingAssembly", ErrorCategory.ObjectNotFound, assemblyName));
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				parentCmdlet.ThrowTerminatingError(new ErrorRecord(ex2, "ErrorLoadingAssembly", ErrorCategory.ObjectNotFound, assemblyName));
			}
			graphicalHostReflectionWrapper.graphicalHostHelperType = graphicalHostReflectionWrapper.graphicalHostAssembly.GetType(graphicalHostHelperTypeName);
			ConstructorInfo constructor = graphicalHostReflectionWrapper.graphicalHostHelperType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
			if (constructor != null)
			{
				graphicalHostReflectionWrapper.graphicalHostHelperObject = constructor.Invoke(new object[0]);
			}
			return graphicalHostReflectionWrapper;
		}

		// Token: 0x06005577 RID: 21879 RVA: 0x001C21E0 File Offset: 0x001C03E0
		internal static string EscapeBinding(string propertyName)
		{
			return propertyName.Replace("/", " ").Replace(".", " ");
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x001C2204 File Offset: 0x001C0404
		internal object CallMethod(string methodName, params object[] arguments)
		{
			MethodInfo method = this.graphicalHostHelperType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
			return method.Invoke(this.graphicalHostHelperObject, arguments);
		}

		// Token: 0x06005579 RID: 21881 RVA: 0x001C2230 File Offset: 0x001C0430
		internal object CallStaticMethod(string methodName, params object[] arguments)
		{
			MethodInfo method = this.graphicalHostHelperType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
			return method.Invoke(null, arguments);
		}

		// Token: 0x0600557A RID: 21882 RVA: 0x001C2254 File Offset: 0x001C0454
		internal object GetPropertyValue(string propertyName)
		{
			PropertyInfo property = this.graphicalHostHelperType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
			return property.GetValue(this.graphicalHostHelperObject, new object[0]);
		}

		// Token: 0x0600557B RID: 21883 RVA: 0x001C2284 File Offset: 0x001C0484
		internal object GetStaticPropertyValue(string propertyName)
		{
			PropertyInfo property = this.graphicalHostHelperType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.NonPublic);
			return property.GetValue(null, new object[0]);
		}

		// Token: 0x0600557C RID: 21884 RVA: 0x001C22B0 File Offset: 0x001C04B0
		private static bool IsInputFromRemoting(PSCmdlet parentCmdlet)
		{
			PSVariable psvariable = parentCmdlet.SessionState.PSVariable.Get("PSSenderInfo");
			return psvariable != null;
		}

		// Token: 0x04002CC4 RID: 11460
		private Assembly graphicalHostAssembly;

		// Token: 0x04002CC5 RID: 11461
		private Type graphicalHostHelperType;

		// Token: 0x04002CC6 RID: 11462
		private object graphicalHostHelperObject;
	}
}
