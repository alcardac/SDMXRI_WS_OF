// -----------------------------------------------------------------------
// <copyright file="TestComplexDataQueryExtension.cs" company="EUROSTAT">
//   Date Created : 2014-10-31
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace CustomRequests.Tests
{
    using Estat.Sri.CustomRequests.Extension;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query.Complex;

    [TestFixture]
    public class TestComplexDataQueryExtension
    {
        [Test]
        public void TestShouldUseAnd()
        {
            var freqCriteria = new ComplexDataQuerySelectionImpl("FREQ", new IComplexComponentValue [] { new ComplexComponentValueImpl("M", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("A", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("B", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension) });
            Assert.IsTrue(freqCriteria.ShouldUseAnd());
        }
        [Test]
        public void TestShouldNotUseAnd()
        {
            var freqCriteria = new ComplexDataQuerySelectionImpl("FREQ", new IComplexComponentValue [] { new ComplexComponentValueImpl("M", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.NotEqual), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("A", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("B", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension) });
            Assert.IsFalse(freqCriteria.ShouldUseAnd());
        }

        [Test]
        public void TestShouldNotUseAnd2()
        {
            var freqCriteria = new ComplexDataQuerySelectionImpl("FREQ", new IComplexComponentValue [] { new ComplexComponentValueImpl("M", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("A", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension), new ComplexComponentValueImpl("B", OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal), SdmxStructureEnumType.Dimension) });
            Assert.IsFalse(freqCriteria.ShouldUseAnd());
        }
    }
}