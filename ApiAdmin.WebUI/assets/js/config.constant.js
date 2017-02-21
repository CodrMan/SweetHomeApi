'use strict';

app.constant('apiURL', 'http://192.168.0.196:9999/');

/**
 * Config constant
 */
app.constant('APP_MEDIAQUERY', {
    'desktopXL': 1200,
    'desktop': 992,
    'tablet': 768,
    'mobile': 480
});
app.constant('JS_REQUIRES', {
    //*** Scripts
    scripts: {
        'ngTableCtrl':     'assets/js/controllers/ngTableCtrl.js',
        'validationCtrl':  'assets/js/controllers/validationCtrl.js',
        'userCtrl':        'assets/js/controllers/userCtrl.js',
        'accountCtrl':       'assets/js/controllers/accountCtrl.js',
        'webApi':          'assets/js/services/webApi.js',
        'htmlToPlaintext': 'assets/js/filters/htmlToPlaintext.js',

        'modernizr':      ['bower_components/components-modernizr/modernizr.js']
},
    modules: [{
        name: 'ngTable',
        files: ['bower_components/ng-table/dist/ng-table.min.js',
                'bower_components/ng-table/dist/ng-table.min.css']
    }, {
        name: 'flow',
        files: ['bower_components/ng-flow/dist/ng-flow-standalone.min.js']
    }, {
        name: 'uiSwitch',
        files: ['bower_components/angular-ui-switch/angular-ui-switch.min.js',
                'bower_components/angular-ui-switch/angular-ui-switch.min.css']
    }, ]
});
