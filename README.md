<p align="center">
  <img src="assets/logo-saquero-cloud.svg" alt="SaqueroCloud" width="160"/>
</p>

<h1 align="center">SaqueroCloud</h1>
<p align="center">Full-stack SaaS Admin Platform — .NET 8 · React · JWT Auth · Clean Architecture</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet" />
  <img src="https://img.shields.io/badge/React-Vite-61DAFB?style=flat-square&logo=react" />
  <img src="https://img.shields.io/badge/Auth-JWT-green?style=flat-square" />
  <img src="https://img.shields.io/badge/Architecture-Clean-orange?style=flat-square" />
  <img src="https://img.shields.io/badge/Status-Active-success?style=flat-square" />
</p>

---

## What is SaqueroCloud?

SaqueroCloud is a full-stack SaaS-style admin platform built with **ASP.NET Core (.NET 8)** and **React**.

It simulates a real-world subscription management system where administrators manage users, subscription plans, active subscriptions and billing lifecycle — with JWT authentication and a complete React dashboard.

---

## Preview

### Dashboard

[![Dashboard](assets/dashboard.png)](assets/dashboard.png)

### Users

[![Users](assets/users.png)](assets/users.png)

### Plans

[![Plans](assets/plans.png)](assets/plans.png)

### Subscriptions

[![Subscriptions](assets/subscriptions.png)](assets/subscriptions.png)

---

## Key Features

- JWT Authentication with role-based access control
- User management
- Subscription plan management
- Assign and cancel subscriptions
- Filter subscriptions by plan
- Expiring subscriptions tracking
- REST API with Swagger documentation
- Full React admin dashboard

---

## Tech Stack

### Backend

| Technology            | Version | Role              |
| --------------------- | ------- | ----------------- |
| .NET                  | 8.0     | Runtime           |
| C#                    | 12      | Language          |
| ASP.NET Core          | 8.0     | Web API           |
| Entity Framework Core | 8.0     | ORM               |
| JWT Bearer            | —       | Authentication    |
| Swagger               | —       | API documentation |

### Frontend

| Technology | Role         |
| ---------- | ------------ |
| React      | UI framework |
| Vite       | Build tool   |
| Axios      | HTTP client  |

---

## Architecture

Clean Architecture with layered separation of concerns.

```text
SaqueroCloud/
├── SaqueroCloud.API
│   ├── Controllers
│   ├── DTOs
│   ├── Models
│   ├── Services
│   ├── Data
│   └── Program.cs
│
└── saquerocloud-frontend
    ├── src/
    │   ├── components
    │   ├── pages
    │   └── services
    └── index.html
```

---

## Getting Started

### Requirements

- .NET 8 SDK
- Node.js 18+
- PowerShell

### Backend

```bash
cd SaqueroCloud.API
dotnet run --urls="http://127.0.0.1:5000"
```

Open Swagger UI: `http://localhost:5000/swagger`

### Frontend

```bash
cd saquerocloud-frontend
npm install
npm run dev
```

### Credentials

Email: Saquero@pruebas.com
Password: Admin1234!

---

## API Endpoints

### Auth

| Method | Endpoint           | Description |
| ------ | ------------------ | ----------- |
| POST   | /api/Auth/login    | Login       |
| POST   | /api/Auth/register | Register    |

### Users

| Method | Endpoint   | Description    |
| ------ | ---------- | -------------- |
| GET    | /api/Users | List all users |

### Plans

| Method | Endpoint                | Description    |
| ------ | ----------------------- | -------------- |
| GET    | /api/subscription-plans | List all plans |

### Subscriptions

| Method | Endpoint                           | Description         |
| ------ | ---------------------------------- | ------------------- |
| POST   | /api/Subscriptions/assign/{userId} | Assign subscription |
| PATCH  | /api/Subscriptions/{id}/cancel     | Cancel subscription |

---

## Part of the Saquero Backend Ecosystem

| Project                                                         | Stack                   | Description                                            |
| --------------------------------------------------------------- | ----------------------- | ------------------------------------------------------ |
| SaqueroCloud                                                    | .NET 8 + React          | SaaS admin platform, JWT auth, subscription management |
| [SaqueroOrderCore](https://github.com/Saquero/SaqueroOrderCore) | Java 21 + Spring Boot 3 | Order lifecycle backend, DDD, Hexagonal                |
| [SaqueroJobs](https://github.com/Saquero/SaqueroJobs)           | .NET 8                  | Background job processing engine                       |
| SaqueroGateway                                                  | .NET 8                  | API Gateway — in progress                              |

---

## Ecosystem Health

| Service          | Port | Health              |
| ---------------- | ---- | ------------------- |
| SaqueroCloud     | 5000 | /health ✅          |
| SaqueroOrderCore | 8080 | /actuator/health ✅ |
| SaqueroJobs      | 5200 | /health ✅          |
| SaqueroGateway   | 5100 | in progress 🔜      |

---

## Future Improvements

- ARCHITECTURE.md documentation
- Unit and integration tests
- Refresh token support
- Docker Compose
- Deploy to Vercel + Render

---

<p align="center">
  <a href="https://linkedin.com/in/manusaquero">
    <img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white" />
  </a>
  <a href="mailto:manusaquero@gmail.com">
    <img src="https://img.shields.io/badge/Email-D14836?style=for-the-badge&logo=gmail&logoColor=white" />
  </a>
  <a href="https://github.com/Saquero">
    <img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white" />
  </a>
</p>
