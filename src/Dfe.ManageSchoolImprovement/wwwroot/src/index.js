// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

import { initAll as govukFrontendInitAll } from 'govuk-frontend'

import $ from 'jquery'
import { initAll as MOJFrontendInitAll } from '@ministryofjustice/frontend'

window.$ = $
govukFrontendInitAll()
MOJFrontendInitAll()
