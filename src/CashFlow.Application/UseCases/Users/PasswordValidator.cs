using CashFlow.Exception;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CashFlow.Application.UseCases.Users;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string PASSWORD_MESSAGE_KEY = "ErrorMessage";

    protected override string GetDefaultMessageTemplate(string propertyName)
    {
        return $"{{{PASSWORD_MESSAGE_KEY}}}";
    }
    
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password) is false 
         && password.Length >= 8 
         && UpperCaseLetter().IsMatch(password) 
         && LowerCaseLetter().IsMatch(password) 
         && Number().IsMatch(password) 
         && SpecialCharacter().IsMatch(password))
            return true;
        
        context.MessageFormatter.AppendArgument(PASSWORD_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
        return false;
    }

    public override string Name => "PasswordValidator";

    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();
    
    [GeneratedRegex(@"[a-z]+")]
    private static partial Regex LowerCaseLetter();
    
    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex Number();
    
    [GeneratedRegex(@"[\!\?\@\*\.\$]+")]
    private static partial Regex SpecialCharacter();
}
