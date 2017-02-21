var app = angular.module('clipApp', ['clip-two']);
app.run(['$rootScope', '$state', '$stateParams', '$window', 'sessionService', function ($rootScope, $state, $stateParams, $window, sessionService) {

    // Set some reference to access them from any scope
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

    // GLOBAL APP SCOPE
    // set below basic information
    $rootScope.app = {
        name: 'Admin Application', // name of your project
        author: 'CheckMan', // author's name or company name
        description: 'Angular Bootstrap Admin Template', // brief description
        version: '1.0', // current version
        year: ((new Date()).getFullYear()), // automatic current year (for copyright information)
        isMobile: (function () {// true if the browser is a mobile device
            var check = false;
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                check = true;
            };
            return check;
        })(),
        layout: {
            isNavbarFixed: true, //true if you want to initialize the template with fixed header
            isSidebarFixed: true, // true if you want to initialize the template with fixed sidebar
            isSidebarClosed: false, // true if you want to initialize the template with closed sidebar
            isFooterFixed: false, // true if you want to initialize the template with fixed footer
            theme: 'theme-1', // indicate the theme chosen for your project
            logo: 'assets/images/logo.png', // relative path of the project logo
        }
    };
    $rootScope.user = {};

    $rootScope.$on('$stateChangeStart',
          function (event, toState, toParams, fromState, fromParams) {
              sessionService.checkAccess(event, toState, toParams, fromState, fromParams);
          }
        );
}]);


app.config(['cfpLoadingBarProvider',
function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeBar = true;
    cfpLoadingBarProvider.includeSpinner = false;

}]);


app.service('sessionService', ['$rootScope', '$state', function ($rootScope, $state) {

    this.checkAccess = function (event, toState, toParams, fromState, fromParams) {
        if (toState.data.requireLogin && $rootScope.isAuthenticated == false) {
            $rootScope.$evalAsync(function () {
                $state.go('login.signin');
            });
        }
    };
}]);


app.factory('authInterceptor', function ($rootScope, $q, $window) {
    return {
        request: function (config) {
            config.headers = config.headers || {};
            if ($window.localStorage.token) {
                config.headers.Authorization = 'Bearer ' + $window.localStorage.token;
                $rootScope.isAuthenticated = true;
            } else {
                $rootScope.isAuthenticated = false;
            }
            return config;
        },
        response: function (response) {
            if (response.status === 401) {
                alert("user is not authenticated");
            }
            return response || $q.when(response);
        }
    };
});


app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
});
