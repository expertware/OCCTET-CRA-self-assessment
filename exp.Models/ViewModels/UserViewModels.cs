namespace exp.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; } = default!;
        public string? Email { get; set; }
        public string? Name { get; set; }
        public List<string?>? Roles { get; set; }
    }
    public class UserDetailsForEditViewModel
    {
        public string Id { get; set; } = default!;
        public string? Email { get; set; }
        public string? Name { get; set; }
        public List<RoleDetailsViewModel>? Roles { get; set; }
    }
    public class UpdateUserDetailsViewModel
    {
        public string Id { get; set; } = default!;
        public string? Name { get; set; }
        public List<string?>? RoleIds { get; set; }
    }
    public class FilterUsers : PagingFilterViewModel
    {
        public string? SearchText { get; set; }
        public string? RoleId { get; set; }
    }
    public class RoleDetailsViewModel
    {
        public string Id { get; set; } = default!;
        public string? Name { get; set; }
    }
}
