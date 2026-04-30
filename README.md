<p align="center">
  <img src="./saquerocloud-frontend/public/favicon.svg" width="80" />
</p>

<h1 align="center">☁️ SaqueroCloud</h1>

<p align="center">
  SaaS Admin Platform for managing users, subscription plans and billing lifecycle
</p>

---

<p align="center">

![API](https://img.shields.io/badge/API-ASP.NET%20Core-blue?style=for-the-badge&logo=dotnet)
![Frontend](https://img.shields.io/badge/Frontend-React-61DAFB?style=for-the-badge&logo=react)
![Auth](https://img.shields.io/badge/Auth-JWT-green?style=for-the-badge)
![Architecture](https://img.shields.io/badge/Architecture-Clean-orange?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Active-success?style=for-the-badge)

</p>

---

## 🚀 What is SaqueroCloud?

SaqueroCloud is a full stack SaaS-style admin platform built with **ASP.NET Core (.NET 8)** and **React**.

It simulates a real-world subscription system where administrators can manage:

- Users 👤  
- Subscription plans 📦  
- Active subscriptions 🔄  
- Billing lifecycle 💳  

---

## 📸 Preview

### 📊 Dashboard
![Dashboard](./assets/dashboard.png)

### 👤 Users
![Users](./assets/users.png)

### 📦 Plans
![Plans](./assets/plans.png)

### 🔄 Subscriptions
![Subscriptions](./assets/subscriptions.png)

---

## 🧠 Key Features

✔ JWT Authentication  
✔ Role-based access control  
✔ User management  
✔ Subscription plan management  
✔ Assign & cancel subscriptions  
✔ Filter subscriptions by plan  
✔ Expiring subscriptions tracking  
✔ REST API + Swagger  
✔ Full React admin dashboard  

---

## 🛠️ Tech Stack

### 🔙 Backend API
- C#  
- ASP.NET Core (.NET 8)  
- Entity Framework Core  
- JWT Authentication  
- Swagger  

### 🖥️ Frontend
- React  
- Vite  
- Axios  
- CSS (SaaS UI)  

### ⚙️ Tools
- Git  
- PowerShell  
- Swagger  
- REST Client  

---

## 📦 Setup

### Backend

```bash
cd SaqueroCloud.API
dotnet run --urls="http://127.0.0.1:5000"
Frontend
cd saquerocloud-frontend
npm install
npm run dev
🔐 Credentials
Email: Saquero@pruebas.com
Password: Admin1234!
🔗 API Endpoints
Auth
POST /api/Auth/login
POST /api/Auth/register
Users
GET /api/Users
Plans
GET /api/subscription-plans
Subscriptions
POST /api/Subscriptions/assign/{userId}
PATCH /api/Subscriptions/{id}/cancel
💡 Why this project?

This is not just a CRUD.

It demonstrates:

✔ Real SaaS architecture
✔ Business logic implementation
✔ Full stack integration
✔ Production-style API design
✔ Admin dashboard workflows

🚀 Future Improvements
Deploy (Vercel + Render)
Add tests
Add refresh tokens
Improve UI animations
Add analytics dashboard
⭐ Support

If you like this project:

👉 Give it a star
👉 Fork it
👉 Use it as a base

📬 Contact

Created by Manu Saquero

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