using GraphQL.Types;
using SoccerStandings.Models;

namespace SoccerStandings.GraphQL
{
    public class TeamType : ObjectGraphType<Team>
    {
        public TeamType()
        {
            this.Name = "Team";
            this.Description = "A team in a league";

            Field(x => x.Code, nullable: false).Description("The unique 3-letter code of a team");
            Field(x => x.Key, nullable: false).Description("Key of a team");
            Field(x => x.Name, nullable: false).Description("Name of a team");
        }
    }
}
