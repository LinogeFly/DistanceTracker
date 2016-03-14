(function () {
    'use strict';

    angular
        .module('app')
        .controller('progressController', ['$scope', 'progressService', 'errorHandlerService', progressController]);

    function progressController($scope, progressService, errorHandlerService) {
        $scope.progress = {
            distance: null,
            isSaving: false
        };

        $scope.saveProgress = function () {
            $scope.progress.isSaving = true;
            progressService.addProgress($scope.progress.distance)
                .then(function (data) {
                    $scope.progress.isSaving = false;
                    $scope.progress.distance = null;
                }, function (errorCode) {
                    errorHandlerService.handle(errorCode);
                });
        };
    }
})();
