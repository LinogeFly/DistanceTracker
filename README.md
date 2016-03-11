# About

It's a small web site for practicing ASP.NET 5 (vNext), AngularJS, Entity Framework 7 and other stuff.

## What it actually does

In a nutshell it's a web site for my wife who likes to run :)

She asked me to make a tool where you could create a long route in Google Maps, say from Helsinki to Paris. And then after every run you would submit a distance you had run. Throughout a long turm you can see were you would be if you were to actually run that route. So she could see if she can make it from Helsinki to Paris in a year, for example.

The web site can be used by anyone who wants. It's available [here](http://distancetracker.azurewebsites.net/).

# TODO

## Major tasks
- Error handling on SPA side
- Tests for SPA side
- Progress data should load in parallel instead of being loaded after route data.

## Minor tasks and bugs
- GET request to "api/progress" gets triggered twice after deleting progress.
- Progress marker renders under A and B point icons.
- Remove bootstrap.js dependency because we use angular-ui-bootstrap anyway so it's not needed.
- Add "Change Password" page. Probably routing is needed because there will be more pages and it can get messy pretty easily.
- Don't show HTTP errors in browser's log for "/api/user" methods.

