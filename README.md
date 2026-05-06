<p align="center">
  <img src="assets/logo-saquero-cloud.svg" width="130"/>
</p>

<h1 align="center">SaqueroCloud</h1>
<p align="center">
  SaaS Admin Platform — .NET 8 · React · JWT Auth · Subscription Management
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8-512BD4?style=flat-square&logo=dotnet"/>
  <img src="https://img.shields.io/badge/React-18-61DAFB?style=flat-square&logo=react"/>
  <img src="https://img.shields.io/badge/Auth-JWT-green?style=flat-square"/>
  <img src="https://img.shields.io/badge/Architecture-Clean-orange?style=flat-square"/>
  <img src="https://img.shields.io/badge/Status-Active-success?style=flat-square"/>
</p>

---

## What is this?

SaqueroCloud is a full-stack SaaS admin platform that simulates a real-world subscription
management system. Administrators can manage users, subscription plans and billing lifecycle
through a clean React dashboard backed by a production-style ASP.NET Core API.

---

## Preview

### 📊 Dashboard — Real-time stats and plan occupancy

![Dashboard](assets/dashboard.png)

### 👤 Users — User management with roles and plan info

![Users](assets/users.png)

### 📦 Plans — Subscription plan management

![Plans](assets/plans.png)

### 🔄 Subscriptions — Assign, edit and cancel subscriptions

![Subscriptions](assets/subscriptions.png)

---

## Key Features

- JWT Authentication with role-based access control (Admin / User)
- User management — create, list, assign roles
- Subscription plan management — create, edit, delete plans with capacity limits
- Subscription lifecycle — assign, cancel, track expiry
- Dashboard with real-time stats and plan occupancy charts
- Expiring subscriptions tracking (configurable days ahead)
- Full React admin dashboard with dark UI
- REST API with Swagger documentation

---

## Tech Stack

### Backend

| Technology        | Role               |
| ----------------- | ------------------ |
| C# / .NET 8       | Language & runtime |
| ASP.NET Core      | Web API framework  |
| Entity Framework  | ORM & migrations   |
| SQLite            | Database (dev)     |
| JWT Bearer        | Authentication     |
| Swagger / OpenAPI | API documentation  |

### Frontend

| Technology | Role                   |
| ---------- | ---------------------- |
| React 18   | UI framework           |
| Vite       | Build tool             |
| Axios      | HTTP client            |
| CSS        | Styling (dark SaaS UI) |

---

## Architecture

Clean layered architecture with clear separation of concerns.

```
React Frontend (port 5173)
        │
        │ REST / JSON
        ▼
ASP.NET Core API (port 5000)
  ├── Controllers   (HTTP layer)
  ├── Services      (business logic)
  ├── Repositories  (data access)
  └── Models        (domain entities)
        │
        ▼
     SQLite (EF Core)
```

See [ARCHITECTURE.md](ARCHITECTURE.md) for detailed design decisions.

---

## Getting Started

### Requirements

- .NET 8 SDK
- Node.js 18+
- Git

### 1. Clone the repo

```bash
git clone https://github.com/Saquero/SaqueroCloud.git
cd SaqueroCloud
```

### 2. Run the backend

```powershell
cd SaqueroCloud.API
dotnet restore
dotnet run --urls="http://localhost:5000"
```

API available at: `http://localhost:5000`
Swagger UI: `http://localhost:5000/swagger`

### 3. Run the frontend

```powershell
cd saquerocloud-frontend
npm install
npm run dev
```

Frontend available at: `http://localhost:5173`

### 4. Login

```
Email:    Saquero@pruebas.com
Password: Admin1234!
```

---

## API Endpoints

| Method | Path                               | Auth   | Description            |
| ------ | ---------------------------------- | ------ | ---------------------- |
| POST   | /api/Auth/login                    | Public | Login, returns JWT     |
| POST   | /api/Auth/register                 | Public | Register user          |
| GET    | /api/Users                         | Admin  | List users             |
| GET    | /api/subscription-plans            | Auth   | List plans             |
| POST   | /api/subscription-plans            | Admin  | Create plan            |
| PUT    | /api/subscription-plans/{id}       | Admin  | Update plan            |
| DELETE | /api/subscription-plans/{id}       | Admin  | Delete plan            |
| GET    | /api/Subscriptions                 | Admin  | List subscriptions     |
| POST   | /api/Subscriptions/assign/{userId} | Admin  | Assign plan to user    |
| PATCH  | /api/Subscriptions/{id}/cancel     | Admin  | Cancel subscription    |
| GET    | /api/Dashboard/summary             | Admin  | Dashboard stats        |
| GET    | /api/Dashboard/expiring            | Admin  | Expiring subscriptions |

---

## Project Structure

```
SaqueroCloud/
├── SaqueroCloud.API/
│   ├── Controllers/       Auth, Users, Plans, Subscriptions, Dashboard
│   ├── Data/              AppDbContext (Entity Framework Core)
│   ├── Middleware/        JWT middleware
│   ├── Migrations/        EF Core database migrations
│   ├── Models/            User, SubscriptionPlan, Subscription
│   ├── Repositories/      Data access layer
│   ├── Services/          Business logic layer
│   └── Program.cs         App bootstrap and DI configuration
└── saquerocloud-frontend/
    ├── src/
    │   ├── components/    Reusable UI components
    │   ├── pages/         Dashboard, Users, Plans, Subscriptions
    │   └── services/      Axios API client
    └── public/            Static assets
```

---

## Roadmap

- [ ] PostgreSQL + Docker Compose
- [ ] Unit tests for Services layer
- [ ] Refresh token implementation
- [ ] Email notifications for expiring subscriptions
- [ ] Deploy (Render + Vercel)

---

## Part of the Saquero Ecosystem

| Project                                                         | Stack            | Description                  |
| --------------------------------------------------------------- | ---------------- | ---------------------------- |
| SaqueroCloud                                                    | .NET 8 + React   | This project                 |
| [SaqueroOrderCore](https://github.com/Saquero/SaqueroOrderCore) | Java 21 + Spring | Order lifecycle backend, DDD |

---

<p align="center">
  <a href="https://linkedin.com/in/manusaquero">
    <img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"/>
  </a>
  <a href="mailto:manusaquero@gmail.com">
    <img src="https://img.shields.io/badge/Email-D14836?style=for-the-badge&logo=gmail&logoColor=white"/>
  </a>
  <a href="https://github.com/Saquero">
    <img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white"/>
  </a>
</p>
