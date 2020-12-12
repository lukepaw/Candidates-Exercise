using System.Collections.Generic;
using CandidatesExercise.Models;

namespace CandidatesExercise.BusinessLogic
{
    /// <summary>
    /// Represents a filter that can be applied to a set of candidates
    /// </summary>
    public interface ICandidateFilter
    {
        /// <summary>
        /// Applies this filter to a set of candidates
        /// </summary>
        /// <param name="candidates">The set of candidates</param>
        /// <returns>Filter set of candidates</returns>
        IEnumerable<Candidate> FilterCandidates(IEnumerable<Candidate> candidates);
    }
}
