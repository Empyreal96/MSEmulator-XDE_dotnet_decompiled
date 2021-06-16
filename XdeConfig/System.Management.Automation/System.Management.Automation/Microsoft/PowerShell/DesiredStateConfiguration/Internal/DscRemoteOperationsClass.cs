using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Reflection;
using System.Security;
using Microsoft.Management.Infrastructure;

namespace Microsoft.PowerShell.DesiredStateConfiguration.Internal
{
	// Token: 0x020009FA RID: 2554
	public static class DscRemoteOperationsClass
	{
		// Token: 0x06005D97 RID: 23959 RVA: 0x001FE980 File Offset: 0x001FCB80
		public static object ConvertCimInstanceToObject(Type targetType, CimInstance instance, string moduleName)
		{
			string className = instance.CimClass.CimSystemProperties.ClassName;
			object obj = null;
			using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
			{
				string script = "param($targetType,$moduleName) & (Microsoft.PowerShell.Core\\Get-Module $moduleName) { New-Object $targetType } ";
				powerShell.AddScript(script);
				powerShell.AddArgument(targetType);
				powerShell.AddArgument(moduleName);
				Collection<PSObject> collection = powerShell.Invoke();
				if (collection.Count != 1)
				{
					Exception innerException = null;
					if (powerShell.Streams.Error != null && powerShell.Streams.Error.Count > 0)
					{
						innerException = powerShell.Streams.Error[0].Exception;
					}
					string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.InstantiatePSClassObjectFailed, new object[]
					{
						className
					});
					InvalidOperationException ex = new InvalidOperationException(message, innerException);
					throw ex;
				}
				obj = collection[0].BaseObject;
			}
			foreach (CimProperty cimProperty in instance.CimInstanceProperties)
			{
				if (cimProperty.Value != null)
				{
					MemberInfo[] member = targetType.GetMember(cimProperty.Name, BindingFlags.Instance | BindingFlags.Public);
					if (member == null || member.Length > 1 || (!(member[0] is PropertyInfo) && !(member[0] is FieldInfo)))
					{
						string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.PropertyNotDeclaredInPSClass, new object[]
						{
							cimProperty.Name,
							className
						});
						InvalidOperationException ex2 = new InvalidOperationException(message);
						throw ex2;
					}
					MemberInfo memberInfo = member[0];
					Type type = (memberInfo is FieldInfo) ? ((FieldInfo)memberInfo).FieldType : ((PropertyInfo)memberInfo).PropertyType;
					CimType cimType = cimProperty.CimType;
					object obj2;
					if (cimType != CimType.Instance)
					{
						if (cimType != CimType.InstanceArray)
						{
							obj2 = LanguagePrimitives.ConvertTo(cimProperty.Value, type, CultureInfo.InvariantCulture);
						}
						else if (type == typeof(Hashtable))
						{
							obj2 = DscRemoteOperationsClass.ConvertCimInstanceHashtable(moduleName, (CimInstance[])cimProperty.Value);
						}
						else
						{
							CimInstance[] array = (CimInstance[])cimProperty.Value;
							if (!type.IsArray)
							{
								string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.ExpectArrayTypeOfPropertyInPSClass, new object[]
								{
									cimProperty.Name,
									className
								});
								InvalidOperationException ex3 = new InvalidOperationException(message);
								throw ex3;
							}
							Type elementType = type.GetElementType();
							Array array2 = Array.CreateInstance(elementType, array.Length);
							for (int i = 0; i < array.Length; i++)
							{
								object obj3 = DscRemoteOperationsClass.ConvertCimInstanceToObject(elementType, array[i], moduleName);
								if (obj3 == null)
								{
									return null;
								}
								array2.SetValue(obj3, i);
							}
							obj2 = array2;
						}
					}
					else
					{
						CimInstance cimInstance = cimProperty.Value as CimInstance;
						if (cimInstance != null && cimInstance.CimClass != null && cimInstance.CimClass.CimSystemProperties != null && string.Equals(cimInstance.CimClass.CimSystemProperties.ClassName, "MSFT_Credential", StringComparison.OrdinalIgnoreCase))
						{
							obj2 = DscRemoteOperationsClass.ConvertCimInstancePsCredential(moduleName, cimInstance);
						}
						else
						{
							obj2 = DscRemoteOperationsClass.ConvertCimInstanceToObject(type, cimInstance, moduleName);
						}
						if (obj2 == null)
						{
							return null;
						}
					}
					if (obj2 == null)
					{
						string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.ConvertCimPropertyToObjectPropertyFailed, new object[]
						{
							cimProperty.Name,
							className
						});
						InvalidOperationException ex4 = new InvalidOperationException(message);
						throw ex4;
					}
					if (memberInfo is FieldInfo)
					{
						((FieldInfo)memberInfo).SetValue(obj, obj2);
					}
					if (memberInfo is PropertyInfo)
					{
						((PropertyInfo)memberInfo).SetValue(obj, obj2);
					}
				}
			}
			return obj;
		}

		// Token: 0x06005D98 RID: 23960 RVA: 0x001FED40 File Offset: 0x001FCF40
		private static object ConvertCimInstanceHashtable(string providerName, CimInstance[] arrayInstance)
		{
			Hashtable hashtable = new Hashtable();
			try
			{
				foreach (CimInstance cimInstance in arrayInstance)
				{
					CimProperty cimProperty = cimInstance.CimInstanceProperties["Key"];
					CimProperty cimProperty2 = cimInstance.CimInstanceProperties["Value"];
					if (cimProperty == null || cimProperty2 == null)
					{
						string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.InvalidHashtable, new object[]
						{
							providerName
						});
						InvalidOperationException ex = new InvalidOperationException(message);
						throw ex;
					}
					hashtable.Add(LanguagePrimitives.ConvertTo<string>(cimProperty.Value), LanguagePrimitives.ConvertTo<string>(cimProperty2.Value));
				}
			}
			catch (Exception innerException)
			{
				string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.InvalidHashtable, new object[]
				{
					providerName
				});
				InvalidOperationException ex2 = new InvalidOperationException(message, innerException);
				throw ex2;
			}
			return hashtable;
		}

		// Token: 0x06005D99 RID: 23961 RVA: 0x001FEE24 File Offset: 0x001FD024
		private static object ConvertCimInstancePsCredential(string providerName, CimInstance propertyInstance)
		{
			string text;
			try
			{
				text = (propertyInstance.CimInstanceProperties["UserName"].Value as string);
				if (string.IsNullOrEmpty(text))
				{
					string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.InvalidUserName, new object[]
					{
						providerName
					});
					InvalidOperationException ex = new InvalidOperationException(message);
					throw ex;
				}
			}
			catch (CimException innerException)
			{
				string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.InvalidUserName, new object[]
				{
					providerName
				});
				InvalidOperationException ex2 = new InvalidOperationException(message, innerException);
				throw ex2;
			}
			string text2;
			try
			{
				text2 = (propertyInstance.CimInstanceProperties["PassWord"].Value as string);
				if (string.IsNullOrEmpty(text2))
				{
					string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.InvalidPassword, new object[]
					{
						providerName
					});
					InvalidOperationException ex3 = new InvalidOperationException(message);
					throw ex3;
				}
			}
			catch (CimException innerException2)
			{
				string message = string.Format(CultureInfo.CurrentCulture, ParserStrings.InvalidPassword, new object[]
				{
					providerName
				});
				InvalidOperationException ex4 = new InvalidOperationException(message, innerException2);
				throw ex4;
			}
			SecureString secureString = new SecureString();
			foreach (char c in text2)
			{
				secureString.AppendChar(c);
			}
			secureString.MakeReadOnly();
			return new PSCredential(text, secureString);
		}
	}
}
