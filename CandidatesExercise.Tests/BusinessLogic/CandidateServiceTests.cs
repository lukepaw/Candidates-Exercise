using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NUnit.Framework;
using CandidatesExercise.Data;
using CandidatesExercise.BusinessLogic;
using CandidatesExercise.BusinessLogic.Processing;
using CandidatesExercise.Models;

namespace CandidatesExercise.Tests.BusinessLogic
{
    public class CandidateServiceTests
    {
        ICandidateRepository _jsonFileCandidatesRepository;

        [SetUp]
        public void Setup()
        {
            var candidatesJson = ReadEmbeddedResourceString("candidates.json");
           
            var candidatesList = JsonSerializer.Deserialize<List<Candidate>>(candidatesJson);

            _jsonFileCandidatesRepository = new InMemoryCandidateRepository(candidatesList);
          
            static string ReadEmbeddedResourceString(string fileName)
            {
                var thisAssembly = typeof(CandidateServiceTests).Assembly;

                var embeddedResourceName = $"{thisAssembly.GetName().Name}.{fileName}";

                using var candidatesJsonStream = thisAssembly.GetManifestResourceStream(embeddedResourceName);
                
                using var streamReader = new StreamReader(candidatesJsonStream);

                return streamReader.ReadToEnd();
            }
        }

        static readonly string _SoftwareEngineerJobTitle = "Software Engineer";

        // The timestamp at the time I have written this file
        static readonly DateTimeOffset _NowAtCodeWriteTime = new DateTimeOffset(2020, 12, 12, 0, 0, 0, 0, TimeSpan.Zero);

        [Test]
        public void GetTopCandidates_With_5Years_MinimumExperience_ReturnsAll_ForJsonCandidates()
        {
            Test_GetTopCandidates_WithMinimumExperience(_jsonFileCandidatesRepository,
                _SoftwareEngineerJobTitle,
                TimeSpan.FromDays(5 * _DaysInYear),
                _NowAtCodeWriteTime,
                new[] { "Fleur Michael", "Mike Spencer", "Dulcie Patton" });
        }

        [Test]
        public void GetTopCandidates_With_7Years_MinimumExperience_ReturnsTwo_ForJsonCandidates()
        {
            Test_GetTopCandidates_WithMinimumExperience(_jsonFileCandidatesRepository,
                _SoftwareEngineerJobTitle,
                TimeSpan.FromDays(7 * _DaysInYear),
                _NowAtCodeWriteTime,
                new[] { "Fleur Michael", "Mike Spencer" });
        }

        void Test_GetTopCandidates_WithMinimumExperience(
            ICandidateRepository candidatesRepository,
            string jobTitle,
            TimeSpan minimumExperience,
            DateTimeOffset ongoingEmploymentEndDate,
            string[] expectedCandidateNamesOrdered)
        {
            var candidateService = new CandidateService(candidatesRepository);

            var searchOptions = new TopCandidateSearchOptions(jobTitle, minimumExperience, ongoingEmploymentEndDate);

            var topCandidates = candidateService.GetTopCandidates(searchOptions)?.ToArray();

            Assert.NotNull(topCandidates);

            Assert.AreEqual(
                topCandidates.Length, expectedCandidateNamesOrdered.Length,
                $"Expected candidates mismatch: expected {expectedCandidateNamesOrdered.Length} candidates, but got {topCandidates.Length}");

            for (var i = 0; i < expectedCandidateNamesOrdered.Length; ++i)
            {
                var candidateName = topCandidates[i].Name;

                var expectedCandidateName = expectedCandidateNamesOrdered[i];

                Assert.AreEqual(candidateName, expectedCandidateName,
                    $"Expected candidates mismatch: expected {expectedCandidateName} at position {i}, but got {candidateName}");
            }
        }


        // Usually I would put this into a separate class, or inline data mocks in the test
        static readonly Candidate[] _MockedCandidates = new[]
        {
            // 5 years as baker
            new Candidate
            {
                Name = "Bob",
                WorkHistory = new []
                {
                    new EmploymentHistory
                    {
                        JobTitle = "Baker",
                        StartDate = new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero),
                        EndDate = new DateTimeOffset(2015, 1, 1, 0, 0, 0, TimeSpan.Zero)
                    },

                    new EmploymentHistory
                    {
                        JobTitle = "Cook",
                        StartDate = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero),
                        EndDate = new DateTimeOffset(2018, 1, 1, 0, 0, 0, TimeSpan.Zero)
                    }
                }
            },

            // 3 years as baker
            new Candidate
            {
                Name = "Tom",
                WorkHistory = new []
                {
                    new EmploymentHistory
                    {
                        JobTitle = "Cook",
                        StartDate = new DateTimeOffset(2011, 1, 1, 0, 0, 0, TimeSpan.Zero),
                        EndDate = new DateTimeOffset(2014, 1, 1, 0, 0, 0, TimeSpan.Zero)
                    },

                    new EmploymentHistory
                    {
                        JobTitle = "Baker",
                        StartDate = new DateTimeOffset(2012, 1, 1, 0, 0, 0, TimeSpan.Zero),
                        EndDate = null
                    }
                }
            },

            // 0 years as baker
            new Candidate
            {
                Name = "Luke",
                WorkHistory = new []
                {
                    new EmploymentHistory
                    {
                        JobTitle = "Software Developer",
                        StartDate = new DateTimeOffset(2015, 1, 1, 0, 0, 0, TimeSpan.Zero),
                        EndDate = null
                    }
                }
            },
        };

        [Test]
        public void FindTopCandidate_ReturnsOne_WhenMatched()
        {
            var candidatesRepository = new InMemoryCandidateRepository(_MockedCandidates);

            var candidateService = new CandidateService(candidatesRepository);

            var searchOptions = new TopCandidateSearchOptions("Baker", GetDurationInYears(5), _NowAtCodeWriteTime);

            var topCandidate = candidateService.FindTopCandidate(searchOptions);

            Assert.NotNull(topCandidate);
            Assert.AreEqual(_MockedCandidates[1].Name, topCandidate.Name); // Tom has most experience as a baker
        }

        [Test]
        public void FindTopCandidate_ReturnsNull_WhenUnmatched()
        {
            var candidatesRepository = new InMemoryCandidateRepository(_MockedCandidates);

            var candidateService = new CandidateService(candidatesRepository);

            var searchOptions = new TopCandidateSearchOptions("Bartender", GetDurationInYears(5), _NowAtCodeWriteTime);

            var topCandidate = candidateService.FindTopCandidate(searchOptions);

            Assert.IsNull(topCandidate);
        }


        // Although leap year may have 366 days, I do not think it will cause issues to work on 365 days intervals
        // Somebody would have to have worked more than a life time to accumulate enough leap year days
        // to create relevant amount of experience to make this method inaccurate 
        const int _DaysInYear = 365;

        static TimeSpan GetDurationInYears(int years) => TimeSpan.FromDays(years * _DaysInYear);
    }
}