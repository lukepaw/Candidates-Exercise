using System.Collections.Generic;
using CandidatesExercise.BusinessLogic.Processing;
using CandidatesExercise.Models;

namespace CandidatesExercise.BusinessLogic
{
    public interface ICandidateService
    {
        /// <summary>
        /// Searches for the best candidate for the given job title, with a minimum amount of experience
        /// The best candidate is the one with most experience
        /// </summary>
        /// <param name="searchOptions">Options defining the search</param>
        /// <returns>The best candidate, or null if no candidates have the minimum amount of experience</returns>
        Candidate FindTopCandidate(TopCandidateSearchOptions searchOptions);
    }
}
