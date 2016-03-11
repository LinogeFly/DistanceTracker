(function () {
    'use strict';

    angular
        .module('app')
        .factory('dialogService', ['$uibModal', dialogService]);

    function dialogService($uibModal) {
        var service = {
            confirm: confirm
        };

        return service;

        function confirm(title) {
            return $uibModal.open({
                templateUrl: 'templates/dialogs/confirm.html',
                size: 'sm',
                controller: ["$scope", function ($scope) {
                    $scope.title = title;
                }]
            });
        }
    }
})();