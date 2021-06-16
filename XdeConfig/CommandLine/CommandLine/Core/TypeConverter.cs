using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CommandLine.Infrastructure;
using CSharpx;
using RailwaySharp.ErrorHandling;

namespace CommandLine.Core
{
	// Token: 0x02000084 RID: 132
	internal static class TypeConverter
	{
		// Token: 0x06000317 RID: 791 RVA: 0x0000C5CA File Offset: 0x0000A7CA
		public static Maybe<object> ChangeType(IEnumerable<string> values, Type conversionType, bool scalar, CultureInfo conversionCulture, bool ignoreValueCase)
		{
			if (!scalar)
			{
				return TypeConverter.ChangeTypeSequence(values, conversionType, conversionCulture, ignoreValueCase);
			}
			return TypeConverter.ChangeTypeScalar(values.Single<string>(), conversionType, conversionCulture, ignoreValueCase);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000C5EC File Offset: 0x0000A7EC
		private static Maybe<object> ChangeTypeSequence(IEnumerable<string> values, Type conversionType, CultureInfo conversionCulture, bool ignoreValueCase)
		{
			Type type = conversionType.GetTypeInfo().GetGenericArguments().SingleOrDefault<Type>().ToMaybe<Type>().FromJustOrFail(new InvalidOperationException("Non scalar properties should be sequence of type IEnumerable<T>."));
			IEnumerable<Maybe<object>> source = from value in values
			select TypeConverter.ChangeTypeScalar(value, type, conversionCulture, ignoreValueCase);
			if (!source.Any((Maybe<object> a) => a.MatchNothing()))
			{
				return Maybe.Just<object>((from c in source
				select ((Just<object>)c).Value).ToUntypedArray(type));
			}
			return Maybe.Nothing<object>();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000C6AC File Offset: 0x0000A8AC
		private static Maybe<object> ChangeTypeScalar(string value, Type conversionType, CultureInfo conversionCulture, bool ignoreValueCase)
		{
			Result<object, Exception> result = TypeConverter.ChangeTypeScalarImpl(value, conversionType, conversionCulture, ignoreValueCase);
			result.Match(delegate(object _, IEnumerable<Exception> __)
			{
			}, delegate(IEnumerable<Exception> e)
			{
				e.First<Exception>().RethrowWhenAbsentIn(new Type[]
				{
					typeof(InvalidCastException),
					typeof(FormatException),
					typeof(OverflowException)
				});
			});
			return result.ToMaybe<object, Exception>();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000C70C File Offset: 0x0000A90C
		private static object ConvertString(string value, Type type, CultureInfo conversionCulture)
		{
			object result;
			try
			{
				result = Convert.ChangeType(value, type, conversionCulture);
			}
			catch (InvalidCastException)
			{
				result = TypeDescriptor.GetConverter(type).ConvertFrom(null, conversionCulture, value);
			}
			return result;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000C748 File Offset: 0x0000A948
		private static Result<object, Exception> ChangeTypeScalarImpl(string value, Type conversionType, CultureInfo conversionCulture, bool ignoreValueCase)
		{
			Func<Type> <>9__3;
			Func<object> <>9__2;
			Func<object> func = delegate()
			{
				Func<object> func3;
				if ((func3 = <>9__2) == null)
				{
					func3 = (<>9__2 = delegate()
					{
						ReflectionHelper.IsFSharpOptionType(conversionType);
						Func<Type> func5;
						if ((func5 = <>9__3) == null)
						{
							func5 = (<>9__3 = (() => Nullable.GetUnderlyingType(conversionType)));
						}
						Func<Type> func6 = func5;
						Type type = func6() ?? conversionType;
						Func<object> func7 = () => TypeConverter.ConvertString(value, type, conversionCulture);
						Func<object> func8 = () => null;
						if (value != null)
						{
							return func7();
						}
						return func8();
					});
				}
				Func<object> func4 = func3;
				if (value.IsBooleanString() && conversionType == typeof(bool))
				{
					return value.ToBoolean();
				}
				if (!conversionType.GetTypeInfo().IsEnum)
				{
					return func4();
				}
				return value.ToEnum(conversionType, ignoreValueCase);
			};
			Func<object> func2 = delegate()
			{
				object result;
				try
				{
					result = conversionType.GetTypeInfo().GetConstructor(new Type[]
					{
						typeof(string)
					}).Invoke(new object[]
					{
						value
					});
				}
				catch (Exception)
				{
					throw new FormatException("Destination conversion type must have a constructor that accepts a string.");
				}
				return result;
			};
			return Result.Try<object>((conversionType.IsPrimitiveEx() || ReflectionHelper.IsFSharpOptionType(conversionType)) ? func : func2);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000C7B4 File Offset: 0x0000A9B4
		private static object ToEnum(this string value, Type conversionType, bool ignoreValueCase)
		{
			object obj;
			try
			{
				obj = Enum.Parse(conversionType, value, ignoreValueCase);
			}
			catch (ArgumentException)
			{
				throw new FormatException();
			}
			if (Enum.IsDefined(conversionType, obj))
			{
				return obj;
			}
			throw new FormatException();
		}
	}
}
