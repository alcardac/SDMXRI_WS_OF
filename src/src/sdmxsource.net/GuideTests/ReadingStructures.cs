/*
 * Created by SharpDevelop.
 * User: sli
 * Date: 16/4/2013
 * Time: 1:18 μμ
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
using Org.Sdmxsource.Sdmx.Api.Factory;
using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
using Org.Sdmxsource.Sdmx.Api.Model;
using Org.Sdmxsource.Sdmx.Api.Model.Objects;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
using Org.Sdmxsource.Sdmx.Api.Util;
using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
using Org.Sdmxsource.Util.Io;

namespace GuideTests
{
	class ReadingStructures
	{
		private IStructureParsingManager spm;
		
		public ReadingStructures() {
			
		}
		
		public void readStructures(String structureFile, SdmxSchemaEnumType schema) {
			IReadableDataLocation rdl = new FileReadableDataLocation(structureFile);
			spm = new StructureParsingManager(SdmxSchemaEnumType.VersionTwoPointOne);
			
			IStructureWorkspace workspace = spm.ParseStructures(rdl);
			
			ISdmxObjects sdmxObjects = workspace.GetStructureObjects(false);
			
			ISet<IMaintainableObject> maintainables =  sdmxObjects.GetAllMaintainables();
			foreach (IMaintainableObject m in maintainables) {
				Console.WriteLine(m.Urn);
				Console.WriteLine(m.StructureType.StructureType + " - " + m.Name);
				Console.WriteLine(" --- ");
			}
		}
		
		public static void Main(string[] args)
		{
			//ReadingStructures rs = new ReadingStructures();
			
			//rs.readStructures("output/structures.xml", SdmxSchemaEnumType.VersionTwoPointOne);
			
			ResolveStructures resolv = new ResolveStructures();
			resolv.resolveStructures("output/codelists_concepts.xml", "output/dsd.xml", SdmxSchemaEnumType.VersionTwoPointOne);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}