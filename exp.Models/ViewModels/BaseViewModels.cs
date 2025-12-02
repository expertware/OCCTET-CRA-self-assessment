using System.ComponentModel;

namespace exp.Models.ViewModels
{
    public class BaseViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class PagingFilterViewModel
    {
        [DefaultValue(10)]
        public int PageSize { get; set; }
        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [DefaultValue("")]
        public string? OrderBy { get; set; }
    }
    public class PagingViewModel<T>
    {
        public int Count { get; set; }
        public int NumberOfPages { get; set; }
        public List<T> Items { get; set; } = new(new List<T>());
    }
    public class ImageViewModel
    {
        [ImageTypeValidation]
        public string? ImgBase64 { get; set; }
    }
}
