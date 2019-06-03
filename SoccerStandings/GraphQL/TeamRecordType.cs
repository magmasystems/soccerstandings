using GraphQL.Types;
using SoccerStandings.Models;

namespace SoccerStandings.GraphQL
{
    public class TeamRecordType : ObjectGraphType<TeamRecord>
    {
        public TeamRecordType()
        {
            this.Name = "TeamRecord";
            this.Description = "The win/loss record for a team in a season";

            Field(x => x.Id).Description("Id of team record");
            Field(x => x.Name, nullable: false).Description("Name of a team");

            Field(x => x.Points).Description("The number of points a team has");

            Field(x => x.Wins).Description("Number of wins a team has in a season");
            Field(x => x.Losses).Description("Number of losses a team has in a season");
            Field(x => x.Draws).Description("Number of draws a team has in a season");

            Field(x => x.GoalsFor).Description("Number of goals a team scored in a season");
            Field(x => x.GoalsAgainst).Description("Number of goals a team has gievm up in a season");
            Field(x => x.GoalDifference).Description("The goal differential of a team");
        }
    }
}
