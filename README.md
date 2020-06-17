# README #

This README would normally document whatever steps are necessary to get your application up and running.

# Setup

#### Secrets
The application **ExporterWeb** uses the `ExporterWeb/appsettings.Secrets.json` file. It is gitignored so you have to create one.

To see a structure of the file, take a look at `ExporterWeb/appsettings.Secrets.Example.json`.

#### Database
In `Package Manager Console` run `Update-Database` to apply all the migrations

#### Result
After you have built and run, go to `https://localhost:PORT/_test` to see the result.

`PORT` may be found in `ExporterWeb/Properties/launchSettings.json`, in the `sslPort` section
