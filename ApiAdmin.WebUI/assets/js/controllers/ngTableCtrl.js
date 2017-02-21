'use strict';

app.controller('ngTableCtrl', ["$scope", "$filter", "$state", "ngTableParams", "webApi", function ($scope, $filter, $state, ngTableParams, webApi) {
    
    function showData(reload) {
        var url = "api/Users";
        webApi.get(url).then(function (value) {
            if (reload) {
                $scope.myData = value.Data;
                $scope.tableParams.reload();
            }
            else {
                $scope.myData = value.Data;
                $scope.tableParams = new ngTableParams({
                    page: 1,
                    count: 10
                }, {
                    getData: function ($defer, params) {
                        params.total($scope.myData.length);
                        var orderedData = params.sorting() ? $filter('orderBy')($scope.myData, params.orderBy()) : $scope.myData;
                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    }
                });
            }
        });
    };

    showData();

    $scope.addUser = function () {
        $state.go('app.pages.user', {'isEdit': true, 'isAddNewUser': true });
    };

    $scope.editUser = function (pid) {
        $state.go('app.pages.user', { 'userId': pid, 'isEdit': true, 'isAddNewUser': false});
    };

    $scope.deleteUser = function (pid) {
        var url = "api/Users";
        var data = { id: pid }
        webApi.delete(url, data).then(function () {
            showData(true);
        });
    };
}]);
