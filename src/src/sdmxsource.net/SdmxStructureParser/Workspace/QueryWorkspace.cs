// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryWorkspace.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query workspace implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Workspace
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    ///     The query workspace implementation
    /// </summary>
    public class QueryWorkspace : IQueryWorkspace
    {
        #region Fields

        /// <summary>
        ///     The provision references.
        /// </summary>
        private readonly IStructureReference _provisionReferences;

        /// <summary>
        ///     The registration references.
        /// </summary>
        private readonly IStructureReference _registrationReferences;

        /// <summary>
        ///     The resolve references.
        /// </summary>
        private readonly bool _resolveReferences;

        /// <summary>
        ///     The simple structure queries.
        /// </summary>
        private readonly IList<IStructureReference> _simpleStructureQueries;

        /// <summary>
        ///     The complex structure query.
        /// </summary>
        private IComplexStructureQuery _complexStructureQuery;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryWorkspace"/> class.
        /// </summary>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="resolveReferences0">
        /// The resolve references 0.
        /// </param>
        public QueryWorkspace(IStructureReference structureReference, bool resolveReferences0)
        {
            this._simpleStructureQueries = new List<IStructureReference> { structureReference };
            this._resolveReferences = resolveReferences0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryWorkspace"/> class.
        /// </summary>
        /// <param name="complexStructureQuery">
        /// The complex structure reference.
        /// </param>
        public QueryWorkspace(IComplexStructureQuery complexStructureQuery)
        {
            this._complexStructureQuery = complexStructureQuery;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryWorkspace"/> class.
        /// </summary>
        /// <param name="provisionReferences0">
        /// The provision references 0.
        /// </param>
        /// <param name="registrationReferences1">
        /// The registration references 1.
        /// </param>
        /// <param name="structureReferences">
        /// The structure references.
        /// </param>
        /// <param name="resolveReferences2">
        /// The resolve references 2.
        /// </param>
        public QueryWorkspace(
            IStructureReference provisionReferences0,
            IStructureReference registrationReferences1,
            IList<IStructureReference> structureReferences,
            bool resolveReferences2)
        {
            this._provisionReferences = provisionReferences0;
            this._registrationReferences = registrationReferences1;
            this._simpleStructureQueries = structureReferences;
            this._resolveReferences = resolveReferences2;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the provision references.
        /// </summary>
        public virtual IStructureReference ProvisionReferences
        {
            get
            {
                return this._provisionReferences;
            }
        }

        public IComplexStructureQuery ComplexStructureQuery
        {
            get
            {
                return this._complexStructureQuery;
            }
        }

        /// <summary>
        ///     Gets the registration references.
        /// </summary>
        public virtual IStructureReference RegistrationReferences
        {
            get
            {
                return this._registrationReferences;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether resolve references.
        /// </summary>
        public virtual bool ResolveReferences
        {
            get
            {
                return this._resolveReferences;
            }
        }

        /// <summary>
        ///     Gets the simple structure queries.
        /// </summary>
        public virtual IList<IStructureReference> SimpleStructureQueries
        {
            get
            {
                return this._simpleStructureQueries;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The has provision queries.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public virtual bool HasProvisionQueries()
        {
            return this._provisionReferences != null;
        }

        /// <summary>
        ///     The has registration queries.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public virtual bool HasRegistrationQueries()
        {
            return this._registrationReferences != null;
        }

        /// <summary>
        ///     The has structure queries.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public virtual bool HasStructureQueries()
        {
            return this._simpleStructureQueries != null && this._simpleStructureQueries.Count > 0;
        }

        #endregion
    }
}