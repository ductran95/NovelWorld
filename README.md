# Novel World app - demo for microservice
## Separated Domain
1. Identity
    * Login, logout using OIDC
    * Register new user
    * Update user data (name, dob, avatar)
2. Master Data
    * CRUD Category
    * CRUD Book
    * CRUD Book's Chapters
    * CRUD other data (FAQ, About, Contact)
3. Library
    * Query book by top reading, top comment, latest update
    * Suggest books based on user history/favorite
4. Reader
    * CRUD Book's Comments
    * Rate book
    * Manage user's reading list, reading history
    * Manage user's notification/email
    * Export/Download book
   
## Code structure
1. Domain layer:
    * DTO with suffix Command, Query only use for internal 
    * DTO with suffix Request use for API and cross-service exchange
    
## Run migrations
1. Identity Service
    * dotnet ef migrations add Initial --project migrations/NovelWorld.Identity.DbMigration
    * dotnet ef database update --project migrations/NovelWorld.Identity.DbMigration
2. Identity Service
   * dotnet ef migrations add Initial --project migrations/NovelWorld.MasterData.DbMigration
   * dotnet ef database update --project migrations/NovelWorld.MasterData.DbMigration   