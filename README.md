# ColorManagementProject

# 🎨 ToAllProject — Color Management Web App

## 📌 Overview
**ToAllProject** is an **ASP.NET WebForms** application that connects to a **PostgreSQL** database  
and allows managing a **Colors** table — including:
- Adding
- Updating
- Deleting
- Reordering colors  

The **UI** communicates with the backend using **AJAX WebMethods** returning **JSON**.

---

## ⚙️ Technologies Used
- **ASP.NET WebForms (C#)**
- **PostgreSQL** (via `Npgsql`)
- **HTML / CSS / JavaScript / jQuery**
- **AJAX** for client–server communication

---

## 📂 Project Structure
```
ToAllProject/
│
├── index.aspx # Main page UI
├── index.aspx.cs # Backend logic (C#, WebMethods)
├── Modules/ColorDto.cs # DTO for color objects
├── Web.config # App configuration, DB connection string
└── Scripts/ # JavaScript & jQuery logic
```

---

## 🔑 Key Features
✅ View colors ordered by **display_order**  
✅ Add new color (**validates unique display_order**)  
✅ Update color (by **name**)  
✅ Delete color (by **id**)  
✅ Save new order for multiple colors at once (**transaction-based**)  
✅ AJAX **JSON API** using `[WebMethod]`  

---

## 🗄 Database Schema

**Table:** `public."Colors"`

| Column        | Type  | Description            |
|---------------|-------|------------------------|
| `id`          | int   | Primary Key            |
| `name`        | text  | Color name             |
| `display_order` | int | Order of appearance    |
| `price`       | float | Price                  |
| `description` | text  | Additional info        |

---

## 🚀 Getting Started

### 1️⃣ Requirements
- .NET Framework **4.x**
- PostgreSQL **9.6+**
- Visual Studio with **ASP.NET** support

### 2️⃣ Setup
1. Restore NuGet packages (including `Npgsql`).
2. Configure the connection string in **Web.config**:
```xml
<connectionStrings>
  <add name="PostgreSqlConnection" 
       connectionString="Host=localhost;Database=YOUR_DB;Username=USER;Password=PASS"
       providerName="Npgsql" />
</connectionStrings>

Ensure your Colors table exists and is populated.

3️⃣ Run
Open the project in Visual Studio

Press F5 to start the web app

Access via:

bash
Copy
Edit
http://localhost:PORT/index.aspx
📌 Notes
All WebMethods return JSON responses for easy AJAX integration.

Messages are localized in Hebrew.

Ensure all files are saved in UTF-8 to support Hebrew text.

