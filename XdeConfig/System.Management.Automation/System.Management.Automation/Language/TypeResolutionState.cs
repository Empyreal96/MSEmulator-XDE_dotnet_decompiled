using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x020005E5 RID: 1509
	internal class TypeResolutionState
	{
		// Token: 0x060040B8 RID: 16568 RVA: 0x00157516 File Offset: 0x00155716
		private TypeResolutionState() : this(TypeResolutionState.systemNamespace, TypeResolutionState.emptyAssemblies)
		{
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x00157528 File Offset: 0x00155728
		internal TypeResolutionState(string[] namespaces, Assembly[] assemblies)
		{
			this.namespaces = (namespaces ?? TypeResolutionState.systemNamespace);
			this.assemblies = (assemblies ?? TypeResolutionState.emptyAssemblies);
			this.typesDefined = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x00157560 File Offset: 0x00155760
		internal TypeResolutionState(TypeResolutionState other, int genericArgumentCount, bool attribute)
		{
			this.namespaces = other.namespaces;
			this.assemblies = other.assemblies;
			this.typesDefined = other.typesDefined;
			this.genericArgumentCount = genericArgumentCount;
			this.attribute = attribute;
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x0015759C File Offset: 0x0015579C
		internal static TypeResolutionState GetDefaultUsingState(ExecutionContext context)
		{
			if (context == null)
			{
				context = LocalPipeline.GetExecutionContextFromTLS();
			}
			TypeResolutionState typeResolutionState = null;
			if (context != null)
			{
				typeResolutionState = context.EngineSessionState.CurrentScope.ScriptScope.TypeResolutionState;
			}
			return typeResolutionState ?? TypeResolutionState.UsingSystem;
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x001575D8 File Offset: 0x001557D8
		internal string GetAlternateTypeName(string typeName)
		{
			string result = null;
			if (this.genericArgumentCount > 0 && typeName.IndexOf('`') < 0)
			{
				result = typeName + "`" + this.genericArgumentCount;
			}
			else if (this.attribute && !typeName.EndsWith("Attribute", StringComparison.OrdinalIgnoreCase))
			{
				result = typeName + "Attribute";
			}
			return result;
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x00157638 File Offset: 0x00155838
		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			TypeResolutionState typeResolutionState = obj as TypeResolutionState;
			if (typeResolutionState == null)
			{
				return false;
			}
			if (this.attribute != typeResolutionState.attribute)
			{
				return false;
			}
			if (this.genericArgumentCount != typeResolutionState.genericArgumentCount)
			{
				return false;
			}
			if (this.namespaces.Length != typeResolutionState.namespaces.Length)
			{
				return false;
			}
			if (this.assemblies.Length != typeResolutionState.assemblies.Length)
			{
				return false;
			}
			for (int i = 0; i < this.namespaces.Length; i++)
			{
				if (!this.namespaces[i].Equals(typeResolutionState.namespaces[i], StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
			}
			for (int j = 0; j < this.assemblies.Length; j++)
			{
				if (!this.assemblies[j].Equals(typeResolutionState.assemblies[j]))
				{
					return false;
				}
			}
			return this.typesDefined.Count == typeResolutionState.typesDefined.Count && this.typesDefined.SetEquals(typeResolutionState.typesDefined);
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x00157728 File Offset: 0x00155928
		public override int GetHashCode()
		{
			StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
			int num = Utils.CombineHashCodes(this.genericArgumentCount.GetHashCode(), this.attribute.GetHashCode());
			for (int i = 0; i < this.namespaces.Length; i++)
			{
				num = Utils.CombineHashCodes(num, ordinalIgnoreCase.GetHashCode(this.namespaces[i]));
			}
			for (int j = 0; j < this.assemblies.Length; j++)
			{
				num = Utils.CombineHashCodes(num, this.assemblies[j].GetHashCode());
			}
			foreach (string text in this.typesDefined)
			{
				num = Utils.CombineHashCodes(num, text.GetHashCode());
			}
			return num;
		}

		// Token: 0x04002081 RID: 8321
		internal static readonly string[] systemNamespace = new string[]
		{
			"System"
		};

		// Token: 0x04002082 RID: 8322
		internal static readonly Assembly[] emptyAssemblies = new Assembly[0];

		// Token: 0x04002083 RID: 8323
		internal static readonly TypeResolutionState UsingSystem = new TypeResolutionState();

		// Token: 0x04002084 RID: 8324
		internal readonly string[] namespaces;

		// Token: 0x04002085 RID: 8325
		internal readonly Assembly[] assemblies;

		// Token: 0x04002086 RID: 8326
		internal readonly HashSet<string> typesDefined;

		// Token: 0x04002087 RID: 8327
		internal readonly int genericArgumentCount;

		// Token: 0x04002088 RID: 8328
		internal readonly bool attribute;
	}
}
