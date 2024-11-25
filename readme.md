# Weather Forecast  - Azure Functions <span>#</span>

This project has three following Azure Functions <br><br>
`1. Register` : This is triggered by a Http POST request and enables adding an entry to the Register table for the place name provided as a route parameter.<br>
`2. Process`  : This function is triggered by an Azure Timer and retrieves weather data from a third-party API for the places listed in the Register table. The received weather data is then updated in the Data field of the Register table<br>
`3. Query`    : This is triggered by an HTTP GET request with a place name as a query parameter and retrieves the weather data for the specified place from the Data field in the Register table.
## How it works

For a `TimerTrigger` to work, you provide a schedule in the form of a [cron expression](https://en.wikipedia.org/wiki/Cron#CRON_expression)(See the link for full details). A cron expression is a string with 6 separate expressions which represent a given schedule via patterns. The pattern we use to represent every 5 minutes is `0 */5 * * * *`. This, in plain text, means: "When seconds is equal to 0, minutes is divisible by 5, for any hour, day of the month, month, day of the week, or year".

## Learn more

<TODO> Documentation


