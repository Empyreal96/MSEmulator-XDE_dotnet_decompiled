using System;
using System.Reflection;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A83 RID: 2691
	internal static class Error
	{
		// Token: 0x06006AEB RID: 27371 RVA: 0x0021890F File Offset: 0x00216B0F
		internal static Exception COMObjectDoesNotSupportEvents()
		{
			return new ArgumentException(ParserStrings.COMObjectDoesNotSupportEvents);
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x0021891B File Offset: 0x00216B1B
		internal static Exception COMObjectDoesNotSupportSourceInterface()
		{
			return new ArgumentException(ParserStrings.COMObjectDoesNotSupportSourceInterface);
		}

		// Token: 0x06006AED RID: 27373 RVA: 0x00218927 File Offset: 0x00216B27
		internal static Exception SetComObjectDataFailed()
		{
			return new InvalidOperationException(ParserStrings.SetComObjectDataFailed);
		}

		// Token: 0x06006AEE RID: 27374 RVA: 0x00218933 File Offset: 0x00216B33
		internal static Exception MethodShouldNotBeCalled()
		{
			return new InvalidOperationException(ParserStrings.MethodShouldNotBeCalled);
		}

		// Token: 0x06006AEF RID: 27375 RVA: 0x0021893F File Offset: 0x00216B3F
		internal static Exception UnexpectedVarEnum(object p0)
		{
			return new InvalidOperationException(Strings.UnexpectedVarEnum(p0));
		}

		// Token: 0x06006AF0 RID: 27376 RVA: 0x0021894C File Offset: 0x00216B4C
		internal static Exception DispBadParamCount(object p0, int parameterCount)
		{
			return new TargetParameterCountException(Strings.DispBadParamCount(p0, parameterCount));
		}

		// Token: 0x06006AF1 RID: 27377 RVA: 0x0021895A File Offset: 0x00216B5A
		internal static Exception DispMemberNotFound(object p0)
		{
			return new MissingMemberException(Strings.DispMemberNotFound(p0));
		}

		// Token: 0x06006AF2 RID: 27378 RVA: 0x00218967 File Offset: 0x00216B67
		internal static Exception DispNoNamedArgs(object p0)
		{
			return new ArgumentException(Strings.DispNoNamedArgs(p0));
		}

		// Token: 0x06006AF3 RID: 27379 RVA: 0x00218974 File Offset: 0x00216B74
		internal static Exception DispOverflow(object p0)
		{
			return new OverflowException(Strings.DispOverflow(p0));
		}

		// Token: 0x06006AF4 RID: 27380 RVA: 0x00218981 File Offset: 0x00216B81
		internal static Exception DispTypeMismatch(object method, string value, string originalTypeName, string destinationTypeName)
		{
			return new ArgumentException(Strings.DispTypeMismatch(method, value, originalTypeName, destinationTypeName));
		}

		// Token: 0x06006AF5 RID: 27381 RVA: 0x00218991 File Offset: 0x00216B91
		internal static Exception DispParamNotOptional(object p0)
		{
			return new ArgumentException(Strings.DispParamNotOptional(p0));
		}

		// Token: 0x06006AF6 RID: 27382 RVA: 0x0021899E File Offset: 0x00216B9E
		internal static Exception CannotRetrieveTypeInformation()
		{
			return new InvalidOperationException(ParserStrings.CannotRetrieveTypeInformation);
		}

		// Token: 0x06006AF7 RID: 27383 RVA: 0x002189AA File Offset: 0x00216BAA
		internal static Exception GetIDsOfNamesInvalid(object p0)
		{
			return new ArgumentException(Strings.GetIDsOfNamesInvalid(p0));
		}

		// Token: 0x06006AF8 RID: 27384 RVA: 0x002189B7 File Offset: 0x00216BB7
		internal static Exception UnsupportedEnumType()
		{
			return new InvalidOperationException(ParserStrings.UnsupportedEnumType);
		}

		// Token: 0x06006AF9 RID: 27385 RVA: 0x002189C3 File Offset: 0x00216BC3
		internal static Exception UnsupportedHandlerType()
		{
			return new InvalidOperationException(ParserStrings.UnsupportedHandlerType);
		}

		// Token: 0x06006AFA RID: 27386 RVA: 0x002189CF File Offset: 0x00216BCF
		internal static Exception CouldNotGetDispId(object p0, object p1)
		{
			return new MissingMemberException(Strings.CouldNotGetDispId(p0, p1));
		}

		// Token: 0x06006AFB RID: 27387 RVA: 0x002189DD File Offset: 0x00216BDD
		internal static Exception AmbiguousConversion(object p0, object p1)
		{
			return new AmbiguousMatchException(Strings.AmbiguousConversion(p0, p1));
		}

		// Token: 0x06006AFC RID: 27388 RVA: 0x002189EB File Offset: 0x00216BEB
		internal static Exception VariantGetAccessorNYI(object p0)
		{
			return new NotImplementedException(Strings.VariantGetAccessorNYI(p0));
		}
	}
}
