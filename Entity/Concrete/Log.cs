using System.ComponentModel.DataAnnotations;
using Core.Entity;

namespace Entity.Concrete;

public class Log : IEntity<int>, IKiraciEntity
{
    public int Id { get; set; }
    public int OrganizasyonId { get; set; }
    [MaxLength(500)] public string MethodName { get; set; } = null!;
    [MaxLength(500)] public string ClassName { get; set; } = null!;
    [MaxLength(4000)] public string Parameters { get; set; } = null!;
    public DateTime ExecutingTime { get; set; }
    [MaxLength(4000)] public string? ReturnValue { get; set; }
    [MaxLength(4000)] public string? Error { get; set; }
    [MaxLength(500)] public string Username { get; set; } = null!;
}