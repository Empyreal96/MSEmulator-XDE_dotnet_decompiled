using System;

namespace System.Management.Automation
{
	// Token: 0x02000800 RID: 2048
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class CredentialAttribute : ArgumentTransformationAttribute
	{
		// Token: 0x06004F3F RID: 20287 RVA: 0x001A4538 File Offset: 0x001A2738
		public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
		{
			PSCredential pscredential = null;
			string text = null;
			bool flag = false;
			if (engineIntrinsics == null || engineIntrinsics.Host == null || engineIntrinsics.Host.UI == null)
			{
				throw PSTraceSource.NewArgumentNullException("engineIntrinsics");
			}
			if (inputData == null)
			{
				flag = true;
			}
			else
			{
				pscredential = LanguagePrimitives.FromObjectAs<PSCredential>(inputData);
				if (pscredential == null)
				{
					flag = true;
					text = LanguagePrimitives.FromObjectAs<string>(inputData);
					if (text == null)
					{
						throw new PSArgumentException("userName");
					}
				}
			}
			if (flag)
			{
				string credentialAttribute_Prompt_Caption = CredentialAttributeStrings.CredentialAttribute_Prompt_Caption;
				string credentialAttribute_Prompt = CredentialAttributeStrings.CredentialAttribute_Prompt;
				pscredential = engineIntrinsics.Host.UI.PromptForCredential(credentialAttribute_Prompt_Caption, credentialAttribute_Prompt, text, "");
			}
			return pscredential;
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06004F40 RID: 20288 RVA: 0x001A45C6 File Offset: 0x001A27C6
		public override bool TransformNullOptionalParameters
		{
			get
			{
				return false;
			}
		}
	}
}
