using System;
using System.Collections.Generic;
using CandidatesExercise.Models;

namespace CandidatesExercise.Data
{
    public interface ICandidateRepository
    {
        /// <summary>
        /// Returns the enumerable of all available candidates
        /// </summary>
        IEnumerable<Candidate> Candidates { get; }
    }
}
