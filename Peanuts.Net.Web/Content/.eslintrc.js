module.exports = {
    'plugins': [],
    'extends': [
        'eslint:recommended'
    ],
    'env': {
        'browser': true,
        'amd': true,
        'jquery': true,
        'node': true
    },
    'globals': {
        '_': true
    },
    ////http://eslint.org/docs/rules/
    'rules': {
        //possible errors

        //best practices
        ////require the use of === and !==
        'eqeqeq': 2,

        //strict mode
        //require or disallow strict mode directives
        'strict': 2,

        //stylistic issues
        ////require or disallow newlines around var declarations
        'one-var-declaration-per-line': 2,
        ////enforce the consistent use of either backticks, double, or single quotes
        'quotes': [
            'error', 'single'
        ],
        'no-use-before-define': 0
    }

};
