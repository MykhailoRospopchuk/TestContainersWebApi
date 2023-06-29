help:
## The following commands can be used:
	@sed -n 's/^##//p' ${MAKEFILE_LIST}


run-app:
## run-app:  Run the application
	dotnet run

roll-migration:
## roll-migration:	roll default migrations
	dotnet ef database update InitMigration --context AppDbContext

roll-back-migration:
## roll-back-migration:	 roll back default migrations
	dotnet ef database update 0 --context AppDbContext