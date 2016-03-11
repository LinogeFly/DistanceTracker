(function () {
    'use strict';

    angular
        .module('app')
        .controller('progressController', ['$scope', 'progressService', progressController]);

    function progressController($scope, progressService) {
        $scope.progress = {
            distance: null,
            isSaving: false
        };

        $scope.saveProgress = function () {
            $scope.progress.isSaving = true;
            progressService.addProgress($scope.progress.distance)
                .then(
                    function (data) {
                        $scope.progress.isSaving = false;
                        $scope.progress.distance = null;
                    }
                );
        };
    }
})();
