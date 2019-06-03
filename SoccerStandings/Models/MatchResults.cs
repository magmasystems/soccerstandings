using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SoccerStandings.Models
{
    /*
     * This is the raw data that is read in from the JSON file. No attempt is name to normalize the data.
     */ 

    [DataContract]
    public class MatchResults
    {
        [DataMember(Name = "id")]
        [IgnoreDataMember]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string LeagueName { get; set; }

        [DataMember(Name = "rounds")]
        public IEnumerable<Round> Rounds { get; set; }
    }

    [DataContract]
    public class Round
    {
        [DataMember(Name = "id")]
        [IgnoreDataMember]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string RoundName { get; set; }

        [DataMember(Name = "matches")]
        public virtual IEnumerable<Match> Matches { get; set; }

        [JsonIgnore]
        public int MatchResultsId { get; set; }   // reference back to the containing League/Year results

        [JsonIgnore]
        public virtual MatchResults MatchResults { get; set; }
    }

    [DataContract]
    public class Match
    {
        [DataMember(Name = "id")]
        [JsonIgnore]
        public int Id { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

        [DataMember(Name = "score1")]
        public int Score1 { get; set; }

        [DataMember(Name = "score2")]
        public int Score2 { get; set; }

        [DataMember(Name = "team1")]
        public Team Team1 { get; set; }

        [DataMember(Name = "team2")]
        public Team Team2 { get; set; }

        [JsonIgnore]
        public int RoundId { get; set; }   // reference back to the containing round

        [JsonIgnore]
        public virtual Round Round { get; set; }
    }

    [DataContract]
    public class Team
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}
