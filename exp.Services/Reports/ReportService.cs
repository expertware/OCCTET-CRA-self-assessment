using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.SurveyResponses;
using exp.Infrastructure.Repositories.VwOrganisationResults;
using exp.Models.ViewModels;
using static exp.Models.Helpers.Enums;

namespace exp.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IVwOrganisationResultRepository _vwOrganisationResultRepository;
        private readonly IVwSectionReports _vwSectionReports;
        private readonly IOrganisationReportsRepository _organisationReportsRepository;
        private readonly ISurveyResponseRepository _surveyResponseRepository;
        public ReportService(IOrganisationReportsRepository organisationReportsRepository,
            IVwSectionReports vwSectionReports,
            IVwOrganisationResultRepository vwOrganisationResultRepository,
            ISurveyResponseRepository surveyResponseRepository)
        {
            _vwOrganisationResultRepository = vwOrganisationResultRepository;
            _vwSectionReports = vwSectionReports;
            _organisationReportsRepository = organisationReportsRepository;
            _surveyResponseRepository = surveyResponseRepository;
        }
        public IQueryable<VwOrganisationResult> FilterReports(IQueryable<VwOrganisationResult> list, ReportFilterViewModel filter)
        {

            if (filter.CountryId != null)
            {
                list = list.Where(x => x.CountryId == filter.CountryId);
            }
            if (filter.StartDate != null && filter.EndDate != null)
            {
                list = list.Where(x => x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate);
            }
            if (filter.ActivityDomainId != null)
            {
                list = list.Where(x => x.ActivitySectorId == filter.ActivityDomainId);
            }
            if (filter.CompanySize != null)
            {
                list = list.Where(x => x.CompanySize == filter.CompanySize);
            }

            return list;

        }
        public IQueryable<VwOrgannisationsReport> FilterReports(IQueryable<VwOrgannisationsReport> list, ReportFilterViewModel filter)
        {

            if (filter.CountryId != null)
            {
                list = list.Where(x => x.CountryId == filter.CountryId);
            }
            if (filter.StartDate != null && filter.EndDate != null)
            {
                list = list.Where(x => x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate);
            }
            if (filter.ActivityDomainId != null)
            {
                list = list.Where(x => x.ActivitySectorId == filter.ActivityDomainId);
            }
            if (filter.CompanySize != null)
            {
                list = list.Where(x => x.CompanySize == filter.CompanySize);
            }

            return list;

        }
        public List<BaseReportViewModel> GetOrganisationsOnCountries(ReportFilterViewModel filter)
        {
            var countries = _organisationReportsRepository.GetAllQuerable();
            if (!countries.Any())
            {
                return new List<BaseReportViewModel>();
            }
            countries = FilterReports(countries, filter);

            var filtered = countries.GroupBy(x => x.CountryName)
                .OrderByDescending(x => x.Select(y => y.Id).Distinct().Count())
                .Select(x => new BaseReportViewModel
                {
                    Name = x.Key,
                    Count = x.Select(y => y.Id).Distinct().Count()
                })
                .ToList();

            return filtered;
        }
        public List<BaseReportViewModel> GetCountOrganisationsByCRAScope(ReportFilterViewModel filter)
        {
            var surveys = _vwOrganisationResultRepository.GetAllQuerable().Where(x => x.SurveyId == SurveysEnum.AwarenessId);

            if (!surveys.Any())
            {
                return new List<BaseReportViewModel>();
            }

            surveys = FilterReports(surveys, filter);
            var reportsDetails = surveys.GroupBy(x => x.Result).Select(y => new BaseReportViewModel
            {
                Name = y.Key,
                Count = y.Select(x => x.Id).Distinct().Count()
            }).ToList();

            return reportsDetails;
        }

        public List<BaseReportViewModel> GetOrganizationsRgistersOnTime(ReportFilterViewModel filter)
        {
            var list = _organisationReportsRepository.GetAllQuerable();

            if (!list.Any())
            {
                return new List<BaseReportViewModel>();
            }

            list = FilterReports(list, filter);
            string groupFormat = "dd/MM/yyyy";

            if (filter.StartDate.HasValue && filter.EndDate.HasValue)
            {
                var daysDiff = (filter.EndDate.Value - filter.StartDate.Value).TotalDays;

                if (daysDiff >= 365)
                {
                    groupFormat = "yyyy";
                }
                else if (daysDiff >= 30)
                {
                    groupFormat = "MM/yyyy";
                }
            }

            var organisationOnTime = list
                .AsEnumerable()
                .Where(x => x.CreatedAt.HasValue)
                .GroupBy(x => x.CreatedAt.Value.ToString(groupFormat))
                .Select(x => new BaseReportViewModel
                {
                    Name = x.Key,
                    Count = x.Select(y => y.Id).Distinct().Count()
                })
                .ToList().OrderBy(x => x.Name).ToList();

            return organisationOnTime;
        }
        public List<BaseReportViewModel> GetOrganizationsBySize(ReportFilterViewModel filter)
        {
            var organisations = _organisationReportsRepository.GetAllQuerable();

            if (!organisations.Any())
            {
                return new List<BaseReportViewModel>();
            }

            organisations = FilterReports(organisations, filter);

            var organisationoOnTime = CompanySizeLabels.Labels.Select(x => new BaseReportViewModel
            {
                Name = x,
                Count = organisations.Where(y => y.CompanySize == x).Select(y => y.Id).Distinct().Count()
            }).ToList();

            return organisationoOnTime;
        }
        public List<BaseReportViewModel> GetTopRegisteredSectors(ReportFilterViewModel filter)
        {
            var sectors = _organisationReportsRepository.GetAllQuerable();

            if (!sectors.Any())
            {
                return new List<BaseReportViewModel>();
            }

            sectors = FilterReports(sectors, filter);

            var topSectors = sectors.GroupBy(x => x.ActivitySectorName).OrderBy(x => x.Select(y => y.Id).Distinct().Count()).Take(filter.Take ?? 10)
                .Select(x => new BaseReportViewModel
                {
                    Name = x.Key,
                    Count = x.Select(y => y.Id).Distinct().Count()
                })
                .ToList();

            return topSectors;
        }
        public MaturityReportsViewModel GetMaturityCategories(ReportFilterViewModel filter)
        {
            var list = _vwSectionReports.GetAllQuerable().Where(x => x.SurveyId == SurveysEnum.MaturityId && x.Result != null);

            if (!list.Any())
            {
                return new MaturityReportsViewModel();
            }

            if (filter.CountryId != null)
            {
                list = list.Where(x => x.CountryId == filter.CountryId);
            }
            if (filter.StartDate != null && filter.EndDate != null)
            {
                list = list.Where(x => x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate);
            }
            if (filter.ActivityDomainId != null)
            {
                list = list.Where(x => x.ActivitySectorId == filter.ActivityDomainId);
            }
            if (filter.CompanySize != null)
            {
                list = list.Where(x => x.CompanySize == filter.CompanySize);
            }
            MaturityReportsViewModel result = new();

            result.OrganisationResult = list.ToList()
              .DistinctBy(x => x.Id)
              .GroupBy(x => x.Name)
              .Select(g => new
              {
                  Name = g.Key,
                  Avg = g.Select(z => Convert.ToInt64(z.Response)).Average()
              })
              .OrderByDescending(x => x.Avg)
              .Take(filter.Take ?? 10)
              .Select(x => new ResultReportsViewModel
              {
                  Category = x.Name,
                  Result = x.Avg.ToString("F2")
              })
             .ToList();
            result.DefaultResult = result.OrganisationResult.Select(x => x.Category).Select(x => new ResultReportsViewModel
            {
                Category = x,
                Result = "4"
            }).ToList();

            result.TotalScore = result.OrganisationResult.Count != 0 ? (result.OrganisationResult.Sum(x => Convert.ToDouble(x.Result)) / result.OrganisationResult.Count).ToString("F2") : "0";
            return result;
        }
        public List<BaseReportViewModel> GetAwarenessofTheCRA(ReportFilterViewModel filter)
        {
            var result = _vwSectionReports.GetAllQuerable().Where(x => x.QuestionId == QuestionForReport.CraFamiliarityId && x.Result != null);

            if (!result.Any())
            {
                return new List<BaseReportViewModel>();
            }

            if (filter.CountryId != null)
            {
                result = result.Where(x => x.CountryId == filter.CountryId);
            }
            if (filter.StartDate != null && filter.EndDate != null)
            {
                result = result.Where(x => x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate);
            }
            if (filter.ActivityDomainId != null)
            {
                result = result.Where(x => x.ActivitySectorId == filter.ActivityDomainId);
            }
            if (filter.CompanySize != null)
            {
                result = result.Where(x => x.CompanySize == filter.CompanySize);
            }
            var resultDetails = new List<BaseReportViewModel> {new BaseReportViewModel
            {
                Count = result.Where(x => x.Response == "Not familiar at all").Select(x => x.SurveyId).Distinct().Count(),
                Name = "Not familiar at all"
            }, new BaseReportViewModel
            {
                Count = result.Where(x => x.Response == "Somewhat familiar").Select(x => x.SurveyId).Distinct().Count(),
                Name = "Somewhat familiar"
            },new BaseReportViewModel
             {
                Count = result.Where(x => x.Response == "Very familiar").Select(x => x.SurveyId).Distinct().Count(),
                Name = "Very familiar"
            }};
            //var resultDetails = result.GroupBy(x => x.Response).Select(x => new BaseReportViewModel
            //{
            //    Count = x.Select(x => x.Id).Distinct().Count(),
            //    Name = x.Key
            //}).ToList();

            return resultDetails;
        }
        public List<BaseReportViewModel> GetOrganisationCRARegulationApply(ReportFilterViewModel filter)
        {
            var result = _vwSectionReports.GetAllQuerable().Where(x => x.SurveyId == SurveysEnum.AwarenessId && x.Result != null);

            if (!result.Any())
            {
                return new List<BaseReportViewModel>();
            }

            if (filter.CountryId != null)
            {
                result = result.Where(x => x.CountryId == filter.CountryId);
            }
            if (filter.StartDate != null && filter.EndDate != null)
            {
                result = result.Where(x => x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate);
            }
            if (filter.ActivityDomainId != null)
            {
                result = result.Where(x => x.ActivitySectorId == filter.ActivityDomainId);
            }
            if (filter.CompanySize != null)
            {
                result = result.Where(x => x.CompanySize == filter.CompanySize);
            }
            var resultDetails = new List<BaseReportViewModel> {new BaseReportViewModel
            {
                Count = result.Where(x => x.Result == "Your SME is in the scope of CRA regulation").Select(x => x.SurveyId).Distinct().Count(),
                Name = "Yes"
            }, new BaseReportViewModel
            {
                Count = result.Where(x => x.Result == "Your SME is not in the scope of CRA regulation").Select(x => x.SurveyId).Distinct().Count(),
                Name = "No"
            }};
            //var resultDetails = result.GroupBy(x => x.Result).Select(x => new BaseReportViewModel
            //{
            //    Count = x.Select(x => x.SurveyId).Distinct().Count(),
            //    Name = x.Key
            //}).ToList();

            return resultDetails;
        }
        public List<BaseReportViewModel> GetAwarenessSurveyReport(ReportFilterViewModel filter)
        {
            var result = _vwSectionReports.GetAllQuerable().Where(x => x.SurveyId == SurveysEnum.AwarenessId && x.Result != null);

            if (!result.Any())
            {
                return new List<BaseReportViewModel>();
            }

            if (filter.CountryId != null)
            {
                result = result.Where(x => x.CountryId == filter.CountryId);
            }
            if (filter.StartDate != null && filter.EndDate != null)
            {
                result = result.Where(x => x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate);
            }
            if (filter.ActivityDomainId != null)
            {
                result = result.Where(x => x.ActivitySectorId == filter.ActivityDomainId);
            }
            if (filter.CompanySize != null)
            {
                result = result.Where(x => x.CompanySize == filter.CompanySize);
            }
            var resultDetails = new List<BaseReportViewModel> {new BaseReportViewModel
            {
                Count = result.Where(x => x.CountryId == null).Select(x => x.SurveyId).Distinct().Count(),
                Name = "Without account"
            }, new BaseReportViewModel
            {
                Count = result.Where(x => x.CountryId != null).Select(x => x.SurveyId).Distinct().Count(),
                Name = "With account"
            } };

            return resultDetails;
        }
    }
}
