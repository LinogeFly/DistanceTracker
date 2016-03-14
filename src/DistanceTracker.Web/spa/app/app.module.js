(function () {
    'use strict';

    angular
        .module('app', ['ngMessages', 'ngRoute', 'ngAnimate', 'ui.bootstrap'])
        .config(['$routeProvider', '$httpProvider', '$provide', config]);

    function config($routeProvider, $httpProvider, $provide) {
        $routeProvider
            .when('/login', {
                templateUrl: 'templates/auth/auth.html',
                controller: "authController"
            });

        $httpProvider.interceptors.push('interceptorsService');

        $provide.decorator("$exceptionHandler", ['$delegate', 'errorHandlerService', exceptionHandlerDecorator]);

        function exceptionHandlerDecorator($delegate, errorHandlerService) {
            return function (exception, cause) {
                $delegate(exception, cause);
                errorHandlerService.handle(exception);
            };
        }
    }
})();