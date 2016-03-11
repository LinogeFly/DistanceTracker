(function () {
    'use strict';

    angular
        .module('app')
        .controller('signupController', ['$scope', 'authService', 'constantsService', signupController]);

    function signupController($scope, authService, constantsService) {
        $scope.lastError = '';
        $scope.isBusy = false;

        $scope.onRegisterClick = function () {
            $scope.lastError = '';
            $scope.isBusy = true;

            authService.register($scope.userModel)
                .then(function () {
                    $scope.isBusy = false;
                    $scope.setAuthView('login');
                }, function (errorCode) {
                    $scope.isBusy = false;
                    if (errorCode === 409)
                        $scope.lastError = constantsService.Errors.Auth.USERNAME_ALREADY_EXISTS;
                    else
                        $scope.lastError = constantsService.Errors.Common.UNEXPECTED_ERROR;
                });
        };
    }
})();
