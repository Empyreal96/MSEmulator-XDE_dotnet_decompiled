using System;
using System.ComponentModel;

namespace System.Management.Automation
{
	// Token: 0x02000190 RID: 400
	public class PSObjectTypeDescriptor : CustomTypeDescriptor
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06001366 RID: 4966 RVA: 0x000787E8 File Offset: 0x000769E8
		// (remove) Token: 0x06001367 RID: 4967 RVA: 0x00078820 File Offset: 0x00076A20
		public event EventHandler<SettingValueExceptionEventArgs> SettingValueException;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06001368 RID: 4968 RVA: 0x00078858 File Offset: 0x00076A58
		// (remove) Token: 0x06001369 RID: 4969 RVA: 0x00078890 File Offset: 0x00076A90
		public event EventHandler<GettingValueExceptionEventArgs> GettingValueException;

		// Token: 0x0600136A RID: 4970 RVA: 0x000788C5 File Offset: 0x00076AC5
		public PSObjectTypeDescriptor(PSObject instance)
		{
			this._instance = instance;
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x000788D4 File Offset: 0x00076AD4
		public PSObject Instance
		{
			get
			{
				return this._instance;
			}
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x000788DC File Offset: 0x00076ADC
		private void CheckAndAddProperty(PSPropertyInfo propertyInfo, Attribute[] attributes, ref PropertyDescriptorCollection returnValue)
		{
			using (PSObjectTypeDescriptor.typeDescriptor.TraceScope("Checking property \"{0}\".", new object[]
			{
				propertyInfo.Name
			}))
			{
				if (!propertyInfo.IsGettable)
				{
					PSObjectTypeDescriptor.typeDescriptor.WriteLine("Property \"{0}\" is write-only so it has been skipped.", new object[]
					{
						propertyInfo.Name
					});
				}
				else
				{
					AttributeCollection attributeCollection = null;
					Type propertyType = typeof(object);
					if (attributes != null && attributes.Length != 0)
					{
						PSProperty psproperty = propertyInfo as PSProperty;
						if (psproperty != null)
						{
							DotNetAdapter.PropertyCacheEntry propertyCacheEntry = psproperty.adapterData as DotNetAdapter.PropertyCacheEntry;
							if (propertyCacheEntry == null)
							{
								PSObjectTypeDescriptor.typeDescriptor.WriteLine("Skipping attribute check for property \"{0}\" because it is an adapted property (not a .NET property).", new object[]
								{
									psproperty.Name
								});
							}
							else if (psproperty.isDeserialized)
							{
								PSObjectTypeDescriptor.typeDescriptor.WriteLine("Skipping attribute check for property \"{0}\" because it has been deserialized.", new object[]
								{
									psproperty.Name
								});
							}
							else
							{
								propertyType = propertyCacheEntry.propertyType;
								attributeCollection = propertyCacheEntry.Attributes;
								foreach (Attribute attribute in attributes)
								{
									if (!attributeCollection.Contains(attribute))
									{
										PSObjectTypeDescriptor.typeDescriptor.WriteLine("Property \"{0}\" does not contain attribute \"{1}\" so it has been skipped.", new object[]
										{
											psproperty.Name,
											attribute
										});
										return;
									}
								}
							}
						}
					}
					if (attributeCollection == null)
					{
						attributeCollection = new AttributeCollection(new Attribute[0]);
					}
					PSObjectTypeDescriptor.typeDescriptor.WriteLine("Adding property \"{0}\".", new object[]
					{
						propertyInfo.Name
					});
					PSObjectPropertyDescriptor psobjectPropertyDescriptor = new PSObjectPropertyDescriptor(propertyInfo.Name, propertyType, !propertyInfo.IsSettable, attributeCollection);
					psobjectPropertyDescriptor.SettingValueException += this.SettingValueException;
					psobjectPropertyDescriptor.GettingValueException += this.GettingValueException;
					returnValue.Add(psobjectPropertyDescriptor);
				}
			}
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00078AC8 File Offset: 0x00076CC8
		public override PropertyDescriptorCollection GetProperties()
		{
			return this.GetProperties(null);
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00078AD4 File Offset: 0x00076CD4
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection result;
			using (PSObjectTypeDescriptor.typeDescriptor.TraceScope("Getting properties.", new object[0]))
			{
				PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
				if (this._instance == null)
				{
					result = propertyDescriptorCollection;
				}
				else
				{
					foreach (PSPropertyInfo propertyInfo in this._instance.Properties)
					{
						this.CheckAndAddProperty(propertyInfo, attributes, ref propertyDescriptorCollection);
					}
					result = propertyDescriptorCollection;
				}
			}
			return result;
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00078B74 File Offset: 0x00076D74
		public override bool Equals(object obj)
		{
			PSObjectTypeDescriptor psobjectTypeDescriptor = obj as PSObjectTypeDescriptor;
			if (psobjectTypeDescriptor == null)
			{
				return false;
			}
			if (this.Instance == null || psobjectTypeDescriptor.Instance == null)
			{
				return object.ReferenceEquals(this, psobjectTypeDescriptor);
			}
			return psobjectTypeDescriptor.Instance.Equals(this.Instance);
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00078BB6 File Offset: 0x00076DB6
		public override int GetHashCode()
		{
			if (this.Instance == null)
			{
				return base.GetHashCode();
			}
			return this.Instance.GetHashCode();
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00078BD4 File Offset: 0x00076DD4
		public override PropertyDescriptor GetDefaultProperty()
		{
			if (this.Instance == null)
			{
				return null;
			}
			string text = null;
			PSMemberSet psstandardMembers = this.Instance.PSStandardMembers;
			if (psstandardMembers != null)
			{
				PSNoteProperty psnoteProperty = psstandardMembers.Properties["DefaultDisplayProperty"] as PSNoteProperty;
				if (psnoteProperty != null)
				{
					text = (psnoteProperty.Value as string);
				}
			}
			if (text == null)
			{
				object[] customAttributes = this.Instance.BaseObject.GetType().GetCustomAttributes(typeof(DefaultPropertyAttribute), true);
				if (customAttributes.Length == 1)
				{
					DefaultPropertyAttribute defaultPropertyAttribute = customAttributes[0] as DefaultPropertyAttribute;
					if (defaultPropertyAttribute != null)
					{
						text = defaultPropertyAttribute.Name;
					}
				}
			}
			PropertyDescriptorCollection properties = this.GetProperties();
			if (text != null)
			{
				foreach (object obj in properties)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
					if (string.Equals(propertyDescriptor.Name, text, StringComparison.OrdinalIgnoreCase))
					{
						return propertyDescriptor;
					}
				}
			}
			return null;
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00078CD0 File Offset: 0x00076ED0
		public override TypeConverter GetConverter()
		{
			if (this.Instance == null)
			{
				return new TypeConverter();
			}
			object baseObject = this.Instance.BaseObject;
			TypeConverter typeConverter = LanguagePrimitives.GetConverter(baseObject.GetType(), null) as TypeConverter;
			if (typeConverter == null)
			{
				typeConverter = TypeDescriptor.GetConverter(baseObject);
			}
			return typeConverter;
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00078D14 File Offset: 0x00076F14
		public override object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this.Instance;
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00078D1C File Offset: 0x00076F1C
		public override EventDescriptor GetDefaultEvent()
		{
			if (this.Instance == null)
			{
				return null;
			}
			return TypeDescriptor.GetDefaultEvent(this.Instance.BaseObject);
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00078D38 File Offset: 0x00076F38
		public override EventDescriptorCollection GetEvents()
		{
			if (this.Instance == null)
			{
				return new EventDescriptorCollection(null);
			}
			return TypeDescriptor.GetEvents(this.Instance.BaseObject);
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00078D59 File Offset: 0x00076F59
		public override EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			if (this.Instance == null)
			{
				return null;
			}
			return TypeDescriptor.GetEvents(this.Instance.BaseObject, attributes);
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00078D76 File Offset: 0x00076F76
		public override AttributeCollection GetAttributes()
		{
			if (this.Instance == null)
			{
				return new AttributeCollection(new Attribute[0]);
			}
			return TypeDescriptor.GetAttributes(this.Instance.BaseObject);
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00078D9C File Offset: 0x00076F9C
		public override string GetClassName()
		{
			if (this.Instance == null)
			{
				return null;
			}
			return TypeDescriptor.GetClassName(this.Instance.BaseObject);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00078DB8 File Offset: 0x00076FB8
		public override string GetComponentName()
		{
			if (this.Instance == null)
			{
				return null;
			}
			return TypeDescriptor.GetComponentName(this.Instance.BaseObject);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00078DD4 File Offset: 0x00076FD4
		public override object GetEditor(Type editorBaseType)
		{
			if (this.Instance == null)
			{
				return null;
			}
			return TypeDescriptor.GetEditor(this.Instance.BaseObject, editorBaseType);
		}

		// Token: 0x0400084A RID: 2122
		internal static PSTraceSource typeDescriptor = PSTraceSource.GetTracer("TypeDescriptor", "Traces the behavior of PSObjectTypeDescriptor, PSObjectTypeDescriptionProvider and PSObjectPropertyDescriptor.", false);

		// Token: 0x0400084D RID: 2125
		private PSObject _instance;
	}
}
