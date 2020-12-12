using System;

namespace CandidatesExercise.BusinessLogic.Processing
{
    public class TopCandidateSearchOptions
    {
        /// <summary>
        /// The job title a candidate must have experience in
        /// The option must be provided, as otherwise it defaults to null
        /// </summary>
        public string JobTitle { get; }

        /// <summary>
        /// The minimum experience a candidate must have in the given job
        /// This option must be provided, as otherwise it defaults to zero
        /// </summary>
        public TimeSpan MinimumExperience { get; }

        /// <summary>
        /// The timestamp used as 'end date' for on-going employments
        /// This is used to calculate duration of those employments
        /// </summary>
        public DateTimeOffset OngoingEmploymentEndDate { get; }

        public TopCandidateSearchOptions(
            string jobTitle,
            TimeSpan minimumExperience,
            DateTimeOffset ongoingEmploymentEndDate)
        {
            if (string.IsNullOrEmpty(jobTitle))
                throw new ArgumentException("The job title cannot be null or empty", nameof(jobTitle));

            if (minimumExperience < TimeSpan.Zero)
                throw new ArgumentException("The minimum experience TimeSpan cannot be less than zero", nameof(minimumExperience));

            JobTitle = jobTitle;
            MinimumExperience = minimumExperience;
            OngoingEmploymentEndDate = ongoingEmploymentEndDate;
        }
    }
}
