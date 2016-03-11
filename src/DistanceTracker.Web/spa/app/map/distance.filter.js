(function () {
    'use strict';

    angular
        .module('app')
        .filter('distance', ['$filter', distanceFilter]);

    function distanceFilter($filter) {
        return function (input, measure) {
            if (typeof input === 'undefined' || input === null) {
                return '';
            }

            if (typeof measure === 'undefined' || measure === 'km') {
                return Math.round(input / 1000);
            }

            return '';
        };
    }
})();