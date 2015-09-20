using System;
using System.Collections.Generic;
using System.Text;
using Org.Sdmxsource.Sdmx.Api.Model.Query;

namespace Estat.Nsi.DataRetriever.Builders
{
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    class SqlQuery : IDataQueryFormat<SqlQuery>
    {
        private StringBuilder _sql = new StringBuilder(string.Empty);

        public string getSql()
        {
            return _sql.ToString();
        }

        public void appendSql(string sql)
        {
            _sql.Append(sql);
        }

    }
}

