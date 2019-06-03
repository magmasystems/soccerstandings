using GraphQL.Types;
using SoccerStandings.Models;
using SoccerStandings.Services;

namespace SoccerStandings.GraphQL
{
    internal class StandingsQuery : ObjectGraphType<Standings>
    {
        private IStandingsService Service { get; set; }

        public StandingsQuery(IStandingsService service)
        {
            this.Service = service;

            Field<StandingsQuery>(
                name: "standings",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return this.Service.GetStandings(id);
                }
            );
        }
    }
}
