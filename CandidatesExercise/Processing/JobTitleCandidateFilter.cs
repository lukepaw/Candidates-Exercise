using System;
using System.Collections.Generic;
using System.Linq;
using CandidatesExercise.Models;

namespace CandidatesExercise.BusinessLogic
{
    /// <summary>
    /// Represents a filter that filters set of candidates to only includes ones with work experience for the given job title
    /// </summary>
    class JobTitleCandidateFilter : ICandidateFilter
    {
        readonly string _jobTitle;
        readonly StringComparison _jobTitleComparison;

        public JobTitleCandidateFilter(string jobTitle, StringComparison jobTitleComparison)
        {
            if (jobTitle == null)
                throw new ArgumentNullException(nameof(jobTitle));

            _jobTitle = jobTitle;
            _jobTitleComparison = jobTitleComparison;
        }

        public IEnumerable<Candidate> FilterCandidates(IEnumerable<Candidate> candidates)
        {
            return candidates.Select(c => new Candidate
            {
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Location = c.Location,
                WorkHistory = c.WorkHistory?.Where(c => c.JobTitle.Equals(_jobTitle, _jobTitleComparison))
            });
        }
    }
}
