using System;
using System.Management.Automation.Language;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000229 RID: 553
	public sealed class CommandParameter
	{
		// Token: 0x06001A00 RID: 6656 RVA: 0x0009B2E0 File Offset: 0x000994E0
		public CommandParameter(string name) : this(name, null)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0009B2F8 File Offset: 0x000994F8
		public CommandParameter(string name, object value)
		{
			if (name != null)
			{
				if (name.Trim().Length == 0)
				{
					throw PSTraceSource.NewArgumentException("name");
				}
				this._name = name;
			}
			else
			{
				this._name = name;
			}
			this._value = value;
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x0009B332 File Offset: 0x00099532
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0009B33A File Offset: 0x0009953A
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0009B344 File Offset: 0x00099544
		internal static CommandParameter FromCommandParameterInternal(CommandParameterInternal internalParameter)
		{
			if (internalParameter == null)
			{
				throw PSTraceSource.NewArgumentNullException("internalParameter");
			}
			string text = null;
			if (internalParameter.ParameterNameSpecified)
			{
				text = internalParameter.ParameterText;
				if (internalParameter.SpaceAfterParameter)
				{
					text += " ";
				}
			}
			if (internalParameter.ParameterAndArgumentSpecified)
			{
				return new CommandParameter(text, internalParameter.ArgumentValue);
			}
			if (text != null)
			{
				return new CommandParameter(text);
			}
			return new CommandParameter(null, internalParameter.ArgumentValue);
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0009B3B0 File Offset: 0x000995B0
		internal static CommandParameterInternal ToCommandParameterInternal(CommandParameter publicParameter, bool forNativeCommand)
		{
			if (publicParameter == null)
			{
				throw PSTraceSource.NewArgumentNullException("publicParameter");
			}
			string name = publicParameter.Name;
			object value = publicParameter.Value;
			if (name == null)
			{
				return CommandParameterInternal.CreateArgument(PositionUtilities.EmptyExtent, value, false, false);
			}
			string text;
			if (!name[0].IsDash())
			{
				text = (forNativeCommand ? name : ("-" + name));
				return CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, name, text, PositionUtilities.EmptyExtent, value, true, false);
			}
			bool spaceAfterParameter = false;
			int num = name.Length;
			while (num > 0 && char.IsWhiteSpace(name[num - 1]))
			{
				spaceAfterParameter = true;
				num--;
			}
			text = name.Substring(0, num);
			bool flag = name[num - 1] == ':';
			string parameterName = text.Substring(1, text.Length - (flag ? 2 : 1));
			if (!flag && value == null)
			{
				return CommandParameterInternal.CreateParameter(PositionUtilities.EmptyExtent, parameterName, text);
			}
			return CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, parameterName, text, PositionUtilities.EmptyExtent, value, spaceAfterParameter, false);
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0009B4A8 File Offset: 0x000996A8
		internal static CommandParameter FromPSObjectForRemoting(PSObject parameterAsPSObject)
		{
			if (parameterAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterAsPSObject");
			}
			string propertyValue = RemotingDecoder.GetPropertyValue<string>(parameterAsPSObject, "N");
			object propertyValue2 = RemotingDecoder.GetPropertyValue<object>(parameterAsPSObject, "V");
			return new CommandParameter(propertyValue, propertyValue2);
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0009B4E4 File Offset: 0x000996E4
		internal PSObject ToPSObjectForRemoting()
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("N", this.Name));
			psobject.Properties.Add(new PSNoteProperty("V", this.Value));
			return psobject;
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0009B530 File Offset: 0x00099730
		internal CimInstance ToCimInstance()
		{
			CimInstance cimInstance = InternalMISerializer.CreateCimInstance("PS_Parameter");
			CimProperty newItem = InternalMISerializer.CreateCimProperty("Name", this.Name, CimType.String);
			cimInstance.CimInstanceProperties.Add(newItem);
			CimType cimType = CimConverter.GetCimType(this.Value.GetType());
			CimProperty newItem2;
			if (cimType == CimType.Unknown)
			{
				newItem2 = InternalMISerializer.CreateCimProperty("Value", PSMISerializer.Serialize(this.Value), CimType.Instance);
			}
			else
			{
				newItem2 = InternalMISerializer.CreateCimProperty("Value", this.Value, cimType);
			}
			cimInstance.CimInstanceProperties.Add(newItem2);
			return cimInstance;
		}

		// Token: 0x04000AB4 RID: 2740
		private string _name;

		// Token: 0x04000AB5 RID: 2741
		private object _value;
	}
}
