let baseConfig = {
    endpoint: 'auth',
    configureEndpoints: ['auth', 'api'],
    loginUrl: 'connect/token',
    profileUrl: 'api/user',
    defaultHeadersForTokenRequests: { 'Content-Type': "application/x-www-form-urlencoded" }
}

export default baseConfig;