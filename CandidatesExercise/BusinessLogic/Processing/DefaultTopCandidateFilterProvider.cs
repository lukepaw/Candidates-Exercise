using System;

namespace CandidatesExercise.BusinessLogic.Processing
{
    class DefaultTopCandidateFilterProvider : ITopCandidateFilterProvider
    {
        public ICandidateFilter GetTopCandidatesFilter(TopCandidateSearchOptions searchOptions)
        {
            if (searchOptions == null)
                throw new ArgumentNullException(nameof(searchOptions));

            return new AggregateCandidateFilter(
                new JobTitleCandidateFilter(searchOptions.JobTitle, StringComparison.OrdinalIgnoreCase),
                new MinimumExperienceCandidateFilter(searchOptions.MinimumExperience, searchOptions.OngoingEmploymentEndDate));
        }
    }
}
