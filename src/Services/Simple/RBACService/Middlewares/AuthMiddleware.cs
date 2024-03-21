using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetX.ServiceCore;
using System.IdentityModel.Tokens.Jwt;

namespace NetX.RBAC.Service;

public class AuthMiddleware<TRequest, TReponse> : IApplicationMiddleware<GrpcContext<TRequest, TReponse>>
{
    private readonly IAccountService _testService;
    private readonly TokenValidationParameters _tokenValidation;

    public AuthMiddleware(IAccountService testService, TokenValidationParameters tokenValidation)
    {
        _testService = testService;
        _tokenValidation = tokenValidation;
    }

    public async Task InvokeAsync(ApplicationDelegate<GrpcContext<TRequest, TReponse>> next, GrpcContext<TRequest, TReponse> context)
    {
        var jwtToken = context.Client.RequestHeaders.GetValue("Authorization")?.Replace("Bearer ", "");
        if (string.IsNullOrEmpty(jwtToken))
            throw new RpcException(new Status(StatusCode.Unauthenticated, "JWT Token is missing."));
        try
        {
            // 使用JwtSecurityTokenHandler来验证JWT令牌
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(jwtToken, _tokenValidation, out var validatedToken);
            
            // 可以进一步检查principal是否有权访问
            // 例如: var hasAccess = principal.HasClaim(c => c.Type == "SomeClaimType" && c.Value == "SomeValue");

        }
        catch (SecurityTokenException ex)
        {
            // JWT验证失败
            throw new RpcException(new Status(StatusCode.Unauthenticated, $"Invalid token: {ex.Message}"));
        }

        await next?.Invoke(context);
    }
}
