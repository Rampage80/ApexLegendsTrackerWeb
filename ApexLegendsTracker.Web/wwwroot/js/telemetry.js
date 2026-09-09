// Lazily loads the Application Insights Web SDK and exposes init/trackEvent for Blazor JS interop.
window.apexTelemetry = {
	_ai: null,
	init: function (connectionString) {
		if (!connectionString || this._ai) {
			return;
		}
		var script = document.createElement('script');
		script.src = 'https://js.monitor.azure.com/scripts/b/ai.2.min.js';
		script.crossOrigin = 'anonymous';
		script.onload = function () {
			window.apexTelemetry._ai = new window.Microsoft.ApplicationInsights.ApplicationInsights({
				config: {
					connectionString: connectionString,
					enableAutoRouteTracking: true,
					enableCorsCorrelation: true,
					enableRequestHeaderTracking: true,
					enableResponseHeaderTracking: true
				}
			});
			window.apexTelemetry._ai.loadAppInsights();
		};
		document.head.appendChild(script);
	},
	trackEvent: function (name, properties) {
		if (this._ai) {
			this._ai.trackEvent({ name: name }, properties || {});
		}
	}
};
