using System.Data.SqlClient;
using GraphQL;
using Newtonsoft.Json.Linq;

namespace SoccerStandings.GraphQL
{
    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public SortOrder Order { get; set; }
        public JObject Variables { get; set; }
    }
}
