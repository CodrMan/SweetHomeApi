'use strict';

app.controller('AccountCtrl', ["$scope", '$state', "$window", 'webApi', function ($scope, $state, $window, webApi) {

    delete $window.localStorage.token;

    $scope.login = { Email: '', Password: '' };
    $scope.loginSubmit = function (form) {

        if (checkValid(form)) {

            var data = {
                Email: $scope.login.Email,
                Password: $scope.login.Password
            }
            webApi.post('api/Account/Login', data).then(function (data) {
                $window.localStorage.token = data.Data.Token;
                $window.localStorage.userId = data.Data.UserId;
                window.location = "index.html";
            });
        } 
    };

    $scope.reg = { Email: '', Password: '', ConfirmPassword: '' };
    $scope.registerSubmit = function (form) {

        if (checkValid(form)) {

            var data = {
                Email: $scope.reg.Email,
                Password: $scope.reg.Password,
                ConfirmPassword: $scope.reg.ConfirmPassword
            }
            webApi.post('api/Account/Register', data).then(function(data) {
                $window.localStorage.token = data.Data.Token;
                $window.localStorage.userId = data.Data.UserId;
                window.location = "index.html";
            });
        }
    };

    $scope.forgotPass = { Email: '' };
    $scope.forgotPasswordSubmit = function (form) {

        if (checkValid(form)) {

            var data = {
                Email: $scope.forgotPass.Email
            }
            webApi.post('api/Account/RestorePassword', data).then(function(data) {
                alert("Please check your email!");
                $state.go('login.signin');
            });
        }
    };

    $scope.logOut = function () {
        delete $window.localStorage.token;
            $state.go('login.signin');
    };

    function checkValid(form) {

        var firstError = null;
        if (form.$invalid) {

            var field = null;
            firstError = null;

            for (field in form) {
                if (field[0] != '$') {
                    if (firstError === null && !form[field].$valid) {
                        firstError = form[field].$name;
                    }

                    if (form[field].$pristine) {
                        form[field].$dirty = true;
                    }
                }
            }

            angular.element('.ng-invalid[name=' + firstError + ']').focus();
            alert("The form cannot be submitted because it contains validation errors!");
            return false;
        }
        else {
            return true;
        }       
    }
}]);