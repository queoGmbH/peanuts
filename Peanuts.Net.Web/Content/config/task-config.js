'use strict';

module.exports = {
    options: {
        cleanFirst: true,
        reportSizes: false,
        watch: true
    },
    browserSync: {
        open: false,
        server: {
            baseDir: 'dist'
        }
    },
    javascripts: {
        entries: {
            app: ['./app.js'],
            inject: ['./inject.js'],
            styleguide: ['./styleguide.js'],
        },
        extensions: ['js', 'json'],
        extractSharedJs: false,
        hotModuleReplacement: false,
        deployUncompressed: true,
        devtool: 'inline-source-map',
        provide: {
            '$': 'jquery',
            'jQuery': 'jquery'
        },
        plugins: require('./lib/add-plugins')()
    },
    stylesheets: {
        autoprefixer: {
            browsers: ['last 3 version']
        },
        type: 'less',
        extensions: 'less',
        excludeFolders: ['app'],
        deployUncompressed: true
    },
    pug: {
        getData: require('./lib/pug-data'),
        htmlmin: {},
        options: {},
        extensions: ['pug', 'json'],
        excludeFolders: ['atomic', 'helper', 'data']
    },
    images: {
        extensions: '*'
    },
    fonts: {
        extensions: ['woff2', 'woff', 'eot', 'ttf', 'svg']
    }
};
