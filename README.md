# Readme

## Introduction

This application was created to scrape shows using the [TVMAZE API](https://www.tvmaze.com/api) and save them into a SQL database.
From there on out you are able to do basic CRUD operations on the database, where a caching layer is implemented to prevent lots of network traffic.

The startup time is a little slow, as it is importing quite a few shows, although scraping is skipped when there are already items in the database.

## Structure

The project is structured as follows

```
src
|_ Api
   A REST API which calls the domain layer
|_ Domain
   The layer that is responsible for caching and calling the infrastructure layer
|_ Domain.Test.Unit
   Project responsible for testing the domain layer
|_ Infrastructure
   The layer that is responsible for talking to the SQL Database
|_ Infrastructure.Test.Unit
   Project responsible for testing the infrastructure layer
```

**IRepository**\
I created the `IRepository` interface in the domain layer, because I want to be able to switch the infrastructure layer from for example the SQL database to an any other type of database, without having the change my domain layer. The domain layer expects that it can make a certain call to the infrastructure layer, but does not care what actually happens there.
Now that the `IRepository` interface is in the domain layer, any change in the infrastructure will still need to match that interface.

## How to use

This project can be run by opening the project in visual studio and pressing F5.
The tests can be run using the test explorer.

> Note: This is a .net 8.0 application, so you will need to have the .net 8.0 sdk installed to run this application
