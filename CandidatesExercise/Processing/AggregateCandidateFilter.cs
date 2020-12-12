using System;
using System.Collections.Generic;
using System.Linq;
using CandidatesExercise.Models;

namespace CandidatesExercise.BusinessLogic
{
    class AggregateCandidateFilter : ICandidateFilter
    {
        readonly IEnumerable<ICandidateFilter> _filters;

        public AggregateCandidateFilter(IEnumerable<ICandidateFilter> filters)
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            _filters = filters;
        }

        public AggregateCandidateFilter(params ICandidateFilter[] filters)
            : this((IEnumerable<ICandidateFilter>) filters)
        { }

        public IEnumerable<Candidate> FilterCandidates(IEnumerable<Candidate> candidates)
            => _filters.Aggregate(candidates, 
                    (filteredCandidates, filter) => filter.FilterCandidates(filteredCandidates));
    }
}
