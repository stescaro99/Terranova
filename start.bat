@echo off
REM
git clone https://github.com/LibreTranslate/LibreTranslate

REM
docker-compose up --build -d

REM
sleep 10

REM
start http://localhost:4200