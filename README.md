# Agriculture Management System

An ASP.NET Web Forms (.NET Framework 4.8) web application for farmers and visitors.
Written in the **semester 5 classroom style**: plain ADO.NET (`SqlConnection`, `SqlCommand`,
`SqlDataAdapter`, `DataSet`), `GridView` / `DataList`, `Session`, `ViewState`, and small helper
methods such as `getcon()`, `gridfill()`, `fillupdate()`, `Imgupload()` and `clear()`.

Every page has its own code-behind that opens its own connection with
`ConfigurationManager.ConnectionStrings["dbcon"]` (see `Web.config`) and runs its own SQL.
There is no extra layer (no DAL, BLL, base page or model classes).

## Pages (15)
| Area | Pages |
|------|-------|
| **Public** | Home (Default), Products, Cart, Equipment, EquipmentCart, Contact |
| **Farmer** | FarmerLogin, FarmerRegister, FarmerDashboard, AddCrop, ViewCrop (My Crops), AddProduct, ManageProducts (My Products), ApplyEquipment, Payment |

A farmer must be logged in to use the Cart and the Equipment rental cart. Both carts are stored in
the database (tables `Cart` and `EquipCart`, one row per farmer per item).

## Setup

### 1. Database (SQL Server Management Studio, in this order)
1. `Database/AgricultureDB.sql` - creates the database, tables (including the `Cart` and `EquipCart` tables) and seed data.
   **It DROPS and recreates the tables, so run it only on a fresh/test database.**
2. `Database/Migrations/02_Schema_Upgrades.sql` - required (payment columns, photo columns, etc.).
3. Optional: `03_Seed_More_Equipment.sql` and `04_Equipment_RealPhotos.sql` (more equipment with photos).

Tables used: Farmers, Crops, Products, Orders, Equipment, EquipmentApplications, Payments, Contact, Cart, EquipCart.

**Already have a database from the old version?** Do NOT run `AgricultureDB.sql` (it deletes your data). Instead run:
- `11_Cart_Tables.sql` - adds the two cart tables (required, otherwise you get "Invalid object name 'Cart'").
- `12_OPTIONAL_Remove_Unused_Tables.sql` - deletes the tables of the removed pages (Admin, Fertilizers, Feedback, Crop gallery).

Seeded farmer login: `hopempambara@example.com` / `Hope@123` (change it after the first login).

### 2. Connection string
Edit `dbcon` in `Web.config`:
```xml
<add name="dbcon"
     connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=AgricultureDB;Integrated Security=True;" ... />
```

### 3. Run
Open `AgricultureManagementSystem.sln` in Visual Studio, press **F5**.

## How the code is organised
```
Public/     Default, Products, Cart, Equipment, EquipmentCart, Contact
Farmer/     login, register, dashboard, crops, products, equipment application, payment
Database/   SQL scripts only (AgricultureDB.sql + Migrations/)
Images/     Crops, Products, Equipment, Farmers photos
```
- **Login:** `Session["farmer"]` = the farmer's email, `Session["fid"]` = FarmerID, `Session["fname"]` = name.
  Each farmer page checks `Session["farmer"] != null` in `Page_Load` and redirects to the login page if not.
- **Passwords** are stored as SHA-256 hashes (the hashing is done inside the SQL query).
- **Edit / delete buttons** use `CommandName` ("cmd_edt", "cmd_dlt") and `CommandArgument`, and the
  selected id is kept in `ViewState["id"]`.

## Known limitations (good to know)
- SQL is written as joined text (`"... where Email='" + txtEmail.Text + "'"`) exactly like the class notes.
  It works, but it is open to SQL injection and breaks if a text contains an apostrophe. In a real
  project use `cmd.Parameters.AddWithValue(...)`.
- The payment page is a simulation: it does not contact a bank.
- Like the class notes, `getcon()` opens a connection and the code never closes it. For a class demo
  this is fine; in a real project close the connection (`con.Close()`) or use a `using` block.
