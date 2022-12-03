using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TerminalCommandsApi.Domain.Enums;

namespace TerminalCommandsApi.Domain.Models;

public class Command
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Platform Platform { get; set; }
    public IdentityUser CreatedBy { get; set; } = null!;
    public required string CreatedById { get; set; }

}