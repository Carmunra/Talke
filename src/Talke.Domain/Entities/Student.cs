using Talke.Domain.Common;
using Talke.Domain.Enums;

namespace Talke.Domain.Entities;

public class Student : Entity
{
    public string Name { get; private set;}
    
    public string Email { get; private set; }

    public ProficiencyLevel? Level { get; private set; }

    public LearningStyle? LearningStyle { get; private set; }

    public bool HasCompletedDiagnostic => Level is not null && LearningStyle is not null;

    public Student(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome é obrigatório.", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório.", nameof(email));

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
    }

    public void CompleteDiagnostic(ProficiencyLevel level, LearningStyle learningStyle)
    {
        Level = level;
        LearningStyle = learningStyle;
    }
}