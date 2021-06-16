using System;
using System.Reflection;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008D3 RID: 2259
	internal static class TelemetryWrapper
	{
		// Token: 0x0600555E RID: 21854 RVA: 0x001C177C File Offset: 0x001BF97C
		static TelemetryWrapper()
		{
			if (!TelemetryWrapper.InitializeEventSource())
			{
				return;
			}
			TelemetryWrapper.eventSourceOptionsForWrite = TelemetryWrapper.InitializeEventSourceOptions("Informational", 70368744177664L);
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x001C17E0 File Offset: 0x001BF9E0
		public static void TraceMessage<T>(string message, T arguments)
		{
			if (TelemetryWrapper.eventSourceOptionsForWrite != null)
			{
				object[] parameters = new object[]
				{
					message,
					TelemetryWrapper.eventSourceOptionsForWrite,
					arguments
				};
				MethodInfo writeGenericMethod = TelemetryWrapper.GetWriteGenericMethod(arguments.GetType());
				try
				{
					writeGenericMethod.Invoke(TelemetryWrapper.eventSourceInstance, parameters);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06005560 RID: 21856 RVA: 0x001C1848 File Offset: 0x001BFA48
		private static bool LoadTelemetryAssembly()
		{
			if (null == TelemetryWrapper.telemetryAssembly)
			{
				try
				{
					TelemetryWrapper.telemetryAssembly = Assembly.Load(new AssemblyName("System.Diagnostics.Tracing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"));
				}
				catch
				{
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x06005561 RID: 21857 RVA: 0x001C1890 File Offset: 0x001BFA90
		private static bool InitializeEventSource()
		{
			if (!TelemetryWrapper.LoadTelemetryAssembly())
			{
				return false;
			}
			if (null != TelemetryWrapper.eventSourceType && TelemetryWrapper.eventSourceInstance != null)
			{
				return true;
			}
			try
			{
				TelemetryWrapper.eventSourceType = TelemetryWrapper.telemetryAssembly.GetType("System.Diagnostics.Tracing.EventSource");
				if (null == TelemetryWrapper.eventSourceType)
				{
					return false;
				}
				object eventSourceSettingsEnumObject = TelemetryWrapper.GetEventSourceSettingsEnumObject("EtwSelfDescribingEventFormat");
				if (eventSourceSettingsEnumObject != null)
				{
					TelemetryWrapper.eventSourceInstance = Activator.CreateInstance(TelemetryWrapper.eventSourceType, new object[]
					{
						"Microsoft.Windows.PowerShell",
						eventSourceSettingsEnumObject,
						TelemetryWrapper.telemetryTraits
					});
				}
				if (TelemetryWrapper.eventSourceInstance == null)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06005562 RID: 21858 RVA: 0x001C1944 File Offset: 0x001BFB44
		private static object GetEventLevelEnumObject(string eventLevel)
		{
			Type type = TelemetryWrapper.telemetryAssembly.GetType("System.Diagnostics.Tracing.EventLevel");
			FieldInfo field = type.GetField(eventLevel);
			int value = (int)field.GetValue(type);
			return Enum.ToObject(type, value);
		}

		// Token: 0x06005563 RID: 21859 RVA: 0x001C1980 File Offset: 0x001BFB80
		private static object GetEventSourceSettingsEnumObject(string eventSourceSettings)
		{
			Type type = TelemetryWrapper.telemetryAssembly.GetType("System.Diagnostics.Tracing.EventSourceSettings");
			if (type != null)
			{
				FieldInfo field = type.GetField(eventSourceSettings);
				if (field != null)
				{
					int value = (int)field.GetValue(type);
					return Enum.ToObject(type, value);
				}
			}
			return null;
		}

		// Token: 0x06005564 RID: 21860 RVA: 0x001C19D0 File Offset: 0x001BFBD0
		private static object InitializeEventSourceOptions(string eventLevel, long eventKeywords)
		{
			if (null != TelemetryWrapper.telemetryAssembly)
			{
				object obj = null;
				try
				{
					Type type = TelemetryWrapper.telemetryAssembly.GetType("System.Diagnostics.Tracing.EventSourceOptions");
					obj = Activator.CreateInstance(type, null);
					PropertyInfo property = type.GetProperty("Level");
					PropertyInfo property2 = type.GetProperty("Keywords");
					object eventLevelEnumObject = TelemetryWrapper.GetEventLevelEnumObject(eventLevel);
					property.SetValue(obj, eventLevelEnumObject, null);
					property2.SetValue(obj, eventKeywords, null);
				}
				catch
				{
					return null;
				}
				return obj;
			}
			return null;
		}

		// Token: 0x06005565 RID: 21861 RVA: 0x001C1A5C File Offset: 0x001BFC5C
		private static MethodInfo GetWriteGenericMethod(Type argumentType)
		{
			try
			{
				Type type = TelemetryWrapper.telemetryAssembly.GetType("System.Diagnostics.Tracing.EventSourceOptions");
				Type[] array = new Type[]
				{
					typeof(string),
					type,
					argumentType
				};
				Type[] typeArguments = new Type[]
				{
					argumentType
				};
				MethodInfo[] methods = TelemetryWrapper.eventSourceType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
				foreach (MethodInfo methodInfo in methods)
				{
					if ("write" == methodInfo.Name.ToLowerInvariant() && methodInfo.IsGenericMethodDefinition)
					{
						MethodInfo methodInfo2 = methodInfo.MakeGenericMethod(typeArguments);
						ParameterInfo[] parameters = methodInfo2.GetParameters();
						if (parameters.Length == array.Length)
						{
							return methodInfo2;
						}
					}
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

		// Token: 0x04002CBC RID: 11452
		private const long measuresKeyword = 70368744177664L;

		// Token: 0x04002CBD RID: 11453
		private static Assembly telemetryAssembly = null;

		// Token: 0x04002CBE RID: 11454
		private static Type eventSourceType = null;

		// Token: 0x04002CBF RID: 11455
		private static object eventSourceInstance = null;

		// Token: 0x04002CC0 RID: 11456
		private static object eventSourceOptionsForWrite = null;

		// Token: 0x04002CC1 RID: 11457
		private static readonly string[] telemetryTraits = new string[]
		{
			"ETW_GROUP",
			"{4f50731a-89cf-4782-b3e0-dce8c90476ba}"
		};
	}
}
