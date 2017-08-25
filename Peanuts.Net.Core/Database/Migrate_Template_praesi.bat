rem Aktualisieren der DB
call flyway\flyway.cmd -url=jdbc:sqlserver://cs-appsrv01;instanceName=SQL2014EXPRESS;databaseName=dev_peanuts_net_praesi -locations=filesystem:.\DBScripts -user=flyway -password=lRJEkxxI5R migrate
pause