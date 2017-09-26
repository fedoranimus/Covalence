let baseConfig = {
    endpoint: 'auth',
    configureEndpoints: ['auth', 'api'],
    loginUrl: 'connect/token',
    signupUrl: 'api/account/register',
    profileUrl: 'api/user',
    defaultHeadersForTokenRequests: { 'Content-Type': "application/x-www-form-urlencoded" }
}

export default baseConfig;