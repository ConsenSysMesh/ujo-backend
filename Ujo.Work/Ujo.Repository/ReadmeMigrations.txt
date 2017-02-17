When developing "not life", we will continue to do mofidications and dropping the datatabase, without the need to have custom migrations.

So just run the DeleteDatabase.sql and Add-Migration InitialCreate -force together with Update-Database (To see the shape of the database)


To generate all the scripts run Update-Database -Script -SourceMigration:0