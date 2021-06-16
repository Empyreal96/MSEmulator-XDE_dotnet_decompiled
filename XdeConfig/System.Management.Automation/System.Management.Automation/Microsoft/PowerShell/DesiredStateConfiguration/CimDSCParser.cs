using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Serialization;

namespace Microsoft.PowerShell.DesiredStateConfiguration
{
	// Token: 0x020009FC RID: 2556
	internal class CimDSCParser
	{
		// Token: 0x06005D9C RID: 23964 RVA: 0x001FEFD8 File Offset: 0x001FD1D8
		internal CimDSCParser(CimMofDeserializer.OnClassNeeded onClassNeeded)
		{
			this.deserializer = CimMofDeserializer.Create();
			this.onClassNeeded = onClassNeeded;
		}

		// Token: 0x06005D9D RID: 23965 RVA: 0x001FEFF2 File Offset: 0x001FD1F2
		internal CimDSCParser(CimMofDeserializer.OnClassNeeded onClassNeeded, MofDeserializerSchemaValidationOption validationOptions)
		{
			this.deserializer = CimMofDeserializer.Create();
			this.deserializer.SchemaValidationOption = validationOptions;
			this.onClassNeeded = onClassNeeded;
		}

		// Token: 0x06005D9E RID: 23966 RVA: 0x001FF018 File Offset: 0x001FD218
		internal List<CimInstance> ParseInstanceMof(string filePath)
		{
			uint num = 0U;
			byte[] fileContent = CimDSCParser.GetFileContent(filePath);
			List<CimInstance> result;
			try
			{
				List<CimInstance> list = new List<CimInstance>(this.deserializer.DeserializeInstances(fileContent, ref num, this.onClassNeeded, null));
				result = list;
			}
			catch (CimException innerException)
			{
				PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(innerException, ParserStrings.CimDeserializationError, new object[]
				{
					filePath
				});
				ex.SetErrorId("CimDeserializationError");
				throw ex;
			}
			return result;
		}

		// Token: 0x06005D9F RID: 23967 RVA: 0x001FF08C File Offset: 0x001FD28C
		internal static byte[] GetFileContent(string fullFilePath)
		{
			if (string.IsNullOrEmpty(fullFilePath))
			{
				throw PSTraceSource.NewArgumentNullException("fullFilePath");
			}
			if (!File.Exists(fullFilePath))
			{
				throw PSTraceSource.NewArgumentException("fullFilePath");
			}
			byte[] result;
			using (FileStream fileStream = File.OpenRead(fullFilePath))
			{
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
				result = array;
			}
			return result;
		}

		// Token: 0x06005DA0 RID: 23968 RVA: 0x001FF108 File Offset: 0x001FD308
		internal List<CimClass> ParseSchemaMofFileBuffer(string mof)
		{
			uint num = 0U;
			byte[] bytes = Encoding.Unicode.GetBytes(mof);
			return new List<CimClass>(this.deserializer.DeserializeClasses(bytes, ref num, null, null, null, this.onClassNeeded, null));
		}

		// Token: 0x06005DA1 RID: 23969 RVA: 0x001FF144 File Offset: 0x001FD344
		internal List<CimClass> ParseSchemaMof(string filePath)
		{
			uint num = 0U;
			byte[] fileContent = CimDSCParser.GetFileContent(filePath);
			List<CimClass> result;
			try
			{
				List<CimClass> list = new List<CimClass>(this.deserializer.DeserializeClasses(fileContent, ref num, null, null, null, this.onClassNeeded, null));
				foreach (CimClass cimClass in list)
				{
					string cimSuperClassName = cimClass.CimSuperClassName;
					string className = cimClass.CimSystemProperties.ClassName;
					if (cimSuperClassName != null && cimSuperClassName.Equals("OMI_BaseResource", StringComparison.OrdinalIgnoreCase))
					{
						string text = Path.GetFileNameWithoutExtension(filePath).Split(new char[]
						{
							'.'
						})[0];
						if (!className.Equals(text, StringComparison.OrdinalIgnoreCase))
						{
							PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.ClassNameNotSameAsDefiningFile, new object[]
							{
								className,
								text
							});
							throw ex;
						}
					}
				}
				result = list;
			}
			catch (CimException innerException)
			{
				PSInvalidOperationException ex2 = PSTraceSource.NewInvalidOperationException(innerException, ParserStrings.CimDeserializationError, new object[]
				{
					filePath
				});
				ex2.SetErrorId("CimDeserializationError");
				throw ex2;
			}
			return result;
		}

		// Token: 0x06005DA2 RID: 23970 RVA: 0x001FF274 File Offset: 0x001FD474
		internal void ValidateInstanceText(string classText)
		{
			uint num = 0U;
			byte[] bytes = Encoding.Unicode.GetBytes(classText);
			this.deserializer.DeserializeInstances(bytes, ref num, this.onClassNeeded, null);
		}

		// Token: 0x040031CD RID: 12749
		private CimMofDeserializer deserializer;

		// Token: 0x040031CE RID: 12750
		private CimMofDeserializer.OnClassNeeded onClassNeeded;
	}
}
