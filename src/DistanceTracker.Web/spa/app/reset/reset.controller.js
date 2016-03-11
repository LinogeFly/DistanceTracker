(function () {
    'use strict';

    angular
        .module('app')
        .controller('resetController', ['$scope', 'progressService', 'routeService', 'dialogService', 'constantsService', resetController]);

    function resetController($scope, progressService, routeService, dialogService, constantsService) {
        $scope.reset = {
            isResetingProgress: false,
            isResetingRoute: false,
        };

        $scope.resetProgress = function () {
            dialogService.confirm(constantsService.Dialog.DELETE_PROGRESS_CONFIRM_TITLE).result.then(function () {
                $scope.reset.isResetingProgress = true;
                progressService.deleteProgress().then(function (data) {
                    $scope.reset.isResetingProgress = false;
                });
            });
        };

        $scope.resetRoute = function () {
            dialogService.confirm(constantsService.Dialog.DELETE_ROUTE_CONFIRM_TITLE).result.then(function () {
                $scope.reset.isResetingRoute = true;
                routeService.deleteRoute().then(function (data) {
                    $scope.reset.isResetingRoute = false;
                });
            });
        };
    }
})();
