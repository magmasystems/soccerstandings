using GraphQL.Types;
using SoccerStandings.Models;

namespace SoccerStandings.GraphQL
{
    public class TeamInputType : InputObjectGraphType
    {
        public TeamInputType()
        {
            this.Name = "TeamInput";
            this.Description = "A team in a league";

            Field<NonNullGraphType<StringGraphType>>("code", "The unique 3-letter code of a team");
            Field<NonNullGraphType<StringGraphType>>("key", "Key of a team");
            Field<NonNullGraphType<StringGraphType>>("name", "Name of a team");
        }
    }
}
