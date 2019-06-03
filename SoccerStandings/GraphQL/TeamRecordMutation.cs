using GraphQL.Types;
using SoccerStandings.Models;
using SoccerStandings.Services;

namespace SoccerStandings.GraphQL
{
    internal class TeamRecordMutation : ObjectGraphType
    {
        private IStandingsService Service { get; set; }

        public TeamRecordMutation(IStandingsService service)
        {
            this.Service = service;

            Field<TeamType>(
                name: "createTeam",
                description: "Adds a new team to the league",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TeamInputType>> { Name = "team" }
                ),
                resolve: context =>
                {
                    var team = context.GetArgument<Team>("team");
                    return this.Service.AddTeam(team);
                }
            );
        }
    }
}
