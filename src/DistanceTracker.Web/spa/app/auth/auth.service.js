(function () {
    'use strict';

    angular
        .module('app')
        .service('authService', ['$http', '$log', '$q', '$rootScope', 'constantsService', authService]);

    function authService($http, $log, $q, $rootScope, constantsService) {
        /* jshint validthis:true */
        this.logIn = logIn;
        this.logOut = logOut;
        this.register = register;
        this.currentUser = currentUser;

        var cachedUser;

        function logIn(userCreds) {
            var deferred = $q.defer();

            $http.post('api/user/login', userCreds)
                .success(function (response) {
                    $rootScope.$broadcast(constantsService.Events.Auth.LOGGED_IN);
                    deferred.resolve();
                })
                .error(function (msg, code) {
                    deferred.reject(code);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }

        function logOut() {
            var deferred = $q.defer();

            $http.get('api/user/logout')
                .success(function (response) {
                    cachedUser = undefined;
                    deferred.resolve();
                })
                .error(function (msg, code) {
                    deferred.reject(code);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }

        function register(user) {
            var deferred = $q.defer();

            $http.post('api/user/register', user)
                .success(function (response) {
                    deferred.resolve();
                })
                .error(function (msg, code) {
                    deferred.reject(code);
                    $log.error(msg, code);
                });

            return deferred.promise;
        }

        function currentUser() {
            var deferred = $q.defer();

            // Check if in cache
            if (typeof cachedUser !== 'undefined') {
                deferred.resolve(cachedUser);
                return deferred.promise;
            }

            // Get from API
            $http.get('api/user')
                .success(function (user) {
                    cachedUser = user;
                    deferred.resolve(user);
                })
                .error(function (msg, code) {
                    if (code !== 401)
                        $log.error(msg, code);

                    deferred.reject(code);
                });

            return deferred.promise;
        }
   }
})();