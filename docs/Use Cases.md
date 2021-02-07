# Novel World app - demo for microservice
# List Actor - User Case

## Reader (logged in)
1. Identity
    * Login, logout using OIDC
    * Update user data (name, dob, avatar)
3. Library
    * Query book by rating, top reading, top comment, latest update
    * Suggest books based on user history/favorite
4. Reader
    * CRUD Book's Comments
    * Rate book
    * Read book, manage reading status, bookmark
    * Manage user's reading list, reading history
    * View, mark as read user's notification
    * Export/Download book (background task & send email)

## Reader (anonymous)
1. Identity
    * Register new user
3. Library
    * Query book by rating top reading, top comment, latest update
4. Reader
    * Read book

## Converter (logged in)
1. Identity
    * Login, logout using OIDC
    * Update user data (name, dob, avatar)
2. Master Data
    * Create, Update Book's Chapters
3. Library
    * Query book by rating, top reading, top comment, latest update
    * Suggest books based on user history/favorite
4. Reader
    * CRUD Book's Comments
    * Rate book  
    * Read book, manage reading status, bookmark
    * Manage user's reading list, reading history
    * View, mark as read user's notification
    * Export/Download book (background task & send email)

## Admin (logged in)
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
    * View, mark as read user's notification
    * Export/Download book (background task & send email)