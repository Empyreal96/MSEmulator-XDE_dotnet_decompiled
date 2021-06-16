using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine.Infrastructure;

namespace CommandLine.Text
{
	// Token: 0x0200005B RID: 91
	public abstract class SentenceBuilder
	{
		// Token: 0x06000256 RID: 598 RVA: 0x00009EBA File Offset: 0x000080BA
		public static SentenceBuilder Create()
		{
			return SentenceBuilder.Factory();
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00009EC6 File Offset: 0x000080C6
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00009ECD File Offset: 0x000080CD
		public static Func<SentenceBuilder> Factory { get; set; } = () => new SentenceBuilder.DefaultSentenceBuilder();

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000259 RID: 601
		public abstract Func<string> RequiredWord { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600025A RID: 602
		public abstract Func<string> ErrorsHeadingText { get; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600025B RID: 603
		public abstract Func<string> UsageHeadingText { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600025C RID: 604
		public abstract Func<bool, string> HelpCommandText { get; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600025D RID: 605
		public abstract Func<bool, string> VersionCommandText { get; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600025E RID: 606
		public abstract Func<Error, string> FormatError { get; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600025F RID: 607
		public abstract Func<IEnumerable<MutuallyExclusiveSetError>, string> FormatMutuallyExclusiveSetErrors { get; }

		// Token: 0x020000BE RID: 190
		private class DefaultSentenceBuilder : SentenceBuilder
		{
			// Token: 0x170000BF RID: 191
			// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000E764 File Offset: 0x0000C964
			public override Func<string> RequiredWord
			{
				get
				{
					return () => "Required.";
				}
			}

			// Token: 0x170000C0 RID: 192
			// (get) Token: 0x06000412 RID: 1042 RVA: 0x0000E785 File Offset: 0x0000C985
			public override Func<string> ErrorsHeadingText
			{
				get
				{
					return () => "ERROR(S):";
				}
			}

			// Token: 0x170000C1 RID: 193
			// (get) Token: 0x06000413 RID: 1043 RVA: 0x0000E7A6 File Offset: 0x0000C9A6
			public override Func<string> UsageHeadingText
			{
				get
				{
					return () => "USAGE:";
				}
			}

			// Token: 0x170000C2 RID: 194
			// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000E7C7 File Offset: 0x0000C9C7
			public override Func<bool, string> HelpCommandText
			{
				get
				{
					return delegate(bool isOption)
					{
						if (!isOption)
						{
							return "Display more information on a specific command.";
						}
						return "Display this help screen.";
					};
				}
			}

			// Token: 0x170000C3 RID: 195
			// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000E7E8 File Offset: 0x0000C9E8
			public override Func<bool, string> VersionCommandText
			{
				get
				{
					return (bool _) => "Display version information.";
				}
			}

			// Token: 0x170000C4 RID: 196
			// (get) Token: 0x06000416 RID: 1046 RVA: 0x0000E809 File Offset: 0x0000CA09
			public override Func<Error, string> FormatError
			{
				get
				{
					return delegate(Error error)
					{
						switch (error.Tag)
						{
						case ErrorType.BadFormatTokenError:
							return "Token '".JoinTo(new string[]
							{
								((BadFormatTokenError)error).Token,
								"' is not recognized."
							});
						case ErrorType.MissingValueOptionError:
							return "Option '".JoinTo(new string[]
							{
								((MissingValueOptionError)error).NameInfo.NameText,
								"' has no value."
							});
						case ErrorType.UnknownOptionError:
							return "Option '".JoinTo(new string[]
							{
								((UnknownOptionError)error).Token,
								"' is unknown."
							});
						case ErrorType.MissingRequiredOptionError:
						{
							MissingRequiredOptionError missingRequiredOptionError = (MissingRequiredOptionError)error;
							if (!missingRequiredOptionError.NameInfo.Equals(NameInfo.EmptyName))
							{
								return "Required option '".JoinTo(new string[]
								{
									missingRequiredOptionError.NameInfo.NameText,
									"' is missing."
								});
							}
							return "A required value not bound to option name is missing.";
						}
						case ErrorType.BadFormatConversionError:
						{
							BadFormatConversionError badFormatConversionError = (BadFormatConversionError)error;
							if (!badFormatConversionError.NameInfo.Equals(NameInfo.EmptyName))
							{
								return "Option '".JoinTo(new string[]
								{
									badFormatConversionError.NameInfo.NameText,
									"' is defined with a bad format."
								});
							}
							return "A value not bound to option name is defined with a bad format.";
						}
						case ErrorType.SequenceOutOfRangeError:
						{
							SequenceOutOfRangeError sequenceOutOfRangeError = (SequenceOutOfRangeError)error;
							if (!sequenceOutOfRangeError.NameInfo.Equals(NameInfo.EmptyName))
							{
								return "A sequence option '".JoinTo(new string[]
								{
									sequenceOutOfRangeError.NameInfo.NameText,
									"' is defined with fewer or more items than required."
								});
							}
							return "A sequence value not bound to option name is defined with few items than required.";
						}
						case ErrorType.RepeatedOptionError:
							return "Option '".JoinTo(new string[]
							{
								((RepeatedOptionError)error).NameInfo.NameText,
								"' is defined multiple times."
							});
						case ErrorType.NoVerbSelectedError:
							return "No verb selected.";
						case ErrorType.BadVerbSelectedError:
							return "Verb '".JoinTo(new string[]
							{
								((BadVerbSelectedError)error).Token,
								"' is not recognized."
							});
						case ErrorType.SetValueExceptionError:
						{
							SetValueExceptionError setValueExceptionError = (SetValueExceptionError)error;
							return "Error setting value to option '".JoinTo(new string[]
							{
								setValueExceptionError.NameInfo.NameText,
								"': ",
								setValueExceptionError.Exception.Message
							});
						}
						}
						throw new InvalidOperationException();
					};
				}
			}

			// Token: 0x170000C5 RID: 197
			// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000E82A File Offset: 0x0000CA2A
			public override Func<IEnumerable<MutuallyExclusiveSetError>, string> FormatMutuallyExclusiveSetErrors
			{
				get
				{
					return delegate(IEnumerable<MutuallyExclusiveSetError> errors)
					{
						IEnumerable<<>f__AnonymousType18<string, List<MutuallyExclusiveSetError>>> bySet = from e in errors
						group e by e.SetName into g
						select new
						{
							SetName = g.Key,
							Errors = g.ToList<MutuallyExclusiveSetError>()
						};
						string[] value2 = bySet.Select(delegate(set)
						{
							string text = string.Join(string.Empty, (from e in set.Errors
							select "'".JoinTo(new string[]
							{
								e.NameInfo.NameText,
								"', "
							})).ToArray<string>());
							int num = set.Errors.Count<MutuallyExclusiveSetError>();
							string text2 = string.Join(string.Empty, (from x in (from s in bySet
							where !s.SetName.Equals(set.SetName)
							from e in s.Errors
							select e).Distinct<MutuallyExclusiveSetError>()
							select "'".JoinTo(new string[]
							{
								x.NameInfo.NameText,
								"', "
							})).ToArray<string>());
							return new StringBuilder("Option").AppendWhen(num > 1, new string[]
							{
								"s"
							}).Append(": ").Append(text.Substring(0, text.Length - 2)).Append(' ').AppendIf(num > 1, "are", "is").Append(" not compatible with: ").Append(text2.Substring(0, text2.Length - 2)).Append('.').ToString();
						}).ToArray<string>();
						return string.Join(Environment.NewLine, value2);
					};
				}
			}
		}
	}
}
