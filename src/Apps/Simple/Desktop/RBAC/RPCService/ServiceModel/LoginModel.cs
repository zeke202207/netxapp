namespace NetX.RBAC.RPCService
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }
        public string CaptchaId { get; set; }
    }
}
