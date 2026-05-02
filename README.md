<p align="center">
  <img src="./assets/logo.svg" width="80" />
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

### 🔧 1. Clonar el repositorio

git clone https://github.com/Saquero/SaqueroCloud.git
cd SaqueroCloud

---

## 🖥️ 2. Backend (.NET API)

cd SaqueroCloud.API
dotnet restore
dotnet run --urls="http://127.0.0.1:5000"

API disponible en:
http://127.0.0.1:5000

Swagger:
http://127.0.0.1:5000/swagger

---

## 🌐 3. Frontend (React)

cd saquerocloud-frontend
npm install
npm run dev

Frontend disponible en:
http://localhost:5173

---

## 🔐 Credenciales de prueba

Email: Saquero@pruebas.com  
Password: Admin1234!

---

## 🔗 API Endpoints

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

---

## 💡 Why this project?

Este proyecto **no es un CRUD más**.  
Demuestra:

- Arquitectura real de un SaaS  
- Implementación de lógica de negocio  
- Integración full‑stack  
- API con diseño de estilo producción  
- Flujos completos de panel de administración  

---

## 🚀 Future Improvements

- Deploy con Vercel + Render  
- Tests automatizados  
- Refresh tokens  
- Mejoras de UI/animaciones  
- Dashboard de analíticas  

---

## ⭐ ¿Te ha gustado?

Si este proyecto te aporta valor:

- Dale una estrella ⭐  
- Úsalo como base  
- Conecta conmigo 🤝
---

## 📬 Contacto

💼 Proyecto creado por 👉 [**Manu Saquero**](https://www.linkedin.com/in/manusaquero/)  

🧠 Software Developer  
🚀 Apasionado por crear productos reales
