// App Insights frontend
import { ApplicationInsights } from '@microsoft/applicationinsights-web'

const clickPluginInstance = new Microsoft.ApplicationInsights.ClickAnalyticsPlugin();
const appInsights = new ApplicationInsights({ config: {
   connectionString: window.appInsights.connectionString,
   extensions: [ clickPluginInstance ],
   extensionConfig: { [clickPluginInstance.identifier] : {
      autoCapture : true,
      dataTags: { useDefaultContentNameOrId: true }
   } },
} });

appInsights.loadAppInsights();
appInsights.setAuthenticatedUserContext(window.appInsights.authenticatedUserId, null, true);
appInsights.trackPageView();

// Track clicks on GOV.UK details components (by CSS class)
const trackedClasses = ['govuk-details__summary-text'];
const trackedSelector = trackedClasses.map(c => '.' + c).join(',');

document.addEventListener('click', function (e) {
   if (!(e.target instanceof Element)) return;

   // Keyboard activation (Enter/Space) targets the <summary>, not the inner text span
   const summary = e.target.closest('.govuk-details__summary');
   const el = e.target.closest(trackedSelector) || summary?.querySelector(trackedSelector);
   if (!el) return;

   const details = el.closest('details');
   appInsights.trackEvent({
      name: 'Accordion click event',
      properties: {
         className: trackedClasses.find(c => el.classList.contains(c)),
         text: el.innerText?.trim(),
         page: window.location.pathname,
         // click fires before the toggle, so the new state is the opposite of the current one
         action: details ? (details.open ? 'collapse' : 'expand') : undefined
      }
   });
});
