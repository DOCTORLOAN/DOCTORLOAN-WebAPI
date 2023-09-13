namespace DoctorLoan.Domain.Conts.Regex;
public static class CommonRegex
{
    public const string Email = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
    public const string Phone = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
    public const string IdentityCard = @"[1-9]{9,14}";
    public const string CapitalLetters = @"[A-Z]";
    public const string LowercaseLetters = @"[a-z]";
    public const string ContainDigits = @"\d";
    public const string NoSpace = @"^[^£# “”]*$";
    public const string IsDigit = @"^\d+$";
    /// <summary>
    /// ^ - start of string anchor
    /// ?!.*DontMatchThis) - a negative lookahead checking if there are any 0 or more characters (matched with greedy .* subpattern - NOTE a lazy .*? version (matching as few characters as possible before the next subpattern match) might get the job done quicker if DontMatchThis is expected closer to the string start) followed with DontMatchThis
    /// .* - any zero or more characters, as many as possible, up to
    /// $ - the end of string (see Anchor Characters: Dollar ($)).
    /// </summary>
    public const string NotAllowedHtml = @"<([a-z]+) *[^/]*?>"; // @"^(?!.*DontMatchThis).*$";
}
