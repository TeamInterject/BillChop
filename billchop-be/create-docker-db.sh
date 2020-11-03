#!/bin/bash

sudo docker pull mcr.microsoft.com/mssql/server:2019-latest
sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Development1" -p 1401:1433 --name sql1 -h sql1 -d mcr.microsoft.com/mssql/server:2019-latest

# Resulting connection string: Server=127.0.0.1,1401; Database=BillChopDb; User Id=SA;Password=Development1
