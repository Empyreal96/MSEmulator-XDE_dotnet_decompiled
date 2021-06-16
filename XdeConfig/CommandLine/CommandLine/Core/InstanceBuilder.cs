using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CommandLine.Infrastructure;
using CSharpx;
using RailwaySharp.ErrorHandling;

namespace CommandLine.Core
{
	// Token: 0x02000068 RID: 104
	internal static class InstanceBuilder
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x0000A81C File Offset: 0x00008A1C
		public static ParserResult<T> Build<T>(Maybe<Func<T>> factory, Func<IEnumerable<string>, IEnumerable<OptionSpecification>, Result<IEnumerable<Token>, Error>> tokenizer, IEnumerable<string> arguments, StringComparer nameComparer, bool ignoreValueCase, CultureInfo parsingCulture, bool autoHelp, bool autoVersion, IEnumerable<ErrorType> nonFatalErrors)
		{
			Type typeInfo = factory.MapValueOrDefault(delegate(Func<T> f)
			{
				T t = f();
				return t.GetType();
			}, typeof(T));
			IEnumerable<SpecificationProperty> specProps = typeInfo.GetSpecifications((PropertyInfo pi) => SpecificationProperty.Create(Specification.FromProperty(pi), pi, Maybe.Nothing<object>())).Memorize<SpecificationProperty>();
			IEnumerable<Specification> specifications = from pt in specProps
			select pt.Specification;
			IEnumerable<OptionSpecification> optionSpecs = specifications.ThrowingValidate(SpecificationGuards.Lookup).OfType<OptionSpecification>().Memorize<OptionSpecification>();
			Func<T> makeDefault = delegate()
			{
				if (!typeof(T).IsMutable())
				{
					return ReflectionHelper.CreateDefaultImmutableInstance<T>((from p in specProps
					select p.Specification.ConversionType).ToArray<Type>());
				}
				return factory.MapValueOrDefault((Func<T> f) => f(), Activator.CreateInstance<T>());
			};
			Func<IEnumerable<Error>, ParserResult<T>> func = delegate(IEnumerable<Error> errs)
			{
				T t = makeDefault();
				return new NotParsed<T>(t.GetType().ToTypeInfo(), errs);
			};
			IEnumerable<string> argumentsList = arguments.Memorize<string>();
			Func<string, Maybe<TypeDescriptor>> <>9__8;
			Func<IEnumerable<string>, Type, bool, Maybe<object>> <>9__10;
			Func<IEnumerable<string>, Type, bool, Maybe<object>> <>9__13;
			Func<Token, MissingValueOptionError> <>9__14;
			Func<Error, bool> <>9__15;
			Func<ParserResult<T>> func2 = delegate()
			{
				Result<IEnumerable<Token>, Error> result = tokenizer(argumentsList, optionSpecs);
				IEnumerable<Token> enumerable2 = result.SucceededWith<IEnumerable<Token>, Error>().Memorize<Token>();
				IEnumerable<Token> tokens = enumerable2;
				Func<string, Maybe<TypeDescriptor>> typeLookup;
				if ((typeLookup = <>9__8) == null)
				{
					typeLookup = (<>9__8 = ((string name) => TypeLookup.FindTypeDescriptorAndSibling(name, optionSpecs, nameComparer)));
				}
				Tuple<IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable<string>, IEnumerable<Token>> tuple = TokenPartitioner.Partition(tokens, typeLookup);
				IEnumerable<KeyValuePair<string, IEnumerable<string>>> enumerable3 = tuple.Item1.Memorize<KeyValuePair<string, IEnumerable<string>>>();
				IEnumerable<string> enumerable4 = tuple.Item2.Memorize<string>();
				IEnumerable<Token> source = tuple.Item3.Memorize<Token>();
				IEnumerable<SpecificationProperty> specProps;
				IEnumerable<SpecificationProperty> propertyTuples = from pt in specProps
				where pt.Specification.IsOption()
				select pt;
				IEnumerable<KeyValuePair<string, IEnumerable<string>>> options = enumerable3;
				Func<IEnumerable<string>, Type, bool, Maybe<object>> converter;
				if ((converter = <>9__10) == null)
				{
					converter = (<>9__10 = ((IEnumerable<string> vals, Type type, bool isScalar) => TypeConverter.ChangeType(vals, type, isScalar, parsingCulture, ignoreValueCase)));
				}
				Result<IEnumerable<SpecificationProperty>, Error> result2 = OptionMapper.MapValues(propertyTuples, options, converter, nameComparer);
				specProps = from pt in specProps
				where pt.Specification.IsValue()
				orderby ((ValueSpecification)pt.Specification).Index
				select pt;
				IEnumerable<string> values = enumerable4;
				Func<IEnumerable<string>, Type, bool, Maybe<object>> converter2;
				if ((converter2 = <>9__13) == null)
				{
					converter2 = (<>9__13 = ((IEnumerable<string> vals, Type type, bool isScalar) => TypeConverter.ChangeType(vals, type, isScalar, parsingCulture, ignoreValueCase)));
				}
				Result<IEnumerable<SpecificationProperty>, Error> result3 = ValueMapper.MapValues(specProps, values, converter2);
				Func<Token, MissingValueOptionError> selector;
				if ((selector = <>9__14) == null)
				{
					selector = (<>9__14 = ((Token token) => new MissingValueOptionError(optionSpecs.Single((OptionSpecification o) => token.Text.MatchName(o.ShortName, o.LongName, nameComparer)).FromOptionSpecification())));
				}
				IEnumerable<MissingValueOptionError> second = source.Select(selector);
				IEnumerable<SpecificationProperty> enumerable5 = result2.SucceededWith<IEnumerable<SpecificationProperty>, Error>().Concat(result3.SucceededWith<IEnumerable<SpecificationProperty>, Error>()).Memorize<SpecificationProperty>();
				List<Error> list = new List<Error>();
				T instance;
				if (typeInfo.IsMutable())
				{
					instance = InstanceBuilder.BuildMutable<T>(factory, enumerable5, list);
				}
				else
				{
					instance = InstanceBuilder.BuildImmutable<T>(typeInfo, factory, specProps, enumerable5, list);
				}
				IEnumerable<Error> second2 = enumerable5.Validate(SpecificationPropertyRules.Lookup(enumerable2));
				IEnumerable<Error> enumerable6 = result.SuccessfulMessages<IEnumerable<Token>, Error>().Concat(second).Concat(result2.SuccessfulMessages<IEnumerable<SpecificationProperty>, Error>()).Concat(result3.SuccessfulMessages<IEnumerable<SpecificationProperty>, Error>()).Concat(second2).Concat(list).Memorize<Error>();
				Func<Error, bool> predicate;
				if ((predicate = <>9__15) == null)
				{
					predicate = (<>9__15 = ((Error e) => nonFatalErrors.Contains(e.Tag)));
				}
				IEnumerable<Error> second3 = enumerable6.Where(predicate);
				return enumerable6.Except(second3).ToParserResult(instance);
			};
			IEnumerable<Error> enumerable = (argumentsList.Any<string>() ? arguments.Preprocess(PreprocessorGuards.Lookup(nameComparer, autoHelp, autoVersion)) : Enumerable.Empty<Error>()).Memorize<Error>();
			if (!argumentsList.Any<string>())
			{
				return func2();
			}
			if (!enumerable.Any<Error>())
			{
				return func2();
			}
			return func(enumerable);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000A9A8 File Offset: 0x00008BA8
		private static T BuildMutable<T>(Maybe<Func<T>> factory, IEnumerable<SpecificationProperty> specPropsWithValue, List<Error> setPropertyErrors)
		{
			T t = factory.MapValueOrDefault((Func<T> f) => f(), Activator.CreateInstance<T>());
			setPropertyErrors.AddRange(t.SetProperties(specPropsWithValue, (SpecificationProperty sp) => sp.Value.IsJust<object>(), (SpecificationProperty sp) => sp.Value.FromJustOrFail(null)));
			setPropertyErrors.AddRange(t.SetProperties(specPropsWithValue, (SpecificationProperty sp) => sp.Value.IsNothing<object>() && sp.Specification.DefaultValue.IsJust<object>(), (SpecificationProperty sp) => sp.Specification.DefaultValue.FromJustOrFail(null)));
			setPropertyErrors.AddRange(t.SetProperties(specPropsWithValue, (SpecificationProperty sp) => sp.Value.IsNothing<object>() && sp.Specification.TargetType == TargetType.Sequence && sp.Specification.DefaultValue.MatchNothing(), (SpecificationProperty sp) => sp.Property.PropertyType.GetTypeInfo().GetGenericArguments().Single<Type>().CreateEmptyArray()));
			return t;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		private static T BuildImmutable<T>(Type typeInfo, Maybe<Func<T>> factory, IEnumerable<SpecificationProperty> specProps, IEnumerable<SpecificationProperty> specPropsWithValue, List<Error> setPropertyErrors)
		{
			ConstructorInfo constructor = typeInfo.GetTypeInfo().GetConstructor((from sp in specProps
			select sp.Property.PropertyType).ToArray<Type>());
			if (constructor == null)
			{
				throw new InvalidOperationException("Type appears to be immutable, but no constructor found for type " + typeInfo.FullName + " to accept values.");
			}
			object[] parameters = (from prms in constructor.GetParameters()
			join sp in specPropsWithValue on prms.Name.ToLower() equals sp.Property.Name.ToLower() into spv
			select new
			{
				prms,
				spv
			}).SelectMany(<>h__TransparentIdentifier0 => <>h__TransparentIdentifier0.spv.DefaultIfEmpty<SpecificationProperty>(), delegate(<>h__TransparentIdentifier0, SpecificationProperty sp)
			{
				if (sp != null)
				{
					return sp.Value.GetValueOrDefault(sp.Specification.DefaultValue.GetValueOrDefault(sp.Specification.ConversionType.CreateDefaultForImmutable()));
				}
				return specProps.First((SpecificationProperty s) => string.Equals(s.Property.Name, <>h__TransparentIdentifier0.prms.Name, StringComparison.CurrentCultureIgnoreCase)).Property.PropertyType.GetDefaultValue();
			}).ToArray<object>();
			return (T)((object)constructor.Invoke(parameters));
		}
	}
}
