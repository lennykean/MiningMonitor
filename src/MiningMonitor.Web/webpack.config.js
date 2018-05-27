const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = (env) => {
    const isProd = env && env.prod;
    const client = {
        entry: './ClientApp/main.client.tsx',
        output: {
            filename: 'js/miningmonitor.js',
            path: __dirname + '/wwwroot/',
            publicPath: 'js/'
        },
        devtool: 'sourcemaps',
        resolve: {
            extensions: ['.ts', '.tsx', '.js', '.jsx']
        },
        module: {
            loaders: [
                { test: /\.tsx?$/, loader: 'awesome-typescript-loader' },
                { test: /\.tsx?$/, loader: 'tslint-loader', enforce: 'pre' },
                {
                    test: /\.(scss|sass)$/,
                    loaders: ExtractTextPlugin.extract({
                        use: [
                            {
                                loader: 'css-loader',
                                options: {
                                    sourceMap: true
                                }
                            },
                            {
                                loader: 'sass-loader',
                                options: {
                                    sourceMap: true
                                }
                            }],
                        publicPath: '/css'
                    })
                }
            ]
        },
        plugins: [
            new ExtractTextPlugin("css/mining-monitor.css"),
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': JSON.stringify(isProd ? 'production' : 'development')
            })
        ].concat(isProd ? [
            new webpack.optimize.UglifyJsPlugin({ sourceMap: true })
        ] : [])
    };
    const server = {
        entry: './ClientApp/main.server.tsx',
        output: {
            filename: 'miningmonitor-server.js',
            libraryTarget: 'commonjs',
            path: __dirname + '/ClientApp/dist'
        },
        target: 'node',
        resolve: {
            extensions: ['.ts', '.tsx', '.js', '.jsx']
        },
        module: {
            loaders: [
                { test: /\.tsx?$/, loader: 'awesome-typescript-loader' },
                { test: /\.tsx?$/, loader: 'tslint-loader', enforce: 'pre' }
            ]
        },
        plugins: [
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': JSON.stringify(isProd ? 'production' : 'development')
            })
        ]
    }
    return [client, server];
};