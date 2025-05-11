@echo off
REM
git clone https://github.com/LibreTranslate/LibreTranslate

REM
docker-compose up --build

REM
timeout /t 10 > nul

REM
start http://localhost:4200