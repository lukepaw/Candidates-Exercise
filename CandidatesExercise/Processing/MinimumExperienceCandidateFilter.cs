using System;
using System.Collections.Generic;
using System.Linq;
using CandidatesExercise.Models;

namespace CandidatesExercise.BusinessLogic
{
    /// <summary>
    /// Represents a filter that filters set of candidates to only include ones with minimum duration of work experience
    /// The filter returns candidates in order of most to least experience
    /// </summary>
    class MinimumExperienceCandidateFilter : ICandidateFilter
    {
        readonly TimeSpan _minimumExperience;
        readonly DateTimeOffset _ongoingEmploymentEndDate;

        public MinimumExperienceCandidateFilter(TimeSpan minimumExperience, DateTimeOffset ongoingEmploymentEndDate)
        {
            _minimumExperience = minimumExperience;
            _ongoingEmploymentEndDate = ongoingEmploymentEndDate;
        }

        public IEnumerable<Candidate> FilterCandidates(IEnumerable<Candidate> candidates)
        {
            // Select candidates and their experience for the job
            var candidatesWithExperience =
                candidates
                    .Select(c => new { Candidate = c, ExperienceDuration = CalculateJobExperienceDuration(c) });

            // Filter to only return ones with the minimum experience
            var matchingCandidates =
                candidatesWithExperience
                    .Where(c => c.ExperienceDuration >= _minimumExperience)
                    .OrderByDescending(c => c.ExperienceDuration)
                    .Select(c => c.Candidate);

            return matchingCandidates;
        }

        TimeSpan CalculateJobExperienceDuration(Candidate candidate)
        {
            // In case the returned data has a null collection (should not happen), we treat it as if it is empty
            if (candidate.WorkHistory == null)
                return TimeSpan.Zero;

            var accumulatedExperience =
                candidate.WorkHistory
                    // Select employment durations. In case there is no end date, we use the provided 'now' argument
                    .Select(wh => (wh.EndDate ?? _ongoingEmploymentEndDate) - wh.StartDate)
                    // Sum calculated time spans. .Sum won't work, as it has no overload for TimeSpan
                    .Aggregate(TimeSpan.Zero, (current, next) => current + next);

            return accumulatedExperience;
        }
    }
}
