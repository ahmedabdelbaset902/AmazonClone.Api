# Amazon Clone Backend Documentation

## 1. Overview

This document describes the **Backend architecture and implementation** of the **Amazon Clone** project, built using **ASP.NET Core Web API** with a focus on clean code, scalability, and real-world backend practices.

The backend is responsible for:

* User authentication & authorization (JWT)
* Product & category management
* Shopping cart management
* Order lifecycle management
* Reviews
* Payment processing (Stripe)
* Integration with Angular frontend

---

## 2. Technology Stack

* **Framework:** ASP.NET Core Web API (.NET 8)
* **ORM:** Entity Framework Core
* **Database:** SQL Server
* **Authentication:** JWT Bearer Token
* **Architecture:** Clean / Layered Architecture
* **Logging:** Serilog (File & Console)
* **Documentation:** Swagger (OpenAPI)
* **Payment Gateway:** Stripe (PaymentIntent & Webhooks)
* **Frontend:** Angular

---

## 3. Project Architecture

The project follows **Clean Architecture** principles with clear separation of concerns.

### 3.1 Layers

#### API Layer

* Controllers
* Request & Response DTOs
* Authentication & Authorization attributes
* Swagger configuration

#### Business Layer (BL)

* Services (AuthService, ProductService, CategoryService, CartService, OrderService, PaymentService)
* Business rules & validations
* AutoMapper profiles

#### Domain Layer

* Entities (User, Product, Category, Cart, CartItem, Order, OrderItem, Review)
* Enums (OrderStatus, Roles)
* BaseEntity (Id, CreatedAt, Status)

#### DAL / Infrastructure Layer

* DbContext
* Generic Repository
* Unit of Work
* EF Core Fluent Configurations

---

## 4. Database Design (ERD Concept)

### 4.1 Core Entities

* User
* Product
* Category
* Cart
* CartItem
* Order
* OrderItem
* Review

---

## 5. Entity Definitions

### User

* Id (PK)
* FirstName
* LastName
* Email
* PasswordHash
* Role (Admin / Customer)
* CreatedAt

### Product

* Id (PK)
* Name
* Description
* Price
* StockQuantity
* CategoryId (FK)
* CreatedAt

### Category

* Id (PK)
* Name
* ParentCategoryId (FK, Nullable)

### Cart

* Id (PK)
* UserId (FK)

### CartItem

* Id (PK)
* CartId (FK)
* ProductId (FK)
* Quantity

### Order

* Id (PK)
* UserId (FK)
* TotalAmount
* OrderStatus (Pending, Paid, Completed)
* PaymentIntentId
* CreatedAt

### OrderItem

* Id (PK)
* OrderId (FK)
* ProductId (FK)
* Quantity
* Price

### Review

* Id (PK)
* UserId (FK)
* ProductId (FK)
* Rating
* Comment

---

## 6. Relationships

* User (1) → (1) Cart
* User (1) → (M) Orders
* Cart (1) → (M) CartItems
* Order (1) → (M) OrderItems
* Product (1) → (M) Reviews
* Category (1) → (M) Products
* Category (1) → (M) SubCategories

---

## 6.1 ERD (Conceptual)

```
User ──┬── Cart ── CartItem ── Product ── Category
       │                           │
       ├── Order ── OrderItem ─────┘
       │
       └── Review ─────────────────┘
```

---

## 7. API Endpoints

### 7.1 Authentication

* POST `/api/auth/register` → Register new user
* POST `/api/auth/login` → Login & receive JWT token

### 7.2 Products

* GET `/api/products`
* GET `/api/products/{id}`
* POST `/api/products` (Admin)
* PUT `/api/products/{id}` (Admin)
* DELETE `/api/products/{id}` (Admin)

### 7.3 Categories

* GET `/api/categories`
* POST `/api/categories` (Admin)

### 7.4 Cart

* GET `/api/cart`
* POST `/api/cart/add`
* PUT `/api/cart/update`
* DELETE `/api/cart/remove/{productId}`

### 7.5 Orders

* POST `/api/orders/checkout`
* GET `/api/orders/user`

### 7.6 Payments (Stripe)

* POST `/api/payments/create-intent`
* POST `/api/payments/webhook`

---

## 8. Authentication & Authorization

* JWT Bearer Token
* Role-based authorization (Admin / Customer)
* UserId extracted from token claims
* Secure endpoints using `[Authorize]`

---

## 9. AutoMapper Strategy

* Entity ↔ DTO mapping
* CartItem → OrderItem mapping during checkout
* Centralized mapping profiles

```csharp
CreateMap<CartItem, OrderItem>();
CreateMap<Order, OrderDto>()
    .ForMember(d => d.TotalAmount,
        o => o.MapFrom(s => s.Items.Sum(i => i.Price * i.Quantity)));
```

---

## 10. Entity Framework Strategy

* Code First
* Fluent API configurations
* Migrations
* Includes using `GetAllQueryable()`

---

## 11. Error Handling & Logging

* Global Exception Handling Middleware
* Unified API response structure
* Serilog for structured logging

```json
{
  "success": false,
  "message": "Cart is empty",
  "data": null
}
```

---

## 12. Future Enhancements

* Wishlist
* Coupons & Discounts
* Admin Dashboard
* Redis Caching
* Advanced Search & Filtering

---

## 13. Notes for Angular Integration

* DTO-based communication only
* JWT Bearer Token in headers
* Pagination & filtering supported
