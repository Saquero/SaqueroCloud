# ARCHITECTURE.md — SaqueroCloud

## Overview

SaqueroCloud is a full-stack SaaS administration platform built with ASP.NET Core, Entity Framework Core and React.

It models the platform core of a subscription-based SaaS product:

- authentication
- role-based authorization
- user administration
- subscription plans
- user subscriptions
- expiration tracking
- plan usage summaries
- operational health monitoring

SaqueroCloud is part of the Saquero Enterprise ecosystem and acts as the SaaS/admin foundation for future integration with background jobs, order services and gateway routing.

---

## Architectural Style

SaqueroCloud uses a pragmatic layered architecture:

```txt
React Dashboard
        |
        | REST / JSON + JWT
        v
ASP.NET Core API
        |
        | Services
        v
Repositories
        |
        | EF Core
        v
SQLite

This project is intentionally simpler than SaqueroOrderCore and SaqueroJobs.

SaqueroOrderCore demonstrates stronger DDD and Hexagonal Architecture.
SaqueroJobs demonstrates background processing and execution lifecycle design.
SaqueroCloud focuses on a complete SaaS admin platform with frontend, auth and subscription management.
Runtime Components
ComponentTechnologyDefault Port
FrontendReact + Vite5173
APIASP.NET Core .NET 85000
DatabaseSQLite + EF Corelocal file
API DocsSwagger / OpenAPI5000
HealthMinimal API endpoint5000/health
Backend Structure
SaqueroCloud.API/
├── Controllers/
│   ├── AuthController.cs
│   ├── UsersController.cs
│   ├── SubscriptionPlansController.cs
│   └── SubscriptionsController.cs
├── Data/
│   └── AppDbContext.cs
├── Middleware/
│   └── ExceptionMiddleware.cs
├── Models/
├── Repositories/
├── Services/
└── Program.cs
Layer Responsibilities
Controllers

Controllers are the HTTP boundary of the API.

They are responsible for:

defining routes
receiving request DTOs
validating model state
calling services
returning HTTP responses

Current controllers:

ControllerResponsibility
AuthControllerRegister and login users
UsersControllerRead, update and delete users
SubscriptionPlansControllerCreate, read, update and delete subscription plans
SubscriptionsControllerAssign, update, cancel and query user subscriptions
Services

Services contain application logic and coordinate repository operations.

Registered services:

IAuthService -> AuthService
IUserService -> UserService
ISubscriptionService -> SubscriptionService

Typical responsibilities:

register new users
validate credentials
generate JWT tokens
paginate users
manage subscription plans
assign plans to users
validate subscription operations
cancel subscriptions
calculate plan usage summary
retrieve expiring subscriptions
Repositories

Repositories isolate persistence logic from services.

Registered repositories:

IUserRepository -> UserRepository
ISubscriptionPlanRepository -> SubscriptionPlanRepository
IUserSubscriptionRepository -> UserSubscriptionRepository

Persistence is handled through EF Core and SQLite.

Middleware

ExceptionMiddleware centralizes API error handling.

This keeps controllers cleaner and avoids duplicating exception response logic across endpoints.

Authentication and Authorization

SaqueroCloud uses JWT Bearer authentication.

JWT configuration is read from:

JwtSettings

The API validates:

issuer
audience
token lifetime
signing key

Authorization is role-based.

Current roles:

Admin
User

Admin-only endpoints use:

[Authorize(Roles = "Admin")]
API Surface
Authentication
POST /api/Auth/register
POST /api/Auth/login
Users
GET    /api/Users?page=1&pageSize=10
GET    /api/Users/{id}
PUT    /api/Users/{id}
DELETE /api/Users/{id}
Subscription Plans
GET    /api/subscription-plans
GET    /api/subscription-plans/{id}
POST   /api/subscription-plans
PUT    /api/subscription-plans/{id}
DELETE /api/subscription-plans/{id}
Subscriptions
GET   /api/Subscriptions/active
GET   /api/Subscriptions/expiring-soon?days=30
GET   /api/Subscriptions/summary
GET   /api/Subscriptions/user/{userId}
POST  /api/Subscriptions/assign/{userId}
PUT   /api/Subscriptions/{subscriptionId}
PATCH /api/Subscriptions/{subscriptionId}/cancel
Monitoring
GET /health
Subscription Domain

SaqueroCloud separates three important concepts.

User

Represents an authenticated platform user.

SubscriptionPlan

Represents a commercial plan that can be assigned to users.

UserSubscription

Represents the relationship between a user and a plan over time.

This separation allows the API to answer platform questions such as:

which subscriptions are active?
which plans are being used?
which subscriptions expire soon?
which subscriptions belong to a specific user?
can this user be assigned to this plan?
Subscription Lifecycle

Current lifecycle:

Create plan
    |
    v
Assign subscription to user
    |
    v
Active subscription
    |
    +--> Update plan or end date
    |
    +--> Query as expiring soon
    |
    v
Cancel subscription

Cancellation is explicit and handled through:

PATCH /api/Subscriptions/{subscriptionId}/cancel

The API does not rely on deleting subscriptions as the main lifecycle operation.

Database Strategy

SaqueroCloud currently uses SQLite for local development.

Advantages:

no external infrastructure required
easy local setup
works well for portfolio/demo usage
EF Core migrations can evolve the schema
future migration to PostgreSQL is straightforward

Program.cs applies migrations automatically on startup:

db.Database.Migrate();
Program.cs Responsibilities

Program.cs wires the whole API:

CORS policy for Vite local ports
SQLite DbContext
repositories
services
JWT authentication
authorization
controllers
Swagger/OpenAPI
Swagger JWT security scheme
automatic EF Core migrations
exception middleware
health endpoint
controller routing
CORS Policy

The frontend is allowed from local Vite development ports:

http://localhost:5173
http://localhost:5174
http://localhost:5175
http://127.0.0.1:5173
http://127.0.0.1:5174
http://127.0.0.1:5175
Swagger

Swagger is enabled in development.

The Swagger UI is configured at the root route:

http://localhost:5000

JWT Bearer support is included through the OpenAPI security definition.

Frontend Architecture

The frontend is a React/Vite admin dashboard.

saquerocloud-frontend/
├── public/
│   ├── favicon.svg
│   └── icons.svg
├── src/
│   ├── App.jsx
│   ├── api.js
│   ├── main.jsx
│   ├── App.css
│   └── index.css
├── package.json
├── package-lock.json
├── vite.config.js
└── eslint.config.js

The frontend is responsible for:

login flow
authenticated API calls
user management views
plan management views
subscription management views
dashboard UI
SaaS-style dark interface
Saquero Enterprise Context

SaqueroCloud is one part of a broader backend portfolio ecosystem.

ProjectResponsibility
SaqueroCloudSaaS platform core: auth, users, plans and subscriptions
SaqueroOrderCoreOrder lifecycle backend with stronger DDD/Hexagonal focus
SaqueroJobsBackground job processing and execution lifecycle tracking
SaqueroGatewayPlanned unified routing/API gateway layer

Target future flow:

Client / Dashboard
        |
        v
SaqueroGateway
        |
        +--> SaqueroCloud
        +--> SaqueroOrderCore
        +--> SaqueroJobs
Current Limitations

Current known limitations:

no refresh token workflow yet
no production database configured yet
no Docker Compose yet
no automated test suite documented yet
no audit log table yet
no background expiration worker yet
no dedicated DashboardController currently present

Dashboard-related information currently comes from subscription endpoints:

/api/Subscriptions/summary
/api/Subscriptions/expiring-soon
Recommended Next Improvements

Recommended order:

Add service-level tests.
Add PostgreSQL support.
Add Docker Compose.
Add audit logging for admin actions.
Add refresh tokens.
Integrate SaqueroJobs for scheduled expiration checks.
Route traffic through SaqueroGateway.
Add CI pipeline.
Design Summary

SaqueroCloud is the SaaS platform core of the Saquero Enterprise ecosystem.

It is not presented as a strict DDD service. It is presented honestly as a complete full-stack SaaS admin platform with clean separation between frontend, API, services, repositories and persistence.

Its value is in showing:

product thinking
full-stack delivery
authentication
authorization
subscription management
API documentation
health monitoring
ecosystem readiness
