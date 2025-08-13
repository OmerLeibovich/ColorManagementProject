# ColorManagementProject

ToAllProject — Color Management Web App
📌 Overview
ToAllProject is an ASP.NET WebForms application that connects to a PostgreSQL database and allows managing a Colors table — including adding, updating, deleting, and reordering colors.
The UI interacts with the backend using AJAX WebMethods returning JSON.

⚙️ Technologies Used
ASP.NET WebForms (C#)

PostgreSQL (via Npgsql)

HTML / CSS / JavaScript / jQuery

AJAX for client–server communication

📂 Project Structure
graphql
Copy
Edit
ToAllProject/
│
├── index.aspx             # Main page UI
├── index.aspx.cs          # Backend logic (C#, WebMethods)
├── Modules/ColorDto.cs    # DTO for color objects
├── Web.config             # App configuration, DB connection string
└── Scripts/               # JavaScript & jQuery logic
🔑 Key Features
View colors ordered by display_order

Add new color (validates unique display_order)

Update color (by name)

Delete color (by id)

Save new order for multiple colors at once (transaction-based)

AJAX JSON API using [WebMethod]

🗄 Database Schema
Table: public."Colors"

Column	Type	Description
id	int	Primary Key
name	text	Color name
display_order	int	Order of appearance
price	float	Price
description	text	Additional info

🚀 Getting Started
1️⃣ Requirements
.NET Framework 4.x

PostgreSQL 9.6+

Visual Studio with ASP.NET support

2️⃣ Setup
Restore NuGet packages (including Npgsql).

Configure connection string in Web.config:

xml
Copy
Edit
<connectionStrings>
  <add name="PostgreSqlConnection" 
       connectionString="Host=localhost;Database=YOUR_DB;Username=USER;Password=PASS"
       providerName="Npgsql" />
</connectionStrings>
Ensure your Colors table exists and is populated.

3️⃣ Run
Open project in Visual Studio

Press F5 to start the web app

Access via http://localhost:PORT/index.aspx

📌 Notes
All WebMethods return JSON responses for easy AJAX integration.

Messages are localized in Hebrew.

Make sure all files are saved in UTF-8 to support Hebrew text.

