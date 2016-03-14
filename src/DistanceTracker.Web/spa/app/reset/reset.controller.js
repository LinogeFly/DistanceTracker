(function () {
    'use strict';

    angular
        .module('app')
        .controller('resetController', ['$scope', 'progressService', 'routeService', 'dialogService', 'constantsService', 'errorHandlerService', resetController]);

    function resetController($scope, progressService, routeService, dialogService, constantsService, errorHandlerService) {
        $scope.reset = {
            isResetingProgress: false,
            isResetingRoute: false,
        };

        $scope.resetProgress = function () {
            dialogService.confirm(constantsService.Dialog.DELETE_PROGRESS_CONFIRM_TITLE).result.then(function () {
                $scope.reset.isResetingProgress = true;
                progressService.deleteProgress().then(
                    function (data) {
                        $scope.reset.isResetingProgress = false;
                    }, function (errorCode) {
                        errorHandlerService.handle(errorCode);
                    });
            });
        };

        $scope.resetRoute = function () {
            dialogService.confirm(constantsService.Dialog.DELETE_ROUTE_CONFIRM_TITLE).result.then(function () {
                $scope.reset.isResetingRoute = true;
                routeService.deleteRoute().then(
                    function (data) {
                        $scope.reset.isResetingRoute = false;
                    }, function (errorCode) {
                        errorHandlerService.handle(errorCode);
                    });
            });
        };
    }
})();
