'use strict';

app.controller('UserCtrl', ["$scope", "$stateParams", "$window", "flowFactory", 'webApi', function ($scope, $stateParams, $window, flowFactory, webApi) {

    function showUser(id) {
        var url = "api/Users";
        var data = { id: id }
        webApi.get(url, data).then(function(data) {
            $scope.userInfo = data.Data;
            if ($scope.userInfo.PhotoUri == null) {
                $scope.noImage = true;
            } else {
                $scope.noImage = false;
            }
         });
    }

    var imageName = '';
    var userId = $stateParams.isEdit ? $stateParams.userId : $window.localStorage.userId;
    $scope.isAddNewUser = $stateParams.isAddNewUser;
    $scope.activeEditTab = $stateParams.isEdit;
    $scope.userInfo = {};
    $scope.obj = new Flow();

    $scope.addUser = function (form) {

        if (checkValid(form)) {

            var url = "api/Users";
            var data = $scope.userInfo;
            webApi.post(url, data).then(function() {
                alert("user adding");
                $scope.userInfo = {};
            });
        }
    };

    $scope.editUser = function(form) {

        if (checkValid(form)) {

            var url = "api/Users";
            var data = $scope.userInfo;
            webApi.put(url, data).then(function() {
                alert("user editing");
                $scope.userInfo = {};
                $scope.noImage = true;
            });
        }
    };

    $scope.removeImage = function () {

        var url = "removeImage";
        var data = {userId: $scope.userInfo.Id}
        webApi.delete(url, data).then(function () {
            $scope.userInfo.PhotoUri = null;
            $scope.noImage = true;
        });
    };

    $scope.uploadImage = function ($file) {

        var url = "uploadImage";
        var data = {
            'userId': $scope.userInfo.Id,
            'file': $file.file
        }
        webApi.postFile(url, data).then(function (data) {
            imageName = data.Data;
            $scope.showConfirmBtn = true;
        });
    };

    $scope.confirmUploadImage = function () {

        var url = "confirmUploadImage";
        var data = {
            UserId: $scope.userInfo.Id,
            ImageName: imageName
        };
        webApi.post(url, data).then(function () {
            $scope.showConfirmBtn = false;
            alert("Image update!");
        });
    };

    if (!$scope.isAddNewUser) {
        showUser(userId);
    }

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