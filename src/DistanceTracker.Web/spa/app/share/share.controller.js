(function () {
    'use strict';

    angular
        .module('app')
        .controller('shareController', ['$scope', '$location', '$q', 'constantsService', 'progressService', 'routeService', 'shareService', 'errorHandlerService', shareController]);

    function shareController($scope, $location, $q, constantsService, progressService, routeService, shareService, errorHandlerService) {
        $scope.share = {
            permaLink: ''
        };

        function bindPermaLink() {
            $q.all([progressService.getProgress(), routeService.getRoute()])
                .then(function (data) {
                    $scope.share.permaLink = getPermaLink(data[0], data[1]);
                }, function (errorCode) {
                    errorHandlerService.handle(errorCode);
                });
        }

        function getPermaLink(progress, route) {
            var result = shareService.getRootUrl() +
                '/#/?progress=' + progress +
                '&start=' + shareService.pointToUrl(route.startLocation) +
                '&end=' + shareService.pointToUrl(route.endLocation);

            if (route.waypoints !== null) {
                result += '&waypoints=' + shareService.waypointsToUrl(route.waypoints);
            }

            return result;
        }

        $scope.$on(constantsService.Events.Route.CHANGED, function () {
            bindPermaLink();
        });

        $scope.$on(constantsService.Events.Route.DELETED, function () {
            bindPermaLink();
        });

        $scope.$on(constantsService.Events.Progress.CHANGED, function () {
            bindPermaLink();
        });

        $scope.$on(constantsService.Events.Progress.DELETED, function () {
            bindPermaLink();
        });

        bindPermaLink();
    }
})();
