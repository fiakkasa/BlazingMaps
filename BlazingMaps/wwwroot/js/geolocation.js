// @see https://github.com/darnton/BlazorDeviceInterop
export let Geolocation = {
    getCurrentPosition: async function (options) {
        var result = { position: null, error: null };
        var getCurrentPositionPromise = new Promise((resolve, reject) => {
            if (!navigator.geolocation) {
                reject({ code: 0, message: 'This device does not support geolocation.' });
            } else {
                navigator.geolocation.getCurrentPosition(resolve, reject, options);
            }
        });
        await getCurrentPositionPromise
            .then((position) => { this.mapPositionToResult(position, result) })
            .catch((error) => { this.mapErrorToResult(error, result) });
        return result;
    },
    mapPositionToResult: function (position, result) {
        result.position = {
            coords: {
                latitude: position.coords.latitude,
                longitude: position.coords.longitude,
                altitude: position.coords.altitude,
                accuracy: position.coords.accuracy,
                altitudeAccuracy: position.coords.altitudeAccuracy,
                heading: position.coords.heading,
                speed: position.coords.speed
            },
            timestamp: position.timestamp
        }
    },

    mapErrorToResult: function (error, result) {
        result.error = {
            code: error.code,
            message: error.message
        }
    }

}
