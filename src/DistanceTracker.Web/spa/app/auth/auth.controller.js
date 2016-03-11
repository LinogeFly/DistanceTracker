(function () {
    'use strict';

    angular
        .module('app')
        .controller('authController', ['$scope', authController]);

    function authController($scope) {
        $scope.authView = "login";
        $scope.setAuthView = function (val) { $scope.authView = val; };
    }
})();
