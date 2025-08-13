<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="index.aspx.cs"
    Inherits="index" %>
<!DOCTYPE html>
<html lang="he" dir="rtl">
<head runat="server">
    <meta charset="UTF-8" />
    <title>טבלת צבעים</title>
    <link rel="stylesheet" href="style.css" />
       <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
</head>
<body>
  <form id="form1" runat="server">
<asp:ScriptManager ID="sm" runat="server" EnablePageMethods="true" />
 <div class="message">
  </div>
  <div class="page">
    <h1>טבלת צבעים</h1>

   <div class="card">
  <div class="table-wrap">
    <asp:Literal ID="litStatus" runat="server"></asp:Literal>
    <table class="colors-table">
      <thead>
        <tr>
          <th class="col-name">תיאור</th>
          <th class="col-price">מחיר</th>
          <th class="col-color">צבע</th>
          <th class="col-order">סדר</th>
          <th class="col-actions">פעולות</th>
        </tr>
      </thead>
      <tbody id="sortable">
        <% foreach (var c in list) { %>
        <tr data-id="<%: c.Id %>">
          <td class="col-name"><%: c.Name %></td>
          <td class="col-price"><%: c.Price.ToString("0.##") %></td>
            <td class="col-color">
            <span class="color-box" style='--color:<%: c.Description %>'>
              <%: c.Description %>
            </span>
          </td>
          <td class="col-order"><%: c.DisplayOrder %></td>
          <td class="col-actions">
            <button type="button" class="btn btn-danger">מחיקה</button>
            <button type="button" class="btn btn-muted">עריכה</button>
          </td>
        </tr>
        <% } %>
      </tbody>
    </table>
  </div>
</div>



<div class="card form-card">
  <div class="form-title">ערכים</div>

  <div class="color-form v2" id="colorForm">
    <div class="form2">
  
      <div class="form2-side">
        <div class="side-row"><span>שם הצבע</span></div>
        <div class="side-row"><span>מחיר </span></div>
        <div class="side-row"><span>סדר הצגה</span></div>
        <div class="side-row"><span>האם מאפיין זה במלאי</span></div>
      </div>

      <div class="form2-main">
  
        <div class="main-row">
          <input type="text" id="ColorName"  value=""  />
          <input type="color" id="ColorInput" class="input-color square" />
        </div>

        <div class="main-row">
          <input type="number" id="priceInput" min="0"/>
        </div>

        <div class="main-row">
          <input type="number" id="orderInput" min="0" />
        </div>

        <div class="main-row checkbox-row">
          <input type="checkbox" id="inStockInput" />
        </div>
      </div>
    </div>

    <div class="form-actions align-left">
      <button type="button" class="btn btn-primary" disabled>עדכן</button>
      <button type="button" class="btn btn-success">חדש</button>
    </div>
  </div>
</div>
</form>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
  <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
<script src="Script.js" charset="utf-8"></script>
</body>
