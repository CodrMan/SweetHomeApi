'use strict';

app.service("webApi", webApi);

function webApi(apiUrl, $q, $http, $state) {
    return {
        get: get,
        post: post,
        put: put,
        "delete": remove,
        postFile: postFile
    };

    function get(url, data) {
        var deffer = $q.defer();
        $http.get(apiUrl + url + getDataString(data))
            .success(function(response) {
                success(response, deffer);
            })
            .error(function(response, code) {
                error(response, deffer, code);
            });
        return deffer.promise;
    }

    function post(url, data) {
        var deffer = $q.defer();
        $http.post(apiUrl + url, data)
            .success(function(response) {
                success(response, deffer);
            })
            .error(function(response, code) {
                error(response, deffer, code);
            });
        return deffer.promise;
    }

    function postFile(url, data) {
        var deffer = $q.defer();

        var fd = new FormData();
        fd.append('file', data.file);
        $http.post(apiUrl + url + getDataString({'userId': data.userId}), fd, {
                transformRequest: angular.identity,
                headers: {'Content-Type': undefined}
            })
            .success(function (response) {
                success(response, deffer);
            })
            .error(function (response, code) {
                error(response, deffer, code);
            });

        return deffer.promise;
    }

    function put(url, data) {
        var deffer = $q.defer();
        $http.put(apiUrl + url, data)
            .success(function(response) {
                success(response, deffer);
            })
            .error(function(response, code) {
                error(response, deffer, code);
            });
        return deffer.promise;
    }

    function remove(url, data) {
        var deffer = $q.defer();
        $http.delete(apiUrl + url + getDataString(data))
            .success(function (response) {
                success(response, deffer);
            })
            .error(function(response, code) {
                error(response, deffer, code);
            });
        return deffer.promise;
    }

    function getDataString(data) {
        if (!data)
            return "";
        var dataString = "?";
        for (var key in data) {
            dataString += key + "=" + data[key] + "&";
        }
        return dataString;
    }

    function success(response, deffer) {
        if (!response) {
            alert("Unexpected server error");
        }
        else if (response.State == "200") {
            deffer.resolve(response);
        }
        else if (response.State == "404") {
            deffer.reject(response);
            alert(response.Message);
        }
        else {
            deffer.reject(response);
            alert(response.Message);
        }
    }

    function error(response, deffer, code) {
        if (!response) {
            alert("Unexpected server error");
        }
        else if (code == 401) {
            alert(response.Message);
            $state.go("login.signin");
        }
        else {
            deffer.reject(response);
            alert(response.Message);
        }
    }
}

webApi.$inject = ['apiURL', '$q', '$http', "$state"];