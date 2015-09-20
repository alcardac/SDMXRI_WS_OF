<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
<tagfile>
  <compound kind="namespace">
    <name>Estat</name>
    <filename>a00113.html</filename>
    <namespace>Estat::Sdmxsource</namespace>
    <namespace>Estat::Sri</namespace>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sdmxsource</name>
    <filename>a00114.html</filename>
    <namespace>Estat::Sdmxsource::Extension</namespace>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sdmxsource::Extension</name>
    <filename>a00115.html</filename>
    <namespace>Estat::Sdmxsource::Extension::Builder</namespace>
    <namespace>Estat::Sdmxsource::Extension::Constant</namespace>
    <namespace>Estat::Sdmxsource::Extension::Extension</namespace>
    <namespace>Estat::Sdmxsource::Extension::Manager</namespace>
    <namespace>Estat::Sdmxsource::Extension::Util</namespace>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sdmxsource::Extension::Builder</name>
    <filename>a00116.html</filename>
    <class kind="class">Estat::Sdmxsource::Extension::Builder::ComplexDataQuery2DataQueryBuilder</class>
    <class kind="class">Estat::Sdmxsource::Extension::Builder::DataQuery2ComplexQueryBuilder</class>
    <class kind="class">Estat::Sdmxsource::Extension::Builder::StructureQuery2ComplexQueryBuilder</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sdmxsource::Extension::Builder::ComplexDataQuery2DataQueryBuilder</name>
    <filename>a00004.html</filename>
    <member kind="function">
      <type>IDataQuery</type>
      <name>Build</name>
      <anchorfile>a00004.html</anchorfile>
      <anchor>a903e49e160ba92a389ca96de4142bdf5</anchor>
      <arglist>(IComplexDataQuery buildFrom)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sdmxsource::Extension::Builder::DataQuery2ComplexQueryBuilder</name>
    <filename>a00017.html</filename>
  </compound>
  <compound kind="class">
    <name>Estat::Sdmxsource::Extension::Builder::StructureQuery2ComplexQueryBuilder</name>
    <filename>a00049.html</filename>
    <member kind="function" virtualness="virtual">
      <type>virtual IComplexStructureQuery</type>
      <name>Build</name>
      <anchorfile>a00049.html</anchorfile>
      <anchor>aa5a34f1d33f21a44f765af2bdf00fed6</anchor>
      <arglist>(IRestStructureQuery structureQuery)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sdmxsource::Extension::Constant</name>
    <filename>a00117.html</filename>
    <class kind="class">Estat::Sdmxsource::Extension::Constant::CustomAnnotationTypeExtensions</class>
    <member kind="enumeration">
      <type></type>
      <name>CustomAnnotationType</name>
      <anchorfile>a00117.html</anchorfile>
      <anchor>aeed9e0aa48c555181b2cfd88d7dd2ca1</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <type>@</type>
      <name>None</name>
      <anchorfile>a00117.html</anchorfile>
      <anchor>aeed9e0aa48c555181b2cfd88d7dd2ca1a6adf97f83acf6453d4a6a4b1070f3754</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <type>@</type>
      <name>CategorySchemeNodeOrder</name>
      <anchorfile>a00117.html</anchorfile>
      <anchor>aeed9e0aa48c555181b2cfd88d7dd2ca1afd9fde6d26d7ecffbedc7ddab39a7c71</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <type>@</type>
      <name>SDMXv20Only</name>
      <anchorfile>a00117.html</anchorfile>
      <anchor>aeed9e0aa48c555181b2cfd88d7dd2ca1a615f05707a0eeddf4eb1b4706bc27c01</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <type>@</type>
      <name>SDMXv21Only</name>
      <anchorfile>a00117.html</anchorfile>
      <anchor>aeed9e0aa48c555181b2cfd88d7dd2ca1a3dff645dd983f83622bd3c87debc05a4</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <type>@</type>
      <name>NonProductionDataflow</name>
      <anchorfile>a00117.html</anchorfile>
      <anchor>aeed9e0aa48c555181b2cfd88d7dd2ca1a420e3d77924dc6c5a9627ff5074c9dad</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sdmxsource::Extension::Constant::CustomAnnotationTypeExtensions</name>
    <filename>a00015.html</filename>
    <member kind="function" static="yes">
      <type>static CustomAnnotationType</type>
      <name>FromAnnotation</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>a7799c784417b5585536bc088ce57ffe1</anchor>
      <arglist>(this IAnnotationMutableObject annotation)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static CustomAnnotationType</type>
      <name>FromAnnotation</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>a8c326acc46c1a56bb3fb3ec65bbf6ec7</anchor>
      <arglist>(this IAnnotation annotation)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static T</type>
      <name>ToAnnotation&lt; T &gt;</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>a07743b8a7d88d0908a4760efa74e5390</anchor>
      <arglist>(this CustomAnnotationType customAnnotation)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static T</type>
      <name>ToAnnotation&lt; T &gt;</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>ab77d892b60b73171e2509942b47b2d10</anchor>
      <arglist>(this CustomAnnotationType customAnnotation, string value)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static string</type>
      <name>ToStringValue</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>ae659e754b2f8659558a6d95d59e27553</anchor>
      <arglist>(this CustomAnnotationType customAnnotation)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static string</type>
      <name>ValueFromAnnotation</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>a654a2b8900b5598ec2fd6a30053758f6</anchor>
      <arglist>(this IAnnotationMutableObject annotation)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static string</type>
      <name>ValueFromAnnotation</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>a7c72ab102b94b7701151ca1f112749d4</anchor>
      <arglist>(this IAnnotation annotation)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>SetNonProduction</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>a9c214dbd5df8ea6d5aea97aa2e7b0524</anchor>
      <arglist>(this IDataflowMutableObject dataflowMutableObject)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static bool</type>
      <name>IsNonProduction</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>acd0a8a086767ae857950ffdab5e0e166</anchor>
      <arglist>(this IDataflowMutableObject dataflow)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static bool</type>
      <name>IsNonProduction</name>
      <anchorfile>a00015.html</anchorfile>
      <anchor>aa5c0a6a59d2e5d65bf30b662b90c45bc</anchor>
      <arglist>(this IDataflowObject dataflow)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sdmxsource::Extension::Extension</name>
    <filename>a00118.html</filename>
    <class kind="class">Estat::Sdmxsource::Extension::Extension::QueryExtensions</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sdmxsource::Extension::Extension::QueryExtensions</name>
    <filename>a00038.html</filename>
    <member kind="function" static="yes">
      <type>static ComplexStructureQueryDetailEnumType</type>
      <name>GetComplexQueryDetail</name>
      <anchorfile>a00038.html</anchorfile>
      <anchor>a201589d9a611ce2af34d897f2954b68e</anchor>
      <arglist>(this bool returnStub)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static StructureQueryDetail</type>
      <name>GetStructureQueryDetail</name>
      <anchorfile>a00038.html</anchorfile>
      <anchor>afa17728a87fde7373210202d9978462b</anchor>
      <arglist>(this bool returnStub)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static bool</type>
      <name>GetReturnStub</name>
      <anchorfile>a00038.html</anchorfile>
      <anchor>a3d7318b7a1b4cd1eae8dad66ea2b784d</anchor>
      <arglist>(this StructureQueryDetail detail)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static IComplexStructureReferenceObject</type>
      <name>ToComplex</name>
      <anchorfile>a00038.html</anchorfile>
      <anchor>a87b2586b7737080d5c3b6c3d8293cc1f</anchor>
      <arglist>(this IStructureReference reference)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static IMaintainableRefObject</type>
      <name>GetMaintainableRefObject</name>
      <anchorfile>a00038.html</anchorfile>
      <anchor>ab5a26434f8a1e38e8fe9bf2f2d7c9545</anchor>
      <arglist>(this IComplexStructureReferenceObject complexStructureQuery)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static ComplexStructureQueryDetail</type>
      <name>ToComplex</name>
      <anchorfile>a00038.html</anchorfile>
      <anchor>aeb49e86f217db00ea00b1ed019869af5</anchor>
      <arglist>(this StructureQueryDetail detail)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static ComplexMaintainableQueryDetail</type>
      <name>ToComplexReference</name>
      <anchorfile>a00038.html</anchorfile>
      <anchor>aa07a921e7f691e98f5adfb9946ecab12</anchor>
      <arglist>(this StructureQueryDetail detail)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sdmxsource::Extension::Manager</name>
    <filename>a00119.html</filename>
    <namespace>Estat::Sdmxsource::Extension::Manager::Data</namespace>
    <class kind="interface">Estat::Sdmxsource::Extension::Manager::IAuthAdvancedMutableStructureSearchManager</class>
    <class kind="interface">Estat::Sdmxsource::Extension::Manager::IAuthAdvancedSdmxMutableObjectRetrievalManager</class>
    <class kind="interface">Estat::Sdmxsource::Extension::Manager::IAuthCrossReferenceMutableRetrievalManager</class>
    <class kind="interface">Estat::Sdmxsource::Extension::Manager::IAuthMutableStructureSearchManager</class>
    <class kind="interface">Estat::Sdmxsource::Extension::Manager::IAuthSdmxMutableObjectRetrievalManager</class>
    <class kind="interface">Estat::Sdmxsource::Extension::Manager::ISpecialMutableObjectRetrievalManager</class>
  </compound>
  <compound kind="interface">
    <name>Estat::Sdmxsource::Extension::Manager::IAuthAdvancedMutableStructureSearchManager</name>
    <filename>a00022.html</filename>
    <member kind="function">
      <type>IMutableObjects</type>
      <name>GetMaintainables</name>
      <anchorfile>a00022.html</anchorfile>
      <anchor>a7da12a717577c09c2aa7368b6bb6554b</anchor>
      <arglist>(IComplexStructureQuery structureQuery, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sdmxsource::Extension::Manager::IAuthAdvancedSdmxMutableObjectRetrievalManager</name>
    <filename>a00023.html</filename>
    <member kind="function">
      <type>IAgencySchemeMutableObject</type>
      <name>GetMutableAgencyScheme</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a98b7b6e7a2c1a16fa3df863a0d4d1c86</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IAgencySchemeMutableObject &gt;</type>
      <name>GetMutableAgencySchemeObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a6ffb0fe8906607aab0a1ad33ce3e54a3</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ICategorisationMutableObject</type>
      <name>GetMutableCategorisation</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a8435c8eded2d7518238fa2b55ff09aa4</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; ICategorisationMutableObject &gt;</type>
      <name>GetMutableCategorisationObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>acd4db60ee619c33ce2319eb691a95302</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>ICategorySchemeMutableObject</type>
      <name>GetMutableCategoryScheme</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>afa5947f48bc2c34d67482e488d5797c7</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; ICategorySchemeMutableObject &gt;</type>
      <name>GetMutableCategorySchemeObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>ac2c4e67758c555e104c92824a0c998c0</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ICodelistMutableObject</type>
      <name>GetMutableCodelist</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>ad029192edc1698594f280507b48e3c8f</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; ICodelistMutableObject &gt;</type>
      <name>GetMutableCodelistObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a347cca1702826f0ab0214d2ea0cedf18</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IConceptSchemeMutableObject</type>
      <name>GetMutableConceptScheme</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>aa3ac73223113d42cdb2d947d10b2638a</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IConceptSchemeMutableObject &gt;</type>
      <name>GetMutableConceptSchemeObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>ae6d41023c29bb453b7c5aabe1ac8bda7</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IContentConstraintMutableObject</type>
      <name>GetMutableContentConstraint</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a067d6c00b07b1714a7b6b3f17df10cce</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IContentConstraintMutableObject &gt;</type>
      <name>GetMutableContentConstraintObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a070c08f663881df911e7662051154e79</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IDataConsumerSchemeMutableObject</type>
      <name>GetMutableDataConsumerScheme</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>abf8644a9a1eec646b18cb15009f9d168</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IDataConsumerSchemeMutableObject &gt;</type>
      <name>GetMutableDataConsumerSchemeObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>ad9507d6c1c00b671faf5ff8287ded394</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IDataProviderSchemeMutableObject</type>
      <name>GetMutableDataProviderScheme</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>ae62ac3e2b8f1b3fadb22f64310779aba</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IDataProviderSchemeMutableObject &gt;</type>
      <name>GetMutableDataProviderSchemeObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>af62dca88a124e5ed6a772ee422b9430d</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IDataStructureMutableObject</type>
      <name>GetMutableDataStructure</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a66dd28a771c8c4704886a85940532a86</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IDataStructureMutableObject &gt;</type>
      <name>GetMutableDataStructureObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a1b4b61f551c813de29608630b16fc8e2</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IDataflowMutableObject</type>
      <name>GetMutableDataflow</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a8e600253f0f6d6da0515035fd022502f</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList&lt; IMaintainableRefObject &gt; allowedDataflow)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IDataflowMutableObject &gt;</type>
      <name>GetMutableDataflowObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a983d5c77d02d6131ea30fcde72b7ee29</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList&lt; IMaintainableRefObject &gt; allowedDataflow)</arglist>
    </member>
    <member kind="function">
      <type>IHierarchicalCodelistMutableObject</type>
      <name>GetMutableHierarchicCodeList</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>aa148a38256316289e30c8523f9fa868d</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IHierarchicalCodelistMutableObject &gt;</type>
      <name>GetMutableHierarchicCodeListObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a22866eb51572ff8d73d59bbf67f6decb</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IMaintainableMutableObject</type>
      <name>GetMutableMaintainable</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>aeb063fb4266a8f46f7526972ea8b0f8c</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IMaintainableMutableObject &gt;</type>
      <name>GetMutableMaintainables</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a0c53a66e6319a7ee05ac7c85b8e80cdf</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>IMetadataStructureDefinitionMutableObject</type>
      <name>GetMutableMetadataStructure</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>ab8b7e3d1923c8d65e8ce9f8da045a689</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IMetadataStructureDefinitionMutableObject &gt;</type>
      <name>GetMutableMetadataStructureObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a914f1282b7aed2c77f2595dc6be72eeb</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IMetadataFlowMutableObject</type>
      <name>GetMutableMetadataflow</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a9078fcfa0bb75322a7a28bcf4c805666</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IMetadataFlowMutableObject &gt;</type>
      <name>GetMutableMetadataflowObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>ab8ca11417800194174416b270ffbdabc</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IOrganisationUnitSchemeMutableObject</type>
      <name>GetMutableOrganisationUnitScheme</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>aecf304b71bac34f68175cb948d593b35</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IOrganisationUnitSchemeMutableObject &gt;</type>
      <name>GetMutableOrganisationUnitSchemeObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a3e697a7cbd8e2ec061388208d956f1d5</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IProcessMutableObject</type>
      <name>GetMutableProcessObject</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a5cfbe69442e93e7f3db583f39850b377</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IProcessMutableObject &gt;</type>
      <name>GetMutableProcessObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a13cc20b82b1f7b3aea50c5f81ac87d2e</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IProvisionAgreementMutableObject</type>
      <name>GetMutableProvisionAgreement</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>aed51a776e2e195b360a2d856c165b60e</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IProvisionAgreementMutableObject &gt;</type>
      <name>GetMutableProvisionAgreementBeans</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a9908fc5cc9eca5c739d7456e5867e486</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IReportingTaxonomyMutableObject</type>
      <name>GetMutableReportingTaxonomy</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>ab76b8bffd15376648e2d14d5c87b7494</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IReportingTaxonomyMutableObject &gt;</type>
      <name>GetMutableReportingTaxonomyObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a09848714f9f86a90ff918122d0416106</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>IStructureSetMutableObject</type>
      <name>GetMutableStructureSet</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a10e69fdae41f8caa79b0fedfa45275f1</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IStructureSetMutableObject &gt;</type>
      <name>GetMutableStructureSetObjects</name>
      <anchorfile>a00023.html</anchorfile>
      <anchor>a88770efd911df9a766c55c24638746bd</anchor>
      <arglist>(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sdmxsource::Extension::Manager::IAuthCrossReferenceMutableRetrievalManager</name>
    <filename>a00024.html</filename>
    <member kind="function">
      <type>IMutableCrossReferencingTree</type>
      <name>GetCrossReferenceTree</name>
      <anchorfile>a00024.html</anchorfile>
      <anchor>aeb638953b16b0afc4db5a0dbc135790d</anchor>
      <arglist>(IMaintainableMutableObject maintainableObject, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>IList&lt; IMaintainableMutableObject &gt;</type>
      <name>GetCrossReferencedStructures</name>
      <anchorfile>a00024.html</anchorfile>
      <anchor>a819487c808daf9b6937764f3f4b2883b</anchor>
      <arglist>(IStructureReference structureReference, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows, params SdmxStructureType[] structures)</arglist>
    </member>
    <member kind="function">
      <type>IList&lt; IMaintainableMutableObject &gt;</type>
      <name>GetCrossReferencedStructures</name>
      <anchorfile>a00024.html</anchorfile>
      <anchor>a7b333e5bd9ded76ce07c7852bb411459</anchor>
      <arglist>(IIdentifiableMutableObject identifiable, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows, params SdmxStructureType[] structures)</arglist>
    </member>
    <member kind="function">
      <type>IList&lt; IMaintainableMutableObject &gt;</type>
      <name>GetCrossReferencingStructures</name>
      <anchorfile>a00024.html</anchorfile>
      <anchor>aab7e21c130d0a48002d3c8f4e3705a7c</anchor>
      <arglist>(IStructureReference structureReference, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows, params SdmxStructureType[] structures)</arglist>
    </member>
    <member kind="function">
      <type>IList&lt; IMaintainableMutableObject &gt;</type>
      <name>GetCrossReferencingStructures</name>
      <anchorfile>a00024.html</anchorfile>
      <anchor>af6653ba84cc467d515bb81e5d19a70a2</anchor>
      <arglist>(IIdentifiableMutableObject identifiable, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows, params SdmxStructureType[] structures)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sdmxsource::Extension::Manager::IAuthMutableStructureSearchManager</name>
    <filename>a00025.html</filename>
    <member kind="function">
      <type>IMaintainableMutableObject</type>
      <name>GetLatest</name>
      <anchorfile>a00025.html</anchorfile>
      <anchor>a6a31cb30a075045d59139cec14def809</anchor>
      <arglist>(IMaintainableMutableObject maintainableObject, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>IMutableObjects</type>
      <name>GetMaintainables</name>
      <anchorfile>a00025.html</anchorfile>
      <anchor>ac97383178ae2b92a9c454ef0ff7ce8e7</anchor>
      <arglist>(IRestStructureQuery structureQuery, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>IMutableObjects</type>
      <name>RetrieveStructures</name>
      <anchorfile>a00025.html</anchorfile>
      <anchor>a41301a550a1fa68b0fee8d526cde1c5d</anchor>
      <arglist>(IList&lt; IStructureReference &gt; queries, bool resolveReferences, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sdmxsource::Extension::Manager::IAuthSdmxMutableObjectRetrievalManager</name>
    <filename>a00026.html</filename>
    <member kind="function">
      <type>IAgencySchemeMutableObject</type>
      <name>GetMutableAgencyScheme</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a67a15bab65ada9893ff7d584ec8abe01</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IAgencySchemeMutableObject &gt;</type>
      <name>GetMutableAgencySchemeObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>aede12ff07d33ad0ce5a3524536f08ddd</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ICategorisationMutableObject</type>
      <name>GetMutableCategorisation</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a8a98ff95b90ce6fdbe0748cb33a96da6</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; ICategorisationMutableObject &gt;</type>
      <name>GetMutableCategorisationObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a5535751459efda585c534fc442427b4d</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>ICategorySchemeMutableObject</type>
      <name>GetMutableCategoryScheme</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a4d798b11d9c0bbf73e8d602b89e27907</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; ICategorySchemeMutableObject &gt;</type>
      <name>GetMutableCategorySchemeObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>af348fc87590f91415c7e11355da57323</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ICodelistMutableObject</type>
      <name>GetMutableCodelist</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a88651a6a00c8233c94b017e652dce0ad</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; ICodelistMutableObject &gt;</type>
      <name>GetMutableCodelistObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a50bb32f74366b2769af90633896e72a4</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IConceptSchemeMutableObject</type>
      <name>GetMutableConceptScheme</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a492217cf3a8433a3fd90dece8ea3fea5</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IConceptSchemeMutableObject &gt;</type>
      <name>GetMutableConceptSchemeObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a734a97d450bb3be961720880f1868e3e</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IContentConstraintMutableObject</type>
      <name>GetMutableContentConstraint</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a925d1e6f1273fa5fc76d8833f99f1e2e</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IContentConstraintMutableObject &gt;</type>
      <name>GetMutableContentConstraintObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a663980a71fb1aa195b8bf8a42a2a33e6</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IDataConsumerSchemeMutableObject</type>
      <name>GetMutableDataConsumerScheme</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a0fa4e0a0e4da0928350e8e322f128dce</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IDataConsumerSchemeMutableObject &gt;</type>
      <name>GetMutableDataConsumerSchemeObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a2bcf78239e8554d1d0eba254d47a5f51</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IDataProviderSchemeMutableObject</type>
      <name>GetMutableDataProviderScheme</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>ae4fd64e897b0406b0325eae7f5943846</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IDataProviderSchemeMutableObject &gt;</type>
      <name>GetMutableDataProviderSchemeObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a4c28347d63c4d4664aac02aeff6768bd</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IDataStructureMutableObject</type>
      <name>GetMutableDataStructure</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>ac1cf1502d51378ba9a9f477d679e3bf2</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IDataStructureMutableObject &gt;</type>
      <name>GetMutableDataStructureObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>ab3c02121bfdb17c9df3a675805902e2b</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IDataflowMutableObject</type>
      <name>GetMutableDataflow</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a966c5d4baf9babc163722119f0e25aa4</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IDataflowMutableObject &gt;</type>
      <name>GetMutableDataflowObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a03939fa83861666b195ad69f7ab21dc5</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>IHierarchicalCodelistMutableObject</type>
      <name>GetMutableHierarchicCodeList</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a376115eeb3cbb8626a9d83826dcb6d7b</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IHierarchicalCodelistMutableObject &gt;</type>
      <name>GetMutableHierarchicCodeListObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a55f37594e67dd14cb5b59669857f148d</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IMaintainableMutableObject</type>
      <name>GetMutableMaintainable</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>ab843554a12b407cbcdec95c35162431e</anchor>
      <arglist>(IStructureReference query, bool returnLatest, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IMaintainableMutableObject &gt;</type>
      <name>GetMutableMaintainables</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a3bb6c7eed73319acbb318af87bc24221</anchor>
      <arglist>(IStructureReference query, bool returnLatest, bool returnStub, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>IMetadataStructureDefinitionMutableObject</type>
      <name>GetMutableMetadataStructure</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a3360e408d7fae89a5988cbe7bf437c1a</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IMetadataStructureDefinitionMutableObject &gt;</type>
      <name>GetMutableMetadataStructureObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a0c79d0d7b47426c73f58890acabeec63</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IMetadataFlowMutableObject</type>
      <name>GetMutableMetadataflow</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>afef51e1bdcfe4cfa872c2f932cc1c24f</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IMetadataFlowMutableObject &gt;</type>
      <name>GetMutableMetadataflowObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a28cf5eeceab2f0db1cac4bb34d2a7d7f</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IOrganisationUnitSchemeMutableObject</type>
      <name>GetMutableOrganisationUnitScheme</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a31956e56de9155f630f1b39abfac9560</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IOrganisationUnitSchemeMutableObject &gt;</type>
      <name>GetMutableOrganisationUnitSchemeObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a1a7cd55be8cf63662bd348c2cdbbf42b</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IProcessMutableObject</type>
      <name>GetMutableProcessObject</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>ac81b1cafda9946ceb622cca79a8f0aae</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IProcessMutableObject &gt;</type>
      <name>GetMutableProcessObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a7c7b0ca6dbae4acb5ed068d02f1b2c45</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IProvisionAgreementMutableObject</type>
      <name>GetMutableProvisionAgreement</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>aa1c312fd7ac20c06dc97f792f0fc9556</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IProvisionAgreementMutableObject &gt;</type>
      <name>GetMutableProvisionAgreementObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>ae3b11820f03468b0d1860163a8bec65e</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IReportingTaxonomyMutableObject</type>
      <name>GetMutableReportingTaxonomy</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>abae0e04c870e0b7ad1dd998a89b5d3a2</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IReportingTaxonomyMutableObject &gt;</type>
      <name>GetMutableReportingTaxonomyObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a3536a41360d5972b911fb73845f9a54c</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>IStructureSetMutableObject</type>
      <name>GetMutableStructureSet</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>aae6e0da2ed2349201335cf1b816f18ad</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; IStructureSetMutableObject &gt;</type>
      <name>GetMutableStructureSetObjects</name>
      <anchorfile>a00026.html</anchorfile>
      <anchor>a80276fe3feea28c22a5484e219b2dd6d</anchor>
      <arglist>(IMaintainableRefObject xref, bool returnLatest, bool returnStub)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sdmxsource::Extension::Manager::ISpecialMutableObjectRetrievalManager</name>
    <filename>a00037.html</filename>
    <member kind="function">
      <type>ISet&lt; ICodelistMutableObject &gt;</type>
      <name>GetMutableCodelistObjects</name>
      <anchorfile>a00037.html</anchorfile>
      <anchor>aba09931487de5ecde793f6141fa61bc0</anchor>
      <arglist>(IMaintainableRefObject codelistReference, IMaintainableRefObject dataflowReference, string componentConceptRef, bool isTranscoded, IList&lt; IMaintainableRefObject &gt; allowedDataflows)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; ICodelistMutableObject &gt;</type>
      <name>GetMutableCodelistObjects</name>
      <anchorfile>a00037.html</anchorfile>
      <anchor>aaf2a7f31194983e3113910e59beb4970</anchor>
      <arglist>(IMaintainableRefObject codelistReference, IList&lt; string &gt; subset)</arglist>
    </member>
    <member kind="function">
      <type>ISet&lt; ICodelistMutableObject &gt;</type>
      <name>GetMutableCodelistObjects</name>
      <anchorfile>a00037.html</anchorfile>
      <anchor>a56996a6946c72117e676edea055d47fa</anchor>
      <arglist>(IMaintainableRefObject codelistReference)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sdmxsource::Extension::Manager::Data</name>
    <filename>a00120.html</filename>
    <class kind="class">Estat::Sdmxsource::Extension::Manager::Data::AbstractAdvancedSdmxDataRetrievalWithWriter</class>
    <class kind="class">Estat::Sdmxsource::Extension::Manager::Data::AbstractSdmxDataRetrievalWithCrossWriter</class>
    <class kind="class">Estat::Sdmxsource::Extension::Manager::Data::AbstractSdmxDataRetrievalWithWriter</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sdmxsource::Extension::Manager::Data::AbstractAdvancedSdmxDataRetrievalWithWriter</name>
    <filename>a00001.html</filename>
    <member kind="function" virtualness="virtual">
      <type>virtual void</type>
      <name>GetData</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>a3d85c9a40022dce91981e423c167592b</anchor>
      <arglist>(IComplexDataQuery dataQuery, IDataWriterEngine dataWriter)</arglist>
    </member>
    <member kind="function" protection="protected">
      <type></type>
      <name>AbstractAdvancedSdmxDataRetrievalWithWriter</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>a9a65e7547944153ea51717495255f11a</anchor>
      <arglist>(IAdvancedSdmxDataRetrievalWithWriter advancedSdmxDataRetrievalWithWriter)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sdmxsource::Extension::Manager::Data::AbstractSdmxDataRetrievalWithCrossWriter</name>
    <filename>a00002.html</filename>
    <member kind="function" virtualness="virtual">
      <type>virtual void</type>
      <name>GetData</name>
      <anchorfile>a00002.html</anchorfile>
      <anchor>a99d883460f3c731414bc57c85bf0af26</anchor>
      <arglist>(IDataQuery dataQuery, ICrossSectionalWriterEngine dataWriter)</arglist>
    </member>
    <member kind="function" protection="protected">
      <type></type>
      <name>AbstractSdmxDataRetrievalWithCrossWriter</name>
      <anchorfile>a00002.html</anchorfile>
      <anchor>a5ecf81c0cd9e1203ad5a319582a0ddd9</anchor>
      <arglist>(ISdmxDataRetrievalWithCrossWriter dataRetrievalWithCrossWriter)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sdmxsource::Extension::Manager::Data::AbstractSdmxDataRetrievalWithWriter</name>
    <filename>a00003.html</filename>
    <member kind="function" virtualness="virtual">
      <type>virtual void</type>
      <name>GetData</name>
      <anchorfile>a00003.html</anchorfile>
      <anchor>a3a5bca3587f714d14b1c4fb3b401e7a3</anchor>
      <arglist>(IDataQuery dataQuery, IDataWriterEngine dataWriter)</arglist>
    </member>
    <member kind="function" protection="protected">
      <type></type>
      <name>AbstractSdmxDataRetrievalWithWriter</name>
      <anchorfile>a00003.html</anchorfile>
      <anchor>a853b4bfddd1032d1cc6c162c06c6fce1</anchor>
      <arglist>(ISdmxDataRetrievalWithWriter dataRetrievalWithWriter)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sdmxsource::Extension::Util</name>
    <filename>a00121.html</filename>
    <class kind="class">Estat::Sdmxsource::Extension::Util::SdmxMessageUtilExt</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sdmxsource::Extension::Util::SdmxMessageUtilExt</name>
    <filename>a00043.html</filename>
    <member kind="function" static="yes">
      <type>static IFooterMessage</type>
      <name>ParseSdmxFooterMessage</name>
      <anchorfile>a00043.html</anchorfile>
      <anchor>a9244af705053a1ee174b22b7fec1f00b</anchor>
      <arglist>(IReadableDataLocation dataLocation)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sri</name>
    <filename>a00122.html</filename>
    <namespace>Estat::Sri::CustomRequests</namespace>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sri::CustomRequests</name>
    <filename>a00123.html</filename>
    <namespace>Estat::Sri::CustomRequests::Builder</namespace>
    <namespace>Estat::Sri::CustomRequests::Constants</namespace>
    <namespace>Estat::Sri::CustomRequests::Extension</namespace>
    <namespace>Estat::Sri::CustomRequests::Factory</namespace>
    <namespace>Estat::Sri::CustomRequests::Manager</namespace>
    <namespace>Estat::Sri::CustomRequests::Model</namespace>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sri::CustomRequests::Builder</name>
    <filename>a00124.html</filename>
    <namespace>Estat::Sri::CustomRequests::Builder::QueryBuilder</namespace>
    <class kind="class">Estat::Sri::CustomRequests::Builder::ConstrainQueryBuilderV2</class>
    <class kind="interface">Estat::Sri::CustomRequests::Builder::IComplexDataQueryBuilder</class>
    <class kind="interface">Estat::Sri::CustomRequests::Builder::IComplexStructureQueryBuilder&lt; T &gt;</class>
    <class kind="interface">Estat::Sri::CustomRequests::Builder::IQueryStructureRequestBuilder&lt; out T &gt;</class>
    <class kind="class">Estat::Sri::CustomRequests::Builder::QueryStructureRequestBuilderV2</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Builder::ConstrainQueryBuilderV2</name>
    <filename>a00014.html</filename>
    <member kind="function" protection="protected">
      <type>override IStructureReference</type>
      <name>BuildDataflowQuery</name>
      <anchorfile>a00014.html</anchorfile>
      <anchor>a3968ea4fedbdb1eb5d6f7d1d2bae7909</anchor>
      <arglist>(DataflowRefType dataflowRefType)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Builder::IComplexDataQueryBuilder</name>
    <filename>a00027.html</filename>
    <member kind="function">
      <type>XDocument</type>
      <name>BuildComplexDataQuery</name>
      <anchorfile>a00027.html</anchorfile>
      <anchor>afb531a9640490364a59cd703386db56c</anchor>
      <arglist>(IComplexDataQuery query)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Builder::IComplexStructureQueryBuilder&lt; T &gt;</name>
    <filename>a00030.html</filename>
    <templarg></templarg>
    <member kind="function">
      <type>T</type>
      <name>BuildComplexStructureQuery</name>
      <anchorfile>a00030.html</anchorfile>
      <anchor>a91100cf1fafaae43e192ea0d22213525</anchor>
      <arglist>(IComplexStructureQuery query)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Builder::IQueryStructureRequestBuilder&lt; out T &gt;</name>
    <filename>a00034.html</filename>
    <templarg>T</templarg>
    <member kind="function">
      <type>T</type>
      <name>BuildStructureQuery</name>
      <anchorfile>a00034.html</anchorfile>
      <anchor>a4f7455cf712a1b24bc71cd891f570910</anchor>
      <arglist>(IEnumerable&lt; IStructureReference &gt; queries, bool resolveReferences)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Builder::QueryStructureRequestBuilderV2</name>
    <filename>a00040.html</filename>
    <base>Estat::Sri::CustomRequests::Builder::IQueryStructureRequestBuilder&lt; out T &gt;</base>
    <member kind="function">
      <type></type>
      <name>QueryStructureRequestBuilderV2</name>
      <anchorfile>a00040.html</anchorfile>
      <anchor>a08bdfb47ed0e990fbc7d751056dc63dc</anchor>
      <arglist>(IHeader header)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>QueryStructureRequestBuilderV2</name>
      <anchorfile>a00040.html</anchorfile>
      <anchor>af6fdfc8a9d04313b8680c94bbf55933f</anchor>
      <arglist>(IHeader header, IBuilder&lt; HeaderType, IHeader &gt; headerXmlBuilder)</arglist>
    </member>
    <member kind="function">
      <type>XDocument</type>
      <name>BuildStructureQuery</name>
      <anchorfile>a00040.html</anchorfile>
      <anchor>a5e1deffeaee602865e226da0353ee1dc</anchor>
      <arglist>(IEnumerable&lt; IStructureReference &gt; query, bool resolveReferences)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sri::CustomRequests::Builder::QueryBuilder</name>
    <filename>a00125.html</filename>
    <class kind="class">Estat::Sri::CustomRequests::Builder::QueryBuilder::ComplexDataQueryCoreBuilderV21</class>
    <class kind="class">Estat::Sri::CustomRequests::Builder::QueryBuilder::ComplexStructureQueryBuilderV21</class>
    <class kind="class">Estat::Sri::CustomRequests::Builder::QueryBuilder::GenericDataQueryBuilderV21</class>
    <class kind="class">Estat::Sri::CustomRequests::Builder::QueryBuilder::GenericTimeSeriesDataQueryBuilderV21</class>
    <class kind="interface">Estat::Sri::CustomRequests::Builder::QueryBuilder::IParameterBuilder</class>
    <class kind="class">Estat::Sri::CustomRequests::Builder::QueryBuilder::ParameterBuilderAnd</class>
    <class kind="class">Estat::Sri::CustomRequests::Builder::QueryBuilder::ParameterBuilderOr</class>
    <class kind="class">Estat::Sri::CustomRequests::Builder::QueryBuilder::StructSpecificDataQueryBuilderV21</class>
    <class kind="class">Estat::Sri::CustomRequests::Builder::QueryBuilder::StructSpecificTimeSeriesQueryBuilderV21</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Builder::QueryBuilder::ComplexDataQueryCoreBuilderV21</name>
    <filename>a00006.html</filename>
    <member kind="function">
      <type>void</type>
      <name>FillDataQueryType</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a58b0777eee11dd9d1759b72f3abd4c70</anchor>
      <arglist>(DataQueryType queryType, IComplexDataQuery complexDataQuery)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Builder::QueryBuilder::ComplexStructureQueryBuilderV21</name>
    <filename>a00011.html</filename>
    <base>Estat::Sri::CustomRequests::Builder::IComplexStructureQueryBuilder&lt; T &gt;</base>
    <member kind="function">
      <type>XDocument</type>
      <name>BuildComplexStructureQuery</name>
      <anchorfile>a00011.html</anchorfile>
      <anchor>a2f0c9e02aa8535f6c342502511c9346f</anchor>
      <arglist>(IComplexStructureQuery query)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Builder::QueryBuilder::GenericDataQueryBuilderV21</name>
    <filename>a00019.html</filename>
    <base>Estat::Sri::CustomRequests::Builder::IComplexDataQueryBuilder</base>
    <member kind="function">
      <type>XDocument</type>
      <name>BuildComplexDataQuery</name>
      <anchorfile>a00019.html</anchorfile>
      <anchor>a56676ddb287aac872912a81cbeb4914d</anchor>
      <arglist>(IComplexDataQuery query)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Builder::QueryBuilder::GenericTimeSeriesDataQueryBuilderV21</name>
    <filename>a00021.html</filename>
    <base>Estat::Sri::CustomRequests::Builder::IComplexDataQueryBuilder</base>
    <member kind="function">
      <type>XDocument</type>
      <name>BuildComplexDataQuery</name>
      <anchorfile>a00021.html</anchorfile>
      <anchor>a53b7cb0ba8da317d9d516cc2b5ffb8e6</anchor>
      <arglist>(IComplexDataQuery query)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Builder::QueryBuilder::StructSpecificDataQueryBuilderV21</name>
    <filename>a00046.html</filename>
    <base>Estat::Sri::CustomRequests::Builder::IComplexDataQueryBuilder</base>
    <member kind="function">
      <type>XDocument</type>
      <name>BuildComplexDataQuery</name>
      <anchorfile>a00046.html</anchorfile>
      <anchor>a9811a4a0923b106fc080460c377fa338</anchor>
      <arglist>(IComplexDataQuery query)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Builder::QueryBuilder::StructSpecificTimeSeriesQueryBuilderV21</name>
    <filename>a00048.html</filename>
    <base>Estat::Sri::CustomRequests::Builder::IComplexDataQueryBuilder</base>
    <member kind="function">
      <type>XDocument</type>
      <name>BuildComplexDataQuery</name>
      <anchorfile>a00048.html</anchorfile>
      <anchor>a468d626ac2fd057867811a808cce14d9</anchor>
      <arglist>(IComplexDataQuery query)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sri::CustomRequests::Constants</name>
    <filename>a00126.html</filename>
    <class kind="class">Estat::Sri::CustomRequests::Constants::SpecialValues</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Constants::SpecialValues</name>
    <filename>a00044.html</filename>
    <member kind="variable">
      <type>const string</type>
      <name>DummyMemberValue</name>
      <anchorfile>a00044.html</anchorfile>
      <anchor>ab5bcffe0fd8b54f771599f463c484248</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sri::CustomRequests::Extension</name>
    <filename>a00127.html</filename>
    <class kind="class">Estat::Sri::CustomRequests::Extension::ComplexDataQueryExtension</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Extension::ComplexDataQueryExtension</name>
    <filename>a00007.html</filename>
    <member kind="function" static="yes">
      <type>static bool</type>
      <name>ShouldUseAnd</name>
      <anchorfile>a00007.html</anchorfile>
      <anchor>af64ce00a535423e1b569516055a1e481</anchor>
      <arglist>(this IComplexDataQuerySelection selection)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sri::CustomRequests::Factory</name>
    <filename>a00128.html</filename>
    <class kind="class">Estat::Sri::CustomRequests::Factory::ComplexDataQueryFactoryV21</class>
    <class kind="class">Estat::Sri::CustomRequests::Factory::ComplexStructureQueryFactoryV21&lt; T &gt;</class>
    <class kind="interface">Estat::Sri::CustomRequests::Factory::IComplexDataQueryFactory</class>
    <class kind="interface">Estat::Sri::CustomRequests::Factory::IComplexStructureQueryFactory&lt; T &gt;</class>
    <class kind="interface">Estat::Sri::CustomRequests::Factory::IQueryStructureRequestFactory</class>
    <class kind="class">Estat::Sri::CustomRequests::Factory::QueryStructureRequestFactory</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Factory::ComplexDataQueryFactoryV21</name>
    <filename>a00008.html</filename>
    <base>Estat::Sri::CustomRequests::Factory::IComplexDataQueryFactory</base>
    <member kind="function">
      <type>IComplexDataQueryBuilder</type>
      <name>GetComplexDataQueryBuilder</name>
      <anchorfile>a00008.html</anchorfile>
      <anchor>a380fb8b0eb3abdb8634f28e993a626cf</anchor>
      <arglist>(IDataQueryFormat&lt; XDocument &gt; format)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Factory::ComplexStructureQueryFactoryV21&lt; T &gt;</name>
    <filename>a00012.html</filename>
    <templarg></templarg>
    <base>Estat::Sri::CustomRequests::Factory::IComplexStructureQueryFactory&lt; T &gt;</base>
    <member kind="function">
      <type>IComplexStructureQueryBuilder&lt; T &gt;</type>
      <name>GetComplexStructureQueryBuilder</name>
      <anchorfile>a00032.html</anchorfile>
      <anchor>a1637596e3f4afaca17300a3fea829398</anchor>
      <arglist>(IStructureQueryFormat&lt; T &gt; format)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Factory::IComplexDataQueryFactory</name>
    <filename>a00029.html</filename>
    <member kind="function">
      <type>IComplexDataQueryBuilder</type>
      <name>GetComplexDataQueryBuilder</name>
      <anchorfile>a00029.html</anchorfile>
      <anchor>a4a3fb8e1e052dcfe3da12bf89ac3e6a4</anchor>
      <arglist>(IDataQueryFormat&lt; XDocument &gt; format)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Factory::IComplexStructureQueryFactory&lt; T &gt;</name>
    <filename>a00032.html</filename>
    <templarg></templarg>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Factory::IQueryStructureRequestFactory</name>
    <filename>a00036.html</filename>
    <member kind="function">
      <type>IQueryStructureRequestBuilder&lt; T &gt;</type>
      <name>GetStructureQueryBuilder&lt; T &gt;</name>
      <anchorfile>a00036.html</anchorfile>
      <anchor>a7985103c83f6dd016e3639a977162f05</anchor>
      <arglist>(IStructureQueryFormat&lt; T &gt; format)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Factory::QueryStructureRequestFactory</name>
    <filename>a00041.html</filename>
    <base>Estat::Sri::CustomRequests::Factory::IQueryStructureRequestFactory</base>
    <member kind="function">
      <type></type>
      <name>QueryStructureRequestFactory</name>
      <anchorfile>a00041.html</anchorfile>
      <anchor>a954c049956f8f1e8e63904570038a6ba</anchor>
      <arglist>(IQueryStructureRequestBuilder&lt; XDocument &gt; requestBuilder)</arglist>
    </member>
    <member kind="function">
      <type>IQueryStructureRequestBuilder&lt; T &gt;</type>
      <name>GetStructureQueryBuilder&lt; T &gt;</name>
      <anchorfile>a00041.html</anchorfile>
      <anchor>ae2a2abe16942ada01e608eb94ff319f9</anchor>
      <arglist>(IStructureQueryFormat&lt; T &gt; format)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sri::CustomRequests::Manager</name>
    <filename>a00129.html</filename>
    <class kind="class">Estat::Sri::CustomRequests::Manager::ComplexDataQueryBuilderManager</class>
    <class kind="class">Estat::Sri::CustomRequests::Manager::ComplexStructureQueryBuilderManager&lt; T &gt;</class>
    <class kind="class">Estat::Sri::CustomRequests::Manager::CustomQueryParseManager</class>
    <class kind="interface">Estat::Sri::CustomRequests::Manager::IComplexDataQueryBuilderManager</class>
    <class kind="interface">Estat::Sri::CustomRequests::Manager::IComplexStructureQueryBuilderManager&lt; T &gt;</class>
    <class kind="interface">Estat::Sri::CustomRequests::Manager::IQueryStructureRequestBuilderManager</class>
    <class kind="class">Estat::Sri::CustomRequests::Manager::QueryStructureRequestBuilderManager</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Manager::ComplexDataQueryBuilderManager</name>
    <filename>a00005.html</filename>
    <base>Estat::Sri::CustomRequests::Manager::IComplexDataQueryBuilderManager</base>
    <member kind="function">
      <type></type>
      <name>ComplexDataQueryBuilderManager</name>
      <anchorfile>a00005.html</anchorfile>
      <anchor>ac077d18cfd5491ca4cdbcccedd610c16</anchor>
      <arglist>(IComplexDataQueryFactory factory)</arglist>
    </member>
    <member kind="function">
      <type>XDocument</type>
      <name>BuildComplexDataQuery</name>
      <anchorfile>a00005.html</anchorfile>
      <anchor>a7a7a6de43b0e4f5d51c682573b8a9355</anchor>
      <arglist>(IComplexDataQuery complexDataQuery, IDataQueryFormat&lt; XDocument &gt; dataQueryFormat)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Manager::ComplexStructureQueryBuilderManager&lt; T &gt;</name>
    <filename>a00010.html</filename>
    <templarg></templarg>
    <base>Estat::Sri::CustomRequests::Manager::IComplexStructureQueryBuilderManager&lt; T &gt;</base>
    <member kind="function">
      <type></type>
      <name>ComplexStructureQueryBuilderManager</name>
      <anchorfile>a00010.html</anchorfile>
      <anchor>a31d98cb943f248f6c9cc3c8eeb95f79f</anchor>
      <arglist>(IComplexStructureQueryFactory&lt; T &gt; factory)</arglist>
    </member>
    <member kind="function">
      <type>T</type>
      <name>BuildComplexStructureQuery</name>
      <anchorfile>a00010.html</anchorfile>
      <anchor>afd1ad9c4dc0c75fde7c189dc202b0e42</anchor>
      <arglist>(IComplexStructureQuery query, IStructureQueryFormat&lt; T &gt; structureQueryFormat)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Manager::CustomQueryParseManager</name>
    <filename>a00016.html</filename>
    <member kind="function">
      <type></type>
      <name>CustomQueryParseManager</name>
      <anchorfile>a00016.html</anchorfile>
      <anchor>af3eae13592fdb1a36877efa4798adb22</anchor>
      <arglist>(SdmxSchemaEnumType sdmxSchema)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Manager::IComplexDataQueryBuilderManager</name>
    <filename>a00028.html</filename>
    <member kind="function">
      <type>XDocument</type>
      <name>BuildComplexDataQuery</name>
      <anchorfile>a00028.html</anchorfile>
      <anchor>a65540c8fe4b0299cdbb8a1f196d8d8ee</anchor>
      <arglist>(IComplexDataQuery complexDataQuery, IDataQueryFormat&lt; XDocument &gt; dataQueryFormat)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Manager::IComplexStructureQueryBuilderManager&lt; T &gt;</name>
    <filename>a00031.html</filename>
    <templarg></templarg>
    <member kind="function">
      <type>T</type>
      <name>BuildComplexStructureQuery</name>
      <anchorfile>a00031.html</anchorfile>
      <anchor>ac294538a133b2385fbe7d7197f70ac0a</anchor>
      <arglist>(IComplexStructureQuery complexStructureQuery, IStructureQueryFormat&lt; T &gt; structureQueryFormat)</arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Manager::IQueryStructureRequestBuilderManager</name>
    <filename>a00035.html</filename>
    <member kind="function">
      <type>T</type>
      <name>BuildStructureQuery&lt; T &gt;</name>
      <anchorfile>a00035.html</anchorfile>
      <anchor>adff1dd9013586dc3cadbaf89a125acd2</anchor>
      <arglist>(IEnumerable&lt; IStructureReference &gt; structureReferences, IStructureQueryFormat&lt; T &gt; structureQueryFormat, bool resolveReferences)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Manager::QueryStructureRequestBuilderManager</name>
    <filename>a00039.html</filename>
    <base>Estat::Sri::CustomRequests::Manager::IQueryStructureRequestBuilderManager</base>
    <member kind="function">
      <type></type>
      <name>QueryStructureRequestBuilderManager</name>
      <anchorfile>a00039.html</anchorfile>
      <anchor>a9016dabce4d498e6bc1d91b8711d53ef</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>QueryStructureRequestBuilderManager</name>
      <anchorfile>a00039.html</anchorfile>
      <anchor>a3e7503a1c7f108bcde00c9af77c01c8a</anchor>
      <arglist>(IHeader header)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>QueryStructureRequestBuilderManager</name>
      <anchorfile>a00039.html</anchorfile>
      <anchor>a5ed022351947c2d3b0078c6c07e781c7</anchor>
      <arglist>(IQueryStructureRequestFactory[] factories)</arglist>
    </member>
    <member kind="function">
      <type>T</type>
      <name>BuildStructureQuery&lt; T &gt;</name>
      <anchorfile>a00039.html</anchorfile>
      <anchor>aa8a2dd08f2f11813b004dad2ac76fa77</anchor>
      <arglist>(IEnumerable&lt; IStructureReference &gt; structureReferences, IStructureQueryFormat&lt; T &gt; structureQueryFormat, bool resolveReferences)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Sri::CustomRequests::Model</name>
    <filename>a00130.html</filename>
    <class kind="class">Estat::Sri::CustomRequests::Model::ComplexQueryFormatV21</class>
    <class kind="class">Estat::Sri::CustomRequests::Model::ConstrainableStructureReference</class>
    <class kind="class">Estat::Sri::CustomRequests::Model::GenericDataDocumentFormatV21</class>
    <class kind="class">Estat::Sri::CustomRequests::Model::GenericTimeSeriesDataFormatV21</class>
    <class kind="interface">Estat::Sri::CustomRequests::Model::IConstrainableStructureReference</class>
    <class kind="class">Estat::Sri::CustomRequests::Model::QueryStructureRequestFormat</class>
    <class kind="class">Estat::Sri::CustomRequests::Model::StructSpecificDataFormatV21</class>
    <class kind="class">Estat::Sri::CustomRequests::Model::StructSpecificTimeSeriesDataFormatV21</class>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Model::ComplexQueryFormatV21</name>
    <filename>a00009.html</filename>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Model::ConstrainableStructureReference</name>
    <filename>a00013.html</filename>
    <base>Estat::Sri::CustomRequests::Model::IConstrainableStructureReference</base>
    <member kind="function">
      <type></type>
      <name>ConstrainableStructureReference</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>a8bde4f2ad44b0e478cfe219ae85c2935</anchor>
      <arglist>(Uri urn)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>ConstrainableStructureReference</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>adaafc5e3d966d5bccee1d176bd902ee0</anchor>
      <arglist>(Uri urn, IContentConstraintObject constraint)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>ConstrainableStructureReference</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>ae614d52e6b7e2356571f984179b5c941</anchor>
      <arglist>(string urn, IContentConstraintObject constraint)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>ConstrainableStructureReference</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>a481e6b600c9a5c4f7a4bdf746fc4aa1f</anchor>
      <arglist>(IStructureReference structureReference, IContentConstraintObject constraint)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>ConstrainableStructureReference</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>afed62d2fc8772e24acd2777b4d40b6ba</anchor>
      <arglist>(string agencyId, string maintainableId, string version, SdmxStructureType targetStructureEnum, IContentConstraintObject constraint, params string[] identfiableIds)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>ConstrainableStructureReference</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>a7ed99c023e2c6e3e38494835f01a67c9</anchor>
      <arglist>(string agencyId, string maintainableId, string version, SdmxStructureEnumType targetStructureEnum, IContentConstraintObject constraint, params string[] identfiableIds)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>ConstrainableStructureReference</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>aa335a2efdef0ab03a34c97c7e97051bc</anchor>
      <arglist>(IMaintainableRefObject crossReference, SdmxStructureEnumType structureEnumType, IContentConstraintObject constraint)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>ConstrainableStructureReference</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>ae431a8cd486744c0415253155dfd1705</anchor>
      <arglist>(string agencyId, string maintainableId, string version, SdmxStructureType targetStructureEnum, IList&lt; string &gt; identfiableIds, IContentConstraintObject constraint)</arglist>
    </member>
    <member kind="function">
      <type>override bool</type>
      <name>Equals</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>aa6fabe12d3967c38c14ab5ef1e24233a</anchor>
      <arglist>(object obj)</arglist>
    </member>
    <member kind="function">
      <type>override int</type>
      <name>GetHashCode</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>ab7548f949f1d697554294d39a5e29003</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function" protection="protected">
      <type>bool</type>
      <name>Equals</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>af375b8339afb589c7a26d57009c6fe1f</anchor>
      <arglist>(ConstrainableStructureReference other)</arglist>
    </member>
    <member kind="property">
      <type>IContentConstraintObject</type>
      <name>ConstraintObject</name>
      <anchorfile>a00013.html</anchorfile>
      <anchor>a449ddca923a0e02810b08e3ea5d87aed</anchor>
      <arglist></arglist>
    </member>
    <member kind="property">
      <type>IContentConstraintObject</type>
      <name>ConstraintObject</name>
      <anchorfile>a00033.html</anchorfile>
      <anchor>a5f476d94d60dedf5e3a71036bed7f9d0</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Model::GenericDataDocumentFormatV21</name>
    <filename>a00018.html</filename>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Model::GenericTimeSeriesDataFormatV21</name>
    <filename>a00020.html</filename>
  </compound>
  <compound kind="interface">
    <name>Estat::Sri::CustomRequests::Model::IConstrainableStructureReference</name>
    <filename>a00033.html</filename>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Model::QueryStructureRequestFormat</name>
    <filename>a00042.html</filename>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Model::StructSpecificDataFormatV21</name>
    <filename>a00045.html</filename>
  </compound>
  <compound kind="class">
    <name>Estat::Sri::CustomRequests::Model::StructSpecificTimeSeriesDataFormatV21</name>
    <filename>a00047.html</filename>
  </compound>
  <compound kind="dir">
    <name>EstatSdmxSourceExtension/Builder</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/EstatSdmxSourceExtension/Builder/</path>
    <filename>dir_db5b049863b10baf7e39812552801e22.html</filename>
    <file>ComplexDataQuery2DataQueryBuilder.cs</file>
    <file>DataQuery2ComplexQueryBuilder.cs</file>
    <file>StructureQuery2ComplexQueryBuilder.cs</file>
  </compound>
  <compound kind="dir">
    <name>CustomRequests/Builder</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/CustomRequests/Builder/</path>
    <filename>dir_5e6aad430a79e9d7b33fe291abf0d51c.html</filename>
    <dir>CustomRequests/Builder/QueryBuilder</dir>
    <file>ConstrainQueryBuilderV2.cs</file>
    <file>IComplexDataQueryBuilder.cs</file>
    <file>IComplexStructureQueryBuilder.cs</file>
    <file>IQueryStructureRequestBuilder.cs</file>
    <file>QueryStructureRequestBuilderV2.cs</file>
  </compound>
  <compound kind="dir">
    <name>EstatSdmxSourceExtension/Constant</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/EstatSdmxSourceExtension/Constant/</path>
    <filename>dir_c80500d1e44ab8560b4e8486592bedeb.html</filename>
    <file>CustomAnnotationType.cs</file>
    <file>CustomAnnotationTypeExtensions.cs</file>
  </compound>
  <compound kind="dir">
    <name>CustomRequests/Constants</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/CustomRequests/Constants/</path>
    <filename>dir_bdb261dfa936386f059a39b74336b7be.html</filename>
    <file>SpecialValues.cs</file>
  </compound>
  <compound kind="dir">
    <name>CustomRequests</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/CustomRequests/</path>
    <filename>dir_d28734209465e71727657e3973474121.html</filename>
    <dir>CustomRequests/Builder</dir>
    <dir>CustomRequests/Constants</dir>
    <dir>CustomRequests/Extension</dir>
    <dir>CustomRequests/Factory</dir>
    <dir>CustomRequests/Manager</dir>
    <dir>CustomRequests/Model</dir>
    <dir>CustomRequests/Properties</dir>
  </compound>
  <compound kind="dir">
    <name>EstatSdmxSourceExtension/Manager/Data</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/EstatSdmxSourceExtension/Manager/Data/</path>
    <filename>dir_3c3666a4f29725840e7655347eed78e4.html</filename>
    <file>AbstractAdvancedSdmxDataRetrievalWithWriter .cs</file>
    <file>AbstractSdmxDataRetrievalWithCrossWriter.cs</file>
    <file>AbstractSdmxDataRetrievalWithWriter.cs</file>
  </compound>
  <compound kind="dir">
    <name>EstatSdmxSourceExtension</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/EstatSdmxSourceExtension/</path>
    <filename>dir_167c39ade6385a27dd07903fd0ec3ec3.html</filename>
    <dir>EstatSdmxSourceExtension/Builder</dir>
    <dir>EstatSdmxSourceExtension/Constant</dir>
    <dir>EstatSdmxSourceExtension/Extension</dir>
    <dir>EstatSdmxSourceExtension/Manager</dir>
    <dir>EstatSdmxSourceExtension/Properties</dir>
    <dir>EstatSdmxSourceExtension/Util</dir>
  </compound>
  <compound kind="dir">
    <name>EstatSdmxSourceExtension/Extension</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/EstatSdmxSourceExtension/Extension/</path>
    <filename>dir_0465b3dd02eafa249b9f714392277bdd.html</filename>
    <file>QueryExtensions.cs</file>
  </compound>
  <compound kind="dir">
    <name>CustomRequests/Extension</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/CustomRequests/Extension/</path>
    <filename>dir_3d9c2ee9972a30955cc8c313ac86dcfe.html</filename>
    <file>ComplexDataQueryExtension.cs</file>
  </compound>
  <compound kind="dir">
    <name>CustomRequests/Factory</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/CustomRequests/Factory/</path>
    <filename>dir_49a37ee0c0be3a1566c07cea6bfecde7.html</filename>
    <file>ComplexDataQueryFactoryV21.cs</file>
    <file>ComplexStructureQueryFactoryV21.cs</file>
    <file>IComplexDataQueryFactory.cs</file>
    <file>IComplexStructureQueryFactory.cs</file>
    <file>IQueryStructureRequestFactory.cs</file>
    <file>QueryStructureRequestFactory.cs</file>
  </compound>
  <compound kind="dir">
    <name>EstatSdmxSourceExtension/Manager</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/EstatSdmxSourceExtension/Manager/</path>
    <filename>dir_ab2ee6f2cc44f3c561b18a18d732ae36.html</filename>
    <dir>EstatSdmxSourceExtension/Manager/Data</dir>
    <file>IAuthAdvancedMutableStructureSearchManager.cs</file>
    <file>IAuthAdvancedSdmxMutableObjectRetrievalManager.cs</file>
    <file>IAuthCrossReferenceMutableRetrievalManager.cs</file>
    <file>IAuthMutableStructureSearchManager.cs</file>
    <file>IAuthSdmxMutableObjectRetrievalManager.cs</file>
    <file>ISpecialMutableObjectRetrievalManager.cs</file>
  </compound>
  <compound kind="dir">
    <name>CustomRequests/Manager</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/CustomRequests/Manager/</path>
    <filename>dir_284cb11dff514200ff6a05032867fead.html</filename>
    <file>ComplexDataQueryBuilderManager.cs</file>
    <file>ComplexStructureQueryBuilderManager.cs</file>
    <file>CustomQueryParseManager.cs</file>
    <file>IComplexDataQueryBuilderManager.cs</file>
    <file>IComplexStructureQueryBuilderManager.cs</file>
    <file>IQueryStructureRequestBuilderManager.cs</file>
    <file>QueryStructureRequestBuilderManager.cs</file>
  </compound>
  <compound kind="dir">
    <name>CustomRequests/Model</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/CustomRequests/Model/</path>
    <filename>dir_bd17c78b72b63ffcb08857537a0a31d9.html</filename>
    <file>ComplexQueryFormatV21.cs</file>
    <file>ConstrainableStructureReference.cs</file>
    <file>GenericDataDocumentFormatV21.cs</file>
    <file>GenericTimeSeriesDataFormatV21.cs</file>
    <file>IConstrainableStructureReference.cs</file>
    <file>QueryStructureRequestFormat.cs</file>
    <file>StructSpecificDataFormatV21.cs</file>
    <file>StructSpecificTimeSeriesDataFormatV21.cs</file>
  </compound>
  <compound kind="dir">
    <name>EstatSdmxSourceExtension/Properties</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/EstatSdmxSourceExtension/Properties/</path>
    <filename>dir_dfff4183285186617dc6e47537f70622.html</filename>
    <file>AssemblyInfo.cs</file>
  </compound>
  <compound kind="dir">
    <name>CustomRequests/Properties</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/CustomRequests/Properties/</path>
    <filename>dir_307ce8ee76b8a9f4faed5311c591cd27.html</filename>
    <file>AssemblyInfo.cs</file>
  </compound>
  <compound kind="dir">
    <name>CustomRequests/Builder/QueryBuilder</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/CustomRequests/Builder/QueryBuilder/</path>
    <filename>dir_74175a097ecf58c5e04b0b2d626e0f69.html</filename>
    <file>ComplexDataQueryCoreBuilderV21.cs</file>
    <file>ComplexStructureQueryBuilderV21.cs</file>
    <file>GenericDataQueryBuilderV21.cs</file>
    <file>GenericTimeSeriesDataQueryBuilderV21.cs</file>
    <file>IParameterBuilder.cs</file>
    <file>ParameterBuilderAnd.cs</file>
    <file>ParameterBuilderOr.cs</file>
    <file>StructSpecificDataQueryBuilderV21.cs</file>
    <file>StructSpecificTimeSeriesQueryBuilderV21.cs</file>
  </compound>
  <compound kind="dir">
    <name>EstatSdmxSourceExtension/Util</name>
    <path>F:/tmp/sources/estat_sdmxsource_extension.net/src/estat_sdmxsource_extension.net/EstatSdmxSourceExtension/Util/</path>
    <filename>dir_e13c2dc641bb70f4717fb0ce63d99761.html</filename>
    <file>SdmxMessageUtilExt.cs</file>
  </compound>
</tagfile>
