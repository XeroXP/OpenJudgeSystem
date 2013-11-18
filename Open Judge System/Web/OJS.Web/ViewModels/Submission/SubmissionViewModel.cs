﻿namespace OJS.Web.ViewModels.Submission
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using OJS.Common.Models;
    using OJS.Data.Models;
    using OJS.Web.ViewModels.TestRun;
    using System.Linq.Expressions;

    public class SubmissionViewModel
    {
        public static Expression<Func<Submission, SubmissionViewModel>> FromSubmission
        {
            get
            {
                return submission => new SubmissionViewModel
                {
                    Id = submission.Id,
                    SubmitedOn = submission.CreatedOn,
                    ProblemId = submission.ProblemId,
                    ProblemName = submission.Problem.Name,
                    ProblemMaximumPoints = submission.Problem.MaximumPoints,
                    Contest = submission.Problem.Contest.Name,
                    ParticipantId = submission.ParticipantId,
                    ParticipantName = submission.Participant.User.UserName,
                    TestResults = submission.TestRuns.AsQueryable().Where(x => !x.Test.IsTrialTest).Select(TestRunViewModel.FromTestRun)
                };
            }
        }

        public int Id { get; set; }

        public int? ParticipantId { get; set; }

        public string ParticipantName { get; set; }

        public DateTime SubmitedOn { get; set; }

        public int? ProblemId { get; set; }

        public string ProblemName { get; set; }

        public int ProblemMaximumPoints { get; set; }

        public string Contest { get; set; }

        public string ProgrammingLanguage { get; set; }

        public IEnumerable<TestRunViewModel> TestResults { get; set; }

        public int Points
        {
            get
            {
                var correctTests = (decimal)TestResults.Count(x => x.ExecutionResult == TestRunResultType.CorrectAnswer);

                var allTests = TestResults.Count();

                return allTests > 0 ? (int)((correctTests / allTests) * this.ProblemMaximumPoints) : 0;
            }
        }

        public bool HasFullPoints
        {
            get
            {
                return this.Points == this.ProblemMaximumPoints ? true : false;
            }
        }

        public int MaxUsedTime
        {
            get
            {
                return TestResults.Count() > 0 ? TestResults.Select(x => x.TimeUsed).Max() : 0;
            }
        }

        public long MaxUsedMemory
        {
            get
            {
                return TestResults.Count() > 0 ? TestResults.Select(x => x.MemoryUsed).Max() : 0;
            }
        }
    }
}