using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.OrganisationSurveys;
using exp.Infrastructure.Repositories.Questions;
using exp.Infrastructure.Repositories.Sections;
using exp.Infrastructure.Repositories.SurveyResponses;
using exp.Models.Helpers;
using exp.Models.ViewModels;
using exp.Services.Organisations;
using exp.Services.Surveys;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static exp.Models.Helpers.Enums;

namespace exp.Services.OrganisationSurveys
{
    public class OrganisationSurveyService : IOrganisationSurveyService
    {
        private readonly IOrganizationSurveyRepository _organizationSurveyRepository;
        private readonly ISurveyService _surveyService;
        private readonly IOrganisationService _organization;
        private readonly IQuestionRepository _questionRepository;
        private readonly ISurveyResponseRepository _surveyResponseRepository;
        private readonly ISectionSurveyRepository _sectionSurveyRepository;

        public OrganisationSurveyService(IOrganizationSurveyRepository organizationSurveyRepository,
            ISurveyService surveyService,
            IOrganisationService organization,
            IQuestionRepository questionRepository,
            ISurveyResponseRepository surveyResponseRepository,
            ISectionSurveyRepository sectionSurveyRepository
            )
        {
            _organizationSurveyRepository = organizationSurveyRepository;
            _surveyService = surveyService;
            _organization = organization;
            _questionRepository = questionRepository;
            _surveyResponseRepository = surveyResponseRepository;
            _sectionSurveyRepository = sectionSurveyRepository;
        }

        #region Create
        public async Task AddOrganizationAtSurvey(int organizationId, int organizationSurveyId)
        {
            var survey = await _organizationSurveyRepository.Get(organizationSurveyId);

            if (survey == null)
            {
                return;
            }
            if (survey.OrganisationId == null)
            {
                survey.OrganisationId = organizationId;
                await _organizationSurveyRepository.Update(survey);
            }
            var sameSurveys = _organizationSurveyRepository.GetAllQuerable().Where(x => x.SurveyId == SurveysEnum.AwarenessId && x.OrganisationId == organizationId && x.Result == null);

            if (sameSurveys.Count() > 1)
            {
                var oldSurveytoDelete = sameSurveys.OrderByDescending(x => x.UpdatedAt).FirstOrDefault();
                var oldResponses = _surveyResponseRepository.GetAllQuerable().Where(x => x.OrganisationSurveyId == oldSurveytoDelete.Id).ToList();
                foreach (var response in oldResponses)
                {
                    await _surveyResponseRepository.DeleteAsync(response);
                }
                await _organizationSurveyRepository.Delete(oldSurveytoDelete.Id);
            }

        }
        public async Task<int> StartPublicSurvey(int surveyId)
        {
            var survey = await _surveyService.SurveyExist(surveyId);
            if (survey.IsPublic != true)
            {
                throw new ForbiddenException();
            }

            var organizationSurvey = new OrganisationSurvey
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrganisationId = null,
                SurveyId = surveyId
            };

            var createdOrganizationSurvey = await _organizationSurveyRepository.Add(organizationSurvey);

            return createdOrganizationSurvey.Id;

        }
        public async Task<int> StartSurvey(int surveyId, int organizationId)
        {
            var survey = await _surveyService.SurveyExist(surveyId);

            var organizationSurveyInprogress = _organizationSurveyRepository.GetAllQuerable().Where(x => x.OrganisationId == organizationId && x.SurveyId == survey.Id && x.Result == null);
            if (organizationSurveyInprogress.Any())
            {
                return organizationSurveyInprogress.FirstOrDefault().Id;
            }
            var organizationSurvey = new OrganisationSurvey
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrganisationId = organizationId,
                SurveyId = surveyId
            };

            var createdOrganizationSurvey = await _organizationSurveyRepository.Add(organizationSurvey);

            return createdOrganizationSurvey.Id;
        }

        private void CheckOrganizationSurvey(int organizationSurveyId, int organizationId)
        {
            var organizationSurvey = _organizationSurveyRepository.GetAllQuerable().Where(x => x.OrganisationId == organizationId && x.Id == organizationSurveyId);

            if (!organizationSurvey.Any())
            {
                throw new ForbiddenException();
            }
        }
        private async Task CheckPublicSurvey(int organizationSurveyId)
        {
            var organizationSurvey = await _organizationSurveyRepository.GetDetails().AsNoTracking().FirstOrDefaultAsync(x => x.Id == organizationSurveyId);

            if (organizationSurvey != null && organizationSurvey.Survey.IsPublic != true)
            {
                throw new ForbiddenException();
            }
        }
        private Question CheckQuestionSurvey(int questionId, int organizationSurveyId)
        {
            var surveyId = _organizationSurveyRepository.GetAllQuerable().FirstOrDefault(x => x.Id == organizationSurveyId)?.SurveyId;
            var question = _questionRepository.GetDetails().FirstOrDefault(x => x.Id == questionId && x.Section.SurveyId == surveyId);

            if (question == null)
            {
                throw new ForbiddenException();
            }

            return question;
        }
        public async Task AddAnswersQuestion(AnswersQuestionViewModels answers, int? organisationId)
        {
            if (organisationId != null)
            {
                CheckOrganizationSurvey(answers.OrganizationSurveyId, (int)organisationId);
            }
            if (organisationId == null)
            {
                await CheckPublicSurvey(answers.OrganizationSurveyId);
            }
            var survey = await _organizationSurveyRepository.GetAll().FirstOrDefaultAsync(x => x.Id == answers.OrganizationSurveyId && x.Survey.IsPublished == true);
            
            if (survey == null)
            {
                throw new NotFoundException();
            }
            if (survey.OrganisationId != null)
            {
                CheckOrganizationSurvey(answers.OrganizationSurveyId, (int)survey.OrganisationId);
            }
            if (survey.Result != null)
            {
                throw new ForbiddenException();
            }

            var question = CheckQuestionSurvey(answers.QuestionId, answers.OrganizationSurveyId);

            var existingAnswer = await _surveyResponseRepository.GetAllQuerable()
                .FirstOrDefaultAsync(x => x.QuestionId == answers.QuestionId && x.OrganisationSurveyId == answers.OrganizationSurveyId);

            if (String.IsNullOrEmpty(answers.Answer))
            {
                if (existingAnswer != null)
                {
                    await _surveyResponseRepository.DeleteAsync(existingAnswer);
                }

                return;
            }

            if (existingAnswer != null)
            {
                existingAnswer.Response = answers.Answer;
                existingAnswer.Mentions = null;

                if (existingAnswer.Response == "Other" && question.QuestionType.Name == QuestionTypes.Check)
                {
                    existingAnswer.Mentions = answers.Mentions;
                }

                await _surveyResponseRepository.Update(existingAnswer);
                await RemoveChildQuestionResponse(question, answers.OrganizationSurveyId);
            }
            else
            {
                var newResponse = new SurveyResponse
                {
                    OrganisationSurveyId = answers.OrganizationSurveyId,
                    QuestionId = answers.QuestionId,
                    Response = answers.Answer
                };
                await _surveyResponseRepository.Add(newResponse);
            }
        }
        public async Task RemoveChildQuestionResponse(Question question, int organizationSurveyId)
        {
            var dependentQuestionIds = question.AnswersPrompts
                    .SelectMany(ap => ap.Questions)
                    .Select(q => q.Id)
                    .Distinct()
                    .ToList();
            foreach (var id in dependentQuestionIds)
            {
                var dependentResponse = _surveyResponseRepository.GetAllQuerable()
                    .FirstOrDefault(x => x.QuestionId == id && x.OrganisationSurveyId == organizationSurveyId);
                if (dependentResponse != null)
                {
                    await _surveyResponseRepository.Delete(dependentResponse.Id);
                }
                var child = await _questionRepository.GetDetails().FirstOrDefaultAsync(x => x.Id == id);
                if (child != null)
                {
                    await RemoveChildQuestionResponse(child, organizationSurveyId);
                }

            }
        }
        public async Task SubmitSurvey(int organisationSurveyId, int? organisationId)
        {
            if (organisationId != null)
            {
                CheckOrganizationSurvey(organisationSurveyId, (int)organisationId);
            }
            if (organisationId == null)
            {
                await CheckPublicSurvey(organisationSurveyId);
            }
            var survey = _organizationSurveyRepository.GetAll().FirstOrDefault(x => x.Id == organisationSurveyId && x.Survey.IsPublished == true);

            if (survey == null)
            {
                throw new NotFoundException("Not found survey!");
            }
            if (survey.OrganisationId != null)
            {
                CheckOrganizationSurvey(organisationSurveyId, (int)survey.OrganisationId);
            }
            var responses = _surveyResponseRepository.GetAllQuerable().Count(x => x.OrganisationSurveyId == organisationSurveyId && (x.Response != "Other" || (x.Response == "Other" && x.Mentions != null)));

            var questionSurvey = _questionRepository.GetAll().ToList().Where(q => q.ParrentAnswerId == null || (q.ParrentAnswer != null && q.ParrentAnswer.Answers == survey.SurveyResponses.FirstOrDefault(r => r.QuestionId == q.ParrentAnswer.QuestionId)?.Response)).Count(x => x.Section.SurveyId == survey.SurveyId);

            if (responses != questionSurvey)
            {
                throw new Exception("Must be answer all questions");
            }

            survey.Result = GetResult(survey.SurveyId, organisationSurveyId);
            await _organizationSurveyRepository.Update(survey);

            //if (survey.Survey.SurveyType == SurveyTypes.RuleCheck)
            //{
            //    var lastQuestion = _surveyResponseRepository.GetAllQuerable().OrderByDescending(x => x.Question.Position).FirstOrDefault();
            //    if (lastQuestion != null)
            //    {
            //        var result = _questionRepository.GetAll().FirstOrDefault(x => x.Id == lastQuestion.QuestionId)?.AnswersPrompts.FirstOrDefault(x => x.Answers == lastQuestion.Response)?.Prompt;

            //        survey.Result = result ?? "Thank you!";

            //        await _organizationSurveyRepository.Update(survey);
            //    }
            //}
            //else if (survey.Survey.SurveyType == SurveyTypes.Score)
            //{
            //    var score = _surveyResponseRepository.GetAllQuerable().Where(x => x.OrganisationSurveyId == organisationSurveyId).Average(x => Convert.ToInt32(x.Response));

            //    survey.Result = score.ToString();

            //    await _organizationSurveyRepository.Update(survey);
            //}

        }
        public string GetResult(int surveyId, int organisationSurveyId)
        {
            var responses = _surveyResponseRepository.GetAllQuerable().Where(x => x.OrganisationSurveyId == organisationSurveyId);

            if (surveyId == 3)
            {
                if (responses.FirstOrDefault(x => x.QuestionId == 48)?.Response == "No")
                {
                    return "Your SME is not in the scope of CRA regulation";
                }
                else if (responses.FirstOrDefault(x => x.QuestionId == 48)?.Response == "Yes")
                {
                    return "Your SME is in the scope of CRA regulation";
                }
                if (responses.FirstOrDefault(x => x.QuestionId == 46)?.Response == "No")
                {
                    return "Your SME is not in the scope of CRA regulation";
                }
                else if (responses.FirstOrDefault(x => x.QuestionId == 46)?.Response == "Yes" && responses.FirstOrDefault(x => x.QuestionId == 50)?.Response == "Yes")
                {
                    return "Your SME is in the scope of CRA regulation";
                }
                else
                {
                    return "Your SME is not in the scope of CRA regulation";
                }
            }

            if (surveyId == 7)
            {
                if (responses.FirstOrDefault(x => x.QuestionId == 129)?.Response == "Yes" || responses.FirstOrDefault(x => x.QuestionId == 129)?.Response == "Unsure")
                {
                    return "Your SME is not in the scope of CRA regulation";
                }
                else if (responses.FirstOrDefault(x => x.QuestionId == 130)?.Response == "Yes")
                {
                    return "Your SME is not in the scope of CRA regulation";
                }
                else if (responses.FirstOrDefault(x => x.QuestionId == 130)?.Response == "Yes" || responses.FirstOrDefault(x => x.QuestionId == 130)?.Response == "Maybe" || responses.FirstOrDefault(x => x.QuestionId == 131)?.Response == "Yes" || responses.FirstOrDefault(x => x.QuestionId == 131)?.Response == "Maybe")
                {
                    return "You may need to confirm with the relevant authority whether you are de facto considered “important” or “critical”";
                }
                else return "Your product is likely the default product and is eligible to take the Maturity Survey.";

            }
            if (surveyId == 8)
            {
                var score = _surveyResponseRepository.GetAllQuerable().Where(x => x.OrganisationSurveyId == organisationSurveyId).Average(x => Convert.ToInt32(x.Response));
                return Math.Round(score, 2).ToString("F2");
            }

            return "Thank you";
        }
        #endregion

        #region Read
        public async Task<List<SurveySectionStatusViewModel>> GetSectionProgress(int organisationSurveyId, int? organisationId)
        {
            if (organisationId != null)
            {
                CheckOrganizationSurvey(organisationSurveyId, (int)organisationId);
            }
            if (organisationId == null)
            {
                await CheckPublicSurvey(organisationSurveyId);
            }
            var responses = await _surveyResponseRepository.GetAll()
             .Where(r => r.OrganisationSurveyId == organisationSurveyId)
             .ToListAsync();
            var survey = await _organizationSurveyRepository.Get(organisationSurveyId);
            var sectionProgress = _sectionSurveyRepository.GetAll().ToList()
                .Where(s => s.SurveyId == survey.SurveyId && s.Survey.IsPublished && s.ParentSectionId == null)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.InverseParentSection,
                    Questions = s.Questions.Where(q => q.ParrentAnswerId == null || (q.ParrentAnswer != null && q.ParrentAnswer.Answers == responses.FirstOrDefault(r => r.QuestionId == q.ParrentAnswer.QuestionId)?.Response)).Select(q => q.Id).ToList()
                })
                .ToList();

            var result = sectionProgress.Where(x => x.Questions.Count > 0 || x.InverseParentSection.Count() > 0).Select(s =>
            {
                var total = s.Questions.Count + s.InverseParentSection.Sum(y => y.Questions.Count());
                var answered = s.Questions.Count(qid => responses.Any(r => r.QuestionId == qid)) + s.InverseParentSection.Sum(y => y.Questions.Count(qid => responses.Any(r => r.QuestionId == qid.Id))); ;

                string status = answered == 0 ? "not_started"
                             : answered < total ? "in_progress"
                             : "finish";

                return new SurveySectionStatusViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Status = status
                };
            }).ToList();

            return result;

        }
        public PagingViewModel<GroupedSurveyDetailsViewModel> GetOrganizationSurveys(PagingFilterViewModel filter, int organizationId)
        {

            var organizationSurvey = _organizationSurveyRepository.GetAll().Where(x => x.OrganisationId == organizationId);
            var surveys = _surveyService.GetSurveys();
            if (!surveys.Any())
            {
                return new PagingViewModel<GroupedSurveyDetailsViewModel>();
            }
            PagingViewModel<GroupedSurveyDetailsViewModel> pagingViewModel = new PagingViewModel<GroupedSurveyDetailsViewModel>();
            pagingViewModel.Count = surveys.Count();
            pagingViewModel.NumberOfPages = pagingViewModel.Count / filter.PageSize;

            if (pagingViewModel.Count % filter.PageSize > 0)
            {
                pagingViewModel.NumberOfPages += 1;
            }

            pagingViewModel.Items = surveys
              .Skip(filter.PageSize * (filter.PageNumber - 1))
              .Take(filter.PageSize)
              .Select(survey =>
              {
                  var orgSurvey = organizationSurvey.Where(y => y.SurveyId == survey.Id);

                  return new GroupedSurveyDetailsViewModel
                  {
                      Id = survey.Id,
                      Description = survey.Description,
                      Name = survey.Name,
                      NumberOfFinishedSurveys = orgSurvey.Count(x => x.Result != null),
                      ProgressedSurveyId = orgSurvey.FirstOrDefault(x => x.Result == null)?.Id,
                  };
              }).OrderBy(x => x.Id)
              .ToList();

            return pagingViewModel;
        }
        public async Task<MaturityReportsViewModel> GetScoreSurveyReports(int organisationSurveyId, int? organisationId)
        {
            if (organisationId != null)
            {
                CheckOrganizationSurvey(organisationSurveyId, (int)organisationId);
            }
            if (organisationId == null)
            {
                await CheckPublicSurvey(organisationSurveyId);
            }
            var organizationSurvey = _organizationSurveyRepository.GetAll().Where(x => x.Id == organisationSurveyId).FirstOrDefault();
            MaturityReportsViewModel raport = new();

            if (organizationSurvey == null)
            {
                throw new NotFoundException("Not found!");
            }
            if (organizationSurvey.Survey.SurveyType != SurveyTypes.Score)
            {
                return raport;
            }
            var survey = _surveyService.GetSurveyDetails(organizationSurvey.SurveyId);

            if (survey == null)
            {
                return raport;
            }


            var results = _surveyResponseRepository.GetDetails().Where(z => z.OrganisationSurveyId == organisationSurveyId);

            if (!results.Any())
            {
                return raport;
            }

            var sections = _sectionSurveyRepository.GetAllQuerable().Where(x => x.SurveyId == SurveysEnum.MaturityId && x.ParentSectionId != null);

            raport.DefaultResult = sections.Select(x => new ResultReportsViewModel
            {
                Category = x.Name,
                Result = "4"
            }).ToList();
            raport.OrganisationResult = sections.Select(x => new ResultReportsViewModel
            {
                Category = x.Name,
                Result = results.Any(y => y.Question.SectionId == x.Id) ? Math.Round(results.Where(y => y.Question.SectionId == x.Id).Average(z => Convert.ToInt64(z.Response)), 2).ToString("F2") : "0"
            }).ToList();

            return raport;
        }
        public MaturityReportsViewModel GetScoreSurveyReports(int organisationSurveyId)
        {
            var organizationSurvey = _organizationSurveyRepository.GetAll().Where(x => x.Id == organisationSurveyId).FirstOrDefault();

            MaturityReportsViewModel raport = new();

            if (organizationSurvey == null)
            {
                throw new NotFoundException("Not found!");
            }
            if (organizationSurvey.Survey.SurveyType != SurveyTypes.Score)
            {
                return raport;
            }
            var survey = _surveyService.GetSurveyDetails(organizationSurvey.SurveyId);

            if (survey == null)
            {
                return raport;
            }

            var results = _surveyResponseRepository.GetDetails().Where(z => z.OrganisationSurveyId == organisationSurveyId);

            if (!results.Any())
            {
                return raport;
            }

            var sections = _sectionSurveyRepository.GetAllQuerable().Where(x => x.SurveyId == SurveysEnum.MaturityId && x.ParentSectionId != null);

            raport.DefaultResult = sections.Select(x => new ResultReportsViewModel
            {
                Category = x.Name,
                Result = "4"
            }).ToList();
            raport.OrganisationResult = sections.Select(x => new ResultReportsViewModel
            {
                Category = x.Name,
                Result = results.Any(y => y.Question.SectionId == x.Id) ? Math.Round(results.Where(y => y.Question.SectionId == x.Id).Average(z => Convert.ToInt64(z.Response)), 2).ToString("F2") : "0"
            }).ToList();

            return raport;
        }
        public PagingViewModel<GroupedSurveyDetailsViewModel> GetOrganizationSurveysForAdm(FilterPagingSurveyAdmViewModel filter)
        {

            var organizationSurvey = _organizationSurveyRepository.GetAll().Where(x => x.OrganisationId == filter.OrganisationId);
            var surveys = _surveyService.GetSurveys();
            if (!surveys.Any())
            {
                return new PagingViewModel<GroupedSurveyDetailsViewModel>();
            }
            PagingViewModel<GroupedSurveyDetailsViewModel> pagingViewModel = new PagingViewModel<GroupedSurveyDetailsViewModel>();
            pagingViewModel.Count = surveys.Count();
            pagingViewModel.NumberOfPages = pagingViewModel.Count / filter.PageSize;

            if (pagingViewModel.Count % filter.PageSize > 0)
            {
                pagingViewModel.NumberOfPages += 1;
            }

            pagingViewModel.Items = surveys
              .Skip(filter.PageSize * (filter.PageNumber - 1))
              .Take(filter.PageSize)
              .Select(survey =>
              {
                  var orgSurvey = organizationSurvey.Where(y => y.SurveyId == survey.Id);

                  return new GroupedSurveyDetailsViewModel
                  {
                      Id = survey.Id,
                      Description = survey.Description,
                      Name = survey.Name,
                      NumberOfFinishedSurveys = orgSurvey.Count(x => x.Result != null),
                      ProgressedSurveyId = orgSurvey.FirstOrDefault(x => x.Result == null)?.Id,
                  };
              }).OrderBy(x => x.Id)
              .ToList();

            return pagingViewModel;
        }
        public async Task<SurveyDetailsViewModel> GetHistorySurvey(int surveyId, int organizationId)
        {
            var organizationSurvey = _organizationSurveyRepository.GetAll().Where(x => x.OrganisationId == organizationId && x.SurveyId == surveyId);

            if (!organizationSurvey.Any())
            {
                return new SurveyDetailsViewModel();
            }
            var survey = await _surveyService.GetSurvey(organizationSurvey.FirstOrDefault()!.SurveyId);

            var surveyDetails = new SurveyDetailsViewModel
            {
                Id = survey.Id,
                Description = survey.Description,
                Name = survey.Name,
                Surveys = organizationSurvey.Select(x => new HistorySurveyDetailsViewModel
                {
                    StartDate = x.CreatedAt,
                    UpdateDate = x.UpdatedAt,
                    Status = x.Result != null ? "Finished" : "Progress",
                    Result = x.Result,
                    OrganisationSurveyId = x.Id
                }).ToList()
            };

            return surveyDetails;

        }
        public async Task<SurveyDetailsViewModel> GetHistorySurveyAdm(int organisationId, int surveyId)
        {
            var organizationSurvey = _organizationSurveyRepository.GetAll().Where(x => x.OrganisationId == organisationId && x.SurveyId == surveyId && x.Result != null);

            if (!organizationSurvey.Any())
            {
                return new SurveyDetailsViewModel();
            }
            var survey = await _surveyService.GetSurvey(organizationSurvey.FirstOrDefault()!.SurveyId);

            var surveyDetails = new SurveyDetailsViewModel
            {
                Id = survey.Id,
                Description = survey.Description,
                Name = survey.Name,
                Surveys = organizationSurvey.Select(x => new HistorySurveyDetailsViewModel
                {
                    StartDate = x.CreatedAt,
                    UpdateDate = x.UpdatedAt,
                    Status = "Finished",
                    Result = x.Result,
                    OrganisationSurveyId = x.Id
                }).ToList()
            };

            return surveyDetails;

        }
        public SurveyResponseDetailsViewModel GetOrganizationSurvey(int organizationSurveyId, int? organisationId)
        {
            if (organisationId != null)
            {
                CheckOrganizationSurvey(organizationSurveyId, (int)organisationId);
            }
            var organizationSurvey = _organizationSurveyRepository.GetAll().Where(x => x.Id == organizationSurveyId).FirstOrDefault();

            if (organizationSurvey == null)
            {
                throw new NotFoundException("Not found!");
            }

            var survey = _surveyService.GetSurveyDetails(organizationSurvey.SurveyId);

            if (survey == null)
            {
                return new SurveyResponseDetailsViewModel();
            }

            var responsesByQuestionId = organizationSurvey.SurveyResponses
                .ToDictionary(r => r.QuestionId, r => r);


            bool IsSubmitted = organizationSurvey.Result != null;

            var sections = survey.SurveySections
                .Where(x => x.ParentSectionId == null)
                .Select(section => new SectionDetailsViewModel
                {
                    Id = section.Id,
                    Title = section.Name,
                    Description = section.Description,
                    Questions = section.Questions
                        .Where(x => IsVisible(x, responsesByQuestionId))
                        .OrderBy(q => q.Position)
                        .Select(q => MapQuestion(q, responsesByQuestionId, IsSubmitted))
                        .ToList(),
                    SubSection = section.InverseParentSection
                        .Where(sub => sub.Questions.Any(x => IsVisible(x, responsesByQuestionId)))
                        .Select(sub => new SectionDetailsViewModel
                        {
                            Id = sub.Id,
                            Title = sub.Name,
                            Description = sub.Description,
                            ParentId = sub.Questions.FirstOrDefault()?.ParrentAnswer?.QuestionId,
                            Questions = sub.Questions
                                .Where(x => IsVisible(x, responsesByQuestionId))
                                .Select(q => MapQuestion(q, responsesByQuestionId, IsSubmitted))
                                .ToList(),
                        })
                        .ToList(),
                })
                .Where(s => s.Questions.Count > 0 || s.SubSection?.Count > 0)
                .ToList();

            return new SurveyResponseDetailsViewModel
            {
                Id = survey.Id,
                Title = survey.Name,
                Type = survey.SurveyType,
                Description = survey.Description,
                OrganizationSurveyId = organizationSurveyId,
                Status = IsSubmitted ? "Finish" : "In progress",
                Submited = IsSubmitted,
                Sections = sections,
                Result = IsSubmitted == true ? organizationSurvey.Result : null
            };
        }
        public async Task<SurveyResponseDetailsViewModel> GetOrganizationSurvey(int organizationSurveyId, bool forAdm)
        {
            if (forAdm == false)
            {
                await CheckPublicSurvey(organizationSurveyId);
            }

            var organizationSurvey = _organizationSurveyRepository.GetAll().Where(x => x.Id == organizationSurveyId).FirstOrDefault();

            if (organizationSurvey == null)
            {
                throw new NotFoundException("Not found!");
            }

            var survey = _surveyService.GetSurveyDetails(organizationSurvey.SurveyId);

            if (survey == null)
            {
                return new SurveyResponseDetailsViewModel();
            }

            var responsesByQuestionId = organizationSurvey.SurveyResponses
                .ToDictionary(r => r.QuestionId, r => r);


            bool IsSubmitted = organizationSurvey.Result != null;

            var sections = survey.SurveySections
                .Where(x => x.ParentSectionId == null)
                .Select(section => new SectionDetailsViewModel
                {
                    Id = section.Id,
                    Title = section.Name,
                    Description = section.Description,
                    Questions = section.Questions
                        .Where(x => IsVisible(x, responsesByQuestionId))
                        .OrderBy(q => q.Position)
                        .Select(q => MapQuestion(q, responsesByQuestionId, IsSubmitted))
                        .ToList(),
                    SubSection = section.InverseParentSection
                        .Where(sub => sub.Questions.Any(x => IsVisible(x, responsesByQuestionId)))
                        .Select(sub => new SectionDetailsViewModel
                        {
                            Id = sub.Id,
                            Title = sub.Name,
                            Description = sub.Description,
                            ParentId = sub.Questions.FirstOrDefault()?.ParrentAnswer?.QuestionId,
                            Questions = sub.Questions
                                .Where(x => IsVisible(x, responsesByQuestionId))
                                .Select(q => MapQuestion(q, responsesByQuestionId, IsSubmitted))
                                .ToList(),
                        })
                        .ToList(),
                })
                .Where(s => s.Questions.Count > 0 || s.SubSection?.Count > 0)
                .ToList();

            return new SurveyResponseDetailsViewModel
            {
                Id = survey.Id,
                Title = survey.Name,
                Type = survey.SurveyType,
                Description = survey.Description,
                OrganizationSurveyId = organizationSurveyId,
                Status = IsSubmitted ? "Finish" : "In progress",
                Submited = IsSubmitted,
                Sections = sections,
                Result = IsSubmitted == true ? organizationSurvey.Result : null
            };
        }
        private bool IsVisible(Question question, Dictionary<int, SurveyResponse> responsesByQuestionId)
        {
            if (question.ParrentAnswerId == null)
                return true;

            if (!responsesByQuestionId.TryGetValue(question.ParrentAnswer!.QuestionId, out var response))
                return false;

            return question.ParrentAnswer?.Answers == response.Response;
        }
        private QuestionAnswersViewModel MapQuestion(Question q, Dictionary<int, SurveyResponse> responses, bool isSubmitted)
        {
            responses.TryGetValue(q.Id, out var response);

            bool isTextType = q.QuestionType.Name == Enums.QuestionTypes.Text;

            return new QuestionAnswersViewModel
            {
                Id = q.Id,
                Title = q.Name,
                Description = q.Description,
                ParentId = q.ParrentAnswer?.QuestionId,
                Type = q.QuestionType.Name,
                Answer = isTextType ? response?.Response : null,
                Answers = isTextType ? new List<AnswerViewModel>() :
                    q.AnswersPrompts.Select(a => new AnswerViewModel
                    {
                        Answer = a.Answers,
                        Comment = a.Comment,
                        Check = response?.Response == a.Answers,
                        Prompt = isSubmitted && response?.Response == a.Answers ? a.Prompt : null,
                        Mentions = response?.Mentions
                    }).ToList(),
                Attachments = q.QuestionAttachments.Select(x => new BaseViewModel { Id = x.AttachmentId, Name = x.Attachment.Name }).ToList()
            };
        }
        #endregion
    }
}
