'use strict';

const _ = require('lodash');
const path = require('path');
const fs = require('fs');

module.exports = function () {
    const files = ['app/data/global.json'];

    let data = {};
    for (let i in files) {
        const jsonPath = path.resolve(process.env.PWD, PATH_CONFIG.src, PATH_CONFIG.pug.src, files[i]);
        const jsonData = JSON.parse(fs.readFileSync(jsonPath));
        data = _.assign(data, jsonData);
    }

    data['packageJson'] = JSON.parse(fs.readFileSync(path.resolve(process.env.PWD, 'package.json')));

    return data;
};
