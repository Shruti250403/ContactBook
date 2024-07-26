namespace ClientPhoneBook.Services.Contract
{
    public interface IAuthService
    {
        string RegisterUserService(RegisterViewModel register);
        string LoginUserService(LoginViewModel login);
    }
}
