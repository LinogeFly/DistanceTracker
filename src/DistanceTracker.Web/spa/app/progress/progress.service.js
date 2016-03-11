(function () {
    'use strict';

    angular
        .module('app')
        .service('progressService', ['$http', '$log', '$q', '$rootScope', 'constantsService', 'shareService', progressService]);

    function progressService($http, $log, $q, $rootScope, constantsService, shareService) {
        /* jshint validthis:true */
        this.addProgress = addProgress;
        this.getProgress = getProgress;
        this.deleteProgress = deleteProgress;
        this.getHistory = getHistory;

        var cachedProgress,
            cachedHistory;

        function getProgress() {
            var deferred = $q.defer();

            // Check if in cache
            if (typeof cachedProgress !== 'undefined') {
                deferred.resolve(cachedProgress);
                return deferred.promise;
            }

            // Check if in GET parameters
            var sharedLinkData = shareService.getSharedLinkData();
            if (typeof sharedLinkData !== 'undefined') {
                cachedProgress = sharedLinkData.progress;
                deferred.resolve(cachedProgress);
                return deferred.promise;
            }

            // Get from API
            $http.get('api/progress')
                .success(function (progress) {
                    cachedProgress = progress;
                    deferred.resolve(progress);
                }).error(function (msg, code) {
                    deferred.reject(msg);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }

        function addProgress(distance) {
            var deferred = $q.defer();

            $http.post('api/progress', distance)
                .success(function (response) {
                    cachedProgress = undefined;
                    cachedHistory = undefined;
                    $rootScope.$broadcast(constantsService.Events.Progress.CHANGED);
                    deferred.resolve();
                })
                .error(function (msg, code) {
                    deferred.reject(msg);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }

        function deleteProgress() {
            var deferred = $q.defer();

            $http.delete('api/progress')
                .success(function (response) {
                    cachedProgress = undefined;
                    cachedHistory = undefined;
                    $rootScope.$broadcast(constantsService.Events.Progress.DELETED);
                    deferred.resolve();
                })
                .error(function (msg, code) {
                    deferred.reject(msg);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }

        function getHistory() {
            var deferred = $q.defer();

            // Check if in cache
            if (typeof cachedHistory !== 'undefined') {
                deferred.resolve(cachedHistory);
                return deferred.promise;
            }

            // Get from API
            $http.get('api/history')
                .success(function (history) {
                    cachedHistory = history;
                    deferred.resolve(history);
                }).error(function (msg, code) {
                    deferred.reject(msg);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }
    }
})();