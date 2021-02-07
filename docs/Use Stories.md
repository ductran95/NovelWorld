# Novel World app - demo for microservice
# List User Story

## Core
1. Authorization
   * By role (0%)
   * By money/credit (0%)
2. Caching

## Identity
1. Login, logout using OAuth2
   * Authenticate user via OAuth2 (100%)
   * Get user's data via OAuth2 (90% - missing role information)
   * Single sign out (0% - will implement with FE)
2. Register new user
   * BE (90% - missing send email verification)
   * FE (50% - sample UI)
3. Update user data (name, dob, avatar)
   * BE (0%)
   * FE (0%)
4. UI layout (0%)

## Master Data
1. CRUD Category
   * BE (100%)
   * FE (0%)
2. CRUD Book
   * BE (100%)
   * FE (0%)
3. CRUD Book's Chapters
   * BE (100%)
   * FE (0%)
4. CRUD other data (FAQ, About, Contact)
   * BE (100%)
   * FE (0%)

## Library
1. Query book by rating, top reading, top comment, latest update
   * BE (0%)
   * FE (0%)
1. Suggest books based on user history/favorite
   * BE (0%)
   * FE (0%)

## Reader
1. CRUD Book's Comments
   * BE (100%)
   * FE (0%)
2. Rate book
   * BE (0%)
   * FE (0%)
3. Read book, manage reading status, bookmark
   * BE (0%)
   * FE (0%)
4. Manage user's reading list, reading history
   * BE (0%)
   * FE (0%)
5. View, mark as read user's notification
   * BE (0%)
   * FE (0%)
6. Export/Download book (background task & send email)
   * BE (0%)
   * FE (0%)