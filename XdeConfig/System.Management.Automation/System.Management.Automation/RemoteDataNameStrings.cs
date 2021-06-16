using System;

namespace System.Management.Automation
{
	// Token: 0x020002D8 RID: 728
	internal static class RemoteDataNameStrings
	{
		// Token: 0x04001098 RID: 4248
		internal const string Destination = "Destination";

		// Token: 0x04001099 RID: 4249
		internal const string RemotingTargetInterface = "RemotingTargetInterface";

		// Token: 0x0400109A RID: 4250
		internal const string ClientRunspacePoolId = "ClientRunspacePoolId";

		// Token: 0x0400109B RID: 4251
		internal const string ClientPowerShellId = "ClientPowerShellId";

		// Token: 0x0400109C RID: 4252
		internal const string Action = "Action";

		// Token: 0x0400109D RID: 4253
		internal const string DataType = "DataType";

		// Token: 0x0400109E RID: 4254
		internal const string TimeZone = "TimeZone";

		// Token: 0x0400109F RID: 4255
		internal const string SenderInfoPreferenceVariable = "PSSenderInfo";

		// Token: 0x040010A0 RID: 4256
		internal const string MustComply = "MustComply";

		// Token: 0x040010A1 RID: 4257
		internal const string IsNegotiationSucceeded = "IsNegotiationSucceeded";

		// Token: 0x040010A2 RID: 4258
		internal const string PSv2TabExpansionFunction = "TabExpansion";

		// Token: 0x040010A3 RID: 4259
		internal const string PSv2TabExpansionFunctionText = "\r\n            param($line, $lastWord)\r\n            & {\r\n                function Write-Members ($sep='.')\r\n                {\r\n                    Invoke-Expression ('$_val=' + $_expression)\r\n\r\n                    $_method = [Management.Automation.PSMemberTypes] `\r\n                        'Method,CodeMethod,ScriptMethod,ParameterizedProperty'\r\n                    if ($sep -eq '.')\r\n                    {\r\n                        $params = @{view = 'extended','adapted','base'}\r\n                    }\r\n                    else\r\n                    {\r\n                        $params = @{static=$true}\r\n                    }\r\n        \r\n                    foreach ($_m in ,$_val | Get-Member @params $_pat |\r\n                        Sort-Object membertype,name)\r\n                    {\r\n                        if ($_m.MemberType -band $_method)\r\n                        {\r\n                            # Return a method...\r\n                            $_base + $_expression + $sep + $_m.name + '('\r\n                        }\r\n                        else {\r\n                            # Return a property...\r\n                            $_base + $_expression + $sep + $_m.name\r\n                        }\r\n                    }\r\n                }\r\n\r\n                # If a command name contains any of these chars, it needs to be quoted\r\n                $_charsRequiringQuotes = ('`&@''#{}()$,;|<> ' + \"`t\").ToCharArray()\r\n\r\n                # If a variable name contains any of these characters it needs to be in braces\r\n                $_varsRequiringQuotes = ('-`&@''#{}()$,;|<> .\\/' + \"`t\").ToCharArray()\r\n\r\n                switch -regex ($lastWord)\r\n                {\r\n                    # Handle property and method expansion rooted at variables...\r\n                    # e.g. $a.b.<tab>\r\n                    '(^.*)(\\$(\\w|:|\\.)+)\\.([*\\w]*)$' {\r\n                        $_base = $matches[1]\r\n                        $_expression = $matches[2]\r\n                        $_pat = $matches[4] + '*'\r\n                        Write-Members\r\n                        break;\r\n                    }\r\n\r\n                    # Handle simple property and method expansion on static members...\r\n                    # e.g. [datetime]::n<tab>\r\n                    '(^.*)(\\[(\\w|\\.|\\+)+\\])(\\:\\:|\\.){0,1}([*\\w]*)$' {\r\n                        $_base = $matches[1]\r\n                        $_expression = $matches[2]\r\n                        $_pat = $matches[5] + '*'\r\n                        Write-Members $(if (! $matches[4]) {'::'} else {$matches[4]})\r\n                        break;\r\n                    }\r\n\r\n                    # Handle complex property and method expansion on static members\r\n                    # where there are intermediate properties...\r\n                    # e.g. [datetime]::now.d<tab>\r\n                    '(^.*)(\\[(\\w|\\.|\\+)+\\](\\:\\:|\\.)(\\w+\\.)+)([*\\w]*)$' {\r\n                        $_base = $matches[1]  # everything before the expression\r\n                        $_expression = $matches[2].TrimEnd('.') # expression less trailing '.'\r\n                        $_pat = $matches[6] + '*'  # the member to look for...\r\n                        Write-Members\r\n                        break;\r\n                    }\r\n\r\n                    # Handle variable name expansion...\r\n                    '(^.*\\$)([*\\w:]+)$' {\r\n                        $_prefix = $matches[1]\r\n                        $_varName = $matches[2]\r\n                        $_colonPos = $_varname.IndexOf(':')\r\n                        if ($_colonPos -eq -1)\r\n                        {\r\n                            $_varName = 'variable:' + $_varName\r\n                            $_provider = ''\r\n                        }\r\n                        else\r\n                        {\r\n                            $_provider = $_varname.Substring(0, $_colonPos+1)\r\n                        }\r\n\r\n                        foreach ($_v in Get-ChildItem ($_varName + '*') | sort Name)\r\n                        { \r\n                            $_nameFound = $_v.name\r\n                            $(if ($_nameFound.IndexOfAny($_varsRequiringQuotes) -eq -1) {'{0}{1}{2}'}\r\n                            else {'{0}{{{1}{2}}}'}) -f $_prefix, $_provider, $_nameFound\r\n                        }\r\n                        break;\r\n                    }\r\n\r\n                    # Do completion on parameters...\r\n                    '^-([*\\w0-9]*)' {\r\n                        $_pat = $matches[1] + '*'\r\n\r\n                        # extract the command name from the string\r\n                        # first split the string into statements and pipeline elements\r\n                        # This doesn't handle strings however.\r\n                        $_command = [regex]::Split($line, '[|;=]')[-1]\r\n\r\n                        #  Extract the trailing unclosed block e.g. ls | foreach { cp\r\n                        if ($_command -match '\\{([^\\{\\}]*)$')\r\n                        {\r\n                            $_command = $matches[1]\r\n                        }\r\n\r\n                        # Extract the longest unclosed parenthetical expression...\r\n                        if ($_command -match '\\(([^()]*)$')\r\n                        {\r\n                            $_command = $matches[1]\r\n                        }\r\n\r\n                        # take the first space separated token of the remaining string\r\n                        # as the command to look up. Trim any leading or trailing spaces\r\n                        # so you don't get leading empty elements.\r\n                        $_command = $_command.TrimEnd('-')\r\n                        $_command,$_arguments = $_command.Trim().Split()\r\n\r\n                        # now get the info object for it, -ArgumentList will force aliases to be resolved\r\n                        # it also retrieves dynamic parameters\r\n                        try\r\n                        {\r\n                            $_command = @(Get-Command -type 'Alias,Cmdlet,Function,Filter,ExternalScript' `\r\n                                -Name $_command -ArgumentList $_arguments)[0]\r\n                        }\r\n                        catch\r\n                        {\r\n                            # see if the command is an alias. If so, resolve it to the real command\r\n                            if(Test-Path alias:\\$_command)\r\n                            {\r\n                                $_command = @(Get-Command -Type Alias $_command)[0].Definition\r\n                            }\r\n\r\n                            # If we were unsuccessful retrieving the command, try again without the parameters\r\n                            $_command = @(Get-Command -type 'Cmdlet,Function,Filter,ExternalScript' `\r\n                                -Name $_command)[0]\r\n                        }\r\n\r\n                        # remove errors generated by the command not being found, and break\r\n                        if(-not $_command) { $error.RemoveAt(0); break; }\r\n\r\n                        # expand the parameter sets and emit the matching elements\r\n                        # need to use psbase.Keys in case 'keys' is one of the parameters\r\n                        # to the cmdlet\r\n                        foreach ($_n in $_command.Parameters.psbase.Keys)\r\n                        {\r\n                            if ($_n -like $_pat) { '-' + $_n }\r\n                        }\r\n                        break;\r\n                    }\r\n\r\n                    # Tab complete against history either #<pattern> or #<id>\r\n                    '^#(\\w*)' {\r\n                        $_pattern = $matches[1]\r\n                        if ($_pattern -match '^[0-9]+$')\r\n                        {\r\n                            Get-History -ea SilentlyContinue -Id $_pattern | Foreach { $_.CommandLine } \r\n                        }\r\n                        else\r\n                        {\r\n                            $_pattern = '*' + $_pattern + '*'\r\n                            Get-History -Count 32767 | Sort-Object -Descending Id| Foreach { $_.CommandLine } | where { $_ -like $_pattern }\r\n                        }\r\n                        break;\r\n                    }\r\n\r\n                    # try to find a matching command...\r\n                    default {\r\n                        # parse the script...\r\n                        $_tokens = [System.Management.Automation.PSParser]::Tokenize($line,\r\n                            [ref] $null)\r\n\r\n                        if ($_tokens)\r\n                        {\r\n                            $_lastToken = $_tokens[$_tokens.count - 1]\r\n                            if ($_lastToken.Type -eq 'Command')\r\n                            {\r\n                                $_cmd = $_lastToken.Content\r\n\r\n                                # don't look for paths...\r\n                                if ($_cmd.IndexOfAny('/\\:') -eq -1)\r\n                                {\r\n                                    # handle parsing errors - the last token string should be the last\r\n                                    # string in the line...\r\n                                    if ($lastword.Length -ge $_cmd.Length -and \r\n                                        $lastword.substring($lastword.length-$_cmd.length) -eq $_cmd)\r\n                                    {\r\n                                        $_pat = $_cmd + '*'\r\n                                        $_base = $lastword.substring(0, $lastword.length-$_cmd.length)\r\n\r\n                                        # get files in current directory first, then look for commands...\r\n                                        $( try {Resolve-Path -ea SilentlyContinue -Relative $_pat } catch {} ;\r\n                                           try { $ExecutionContext.InvokeCommand.GetCommandName($_pat, $true, $false) |\r\n                                               Sort-Object -Unique } catch {} ) |\r\n                                                   # If the command contains non-word characters (space, ) ] ; ) etc.)\r\n                                                   # then it needs to be quoted and prefixed with &\r\n                                                   ForEach-Object {\r\n                                                        if ($_.IndexOfAny($_charsRequiringQuotes) -eq -1) { $_ }\r\n                                                        elseif ($_.IndexOf('''') -ge 0) {'& ''{0}''' -f $_.Replace('''','''''') }\r\n                                                        else { '& ''{0}''' -f $_ }} |\r\n                                                   ForEach-Object {'{0}{1}' -f $_base,$_ }\r\n                                    }\r\n                                }\r\n                            }\r\n                        }\r\n                    }\r\n                }\r\n            }\r\n        ";

		// Token: 0x040010A4 RID: 4260
		internal const string CallId = "ci";

		// Token: 0x040010A5 RID: 4261
		internal const string MethodId = "mi";

		// Token: 0x040010A6 RID: 4262
		internal const string MethodParameters = "mp";

		// Token: 0x040010A7 RID: 4263
		internal const string MethodReturnValue = "mr";

		// Token: 0x040010A8 RID: 4264
		internal const string MethodException = "me";

		// Token: 0x040010A9 RID: 4265
		internal const string PS_STARTUP_PROTOCOL_VERSION_NAME = "protocolversion";

		// Token: 0x040010AA RID: 4266
		internal const string PublicKeyAsXml = "PublicKeyAsXml";

		// Token: 0x040010AB RID: 4267
		internal const string PSVersion = "PSVersion";

		// Token: 0x040010AC RID: 4268
		internal const string SerializationVersion = "SerializationVersion";

		// Token: 0x040010AD RID: 4269
		internal const string MethodArrayElementType = "mat";

		// Token: 0x040010AE RID: 4270
		internal const string MethodArrayLengths = "mal";

		// Token: 0x040010AF RID: 4271
		internal const string MethodArrayElements = "mae";

		// Token: 0x040010B0 RID: 4272
		internal const string ObjectType = "T";

		// Token: 0x040010B1 RID: 4273
		internal const string ObjectValue = "V";

		// Token: 0x040010B2 RID: 4274
		internal const string DiscoveryName = "Name";

		// Token: 0x040010B3 RID: 4275
		internal const string DiscoveryType = "CommandType";

		// Token: 0x040010B4 RID: 4276
		internal const string DiscoveryModule = "Namespace";

		// Token: 0x040010B5 RID: 4277
		internal const string DiscoveryFullyQualifiedModule = "FullyQualifiedModule";

		// Token: 0x040010B6 RID: 4278
		internal const string DiscoveryArgumentList = "ArgumentList";

		// Token: 0x040010B7 RID: 4279
		internal const string DiscoveryCount = "Count";

		// Token: 0x040010B8 RID: 4280
		internal const string PSInvocationSettings = "PSInvocationSettings";

		// Token: 0x040010B9 RID: 4281
		internal const string ApartmentState = "ApartmentState";

		// Token: 0x040010BA RID: 4282
		internal const string RemoteStreamOptions = "RemoteStreamOptions";

		// Token: 0x040010BB RID: 4283
		internal const string AddToHistory = "AddToHistory";

		// Token: 0x040010BC RID: 4284
		internal const string PowerShell = "PowerShell";

		// Token: 0x040010BD RID: 4285
		internal const string IsNested = "IsNested";

		// Token: 0x040010BE RID: 4286
		internal const string HistoryString = "History";

		// Token: 0x040010BF RID: 4287
		internal const string RedirectShellErrorOutputPipe = "RedirectShellErrorOutputPipe";

		// Token: 0x040010C0 RID: 4288
		internal const string Commands = "Cmds";

		// Token: 0x040010C1 RID: 4289
		internal const string ExtraCommands = "ExtraCmds";

		// Token: 0x040010C2 RID: 4290
		internal const string CommandText = "Cmd";

		// Token: 0x040010C3 RID: 4291
		internal const string IsScript = "IsScript";

		// Token: 0x040010C4 RID: 4292
		internal const string UseLocalScopeNullable = "UseLocalScope";

		// Token: 0x040010C5 RID: 4293
		internal const string MergeUnclaimedPreviousCommandResults = "MergePreviousResults";

		// Token: 0x040010C6 RID: 4294
		internal const string MergeMyResult = "MergeMyResult";

		// Token: 0x040010C7 RID: 4295
		internal const string MergeToResult = "MergeToResult";

		// Token: 0x040010C8 RID: 4296
		internal const string MergeError = "MergeError";

		// Token: 0x040010C9 RID: 4297
		internal const string MergeWarning = "MergeWarning";

		// Token: 0x040010CA RID: 4298
		internal const string MergeVerbose = "MergeVerbose";

		// Token: 0x040010CB RID: 4299
		internal const string MergeDebug = "MergeDebug";

		// Token: 0x040010CC RID: 4300
		internal const string MergeInformation = "MergeInformation";

		// Token: 0x040010CD RID: 4301
		internal const string Parameters = "Args";

		// Token: 0x040010CE RID: 4302
		internal const string ParameterName = "N";

		// Token: 0x040010CF RID: 4303
		internal const string ParameterValue = "V";

		// Token: 0x040010D0 RID: 4304
		internal const string NoInput = "NoInput";

		// Token: 0x040010D1 RID: 4305
		internal const string ExceptionAsErrorRecord = "ExceptionAsErrorRecord";

		// Token: 0x040010D2 RID: 4306
		internal const string PipelineState = "PipelineState";

		// Token: 0x040010D3 RID: 4307
		internal const string RunspaceState = "RunspaceState";

		// Token: 0x040010D4 RID: 4308
		internal const string PSEventArgsComputerName = "PSEventArgs.ComputerName";

		// Token: 0x040010D5 RID: 4309
		internal const string PSEventArgsRunspaceId = "PSEventArgs.RunspaceId";

		// Token: 0x040010D6 RID: 4310
		internal const string PSEventArgsEventIdentifier = "PSEventArgs.EventIdentifier";

		// Token: 0x040010D7 RID: 4311
		internal const string PSEventArgsSourceIdentifier = "PSEventArgs.SourceIdentifier";

		// Token: 0x040010D8 RID: 4312
		internal const string PSEventArgsTimeGenerated = "PSEventArgs.TimeGenerated";

		// Token: 0x040010D9 RID: 4313
		internal const string PSEventArgsSender = "PSEventArgs.Sender";

		// Token: 0x040010DA RID: 4314
		internal const string PSEventArgsSourceArgs = "PSEventArgs.SourceArgs";

		// Token: 0x040010DB RID: 4315
		internal const string PSEventArgsMessageData = "PSEventArgs.MessageData";

		// Token: 0x040010DC RID: 4316
		internal const string MinRunspaces = "MinRunspaces";

		// Token: 0x040010DD RID: 4317
		internal const string MaxRunspaces = "MaxRunspaces";

		// Token: 0x040010DE RID: 4318
		internal const string ThreadOptions = "PSThreadOptions";

		// Token: 0x040010DF RID: 4319
		internal const string HostInfo = "HostInfo";

		// Token: 0x040010E0 RID: 4320
		internal const string RunspacePoolOperationResponse = "SetMinMaxRunspacesResponse";

		// Token: 0x040010E1 RID: 4321
		internal const string AvailableRunspaces = "AvailableRunspaces";

		// Token: 0x040010E2 RID: 4322
		internal const string PublicKey = "PublicKey";

		// Token: 0x040010E3 RID: 4323
		internal const string EncryptedSessionKey = "EncryptedSessionKey";

		// Token: 0x040010E4 RID: 4324
		internal const string ApplicationArguments = "ApplicationArguments";

		// Token: 0x040010E5 RID: 4325
		internal const string ApplicationPrivateData = "ApplicationPrivateData";

		// Token: 0x040010E6 RID: 4326
		internal const string ProgressRecord_Activity = "Activity";

		// Token: 0x040010E7 RID: 4327
		internal const string ProgressRecord_ActivityId = "ActivityId";

		// Token: 0x040010E8 RID: 4328
		internal const string ProgressRecord_CurrentOperation = "CurrentOperation";

		// Token: 0x040010E9 RID: 4329
		internal const string ProgressRecord_ParentActivityId = "ParentActivityId";

		// Token: 0x040010EA RID: 4330
		internal const string ProgressRecord_PercentComplete = "PercentComplete";

		// Token: 0x040010EB RID: 4331
		internal const string ProgressRecord_Type = "Type";

		// Token: 0x040010EC RID: 4332
		internal const string ProgressRecord_SecondsRemaining = "SecondsRemaining";

		// Token: 0x040010ED RID: 4333
		internal const string ProgressRecord_StatusDescription = "StatusDescription";
	}
}
