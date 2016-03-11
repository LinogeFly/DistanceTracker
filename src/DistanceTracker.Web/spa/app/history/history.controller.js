(function () {
    'use strict';

    angular
        .module('app')
        .controller('historyController', ['$scope', 'constantsService', 'progressService', historyController]);

    function historyController($scope, constantsService, progressService) {
        $scope.history = [];

        function bindHistory() {
            progressService.getHistory()
                .then(
                    function (data) {
                        $scope.history = data;
                    }
                );
        }

        $scope.$on(constantsService.Events.Progress.CHANGED, function () {
            bindHistory();
        });

        $scope.$on(constantsService.Events.Progress.DELETED, function () {
            bindHistory();
        });

        bindHistory();
    }
})();
