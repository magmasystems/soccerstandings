using SoccerStandings.Services;
using System.Collections.Generic;
using System.Linq;

namespace SoccerStandings.Models
{
    public class StandingsModel
    {
        public IStandingsService StandingsService { get; set; }
        public Standings Standings { get; set; }
        public string LeagueAndYear => this.StandingsService?.Matches.LeagueName;
        public IEnumerable<TeamRecord> StandingsByPoints => this.Standings.TeamRecords.OrderByDescending(team => team.Points);

        public StandingsModel(IStandingsService service)
        {
            this.StandingsService = service;
        }

        public void AddMatch(Match match)
        {
            if (!this.Standings._TeamRecords.TryGetValue(match.Team1.Name, out var team1))
            {
                this.Standings._TeamRecords.Add(match.Team1.Name, new TeamRecord(match.Team1.Name));
            }
            if (!this.Standings._TeamRecords.TryGetValue(match.Team2.Name, out var team2))
            {
                this.Standings._TeamRecords.Add(match.Team2.Name, new TeamRecord(match.Team2.Name));
            }

            team1 = this.Standings._TeamRecords[match.Team1.Name];
            team2 = this.Standings._TeamRecords[match.Team2.Name];

            if (match.Score1 == match.Score2)
            {
                team1.AddDraw(match.Score1);
                team2.AddDraw(match.Score1);
            }
            else if (match.Score1 > match.Score2)
            {
                team1.AddWin(match.Score1, match.Score2);
                team2.AddLoss(match.Score2, match.Score1);
            }
            else
            {
                team1.AddLoss(match.Score1, match.Score2);
                team2.AddWin(match.Score2, match.Score1);
            }
        }

        public Standings CalculateStandings()
        {
            this.Standings = this.CalculateStandings(this.StandingsService.Matches);
            return this.Standings;
        }

        private Standings CalculateStandings(MatchResults matches)
        {
            this.Standings = new Standings { LeagueName = matches.LeagueName };

            foreach (var match in from round in matches.Rounds
                                  from match in round.Matches
                                  select match)
            {
                this.AddMatch(match);
            }

            this.Standings.TeamRecords = this.Standings._TeamRecords.Values;
            this.StandingsService.SaveStandingsToDatabase(this.Standings);

            return this.Standings;
        }
    }

}
