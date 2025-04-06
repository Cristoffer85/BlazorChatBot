# TWbot - for fetching global TWh data usage/demand per country

## Description
A basic (hosted) Blazor webassembly App fetching data from right now (1) RESTful API from ember-energy.org, filtering that data and lets user either manually in application search and browse through TWh data per country and per month, or use the AI-chatbot (...in its very early stages yet) to instead prompt for data.
Uses .NET9

## Installation

Project is only still local, so: 
1. download or copy this repository from github
2. make sure  you have .NET9 installed, and also dotenv to create your own .env file in root where you switch your own values instead of <randomvalue>
    EMBER_API_KEY=<your-APIkey from https://ember-energy.org/data/api/ here>
    OPENAI_API_KEY=<your-APIkey from https://platform.openai.com/api-keys here>
    DB_CONNECTION_STRING=<your-DB-connection for Microsoft SQL Server Management Studio/MSSQL> 
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