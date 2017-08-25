'use strict';

module.exports = (env) => {
    return (webpack) => {
        let plugins = [];
        plugins.push(new webpack.IgnorePlugin(/jsdom/));

        return plugins;
    };
};
