docker-compose up -d

# docker cp ucemedb.sql DockerUcemeDb:/var/opt/mssql/backup
# docker cp ucemedb.sql sqlcmd:/var/opt/mssql/backup

# docker exec -it "DockerUcemeDb" /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Ch4mp10ns' -i /var/opt/mssql/backup