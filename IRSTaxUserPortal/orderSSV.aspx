﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="orderSSV.aspx.vb" Inherits="IRSTaxUserPortal.orderSSV" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>orderSSV</title>
    <!-- Bootstrap Icons -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- Centering Wrapper -->
  <div class="d-flex justify-content-center align-items-center min-vh-100">
    <div class="card shadow border-0" style="max-width: 720px; width: 100%;">
      <!-- Logo at the top -->
      <div class="card-header bg-primary bg-opacity-10 text-center border-bottom">
        <img src="/img/OrderSSV.png" alt="Logo" class="img-fluid" style="max-height: 80px;" />
      </div>

      <div class="card-body p-3">
          <asp:Label ID="lblMessage" runat="server" 
                CssClass="text-danger fw-bold d-block mb-3"></asp:Label>
        <!-- Row 1: Name + SSN -->
        <div class="row mb-3 pb-2 border-bottom border-dark bg-primary bg-opacity-10 rounded p-3">
          <div class="col-md-6 mb-2">
            <label class="form-label fw-semibold small mb-1">Taxpayer Full Name:</label>
            <asp:TextBox  ID="txtTaxPayerName" class="form-control form-control-sm" runat="server"   MaxLength="50" ></asp:TextBox>
          </div>
          <div class="col-md-6">
            <label class="form-label fw-semibold small mb-1">Social Security Number:</label>
             <asp:TextBox ID="txtSocialSecurityNumber" class="form-control form-control-sm" runat="server"   MaxLength="50" placeholder="145-74-9891"></asp:TextBox>
            <small class="text-muted">Example Format: 145-74-9891</small>
          </div>
        </div>

        <!-- Row 2: DOB + Gender -->
        <div class="row mb-3 pb-2 border-bottom border-dark p-3">
          <div class="col-md-6 mb-2">
            <label class="form-label fw-semibold small mb-1">Date of Birth:</label>
             <asp:TextBox  ID="txtDob" class="form-control form-control-sm" runat="server"   MaxLength="10" placeholder="04/15/1941" ></asp:TextBox>
            <small class="text-muted">Example Format: 04/15/1941</small>
          </div>
          <div class="col-md-6">
            <label class="form-label fw-semibold small mb-2 d-block">Gender:</label>
            <div class="form-check form-check-inline">
              <input type="radio" name="gender" class="form-check-input" id="rbMale" value="Male" runat="server"  Checked="True" />
              <label class="form-check-label small" for="rbMale">Male</label>
            </div>
            <div class="form-check form-check-inline">
              <input type="radio" name="gender" class="form-check-input" id="rbFemale" value="Female"  runat="server"  />
              <label class="form-check-label small" for="rbFemale">Female</label>
            </div>
          </div>
        </div>

        <!-- Row 3: Loan Number -->
        <div class="mb-3 pb-2 border-bottom border-dark bg-primary bg-opacity-10 rounded p-3">
          <label class="form-label fw-semibold small mb-1">Loan Number:</label>
         <asp:TextBox type="text" ID="txtLoanNumber" class="form-control form-control-sm" runat="server"   MaxLength="50" ></asp:TextBox>
        </div>

        <!-- Row 4: File Upload -->
        <div class="mb-3 pb-2 border-bottom border-dark p-3">
          <label class="form-label fw-semibold small mb-2">Upload Form SSA-89:</label>
           <asp:FileUpload  ID="ssa89File" class="form-control form-control-sm"  runat="server"  />
        </div>

        <!-- Submit -->
        <div class="text-center bg-primary bg-opacity-10 py-3 rounded mt-3">
            <asp:Button ID="btnSubmit" runat="server" Text="Upload and Submit Order" class="btn btn-primary px-5 fw-bold"/>
        </div>
      </div>
    </div>
  </div>
    </form>
</body>
</html>
