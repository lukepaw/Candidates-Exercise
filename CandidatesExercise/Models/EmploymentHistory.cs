using System;

namespace CandidatesExercise.Models
{
    public class EmploymentHistory
    {
        public string JobTitle { get; set; }

        public string Company { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }
    }
}
