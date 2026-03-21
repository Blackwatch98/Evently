namespace Evently.Domain.Entities;

public sealed class Role
{
    public int IdRole { get; private set; }
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int Priority { get; private set; }

    private Role()
    {
    }

    public Role(int idRole, string name, string description, int priority)
    {
        IdRole = idRole;
        Name = name;
        Description = description;
        Priority = priority;
    }
}
