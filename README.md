🛍️ ShopApp – Full-Stack E-Commerce Application
ShopApp is a full-stack e-commerce platform built with ASP.NET Core on the backend and React on the frontend. It provides a multi-vendor marketplace experience where users can browse products, manage their basket, and complete orders, while sellers can manage their own listings.

# 📌 Features
## 🧑‍💻 User Side
  🔐 JWT-based Authentication (via Auth0)

  🛍️ Product listing & filtering

  🧺 Shopping basket with real-time quantity updates

  💳 Checkout workflow

  📦 Address management

## 🧑‍💼 Seller Panel
  🛒 Add/update/delete products

  📊 Manage product prices and stock

  🚚 Define shipping conditions (e.g., free shipping thresholds)

# ⚙️ Technologies
## 🔧 Backend (API)
ASP.NET Core 8

Entity Framework Core with Code-First approach

MySQL as relational database

Redis for caching

JWT Authentication

AutoMapper, FluentValidation, Swagger, Serilog

## 🖥️ Frontend
React.js with functional components

React Router v6

Axios for HTTP requests

TailwindCSS / CSS Modules for styling

Auth0 for user authentication

# 🚀 Getting Started
## 📦 Prerequisites
.NET 8 SDK

Node.js

MySQL

Redis server (optional, for caching)

Auth0 account


# Clone repo
git clone https://github.com/SezerKARR/ShopApi.git
cd ShopApi

# Update your MySQL and Redis settings in appsettings.json
# Run migrations
dotnet ef database update

# Run the API
dotnet run
🌐 Frontend Setup (if using React)
bash
Kopyala
Düzenle
# Clone frontend repo
cd shop-client

# Install dependencies
npm install

# 📦 Database Configuration
DB_HOST=YOUR_DB_HOST
DB_PORT=3306
DB_NAME=shopapi
DB_USER=YOUR_DB_USER
DB_PASSWORD=YOUR_DB_PASSWORD

# 🐇 RabbitMQ Configuration
RABBITMQ_HOST=YOUR_RABBITMQ_HOST
RabbitMQ__HostName=YOUR_RABBITMQ_HOST

# 🔐 Google OAuth Configuration (Auth0, Google Login)
GOOGLE_CLIENT_ID=YOUR_GOOGLE_CLIENT_ID
GOOGLE_CLIENT_SECRET=YOUR_GOOGLE_CLIENT_SECRET
GOOGLE_PROJECT_ID=YOUR_GOOGLE_PROJECT_ID
GOOGLE_AUTH_URI=https://accounts.google.com/o/oauth2/auth
GOOGLE_TOKEN_URI=https://oauth2.googleapis.com/token
GOOGLE_AUTH_PROVIDER_X509_CERT_URL=https://www.googleapis.com/oauth2/v1/certs
GOOGLE_REDIRECT_URIS=http://localhost:5173/signin-google

# Run the frontend
npm run dev


