(function () {
    'use strict';

    angular
        .module('app')
        .controller('historyController', ['$scope', 'constantsService', 'progressService', 'errorHandlerService', historyController]);

    function historyController($scope, constantsService, progressService, errorHandlerService) {
        $scope.history = [];

        function bindHistory() {
            progressService.getHistory()
                .then(function (data) {
                    $scope.history = data;
                }, function (errorCode) {
                    errorHandlerService.handle(errorCode);
                });
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
