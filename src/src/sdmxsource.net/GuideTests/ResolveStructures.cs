/*
 * Created by SharpDevelop.
 * User: sli
 * Date: 16/4/2013
 * Time: 4:32 μμ
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
using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
using Org.Sdmxsource.Sdmx.Api.Model;
using Org.Sdmxsource.Sdmx.Api.Model.Objects;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
using Org.Sdmxsource.Sdmx.Api.Util;
using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
using Org.Sdmxsource.Util.Io;
using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;

namespace GuideTests
{
	/// <summary>
	/// Description of ResolveStructures.
	/// </summary>
	public class ResolveStructures
	{
		private IStructureParsingManager spm;
		
		public ResolveStructures()
		{
		}
		
		public void resolveStructures(String codelistsConceptsFile, String dsdFile, SdmxSchemaEnumType schema) {
			
			spm = new StructureParsingManager(SdmxSchemaEnumType.VersionTwoPointOne);
			
			IReadableDataLocation codelistRdl = new FileReadableDataLocation(codelistsConceptsFile);
			ISdmxObjectRetrievalManager retManager = new InMemoryRetrievalManager(codelistRdl, spm, null);
			
			Console.WriteLine("In memory objects built!!!");
			
			IReadableDataLocation dsdRdl = new FileReadableDataLocation(dsdFile);
			ResolutionSettings settings = new ResolutionSettings(ResolveExternalSetting.Resolve, ResolveCrossReferences.ResolveExcludeAgencies);
								
			IStructureWorkspace workspace = spm.ParseStructures(dsdRdl, settings, retManager);
			ISdmxObjects sdmxObjects = workspace.GetStructureObjects(true);
			
			ISet<IMaintainableObject> maintainables =  sdmxObjects.GetAllMaintainables();
			foreach (IMaintainableObject m in maintainables) {
				Console.WriteLine(m.Urn);
				Console.WriteLine(m.StructureType.StructureType + " - " + m.Name);
				Console.WriteLine(" --- ");
			}
		}
		
	}
}
