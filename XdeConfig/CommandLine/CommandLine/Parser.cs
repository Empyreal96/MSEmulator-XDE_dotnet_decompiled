using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine.Core;
using CommandLine.Text;
using CSharpx;
using RailwaySharp.ErrorHandling;

namespace CommandLine
{
	// Token: 0x02000045 RID: 69
	public class Parser : IDisposable
	{
		// Token: 0x06000159 RID: 345 RVA: 0x00005721 File Offset: 0x00003921
		public Parser()
		{
			this.settings = new ParserSettings
			{
				Consumed = true
			};
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000573B File Offset: 0x0000393B
		public Parser(Action<ParserSettings> configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			this.settings = new ParserSettings();
			configuration(this.settings);
			this.settings.Consumed = true;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005774 File Offset: 0x00003974
		internal Parser(ParserSettings settings)
		{
			this.settings = settings;
			this.settings.Consumed = true;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005790 File Offset: 0x00003990
		~Parser()
		{
			this.Dispose(false);
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000057C0 File Offset: 0x000039C0
		public static Parser Default
		{
			get
			{
				return Parser.DefaultParser.Value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000057CC File Offset: 0x000039CC
		public ParserSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000057D4 File Offset: 0x000039D4
		public ParserResult<T> ParseArguments<T>(IEnumerable<string> args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			return Parser.MakeParserResult<T>(InstanceBuilder.Build<T>(typeof(T).IsMutable() ? Maybe.Just<Func<T>>(new Func<T>(Activator.CreateInstance<T>)) : Maybe.Nothing<Func<T>>(), (IEnumerable<string> arguments, IEnumerable<OptionSpecification> optionSpecs) => Parser.Tokenize(arguments, optionSpecs, this.settings), args, this.settings.NameComparer, this.settings.CaseInsensitiveEnumValues, this.settings.ParsingCulture, this.settings.AutoHelp, this.settings.AutoVersion, Parser.HandleUnknownArguments(this.settings.IgnoreUnknownArguments)), this.settings);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000587C File Offset: 0x00003A7C
		public ParserResult<T> ParseArguments<T>(Func<T> factory, IEnumerable<string> args) where T : new()
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			if (!typeof(T).IsMutable())
			{
				throw new ArgumentException("factory");
			}
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			return Parser.MakeParserResult<T>(InstanceBuilder.Build<T>(Maybe.Just<Func<T>>(factory), (IEnumerable<string> arguments, IEnumerable<OptionSpecification> optionSpecs) => Parser.Tokenize(arguments, optionSpecs, this.settings), args, this.settings.NameComparer, this.settings.CaseInsensitiveEnumValues, this.settings.ParsingCulture, this.settings.AutoHelp, this.settings.AutoVersion, Parser.HandleUnknownArguments(this.settings.IgnoreUnknownArguments)), this.settings);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000592C File Offset: 0x00003B2C
		public ParserResult<object> ParseArguments(IEnumerable<string> args, params Type[] types)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			if (types.Length == 0)
			{
				throw new ArgumentOutOfRangeException("types");
			}
			return Parser.MakeParserResult<object>(InstanceChooser.Choose((IEnumerable<string> arguments, IEnumerable<OptionSpecification> optionSpecs) => Parser.Tokenize(arguments, optionSpecs, this.settings), types, args, this.settings.NameComparer, this.settings.CaseInsensitiveEnumValues, this.settings.ParsingCulture, this.settings.AutoHelp, this.settings.AutoVersion, Parser.HandleUnknownArguments(this.settings.IgnoreUnknownArguments)), this.settings);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000059C9 File Offset: 0x00003BC9
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000059D8 File Offset: 0x00003BD8
		private static Result<IEnumerable<Token>, Error> Tokenize(IEnumerable<string> arguments, IEnumerable<OptionSpecification> optionSpecs, ParserSettings settings)
		{
			return Tokenizer.ConfigureTokenizer(settings.NameComparer, settings.IgnoreUnknownArguments, settings.EnableDashDash)(arguments, optionSpecs);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000059F8 File Offset: 0x00003BF8
		private static ParserResult<T> MakeParserResult<T>(ParserResult<T> parserResult, ParserSettings settings)
		{
			return Parser.DisplayHelp<T>(parserResult, settings.HelpWriter, settings.MaximumDisplayWidth);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005A0C File Offset: 0x00003C0C
		private static ParserResult<T> DisplayHelp<T>(ParserResult<T> parserResult, TextWriter helpWriter, int maxDisplayWidth)
		{
			Action<IEnumerable<Error>, TextWriter> <>9__1;
			parserResult.WithNotParsed(delegate(IEnumerable<Error> errors)
			{
				Maybe<Tuple<IEnumerable<Error>, TextWriter>> maybe = Maybe.Merge<IEnumerable<Error>, TextWriter>(errors.ToMaybe<Error>(), helpWriter.ToMaybe<TextWriter>());
				Action<IEnumerable<Error>, TextWriter> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(IEnumerable<Error> _, TextWriter writer)
					{
						writer.Write(HelpText.AutoBuild<T>(parserResult, maxDisplayWidth));
					});
				}
				maybe.Do(action);
			});
			return parserResult;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005A52 File Offset: 0x00003C52
		private static IEnumerable<ErrorType> HandleUnknownArguments(bool ignoreUnknownArguments)
		{
			if (!ignoreUnknownArguments)
			{
				return Enumerable.Empty<ErrorType>();
			}
			return Enumerable.Empty<ErrorType>().Concat(ErrorType.UnknownOptionError);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005A68 File Offset: 0x00003C68
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing)
			{
				if (this.settings != null)
				{
					this.settings.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x0400006D RID: 109
		private bool disposed;

		// Token: 0x0400006E RID: 110
		private readonly ParserSettings settings;

		// Token: 0x0400006F RID: 111
		private static readonly Lazy<Parser> DefaultParser = new Lazy<Parser>(() => new Parser(new ParserSettings
		{
			HelpWriter = Console.Error
		}));
	}
}
