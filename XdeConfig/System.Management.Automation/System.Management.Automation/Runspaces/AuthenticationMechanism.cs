using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020002C9 RID: 713
	public enum AuthenticationMechanism
	{
		// Token: 0x04001010 RID: 4112
		Default,
		// Token: 0x04001011 RID: 4113
		Basic,
		// Token: 0x04001012 RID: 4114
		Negotiate,
		// Token: 0x04001013 RID: 4115
		NegotiateWithImplicitCredential,
		// Token: 0x04001014 RID: 4116
		Credssp,
		// Token: 0x04001015 RID: 4117
		Digest,
		// Token: 0x04001016 RID: 4118
		Kerberos
	}
}
