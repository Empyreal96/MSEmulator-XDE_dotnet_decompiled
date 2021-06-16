using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLine.Core
{
	// Token: 0x0200008A RID: 138
	internal sealed class Verb
	{
		// Token: 0x0600032C RID: 812 RVA: 0x0000CAAB File Offset: 0x0000ACAB
		public Verb(string name, string helpText, bool hidden = false)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.name = name;
			if (helpText == null)
			{
				throw new ArgumentNullException("helpText");
			}
			this.helpText = helpText;
			this.hidden = hidden;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000CAE6 File Offset: 0x0000ACE6
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000CAEE File Offset: 0x0000ACEE
		public string HelpText
		{
			get
			{
				return this.helpText;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000CAF6 File Offset: 0x0000ACF6
		public bool Hidden
		{
			get
			{
				return this.hidden;
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000CAFE File Offset: 0x0000ACFE
		public static Verb FromAttribute(VerbAttribute attribute)
		{
			return new Verb(attribute.Name, attribute.HelpText, attribute.Hidden);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000CB18 File Offset: 0x0000AD18
		public static IEnumerable<Tuple<Verb, Type>> SelectFromTypes(IEnumerable<Type> types)
		{
			return from type in types
			let attrs = type.GetTypeInfo().GetCustomAttributes(typeof(VerbAttribute), true).ToArray<object>()
			where attrs.Length == 1
			select Tuple.Create<Verb, Type>(Verb.FromAttribute((VerbAttribute)attrs.Single<object>()), type);
		}

		// Token: 0x040000ED RID: 237
		private readonly string name;

		// Token: 0x040000EE RID: 238
		private readonly string helpText;

		// Token: 0x040000EF RID: 239
		private readonly bool hidden;
	}
}
