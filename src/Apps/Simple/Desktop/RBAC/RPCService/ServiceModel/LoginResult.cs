﻿namespace NetX.RBAC.RPCService
{
    public class LoginResult
    {
        public bool Success { get; set; }

        public string Token { get; set; }

        //public string RefreshToken { get; set; }

        public string Message { get; set; }
    }
}
