#!/usr/bin/env bash
set -e

echo "Waiting for SQL Server..."
for i in $(seq 1 180); do
  /opt/mssql-tools/bin/sqlcmd -C -S mssql -U sa -P "Str0ngPassw0rd!" -Q "SELECT 1" >/dev/null 2>&1 && break
  sleep 2
done

echo "Running init-create-db.sql..."
/opt/mssql-tools/bin/sqlcmd -C -S mssql -U sa -P "Str0ngPassw0rd!" -b -i /scripts/init-create-db.sql

echo "Waiting for Evently ONLINE..."
for i in $(seq 1 180); do
  st=$(/opt/mssql-tools/bin/sqlcmd -C -S mssql -U sa -P "Str0ngPassw0rd!" -h -1 -W -Q "SET NOCOUNT ON; SELECT state_desc FROM sys.databases WHERE name=N'Evently';" 2>/dev/null || true)
  [ "$st" = "ONLINE" ] && break
  sleep 2
done

echo "Running init-schemas.sql..."
/opt/mssql-tools/bin/sqlcmd -C -S mssql -U sa -P "Str0ngPassw0rd!" -b -i /scripts/init-schemas.sql

echo "Running init-app-user.sql..."
/opt/mssql-tools/bin/sqlcmd -C -S mssql -U sa -P "Str0ngPassw0rd!" -b -i /scripts/init-app-user.sql

echo "Init done."
