using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000261 RID: 609
	internal enum RemoteHostMethodId
	{
		// Token: 0x04000C65 RID: 3173
		GetName = 1,
		// Token: 0x04000C66 RID: 3174
		GetVersion,
		// Token: 0x04000C67 RID: 3175
		GetInstanceId,
		// Token: 0x04000C68 RID: 3176
		GetCurrentCulture,
		// Token: 0x04000C69 RID: 3177
		GetCurrentUICulture,
		// Token: 0x04000C6A RID: 3178
		SetShouldExit,
		// Token: 0x04000C6B RID: 3179
		EnterNestedPrompt,
		// Token: 0x04000C6C RID: 3180
		ExitNestedPrompt,
		// Token: 0x04000C6D RID: 3181
		NotifyBeginApplication,
		// Token: 0x04000C6E RID: 3182
		NotifyEndApplication,
		// Token: 0x04000C6F RID: 3183
		ReadLine,
		// Token: 0x04000C70 RID: 3184
		ReadLineAsSecureString,
		// Token: 0x04000C71 RID: 3185
		Write1,
		// Token: 0x04000C72 RID: 3186
		Write2,
		// Token: 0x04000C73 RID: 3187
		WriteLine1,
		// Token: 0x04000C74 RID: 3188
		WriteLine2,
		// Token: 0x04000C75 RID: 3189
		WriteLine3,
		// Token: 0x04000C76 RID: 3190
		WriteErrorLine,
		// Token: 0x04000C77 RID: 3191
		WriteDebugLine,
		// Token: 0x04000C78 RID: 3192
		WriteProgress,
		// Token: 0x04000C79 RID: 3193
		WriteVerboseLine,
		// Token: 0x04000C7A RID: 3194
		WriteWarningLine,
		// Token: 0x04000C7B RID: 3195
		Prompt,
		// Token: 0x04000C7C RID: 3196
		PromptForCredential1,
		// Token: 0x04000C7D RID: 3197
		PromptForCredential2,
		// Token: 0x04000C7E RID: 3198
		PromptForChoice,
		// Token: 0x04000C7F RID: 3199
		GetForegroundColor,
		// Token: 0x04000C80 RID: 3200
		SetForegroundColor,
		// Token: 0x04000C81 RID: 3201
		GetBackgroundColor,
		// Token: 0x04000C82 RID: 3202
		SetBackgroundColor,
		// Token: 0x04000C83 RID: 3203
		GetCursorPosition,
		// Token: 0x04000C84 RID: 3204
		SetCursorPosition,
		// Token: 0x04000C85 RID: 3205
		GetWindowPosition,
		// Token: 0x04000C86 RID: 3206
		SetWindowPosition,
		// Token: 0x04000C87 RID: 3207
		GetCursorSize,
		// Token: 0x04000C88 RID: 3208
		SetCursorSize,
		// Token: 0x04000C89 RID: 3209
		GetBufferSize,
		// Token: 0x04000C8A RID: 3210
		SetBufferSize,
		// Token: 0x04000C8B RID: 3211
		GetWindowSize,
		// Token: 0x04000C8C RID: 3212
		SetWindowSize,
		// Token: 0x04000C8D RID: 3213
		GetWindowTitle,
		// Token: 0x04000C8E RID: 3214
		SetWindowTitle,
		// Token: 0x04000C8F RID: 3215
		GetMaxWindowSize,
		// Token: 0x04000C90 RID: 3216
		GetMaxPhysicalWindowSize,
		// Token: 0x04000C91 RID: 3217
		GetKeyAvailable,
		// Token: 0x04000C92 RID: 3218
		ReadKey,
		// Token: 0x04000C93 RID: 3219
		FlushInputBuffer,
		// Token: 0x04000C94 RID: 3220
		SetBufferContents1,
		// Token: 0x04000C95 RID: 3221
		SetBufferContents2,
		// Token: 0x04000C96 RID: 3222
		GetBufferContents,
		// Token: 0x04000C97 RID: 3223
		ScrollBufferContents,
		// Token: 0x04000C98 RID: 3224
		PushRunspace,
		// Token: 0x04000C99 RID: 3225
		PopRunspace,
		// Token: 0x04000C9A RID: 3226
		GetIsRunspacePushed,
		// Token: 0x04000C9B RID: 3227
		GetRunspace,
		// Token: 0x04000C9C RID: 3228
		PromptForChoiceMultipleSelection
	}
}
