namespace CandidatesExercise.BusinessLogic.Processing
{
    public interface ITopCandidateFilterProvider
    {
        ICandidateFilter GetTopCandidatesFilter(TopCandidateSearchOptions searchOptions);
    }
}
