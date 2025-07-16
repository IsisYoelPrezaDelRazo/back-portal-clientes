namespace Application.DTOs
{
    public class UserProfileDto
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required List<string> Roles { get; set; }
        public required string EmpresaId { get; set; }
        public required string TenantId { get; set; }
    }
}
