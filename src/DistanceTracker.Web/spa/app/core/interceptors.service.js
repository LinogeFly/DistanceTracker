(function () {
    'use strict';

    angular
        .module('app')
        .factory('interceptorsService', ['$q', '$location', interceptorsService]);

    function interceptorsService($q, $location) {
        return {
            'responseError': function (rejection) {
                if (rejection.status === 401)
                    $location.path('/login');

                return $q.reject(rejection);
            }
        };
    }
})();