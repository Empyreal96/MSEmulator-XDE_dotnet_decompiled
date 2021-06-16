using System;

namespace CommandLine
{
	// Token: 0x0200003F RID: 63
	public sealed class InvalidAttributeConfigurationError : Error
	{
		// Token: 0x0600013F RID: 319 RVA: 0x000053C2 File Offset: 0x000035C2
		internal InvalidAttributeConfigurationError() : base(ErrorType.InvalidAttributeConfigurationError, true)
		{
		}

		// Token: 0x04000065 RID: 101
		public const string ErrorMessage = "Check if Option or Value attribute values are set properly for the given type.";
	}
}
