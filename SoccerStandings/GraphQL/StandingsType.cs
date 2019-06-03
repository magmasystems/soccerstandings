using GraphQL.Types;
using SoccerStandings.Models;

namespace SoccerStandings.GraphQL
{
    public class StandingsType : ObjectGraphType<Standings>
    {
        public StandingsType()
        {
            this.Name = "Standings";
            this.Description = "The table of a certain league for a certain year";

            Field(x => x.Id).Description("Id of the standings");
            Field(x => x.LeagueName).Description("Name of the league");
            Field<ListGraphType<TeamRecordType>>("teamrecords", "The season record of each team in a league for a year");
        }
    }
}
