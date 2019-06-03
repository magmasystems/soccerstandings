using System.Data.SqlClient;
using GraphQL.Types;

namespace SoccerStandings.GraphQL
{
    public class SortOrderType : EnumerationGraphType<SortOrder>
    {
        public SortOrderType()
        {
            Name = "SortOrderType";
        }
    }
}