using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Application.DTOs;
using System.Collections.Generic;

namespace Application.UseCases
{
    public class UserProfileService
    {
        public UserProfileDto GetProfile(HttpContext context)
        {
            var user = context.User;
            // Log all user claims
            System.Console.WriteLine("User Claims:");
            foreach (var claim in user.Claims)
            {
                System.Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            }
            if (user == null || user.Identity?.IsAuthenticated != true)
                throw new System.Exception("The user is not authenticated or the token is invalid.");

            string? userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;
            string? email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value;
            string? fullName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "name")?.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "roles" || c.Type == "role").Select(c => c.Value).ToList();
            string? empresaId = user.Claims.FirstOrDefault(c => c.Type == "empresaId")?.Value;
            string? tenantId = user.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

            // if (string.IsNullOrEmpty(userId))
            //     throw new System.Exception("Claim 'sub' (userId) is missing in the token.");
            // if (string.IsNullOrEmpty(email))
            //     throw new System.Exception("Claim 'email' is missing in the token.");
            // if (string.IsNullOrEmpty(fullName))
            //     throw new System.Exception("Claim 'name' (fullName) is missing in the token.");
            // if (roles == null || roles.Count == 0)
            //     throw new System.Exception("Claim 'roles' is missing in the token.");
            // if (string.IsNullOrEmpty(empresaId))
            //     throw new System.Exception("Custom claim 'empresaId' is missing in the token.");
            // if (string.IsNullOrEmpty(tenantId))
            //     throw new System.Exception("Claim 'tid' (tenantId) is missing in the token.");

            return new UserProfileDto
            {
                UserId = userId,
                Email = email,
                FullName = fullName,
                Roles = roles,
                EmpresaId = empresaId,
                TenantId = tenantId
            };
        }
    }
}
