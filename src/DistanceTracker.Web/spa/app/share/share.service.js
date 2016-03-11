(function () {
    'use strict';

    angular
        .module('app')
        .service('shareService', ['$http', '$location', shareService]);

    function shareService($http, $location) {
        /* jshint validthis:true */
        this.isSharedLink = isSharedLink;
        this.getSharedLinkData = getSharedLinkData;
        this.pointToUrl = pointToUrl;
        this.waypointsToUrl = waypointsToUrl;
        this.getRootUrl = getRootUrl;

        function isSharedLink() {
            return typeof getSharedLinkData() !== 'undefined';
        }

        function getSharedLinkData() {
            var result = {};

            // Mandatory
            if (typeof $location.search().start === 'undefined') {
                return;
            } else {
                result.start = $location.search().start;
            }

            // Mandatory
            if (typeof $location.search().end === 'undefined') {
                return;
            } else {
                result.end = $location.search().end;
            }

            // Optional
            if (typeof $location.search().progress === 'undefined') {
                result.progress = 0;
            } else {
                result.progress = $location.search().progress;
            }

            // Optional
            if (typeof $location.search().waypoints === 'undefined') {
                result.waypoints = [];
            } else {
                result.waypoints = urlToWaypoints($location.search().waypoints);
            }

            return result;
        }

        function waypointsToUrl(waypoints) {
            if (!waypoints.length) {
                return '';
            }

            return waypoints.map(function (point) {
                return pointToUrl(point);
            }).join('!');
        }

        function urlToWaypoints(str) {
            if (str === '')
                return [];

            var result = [];

            str.split('!').forEach(function (pointStr) {
                result.push(urlToPoint(pointStr));
            });

            return result;
        }

        function pointToUrl(point) {
            return point.lat + ',' + point.lng;
        }

        function urlToPoint(str) {
            var latlng = str.split(',');

            return {
                lat: parseFloat(latlng[0]),
                lng: parseFloat(latlng[1])
            };
        }

        function getRootUrl() {
            var url = $location.protocol() + '://' + $location.host();
            if ($location.port())
                url += ':' + $location.port();

            return url;
        }
    }
})();