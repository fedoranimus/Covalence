let baseConfig = {
    endpoint: 'auth',
    configureEndpoints: ['auth', 'api'],
    loginUrl: 'token',
    defaultHeadersForTokenRequests: { 'Content-Type': "application/x-www-form-urlencoded" }
}

export default baseConfig;