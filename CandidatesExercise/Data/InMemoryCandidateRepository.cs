using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using CandidatesExercise.Models;

namespace CandidatesExercise.Data
{
    public sealed class InMemoryCandidateRepository : ICandidateRepository
    {
        readonly IEnumerable<Candidate> _candidates;

        public InMemoryCandidateRepository(IEnumerable<Candidate> candidates)
        {
            if (candidates == null)
                throw new ArgumentNullException(nameof(candidates));

            _candidates = candidates;
        }

        public IEnumerable<Candidate> Candidates => _candidates;
    }
}
