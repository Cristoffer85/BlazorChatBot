# TWbot - for fetching global TWh data usage/demand per country

## Description
A basic (hosted) Blazor webassembly App fetching electrical data from (1) RESTful API (https://ember-energy.org) per country.   
The application filters data and lets user either manually in application search and browse through TWh data per country and per month, or use the AI-chatbot (...in its very early stages yet) to instead prompt for data.
Uses .NET9

## Installation

Project is only still local, so: 
1. download or copy this repository from github
2. make sure  you have .NET9 installed, MSSQL and also dotenv to create your own .env file in root where you add below data and replace after = your own keys and passwords (including remove < >)  
    
    EMBER_API_KEY=<.your-APIkey from https://ember-energy.org/data/api/ here>      
    MSSQL_PASSWORD=<.your-password for MSSQL here>   

user right now for MSSQL is sa/system admin, and server default localhost/SQLExpress. Adjustments for this are in appsettings.json

3. use IDE of choice, stand in root in terminal and 'dotnet watch run --project Server' (watch = optional but if youd like the hot reload that React offers etc)

## Usage
Right now no IAM implemented yet, so just basic start application thats started locally and used in browser of choice. Typically started by itself on https://localhost:5111 etc
Browse through right now minimal application and 

## Features
1. Manually search and browse through TWh data per country in tab "Electrical data", or
2. Talk and prompt with chatbot using "Natural" language instead if that is more of your preference

## Credits
Classmates from school, Myself, my Family, mighty duck rubber duck.

## License
🏆 MIT License

## Badges
![badmath](https://img.shields.io/badge/C#-50%25-blue)

## Tests
No tests yet implemented.