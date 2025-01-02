public class TokenService
{
    private string _currentToken;
    public string CurrentToken 
    { 
        get => _currentToken;
        set => _currentToken = value;
    }
} 