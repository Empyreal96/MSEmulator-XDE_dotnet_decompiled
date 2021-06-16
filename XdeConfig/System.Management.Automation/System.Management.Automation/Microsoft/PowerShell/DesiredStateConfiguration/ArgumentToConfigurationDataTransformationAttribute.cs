using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.DesiredStateConfiguration
{
	// Token: 0x020009FB RID: 2555
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class ArgumentToConfigurationDataTransformationAttribute : ArgumentTransformationAttribute
	{
		// Token: 0x06005D9A RID: 23962 RVA: 0x001FEF88 File Offset: 0x001FD188
		public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
		{
			string text = inputData as string;
			if (string.IsNullOrEmpty(text))
			{
				return inputData;
			}
			if (engineIntrinsics == null)
			{
				throw PSTraceSource.NewArgumentNullException("engineIntrinsics");
			}
			return PsUtils.EvaluatePowerShellDataFileAsModuleManifest("ConfigurationData", text, engineIntrinsics.SessionState.Internal.ExecutionContext, false);
		}
	}
}
