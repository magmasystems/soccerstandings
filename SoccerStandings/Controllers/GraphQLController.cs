using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using SoccerStandings.GraphQL;
using SoccerStandings.Services;

namespace SoccerStandings.Controllers
{
    [Route(Startup.GraphQLPath)]
    public class GraphQLController : Controller
    {
        private IStandingsService Service { get; set; }

        public GraphQLController(IStandingsService service)
        {
            this.Service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var schema = new Schema
            {
                Query = new TeamRecordQuery(this.Service),
                Mutation = new TeamRecordMutation(this.Service)
            };

            var result = await new DocumentExecuter().ExecuteAsync(x =>
            {
                x.Schema = schema;
                x.Query = query.Query;
                x.Inputs = query.Variables.ToInputs();
                x.ExposeExceptions = true;
                x.EnableMetrics = true;
            });

            if (result.Errors?.Count > 0)
            {
                Console.WriteLine($"================ QUERY EXCEPTION: {result.Errors[0].Message} ===================");
                return BadRequest(result.Errors[0]);
            }

            return Ok(result);
        }
    }
}
