# Novel World app - demo for microservice
## Run migrations
1. Identity Service
    * dotnet ef migrations add Initial --project migrations/NovelWorld.Identity.DbMigration
    * dotnet ef database update --project migrations/NovelWorld.Identity.DbMigration