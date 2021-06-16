using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Language
{
	// Token: 0x02000604 RID: 1540
	internal class BinderUtils
	{
		// Token: 0x06004336 RID: 17206 RVA: 0x001618F1 File Offset: 0x0015FAF1
		internal static BindingRestrictions GetVersionCheck(DynamicMetaObjectBinder binder, int expectedVersionNumber)
		{
			return BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Field(Expression.Constant(binder), "_version"), ExpressionCache.Constant(expectedVersionNumber)));
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x00161914 File Offset: 0x0015FB14
		internal static BindingRestrictions GetLanguageModeCheckIfHasEverUsedConstrainedLanguage()
		{
			BindingRestrictions result = BindingRestrictions.Empty;
			if (ExecutionContext.HasEverUsedConstrainedLanguage)
			{
				ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
				PSLanguageMode languageMode = executionContextFromTLS.LanguageMode;
				if (languageMode == PSLanguageMode.ConstrainedLanguage)
				{
					result = BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Property(ExpressionCache.GetExecutionContextFromTLS, CachedReflectionInfo.ExecutionContext_LanguageMode), Expression.Constant(PSLanguageMode.ConstrainedLanguage)));
				}
				else
				{
					result = BindingRestrictions.GetExpressionRestriction(Expression.NotEqual(Expression.Property(ExpressionCache.GetExecutionContextFromTLS, CachedReflectionInfo.ExecutionContext_LanguageMode), Expression.Constant(PSLanguageMode.ConstrainedLanguage)));
				}
			}
			return result;
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x0016198C File Offset: 0x0015FB8C
		internal static BindingRestrictions GetOptionalVersionAndLanguageCheckForType(DynamicMetaObjectBinder binder, Type targetType, int expectedVersionNumber)
		{
			BindingRestrictions bindingRestrictions = BindingRestrictions.Empty;
			if (!CoreTypes.Contains(targetType))
			{
				if (expectedVersionNumber != -1)
				{
					bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetVersionCheck(binder, expectedVersionNumber));
				}
				bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetLanguageModeCheckIfHasEverUsedConstrainedLanguage());
			}
			return bindingRestrictions;
		}
	}
}
