### Requirements
.NET Core 3.1

### Summary 
I have not included any external packages in the implementation. 
Normally I would use a DI package, but thought it was an overkill.

To verify the requirement, please run the tests from the `CandidatesExercise.Tests` project. Two tests operate on the provided json data, and two on mock data.
The json data tests verify the `CandidateService.GetTopCandidates` method, which is the implementation responsible for calling filters on candidate data.

The actual interface method that fullfills the requirement is `ICandidateService.FindTopCandidate`, which simply calls the other method and selects one candidate.
Because the single-result method would return the same candidate for both tests, I opted to test it with mocked data instead.

### Details
`ICandidateRepository`  
Provides candidates data as an `IEnumerable<Candidate>`.  
The only implementation is `InMemoryCandidateRepository`, which in tests is initialized from deserialized json data and hard-coded mock data.

`ICandidateFilter`  
Filters a set of candidates, then returns the filtered set.  
The implmentations are:
`JobTitleCandidateFilter`, which filters candidates by having any experience with the given job title,
`MinimumExperienceCandidateFilter`, which returns candidates with minimum experience, ordered from most to least,
`AggregateCandidateFilter`, which allows applying multiple filters in conjuction.

`ITopCandidateFilterProvider`  
Provides a `ICandidateFilter` that performs the 'match by job title with minimum experience' filtering.  
The filter is created using the other `ICandidateFilter` implementations (i.e. aggregate of job title and minimum experience).

`ICandidateService`  
Provides the method that fullfils the requirement of retriving the best match candidate

The dependency graph is:
```
ICandidateService 
   -> ICandidateRepository
   -> ITopCandidateFilterProvider -> ICandidateFilter
```

Regards,
Lukasz
