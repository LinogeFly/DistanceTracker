(function () {
    'use strict';

    angular
        .module('app')
        .service('routeService', ['$http', '$log', '$q', '$rootScope', 'constantsService', 'shareService', routeService]);

    function routeService($http, $log, $q, $rootScope, constantsService, shareService) {
        /* jshint validthis:true */
        this.getRoute = getRoute;
        this.saveRoute = saveRoute;
        this.deleteRoute = deleteRoute;

        var cachedRoute;

        function getRoute() {
            var deferred = $q.defer();

            // Check if in cache
            if (typeof cachedRoute !== 'undefined') {
                deferred.resolve(cachedRoute);
                return deferred.promise;
            }

            // Check if in GET parameters
            var sharedLinkData = shareService.getSharedLinkData();
            if (typeof sharedLinkData !== 'undefined') {
                var route = {
                    startLocation: sharedLinkData.start,
                    endLocation: sharedLinkData.end,
                    waypoints: sharedLinkData.waypoints
                };
                cachedRoute = route;
                deferred.resolve(cachedRoute);
                return deferred.promise;
            }

            // Get from API
            $http.get('api/route')
                .success(function (route) {
                    cachedRoute = route;
                    deferred.resolve(route);
                }).error(function (msg, code) {
                    deferred.reject(msg);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }

        function saveRoute(googleRoute) {
            var deferred = $q.defer();

            var postRoute = {
                startLocation: googleRoute.legs[0].start_location,
                endLocation: googleRoute.legs[0].end_location,
                waypoints: googleRoute.legs[0].via_waypoints
            };

            $http.post('api/route', postRoute)
                .success(function () {
                    cachedRoute = undefined;
                    $rootScope.$broadcast(constantsService.Events.Route.CHANGED);
                    deferred.resolve();
                }).error(function (msg, code) {
                    deferred.reject(msg);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }

        function deleteRoute() {
            var deferred = $q.defer();

            $http.delete('api/route')
                .success(function () {
                    cachedRoute = undefined;
                    $rootScope.$broadcast(constantsService.Events.Route.DELETED);
                    deferred.resolve();
                })
                .error(function (msg, code) {
                    deferred.reject(msg);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }
    }
})();