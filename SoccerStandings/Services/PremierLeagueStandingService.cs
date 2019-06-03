using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SoccerStandings.Models;
using SoccerStandings.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace SoccerStandings.Services
{
    public interface IStandingsService : IDisposable
    {
        MatchResults Matches { get; }

        void SaveStandingsToDatabase(Standings standings);

        TeamRecord GetTeamRecord(string name);
        TeamRecord GetLeader();
        Standings GetStandings(int id, SortOrder order = SortOrder.Unspecified);

        Team AddTeam(Team team);
    }

    public class PremierLeagueStandingService : IStandingsService
    {
        public MatchResults Matches { get; private set; }
        private bool SaveTheData { get; }
        private StandingsDbContext TheDbContext { get; set; }

        public PremierLeagueStandingService(IConfiguration configuration)
        {
            if (configuration != null)
            {
                this.SaveTheData = configuration.GetValue<bool>("soccerService:saveData", false);
            }

			this.TheDbContext = new StandingsDbContext();

            this.Matches = ReadMatches();

            if (this.SaveTheData)
            {
                this.SaveMatchesToDatabase();
            }
        }

        public void Dispose()
		{
			this.TheDbContext?.Dispose();
		}

        private MatchResults ReadMatches()
        {
            const string filename = "Premier League Match Results.json";
            var textMatches = File.ReadAllText(filename);

            var results = JsonConvert.DeserializeObject<MatchResults>(textMatches);
            return results;
        }

        public MatchResults LoadMatchResults()
		{
			try
			{
				this.TheDbContext.Database.EnsureCreated();

				var matchResults =
                    TheDbContext.MatchResults
	                .Include(x => x.Rounds)
	                    .ThenInclude(r => r.Matches)
		                    .ThenInclude(m => m.Team1)
	                .Include(x => x.Rounds)
	                    .ThenInclude(r => r.Matches)
		                    .ThenInclude(m => m.Team2)
	                .Select(x => x)
	                .ToList();

				return matchResults.FirstOrDefault();
			}
            catch (Exception exc)
			{
				throw exc;
			}
		}

		public void ClearMatchResultsTable()
		{
			try
			{
				var results = this.LoadMatchResults();
				this.TheDbContext.RemoveRange(results);
				this.TheDbContext.SaveChanges();
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
		}

		public Standings LoadStandings()
		{
			try
			{
				using (var dbContext = new StandingsDbContext())
				{
					dbContext.Database.EnsureCreated();

					var standings =
						dbContext.Standings
						.Include(x => x.TeamRecords)
						.Select(x => x)
						.ToList();
					return standings.FirstOrDefault();
				}
			}
			catch (Exception exc)
			{
                throw exc;
			}
		}

		public void ClearStandingsTable()
		{
			try
			{
				using (var dbContext = new StandingsDbContext())
				{
					var oldStandings = this.LoadStandings();
					dbContext.RemoveRange(oldStandings);
					dbContext.SaveChanges();
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
		}

		private void SaveMatchesToDatabase()
        {
            try
            {
                using (var dbContext = new StandingsDbContext())
                {
					var matchResults = this.LoadMatchResults();
                    dbContext.RemoveRange(matchResults);

                    dbContext.MatchResults.Add(this.Matches);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void SaveStandingsToDatabase(Standings standings)
        {
            if (!this.SaveTheData)
            {
                return;
            }

            try
            {
                using (var dbContext = new StandingsDbContext())
                {
					var oldStandings = this.LoadStandings();
                    dbContext.RemoveRange(oldStandings);

                    dbContext.Standings.Add(standings);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        // For GraphQL

        public Standings GetStandings(int id, SortOrder order = SortOrder.Unspecified)
        {
            //return this.GetStandings2(id, order);

            using (var dbContext = new StandingsDbContext())
            {
                var query = dbContext.Standings
                                .Include(st => st.TeamRecords)
                                .Where(st => st.Id == id)
                                .Select(st => st)
                                ;
                            
                var result = query.FirstOrDefault();
                if (result != null && order != SortOrder.Unspecified)
                {
                    result.TeamRecords = (order == SortOrder.Ascending)
                        ? result.TeamRecords.OrderBy(x => x.Points).ToList()
                        : result.TeamRecords.OrderByDescending(x => x.Points).ToList();
                }

                return result;
            }
        }

        private Standings GetStandings2(int id, SortOrder order = SortOrder.Unspecified)
        {
            using (var dbContext = new StandingsDbContext())
            {
                var query = dbContext.Standings
                            .Include(st => st.TeamRecords)
                            .Where(st => st.Id == id)
                            .Select(st => new
                            {
                                Parent = st,
                                Children = st.TeamRecords.OrderBy(tr => tr.Points)
                                //(order == SortOrder.Ascending) ? st.TeamRecords.OrderBy(tr => tr.Points) : (order == SortOrder.Descending) ? st.TeamRecords.OrderByDescending(tr => tr.Points) : st.TeamRecords
                            }).ToList();

                var result = query.Select(st => st.Parent).FirstOrDefault();
                return result;
            }
        }

        public TeamRecord GetTeamRecord(string name)
        {
            using (var dbContext = new StandingsDbContext())
            {
                var query = from r in dbContext.TeamRecord
                            where r.Name == name
                            select r;

                return query.FirstOrDefault();
            }
        }

        public TeamRecord GetLeader()
        {
            using (var dbContext = new StandingsDbContext())
            {
                var query = from r in dbContext.TeamRecord
                            where r.Points == (dbContext.TeamRecord.Max(x => x.Points))
                            select r;
                return query.FirstOrDefault();
            }
        }

        public Team AddTeam(Team team)
        {
            if (team == null)
                return null;

            return team;
        }
    }
}
