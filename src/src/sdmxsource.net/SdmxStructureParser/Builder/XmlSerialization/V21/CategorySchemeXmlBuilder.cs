// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category scheme xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The category scheme xml bean builder.
    /// </summary>
    public class CategorySchemeXmlBuilder : ItemSchemeAssembler, IBuilder<CategorySchemeType, ICategorySchemeObject>
    {
        #region Fields

        /// <summary>
        ///     The category bean assembler bean.
        /// </summary>
        private readonly CategoryAssembler _categoryAssemblerBean = new CategoryAssembler();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CategorySchemeType"/>.
        /// </returns>
        public virtual CategorySchemeType Build(ICategorySchemeObject buildFrom)
        {
            // Create outgoing build
            var builtObj = new CategorySchemeType();

            // Populate it from inherited super
            this.AssembleItemScheme(builtObj, buildFrom);

            // Populate it using this class's specifics
            foreach (ICategoryObject eachCat in buildFrom.Items)
            {
                var newCat = new Category();
                builtObj.Item.Add(newCat);
                this._categoryAssemblerBean.Assemble(newCat.Content, eachCat);
                this.AssembleChildCategories(newCat.Content, eachCat);
            }

            return builtObj;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recursively pickup and assemble child Categories of Categories
        /// </summary>
        /// <param name="catType">
        /// parent destination Category xml bean
        /// </param>
        /// <param name="catBean">
        /// parent source Category bean
        /// </param>
        private void AssembleChildCategories(CategoryType catType, ICategoryObject catBean)
        {
            var stack = new Stack<KeyValuePair<CategoryType, ICategoryObject>>();
            stack.Push(new KeyValuePair<CategoryType, ICategoryObject>(catType, catBean));

            while (stack.Count > 0)
            {
                 var current = stack.Pop();
                catType = current.Key;
                catBean = current.Value;
                foreach (ICategoryObject eachCat in catBean.Items)
                {
                    var newCat = new Category();
                    catType.Item.Add(newCat);
                    this._categoryAssemblerBean.Assemble(newCat.Content, eachCat);
                    stack.Push(new KeyValuePair<CategoryType, ICategoryObject>(newCat.Content, eachCat));

                    ////this.AssembleChildCategories(newCat, eachCat);
                }
            }
        }

        #endregion
    }
}