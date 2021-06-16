using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000496 RID: 1174
	[Cmdlet("Set", "StrictMode", DefaultParameterSetName = "Version", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113450")]
	public class SetStrictModeCommand : PSCmdlet
	{
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060034BE RID: 13502 RVA: 0x0011EB26 File Offset: 0x0011CD26
		// (set) Token: 0x060034BF RID: 13503 RVA: 0x0011EB2E File Offset: 0x0011CD2E
		[Parameter(ParameterSetName = "Off", Mandatory = true)]
		public SwitchParameter Off
		{
			get
			{
				return this.off;
			}
			set
			{
				this.off = value;
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060034C0 RID: 13504 RVA: 0x0011EB37 File Offset: 0x0011CD37
		// (set) Token: 0x060034C1 RID: 13505 RVA: 0x0011EB3F File Offset: 0x0011CD3F
		[SetStrictModeCommand.ArgumentToVersionTransformationAttribute]
		[SetStrictModeCommand.ValidateVersionAttribute]
		[Alias(new string[]
		{
			"v"
		})]
		[Parameter(ParameterSetName = "Version", Mandatory = true)]
		public Version Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x0011EB48 File Offset: 0x0011CD48
		protected override void EndProcessing()
		{
			if (this.off.IsPresent)
			{
				this.version = new Version(0, 0);
			}
			base.Context.EngineSessionState.CurrentScope.StrictModeVersion = this.version;
		}

		// Token: 0x04001AF8 RID: 6904
		private SwitchParameter off;

		// Token: 0x04001AF9 RID: 6905
		private Version version;

		// Token: 0x02000497 RID: 1175
		private sealed class ArgumentToVersionTransformationAttribute : ArgumentTransformationAttribute
		{
			// Token: 0x060034C4 RID: 13508 RVA: 0x0011EB88 File Offset: 0x0011CD88
			public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
			{
				object obj = PSObject.Base(inputData);
				string text = obj as string;
				if (text != null)
				{
					if (text.Equals("latest", StringComparison.OrdinalIgnoreCase))
					{
						return PSVersionInfo.PSVersion;
					}
					if (text.Contains("."))
					{
						return inputData;
					}
				}
				if (obj is double)
				{
					return inputData;
				}
				int major;
				if (LanguagePrimitives.TryConvertTo<int>(obj, out major))
				{
					return new Version(major, 0);
				}
				return inputData;
			}
		}

		// Token: 0x02000498 RID: 1176
		private sealed class ValidateVersionAttribute : ValidateArgumentsAttribute
		{
			// Token: 0x060034C6 RID: 13510 RVA: 0x0011EBF0 File Offset: 0x0011CDF0
			protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
			{
				Version version = arguments as Version;
				if (version == null || !PSVersionInfo.IsValidPSVersion(version))
				{
					throw new ValidationMetadataException("InvalidPSVersion", null, Metadata.ValidateVersionFailure, new object[]
					{
						arguments
					});
				}
			}
		}
	}
}
