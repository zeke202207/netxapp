namespace NetX.RBAC.RPCService
{
    public interface IAccountRPC
    {
        Task<CaptchaModel> GetCaptchaAsync();

        Task<LoginResult> LoginAsync(LoginModel loginModel);

        Task LogoutAsync();
    }
}
