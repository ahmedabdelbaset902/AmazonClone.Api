تمام يا أحمد 😎، خلينا نعمللك **نسخة محدثة من Documentation** تتماشى بالضبط مع اللي اشتغلت عليه في الـ Backend الحالي، مع الـ Cart, Orders, JWT Auth، و AutoMapper. ركّزت على اللي موجود فعليًا عندك.

---

# Amazon Clone Backend Documentation

## 1. Overview

المستند ده بيشرح تصميم وتطوير الـ **Backend** لمشروع **Amazon Clone** باستخدام:

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server

الـ Backend مسؤول عن:

* إدارة المستخدمين (Register, Login, JWT Authentication)
* إدارة المنتجات والتصنيفات
* إدارة عربة التسوق (Cart)
* إدارة الطلبات (Orders)
* المراجعات (Reviews)
* الربط مع Frontend Angular

---

## 2. Technology Stack

* **Framework:** ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Database:** SQL Server
* **Authentication:** JWT Bearer Token
* **Architecture:** Clean / Layered Architecture
* **Frontend:** Angular (Amazon Clone UI)

---

## 3. Project Architecture

### 3.1 Layers

* **API Layer**

  * Controllers
  * Request & Response DTOs

* **Business Layer (Bl)**

  * Services (CartService, OrderService, ProductService, CategoryService, AuthService)
  * Business Logic
  * AutoMapper Profiles

* **Domain Layer**

  * Entities (User, Product, Category, Cart, CartItem, Order, OrderItem, Review)
  * Enums (OrderStatus, Roles)

* **DAL / Infrastructure Layer**

  * DbContext
  * Repositories (GenericRepository, UnitOfWork)
  * EF Core Configurations

---

## 4. Database Design (ERD Concept)

### 4.1 Main Entities

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

### 5.1 User

* Id (PK)
* FirstName
* LastName
* Email
* PasswordHash
* CreatedAt
* Role (Admin / Customer)

### 5.2 Product

* Id (PK)
* Name
* Description
* Price
* StockQuantity
* CategoryId (FK)
* CreatedAt

### 5.3 Category

* Id (PK)
* Name
* ParentCategoryId (FK, nullable)

### 5.4 Cart

* Id (PK)
* UserId (FK)

### 5.5 CartItem

* Id (PK)
* CartId (FK)
* ProductId (FK)
* Quantity

### 5.6 Order

* Id (PK)
* UserId (FK)
* TotalAmount
* OrderStatus (Pending, Completed)
* CreatedAt

### 5.7 OrderItem

* Id (PK)
* OrderId (FK)
* ProductId (FK)
* Quantity
* Price

### 5.8 Review

* Id (PK)
* UserId (FK)
* ProductId (FK)
* Rating
* Comment

---

## 6. Relationships

* User (1) → (M) Orders
* User (1) → (1) Cart
* Cart (1) → (M) CartItems
* CartItem (M) → (1) Product
* Order (1) → (M) OrderItems
* OrderItem (M) → (1) Product
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
* POST `/api/auth/login` → Login and receive JWT token

### 7.2 Products

* GET `/api/products` → Get all products
* GET `/api/products/{id}` → Get product by ID
* POST `/api/products` → Create product (Admin)
* PUT `/api/products/{id}` → Update product
* DELETE `/api/products/{id}` → Delete product

### 7.3 Categories

* GET `/api/categories` → Get all categories
* POST `/api/categories` → Create category (Admin)

### 7.4 Cart

* GET `/api/cart/{userId}` → Get user's cart
* POST `/api/cart/add` → Add product to cart
* PUT `/api/cart/update` → Update product quantity
* DELETE `/api/cart/remove/{productId}` → Remove product from cart

### 7.5 Orders

* POST `/api/orders/checkout/{userId}` → Create order from user's cart
* GET `/api/orders/user/{userId}` → Get all orders for a user

---

## 8. Authentication & Authorization

* **JWT Token**
* **Roles:** Admin, Customer
* **Protect Endpoints** using `[Authorize]`
* Extract `UserId` from token for user-specific actions

---

## 9. AutoMapper Strategy

* CartItem → OrderItem (during checkout)
* Entity → DTO mapping for all responses

### 9.1 Example

```csharp
CreateMap<CartItem, OrderItem>().ReverseMap();
CreateMap<Order, OrderDto>()
    .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Items.Sum(i => i.Price * i.Quantity)))
    .ReverseMap();
```

---

## 10. Entity Framework Strategy

* Code First Approach
* Fluent API Configurations
* Migrations
* `GetAllQueryable()` for `Include` navigation properties

---

## 10.1 DbContext Example

```csharp
public class AmazonCloneContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(c => c.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId);

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Cart>(c => c.UserId);
    }
}
```

---

## 11. Error Handling

* Global Exception Middleware
* Standard API Response:

```json
{
  "success": false,
  "message": "Cart is empty",
  "data": null
}
```

---

## 12. Future Enhancements

* Payment Gateway Integration (Stripe / PayPal)
* Wishlist
* Coupons & Discounts
* Admin Dashboard
* Caching (Redis)

---

## 13. Notes for Angular Integration

* Use **DTOs only**
* Include **Bearer Token** in HTTP Headers
* Pagination & Filtering for Products



