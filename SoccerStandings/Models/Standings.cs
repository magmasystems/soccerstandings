using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace SoccerStandings.Models
{
    [DataContract]
    public class Standings
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string LeagueName { get; set; }

        [DataMember]
        public IEnumerable<TeamRecord> TeamRecords { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        internal Dictionary<string, TeamRecord> _TeamRecords { get; set;  } = new Dictionary<string, TeamRecord>();
    }
}
