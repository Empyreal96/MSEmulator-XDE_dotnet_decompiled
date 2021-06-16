using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Management.Infrastructure;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000119 RID: 281
	public static class LanguagePrimitives
	{
		// Token: 0x06000F01 RID: 3841 RVA: 0x000529EC File Offset: 0x00050BEC
		internal static void CreateMemberNotFoundError(PSObject pso, DictionaryEntry property, Type resultType)
		{
			string availableProperties = LanguagePrimitives.GetAvailableProperties(pso);
			string message = StringUtil.Format(ExtendedTypeSystem.PropertyNotFound, new object[]
			{
				property.Key.ToString(),
				resultType.FullName,
				availableProperties
			});
			LanguagePrimitives.typeConversion.WriteLine("Issuing an error message about not being able to create an object from hashtable.", new object[0]);
			throw new InvalidOperationException(message);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00052A4A File Offset: 0x00050C4A
		internal static void CreateMemberSetValueError(SetValueException e)
		{
			LanguagePrimitives.typeConversion.WriteLine("Issuing an error message about not being able to set the properties for an object.", new object[0]);
			throw e;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00052A64 File Offset: 0x00050C64
		static LanguagePrimitives()
		{
			Type[][] array = new Type[11][];
			array[0] = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(int),
				typeof(long),
				typeof(double),
				typeof(short),
				typeof(short),
				typeof(float),
				typeof(double),
				typeof(decimal)
			};
			array[1] = new Type[]
			{
				typeof(int),
				typeof(int),
				typeof(long),
				typeof(int),
				typeof(long),
				typeof(double),
				typeof(int),
				typeof(int),
				typeof(double),
				typeof(double),
				typeof(decimal)
			};
			array[2] = new Type[]
			{
				typeof(long),
				typeof(long),
				typeof(long),
				typeof(long),
				typeof(long),
				typeof(decimal),
				typeof(long),
				typeof(long),
				typeof(double),
				typeof(double),
				typeof(decimal)
			};
			array[3] = new Type[]
			{
				typeof(int),
				typeof(int),
				typeof(long),
				typeof(ushort),
				typeof(uint),
				typeof(ulong),
				typeof(int),
				typeof(ushort),
				typeof(float),
				typeof(double),
				typeof(decimal)
			};
			array[4] = new Type[]
			{
				typeof(long),
				typeof(long),
				typeof(long),
				typeof(uint),
				typeof(uint),
				typeof(ulong),
				typeof(long),
				typeof(uint),
				typeof(double),
				typeof(double),
				typeof(decimal)
			};
			array[5] = new Type[]
			{
				typeof(double),
				typeof(double),
				typeof(decimal),
				typeof(ulong),
				typeof(ulong),
				typeof(ulong),
				typeof(double),
				typeof(ulong),
				typeof(double),
				typeof(double),
				typeof(decimal)
			};
			array[6] = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(int),
				typeof(long),
				typeof(double),
				typeof(sbyte),
				typeof(short),
				typeof(float),
				typeof(double),
				typeof(decimal)
			};
			array[7] = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(ushort),
				typeof(uint),
				typeof(ulong),
				typeof(short),
				typeof(byte),
				typeof(float),
				typeof(double),
				typeof(decimal)
			};
			Type[][] array2 = array;
			int num = 8;
			Type[] array3 = new Type[11];
			array3[0] = typeof(float);
			array3[1] = typeof(double);
			array3[2] = typeof(double);
			array3[3] = typeof(float);
			array3[4] = typeof(double);
			array3[5] = typeof(double);
			array3[6] = typeof(float);
			array3[7] = typeof(float);
			array3[8] = typeof(float);
			array3[9] = typeof(double);
			array2[num] = array3;
			Type[][] array4 = array;
			int num2 = 9;
			Type[] array5 = new Type[11];
			array5[0] = typeof(double);
			array5[1] = typeof(double);
			array5[2] = typeof(double);
			array5[3] = typeof(double);
			array5[4] = typeof(double);
			array5[5] = typeof(double);
			array5[6] = typeof(double);
			array5[7] = typeof(double);
			array5[8] = typeof(double);
			array5[9] = typeof(double);
			array4[num2] = array5;
			array[10] = new Type[]
			{
				typeof(decimal),
				typeof(decimal),
				typeof(decimal),
				typeof(decimal),
				typeof(decimal),
				typeof(decimal),
				typeof(decimal),
				typeof(decimal),
				null,
				null,
				typeof(decimal)
			};
			LanguagePrimitives.LargestTypeTable = array;
			LanguagePrimitives.typeCodeTraits = new LanguagePrimitives.TypeCodeTraits[]
			{
				LanguagePrimitives.TypeCodeTraits.None,
				LanguagePrimitives.TypeCodeTraits.None,
				LanguagePrimitives.TypeCodeTraits.None,
				LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.SignedInteger | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.UnsignedInteger | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.SignedInteger | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.UnsignedInteger | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.SignedInteger | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.UnsignedInteger | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.SignedInteger | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.UnsignedInteger | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.Floating | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.Floating | LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.Decimal,
				LanguagePrimitives.TypeCodeTraits.CimIntrinsicType,
				LanguagePrimitives.TypeCodeTraits.None,
				LanguagePrimitives.TypeCodeTraits.CimIntrinsicType
			};
			LanguagePrimitives.typeConversion = PSTraceSource.GetTracer("TypeConversion", "Traces the type conversion algorithm", false);
			LanguagePrimitives.nameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				{
					"SInt8",
					"SByte"
				},
				{
					"UInt8",
					"Byte"
				},
				{
					"SInt16",
					"Int16"
				},
				{
					"UInt16",
					"UInt16"
				},
				{
					"SInt32",
					"Int32"
				},
				{
					"UInt32",
					"UInt32"
				},
				{
					"SInt64",
					"Int64"
				},
				{
					"UInt64",
					"UInt64"
				},
				{
					"Real32",
					"Single"
				},
				{
					"Real64",
					"double"
				},
				{
					"Boolean",
					"bool"
				},
				{
					"String",
					"string"
				},
				{
					"DateTime",
					"DateTime"
				},
				{
					"Reference",
					"CimInstance"
				},
				{
					"Char16",
					"char"
				},
				{
					"Instance",
					"CimInstance"
				},
				{
					"BooleanArray",
					"bool[]"
				},
				{
					"UInt8Array",
					"byte[]"
				},
				{
					"SInt8Array",
					"Sbyte[]"
				},
				{
					"UInt16Array",
					"uint16[]"
				},
				{
					"SInt16Array",
					"int64[]"
				},
				{
					"UInt32Array",
					"UInt32[]"
				},
				{
					"SInt32Array",
					"Int32[]"
				},
				{
					"UInt64Array",
					"UInt64[]"
				},
				{
					"SInt64Array",
					"Int64[]"
				},
				{
					"Real32Array",
					"Single[]"
				},
				{
					"Real64Array",
					"double[]"
				},
				{
					"Char16Array",
					"char[]"
				},
				{
					"DateTimeArray",
					"DateTime[]"
				},
				{
					"StringArray",
					"string[]"
				},
				{
					"ReferenceArray",
					"CimInstance[]"
				},
				{
					"InstanceArray",
					"CimInstance[]"
				},
				{
					"Unknown",
					"UnknownType"
				}
			};
			LanguagePrimitives.converterCache = new Dictionary<LanguagePrimitives.ConversionTypePair, LanguagePrimitives.ConversionData>(256);
			LanguagePrimitives.NumericTypes = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(ushort),
				typeof(uint),
				typeof(ulong),
				typeof(sbyte),
				typeof(byte),
				typeof(float),
				typeof(double),
				typeof(decimal)
			};
			LanguagePrimitives.IntegerTypes = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(ushort),
				typeof(uint),
				typeof(ulong),
				typeof(sbyte),
				typeof(byte)
			};
			LanguagePrimitives.SignedIntegerTypes = new Type[]
			{
				typeof(sbyte),
				typeof(short),
				typeof(int),
				typeof(long)
			};
			LanguagePrimitives.UnsignedIntegerTypes = new Type[]
			{
				typeof(byte),
				typeof(ushort),
				typeof(uint),
				typeof(ulong)
			};
			LanguagePrimitives.RealTypes = new Type[]
			{
				typeof(float),
				typeof(double),
				typeof(decimal)
			};
			LanguagePrimitives.possibleTypeConverter = new Dictionary<string, bool>(16);
			LanguagePrimitives.RebuildConversionCache();
			LanguagePrimitives.InitializeGetEnumerableCache();
			AppDomain.CurrentDomain.AssemblyResolve += LanguagePrimitives.AssemblyResolveHelper;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0005367C File Offset: 0x0005187C
		internal static void UpdateTypeConvertFromTypeTable(string typeName)
		{
			lock (LanguagePrimitives.converterCache)
			{
				foreach (LanguagePrimitives.ConversionTypePair conversionTypePair in LanguagePrimitives.converterCache.Keys.ToArray<LanguagePrimitives.ConversionTypePair>())
				{
					if (string.Equals(conversionTypePair.to.FullName, typeName, StringComparison.OrdinalIgnoreCase) || string.Equals(conversionTypePair.from.FullName, typeName, StringComparison.OrdinalIgnoreCase))
					{
						LanguagePrimitives.converterCache.Remove(conversionTypePair);
					}
				}
				LanguagePrimitives.possibleTypeConverter[typeName] = true;
			}
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0005371C File Offset: 0x0005191C
		private static IEnumerable GetEnumerableFromIEnumerableT(object obj)
		{
			foreach (Type type in obj.GetType().GetInterfaces())
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					return new LanguagePrimitives.EnumerableTWrapper(obj, type);
				}
			}
			return null;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00053780 File Offset: 0x00051980
		private static LanguagePrimitives.GetEnumerableDelegate GetOrCalculateEnumerable(Type type)
		{
			LanguagePrimitives.GetEnumerableDelegate getEnumerableDelegate = null;
			lock (LanguagePrimitives.getEnumerableCache)
			{
				if (!LanguagePrimitives.getEnumerableCache.TryGetValue(type, out getEnumerableDelegate))
				{
					getEnumerableDelegate = LanguagePrimitives.CalculateGetEnumerable(type);
					LanguagePrimitives.getEnumerableCache.Add(type, getEnumerableDelegate);
				}
			}
			return getEnumerableDelegate;
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000537E0 File Offset: 0x000519E0
		private static void InitializeGetEnumerableCache()
		{
			lock (LanguagePrimitives.getEnumerableCache)
			{
				LanguagePrimitives.getEnumerableCache.Clear();
				LanguagePrimitives.getEnumerableCache.Add(typeof(string), new LanguagePrimitives.GetEnumerableDelegate(LanguagePrimitives.ReturnNullEnumerable));
				LanguagePrimitives.getEnumerableCache.Add(typeof(int), new LanguagePrimitives.GetEnumerableDelegate(LanguagePrimitives.ReturnNullEnumerable));
				LanguagePrimitives.getEnumerableCache.Add(typeof(double), new LanguagePrimitives.GetEnumerableDelegate(LanguagePrimitives.ReturnNullEnumerable));
			}
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00053884 File Offset: 0x00051A84
		internal static bool IsTypeEnumerable(Type type)
		{
			LanguagePrimitives.GetEnumerableDelegate orCalculateEnumerable = LanguagePrimitives.GetOrCalculateEnumerable(type);
			return orCalculateEnumerable != new LanguagePrimitives.GetEnumerableDelegate(LanguagePrimitives.ReturnNullEnumerable);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x000538AC File Offset: 0x00051AAC
		public static IEnumerable GetEnumerable(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			Type type = obj.GetType();
			if (type == typeof(PSObject))
			{
				PSObject psobject = (PSObject)obj;
				obj = psobject.BaseObject;
				type = obj.GetType();
			}
			LanguagePrimitives.GetEnumerableDelegate orCalculateEnumerable = LanguagePrimitives.GetOrCalculateEnumerable(type);
			return orCalculateEnumerable(obj);
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x000538FB File Offset: 0x00051AFB
		private static IEnumerable ReturnNullEnumerable(object obj)
		{
			return null;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x000538FE File Offset: 0x00051AFE
		private static IEnumerable DataTableEnumerable(object obj)
		{
			return ((DataTable)obj).Rows;
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0005390C File Offset: 0x00051B0C
		private static IEnumerable TypicalEnumerable(object obj)
		{
			IEnumerable enumerable = (IEnumerable)obj;
			IEnumerable result;
			try
			{
				if (enumerable.GetEnumerator() == null)
				{
					result = LanguagePrimitives.GetEnumerableFromIEnumerableT(obj);
				}
				else
				{
					result = enumerable;
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				enumerable = LanguagePrimitives.GetEnumerableFromIEnumerableT(obj);
				if (enumerable == null)
				{
					throw new ExtendedTypeSystemException("ExceptionInGetEnumerator", ex, ExtendedTypeSystem.EnumerationException, new object[]
					{
						ex.Message
					});
				}
				result = enumerable;
			}
			return result;
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00053980 File Offset: 0x00051B80
		private static LanguagePrimitives.GetEnumerableDelegate CalculateGetEnumerable(Type objectType)
		{
			if (typeof(DataTable).IsAssignableFrom(objectType))
			{
				return new LanguagePrimitives.GetEnumerableDelegate(LanguagePrimitives.DataTableEnumerable);
			}
			if (typeof(IEnumerable).IsAssignableFrom(objectType) && !typeof(IDictionary).IsAssignableFrom(objectType) && !typeof(XmlNode).IsAssignableFrom(objectType))
			{
				return new LanguagePrimitives.GetEnumerableDelegate(LanguagePrimitives.TypicalEnumerable);
			}
			return new LanguagePrimitives.GetEnumerableDelegate(LanguagePrimitives.ReturnNullEnumerable);
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x000539FC File Offset: 0x00051BFC
		public static IEnumerator GetEnumerator(object obj)
		{
			IEnumerator enumerator = LanguagePrimitives._getEnumeratorSite.Target(LanguagePrimitives._getEnumeratorSite, obj);
			if (!(enumerator is EnumerableOps.NonEnumerableObjectEnumerator))
			{
				return enumerator;
			}
			return null;
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00053A2C File Offset: 0x00051C2C
		public static PSDataCollection<PSObject> GetPSDataCollection(object inputValue)
		{
			PSDataCollection<PSObject> psdataCollection = new PSDataCollection<PSObject>();
			if (inputValue != null)
			{
				IEnumerator enumerator = LanguagePrimitives.GetEnumerator(inputValue);
				if (enumerator != null)
				{
					while (enumerator.MoveNext())
					{
						psdataCollection.Add((enumerator.Current == null) ? null : PSObject.AsPSObject(enumerator.Current));
					}
				}
				else
				{
					psdataCollection.Add(PSObject.AsPSObject(inputValue));
				}
			}
			psdataCollection.Complete();
			return psdataCollection;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00053A87 File Offset: 0x00051C87
		public new static bool Equals(object first, object second)
		{
			return LanguagePrimitives.Equals(first, second, false, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00053A96 File Offset: 0x00051C96
		public static bool Equals(object first, object second, bool ignoreCase)
		{
			return LanguagePrimitives.Equals(first, second, ignoreCase, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00053AA8 File Offset: 0x00051CA8
		public static bool Equals(object first, object second, bool ignoreCase, IFormatProvider formatProvider)
		{
			if (formatProvider == null)
			{
				formatProvider = CultureInfo.InvariantCulture;
			}
			CultureInfo cultureInfo = formatProvider as CultureInfo;
			if (cultureInfo == null)
			{
				throw PSTraceSource.NewArgumentException("formatProvider");
			}
			first = PSObject.Base(first);
			second = PSObject.Base(second);
			if (first == null)
			{
				return second == null;
			}
			if (second == null)
			{
				return false;
			}
			string text = first as string;
			if (text != null)
			{
				string text2 = second as string;
				if (text2 == null)
				{
					text2 = (string)LanguagePrimitives.ConvertTo(second, typeof(string), cultureInfo);
				}
				return cultureInfo.CompareInfo.Compare(text, text2, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None) == 0;
			}
			if (first.Equals(second))
			{
				return true;
			}
			Type type = first.GetType();
			Type type2 = second.GetType();
			int num = LanguagePrimitives.TypeTableIndex(type);
			int num2 = LanguagePrimitives.TypeTableIndex(type2);
			if (num != -1 && num2 != -1)
			{
				return LanguagePrimitives.NumericCompare(first, second, num, num2) == 0;
			}
			if (type == typeof(char) && ignoreCase)
			{
				string text2 = second as string;
				if (text2 != null && text2.Length == 1)
				{
					char c = cultureInfo.TextInfo.ToUpper((char)first);
					char obj = cultureInfo.TextInfo.ToUpper(text2[0]);
					return c.Equals(obj);
				}
				if (type2 == typeof(char))
				{
					char c2 = cultureInfo.TextInfo.ToUpper((char)first);
					char obj2 = cultureInfo.TextInfo.ToUpper((char)second);
					return c2.Equals(obj2);
				}
			}
			try
			{
				object obj3 = LanguagePrimitives.ConvertTo(second, type, cultureInfo);
				return first.Equals(obj3);
			}
			catch (InvalidCastException)
			{
			}
			return false;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00053C4C File Offset: 0x00051E4C
		public static int Compare(object first, object second)
		{
			return LanguagePrimitives.Compare(first, second, false, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00053C5B File Offset: 0x00051E5B
		public static int Compare(object first, object second, bool ignoreCase)
		{
			return LanguagePrimitives.Compare(first, second, ignoreCase, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00053C6C File Offset: 0x00051E6C
		public static int Compare(object first, object second, bool ignoreCase, IFormatProvider formatProvider)
		{
			if (formatProvider == null)
			{
				formatProvider = CultureInfo.InvariantCulture;
			}
			CultureInfo cultureInfo = formatProvider as CultureInfo;
			if (cultureInfo == null)
			{
				throw PSTraceSource.NewArgumentException("formatProvider");
			}
			first = PSObject.Base(first);
			second = PSObject.Base(second);
			if (first == null)
			{
				if (second == null)
				{
					return 0;
				}
				switch (LanguagePrimitives.GetTypeCode(second.GetType()))
				{
				case TypeCode.SByte:
					if (Math.Sign((sbyte)second) >= 0)
					{
						return -1;
					}
					return 1;
				case TypeCode.Int16:
					if (Math.Sign((short)second) >= 0)
					{
						return -1;
					}
					return 1;
				case TypeCode.Int32:
					if (Math.Sign((int)second) >= 0)
					{
						return -1;
					}
					return 1;
				case TypeCode.Int64:
					if (Math.Sign((long)second) >= 0)
					{
						return -1;
					}
					return 1;
				case TypeCode.Single:
					if (Math.Sign((float)second) >= 0)
					{
						return -1;
					}
					return 1;
				case TypeCode.Double:
					if (Math.Sign((double)second) >= 0)
					{
						return -1;
					}
					return 1;
				case TypeCode.Decimal:
					if (Math.Sign((decimal)second) >= 0)
					{
						return -1;
					}
					return 1;
				}
				return -1;
			}
			else
			{
				if (second == null)
				{
					switch (LanguagePrimitives.GetTypeCode(first.GetType()))
					{
					case TypeCode.SByte:
						if (Math.Sign((sbyte)first) >= 0)
						{
							return 1;
						}
						return -1;
					case TypeCode.Int16:
						if (Math.Sign((short)first) >= 0)
						{
							return 1;
						}
						return -1;
					case TypeCode.Int32:
						if (Math.Sign((int)first) >= 0)
						{
							return 1;
						}
						return -1;
					case TypeCode.Int64:
						if (Math.Sign((long)first) >= 0)
						{
							return 1;
						}
						return -1;
					case TypeCode.Single:
						if (Math.Sign((float)first) >= 0)
						{
							return 1;
						}
						return -1;
					case TypeCode.Double:
						if (Math.Sign((double)first) >= 0)
						{
							return 1;
						}
						return -1;
					case TypeCode.Decimal:
						if (Math.Sign((decimal)first) >= 0)
						{
							return 1;
						}
						return -1;
					}
					return 1;
				}
				string text = first as string;
				if (text != null)
				{
					string text2 = second as string;
					if (text2 == null)
					{
						try
						{
							text2 = (string)LanguagePrimitives.ConvertTo(second, typeof(string), cultureInfo);
						}
						catch (PSInvalidCastException ex)
						{
							throw PSTraceSource.NewArgumentException("second", ExtendedTypeSystem.ComparisonFailure, new object[]
							{
								first.ToString(),
								second.ToString(),
								ex.Message
							});
						}
					}
					return cultureInfo.CompareInfo.Compare(text, text2, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
				}
				Type type = first.GetType();
				Type type2 = second.GetType();
				int num = LanguagePrimitives.TypeTableIndex(type);
				int num2 = LanguagePrimitives.TypeTableIndex(type2);
				if (num != -1 && num2 != -1)
				{
					return LanguagePrimitives.NumericCompare(first, second, num, num2);
				}
				object obj;
				try
				{
					obj = LanguagePrimitives.ConvertTo(second, type, cultureInfo);
				}
				catch (PSInvalidCastException ex2)
				{
					throw PSTraceSource.NewArgumentException("second", ExtendedTypeSystem.ComparisonFailure, new object[]
					{
						first.ToString(),
						second.ToString(),
						ex2.Message
					});
				}
				IComparable comparable = first as IComparable;
				if (comparable != null)
				{
					return comparable.CompareTo(obj);
				}
				if (first.Equals(second))
				{
					return 0;
				}
				throw PSTraceSource.NewArgumentException("first", ExtendedTypeSystem.NotIcomparable, new object[]
				{
					first.ToString()
				});
			}
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00053F9C File Offset: 0x0005219C
		public static bool IsTrue(object obj)
		{
			if (obj == null || obj == AutomationNull.Value)
			{
				return false;
			}
			obj = PSObject.Base(obj);
			Type type = obj.GetType();
			if (type == typeof(bool))
			{
				return (bool)obj;
			}
			if (type == typeof(string))
			{
				return LanguagePrimitives.IsTrue((string)obj);
			}
			if (LanguagePrimitives.IsNumeric(LanguagePrimitives.GetTypeCode(type)))
			{
				LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.GetConversionData(type, typeof(bool)) ?? LanguagePrimitives.CacheConversion<bool>(type, typeof(bool), LanguagePrimitives.CreateNumericToBoolConverter(type), ConversionRank.Language);
				return (bool)conversionData.Invoke(obj, typeof(bool), false, null, null, null);
			}
			if (type == typeof(SwitchParameter))
			{
				return ((SwitchParameter)obj).ToBool();
			}
			IList list = obj as IList;
			return list == null || LanguagePrimitives.IsTrue(list);
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x00054087 File Offset: 0x00052287
		internal static bool IsTrue(string s)
		{
			return s.Length != 0;
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00054098 File Offset: 0x00052298
		internal static bool IsTrue(IList objectArray)
		{
			switch (objectArray.Count)
			{
			case 0:
				return false;
			case 1:
			{
				IList list = objectArray[0] as IList;
				if (list == null)
				{
					return LanguagePrimitives.IsTrue(objectArray[0]);
				}
				return list.Count >= 1;
			}
			default:
				return true;
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x000540E9 File Offset: 0x000522E9
		internal static bool IsNull(object obj)
		{
			return obj == null || obj == AutomationNull.Value;
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x000540F8 File Offset: 0x000522F8
		internal static PSObject AsPSObjectOrNull(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			return PSObject.AsPSObject(obj);
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00054108 File Offset: 0x00052308
		internal static int TypeTableIndex(Type type)
		{
			switch (LanguagePrimitives.GetTypeCode(type))
			{
			case TypeCode.SByte:
				return 6;
			case TypeCode.Byte:
				return 7;
			case TypeCode.Int16:
				return 0;
			case TypeCode.UInt16:
				return 3;
			case TypeCode.Int32:
				return 1;
			case TypeCode.UInt32:
				return 4;
			case TypeCode.Int64:
				return 2;
			case TypeCode.UInt64:
				return 5;
			case TypeCode.Single:
				return 8;
			case TypeCode.Double:
				return 9;
			case TypeCode.Decimal:
				return 10;
			default:
				return -1;
			}
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0005416C File Offset: 0x0005236C
		private static int NumericCompareDecimal(decimal decimalNumber, object otherNumber)
		{
			object obj = null;
			try
			{
				obj = Convert.ChangeType(otherNumber, typeof(decimal), CultureInfo.InvariantCulture);
			}
			catch (OverflowException)
			{
				try
				{
					double num = (double)Convert.ChangeType(decimalNumber, typeof(double), CultureInfo.InvariantCulture);
					double num2 = (double)Convert.ChangeType(otherNumber, typeof(double), CultureInfo.InvariantCulture);
					return ((IComparable)num).CompareTo(num2);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					return -1;
				}
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
				return -1;
			}
			return ((IComparable)decimalNumber).CompareTo(obj);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00054238 File Offset: 0x00052438
		private static int NumericCompare(object number1, object number2, int index1, int index2)
		{
			if (index1 == 10 && (index2 == 8 || index2 == 9))
			{
				return LanguagePrimitives.NumericCompareDecimal((decimal)number1, number2);
			}
			if (index2 == 10 && (index1 == 8 || index1 == 9))
			{
				return -LanguagePrimitives.NumericCompareDecimal((decimal)number2, number1);
			}
			Type conversionType = LanguagePrimitives.LargestTypeTable[index1][index2];
			object obj = Convert.ChangeType(number1, conversionType, CultureInfo.InvariantCulture);
			object obj2 = Convert.ChangeType(number2, conversionType, CultureInfo.InvariantCulture);
			return ((IComparable)obj).CompareTo(obj2);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x000542AC File Offset: 0x000524AC
		internal static TypeCode GetTypeCode(Type type)
		{
			return type.GetTypeCode();
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x000542B4 File Offset: 0x000524B4
		internal static T FromObjectAs<T>(object castObject)
		{
			T result = default(T);
			PSObject psobject = castObject as PSObject;
			if (psobject == null)
			{
				try
				{
					return (T)((object)castObject);
				}
				catch (InvalidCastException)
				{
					return default(T);
				}
			}
			try
			{
				result = (T)((object)psobject.BaseObject);
			}
			catch (InvalidCastException)
			{
				result = default(T);
			}
			return result;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00054320 File Offset: 0x00052520
		internal static bool IsSignedInteger(TypeCode typeCode)
		{
			return (LanguagePrimitives.typeCodeTraits[(int)typeCode] & LanguagePrimitives.TypeCodeTraits.SignedInteger) != LanguagePrimitives.TypeCodeTraits.None;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00054331 File Offset: 0x00052531
		internal static bool IsUnsignedInteger(TypeCode typeCode)
		{
			return (LanguagePrimitives.typeCodeTraits[(int)typeCode] & LanguagePrimitives.TypeCodeTraits.UnsignedInteger) != LanguagePrimitives.TypeCodeTraits.None;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00054342 File Offset: 0x00052542
		internal static bool IsInteger(TypeCode typeCode)
		{
			return (LanguagePrimitives.typeCodeTraits[(int)typeCode] & LanguagePrimitives.TypeCodeTraits.Integer) != LanguagePrimitives.TypeCodeTraits.None;
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00054353 File Offset: 0x00052553
		internal static bool IsFloating(TypeCode typeCode)
		{
			return (LanguagePrimitives.typeCodeTraits[(int)typeCode] & LanguagePrimitives.TypeCodeTraits.Floating) != LanguagePrimitives.TypeCodeTraits.None;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00054364 File Offset: 0x00052564
		internal static bool IsNumeric(TypeCode typeCode)
		{
			return (LanguagePrimitives.typeCodeTraits[(int)typeCode] & LanguagePrimitives.TypeCodeTraits.Numeric) != LanguagePrimitives.TypeCodeTraits.None;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00054376 File Offset: 0x00052576
		internal static bool IsCimIntrinsicScalarType(TypeCode typeCode)
		{
			return (LanguagePrimitives.typeCodeTraits[(int)typeCode] & LanguagePrimitives.TypeCodeTraits.CimIntrinsicType) != LanguagePrimitives.TypeCodeTraits.None;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00054388 File Offset: 0x00052588
		internal static bool IsCimIntrinsicScalarType(Type type)
		{
			TypeCode typeCode = LanguagePrimitives.GetTypeCode(type);
			return (LanguagePrimitives.IsCimIntrinsicScalarType(typeCode) && !type.GetTypeInfo().IsEnum) || type == typeof(TimeSpan);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x000543C8 File Offset: 0x000525C8
		internal static bool IsBooleanType(Type type)
		{
			return type == typeof(bool) || type == typeof(bool?);
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x000543F1 File Offset: 0x000525F1
		internal static bool IsSwitchParameterType(Type type)
		{
			return type == typeof(SwitchParameter) || type == typeof(SwitchParameter?);
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0005441A File Offset: 0x0005261A
		internal static bool IsBoolOrSwitchParameterType(Type type)
		{
			return LanguagePrimitives.IsBooleanType(type) || LanguagePrimitives.IsSwitchParameterType(type);
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00054430 File Offset: 0x00052630
		internal static void DoConversionsForSetInGenericDictionary(IDictionary dictionary, ref object key, ref object value)
		{
			foreach (Type type in dictionary.GetType().GetInterfaces())
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IDictionary<, >))
				{
					Type[] genericArguments = type.GetGenericArguments();
					key = LanguagePrimitives.ConvertTo(key, genericArguments[0], CultureInfo.InvariantCulture);
					value = LanguagePrimitives.ConvertTo(value, genericArguments[1], CultureInfo.InvariantCulture);
				}
			}
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x000544B0 File Offset: 0x000526B0
		private static TypeConverter GetIntegerSystemConverter(Type type)
		{
			if (type == typeof(short))
			{
				return new Int16Converter();
			}
			if (type == typeof(int))
			{
				return new Int32Converter();
			}
			if (type == typeof(long))
			{
				return new Int64Converter();
			}
			if (type == typeof(ushort))
			{
				return new UInt16Converter();
			}
			if (type == typeof(uint))
			{
				return new UInt32Converter();
			}
			if (type == typeof(ulong))
			{
				return new UInt64Converter();
			}
			if (type == typeof(byte))
			{
				return new ByteConverter();
			}
			if (type == typeof(sbyte))
			{
				return new SByteConverter();
			}
			return null;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00054580 File Offset: 0x00052780
		internal static object GetConverter(Type type, TypeTable backupTypeTable)
		{
			object obj = null;
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS != null)
			{
				LanguagePrimitives.tracer.WriteLine("ecFromTLS != null", new object[0]);
				obj = executionContextFromTLS.TypeTable.GetTypeConverter(type.FullName);
			}
			if (obj == null && backupTypeTable != null)
			{
				LanguagePrimitives.tracer.WriteLine("Using provided TypeTable to get the type converter", new object[0]);
				obj = backupTypeTable.GetTypeConverter(type.FullName);
			}
			if (obj != null)
			{
				LanguagePrimitives.tracer.WriteLine("typesXmlConverter != null", new object[0]);
				return obj;
			}
			object[] customAttributes = type.GetTypeInfo().GetCustomAttributes(typeof(TypeConverterAttribute), false);
			object[] array = customAttributes;
			int num = 0;
			if (num >= array.Length)
			{
				return null;
			}
			object obj2 = array[num];
			TypeConverterAttribute typeConverterAttribute = (TypeConverterAttribute)obj2;
			string converterTypeName = typeConverterAttribute.ConverterTypeName;
			LanguagePrimitives.typeConversion.WriteLine("{0}'s TypeConverterAttribute points to {1}.", new object[]
			{
				type,
				converterTypeName
			});
			return LanguagePrimitives.NewConverterInstance(converterTypeName);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00054674 File Offset: 0x00052874
		private static object NewConverterInstance(string assemblyQualifiedTypeName)
		{
			int num = assemblyQualifiedTypeName.IndexOf(",", StringComparison.Ordinal);
			if (num == -1)
			{
				LanguagePrimitives.typeConversion.WriteLine("Type name \"{0}\" should be assembly qualified.", new object[]
				{
					assemblyQualifiedTypeName
				});
				return null;
			}
			string text = assemblyQualifiedTypeName.Substring(num + 2);
			string text2 = assemblyQualifiedTypeName.Substring(0, num);
			foreach (Assembly assembly in ClrFacade.GetAssemblies(text2))
			{
				if (assembly.FullName == text)
				{
					Type type = null;
					try
					{
						type = assembly.GetType(text2, false, false);
					}
					catch (ArgumentException ex)
					{
						LanguagePrimitives.typeConversion.WriteLine("Assembly \"{0}\" threw an exception when retrieving the type \"{1}\": \"{2}\".", new object[]
						{
							text,
							text2,
							ex.Message
						});
						return null;
					}
					try
					{
						return Activator.CreateInstance(type);
					}
					catch (Exception ex2)
					{
						CommandProcessorBase.CheckForSevereException(ex2);
						TargetInvocationException ex3 = ex2 as TargetInvocationException;
						string text3 = (ex3 == null || ex3.InnerException == null) ? ex2.Message : ex3.InnerException.Message;
						LanguagePrimitives.typeConversion.WriteLine("Creating an instance of type \"{0}\" caused an exception to be thrown: \"{1}\"", new object[]
						{
							assemblyQualifiedTypeName,
							text3
						});
						return null;
					}
				}
			}
			LanguagePrimitives.typeConversion.WriteLine("Could not create an instance of type \"{0}\".", new object[]
			{
				assemblyQualifiedTypeName
			});
			return null;
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0005480C File Offset: 0x00052A0C
		public static string ConvertTypeNameToPSTypeName(string typeName)
		{
			if (string.IsNullOrWhiteSpace(typeName))
			{
				return "[object]";
			}
			string arg;
			if (LanguagePrimitives.nameMap.TryGetValue(typeName, out arg))
			{
				return '[' + arg + ']';
			}
			Type valueToConvert;
			if (TypeResolver.TryResolveType(typeName, out valueToConvert))
			{
				return '[' + LanguagePrimitives.ConvertTo<string>(valueToConvert) + ']';
			}
			return "[UnknownType:" + typeName + ']';
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00054881 File Offset: 0x00052A81
		public static object ConvertTo(object valueToConvert, Type resultType)
		{
			return LanguagePrimitives.ConvertTo(valueToConvert, resultType, true, CultureInfo.InvariantCulture, null);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00054891 File Offset: 0x00052A91
		public static object ConvertTo(object valueToConvert, Type resultType, IFormatProvider formatProvider)
		{
			return LanguagePrimitives.ConvertTo(valueToConvert, resultType, true, formatProvider, null);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x000548A0 File Offset: 0x00052AA0
		public static object ConvertPSObjectToType(PSObject valueToConvert, Type resultType, bool recursion, IFormatProvider formatProvider, bool ignoreUnknownMembers)
		{
			if (valueToConvert != null)
			{
				ConstructorInfo constructor = resultType.GetConstructor(PSTypeExtensions.EmptyTypes);
				LanguagePrimitives.ConvertViaNoArgumentConstructor convertViaNoArgumentConstructor = new LanguagePrimitives.ConvertViaNoArgumentConstructor(constructor, resultType);
				return convertViaNoArgumentConstructor.Convert(PSObject.Base(valueToConvert), resultType, recursion, valueToConvert, formatProvider, null, ignoreUnknownMembers);
			}
			return null;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x000548D9 File Offset: 0x00052AD9
		public static T ConvertTo<T>(object valueToConvert)
		{
			return (T)((object)LanguagePrimitives.ConvertTo(valueToConvert, typeof(T), true, CultureInfo.InvariantCulture, null));
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x000548F7 File Offset: 0x00052AF7
		public static bool TryConvertTo<T>(object valueToConvert, out T result)
		{
			return LanguagePrimitives.TryConvertTo<T>(valueToConvert, CultureInfo.InvariantCulture, out result);
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00054908 File Offset: 0x00052B08
		public static bool TryConvertTo<T>(object valueToConvert, IFormatProvider formatProvider, out T result)
		{
			result = default(T);
			try
			{
				result = (T)((object)LanguagePrimitives.ConvertTo(valueToConvert, typeof(T), formatProvider));
			}
			catch (InvalidCastException)
			{
				return false;
			}
			catch (ArgumentException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00054964 File Offset: 0x00052B64
		public static bool TryConvertTo(object valueToConvert, Type resultType, out object result)
		{
			return LanguagePrimitives.TryConvertTo(valueToConvert, resultType, CultureInfo.InvariantCulture, out result);
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00054974 File Offset: 0x00052B74
		public static bool TryConvertTo(object valueToConvert, Type resultType, IFormatProvider formatProvider, out object result)
		{
			result = null;
			try
			{
				result = LanguagePrimitives.ConvertTo(valueToConvert, resultType, formatProvider);
			}
			catch (InvalidCastException)
			{
				return false;
			}
			catch (ArgumentException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x000549BC File Offset: 0x00052BBC
		private static MethodInfo FindCastOperator(string methodName, Type targetType, Type originalType, Type resultType)
		{
			MethodInfo result;
			using (LanguagePrimitives.typeConversion.TraceScope("Looking for \"{0}\" cast operator.", new object[]
			{
				methodName
			}))
			{
				MemberInfo[] methods = ClrFacade.GetMethods(targetType, methodName);
				foreach (MethodInfo methodInfo in methods)
				{
					if (resultType.IsAssignableFrom(methodInfo.ReturnType))
					{
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(originalType))
						{
							LanguagePrimitives.typeConversion.WriteLine("Found \"{0}\" cast operator in type {1}.", new object[]
							{
								methodName,
								targetType.FullName
							});
							return methodInfo;
						}
					}
				}
				LanguagePrimitives.typeConversion.TraceScope("Cast operator for \"{0}\" not found.", new object[]
				{
					methodName
				});
				result = null;
			}
			return result;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00054AAC File Offset: 0x00052CAC
		private static object ConvertNumericThroughDouble(object valueToConvert, Type resultType)
		{
			object result;
			using (LanguagePrimitives.typeConversion.TraceScope("Numeric Conversion through System.Double.", new object[0]))
			{
				object value = Convert.ChangeType(valueToConvert, typeof(double), CultureInfo.InvariantCulture.NumberFormat);
				result = Convert.ChangeType(value, resultType, CultureInfo.InvariantCulture.NumberFormat);
			}
			return result;
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00054B1C File Offset: 0x00052D1C
		private static ManagementObject ConvertToWMI(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Standard type conversion to a ManagementObject.", new object[0]);
			string text;
			try
			{
				text = PSObject.ToString(null, valueToConvert, "\n", null, null, true, true);
			}
			catch (ExtendedTypeSystemException ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting value to string: {0}", new object[]
				{
					ex.Message
				});
				throw new PSInvalidCastException("InvalidCastGetStringToWMI", ex, ExtendedTypeSystem.InvalidCastExceptionNoStringForConversion, new object[]
				{
					resultType.ToString(),
					ex.Message
				});
			}
			ManagementObject result;
			try
			{
				ManagementObject managementObject = new ManagementObject(text);
				if (managementObject.SystemProperties["__CLASS"] == null)
				{
					string message = StringUtil.Format(ExtendedTypeSystem.InvalidWMIPath, text);
					throw new PSInvalidCastException(message);
				}
				result = managementObject;
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				LanguagePrimitives.typeConversion.WriteLine("Exception creating WMI object: \"{0}\".", new object[]
				{
					ex2.Message
				});
				throw new PSInvalidCastException("InvalidCastToWMI", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex2.Message
				});
			}
			return result;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00054C5C File Offset: 0x00052E5C
		private static ManagementObjectSearcher ConvertToWMISearcher(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Standard type conversion to a collection of ManagementObjects.", new object[0]);
			string queryString;
			try
			{
				queryString = PSObject.ToString(null, valueToConvert, "\n", null, null, true, true);
			}
			catch (ExtendedTypeSystemException ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting value to string: {0}", new object[]
				{
					ex.Message
				});
				throw new PSInvalidCastException("InvalidCastGetStringToWMISearcher", ex, ExtendedTypeSystem.InvalidCastExceptionNoStringForConversion, new object[]
				{
					resultType.ToString(),
					ex.Message
				});
			}
			ManagementObjectSearcher result;
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
				result = managementObjectSearcher;
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				LanguagePrimitives.typeConversion.WriteLine("Exception running WMI object query: \"{0}\".", new object[]
				{
					ex2.Message
				});
				throw new PSInvalidCastException("InvalidCastToWMISearcher", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex2.Message
				});
			}
			return result;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00054D74 File Offset: 0x00052F74
		private static ManagementClass ConvertToWMIClass(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Standard type conversion to a ManagementClass.", new object[0]);
			string text;
			try
			{
				text = PSObject.ToString(null, valueToConvert, "\n", null, null, true, true);
			}
			catch (ExtendedTypeSystemException ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting value to string: {0}", new object[]
				{
					ex.Message
				});
				throw new PSInvalidCastException("InvalidCastGetStringToWMIClass", ex, ExtendedTypeSystem.InvalidCastExceptionNoStringForConversion, new object[]
				{
					resultType.ToString(),
					ex.Message
				});
			}
			ManagementClass result;
			try
			{
				ManagementClass managementClass = new ManagementClass(text);
				if (managementClass.SystemProperties["__CLASS"] == null)
				{
					string message = StringUtil.Format(ExtendedTypeSystem.InvalidWMIClassPath, text);
					throw new PSInvalidCastException(message);
				}
				result = managementClass;
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				LanguagePrimitives.typeConversion.WriteLine("Exception creating WMI class: \"{0}\".", new object[]
				{
					ex2.Message
				});
				throw new PSInvalidCastException("InvalidCastToWMIClass", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex2.Message
				});
			}
			return result;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00054EB4 File Offset: 0x000530B4
		private static CommaDelimitedStringCollection ConvertToCommaDelimitedStringCollection(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Standard type conversion to a CommaDelimitedStringCollection.", new object[0]);
			CommaDelimitedStringCollection commaDelimitedStringCollection = new CommaDelimitedStringCollection();
			LanguagePrimitives.AddItemsToCollection(valueToConvert, resultType, formatProvider, backupTable, commaDelimitedStringCollection);
			return commaDelimitedStringCollection;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00054EEC File Offset: 0x000530EC
		private static DirectoryEntry ConvertToADSI(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Standard type conversion to  DirectoryEntry.", new object[0]);
			string path;
			try
			{
				path = PSObject.ToString(null, valueToConvert, "\n", null, null, true, true);
			}
			catch (ExtendedTypeSystemException ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting value to string: {0}", new object[]
				{
					ex.Message
				});
				throw new PSInvalidCastException("InvalidCastGetStringToADSIClass", ex, ExtendedTypeSystem.InvalidCastExceptionNoStringForConversion, new object[]
				{
					resultType.ToString(),
					ex.Message
				});
			}
			DirectoryEntry result;
			try
			{
				DirectoryEntry directoryEntry = new DirectoryEntry(path);
				result = directoryEntry;
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				LanguagePrimitives.typeConversion.WriteLine("Exception creating ADSI class: \"{0}\".", new object[]
				{
					ex2.Message
				});
				throw new PSInvalidCastException("InvalidCastToADSIClass", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex2.Message
				});
			}
			return result;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00055004 File Offset: 0x00053204
		private static DirectorySearcher ConvertToADSISearcher(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Standard type conversion to ADSISearcher", new object[0]);
			DirectorySearcher result;
			try
			{
				result = new DirectorySearcher((string)valueToConvert);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				LanguagePrimitives.typeConversion.WriteLine("Exception creating ADSI searcher: \"{0}\".", new object[]
				{
					ex.Message
				});
				throw new PSInvalidCastException("InvalidCastToADSISearcher", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x000550A0 File Offset: 0x000532A0
		private static StringCollection ConvertToStringCollection(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Standard type conversion to a StringCollection.", new object[0]);
			StringCollection stringCollection = new StringCollection();
			LanguagePrimitives.AddItemsToCollection(valueToConvert, resultType, formatProvider, backupTable, stringCollection);
			return stringCollection;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x000550D8 File Offset: 0x000532D8
		private static void AddItemsToCollection(object valueToConvert, Type resultType, IFormatProvider formatProvider, TypeTable backupTable, StringCollection stringCollection)
		{
			try
			{
				string[] value = (string[])LanguagePrimitives.ConvertTo(valueToConvert, typeof(string[]), false, formatProvider, backupTable);
				stringCollection.AddRange(value);
			}
			catch (PSInvalidCastException)
			{
				LanguagePrimitives.typeConversion.WriteLine("valueToConvert contains non-string type values", new object[0]);
				ArgumentException ex = new ArgumentException(StringUtil.Format(ExtendedTypeSystem.CannotConvertValueToStringArray, valueToConvert.ToString()));
				throw new PSInvalidCastException(StringUtil.Format("InvalidCastTo{0}Class", resultType.Name), ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				LanguagePrimitives.typeConversion.WriteLine("Exception creating StringCollection class: \"{0}\".", new object[]
				{
					ex2.Message
				});
				throw new PSInvalidCastException(StringUtil.Format("InvalidCastTo{0}Class", resultType.Name), ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex2.Message
				});
			}
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00055200 File Offset: 0x00053400
		private static XmlDocument ConvertToXml(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			XmlDocument result;
			using (LanguagePrimitives.typeConversion.TraceScope("Standard type conversion to XmlDocument.", new object[0]))
			{
				string s;
				try
				{
					s = PSObject.ToString(null, valueToConvert, "\n", null, null, true, true);
				}
				catch (ExtendedTypeSystemException ex)
				{
					LanguagePrimitives.typeConversion.WriteLine("Exception converting value to string: {0}", new object[]
					{
						ex.Message
					});
					throw new PSInvalidCastException("InvalidCastGetStringToXmlDocument", ex, ExtendedTypeSystem.InvalidCastExceptionNoStringForConversion, new object[]
					{
						resultType.ToString(),
						ex.Message
					});
				}
				try
				{
					using (TextReader textReader = new StringReader(s))
					{
						XmlReaderSettings xmlReaderSettings = InternalDeserializer.XmlReaderSettingsForUntrustedXmlDocument.Clone();
						xmlReaderSettings.IgnoreWhitespace = true;
						xmlReaderSettings.IgnoreProcessingInstructions = false;
						xmlReaderSettings.IgnoreComments = false;
						XmlReader reader = XmlReader.Create(textReader, xmlReaderSettings);
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.PreserveWhitespace = false;
						xmlDocument.Load(reader);
						result = xmlDocument;
					}
				}
				catch (Exception ex2)
				{
					LanguagePrimitives.typeConversion.WriteLine("Exception loading XML: \"{0}\".", new object[]
					{
						ex2.Message
					});
					CommandProcessorBase.CheckForSevereException(ex2);
					throw new PSInvalidCastException("InvalidCastToXmlDocument", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex2.Message
					});
				}
			}
			return result;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x000553BC File Offset: 0x000535BC
		private static CultureInfo GetCultureFromFormatProvider(IFormatProvider formatProvider)
		{
			CultureInfo cultureInfo = formatProvider as CultureInfo;
			if (cultureInfo == null)
			{
				cultureInfo = CultureInfo.InvariantCulture;
			}
			return cultureInfo;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x000553DC File Offset: 0x000535DC
		private static bool IsCustomTypeConversion(object valueToConvert, Type resultType, IFormatProvider formatProvider, out object result, TypeTable backupTypeTable)
		{
			bool result2;
			using (LanguagePrimitives.typeConversion.TraceScope("Custom type conversion.", new object[0]))
			{
				object obj = PSObject.Base(valueToConvert);
				Type type = obj.GetType();
				object converter = LanguagePrimitives.GetConverter(type, backupTypeTable);
				if (converter != null)
				{
					TypeConverter typeConverter = converter as TypeConverter;
					if (typeConverter != null)
					{
						LanguagePrimitives.typeConversion.WriteLine("Original type's converter is TypeConverter.", new object[0]);
						if (typeConverter.CanConvertTo(resultType))
						{
							LanguagePrimitives.typeConversion.WriteLine("TypeConverter can convert to resultType.", new object[0]);
							try
							{
								result = typeConverter.ConvertTo(null, LanguagePrimitives.GetCultureFromFormatProvider(formatProvider), obj, resultType);
								return true;
							}
							catch (Exception ex)
							{
								LanguagePrimitives.typeConversion.WriteLine("Exception converting with Original type's TypeConverter: \"{0}\".", new object[]
								{
									ex.Message
								});
								CommandProcessorBase.CheckForSevereException(ex);
								throw new PSInvalidCastException("InvalidCastTypeConvertersConvertTo", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
								{
									valueToConvert.ToString(),
									resultType.ToString(),
									ex.Message
								});
							}
						}
						LanguagePrimitives.typeConversion.WriteLine("TypeConverter cannot convert to resultType.", new object[0]);
					}
					PSTypeConverter pstypeConverter = converter as PSTypeConverter;
					if (pstypeConverter != null)
					{
						LanguagePrimitives.typeConversion.WriteLine("Original type's converter is PSTypeConverter.", new object[0]);
						PSObject sourceValue = PSObject.AsPSObject(valueToConvert);
						if (pstypeConverter.CanConvertTo(sourceValue, resultType))
						{
							LanguagePrimitives.typeConversion.WriteLine("Original type's PSTypeConverter can convert to resultType.", new object[0]);
							try
							{
								result = pstypeConverter.ConvertTo(sourceValue, resultType, formatProvider, true);
								return true;
							}
							catch (Exception ex2)
							{
								LanguagePrimitives.typeConversion.WriteLine("Exception converting with Original type's PSTypeConverter: \"{0}\".", new object[]
								{
									ex2.Message
								});
								CommandProcessorBase.CheckForSevereException(ex2);
								throw new PSInvalidCastException("InvalidCastPSTypeConvertersConvertTo", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
								{
									valueToConvert.ToString(),
									resultType.ToString(),
									ex2.Message
								});
							}
						}
						LanguagePrimitives.typeConversion.WriteLine("Original type's PSTypeConverter cannot convert to resultType.", new object[0]);
					}
				}
				LanguagePrimitives.tracer.WriteLine("No converter found in original type.", new object[0]);
				converter = LanguagePrimitives.GetConverter(resultType, backupTypeTable);
				if (converter != null)
				{
					TypeConverter typeConverter2 = converter as TypeConverter;
					if (typeConverter2 != null)
					{
						LanguagePrimitives.typeConversion.WriteLine("Destination type's converter is TypeConverter that can convert from originalType.", new object[0]);
						if (typeConverter2.CanConvertFrom(type))
						{
							LanguagePrimitives.typeConversion.WriteLine("Destination type's converter can convert from originalType.", new object[0]);
							try
							{
								result = typeConverter2.ConvertFrom(null, LanguagePrimitives.GetCultureFromFormatProvider(formatProvider), obj);
								return true;
							}
							catch (Exception ex3)
							{
								LanguagePrimitives.typeConversion.WriteLine("Exception converting with Destination type's TypeConverter: \"{0}\".", new object[]
								{
									ex3.Message
								});
								CommandProcessorBase.CheckForSevereException(ex3);
								throw new PSInvalidCastException("InvalidCastTypeConvertersConvertFrom", ex3, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
								{
									valueToConvert.ToString(),
									resultType.ToString(),
									ex3.Message
								});
							}
						}
						LanguagePrimitives.typeConversion.WriteLine("Destination type's converter cannot convert from originalType.", new object[0]);
					}
					PSTypeConverter pstypeConverter2 = converter as PSTypeConverter;
					if (pstypeConverter2 != null)
					{
						LanguagePrimitives.typeConversion.WriteLine("Destination type's converter is PSTypeConverter.", new object[0]);
						PSObject sourceValue2 = PSObject.AsPSObject(valueToConvert);
						if (pstypeConverter2.CanConvertFrom(sourceValue2, resultType))
						{
							LanguagePrimitives.typeConversion.WriteLine("Destination type's converter can convert from originalType.", new object[0]);
							try
							{
								result = pstypeConverter2.ConvertFrom(sourceValue2, resultType, formatProvider, true);
								return true;
							}
							catch (Exception ex4)
							{
								LanguagePrimitives.typeConversion.WriteLine("Exception converting with Destination type's PSTypeConverter: \"{0}\".", new object[]
								{
									ex4.Message
								});
								CommandProcessorBase.CheckForSevereException(ex4);
								throw new PSInvalidCastException("InvalidCastPSTypeConvertersConvertFrom", ex4, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
								{
									valueToConvert.ToString(),
									resultType.ToString(),
									ex4.Message
								});
							}
						}
						LanguagePrimitives.typeConversion.WriteLine("Destination type's converter cannot convert from originalType.", new object[0]);
					}
				}
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00055850 File Offset: 0x00053A50
		private static object ConvertNumericChar(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			object result;
			try
			{
				object obj = Convert.ChangeType(Convert.ChangeType(valueToConvert, typeof(int), formatProvider), resultType, formatProvider);
				LanguagePrimitives.typeConversion.WriteLine("Numeric conversion succeeded.", new object[0]);
				result = obj;
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting with IConvertible: \"{0}\".", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				throw new PSInvalidCastException("InvalidCastIConvertible", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00055904 File Offset: 0x00053B04
		private static object ConvertNumeric(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			object result;
			try
			{
				object obj = Convert.ChangeType(valueToConvert, resultType, formatProvider);
				LanguagePrimitives.typeConversion.WriteLine("Numeric conversion succeeded.", new object[0]);
				result = obj;
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting with IConvertible: \"{0}\".", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				throw new PSInvalidCastException("InvalidCastIConvertible", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x000559A8 File Offset: 0x00053BA8
		private static char[] ConvertStringToCharArray(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Returning value to convert's ToCharArray().", new object[0]);
			return ((string)valueToConvert).ToCharArray();
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x000559CC File Offset: 0x00053BCC
		private static Regex ConvertStringToRegex(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Returning new RegEx(value to convert).", new object[0]);
			Regex result;
			try
			{
				result = new Regex((string)valueToConvert);
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception in RegEx constructor: \"{0}\".", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				throw new PSInvalidCastException("InvalidCastFromStringToRegex", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00055A68 File Offset: 0x00053C68
		private static CimSession ConvertStringToCimSession(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Returning CimSession.Create(value to convert).", new object[0]);
			CimSession result;
			try
			{
				result = CimSession.Create((string)valueToConvert);
			}
			catch (CimException ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception in CimSession.Create: \"{0}\".", new object[]
				{
					ex.Message
				});
				throw new PSInvalidCastException("InvalidCastFromStringToCimSession", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00055B00 File Offset: 0x00053D00
		private static Type ConvertStringToType(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			Exception innerException;
			Type type = TypeResolver.ResolveType((string)valueToConvert, out innerException);
			if (type == null)
			{
				throw new PSInvalidCastException("InvalidCastFromStringToType", innerException, ExtendedTypeSystem.InvalidCastException, new object[]
				{
					valueToConvert.ToString(),
					LanguagePrimitives.ObjectToTypeNameString(valueToConvert),
					resultType.ToString()
				});
			}
			return type;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x00055B5C File Offset: 0x00053D5C
		private static object ConvertStringToInteger(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			string text = valueToConvert as string;
			if (text.Length == 0)
			{
				LanguagePrimitives.typeConversion.WriteLine("Returning numeric zero.", new object[0]);
				return Convert.ChangeType(0, resultType, CultureInfo.InvariantCulture);
			}
			LanguagePrimitives.typeConversion.WriteLine("Converting to integer.", new object[0]);
			TypeConverter integerSystemConverter = LanguagePrimitives.GetIntegerSystemConverter(resultType);
			object result;
			try
			{
				result = integerSystemConverter.ConvertFrom(text);
			}
			catch (Exception innerException)
			{
				CommandProcessorBase.CheckForSevereException(innerException);
				if (innerException.InnerException != null)
				{
					innerException = innerException.InnerException;
				}
				LanguagePrimitives.typeConversion.WriteLine("Exception converting to integer: \"{0}\".", new object[]
				{
					innerException.Message
				});
				CommandProcessorBase.CheckForSevereException(innerException);
				if (innerException is FormatException)
				{
					LanguagePrimitives.typeConversion.WriteLine("Converting to integer passing through double.", new object[0]);
					try
					{
						return LanguagePrimitives.ConvertNumericThroughDouble(text, resultType);
					}
					catch (Exception ex)
					{
						LanguagePrimitives.typeConversion.WriteLine("Exception converting to integer through double: \"{0}\".", new object[]
						{
							ex.Message
						});
						CommandProcessorBase.CheckForSevereException(ex);
					}
				}
				throw new PSInvalidCastException("InvalidCastFromStringToInteger", innerException, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					text,
					resultType.ToString(),
					innerException.Message
				});
			}
			return result;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00055CB0 File Offset: 0x00053EB0
		private static object ConvertStringToDecimal(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			if (((string)valueToConvert).Length == 0)
			{
				LanguagePrimitives.typeConversion.WriteLine("Returning numeric zero.", new object[0]);
				return Convert.ChangeType(0, resultType, CultureInfo.InvariantCulture);
			}
			LanguagePrimitives.typeConversion.WriteLine("Converting to decimal.", new object[0]);
			object result;
			try
			{
				result = Convert.ChangeType(valueToConvert, resultType, CultureInfo.InvariantCulture.NumberFormat);
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting to decimal: \"{0}\". Converting to decimal passing through double.", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				if (ex is FormatException)
				{
					try
					{
						return LanguagePrimitives.ConvertNumericThroughDouble(valueToConvert, resultType);
					}
					catch (Exception ex2)
					{
						LanguagePrimitives.typeConversion.WriteLine("Exception converting to integer through double: \"{0}\".", new object[]
						{
							ex2.Message
						});
						CommandProcessorBase.CheckForSevereException(ex2);
					}
				}
				throw new PSInvalidCastException("InvalidCastFromStringToDecimal", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00055DD8 File Offset: 0x00053FD8
		private static object ConvertStringToReal(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			if (((string)valueToConvert).Length == 0)
			{
				LanguagePrimitives.typeConversion.WriteLine("Returning numeric zero.", new object[0]);
				return Convert.ChangeType(0, resultType, CultureInfo.InvariantCulture);
			}
			LanguagePrimitives.typeConversion.WriteLine("Converting to double or single.", new object[0]);
			object result;
			try
			{
				result = Convert.ChangeType(valueToConvert, resultType, CultureInfo.InvariantCulture.NumberFormat);
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting to double or single: \"{0}\".", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				throw new PSInvalidCastException("InvalidCastFromStringToDoubleOrSingle", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00055EB0 File Offset: 0x000540B0
		private static object ConvertAssignableFrom(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Result type is assignable from value to convert's type", new object[0]);
			return valueToConvert;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00055EC8 File Offset: 0x000540C8
		private static PSObject ConvertToPSObject(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Returning PSObject.AsPSObject(valueToConvert).", new object[0]);
			return PSObject.AsPSObject(valueToConvert);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00055EE5 File Offset: 0x000540E5
		private static object ConvertToVoid(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("returning AutomationNull.Value.", new object[0]);
			return AutomationNull.Value;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00055F01 File Offset: 0x00054101
		private static bool ConvertClassToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting ref to boolean.", new object[0]);
			return valueToConvert != null;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00055F1F File Offset: 0x0005411F
		private static bool ConvertValueToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting value to boolean.", new object[0]);
			return true;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00055F37 File Offset: 0x00054137
		private static bool ConvertStringToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting string to boolean.", new object[0]);
			return LanguagePrimitives.IsTrue((string)valueToConvert);
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00055F59 File Offset: 0x00054159
		private static bool ConvertInt16ToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (short)valueToConvert != 0;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00055F67 File Offset: 0x00054167
		private static bool ConvertInt32ToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (int)valueToConvert != 0;
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00055F75 File Offset: 0x00054175
		private static bool ConvertInt64ToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (long)valueToConvert != 0L;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00055F84 File Offset: 0x00054184
		private static bool ConvertUInt16ToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (ushort)valueToConvert != 0;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00055F92 File Offset: 0x00054192
		private static bool ConvertUInt32ToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (uint)valueToConvert != 0U;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00055FA0 File Offset: 0x000541A0
		private static bool ConvertUInt64ToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (ulong)valueToConvert != 0UL;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00055FAF File Offset: 0x000541AF
		private static bool ConvertSByteToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (sbyte)valueToConvert != 0;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00055FBD File Offset: 0x000541BD
		private static bool ConvertByteToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (byte)valueToConvert != 0;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00055FCB File Offset: 0x000541CB
		private static bool ConvertSingleToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (float)valueToConvert != 0f;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00055FDD File Offset: 0x000541DD
		private static bool ConvertDoubleToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (double)valueToConvert != 0.0;
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00055FF3 File Offset: 0x000541F3
		private static bool ConvertDecimalToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return (decimal)valueToConvert != 0m;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00056008 File Offset: 0x00054208
		private static LanguagePrimitives.PSConverter<bool> CreateNumericToBoolConverter(Type fromType)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
			ParameterExpression[] parameters = new ParameterExpression[]
			{
				parameterExpression,
				Expression.Parameter(typeof(Type)),
				Expression.Parameter(typeof(bool)),
				Expression.Parameter(typeof(PSObject)),
				Expression.Parameter(typeof(IFormatProvider)),
				Expression.Parameter(typeof(TypeTable))
			};
			return Expression.Lambda<LanguagePrimitives.PSConverter<bool>>(Expression.NotEqual(Expression.Default(fromType), parameterExpression.Cast(fromType)), parameters).Compile();
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x000560AC File Offset: 0x000542AC
		private static bool ConvertCharToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting char to boolean.", new object[0]);
			char c = (char)valueToConvert;
			return c != '\0';
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x000560DC File Offset: 0x000542DC
		private static bool ConvertSwitchParameterToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting SwitchParameter to boolean.", new object[0]);
			return ((SwitchParameter)valueToConvert).ToBool();
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0005610C File Offset: 0x0005430C
		private static bool ConvertIListToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting IList to boolean.", new object[0]);
			return LanguagePrimitives.IsTrue((IList)valueToConvert);
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00056130 File Offset: 0x00054330
		private static string ConvertNumericToString(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			if (originalValueToConvert != null && originalValueToConvert.TokenText != null)
			{
				return originalValueToConvert.TokenText;
			}
			LanguagePrimitives.typeConversion.WriteLine("Converting numeric to string.", new object[0]);
			string result;
			try
			{
				result = (string)Convert.ChangeType(valueToConvert, resultType, CultureInfo.InvariantCulture.NumberFormat);
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Converting numeric to string Exception: \"{0}\".", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				throw new PSInvalidCastException("InvalidCastFromNumericToString", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x000561EC File Offset: 0x000543EC
		private static string ConvertNonNumericToString(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			string result;
			try
			{
				LanguagePrimitives.typeConversion.WriteLine("Converting object to string.", new object[0]);
				result = PSObject.ToStringParser(executionContextFromTLS, valueToConvert);
			}
			catch (ExtendedTypeSystemException ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Converting object to string Exception: \"{0}\".", new object[]
				{
					ex.Message
				});
				throw new PSInvalidCastException("InvalidCastFromAnyTypeToString", ex, ExtendedTypeSystem.InvalidCastCannotRetrieveString, new object[0]);
			}
			return result;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00056268 File Offset: 0x00054468
		private static Hashtable ConvertIDictionaryToHashtable(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting to Hashtable.", new object[0]);
			return new Hashtable(valueToConvert as IDictionary);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0005628A File Offset: 0x0005448A
		private static PSReference ConvertToPSReference(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting to PSReference.", new object[0]);
			return PSReference.CreateInstance(valueToConvert, valueToConvert.GetType());
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x000562B0 File Offset: 0x000544B0
		private static Delegate ConvertScriptBlockToDelegate(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			Exception ex = null;
			try
			{
				return ((ScriptBlock)valueToConvert).GetDelegate(resultType);
			}
			catch (ArgumentNullException ex2)
			{
				ex = ex2;
			}
			catch (ArgumentException ex3)
			{
				ex = ex3;
			}
			catch (MissingMethodException ex4)
			{
				ex = ex4;
			}
			catch (MemberAccessException ex5)
			{
				ex = ex5;
			}
			LanguagePrimitives.typeConversion.WriteLine("Converting script block to delegate Exception: \"{0}\".", new object[]
			{
				ex.Message
			});
			throw new PSInvalidCastException("InvalidCastFromScriptBlockToDelegate", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
			{
				valueToConvert.ToString(),
				resultType.ToString(),
				ex.Message
			});
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00056378 File Offset: 0x00054578
		private static object ConvertToNullable(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return LanguagePrimitives.ConvertTo(valueToConvert, Nullable.GetUnderlyingType(resultType), recursion, formatProvider, backupTable);
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0005638C File Offset: 0x0005458C
		private static object ConvertRelatedArrays(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("The element type of result is assignable from the element type of the value to convert", new object[0]);
			Array array = (Array)valueToConvert;
			Array array2 = Array.CreateInstance(resultType.GetElementType(), array.Length);
			array.CopyTo(array2, 0);
			return array2;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x000563D0 File Offset: 0x000545D0
		private static object ConvertUnrelatedArrays(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			Array array = valueToConvert as Array;
			Type elementType = resultType.GetElementType();
			Array array2 = Array.CreateInstance(elementType, array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				object value = LanguagePrimitives.ConvertTo(array.GetValue(i), elementType, false, formatProvider, backupTable);
				array2.SetValue(value, i);
			}
			return array2;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00056428 File Offset: 0x00054628
		private static object ConvertEnumerableToArray(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			object result;
			try
			{
				ArrayList arrayList = new ArrayList();
				Type type = (resultType == typeof(Array)) ? typeof(object) : resultType.GetElementType();
				LanguagePrimitives.typeConversion.WriteLine("Converting elements in the value to convert to the result's element type.", new object[0]);
				foreach (object valueToConvert2 in LanguagePrimitives.GetEnumerable(valueToConvert))
				{
					arrayList.Add(LanguagePrimitives.ConvertTo(valueToConvert2, type, false, formatProvider, backupTable));
				}
				result = arrayList.ToArray(type);
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Element conversion exception: \"{0}\".", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				throw new PSInvalidCastException("InvalidCastExceptionEnumerableToArray", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0005654C File Offset: 0x0005474C
		private static object ConvertScalarToArray(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Value to convert is scalar.", new object[0]);
			if (originalValueToConvert != null && originalValueToConvert.TokenText != null)
			{
				valueToConvert = originalValueToConvert;
			}
			object result;
			try
			{
				Type type = (resultType == typeof(Array)) ? typeof(object) : resultType.GetElementType();
				result = new ArrayList
				{
					LanguagePrimitives.ConvertTo(valueToConvert, type, false, formatProvider, backupTable)
				}.ToArray(type);
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Element conversion exception: \"{0}\".", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				throw new PSInvalidCastException("InvalidCastExceptionScalarToArray", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x00056638 File Offset: 0x00054838
		private static object ConvertIntegerToEnum(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			object obj;
			try
			{
				obj = Enum.ToObject(resultType, valueToConvert);
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Integer to System.Enum exception: \"{0}\".", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				throw new PSInvalidCastException("InvalidCastExceptionIntegerToEnum", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			LanguagePrimitives.EnumSingleTypeConverter.ThrowForUndefinedEnum("UndefinedIntegerToEnum", obj, valueToConvert, resultType);
			return obj;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x000566C8 File Offset: 0x000548C8
		private static object ConvertStringToEnum(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			string text = valueToConvert as string;
			object obj = null;
			LanguagePrimitives.typeConversion.WriteLine("Calling case sensitive Enum.Parse", new object[0]);
			try
			{
				obj = Enum.Parse(resultType, text);
			}
			catch (ArgumentException ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Enum.Parse Exception: \"{0}\".", new object[]
				{
					ex.Message
				});
				try
				{
					LanguagePrimitives.typeConversion.WriteLine("Calling case insensitive Enum.Parse", new object[0]);
					obj = Enum.Parse(resultType, text, true);
				}
				catch (ArgumentException ex2)
				{
					LanguagePrimitives.typeConversion.WriteLine("Enum.Parse Exception: \"{0}\".", new object[]
					{
						ex2.Message
					});
				}
				catch (Exception ex3)
				{
					CommandProcessorBase.CheckForSevereException(ex3);
					LanguagePrimitives.typeConversion.WriteLine("Case insensitive Enum.Parse threw an exception.", new object[0]);
					throw new PSInvalidCastException("CaseInsensitiveEnumParseThrewAnException", ex3, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex3.Message
					});
				}
			}
			catch (Exception ex4)
			{
				CommandProcessorBase.CheckForSevereException(ex4);
				LanguagePrimitives.typeConversion.WriteLine("Case Sensitive Enum.Parse threw an exception.", new object[0]);
				throw new PSInvalidCastException("CaseSensitiveEnumParseThrewAnException", ex4, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex4.Message
				});
			}
			if (obj == null)
			{
				LanguagePrimitives.typeConversion.WriteLine("Calling substring disambiguation.", new object[0]);
				try
				{
					string value = EnumMinimumDisambiguation.EnumDisambiguate(text, resultType);
					obj = Enum.Parse(resultType, value);
				}
				catch (Exception ex5)
				{
					CommandProcessorBase.CheckForSevereException(ex5);
					LanguagePrimitives.typeConversion.WriteLine("Substring disambiguation threw an exception.", new object[0]);
					throw new PSInvalidCastException("SubstringDisambiguationEnumParseThrewAnException", ex5, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex5.Message
					});
				}
			}
			LanguagePrimitives.EnumSingleTypeConverter.ThrowForUndefinedEnum("EnumParseUndefined", obj, valueToConvert, resultType);
			LanguagePrimitives.tracer.WriteLine("returning \"{0}\" from conversion to Enum.", new object[]
			{
				obj
			});
			return obj;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00056914 File Offset: 0x00054B14
		private static object ConvertEnumerableToEnum(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(valueToConvert);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			while (ParserOps.MoveNext(null, null, enumerator))
			{
				if (flag)
				{
					stringBuilder.Append(',');
				}
				else
				{
					flag = true;
				}
				string text = enumerator.Current as string;
				if (text == null)
				{
					object obj = LanguagePrimitives.ConvertTo(enumerator.Current, resultType, recursion, formatProvider, backupTable);
					if (obj == null)
					{
						throw new PSInvalidCastException("InvalidCastEnumStringNotFound", null, ExtendedTypeSystem.InvalidCastExceptionEnumerationNoValue, new object[]
						{
							enumerator.Current,
							resultType,
							LanguagePrimitives.EnumSingleTypeConverter.EnumValues(resultType)
						});
					}
					stringBuilder.Append(obj.ToString());
				}
				stringBuilder.Append(text);
			}
			return LanguagePrimitives.ConvertStringToEnum(stringBuilder.ToString(), resultType, recursion, originalValueToConvert, formatProvider, backupTable);
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x000569D8 File Offset: 0x00054BD8
		private static object ConvertIConvertible(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			object result;
			try
			{
				object obj = Convert.ChangeType(valueToConvert, resultType, formatProvider);
				LanguagePrimitives.typeConversion.WriteLine("Conversion using IConvertible succeeded.", new object[0]);
				result = obj;
			}
			catch (Exception ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting with IConvertible: \"{0}\".", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
				throw new PSInvalidCastException("InvalidCastIConvertible", ex, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
				{
					valueToConvert.ToString(),
					resultType.ToString(),
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00056A7C File Offset: 0x00054C7C
		private static object ConvertNumericIConvertible(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			if (originalValueToConvert != null && originalValueToConvert.TokenText != null)
			{
				return LanguagePrimitives.ConvertTo(originalValueToConvert.TokenText, resultType, recursion, formatProvider, backupTable);
			}
			string valueToConvert2 = (string)LanguagePrimitives.ConvertTo(valueToConvert, typeof(string), recursion, formatProvider, backupTable);
			return LanguagePrimitives.ConvertTo(valueToConvert2, resultType, recursion, formatProvider, backupTable);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00056ACD File Offset: 0x00054CCD
		private static object ConvertNullToNumeric(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting null to zero.", new object[0]);
			return Convert.ChangeType(0, resultType, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00056AF5 File Offset: 0x00054CF5
		private static char ConvertNullToChar(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting null to '0'.", new object[0]);
			return '\0';
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00056B0D File Offset: 0x00054D0D
		private static string ConvertNullToString(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting null to \"\".", new object[0]);
			return string.Empty;
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00056B29 File Offset: 0x00054D29
		private static PSReference ConvertNullToPSReference(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return new PSReference<LanguagePrimitives.Null>(null);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00056B31 File Offset: 0x00054D31
		private static object ConvertNullToRef(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return valueToConvert;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00056B34 File Offset: 0x00054D34
		private static bool ConvertNullToBool(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting null to boolean.", new object[0]);
			return false;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00056B4C File Offset: 0x00054D4C
		private static object ConvertNullToNullable(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			return null;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00056B4F File Offset: 0x00054D4F
		private static SwitchParameter ConvertNullToSwitch(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting null to SwitchParameter(false).", new object[0]);
			return new SwitchParameter(false);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00056B6C File Offset: 0x00054D6C
		private static object ConvertNullToVoid(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.typeConversion.WriteLine("Converting null to AutomationNull.Value.", new object[0]);
			return AutomationNull.Value;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00056B88 File Offset: 0x00054D88
		private static object ConvertNoConversion(object valueToConvert, Type resultType, bool recurse, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.ThrowInvalidCastException(valueToConvert, resultType);
			return null;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00056B93 File Offset: 0x00054D93
		private static object ConvertNotSupportedConversion(object valueToConvert, Type resultType, bool recurse, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
		{
			LanguagePrimitives.ThrowInvalidConversionException(valueToConvert, resultType);
			return null;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00056BA0 File Offset: 0x00054DA0
		private static LanguagePrimitives.ConversionData CacheConversion<T>(Type fromType, Type toType, LanguagePrimitives.PSConverter<T> converter, ConversionRank rank)
		{
			LanguagePrimitives.ConversionTypePair key = new LanguagePrimitives.ConversionTypePair(fromType, toType);
			LanguagePrimitives.ConversionData conversionData = null;
			lock (LanguagePrimitives.converterCache)
			{
				if (!LanguagePrimitives.converterCache.TryGetValue(key, out conversionData))
				{
					conversionData = new LanguagePrimitives.ConversionData<T>(converter, rank);
					LanguagePrimitives.converterCache.Add(key, conversionData);
				}
			}
			return conversionData;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00056C08 File Offset: 0x00054E08
		private static LanguagePrimitives.ConversionData GetConversionData(Type fromType, Type toType)
		{
			LanguagePrimitives.ConversionData result;
			lock (LanguagePrimitives.converterCache)
			{
				LanguagePrimitives.ConversionData conversionData = null;
				LanguagePrimitives.converterCache.TryGetValue(new LanguagePrimitives.ConversionTypePair(fromType, toType), out conversionData);
				result = conversionData;
			}
			return result;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00056C5C File Offset: 0x00054E5C
		internal static ConversionRank GetConversionRank(Type fromType, Type toType)
		{
			return LanguagePrimitives.FigureConversion(fromType, toType).Rank;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00056C6C File Offset: 0x00054E6C
		internal static void RebuildConversionCache()
		{
			lock (LanguagePrimitives.converterCache)
			{
				LanguagePrimitives.converterCache.Clear();
				Type typeFromHandle = typeof(string);
				Type typeFromHandle2 = typeof(LanguagePrimitives.Null);
				Type typeFromHandle3 = typeof(float);
				Type typeFromHandle4 = typeof(double);
				Type typeFromHandle5 = typeof(decimal);
				Type typeFromHandle6 = typeof(bool);
				Type typeFromHandle7 = typeof(char);
				foreach (Type type in LanguagePrimitives.NumericTypes)
				{
					LanguagePrimitives.CacheConversion<string>(type, typeFromHandle, new LanguagePrimitives.PSConverter<string>(LanguagePrimitives.ConvertNumericToString), ConversionRank.NumericString);
					LanguagePrimitives.CacheConversion<object>(type, typeFromHandle7, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertIConvertible), ConversionRank.NumericString);
					LanguagePrimitives.CacheConversion<object>(typeFromHandle2, type, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNullToNumeric), ConversionRank.NullToValue);
				}
				LanguagePrimitives.CacheConversion<bool>(typeof(short), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertInt16ToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(int), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertInt32ToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(long), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertInt64ToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(ushort), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertUInt16ToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(uint), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertUInt32ToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(ulong), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertUInt64ToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(sbyte), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertSByteToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(byte), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertByteToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(float), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertSingleToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(double), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertDoubleToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(decimal), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertDecimalToBool), ConversionRank.Language);
				for (int j = 0; j < LanguagePrimitives.UnsignedIntegerTypes.Length; j++)
				{
					LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.UnsignedIntegerTypes[j], LanguagePrimitives.UnsignedIntegerTypes[j], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertAssignableFrom), ConversionRank.Identity);
					LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.SignedIntegerTypes[j], LanguagePrimitives.SignedIntegerTypes[j], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertAssignableFrom), ConversionRank.Identity);
					LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.UnsignedIntegerTypes[j], LanguagePrimitives.SignedIntegerTypes[j], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit);
					LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.SignedIntegerTypes[j], LanguagePrimitives.UnsignedIntegerTypes[j], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit1);
					for (int k = j + 1; k < LanguagePrimitives.UnsignedIntegerTypes.Length; k++)
					{
						LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.UnsignedIntegerTypes[j], LanguagePrimitives.UnsignedIntegerTypes[k], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericImplicit);
						LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.SignedIntegerTypes[j], LanguagePrimitives.SignedIntegerTypes[k], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericImplicit);
						LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.UnsignedIntegerTypes[j], LanguagePrimitives.SignedIntegerTypes[k], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericImplicit);
						LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.SignedIntegerTypes[j], LanguagePrimitives.UnsignedIntegerTypes[k], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit1);
						LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.UnsignedIntegerTypes[k], LanguagePrimitives.UnsignedIntegerTypes[j], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit);
						LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.SignedIntegerTypes[k], LanguagePrimitives.SignedIntegerTypes[j], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit);
						LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.UnsignedIntegerTypes[k], LanguagePrimitives.SignedIntegerTypes[j], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit);
						LanguagePrimitives.CacheConversion<object>(LanguagePrimitives.SignedIntegerTypes[k], LanguagePrimitives.UnsignedIntegerTypes[j], new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit);
					}
				}
				foreach (Type type2 in LanguagePrimitives.IntegerTypes)
				{
					LanguagePrimitives.CacheConversion<object>(typeFromHandle, type2, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertStringToInteger), ConversionRank.NumericString);
					foreach (Type type3 in LanguagePrimitives.RealTypes)
					{
						LanguagePrimitives.CacheConversion<object>(type2, type3, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericImplicit);
						LanguagePrimitives.CacheConversion<object>(type3, type2, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit);
					}
				}
				LanguagePrimitives.CacheConversion<object>(typeFromHandle3, typeFromHandle4, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericImplicit);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle4, typeFromHandle3, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle3, typeFromHandle5, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle4, typeFromHandle5, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle5, typeFromHandle3, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit1);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle5, typeFromHandle4, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumeric), ConversionRank.NumericExplicit1);
				LanguagePrimitives.CacheConversion<Regex>(typeFromHandle, typeof(Regex), new LanguagePrimitives.PSConverter<Regex>(LanguagePrimitives.ConvertStringToRegex), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<char[]>(typeFromHandle, typeof(char[]), new LanguagePrimitives.PSConverter<char[]>(LanguagePrimitives.ConvertStringToCharArray), ConversionRank.StringToCharArray);
				LanguagePrimitives.CacheConversion<Type>(typeFromHandle, typeof(Type), new LanguagePrimitives.PSConverter<Type>(LanguagePrimitives.ConvertStringToType), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle, typeFromHandle5, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertStringToDecimal), ConversionRank.NumericString);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle, typeFromHandle3, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertStringToReal), ConversionRank.NumericString);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle, typeFromHandle4, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertStringToReal), ConversionRank.NumericString);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle7, typeFromHandle3, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumericChar), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle7, typeFromHandle4, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumericChar), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeFromHandle7, typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertCharToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<char>(typeFromHandle2, typeFromHandle7, new LanguagePrimitives.PSConverter<char>(LanguagePrimitives.ConvertNullToChar), ConversionRank.NullToValue);
				LanguagePrimitives.CacheConversion<string>(typeFromHandle2, typeFromHandle, new LanguagePrimitives.PSConverter<string>(LanguagePrimitives.ConvertNullToString), ConversionRank.ToString);
				LanguagePrimitives.CacheConversion<bool>(typeFromHandle2, typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertNullToBool), ConversionRank.NullToValue);
				LanguagePrimitives.CacheConversion<PSReference>(typeFromHandle2, typeof(PSReference), new LanguagePrimitives.PSConverter<PSReference>(LanguagePrimitives.ConvertNullToPSReference), ConversionRank.NullToRef);
				LanguagePrimitives.CacheConversion<SwitchParameter>(typeFromHandle2, typeof(SwitchParameter), new LanguagePrimitives.PSConverter<SwitchParameter>(LanguagePrimitives.ConvertNullToSwitch), ConversionRank.NullToValue);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle2, typeof(void), new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNullToVoid), ConversionRank.NullToValue);
				LanguagePrimitives.CacheConversion<object>(typeFromHandle6, typeFromHandle6, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertAssignableFrom), ConversionRank.Identity);
				LanguagePrimitives.CacheConversion<bool>(typeFromHandle, typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertStringToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<bool>(typeof(SwitchParameter), typeFromHandle6, new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertSwitchParameterToBool), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<ManagementObjectSearcher>(typeFromHandle, typeof(ManagementObjectSearcher), new LanguagePrimitives.PSConverter<ManagementObjectSearcher>(LanguagePrimitives.ConvertToWMISearcher), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<ManagementClass>(typeFromHandle, typeof(ManagementClass), new LanguagePrimitives.PSConverter<ManagementClass>(LanguagePrimitives.ConvertToWMIClass), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<ManagementObject>(typeFromHandle, typeof(ManagementObject), new LanguagePrimitives.PSConverter<ManagementObject>(LanguagePrimitives.ConvertToWMI), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<DirectoryEntry>(typeFromHandle, typeof(DirectoryEntry), new LanguagePrimitives.PSConverter<DirectoryEntry>(LanguagePrimitives.ConvertToADSI), ConversionRank.Language);
				LanguagePrimitives.CacheConversion<DirectorySearcher>(typeFromHandle, typeof(DirectorySearcher), new LanguagePrimitives.PSConverter<DirectorySearcher>(LanguagePrimitives.ConvertToADSISearcher), ConversionRank.Language);
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x000574F4 File Offset: 0x000556F4
		internal static PSObject SetObjectProperties(object o, PSObject psObject, Type resultType, LanguagePrimitives.MemberNotFoundError memberNotFoundErrorAction, LanguagePrimitives.MemberSetValueError memberSetValueErrorAction, IFormatProvider formatProvider, bool recursion = false, bool ignoreUnknownMembers = false)
		{
			if (Deserializer.IsDeserializedInstanceOfType(psObject, resultType))
			{
				try
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					foreach (PSPropertyInfo pspropertyInfo in psObject.Properties)
					{
						if (pspropertyInfo is PSProperty)
						{
							dictionary.Add(pspropertyInfo.Name, pspropertyInfo.Value);
						}
					}
					return LanguagePrimitives.SetObjectProperties(o, dictionary, resultType, memberNotFoundErrorAction, memberSetValueErrorAction, false);
				}
				catch (SetValueException)
				{
					goto IL_142;
				}
				catch (InvalidOperationException)
				{
					goto IL_142;
				}
			}
			object obj = PSObject.Base(psObject);
			IDictionary dictionary2 = obj as IDictionary;
			if (dictionary2 != null)
			{
				return LanguagePrimitives.SetObjectProperties(o, dictionary2, resultType, memberNotFoundErrorAction, memberSetValueErrorAction, false);
			}
			PSObject psobject = obj as PSObject;
			if (psobject != null)
			{
				Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
				foreach (PSPropertyInfo pspropertyInfo2 in psobject.Properties)
				{
					dictionary3.Add(pspropertyInfo2.Name, pspropertyInfo2.Value);
				}
				try
				{
					return LanguagePrimitives.SetObjectProperties(o, dictionary3, resultType, memberNotFoundErrorAction, memberSetValueErrorAction, false, formatProvider, recursion, ignoreUnknownMembers);
				}
				catch (InvalidOperationException innerException)
				{
					throw new PSInvalidCastException("ConvertToFinalInvalidCastException", innerException, ExtendedTypeSystem.InvalidCastException, new object[]
					{
						psObject.ToString(),
						LanguagePrimitives.ObjectToTypeNameString(psObject),
						resultType.ToString()
					});
				}
			}
			IL_142:
			LanguagePrimitives.ThrowInvalidCastException(psObject, resultType);
			return null;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00057690 File Offset: 0x00055890
		internal static PSObject SetObjectProperties(object o, IDictionary properties, Type resultType, LanguagePrimitives.MemberNotFoundError memberNotFoundErrorAction, LanguagePrimitives.MemberSetValueError memberSetValueErrorAction, bool enableMethodCall)
		{
			return LanguagePrimitives.SetObjectProperties(o, properties, resultType, memberNotFoundErrorAction, memberSetValueErrorAction, enableMethodCall, CultureInfo.InvariantCulture, false, false);
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000576B4 File Offset: 0x000558B4
		internal static PSObject SetObjectProperties(object o, IDictionary properties, Type resultType, LanguagePrimitives.MemberNotFoundError memberNotFoundErrorAction, LanguagePrimitives.MemberSetValueError memberSetValueErrorAction, bool enableMethodCall, IFormatProvider formatProvider, bool recursion = false, bool ignoreUnknownMembers = false)
		{
			PSObject psobject = PSObject.AsPSObject(o);
			if (properties != null)
			{
				foreach (object obj in properties)
				{
					DictionaryEntry property = (DictionaryEntry)obj;
					PSMethodInfo psmethodInfo = enableMethodCall ? psobject.Methods[property.Key.ToString()] : null;
					try
					{
						if (psmethodInfo != null)
						{
							psmethodInfo.Invoke(new object[]
							{
								property.Value
							});
						}
						else
						{
							PSPropertyInfo pspropertyInfo = psobject.Properties[property.Key.ToString()];
							if (pspropertyInfo != null)
							{
								object value = property.Value;
								Type resultType2;
								if (recursion && property.Value != null && TypeResolver.TryResolveType(pspropertyInfo.TypeNameOfValue, out resultType2))
								{
									if (formatProvider == null)
									{
										formatProvider = CultureInfo.InvariantCulture;
									}
									try
									{
										PSObject psobject2 = property.Value as PSObject;
										if (psobject2 != null)
										{
											value = LanguagePrimitives.ConvertPSObjectToType(psobject2, resultType2, recursion, formatProvider, ignoreUnknownMembers);
										}
										else if (property.Value is PSCustomObject)
										{
											value = LanguagePrimitives.ConvertPSObjectToType(new PSObject(property.Value), resultType2, recursion, formatProvider, ignoreUnknownMembers);
										}
										else
										{
											value = LanguagePrimitives.ConvertTo(property.Value, resultType2, recursion, formatProvider, null);
										}
									}
									catch (SetValueException)
									{
									}
								}
								pspropertyInfo.Value = value;
							}
							else if (psobject.BaseObject is PSCustomObject)
							{
								string text = property.Key as string;
								string text2 = property.Value as string;
								if (text != null && text2 != null && text.Equals("PSTypeName", StringComparison.OrdinalIgnoreCase))
								{
									psobject.TypeNames.Insert(0, text2);
								}
								else
								{
									psobject.Properties.Add(new PSNoteProperty(property.Key.ToString(), property.Value));
								}
							}
							else if (!ignoreUnknownMembers)
							{
								memberNotFoundErrorAction(psobject, property, resultType);
							}
						}
					}
					catch (SetValueException e)
					{
						memberSetValueErrorAction(e);
					}
				}
			}
			return psobject;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x000578F8 File Offset: 0x00055AF8
		private static string GetAvailableProperties(PSObject pso)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			if (pso != null && pso.Properties != null)
			{
				foreach (PSPropertyInfo pspropertyInfo in pso.Properties)
				{
					if (!flag)
					{
						stringBuilder.Append(" , ");
					}
					stringBuilder.Append(string.Concat(new string[]
					{
						"[",
						pspropertyInfo.Name,
						" <",
						pspropertyInfo.TypeNameOfValue,
						">]"
					}));
					if (flag)
					{
						flag = false;
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x000579B8 File Offset: 0x00055BB8
		internal static LanguagePrimitives.ConversionData FigureConversion(object valueToConvert, Type resultType, out bool debase)
		{
			PSObject psobject;
			Type fromType;
			if (valueToConvert == null || valueToConvert == AutomationNull.Value)
			{
				psobject = null;
				fromType = typeof(LanguagePrimitives.Null);
			}
			else
			{
				psobject = (valueToConvert as PSObject);
				fromType = valueToConvert.GetType();
			}
			debase = false;
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(fromType, resultType);
			if (conversionData.Rank != ConversionRank.None)
			{
				return conversionData;
			}
			if (psobject != null)
			{
				debase = true;
				valueToConvert = PSObject.Base(valueToConvert);
				if (valueToConvert == null)
				{
					fromType = typeof(LanguagePrimitives.Null);
				}
				else
				{
					fromType = ((valueToConvert is PSObject) ? typeof(LanguagePrimitives.InternalPSObject) : valueToConvert.GetType());
				}
				conversionData = LanguagePrimitives.FigureConversion(fromType, resultType);
			}
			return conversionData;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00057A44 File Offset: 0x00055C44
		internal static object ConvertTo(object valueToConvert, Type resultType, bool recursion, IFormatProvider formatProvider, TypeTable backupTypeTable)
		{
			object result;
			using (LanguagePrimitives.typeConversion.TraceScope("Converting \"{0}\" to \"{1}\".", new object[]
			{
				valueToConvert,
				resultType
			}))
			{
				if (resultType == null)
				{
					throw PSTraceSource.NewArgumentNullException("resultType");
				}
				bool flag;
				LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(valueToConvert, resultType, out flag);
				result = conversionData.Invoke(flag ? PSObject.Base(valueToConvert) : valueToConvert, resultType, recursion, flag ? ((PSObject)valueToConvert) : null, formatProvider, backupTypeTable);
			}
			return result;
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00057AD4 File Offset: 0x00055CD4
		internal static Tuple<string, string> GetInvalidCastMessages(object valueToConvert, Type resultType)
		{
			string item;
			string item2;
			if (PSObject.Base(valueToConvert) != null)
			{
				LanguagePrimitives.typeConversion.WriteLine("Type Conversion failed.", new object[0]);
				item = "ConvertToFinalInvalidCastException";
				item2 = StringUtil.Format(ExtendedTypeSystem.InvalidCastException, new object[]
				{
					valueToConvert.ToString(),
					LanguagePrimitives.ObjectToTypeNameString(valueToConvert),
					resultType.ToString()
				});
				return Tuple.Create<string, string>(item, item2);
			}
			if (resultType.GetTypeInfo().IsEnum)
			{
				LanguagePrimitives.typeConversion.WriteLine("Issuing an error message about not being able to convert null to an Enum type.", new object[0]);
				item = "nullToEnumInvalidCast";
				item2 = StringUtil.Format(ExtendedTypeSystem.InvalidCastExceptionEnumerationNull, resultType, LanguagePrimitives.EnumSingleTypeConverter.EnumValues(resultType));
				return Tuple.Create<string, string>(item, item2);
			}
			LanguagePrimitives.typeConversion.WriteLine("Cannot convert null.", new object[0]);
			item = "nullToObjectInvalidCast";
			item2 = StringUtil.Format(ExtendedTypeSystem.InvalidCastFromNull, resultType.ToString());
			return Tuple.Create<string, string>(item, item2);
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x00057BB0 File Offset: 0x00055DB0
		internal static object ThrowInvalidCastException(object valueToConvert, Type resultType)
		{
			Tuple<string, string> invalidCastMessages = LanguagePrimitives.GetInvalidCastMessages(valueToConvert, resultType);
			throw new PSInvalidCastException(invalidCastMessages.Item1, invalidCastMessages.Item2, null);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00057BD8 File Offset: 0x00055DD8
		internal static object ThrowInvalidConversionException(object valueToConvert, Type resultType)
		{
			LanguagePrimitives.typeConversion.WriteLine("Issuing an error message about not being able to convert to non-core type.", new object[0]);
			throw new PSInvalidCastException("ConversionSupportedOnlyToCoreTypes", null, ExtendedTypeSystem.InvalidCastExceptionNonCoreType, new object[]
			{
				resultType.ToString()
			});
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00057C1C File Offset: 0x00055E1C
		private static LanguagePrimitives.ConversionData FigureLanguageConversion(Type fromType, Type toType, out LanguagePrimitives.PSConverter<object> valueDependentConversion, out ConversionRank valueDependentRank)
		{
			valueDependentConversion = null;
			valueDependentRank = ConversionRank.None;
			Type underlyingType = Nullable.GetUnderlyingType(toType);
			if (underlyingType != null)
			{
				LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(fromType, underlyingType);
				if (conversionData.Rank != ConversionRank.None)
				{
					return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertToNullable), conversionData.Rank);
				}
			}
			if (toType == typeof(void))
			{
				return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertToVoid), ConversionRank.Language);
			}
			if (toType == typeof(bool))
			{
				LanguagePrimitives.PSConverter<bool> converter;
				if (typeof(IList).IsAssignableFrom(fromType))
				{
					converter = new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertIListToBool);
				}
				else if (fromType.GetTypeInfo().IsEnum)
				{
					converter = LanguagePrimitives.CreateNumericToBoolConverter(fromType);
				}
				else if (fromType.GetTypeInfo().IsValueType)
				{
					converter = new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertValueToBool);
				}
				else
				{
					converter = new LanguagePrimitives.PSConverter<bool>(LanguagePrimitives.ConvertClassToBool);
				}
				return LanguagePrimitives.CacheConversion<bool>(fromType, toType, converter, ConversionRank.Language);
			}
			if (toType == typeof(string))
			{
				return LanguagePrimitives.CacheConversion<string>(fromType, toType, new LanguagePrimitives.PSConverter<string>(LanguagePrimitives.ConvertNonNumericToString), ConversionRank.ToString);
			}
			if (toType.IsArray)
			{
				Type elementType = toType.GetElementType();
				if (fromType.IsArray)
				{
					if (elementType.IsAssignableFrom(fromType.GetElementType()))
					{
						return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertRelatedArrays), ConversionRank.Language);
					}
					return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertUnrelatedArrays), ConversionRank.UnrelatedArrays);
				}
				else
				{
					if (LanguagePrimitives.IsTypeEnumerable(fromType))
					{
						return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertEnumerableToArray), ConversionRank.Language);
					}
					LanguagePrimitives.ConversionData conversionData2 = LanguagePrimitives.FigureConversion(fromType, elementType);
					if (conversionData2.Rank != ConversionRank.None)
					{
						valueDependentRank = (conversionData2.Rank & ConversionRank.ValueDependent);
						valueDependentConversion = new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertScalarToArray);
						return null;
					}
				}
			}
			if (toType == typeof(Array))
			{
				if (fromType.IsArray || fromType == typeof(Array))
				{
					return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertAssignableFrom), ConversionRank.Assignable);
				}
				if (LanguagePrimitives.IsTypeEnumerable(fromType))
				{
					return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertEnumerableToArray), ConversionRank.Language);
				}
				valueDependentRank = ConversionRank.AssignableS2A;
				valueDependentConversion = new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertScalarToArray);
				return null;
			}
			else if (toType == typeof(Hashtable))
			{
				if (typeof(IDictionary).IsAssignableFrom(fromType))
				{
					return LanguagePrimitives.CacheConversion<Hashtable>(fromType, toType, new LanguagePrimitives.PSConverter<Hashtable>(LanguagePrimitives.ConvertIDictionaryToHashtable), ConversionRank.Language);
				}
				return null;
			}
			else
			{
				if (toType == typeof(PSReference))
				{
					return LanguagePrimitives.CacheConversion<PSReference>(fromType, toType, new LanguagePrimitives.PSConverter<PSReference>(LanguagePrimitives.ConvertToPSReference), ConversionRank.Language);
				}
				if (toType == typeof(XmlDocument))
				{
					return LanguagePrimitives.CacheConversion<XmlDocument>(fromType, toType, new LanguagePrimitives.PSConverter<XmlDocument>(LanguagePrimitives.ConvertToXml), ConversionRank.Language);
				}
				if (toType == typeof(StringCollection))
				{
					ConversionRank rank = (fromType.IsArray || LanguagePrimitives.IsTypeEnumerable(fromType)) ? ConversionRank.Language : ConversionRank.LanguageS2A;
					return LanguagePrimitives.CacheConversion<StringCollection>(fromType, toType, new LanguagePrimitives.PSConverter<StringCollection>(LanguagePrimitives.ConvertToStringCollection), rank);
				}
				if (toType == typeof(CommaDelimitedStringCollection))
				{
					ConversionRank rank2 = (fromType.IsArray || LanguagePrimitives.IsTypeEnumerable(fromType)) ? ConversionRank.Language : ConversionRank.LanguageS2A;
					return LanguagePrimitives.CacheConversion<CommaDelimitedStringCollection>(fromType, toType, new LanguagePrimitives.PSConverter<CommaDelimitedStringCollection>(LanguagePrimitives.ConvertToCommaDelimitedStringCollection), rank2);
				}
				if (toType.IsSubclassOf(typeof(Delegate)) && (fromType == typeof(ScriptBlock) || fromType.IsSubclassOf(typeof(ScriptBlock))))
				{
					return LanguagePrimitives.CacheConversion<Delegate>(fromType, toType, new LanguagePrimitives.PSConverter<Delegate>(LanguagePrimitives.ConvertScriptBlockToDelegate), ConversionRank.Language);
				}
				if (toType == typeof(LanguagePrimitives.InternalPSCustomObject))
				{
					Type typeFromHandle = typeof(PSObject);
					ConstructorInfo constructor = typeFromHandle.GetConstructor(PSTypeExtensions.EmptyTypes);
					LanguagePrimitives.ConvertViaNoArgumentConstructor @object = new LanguagePrimitives.ConvertViaNoArgumentConstructor(constructor, typeFromHandle);
					return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(@object.Convert), ConversionRank.Language);
				}
				TypeCode typeCode = LanguagePrimitives.GetTypeCode(fromType);
				if (LanguagePrimitives.IsInteger(typeCode) && toType.GetTypeInfo().IsEnum)
				{
					return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertIntegerToEnum), ConversionRank.Language);
				}
				return null;
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0005805B File Offset: 0x0005625B
		private static LanguagePrimitives.PSConverter<object> FigureStaticCreateMethodConversion(Type fromType, Type toType)
		{
			if (fromType == typeof(string) && toType == typeof(CimSession))
			{
				return new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertStringToCimSession);
			}
			return null;
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00058090 File Offset: 0x00056290
		private static LanguagePrimitives.PSConverter<object> FigureParseConversion(Type fromType, Type toType)
		{
			if (toType.GetTypeInfo().IsEnum)
			{
				if (fromType == typeof(string))
				{
					return new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertStringToEnum);
				}
				if (LanguagePrimitives.IsTypeEnumerable(fromType))
				{
					return new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertEnumerableToEnum);
				}
			}
			else if (fromType == typeof(string))
			{
				MethodInfo methodInfo = null;
				try
				{
					methodInfo = toType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, new Type[]
					{
						typeof(string),
						typeof(IFormatProvider)
					}, null);
				}
				catch (AmbiguousMatchException ex)
				{
					LanguagePrimitives.typeConversion.WriteLine("Exception finding Parse method with CultureInfo: \"{0}\".", new object[]
					{
						ex.Message
					});
				}
				catch (ArgumentException ex2)
				{
					LanguagePrimitives.typeConversion.WriteLine("Exception finding Parse method with CultureInfo: \"{0}\".", new object[]
					{
						ex2.Message
					});
				}
				if (methodInfo != null)
				{
					return new LanguagePrimitives.PSConverter<object>(new LanguagePrimitives.ConvertViaParseMethod
					{
						parse = methodInfo
					}.ConvertWithCulture);
				}
				try
				{
					methodInfo = toType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, new Type[]
					{
						typeof(string)
					}, null);
				}
				catch (AmbiguousMatchException ex3)
				{
					LanguagePrimitives.typeConversion.WriteLine("Exception finding Parse method: \"{0}\".", new object[]
					{
						ex3.Message
					});
				}
				catch (ArgumentException ex4)
				{
					LanguagePrimitives.typeConversion.WriteLine("Exception finding Parse method: \"{0}\".", new object[]
					{
						ex4.Message
					});
				}
				if (methodInfo != null)
				{
					return new LanguagePrimitives.PSConverter<object>(new LanguagePrimitives.ConvertViaParseMethod
					{
						parse = methodInfo
					}.ConvertWithoutCulture);
				}
			}
			return null;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x00058274 File Offset: 0x00056474
		internal static Tuple<LanguagePrimitives.PSConverter<object>, ConversionRank> FigureIEnumerableConstructorConversion(Type fromType, Type toType)
		{
			TypeInfo typeInfo = toType.GetTypeInfo();
			if (typeInfo.IsAbstract)
			{
				return null;
			}
			try
			{
				bool flag = false;
				bool flag2 = false;
				Type type = null;
				ConstructorInfo constructorInfo = null;
				if (typeInfo.IsGenericType && !typeInfo.ContainsGenericParameters && (typeof(IList).IsAssignableFrom(toType) || typeof(ICollection).IsAssignableFrom(toType) || typeof(IEnumerable).IsAssignableFrom(toType)))
				{
					Type[] genericArguments = toType.GetGenericArguments();
					if (genericArguments.Length != 1)
					{
						LanguagePrimitives.typeConversion.WriteLine("toType has more than one generic arguments. Here we only care about the toType which contains only one generic argument and whose constructor takes IEnumerable<T>, ICollection<T> or IList<T>.", new object[0]);
						return null;
					}
					type = genericArguments[0];
					if (typeof(Array) == fromType || typeof(object[]) == fromType || type.IsAssignableFrom(fromType) || LanguagePrimitives.FigureConversion(fromType, type) != null)
					{
						flag2 = type.IsAssignableFrom(fromType);
						ConstructorInfo[] constructors = toType.GetConstructors();
						Type left = typeof(IEnumerable<>).MakeGenericType(new Type[]
						{
							type
						});
						Type left2 = typeof(ICollection<>).MakeGenericType(new Type[]
						{
							type
						});
						Type left3 = typeof(IList<>).MakeGenericType(new Type[]
						{
							type
						});
						foreach (ConstructorInfo constructorInfo2 in constructors)
						{
							ParameterInfo[] parameters = constructorInfo2.GetParameters();
							if (parameters.Length == 1)
							{
								Type parameterType = parameters[0].ParameterType;
								if (left == parameterType || left2 == parameterType || left3 == parameterType)
								{
									constructorInfo = constructorInfo2;
									flag = true;
									break;
								}
							}
						}
					}
				}
				if (flag)
				{
					LanguagePrimitives.ConvertViaIEnumerableConstructor convertViaIEnumerableConstructor = new LanguagePrimitives.ConvertViaIEnumerableConstructor();
					try
					{
						Type type2 = typeof(List<>).MakeGenericType(new Type[]
						{
							type
						});
						ConstructorInfo constructor = type2.GetConstructor(new Type[]
						{
							typeof(int)
						});
						convertViaIEnumerableConstructor.ListCtorLambda = LanguagePrimitives.CreateCtorLambdaClosure<int, IList>(constructor, typeof(int), false);
						ParameterInfo[] parameters2 = constructorInfo.GetParameters();
						Type parameterType2 = parameters2[0].ParameterType;
						convertViaIEnumerableConstructor.TargetCtorLambda = LanguagePrimitives.CreateCtorLambdaClosure<IList, object>(constructorInfo, parameterType2, false);
						convertViaIEnumerableConstructor.ElementType = type;
						convertViaIEnumerableConstructor.IsScalar = flag2;
					}
					catch (Exception ex)
					{
						CommandProcessorBase.CheckForSevereException(ex);
						LanguagePrimitives.typeConversion.WriteLine("Exception building constructor lambda: \"{0}\"", new object[]
						{
							ex.Message
						});
						return null;
					}
					ConversionRank conversionRank = flag2 ? ConversionRank.ConstructorS2A : ConversionRank.Constructor;
					LanguagePrimitives.typeConversion.WriteLine("Conversion is figured out. Conversion rank: \"{0}\"", new object[]
					{
						conversionRank
					});
					return new Tuple<LanguagePrimitives.PSConverter<object>, ConversionRank>(new LanguagePrimitives.PSConverter<object>(convertViaIEnumerableConstructor.Convert), conversionRank);
				}
				LanguagePrimitives.typeConversion.WriteLine("Fail to figure out the conversion from \"{0}\" to \"{1}\"", new object[]
				{
					fromType.FullName,
					toType.FullName
				});
				return null;
			}
			catch (ArgumentException ex2)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception finding IEnumerable conversion: \"{0}\".", new object[]
				{
					ex2.Message
				});
			}
			catch (InvalidOperationException ex3)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception finding IEnumerable conversion: \"{0}\".", new object[]
				{
					ex3.Message
				});
			}
			catch (NotSupportedException ex4)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception finding IEnumerable conversion: \"{0}\".", new object[]
				{
					ex4.Message
				});
			}
			return null;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00058658 File Offset: 0x00056858
		private static Func<T1, T2> CreateCtorLambdaClosure<T1, T2>(ConstructorInfo ctor, Type realParamType, bool useExplicitConversion)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T1), "args");
			Expression expr = useExplicitConversion ? Expression.Call(CachedReflectionInfo.Convert_ChangeType, parameterExpression, Expression.Constant(realParamType, typeof(Type))) : Expression.Convert(parameterExpression, realParamType);
			NewExpression expr2 = Expression.New(ctor, new Expression[]
			{
				expr.Cast(realParamType)
			});
			return Expression.Lambda<Func<T1, T2>>(expr2.Cast(typeof(T2)), new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000586E4 File Offset: 0x000568E4
		internal static LanguagePrimitives.PSConverter<object> FigureConstructorConversion(Type fromType, Type toType)
		{
			if (LanguagePrimitives.IsIntegralType(fromType) && (typeof(IList).IsAssignableFrom(toType) || typeof(ICollection).IsAssignableFrom(toType)))
			{
				LanguagePrimitives.typeConversion.WriteLine("Ignoring the collection constructor that takes an integer, since this is not semantically a conversion.", new object[0]);
				return null;
			}
			ConstructorInfo constructorInfo = null;
			try
			{
				constructorInfo = toType.GetConstructor(new Type[]
				{
					fromType
				});
			}
			catch (AmbiguousMatchException ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception finding Constructor: \"{0}\".", new object[]
				{
					ex.Message
				});
			}
			catch (ArgumentException ex2)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception finding Constructor: \"{0}\".", new object[]
				{
					ex2.Message
				});
			}
			if (constructorInfo == null)
			{
				return null;
			}
			LanguagePrimitives.typeConversion.WriteLine("Found Constructor.", new object[0]);
			LanguagePrimitives.ConvertViaConstructor convertViaConstructor = new LanguagePrimitives.ConvertViaConstructor();
			try
			{
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				Type parameterType = parameters[0].ParameterType;
				bool useExplicitConversion = parameterType.GetTypeInfo().IsValueType && fromType != parameterType && Nullable.GetUnderlyingType(parameterType) == null;
				convertViaConstructor.TargetCtorLambda = LanguagePrimitives.CreateCtorLambdaClosure<object, object>(constructorInfo, parameterType, useExplicitConversion);
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				LanguagePrimitives.typeConversion.WriteLine("Exception building constructor lambda: \"{0}\"", new object[]
				{
					ex3.Message
				});
				return null;
			}
			LanguagePrimitives.typeConversion.WriteLine("Conversion is figured out.", new object[0]);
			return new LanguagePrimitives.PSConverter<object>(convertViaConstructor.Convert);
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x00058890 File Offset: 0x00056A90
		private static bool IsIntegralType(Type type)
		{
			return type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00058930 File Offset: 0x00056B30
		internal static LanguagePrimitives.PSConverter<object> FigurePropertyConversion(Type fromType, Type toType, ref ConversionRank rank)
		{
			TypeInfo typeInfo = toType.GetTypeInfo();
			if (!typeof(PSObject).IsAssignableFrom(fromType) || typeInfo.IsAbstract)
			{
				return null;
			}
			ConstructorInfo constructorInfo = null;
			try
			{
				constructorInfo = toType.GetConstructor(PSTypeExtensions.EmptyTypes);
			}
			catch (AmbiguousMatchException ex)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception finding Constructor: \"{0}\".", new object[]
				{
					ex.Message
				});
			}
			catch (ArgumentException ex2)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception finding Constructor: \"{0}\".", new object[]
				{
					ex2.Message
				});
			}
			if (constructorInfo == null && !typeInfo.IsValueType)
			{
				return null;
			}
			if (toType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length == 0 && toType.GetFields(BindingFlags.Instance | BindingFlags.Public).Length == 0)
			{
				return null;
			}
			LanguagePrimitives.typeConversion.WriteLine("Found Constructor.", new object[0]);
			try
			{
				LanguagePrimitives.ConvertViaNoArgumentConstructor @object = new LanguagePrimitives.ConvertViaNoArgumentConstructor(constructorInfo, toType);
				rank = ConversionRank.Constructor;
				return new LanguagePrimitives.PSConverter<object>(@object.Convert);
			}
			catch (ArgumentException ex3)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting via no argument constructor: \"{0}\".", new object[]
				{
					ex3.Message
				});
			}
			catch (InvalidOperationException ex4)
			{
				LanguagePrimitives.typeConversion.WriteLine("Exception converting via no argument constructor: \"{0}\".", new object[]
				{
					ex4.Message
				});
			}
			rank = ConversionRank.None;
			return null;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00058AA8 File Offset: 0x00056CA8
		internal static LanguagePrimitives.PSConverter<object> FigureCastConversion(Type fromType, Type toType, ref ConversionRank rank)
		{
			MethodInfo methodInfo = LanguagePrimitives.FindCastOperator("op_Implicit", toType, fromType, toType);
			if (methodInfo == null)
			{
				methodInfo = LanguagePrimitives.FindCastOperator("op_Explicit", toType, fromType, toType);
				if (methodInfo == null)
				{
					methodInfo = LanguagePrimitives.FindCastOperator("op_Implicit", fromType, fromType, toType);
					if (methodInfo == null)
					{
						methodInfo = LanguagePrimitives.FindCastOperator("op_Explicit", fromType, fromType, toType);
					}
				}
			}
			if (methodInfo != null)
			{
				rank = (methodInfo.Name.Equals("op_Implicit", StringComparison.OrdinalIgnoreCase) ? ConversionRank.ImplicitCast : ConversionRank.ExplicitCast);
				return new LanguagePrimitives.PSConverter<object>(new LanguagePrimitives.ConvertViaCast
				{
					cast = methodInfo
				}.Convert);
			}
			return null;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00058B48 File Offset: 0x00056D48
		private static bool TypeConverterPossiblyExists(Type type)
		{
			lock (LanguagePrimitives.possibleTypeConverter)
			{
				if (LanguagePrimitives.possibleTypeConverter.ContainsKey(type.FullName))
				{
					return true;
				}
			}
			object[] customAttributes = type.GetTypeInfo().GetCustomAttributes(typeof(TypeConverterAttribute), false);
			return customAttributes.Any<object>();
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00058BBC File Offset: 0x00056DBC
		internal static LanguagePrimitives.ConversionData FigureConversion(Type fromType, Type toType)
		{
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.GetConversionData(fromType, toType);
			if (conversionData != null)
			{
				return conversionData;
			}
			if (fromType == typeof(LanguagePrimitives.Null))
			{
				return LanguagePrimitives.FigureConversionFromNull(toType);
			}
			if (toType.IsAssignableFrom(fromType))
			{
				return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertAssignableFrom), (toType == fromType) ? ConversionRank.Identity : ConversionRank.Assignable);
			}
			if (typeof(PSObject).IsAssignableFrom(fromType) && typeof(LanguagePrimitives.InternalPSObject) != fromType)
			{
				return LanguagePrimitives.CacheConversion<object>(fromType, toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNoConversion), ConversionRank.None);
			}
			if (toType == typeof(PSObject))
			{
				return LanguagePrimitives.CacheConversion<PSObject>(fromType, toType, new LanguagePrimitives.PSConverter<PSObject>(LanguagePrimitives.ConvertToPSObject), ConversionRank.PSObject);
			}
			ConversionRank conversionRank = ConversionRank.None;
			LanguagePrimitives.PSConverter<object> psconverter;
			if (ExecutionContext.HasEverUsedConstrainedLanguage)
			{
				ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
				if (executionContextFromTLS != null && executionContextFromTLS.LanguageMode == PSLanguageMode.ConstrainedLanguage && toType != typeof(object) && toType != typeof(object[]) && !CoreTypes.Contains(toType))
				{
					psconverter = new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNotSupportedConversion);
					conversionRank = ConversionRank.None;
					return LanguagePrimitives.CacheConversion<object>(fromType, toType, psconverter, conversionRank);
				}
			}
			LanguagePrimitives.PSConverter<object> psconverter2 = null;
			ConversionRank conversionRank2 = ConversionRank.None;
			LanguagePrimitives.ConversionData conversionData2 = LanguagePrimitives.FigureLanguageConversion(fromType, toType, out psconverter2, out conversionRank2);
			if (conversionData2 != null)
			{
				return conversionData2;
			}
			conversionRank = ((psconverter2 != null) ? ConversionRank.Language : ConversionRank.None);
			psconverter = LanguagePrimitives.FigureParseConversion(fromType, toType);
			if (psconverter == null)
			{
				psconverter = LanguagePrimitives.FigureStaticCreateMethodConversion(fromType, toType);
				if (psconverter == null)
				{
					psconverter = LanguagePrimitives.FigureConstructorConversion(fromType, toType);
					conversionRank = ConversionRank.Constructor;
					if (psconverter == null)
					{
						psconverter = LanguagePrimitives.FigureCastConversion(fromType, toType, ref conversionRank);
						if (psconverter == null)
						{
							if (typeof(IConvertible).IsAssignableFrom(fromType))
							{
								if (LanguagePrimitives.IsNumeric(LanguagePrimitives.GetTypeCode(fromType)) && !fromType.GetTypeInfo().IsEnum)
								{
									if (!toType.IsArray && LanguagePrimitives.GetConversionRank(typeof(string), toType) != ConversionRank.None)
									{
										psconverter = new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNumericIConvertible);
										conversionRank = ConversionRank.IConvertible;
									}
								}
								else if (fromType != typeof(string))
								{
									psconverter = new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertIConvertible);
									conversionRank = ConversionRank.IConvertible;
								}
							}
							else if (typeof(IDictionary).IsAssignableFrom(fromType))
							{
								ConstructorInfo constructor = toType.GetConstructor(PSTypeExtensions.EmptyTypes);
								TypeInfo typeInfo = toType.GetTypeInfo();
								if (constructor != null || (typeInfo.IsValueType && !typeInfo.IsPrimitive))
								{
									LanguagePrimitives.ConvertViaNoArgumentConstructor @object = new LanguagePrimitives.ConvertViaNoArgumentConstructor(constructor, toType);
									psconverter = new LanguagePrimitives.PSConverter<object>(@object.Convert);
									conversionRank = ConversionRank.Constructor;
								}
							}
						}
					}
					else
					{
						conversionRank = ConversionRank.Constructor;
					}
				}
				else
				{
					conversionRank = ConversionRank.Create;
				}
			}
			else
			{
				conversionRank = ConversionRank.Parse;
			}
			if (psconverter == null)
			{
				Tuple<LanguagePrimitives.PSConverter<object>, ConversionRank> tuple = LanguagePrimitives.FigureIEnumerableConstructorConversion(fromType, toType);
				if (tuple != null)
				{
					psconverter = tuple.Item1;
					conversionRank = tuple.Item2;
				}
			}
			if (psconverter == null)
			{
				psconverter = LanguagePrimitives.FigurePropertyConversion(fromType, toType, ref conversionRank);
			}
			if (LanguagePrimitives.TypeConverterPossiblyExists(fromType) || LanguagePrimitives.TypeConverterPossiblyExists(toType) || (psconverter != null && psconverter2 != null))
			{
				psconverter = new LanguagePrimitives.PSConverter<object>(new LanguagePrimitives.ConvertCheckingForCustomConverter
				{
					tryfirstConverter = psconverter2,
					fallbackConverter = psconverter
				}.Convert);
				if (conversionRank2 > conversionRank)
				{
					conversionRank = conversionRank2;
				}
				else if (conversionRank == ConversionRank.None)
				{
					conversionRank = ConversionRank.Custom;
				}
			}
			else if (psconverter2 != null)
			{
				psconverter = psconverter2;
				conversionRank = conversionRank2;
			}
			if (psconverter == null)
			{
				psconverter = new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNoConversion);
				conversionRank = ConversionRank.None;
			}
			return LanguagePrimitives.CacheConversion<object>(fromType, toType, psconverter, conversionRank);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00058EE0 File Offset: 0x000570E0
		private static LanguagePrimitives.ConversionData FigureConversionFromNull(Type toType)
		{
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.GetConversionData(typeof(LanguagePrimitives.Null), toType);
			if (conversionData != null)
			{
				return conversionData;
			}
			if (Nullable.GetUnderlyingType(toType) != null)
			{
				return LanguagePrimitives.CacheConversion<object>(typeof(LanguagePrimitives.Null), toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNullToNullable), ConversionRank.NullToValue);
			}
			if (!toType.GetTypeInfo().IsValueType)
			{
				return LanguagePrimitives.CacheConversion<object>(typeof(LanguagePrimitives.Null), toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNullToRef), ConversionRank.NullToRef);
			}
			return LanguagePrimitives.CacheConversion<object>(typeof(LanguagePrimitives.Null), toType, new LanguagePrimitives.PSConverter<object>(LanguagePrimitives.ConvertNoConversion), ConversionRank.None);
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00058F80 File Offset: 0x00057180
		internal static string ObjectToTypeNameString(object o)
		{
			if (o == null)
			{
				return "null";
			}
			PSObject psobject = PSObject.AsPSObject(o);
			ConsolidatedString internalTypeNames = psobject.InternalTypeNames;
			if (internalTypeNames != null && internalTypeNames.Count > 0)
			{
				return internalTypeNames[0];
			}
			return ToStringCodeMethods.Type(o.GetType(), false);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00058FC4 File Offset: 0x000571C4
		private static Assembly AssemblyResolveHelper(object sender, ResolveEventArgs args)
		{
			foreach (Assembly assembly in ClrFacade.GetAssemblies(null))
			{
				if (assembly.FullName == args.Name)
				{
					return assembly;
				}
			}
			return null;
		}

		// Token: 0x040006AB RID: 1707
		internal const string OrderedAttribute = "ordered";

		// Token: 0x040006AC RID: 1708
		[TraceSource("ETS", "Extended Type System")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("ETS", "Extended Type System");

		// Token: 0x040006AD RID: 1709
		private static Dictionary<Type, LanguagePrimitives.GetEnumerableDelegate> getEnumerableCache = new Dictionary<Type, LanguagePrimitives.GetEnumerableDelegate>(32);

		// Token: 0x040006AE RID: 1710
		private static readonly CallSite<Func<CallSite, object, IEnumerator>> _getEnumeratorSite = CallSite<Func<CallSite, object, IEnumerator>>.Create(PSEnumerableBinder.Get());

		// Token: 0x040006AF RID: 1711
		internal static Type[][] LargestTypeTable;

		// Token: 0x040006B0 RID: 1712
		private static readonly LanguagePrimitives.TypeCodeTraits[] typeCodeTraits;

		// Token: 0x040006B1 RID: 1713
		internal static PSTraceSource typeConversion;

		// Token: 0x040006B2 RID: 1714
		private static Dictionary<string, string> nameMap;

		// Token: 0x040006B3 RID: 1715
		private static Dictionary<LanguagePrimitives.ConversionTypePair, LanguagePrimitives.ConversionData> converterCache;

		// Token: 0x040006B4 RID: 1716
		private static Type[] NumericTypes;

		// Token: 0x040006B5 RID: 1717
		private static Type[] IntegerTypes;

		// Token: 0x040006B6 RID: 1718
		private static Type[] SignedIntegerTypes;

		// Token: 0x040006B7 RID: 1719
		private static Type[] UnsignedIntegerTypes;

		// Token: 0x040006B8 RID: 1720
		private static Type[] RealTypes;

		// Token: 0x040006B9 RID: 1721
		private static Dictionary<string, bool> possibleTypeConverter;

		// Token: 0x0200011A RID: 282
		// (Invoke) Token: 0x06000F98 RID: 3992
		internal delegate void MemberNotFoundError(PSObject pso, DictionaryEntry property, Type resultType);

		// Token: 0x0200011B RID: 283
		// (Invoke) Token: 0x06000F9C RID: 3996
		internal delegate void MemberSetValueError(SetValueException e);

		// Token: 0x0200011C RID: 284
		private class EnumerableTWrapper : IEnumerable
		{
			// Token: 0x06000F9F RID: 3999 RVA: 0x00059024 File Offset: 0x00057224
			internal EnumerableTWrapper(object enumerable, Type enumerableType)
			{
				this._enumerable = enumerable;
				this._enumerableType = enumerableType;
				this.CreateGetEnumerator();
			}

			// Token: 0x06000FA0 RID: 4000 RVA: 0x00059040 File Offset: 0x00057240
			private void CreateGetEnumerator()
			{
				this._getEnumerator = new DynamicMethod("GetEnumerator", typeof(object), new Type[]
				{
					typeof(object)
				}, typeof(LanguagePrimitives).GetTypeInfo().Module, true);
				ILGenerator ilgenerator = this._getEnumerator.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Castclass, this._enumerableType);
				MethodInfo method = this._enumerableType.GetMethod("GetEnumerator", new Type[0]);
				ilgenerator.Emit(OpCodes.Callvirt, method);
				ilgenerator.Emit(OpCodes.Ret);
			}

			// Token: 0x06000FA1 RID: 4001 RVA: 0x000590E8 File Offset: 0x000572E8
			public IEnumerator GetEnumerator()
			{
				return (IEnumerator)this._getEnumerator.Invoke(null, new object[]
				{
					this._enumerable
				});
			}

			// Token: 0x040006BA RID: 1722
			private object _enumerable;

			// Token: 0x040006BB RID: 1723
			private Type _enumerableType;

			// Token: 0x040006BC RID: 1724
			private DynamicMethod _getEnumerator;
		}

		// Token: 0x0200011D RID: 285
		// (Invoke) Token: 0x06000FA3 RID: 4003
		private delegate IEnumerable GetEnumerableDelegate(object obj);

		// Token: 0x0200011E RID: 286
		[Flags]
		private enum TypeCodeTraits
		{
			// Token: 0x040006BE RID: 1726
			None = 0,
			// Token: 0x040006BF RID: 1727
			SignedInteger = 1,
			// Token: 0x040006C0 RID: 1728
			UnsignedInteger = 2,
			// Token: 0x040006C1 RID: 1729
			Floating = 4,
			// Token: 0x040006C2 RID: 1730
			CimIntrinsicType = 8,
			// Token: 0x040006C3 RID: 1731
			Decimal = 16,
			// Token: 0x040006C4 RID: 1732
			Integer = 3,
			// Token: 0x040006C5 RID: 1733
			Numeric = 23
		}

		// Token: 0x0200011F RID: 287
		internal class EnumSingleTypeConverter : PSTypeConverter
		{
			// Token: 0x06000FA6 RID: 4006 RVA: 0x00059118 File Offset: 0x00057318
			private static LanguagePrimitives.EnumSingleTypeConverter.EnumHashEntry GetEnumHashEntry(Type enumType)
			{
				LanguagePrimitives.EnumSingleTypeConverter.EnumHashEntry result;
				lock (LanguagePrimitives.EnumSingleTypeConverter.enumTable)
				{
					LanguagePrimitives.EnumSingleTypeConverter.EnumHashEntry enumHashEntry;
					if (LanguagePrimitives.EnumSingleTypeConverter.enumTable.TryGetValue(enumType, out enumHashEntry))
					{
						result = enumHashEntry;
					}
					else
					{
						if (LanguagePrimitives.EnumSingleTypeConverter.enumTable.Count == 100)
						{
							LanguagePrimitives.EnumSingleTypeConverter.enumTable.Clear();
						}
						ulong num = 0UL;
						bool hasNegativeValue = false;
						Array values = Enum.GetValues(enumType);
						if (LanguagePrimitives.IsSignedInteger(enumType.GetTypeCode()))
						{
							using (IEnumerator enumerator = values.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object value = enumerator.Current;
									long num2 = Convert.ToInt64(value, CultureInfo.CurrentCulture);
									if (num2 < 0L)
									{
										hasNegativeValue = true;
										break;
									}
									num |= Convert.ToUInt64(value, CultureInfo.CurrentCulture);
								}
								goto IL_F6;
							}
						}
						foreach (object value2 in values)
						{
							num |= Convert.ToUInt64(value2, CultureInfo.CurrentCulture);
						}
						IL_F6:
						bool hasFlagsAttribute = enumType.GetTypeInfo().GetCustomAttributes(typeof(FlagsAttribute), false).Any<object>();
						enumHashEntry = new LanguagePrimitives.EnumSingleTypeConverter.EnumHashEntry(Enum.GetNames(enumType), values, num, hasNegativeValue, hasFlagsAttribute);
						LanguagePrimitives.EnumSingleTypeConverter.enumTable.Add(enumType, enumHashEntry);
						result = enumHashEntry;
					}
				}
				return result;
			}

			// Token: 0x06000FA7 RID: 4007 RVA: 0x000592B4 File Offset: 0x000574B4
			public override bool CanConvertFrom(object sourceValue, Type destinationType)
			{
				return sourceValue is string && destinationType.GetTypeInfo().IsEnum;
			}

			// Token: 0x06000FA8 RID: 4008 RVA: 0x000592CC File Offset: 0x000574CC
			private static bool IsDefinedEnum(object enumValue, Type enumType)
			{
				bool result;
				if (enumValue == null)
				{
					result = false;
				}
				else
				{
					LanguagePrimitives.EnumSingleTypeConverter.EnumHashEntry enumHashEntry = LanguagePrimitives.EnumSingleTypeConverter.GetEnumHashEntry(enumType);
					if (enumHashEntry.hasNegativeValue)
					{
						result = true;
					}
					else
					{
						if (LanguagePrimitives.IsSignedInteger(enumValue.GetType().GetTypeCode()))
						{
							long num = Convert.ToInt64(enumValue, CultureInfo.CurrentCulture);
							if (num < 0L)
							{
								return false;
							}
						}
						ulong num2 = Convert.ToUInt64(enumValue, CultureInfo.CurrentCulture);
						if (enumHashEntry.hasFlagsAttribute)
						{
							result = (((num2 | enumHashEntry.allValues) ^ enumHashEntry.allValues) == 0UL);
						}
						else
						{
							result = (Array.IndexOf(enumHashEntry.values, enumValue) >= 0);
						}
					}
				}
				return result;
			}

			// Token: 0x06000FA9 RID: 4009 RVA: 0x0005935A File Offset: 0x0005755A
			internal static void ThrowForUndefinedEnum(string errorId, object enumValue, Type enumType)
			{
				LanguagePrimitives.EnumSingleTypeConverter.ThrowForUndefinedEnum(errorId, enumValue, enumValue, enumType);
			}

			// Token: 0x06000FAA RID: 4010 RVA: 0x00059368 File Offset: 0x00057568
			internal static void ThrowForUndefinedEnum(string errorId, object enumValue, object valueToUseToThrow, Type enumType)
			{
				if (!LanguagePrimitives.EnumSingleTypeConverter.IsDefinedEnum(enumValue, enumType))
				{
					LanguagePrimitives.typeConversion.WriteLine("Value {0} is not defined in the Enum {1}.", new object[]
					{
						valueToUseToThrow,
						enumType
					});
					throw new PSInvalidCastException(errorId, null, ExtendedTypeSystem.InvalidCastExceptionEnumerationNoValue, new object[]
					{
						valueToUseToThrow,
						enumType,
						LanguagePrimitives.EnumSingleTypeConverter.EnumValues(enumType)
					});
				}
			}

			// Token: 0x06000FAB RID: 4011 RVA: 0x000593C4 File Offset: 0x000575C4
			internal static string EnumValues(Type enumType)
			{
				LanguagePrimitives.EnumSingleTypeConverter.EnumHashEntry enumHashEntry = LanguagePrimitives.EnumSingleTypeConverter.GetEnumHashEntry(enumType);
				return string.Join(CultureInfo.CurrentUICulture.TextInfo.ListSeparator, enumHashEntry.names);
			}

			// Token: 0x06000FAC RID: 4012 RVA: 0x000593F2 File Offset: 0x000575F2
			public override object ConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
			{
				return LanguagePrimitives.EnumSingleTypeConverter.BaseConvertFrom(sourceValue, destinationType, formatProvider, ignoreCase, false);
			}

			// Token: 0x06000FAD RID: 4013 RVA: 0x00059400 File Offset: 0x00057600
			protected static object BaseConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase, bool multipleValues)
			{
				string text = sourceValue as string;
				if (text == null)
				{
					throw new PSInvalidCastException("InvalidCastEnumFromTypeNotAString", null, ExtendedTypeSystem.InvalidCastException, new object[]
					{
						sourceValue,
						LanguagePrimitives.ObjectToTypeNameString(sourceValue),
						destinationType
					});
				}
				if (text.Length == 0)
				{
					throw new PSInvalidCastException("InvalidCastEnumFromEmptyString", null, ExtendedTypeSystem.InvalidCastException, new object[]
					{
						sourceValue,
						LanguagePrimitives.ObjectToTypeNameString(sourceValue),
						destinationType
					});
				}
				text = text.Trim();
				if (text.Length == 0)
				{
					throw new PSInvalidCastException("InvalidEnumCastFromEmptyStringAfterTrim", null, ExtendedTypeSystem.InvalidCastException, new object[]
					{
						sourceValue,
						LanguagePrimitives.ObjectToTypeNameString(sourceValue),
						destinationType
					});
				}
				if (char.IsDigit(text[0]) || text[0] == '+' || text[0] == '-')
				{
					Type underlyingType = Enum.GetUnderlyingType(destinationType);
					try
					{
						object obj = Enum.ToObject(destinationType, Convert.ChangeType(text, underlyingType, formatProvider));
						LanguagePrimitives.EnumSingleTypeConverter.ThrowForUndefinedEnum("UndefinedInEnumSingleTypeConverter", obj, text, destinationType);
						return obj;
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				}
				string[] array;
				WildcardPattern[] array2;
				if (!multipleValues)
				{
					if (text.Contains(","))
					{
						throw new PSInvalidCastException("InvalidCastEnumCommaAndNoFlags", null, ExtendedTypeSystem.InvalidCastExceptionEnumerationNoFlagAndComma, new object[]
						{
							sourceValue,
							destinationType
						});
					}
					array = new string[]
					{
						text
					};
					array2 = new WildcardPattern[1];
					if (WildcardPattern.ContainsWildcardCharacters(text))
					{
						array2[0] = new WildcardPattern(text, ignoreCase ? WildcardOptions.IgnoreCase : WildcardOptions.None);
					}
					else
					{
						array2[0] = null;
					}
				}
				else
				{
					array = text.Split(new char[]
					{
						','
					});
					array2 = new WildcardPattern[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						string pattern = array[i];
						if (WildcardPattern.ContainsWildcardCharacters(pattern))
						{
							array2[i] = new WildcardPattern(pattern, ignoreCase ? WildcardOptions.IgnoreCase : WildcardOptions.None);
						}
						else
						{
							array2[i] = null;
						}
					}
				}
				LanguagePrimitives.EnumSingleTypeConverter.EnumHashEntry enumHashEntry = LanguagePrimitives.EnumSingleTypeConverter.GetEnumHashEntry(destinationType);
				string[] names = enumHashEntry.names;
				Array values = enumHashEntry.values;
				ulong num = 0UL;
				StringComparison comparisonType;
				if (ignoreCase)
				{
					comparisonType = StringComparison.OrdinalIgnoreCase;
				}
				else
				{
					comparisonType = StringComparison.Ordinal;
				}
				for (int j = 0; j < array.Length; j++)
				{
					string text2 = array[j];
					WildcardPattern wildcardPattern = array2[j];
					bool flag = false;
					int k = 0;
					while (k < names.Length)
					{
						string text3 = names[k];
						if (wildcardPattern != null)
						{
							if (wildcardPattern.IsMatch(text3))
							{
								goto IL_24A;
							}
						}
						else if (string.Compare(text2, text3, comparisonType) == 0)
						{
							goto IL_24A;
						}
						IL_2C3:
						k++;
						continue;
						IL_24A:
						if (!multipleValues && flag)
						{
							object obj2 = Enum.ToObject(destinationType, num);
							object obj3 = Enum.ToObject(destinationType, Convert.ToUInt64(values.GetValue(j), CultureInfo.CurrentCulture));
							throw new PSInvalidCastException("InvalidCastEnumTwoStringsFoundAndNoFlags", null, ExtendedTypeSystem.InvalidCastExceptionEnumerationMoreThanOneValue, new object[]
							{
								sourceValue,
								destinationType,
								obj2,
								obj3
							});
						}
						flag = true;
						num |= Convert.ToUInt64(values.GetValue(k), CultureInfo.CurrentCulture);
						goto IL_2C3;
					}
					if (!flag)
					{
						throw new PSInvalidCastException("InvalidCastEnumStringNotFound", null, ExtendedTypeSystem.InvalidCastExceptionEnumerationNoValue, new object[]
						{
							text2,
							destinationType,
							LanguagePrimitives.EnumSingleTypeConverter.EnumValues(destinationType)
						});
					}
				}
				return Enum.ToObject(destinationType, num);
			}

			// Token: 0x06000FAE RID: 4014 RVA: 0x00059744 File Offset: 0x00057944
			public override bool CanConvertTo(object sourceValue, Type destinationType)
			{
				return false;
			}

			// Token: 0x06000FAF RID: 4015 RVA: 0x00059747 File Offset: 0x00057947
			public override object ConvertTo(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
			{
				throw PSTraceSource.NewNotSupportedException();
			}

			// Token: 0x040006C6 RID: 1734
			private const int maxEnumTableSize = 100;

			// Token: 0x040006C7 RID: 1735
			private static readonly Dictionary<Type, LanguagePrimitives.EnumSingleTypeConverter.EnumHashEntry> enumTable = new Dictionary<Type, LanguagePrimitives.EnumSingleTypeConverter.EnumHashEntry>();

			// Token: 0x02000120 RID: 288
			private class EnumHashEntry
			{
				// Token: 0x06000FB2 RID: 4018 RVA: 0x00059762 File Offset: 0x00057962
				internal EnumHashEntry(string[] names, Array values, ulong allValues, bool hasNegativeValue, bool hasFlagsAttribute)
				{
					this.names = names;
					this.values = values;
					this.allValues = allValues;
					this.hasNegativeValue = hasNegativeValue;
					this.hasFlagsAttribute = hasFlagsAttribute;
				}

				// Token: 0x040006C8 RID: 1736
				internal string[] names;

				// Token: 0x040006C9 RID: 1737
				internal Array values;

				// Token: 0x040006CA RID: 1738
				internal ulong allValues;

				// Token: 0x040006CB RID: 1739
				internal bool hasNegativeValue;

				// Token: 0x040006CC RID: 1740
				internal bool hasFlagsAttribute;
			}
		}

		// Token: 0x02000121 RID: 289
		internal class EnumMultipleTypeConverter : LanguagePrimitives.EnumSingleTypeConverter
		{
			// Token: 0x06000FB3 RID: 4019 RVA: 0x0005978F File Offset: 0x0005798F
			public override object ConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
			{
				return LanguagePrimitives.EnumSingleTypeConverter.BaseConvertFrom(sourceValue, destinationType, formatProvider, ignoreCase, true);
			}
		}

		// Token: 0x02000122 RID: 290
		private class ConvertViaParseMethod
		{
			// Token: 0x06000FB5 RID: 4021 RVA: 0x000597A4 File Offset: 0x000579A4
			internal object ConvertWithCulture(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
			{
				object result;
				try
				{
					object obj = this.parse.Invoke(null, new object[]
					{
						valueToConvert,
						formatProvider
					});
					LanguagePrimitives.typeConversion.WriteLine("Parse result: {0}", new object[]
					{
						obj
					});
					result = obj;
				}
				catch (TargetInvocationException ex)
				{
					Exception ex2 = ex.InnerException ?? ex;
					LanguagePrimitives.typeConversion.WriteLine("Exception calling Parse method with CultureInfo: \"{0}\".", new object[]
					{
						ex2.Message
					});
					throw new PSInvalidCastException("InvalidCastParseTargetInvocationWithFormatProvider", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex2.Message
					});
				}
				catch (Exception ex3)
				{
					CommandProcessorBase.CheckForSevereException(ex3);
					LanguagePrimitives.typeConversion.WriteLine("Exception calling Parse method with CultureInfo: \"{0}\".", new object[]
					{
						ex3.Message
					});
					throw new PSInvalidCastException("InvalidCastParseExceptionWithFormatProvider", ex3, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex3.Message
					});
				}
				return result;
			}

			// Token: 0x06000FB6 RID: 4022 RVA: 0x000598E4 File Offset: 0x00057AE4
			internal object ConvertWithoutCulture(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
			{
				object result;
				try
				{
					object obj = this.parse.Invoke(null, new object[]
					{
						valueToConvert
					});
					LanguagePrimitives.typeConversion.WriteLine("Parse result: \"{0}\".", new object[]
					{
						obj
					});
					result = obj;
				}
				catch (TargetInvocationException ex)
				{
					Exception ex2 = ex.InnerException ?? ex;
					LanguagePrimitives.typeConversion.WriteLine("Exception calling Parse method: \"{0}\".", new object[]
					{
						ex2.Message
					});
					throw new PSInvalidCastException("InvalidCastParseTargetInvocation", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex2.Message
					});
				}
				catch (Exception ex3)
				{
					CommandProcessorBase.CheckForSevereException(ex3);
					LanguagePrimitives.typeConversion.WriteLine("Exception calling Parse method: \"{0}\".", new object[]
					{
						ex3.Message
					});
					throw new PSInvalidCastException("InvalidCastParseException", ex3, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex3.Message
					});
				}
				return result;
			}

			// Token: 0x040006CD RID: 1741
			internal MethodInfo parse;
		}

		// Token: 0x02000123 RID: 291
		private class ConvertViaConstructor
		{
			// Token: 0x06000FB8 RID: 4024 RVA: 0x00059A28 File Offset: 0x00057C28
			internal object Convert(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
			{
				object result;
				try
				{
					object obj = this.TargetCtorLambda(valueToConvert);
					LanguagePrimitives.typeConversion.WriteLine("Constructor result: \"{0}\".", new object[]
					{
						obj
					});
					result = obj;
				}
				catch (TargetInvocationException ex)
				{
					Exception ex2 = ex.InnerException ?? ex;
					LanguagePrimitives.typeConversion.WriteLine("Exception invoking Constructor: \"{0}\".", new object[]
					{
						ex2.Message
					});
					throw new PSInvalidCastException("InvalidCastConstructorTargetInvocationException", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex2.Message
					});
				}
				catch (Exception ex3)
				{
					CommandProcessorBase.CheckForSevereException(ex3);
					LanguagePrimitives.typeConversion.WriteLine("Exception invoking Constructor: \"{0}\".", new object[]
					{
						ex3.Message
					});
					throw new PSInvalidCastException("InvalidCastConstructorException", ex3, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex3.Message
					});
				}
				return result;
			}

			// Token: 0x040006CE RID: 1742
			internal Func<object, object> TargetCtorLambda;
		}

		// Token: 0x02000124 RID: 292
		private class ConvertViaIEnumerableConstructor
		{
			// Token: 0x06000FBA RID: 4026 RVA: 0x00059B5C File Offset: 0x00057D5C
			internal object Convert(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
			{
				IList list = null;
				int num = 0;
				Array array = null;
				try
				{
					if (this.IsScalar || !(valueToConvert is Array))
					{
						num = 1;
					}
					else
					{
						array = (Array)valueToConvert;
						num = array.Length;
					}
					list = this.ListCtorLambda(num);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					LanguagePrimitives.ThrowInvalidCastException(valueToConvert, resultType);
					return null;
				}
				if (this.IsScalar)
				{
					list.Add(valueToConvert);
				}
				else if (num == 1)
				{
					object value = LanguagePrimitives.ConvertTo(valueToConvert, this.ElementType, formatProvider);
					list.Add(value);
				}
				else
				{
					foreach (object obj in array)
					{
						object valueToConvert2 = PSObject.Base(obj);
						object value2;
						if (!LanguagePrimitives.TryConvertTo(valueToConvert2, this.ElementType, formatProvider, out value2))
						{
							LanguagePrimitives.ThrowInvalidCastException(valueToConvert, resultType);
							return null;
						}
						list.Add(value2);
					}
				}
				object result;
				try
				{
					object obj2 = this.TargetCtorLambda(list);
					LanguagePrimitives.typeConversion.WriteLine("IEnumerable Constructor result: \"{0}\".", new object[]
					{
						obj2
					});
					result = obj2;
				}
				catch (TargetInvocationException ex)
				{
					Exception ex2 = ex.InnerException ?? ex;
					LanguagePrimitives.typeConversion.WriteLine("Exception invoking IEnumerable Constructor: \"{0}\".", new object[]
					{
						ex2.Message
					});
					throw new PSInvalidCastException("InvalidCastConstructorTargetInvocationException", ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex2.Message
					});
				}
				catch (Exception ex3)
				{
					CommandProcessorBase.CheckForSevereException(ex3);
					LanguagePrimitives.typeConversion.WriteLine("Exception invoking IEnumerable Constructor: \"{0}\".", new object[]
					{
						ex3.Message
					});
					throw new PSInvalidCastException("InvalidCastConstructorException", ex3, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex3.Message
					});
				}
				return result;
			}

			// Token: 0x040006CF RID: 1743
			internal Func<int, IList> ListCtorLambda;

			// Token: 0x040006D0 RID: 1744
			internal Func<IList, object> TargetCtorLambda;

			// Token: 0x040006D1 RID: 1745
			internal Type ElementType;

			// Token: 0x040006D2 RID: 1746
			internal bool IsScalar;
		}

		// Token: 0x02000125 RID: 293
		private class ConvertViaNoArgumentConstructor
		{
			// Token: 0x06000FBC RID: 4028 RVA: 0x00059DA0 File Offset: 0x00057FA0
			internal ConvertViaNoArgumentConstructor(ConstructorInfo constructor, Type type)
			{
				NewExpression expr = (constructor != null) ? Expression.New(constructor) : Expression.New(type);
				this.constructor = Expression.Lambda<Func<object>>(expr.Cast(typeof(object)), new ParameterExpression[0]).Compile();
			}

			// Token: 0x06000FBD RID: 4029 RVA: 0x00059DF1 File Offset: 0x00057FF1
			internal object Convert(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
			{
				return this.Convert(valueToConvert, resultType, recursion, originalValueToConvert, formatProvider, backupTable, false);
			}

			// Token: 0x06000FBE RID: 4030 RVA: 0x00059E04 File Offset: 0x00058004
			internal object Convert(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable, bool ignoreUnknownMembers)
			{
				object result;
				try
				{
					ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
					if (executionContextFromTLS != null && (executionContextFromTLS == null || executionContextFromTLS.LanguageMode != PSLanguageMode.FullLanguage))
					{
						RuntimeException ex = InterpreterError.NewInterpreterException(valueToConvert, typeof(RuntimeException), null, "HashtableToObjectConversionNotSupportedInDataSection", ParserStrings.HashtableToObjectConversionNotSupportedInDataSection, new object[]
						{
							resultType.ToString()
						});
						throw ex;
					}
					object obj = this.constructor();
					PSObject psobject = valueToConvert as PSObject;
					if (psobject != null)
					{
						LanguagePrimitives.SetObjectProperties(obj, psobject, resultType, new LanguagePrimitives.MemberNotFoundError(LanguagePrimitives.CreateMemberNotFoundError), new LanguagePrimitives.MemberSetValueError(LanguagePrimitives.CreateMemberSetValueError), formatProvider, recursion, ignoreUnknownMembers);
					}
					else
					{
						IDictionary properties = valueToConvert as IDictionary;
						LanguagePrimitives.SetObjectProperties(obj, properties, resultType, new LanguagePrimitives.MemberNotFoundError(LanguagePrimitives.CreateMemberNotFoundError), new LanguagePrimitives.MemberSetValueError(LanguagePrimitives.CreateMemberSetValueError), false);
					}
					LanguagePrimitives.typeConversion.WriteLine("Constructor result: \"{0}\".", new object[]
					{
						obj
					});
					result = obj;
				}
				catch (TargetInvocationException ex2)
				{
					Exception ex3 = ex2.InnerException ?? ex2;
					LanguagePrimitives.typeConversion.WriteLine("Exception invoking Constructor: \"{0}\".", new object[]
					{
						ex3.Message
					});
					throw new PSInvalidCastException("InvalidCastConstructorTargetInvocationException", ex3, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex3.Message
					});
				}
				catch (InvalidOperationException ex4)
				{
					Exception ex5 = ex4.InnerException ?? ex4;
					throw new PSInvalidCastException("ObjectCreationError", ex4, ExtendedTypeSystem.ObjectCreationError, new object[]
					{
						resultType.ToString(),
						ex5.Message
					});
				}
				catch (SetValueException ex6)
				{
					Exception ex7 = ex6.InnerException ?? ex6;
					throw new PSInvalidCastException("ObjectCreationError", ex7, ExtendedTypeSystem.ObjectCreationError, new object[]
					{
						resultType.ToString(),
						ex7.Message
					});
				}
				catch (RuntimeException ex8)
				{
					Exception ex9 = ex8.InnerException ?? ex8;
					LanguagePrimitives.typeConversion.WriteLine("Exception invoking Constructor: \"{0}\".", new object[]
					{
						ex9.Message
					});
					throw new PSInvalidCastException("InvalidCastConstructorException", ex9, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex9.Message
					});
				}
				catch (Exception ex10)
				{
					CommandProcessorBase.CheckForSevereException(ex10);
					LanguagePrimitives.typeConversion.WriteLine("Exception invoking Constructor: \"{0}\".", new object[]
					{
						ex10.Message
					});
					throw new PSInvalidCastException("InvalidCastConstructorException", ex10, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex10.Message
					});
				}
				return result;
			}

			// Token: 0x040006D3 RID: 1747
			private readonly Func<object> constructor;
		}

		// Token: 0x02000126 RID: 294
		private class ConvertViaCast
		{
			// Token: 0x06000FBF RID: 4031 RVA: 0x0005A100 File Offset: 0x00058300
			internal object Convert(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
			{
				object result;
				try
				{
					result = this.cast.Invoke(null, new object[]
					{
						valueToConvert
					});
				}
				catch (TargetInvocationException ex)
				{
					Exception ex2 = ex.InnerException ?? ex;
					LanguagePrimitives.typeConversion.WriteLine("Cast operator exception: \"{0}\".", new object[]
					{
						ex2.Message
					});
					throw new PSInvalidCastException("InvalidCastTargetInvocationException" + this.cast.Name, ex2, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex2.Message
					});
				}
				catch (Exception ex3)
				{
					CommandProcessorBase.CheckForSevereException(ex3);
					LanguagePrimitives.typeConversion.WriteLine("Cast operator exception: \"{0}\".", new object[]
					{
						ex3.Message
					});
					throw new PSInvalidCastException("InvalidCastException" + this.cast.Name, ex3, ExtendedTypeSystem.InvalidCastExceptionWithInnerException, new object[]
					{
						valueToConvert.ToString(),
						resultType.ToString(),
						ex3.Message
					});
				}
				return result;
			}

			// Token: 0x040006D4 RID: 1748
			internal MethodInfo cast;
		}

		// Token: 0x02000127 RID: 295
		private class ConvertCheckingForCustomConverter
		{
			// Token: 0x06000FC1 RID: 4033 RVA: 0x0005A240 File Offset: 0x00058440
			internal object Convert(object valueToConvert, Type resultType, bool recursion, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
			{
				object result = null;
				if (this.tryfirstConverter != null)
				{
					try
					{
						return this.tryfirstConverter(valueToConvert, resultType, recursion, originalValueToConvert, formatProvider, backupTable);
					}
					catch (InvalidCastException)
					{
					}
				}
				if (LanguagePrimitives.IsCustomTypeConversion(originalValueToConvert ?? valueToConvert, resultType, formatProvider, out result, backupTable))
				{
					LanguagePrimitives.typeConversion.WriteLine("Custom Type Conversion succeeded.", new object[0]);
					return result;
				}
				if (this.fallbackConverter != null)
				{
					return this.fallbackConverter(valueToConvert, resultType, recursion, originalValueToConvert, formatProvider, backupTable);
				}
				throw new PSInvalidCastException("ConvertToFinalInvalidCastException", null, ExtendedTypeSystem.InvalidCastException, new object[]
				{
					valueToConvert.ToString(),
					LanguagePrimitives.ObjectToTypeNameString(valueToConvert),
					resultType.ToString()
				});
			}

			// Token: 0x040006D5 RID: 1749
			internal LanguagePrimitives.PSConverter<object> tryfirstConverter;

			// Token: 0x040006D6 RID: 1750
			internal LanguagePrimitives.PSConverter<object> fallbackConverter;
		}

		// Token: 0x02000128 RID: 296
		private class ConversionTypePair
		{
			// Token: 0x170003D1 RID: 977
			// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0005A30C File Offset: 0x0005850C
			// (set) Token: 0x06000FC4 RID: 4036 RVA: 0x0005A314 File Offset: 0x00058514
			internal Type from { get; private set; }

			// Token: 0x170003D2 RID: 978
			// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0005A31D File Offset: 0x0005851D
			// (set) Token: 0x06000FC6 RID: 4038 RVA: 0x0005A325 File Offset: 0x00058525
			internal Type to { get; private set; }

			// Token: 0x06000FC7 RID: 4039 RVA: 0x0005A32E File Offset: 0x0005852E
			internal ConversionTypePair(Type from, Type to)
			{
				this.from = from;
				this.to = to;
			}

			// Token: 0x06000FC8 RID: 4040 RVA: 0x0005A344 File Offset: 0x00058544
			public override int GetHashCode()
			{
				return this.from.GetHashCode() + 37 * this.to.GetHashCode();
			}

			// Token: 0x06000FC9 RID: 4041 RVA: 0x0005A360 File Offset: 0x00058560
			public override bool Equals(object other)
			{
				LanguagePrimitives.ConversionTypePair conversionTypePair = other as LanguagePrimitives.ConversionTypePair;
				return conversionTypePair != null && this.from == conversionTypePair.from && this.to == conversionTypePair.to;
			}
		}

		// Token: 0x02000129 RID: 297
		// (Invoke) Token: 0x06000FCB RID: 4043
		internal delegate T PSConverter<T>(object valueToConvert, Type resultType, bool recurse, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable);

		// Token: 0x0200012A RID: 298
		// (Invoke) Token: 0x06000FCF RID: 4047
		internal delegate object PSNullConverter(object nullOrAutomationNull);

		// Token: 0x0200012B RID: 299
		internal interface ConversionData
		{
			// Token: 0x170003D3 RID: 979
			// (get) Token: 0x06000FD2 RID: 4050
			object Converter { get; }

			// Token: 0x170003D4 RID: 980
			// (get) Token: 0x06000FD3 RID: 4051
			ConversionRank Rank { get; }

			// Token: 0x06000FD4 RID: 4052
			object Invoke(object valueToConvert, Type resultType, bool recurse, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable);
		}

		// Token: 0x0200012C RID: 300
		internal class ConversionData<T> : LanguagePrimitives.ConversionData
		{
			// Token: 0x06000FD5 RID: 4053 RVA: 0x0005A39F File Offset: 0x0005859F
			public ConversionData(LanguagePrimitives.PSConverter<T> converter, ConversionRank rank)
			{
				this.converter = converter;
				this.rank = rank;
			}

			// Token: 0x170003D5 RID: 981
			// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x0005A3B5 File Offset: 0x000585B5
			public object Converter
			{
				get
				{
					return this.converter;
				}
			}

			// Token: 0x170003D6 RID: 982
			// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x0005A3BD File Offset: 0x000585BD
			public ConversionRank Rank
			{
				get
				{
					return this.rank;
				}
			}

			// Token: 0x06000FD8 RID: 4056 RVA: 0x0005A3C5 File Offset: 0x000585C5
			public object Invoke(object valueToConvert, Type resultType, bool recurse, PSObject originalValueToConvert, IFormatProvider formatProvider, TypeTable backupTable)
			{
				return this.converter(valueToConvert, resultType, recurse, originalValueToConvert, formatProvider, backupTable);
			}

			// Token: 0x040006D9 RID: 1753
			private readonly LanguagePrimitives.PSConverter<T> converter;

			// Token: 0x040006DA RID: 1754
			private readonly ConversionRank rank;
		}

		// Token: 0x0200012D RID: 301
		internal class InternalPSCustomObject
		{
		}

		// Token: 0x02000131 RID: 305
		internal class InternalPSObject : PSObject
		{
		}

		// Token: 0x02000132 RID: 306
		internal class Null
		{
		}
	}
}
