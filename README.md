<p align="center">
  <img src="assets/logo-saquero-cloud.svg" alt="SaqueroCloud" width="160"/>
</p>

<h1 align="center">SaqueroCloud</h1>
<p align="center">SaaS Platform Core — .NET 8 · React · JWT Auth · Subscription Management</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet" />
  <img src="https://img.shields.io/badge/React-18-61DAFB?style=flat-square&logo=react" />
  <img src="https://img.shields.io/badge/Auth-JWT-green?style=flat-square" />
  <img src="https://img.shields.io/badge/Architecture-Clean-orange?style=flat-square" />
  <img src="https://img.shields.io/badge/EF_Core-SQLite-blue?style=flat-square" />
  <img src="https://img.shields.io/badge/Status-Active-success?style=flat-square" />
</p>

---

## What is SaqueroCloud?

SaqueroCloud is a full-stack SaaS administration platform built with **.NET 8** and **React**.

It simulates the core platform layer of a subscription-based SaaS product: authentication, user management, subscription plans, active subscriptions, expiration tracking, role-based access control and operational health monitoring.

This project is part of the **Saquero Enterprise** ecosystem and acts as the platform/admin core that can be connected with background jobs, order services and API gateway components.

---

## Preview

### Dashboard — Platform overview and subscription metrics

![Dashboard](assets/dashboard.png)

### Users — Admin user management

![Users](assets/users.png)

### Plans — Subscription plan management

![Plans](assets/plans.png)

### Subscriptions — Assignment, updates and cancellation

![Subscriptions](assets/subscriptions.png)

---

## Key Design Decisions

**JWT authentication with role-based authorization.**  
The API uses JWT Bearer authentication and protects administrative operations with role-based access control.

**Subscription plans and subscriptions are separated.**  
A SubscriptionPlan defines the commercial capacity and metadata of a plan. A subscription represents a user assigned to a plan for a period of time.

**Cancellation is handled as a lifecycle operation.**  
Subscriptions are cancelled through an explicit endpoint instead of being hard-deleted. This keeps historical records available.

**The frontend is separated from the API.**  
The React/Vite dashboard consumes the ASP.NET Core API through REST endpoints and stores authentication state client-side.

**SQLite is used for local development.**  
The project runs without external infrastructure. EF Core migrations are applied automatically on startup.

**The platform exposes a health endpoint.**  
The /health endpoint makes the service easy to monitor and prepares it for ecosystem-level orchestration.

---

## Tech Stack

### Backend

| Technology | Role |
| --- | --- |
| .NET 8 | Runtime |
| C# | Backend language |
| ASP.NET Core | Web API |
| Entity Framework Core | ORM and migrations |
| SQLite | Local development database |
| JWT Bearer | Authentication |
| Swagger / OpenAPI | API documentation |

### Frontend

| Technology | Role |
| --- | --- |
| React 18 | UI framework |
| Vite | Frontend build tool |
| Axios | API client |
| CSS | SaaS dashboard styling |

---

## Architecture

`	xt
React Frontend
Vite dashboard running on port 5173
        |
        | REST / JSON
        v
ASP.NET Core API
SaqueroCloud.API running on port 5000
        |
        | EF Core
        v
SQLite database

Backend structure:

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

Frontend structure:

saquerocloud-frontend/
├── src/
│   ├── components/
│   ├── pages/
│   └── services/
└── public/

See ARCHITECTURE.md for the full design documentation.

API Endpoints
Authentication
MethodEndpointAuthDescription
POST/api/Auth/registerPublicRegister a new user
POST/api/Auth/loginPublicLogin and receive JWT token
Users
MethodEndpointAuthDescription
GET/api/Users?page=1&pageSize=10AdminList users with pagination
GET/api/Users/{id}AuthGet user by ID
PUT/api/Users/{id}AdminUpdate user
DELETE/api/Users/{id}AdminDelete user
Subscription Plans
MethodEndpointAuthDescription
GET/api/subscription-plansPublicList all plans
GET/api/subscription-plans/{id}PublicGet plan by ID
POST/api/subscription-plansAdminCreate plan
PUT/api/subscription-plans/{id}AdminUpdate plan
DELETE/api/subscription-plans/{id}AdminDelete plan
Subscriptions
MethodEndpointAuthDescription
GET/api/Subscriptions/activeAdminList active subscriptions
GET/api/Subscriptions/expiring-soon?days=30AdminList subscriptions expiring soon
GET/api/Subscriptions/summaryAdminPlan usage summary
GET/api/Subscriptions/user/{userId}AuthList subscriptions for a user
POST/api/Subscriptions/assign/{userId}AdminAssign subscription to user
PUT/api/Subscriptions/{subscriptionId}AdminUpdate subscription
PATCH/api/Subscriptions/{subscriptionId}/cancelAdminCancel subscription
Monitoring
MethodEndpointAuthDescription
GET/healthPublicService health check
Getting Started
Requirements
.NET 8 SDK
Node.js 18+
Git
PowerShell
Clone the repository
git clone https://github.com/Saquero/SaqueroCloud.git
cd SaqueroCloud
Run the backend
cd SaqueroCloud.API
dotnet restore
dotnet run --urls="http://localhost:5000"

API:

http://localhost:5000

Swagger UI in development:

http://localhost:5000

Health check:

http://localhost:5000/health
Run the frontend
cd saquerocloud-frontend
npm install
npm run dev

Frontend:

http://localhost:5173
Demo credentials
Email:    Saquero@pruebas.com
Password: Admin1234!
Example Requests
Login
Invoke-RestMethod -Uri "http://localhost:5000/api/Auth/login" -Method POST -ContentType "application/json" -Body '{
  "email": "Saquero@pruebas.com",
  "password": "Admin1234!"
}'
Health check
Invoke-RestMethod -Uri "http://localhost:5000/health"
List public plans
Invoke-RestMethod -Uri "http://localhost:5000/api/subscription-plans"
Part of the Saquero Enterprise Ecosystem
ProjectStackDescription
SaqueroCloud.NET 8 + ReactSaaS platform core, auth and subscription management
SaqueroOrderCoreJava 21 + Spring Boot 3Order lifecycle backend, DDD and Hexagonal Architecture
SaqueroJobs.NET 8Background job processing engine
SaqueroGateway.NET 8API Gateway — planned
Ecosystem Health
ServicePortHealth
SaqueroCloud5000/health
SaqueroOrderCore8080/actuator/health
SaqueroJobs5200/health
SaqueroGateway5100planned
Roadmap
PostgreSQL support for production-like environments
Docker Compose for local infrastructure
Unit tests for services and repository boundaries
Refresh token workflow
Audit log for administrative actions
Background expiration checks through SaqueroJobs
Gateway routing through SaqueroGateway
<p align="center"> <a href="https://linkedin.com/in/manusaquero"> <img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white" /> </a> <a href="mailto:manusaquero@gmail.com"> <img src="https://img.shields.io/badge/Email-D14836?style=for-the-badge&logo=gmail&logoColor=white" /> </a> <a href="https://github.com/Saquero"> <img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white" /> </a> </p> "@

 = @"

ARCHITECTURE.md — SaqueroCloud
Overview

SaqueroCloud is a full-stack SaaS administration platform built with ASP.NET Core, Entity Framework Core and React.

The project models the platform core of a SaaS product: authentication, role-based access control, users, subscription plans, active subscriptions, expiration tracking and service health monitoring.

It is intentionally simple to run locally, but structured like a real backend platform.

Architectural Style

SaqueroCloud follows a clean layered architecture:

Controllers handle HTTP concerns.
Services orchestrate business operations.
Repositories isolate persistence.
Models and DTOs represent platform data and API contracts.
Middleware centralizes cross-cutting concerns.
React frontend consumes the API through REST.

This is not a strict DDD/Hexagonal implementation like SaqueroOrderCore or SaqueroJobs. It is a pragmatic layered SaaS platform designed to be clear, maintainable and easy to extend.

System Context
User / Admin
    |
    v
React Dashboard
    |
    | REST / JSON + JWT
    v
SaqueroCloud.API
    |
    | EF Core
    v
SQLite Database
Runtime ports
ComponentPort
React frontend5173
ASP.NET Core API5000
Swagger UI5000
Health endpoint5000/health
Backend Layers
Controllers

Location:

SaqueroCloud.API/Controllers

Controllers are thin HTTP entry points. They validate request state, call services and return HTTP responses.

Current controllers:

ControllerResponsibility
AuthControllerRegister and login users
UsersControllerRead, update and delete users
SubscriptionPlansControllerManage subscription plans
SubscriptionsControllerAssign, update, cancel and query subscriptions

Controllers do not access Entity Framework directly.

Services

Services contain application/business logic.

They coordinate repositories, enforce workflows and return DTOs or operation results.

Current registered services:

IAuthService -> AuthService
IUserService -> UserService
ISubscriptionService -> SubscriptionService

Examples of service responsibilities:

Register users
Validate login credentials
Generate JWT tokens
List users with pagination
Create and update subscription plans
Assign subscriptions to users
Validate plan capacity before assignment
Cancel active subscriptions
Build subscription usage summaries
Query subscriptions expiring soon
Repositories

Repositories isolate database access from the service layer.

Current registered repositories:

IUserRepository -> UserRepository
ISubscriptionPlanRepository -> SubscriptionPlanRepository
IUserSubscriptionRepository -> UserSubscriptionRepository

The API uses Entity Framework Core with SQLite through AppDbContext.

Middleware

ExceptionMiddleware is registered early in the request pipeline.

Its role is to centralize error handling and avoid spreading exception response logic across controllers.

Authentication and Authorization

SaqueroCloud uses JWT Bearer authentication.

Configuration is loaded from:

JwtSettings

JWT validation includes:

issuer validation
audience validation
lifetime validation
signing key validation

Authorization is role-based.

Current roles:

Admin
User

Admin-only operations are protected with:

[Authorize(Roles = "Admin")]

Examples:

Create plan
Update plan
Delete plan
List users
Update users
Delete users
Assign subscriptions
Cancel subscriptions
View subscription summaries
Subscription Model

SaqueroCloud separates three core concepts:

User

Represents an application user with authentication and role information.

SubscriptionPlan

Represents a commercial plan that can be assigned to users.

Typical plan concerns:

name
description
price
max users / capacity
active subscriptions
UserSubscription

Represents one user's assigned subscription to a plan.

Typical subscription concerns:

user
plan
start date
end date
active/cancelled state

This separation allows the platform to answer operational questions such as:

Which users are subscribed?
Which plans are close to capacity?
Which subscriptions expire soon?
Which subscriptions are active?
Which user belongs to which plan?
Subscription Lifecycle

Current lifecycle operations:

Assign subscription
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

Cancellation is explicit and does not require deleting the subscription record.

This keeps the domain ready for future audit history, renewal workflows and background expiration checks.

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
Program.cs Responsibilities

Program.cs wires the application together.

Current responsibilities:

CORS policy for local frontend ports
SQLite DbContext registration
Repository registration
Service registration
JWT authentication
Authorization
Controllers
Swagger/OpenAPI with JWT support
Automatic EF Core migrations on startup
Exception middleware
Swagger UI in development
Health endpoint
Controller routing
CORS

The API allows local frontend development origins:

http://localhost:5173
http://localhost:5174
http://localhost:5175
http://127.0.0.1:5173
http://127.0.0.1:5174
http://127.0.0.1:5175

This supports Vite port changes without breaking local development.

Swagger

Swagger is enabled in development.

The Swagger UI is configured at the root route:

http://localhost:5000

JWT support is included through an OpenAPI Bearer security scheme.

Database Strategy

SaqueroCloud currently uses SQLite with EF Core.

Benefits:

no external database required
fast local setup
good for portfolio demos
migrations handled through EF Core
easy future migration to PostgreSQL or SQL Server

Automatic migrations run during startup:

db.Database.Migrate();

This keeps local development friction low.

Error Handling

Errors are centralized through ExceptionMiddleware.

This avoids repeating try/catch blocks across all controllers and keeps the HTTP layer cleaner.

Some expected business conflicts are still handled directly where context-specific responses are clearer.

Example:

duplicated email during registration
plan not found
invalid subscription assignment
deleting a plan with associated subscriptions
Frontend Architecture

The React frontend is a separate Vite application.

Expected structure:

saquerocloud-frontend/
├── src/
│   ├── components/
│   ├── pages/
│   └── services/
└── public/

Responsibilities:

login
dashboard overview
users view
plans management
subscriptions management
API calls through Axios
JWT persistence and authorization headers

The frontend is intentionally kept separate from the API so both sides can evolve independently.

Saquero Enterprise Fit

SaqueroCloud is the SaaS platform core of the Saquero Enterprise ecosystem.

ProjectResponsibility
SaqueroCloudAuthentication, users, subscriptions and SaaS administration
SaqueroOrderCoreOrder lifecycle and domain-heavy backend workflows
SaqueroJobsBackground processing, scheduled jobs and execution tracking
SaqueroGatewayUnified entry point and routing layer

Future ecosystem flow:

Frontend / Client
        |
        v
SaqueroGateway
        |
        +--> SaqueroCloud
        +--> SaqueroOrderCore
        +--> SaqueroJobs

SaqueroCloud provides the administrative and subscription foundation that the other services can later consume or integrate with.

Current Limitations

The project is intentionally honest about what is implemented.

Current limitations:

no refresh token workflow yet
no production database configured yet
no Docker Compose yet
no automated test suite documented yet
no audit log table yet
no background expiration worker yet
no dedicated DashboardController currently present

The dashboard currently relies on subscription-related endpoints such as:

/api/Subscriptions/summary
/api/Subscriptions/expiring-soon
Recommended Next Improvements

Priority order:

Add tests for services and business workflows.
Add PostgreSQL and Docker Compose.
Add audit logging for admin operations.
Add refresh tokens.
Add SaqueroJobs integration for expiration checks and notifications.
Add SaqueroGateway routing.
Add CI pipeline.
Design Summary

SaqueroCloud is not just a login CRUD.

It is the foundation of a SaaS platform:

authentication
authorization
user administration
plan management
subscription assignment
cancellation lifecycle
expiration tracking
health monitoring
React admin dashboard
ecosystem-ready API design

The architecture is deliberately simple, readable and extendable.
