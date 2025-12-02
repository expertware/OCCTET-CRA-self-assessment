using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exp.Models.ViewModels
{
    public class BaseReportViewModel
    {
        public string Name { get; set; } = default!;
        public int Count { get; set; }
    }
    public class SectionReportViewModel
    {
        public string Name { get; set; } = default!;
        public string Avg { get; set; } = default!;
    }
    public class ReportFilterViewModel
    {
        public int? CountryId  { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CompanySize { get; set; }
        public int? Take { get; set; } = 10;
        public int? ActivityDomainId { get; set; }
    }
}
