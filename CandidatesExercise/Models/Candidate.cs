using System.Collections.Generic;

namespace CandidatesExercise.Models
{
    public class Candidate
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Location { get; set; }

        public IEnumerable<EmploymentHistory> WorkHistory { get; set; }
    }
}
