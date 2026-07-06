using Microsoft.AspNetCore.Identity;

namespace Blog.Models;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
}