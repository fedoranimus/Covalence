let baseConfig = {
    endpoint: 'auth',
    configureEndpoints: ['auth', 'api'],
    loginUrl: 'connect/token',
    signupUrl: 'api/account/register',
    profileUrl: 'api/user',
    useRefreshToken: true,
    loginOnSignup: true,
    logoutOnInvalidtoken: true,
    defaultHeadersForTokenRequests: { 'Content-Type': "application/x-www-form-urlencoded" }
}

export default baseConfig;