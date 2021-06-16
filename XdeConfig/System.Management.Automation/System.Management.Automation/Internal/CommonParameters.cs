using System;
using System.Collections;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000015 RID: 21
	public sealed class CommonParameters
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00005A50 File Offset: 0x00003C50
		internal CommonParameters(MshCommandRuntime commandRuntime)
		{
			if (commandRuntime == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandRuntime");
			}
			this.commandRuntime = commandRuntime;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00005A6D File Offset: 0x00003C6D
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00005A7F File Offset: 0x00003C7F
		[Parameter]
		[Alias(new string[]
		{
			"vb"
		})]
		public SwitchParameter Verbose
		{
			get
			{
				return this.commandRuntime.Verbose;
			}
			set
			{
				this.commandRuntime.Verbose = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00005A92 File Offset: 0x00003C92
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00005AA4 File Offset: 0x00003CA4
		[Alias(new string[]
		{
			"db"
		})]
		[Parameter]
		public SwitchParameter Debug
		{
			get
			{
				return this.commandRuntime.Debug;
			}
			set
			{
				this.commandRuntime.Debug = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005AB7 File Offset: 0x00003CB7
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00005AC4 File Offset: 0x00003CC4
		[Alias(new string[]
		{
			"ea"
		})]
		[Parameter]
		public ActionPreference ErrorAction
		{
			get
			{
				return this.commandRuntime.ErrorAction;
			}
			set
			{
				this.commandRuntime.ErrorAction = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005AD2 File Offset: 0x00003CD2
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00005ADF File Offset: 0x00003CDF
		[Parameter]
		[Alias(new string[]
		{
			"wa"
		})]
		public ActionPreference WarningAction
		{
			get
			{
				return this.commandRuntime.WarningPreference;
			}
			set
			{
				this.commandRuntime.WarningPreference = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005AED File Offset: 0x00003CED
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00005AFA File Offset: 0x00003CFA
		[Parameter]
		[Alias(new string[]
		{
			"infa"
		})]
		public ActionPreference InformationAction
		{
			get
			{
				return this.commandRuntime.InformationPreference;
			}
			set
			{
				this.commandRuntime.InformationPreference = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005B08 File Offset: 0x00003D08
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00005B15 File Offset: 0x00003D15
		[Parameter]
		[Alias(new string[]
		{
			"ev"
		})]
		[CommonParameters.ValidateVariableName]
		public string ErrorVariable
		{
			get
			{
				return this.commandRuntime.ErrorVariable;
			}
			set
			{
				this.commandRuntime.ErrorVariable = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005B23 File Offset: 0x00003D23
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00005B30 File Offset: 0x00003D30
		[Parameter]
		[CommonParameters.ValidateVariableName]
		[Alias(new string[]
		{
			"wv"
		})]
		public string WarningVariable
		{
			get
			{
				return this.commandRuntime.WarningVariable;
			}
			set
			{
				this.commandRuntime.WarningVariable = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00005B3E File Offset: 0x00003D3E
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00005B4B File Offset: 0x00003D4B
		[Parameter]
		[Alias(new string[]
		{
			"iv"
		})]
		[CommonParameters.ValidateVariableName]
		public string InformationVariable
		{
			get
			{
				return this.commandRuntime.InformationVariable;
			}
			set
			{
				this.commandRuntime.InformationVariable = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00005B59 File Offset: 0x00003D59
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00005B66 File Offset: 0x00003D66
		[Alias(new string[]
		{
			"ov"
		})]
		[Parameter]
		[CommonParameters.ValidateVariableName]
		public string OutVariable
		{
			get
			{
				return this.commandRuntime.OutVariable;
			}
			set
			{
				this.commandRuntime.OutVariable = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005B74 File Offset: 0x00003D74
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00005B81 File Offset: 0x00003D81
		[Alias(new string[]
		{
			"ob"
		})]
		[Parameter]
		[ValidateRange(0, 2147483647)]
		public int OutBuffer
		{
			get
			{
				return this.commandRuntime.OutBuffer;
			}
			set
			{
				this.commandRuntime.OutBuffer = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005B8F File Offset: 0x00003D8F
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00005B9C File Offset: 0x00003D9C
		[CommonParameters.ValidateVariableName]
		[Alias(new string[]
		{
			"pv"
		})]
		[Parameter]
		public string PipelineVariable
		{
			get
			{
				return this.commandRuntime.PipelineVariable;
			}
			set
			{
				this.commandRuntime.PipelineVariable = value;
			}
		}

		// Token: 0x0400005C RID: 92
		internal static string[] CommonWorkflowParameters = new string[]
		{
			"PSComputerName",
			"JobName",
			"PSApplicationName",
			"PSCredential",
			"PSPort",
			"PSConfigurationName",
			"PSConnectionURI",
			"PSSessionOption",
			"PSAuthentication",
			"PSAuthenticationLevel",
			"PSCertificateThumbprint",
			"PSConnectionRetryCount",
			"PSConnectionRetryIntervalSec",
			"PSRunningTimeoutSec",
			"PSElapsedTimeoutSec",
			"PSPersist",
			"PSPrivateMetadata",
			"InputObject",
			"PSParameterCollection",
			"AsJob",
			"PSUseSSL",
			"PSAllowRedirection"
		};

		// Token: 0x0400005D RID: 93
		internal static Type[] CommonWorkflowParameterTypes = new Type[]
		{
			typeof(string[]),
			typeof(string),
			typeof(string),
			typeof(PSCredential),
			typeof(uint),
			typeof(string),
			typeof(string[]),
			typeof(PSSessionOption),
			typeof(AuthenticationMechanism),
			typeof(AuthenticationLevel),
			typeof(string),
			typeof(uint),
			typeof(uint),
			typeof(int),
			typeof(int),
			typeof(bool),
			typeof(object),
			typeof(object),
			typeof(Hashtable),
			typeof(bool),
			typeof(bool),
			typeof(bool)
		};

		// Token: 0x0400005E RID: 94
		private MshCommandRuntime commandRuntime;

		// Token: 0x02000018 RID: 24
		internal class ValidateVariableName : ValidateArgumentsAttribute
		{
			// Token: 0x06000117 RID: 279 RVA: 0x00005DD8 File Offset: 0x00003FD8
			protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
			{
				string text = arguments as string;
				if (text != null)
				{
					if (text.StartsWith("+", StringComparison.Ordinal))
					{
						text = text.Substring(1);
					}
					VariablePath variablePath = new VariablePath(text);
					if (!variablePath.IsVariable)
					{
						throw new ValidationMetadataException("ArgumentNotValidVariableName", null, Metadata.ValidateVariableName, new object[]
						{
							text
						});
					}
				}
			}
		}
	}
}
