using System;
using System.Collections.Generic;
using System.Linq;
using CandidatesExercise.BusinessLogic.Processing;
using CandidatesExercise.Data;
using CandidatesExercise.Models;

namespace CandidatesExercise.BusinessLogic
{
    public class CandidateService : ICandidateService
    {
        readonly ICandidateRepository _candidateRepository;
        readonly ITopCandidateFilterProvider _topCandidateFilterProvider;

        public CandidateService(
            ICandidateRepository candidateRepository, 
            ITopCandidateFilterProvider topCandidateFilterProvider)
        {
            if (candidateRepository == null)
                throw new ArgumentNullException(nameof(candidateRepository));

            if (topCandidateFilterProvider == null)
                throw new ArgumentNullException(nameof(topCandidateFilterProvider));

            _candidateRepository = candidateRepository;
            _topCandidateFilterProvider = topCandidateFilterProvider;
        }

        // Default implementation. Would not exist in a DI scenario
        public CandidateService(ICandidateRepository candidateRepository)
            : this(candidateRepository, new DefaultTopCandidateFilterProvider())
        { }

        public Candidate FindTopCandidate(TopCandidateSearchOptions searchOptions)
            // Simply return the first candidate from GetTopCandidates
            // Note if GetTopCandidates returned a list (materialized from db query for example),
            // this would be best implemented with its own query instead
            => GetTopCandidates(searchOptions).FirstOrDefault();

        /// <summary>
        /// Searches for best candidates for a job title, with a minimum amount of experience
        /// The returned candidates will be ordered based on the amount of experience they have (most to least)
        /// </summary>
        /// <param name="searchOptions">Options defining the search</param>
        /// <returns>A set of candidates, ordered by their experience at the given job</returns>
        public IEnumerable<Candidate> GetTopCandidates(TopCandidateSearchOptions searchOptions)
        {
            if (searchOptions == null)
                throw new ArgumentNullException(nameof(searchOptions));

            var filter = _topCandidateFilterProvider.GetTopCandidatesFilter(searchOptions);

            var candidates = _candidateRepository.Candidates;

            var filteredCandidates = filter.FilterCandidates(candidates);

            return filteredCandidates;
        }
    }
}
