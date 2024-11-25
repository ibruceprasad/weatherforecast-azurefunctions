# Weather Forecast  - Azure Functions <span>#</span>

This project has three following Azure Functions <br><br>
`1. Register` : This is triggered by a Http POST request and enables adding an entry to the Register table for the place name provided as a route parameter.<br>
`2. Process`  : This function is triggered by an Azure Timer and retrieves weather data from a third-party API for the places listed in the Register table. The received weather data is then updated in the Data field of the Register table<br>
`3. Query`    : This is triggered by an HTTP GET request with a place name as a query parameter and retrieves the weather data for the specified place from the Data field in the Register table.<br><br>
## Technologies
<br><br>
1. .Net 8 Isolated Worer model <br>  
2. Http Trigger, Timer Tigger, Sql Input and Output binding br>
3. xUnit unit test <br>
<br><br>
## Project Structure <br>
![Project structure](weatherforecast-azurefunctions/Properties/Screenshot 2024-11-26 070242.jpg)





