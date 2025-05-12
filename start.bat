@echo off
REM
git clone https://github.com/LibreTranslate/LibreTranslate

REM
start cmd /k docker-compose up --build

REM
sleep 10

REM
start http://localhost:4200