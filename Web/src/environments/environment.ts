// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  baseUrl: 'https://localhost:7247',
  apiUrl: 'https://localhost:7247/api',
  authUrl: 'https://localhost:7054/api',
  reviewServiceUrl: 'https://localhost:7052/api',
  orderServiceUrl: 'https://localhost:7047/api',
  notificationServiceUrl: 'https://localhost:7277/api',
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
