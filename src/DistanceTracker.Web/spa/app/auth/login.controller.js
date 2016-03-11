(function () {
    'use strict';

    angular
        .module('app')
        .controller('loginController', ['$scope', 'authService', 'constantsService', loginController]);

    function loginController($scope, authService, constantsService) {
        $scope.lastError = '';
        $scope.isBusy = false;
        $scope.userModel = {
            rememberMe: true
        };

        $scope.onLogInClick = function () {
            $scope.lastError = '';
            $scope.isBusy = true;

            authService.logIn($scope.userModel)
                .then(function () {
                    $scope.isBusy = false;
                }, function (errorCode) {
                    $scope.isBusy = false;
                    if (errorCode === 400)
                        $scope.lastError = constantsService.Errors.Auth.INVALID_USERNAME_OR_PASSWORD;
                    else
                        $scope.lastError = constantsService.Errors.Common.UNEXPECTED_ERROR;
                });
        };
    }
})();
