(function () {
    'use strict';

    angular
        .module('app', ['ngMessages', 'ngRoute', 'ngAnimate', 'ui.bootstrap'])
        .config(['$routeProvider', '$httpProvider', config]);

    function config($routeProvider, $httpProvider) {
        $routeProvider
            .when('/login', {
                templateUrl: 'templates/auth/auth.html',
                controller: "authController"
            });

        $httpProvider.interceptors.push('interceptorsService');
    }
})();