using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ProcessManagement.Infrastructure.Auth.AuthenticationHandlers
{
    class SessionAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public bool Cookie { get; set; }
    }

    class SessionAuthenticationHandler : SignInAuthenticationHandler<SessionAuthenticationSchemeOptions>
    {
        public const string SchemeName = "SessionAuthenticationScheme";
        private readonly ISessionStore _sessionStore;

        public SessionAuthenticationHandler(
            IOptionsMonitor<SessionAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISessionStore sessionStore)
            : base(options, logger, encoder, clock)
        {
            _sessionStore = sessionStore;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var sid = Request.Cookies.FirstOrDefault(c => c.Key == "SID").Value;
            if (string.IsNullOrEmpty(sid))
            {
                return await Task.FromResult(AuthenticateResult.Fail("No session cookie"));
            }

            var sessionModel = await _sessionStore.RetrieveAsync(sid);

            if (sessionModel == null) {
                return await Task.FromResult(AuthenticateResult.Fail("Session not found"));
            }

            var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, sessionModel.UserId),
                    new Claim(ClaimTypes.Email, ""),
                    new Claim(ClaimTypes.Name, "") };

            var claimsIdentity = new ClaimsIdentity(claims,
                            nameof(SessionAuthenticationHandler));

            var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var sid = Guid.NewGuid().ToString();
            Response.Cookies.Append("SID", sid);
            var sessionModel = new SessionModel()
            {
                Id = sid,
                UserId = "",
            };
            _sessionStore.StoreAsync(sessionModel);

            return Task.CompletedTask;
        }

        protected override Task HandleSignOutAsync(AuthenticationProperties properties)
        {
            Response.Cookies.Delete("SID");
            var sid = Request.Cookies.FirstOrDefault(c => c.Key == "SID").Value;
            _sessionStore.RemoveAsync(sid);
            return Task.CompletedTask;
        }
    }

}
