using System.Data.SqlClient;
using GraphQL.Types;
using SoccerStandings.Models;
using SoccerStandings.Services;

namespace SoccerStandings.GraphQL
{
    internal class TeamRecordQuery : ObjectGraphType<TeamRecord>
    {
        private IStandingsService Service { get; set; }

        public TeamRecordQuery(IStandingsService service)
        {
            this.Service = service;

            Field<TeamRecordType>(
                name: "teamrecord",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "name" }),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    return this.Service.GetTeamRecord(name);
                }
            );

            Field<TeamRecordType>(
                name: "leader",
                arguments: null,
                resolve: context =>
                {
                    return this.Service.GetLeader();
                }
            );

            Field<StandingsType>(
                name: "standings",
                description: "Gets the standings for a certain StandingsId",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id" },
                    new QueryArgument<SortOrderType> { Name = "order", DefaultValue = SortOrder.Unspecified }),              
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var order = context.GetArgument("order", SortOrder.Unspecified);
                    return this.Service.GetStandings(id, order);
                }
            );
        }
    }
}
