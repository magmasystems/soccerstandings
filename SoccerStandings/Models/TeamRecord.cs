using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SoccerStandings.Models
{
    public class TeamRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }

        [NotMapped]
        public int GoalDifference { get { return this.GoalsFor = this.GoalsAgainst; } }

        [JsonIgnore]
        public int StandingsId { get; set; }

        [JsonIgnore]
        public virtual Standings Standings { get; set; }

        public TeamRecord()
        { }

        public TeamRecord(string name) : this()
        {
            this.Name = name;
        }

        internal void AddDraw(int score)
        {
            this.Draws++;
            this.Points++;
            this.GoalsFor += score;
            this.GoalsAgainst += score;
        }

        internal void AddWin(int score1, int score2)
        {
            this.Wins++;
            this.Points += 3;
            this.GoalsFor += score1;
            this.GoalsAgainst += score2;
        }

        internal void AddLoss(int score2, int score1)
        {
            this.Losses++;
            this.Points += 0;
            this.GoalsFor += score2;
            this.GoalsAgainst += score1;
        }
    }
}
