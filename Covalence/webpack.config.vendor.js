var path = require('path');
var webpack = require('webpack');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var extractCSS = new ExtractTextPlugin('vendor.css');

module.exports = ({ prod } = {}) => {
    const isDevBuild = !prod;
    
    return [{
        stats: { modules: false },
        mode: isDevBuild ? 'development' : 'production',
        resolve: {
            extensions: ['.js']
        },
        module: {
            rules: [
                { test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, loader: 'url-loader?limit=100000' },
                { test: /\.css$/, use: extractCSS.extract([isDevBuild ? 'css-loader' : 'css-loader?minimize']) }
            ]
        },
        entry: {
            vendor: [
                'aurelia-event-aggregator',
                'aurelia-fetch-client',
                'aurelia-framework',
                'aurelia-history-browser',
                'aurelia-logging-console',
                'aurelia-pal-browser',
                'aurelia-polyfills',
                'aurelia-route-recognizer',
                'aurelia-router',
                'aurelia-templating-binding',
                'aurelia-templating-resources',
                'aurelia-templating-router',
                'aurelia-api',
                'aurelia-authentication',
                'aurelia-validation',
                'aurelia-store',
                'aurelia-google-maps',
                'bulma/css/bulma.css',
                'bulma-tooltip/dist/css/bulma-tooltip.min.css',
                'font-awesome/css/font-awesome.css',
                'markdown-it',
                'markdown-it-katex',
                'rxjs'
            ],
        },
        output: {
            path: path.join(__dirname, 'wwwroot', 'dist'),
            publicPath: '/dist/',
            filename: '[name].js',
            library: '[name]_[hash]',
        },
        plugins: [
            extractCSS,
            new webpack.DllPlugin({
                path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
                name: '[name]_[hash]'
            })
        ]
    }]
};
