# 🔐 ASP.NET Core 8 JWT Authentication & Authorization API

![image alt](https://github.com/CoderMahmoud/JwtDemo/blob/17e7c3cfddf732528bdcc3fb8177611528080c02/logo.png).

## Overview

This repository implements a **complete and secure Authentication & Authorization system** in **ASP.NET Core Web API (.NET 8)** using **JWT**. It includes:

- Access & Refresh Token with **Token Rotation**
- **Password Hashing**
- **Token Revocation**
- **Email Confirmation** via token
- **Role Management**: assign/remove roles to/from users
- **Permission-based Authorization** with:
  - Custom Claims
  - Custom Policies
  - Custom Authorization Attributes
- **Background Service** to remove expired email confirmation tokens

✅ Built with **Clean Architecture**, **SOLID principles**, and uses **EF Core 8** with **SQL Server**  
📧 Email functionality is handled using **FluentEmail**

---

##  Technologies Used

- **ASP.NET Core 8**
- **Entity Framework Core 8**
- **SQL Server**
- **JWT Bearer Authentication**
- **FluentEmail** (for email delivery)
- **Background Services** (HostedService)
- **Clean Architecture**
- **SOLID Principles**

---

##  Features

### 🔐 Authentication

- Login with **Email & Password**
- **Access Token** (short-lived) & **Refresh Token** (long-lived)
- **Token Rotation** to prevent reuse
- **Revoke Refresh Tokens**
- **Secure Password Hashing** using ASP.NET Identity password hasher

### 📧 Email Verification

- Send email confirmation after registration
- Confirm email via secure token
- Background service to delete expired confirmation tokens

### 🛡️ Authorization

- **Role-based Access Control (RBAC)**
- Assign and remove roles from users
- Use `[Authorize(Roles = "Admin")]` to restrict endpoints
- **Permission-based Authorization** using:
  - Custom Claims (`"permission": "users.read"`)
  - Custom Policies (`RequirePermission`)
  - Custom Attributes (`[HasPermission("users.update")]`)

###  Role Management

- Admins can:
  - **Assign roles** to users (e.g., Admin, Moderator)
  - **Revoke roles** from users
- Each role can have predefined permissions

---

##  Architecture

The solution is structured following **Clean Architecture**:

