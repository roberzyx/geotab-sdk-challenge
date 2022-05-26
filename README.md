![geotab-banner](images/geotab-banner.png)
# Geotab SDK challenge
Backup application to progressively download vehicle data.

Create a backup application to progressively download vehicle data. You must use the [Geotab SDK](https://geotab.github.io/sdk/) to get the required data and store it in a file locally. Requirements:
- Download all available vehicles locally in a CSV file (one file per vehicle).
- Each vehicle file should include at least: ID, timestamp, VIN, Coordinates, and Odometer.
- Incrementally download new data and append it to the end of the same file (e.g. every minute).
- Use any language, but make it clear and easy to execute on any computer (ex. Shell scripts, python, node, java, .net, etc. are considered “easy to execute”).

Demo database and credentials: Sent by email and it’ll be active for a few weeks.
Evaluation criterias: follow the requisitions, simplicity & clarity, easy to execute. The combination of these criterias will imply “easy to maintain”.

## How to run the app
The project has been developed in Visual Studio Code using .Net Core 6.0.

To run the code must do the following:

```shell
> git clone https://github.com/roberzyx/geotab-sdk-challenge.git geotab-sdk-challenge
> cd geotab-sdk-challenge
> dotnet.exe run -- user password database server backupFrequency(min) maxNumberOfBackups
```

