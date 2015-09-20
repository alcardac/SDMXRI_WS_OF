// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestIdentifiableRefObjetcImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit class for <see cref="IdentifiableRefObjetcImpl" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System;
    using System.ComponentModel;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     Test unit class for <see cref="IdentifiableRefObjetcImpl" />
    /// </summary>
    [TestFixture]
    public class TestIdentifiableRefObjetcImpl
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Test method for <see cref="IdentifiableRefObjetcImpl" />
        /// </summary>
        [Test]
        public void Test()
        {
            foreach (SdmxStructureType sdmxStructureType in SdmxStructureType.Values)
            {
                if (sdmxStructureType.IsIdentifiable)
                {
                    if (sdmxStructureType.ParentStructureType != null
                        && sdmxStructureType.ParentStructureType.IsMaintainable)
                    {
                        Uri urn = sdmxStructureType.ParentStructureType.GenerateUrn("AGENCY", "ID", "2.2");
                        SdmxStructureType targetObj = sdmxStructureType;
                        
                        var result = new IdentifiableRefObjetcImpl(new StructureReferenceImpl(urn), GetID(targetObj), targetObj);
                        Assert.AreEqual(result.StructureEnumType.EnumType, targetObj.EnumType);
                        Assert.AreEqual(GetID(targetObj), result.Id);
                        Assert.IsNull(result.ChildReference);
                        Assert.IsNull(result.ParentIdentifiableReference);
                        Assert.AreEqual(
                            sdmxStructureType.ParentStructureType, 
                            result.ParentMaintainableReferece.MaintainableStructureEnumType);
                        if (!targetObj.HasFixedId)
                        {
                            var ids = new[] { "1", "2", "A", "B" };
                            result = new IdentifiableRefObjetcImpl(new StructureReferenceImpl(urn), ids, targetObj);
                            Assert.AreEqual(result.StructureEnumType.EnumType, targetObj.EnumType);
                            Assert.AreEqual("1", result.Id);
                            Assert.AreEqual("2", result.ChildReference.Id);
                            Assert.IsNotNull(result.ChildReference);
                            Assert.IsNotNull(result.ChildReference.ParentIdentifiableReference);
                            Assert.IsNull(result.ParentIdentifiableReference);
                            Assert.AreEqual(sdmxStructureType.ParentStructureType, result.ParentMaintainableReferece.MaintainableStructureEnumType);
                            IIdentifiableRefObject child = result;
                            for (int i = 0; i < ids.Length; i++)
                            {
                                string id = ids[i];
                                Assert.AreEqual(id, child.Id);
                                child = child.ChildReference;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <param name="targetObj">
        /// The target object.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetID(SdmxStructureType targetObj)
        {
            return targetObj.HasFixedId ? targetObj.FixedId : "TEST";
        }

        #endregion
    }
}