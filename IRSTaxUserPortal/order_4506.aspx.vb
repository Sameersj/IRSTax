﻿Imports System.Data.SqlClient
Imports System.IO
Imports IRSTaxRecords
Imports IRSTaxRecords.Core

Public Class order_4506
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Public Shared Function InsertOrder(
        ByVal newListId As Integer,
        ByVal listType As Integer,
        ByVal customerId As Integer,
        ByVal requestName As String,
        ByVal ssnNumber As String,
        ByVal taxYear2024 As String,
        ByVal taxYear2023 As String,
        ByVal taxYear2022 As String,
        ByVal taxYear2021 As String,
        ByVal taxYear2020 As String,
        ByVal typeOfForm As String,
        ByVal email As String,
        ByVal fax As String,
        ByVal faxNo As String,
        ByVal orderDate As DateTime,
        ByVal companyId As Integer,
        ByVal loanNumber As String,
        ByVal pdfFile As String,
        Optional dob As String = "",
        Optional gender As String = ""
        )

        Dim query As String = "
            INSERT INTO tblorder
            (
                fldListid,
                fldlisttype,
                fldCustomerID,
                fldRequestName,
                fldssnno,
                fldTaxyear2024,
                fldTaxyear2023,
                fldTaxyear2022,
                fldTaxYear2021,
                fldTaxYear2020,
                fldtypeofform,
                fldemail,
                fldfax,
                fldfaxno,
                fldStatus,
                fldBillingStatus,
                fldOrderDate,
                fldCompanyID,
                fldLoanNumber,
                fldPdf,
                fldDOB,
                fldSex
            )
            VALUES
            (
                @NewListId,
                @ListType,
                @CustomerID,
                @RequestName,
                @SSNNumber,
                @TaxYear2024,
                @TaxYear2023,
                @TaxYear2022,
                @TaxYear2021,
                @TaxYear2020,
                @TypeOfForm,
                @Email,
                @Fax,
                @FaxNo,
                'p',
                0,
                @OrderDate,
                @CompanyID,
                @LoanNumber,
                @PDFFile,
                @DOB,
                @Sex
            )"
        Dim connStr As String = ConfigurationManager.ConnectionStrings("IRSConnection").ConnectionString
        Using connection As New SqlConnection(connStr)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@NewListId", newListId)
                command.Parameters.AddWithValue("@ListType", listType)
                command.Parameters.AddWithValue("@CustomerID", customerId)
                command.Parameters.AddWithValue("@RequestName", requestName)
                command.Parameters.AddWithValue("@SSNNumber", ssnNumber)
                command.Parameters.AddWithValue("@TaxYear2024", If(taxYear2024, DBNull.Value))
                command.Parameters.AddWithValue("@TaxYear2023", If(taxYear2023, DBNull.Value))
                command.Parameters.AddWithValue("@TaxYear2022", If(taxYear2022, DBNull.Value))
                command.Parameters.AddWithValue("@TaxYear2021", If(taxYear2021, DBNull.Value))
                command.Parameters.AddWithValue("@TaxYear2020", If(taxYear2020, DBNull.Value))
                command.Parameters.AddWithValue("@TypeOfForm", typeOfForm)
                command.Parameters.AddWithValue("@Email", email)
                command.Parameters.AddWithValue("@Fax", fax)
                command.Parameters.AddWithValue("@FaxNo", faxNo)
                command.Parameters.AddWithValue("@OrderDate", orderDate)
                command.Parameters.AddWithValue("@CompanyID", companyId)
                command.Parameters.AddWithValue("@LoanNumber", loanNumber)
                command.Parameters.AddWithValue("@PDFFile", pdfFile)
                command.Parameters.AddWithValue("@DOB", dob)
                command.Parameters.AddWithValue("@Sex", gender)

                connection.Open()
                command.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Shared Function UploadFile(file As HttpPostedFile, uploadFolderPath As String) As String
        If file Is Nothing OrElse file.ContentLength = 0 Then
            Return Nothing
        End If

        Try
            Dim serverPath As String = HttpContext.Current.Server.MapPath(uploadFolderPath)
            If Not Directory.Exists(serverPath) Then
                Directory.CreateDirectory(serverPath)
            End If

            Dim extension As String = Path.GetExtension(file.FileName)
            Dim newFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") & "_" & Guid.NewGuid().ToString() & extension


            Dim filePath As String = Path.Combine(serverPath, newFileName)
            file.SaveAs(filePath)

            Return filePath
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function ValidateForm() As Boolean
        Dim msg As String = ""

        If txtTaxPayerName.Text.Trim = "" Then msg &= "Please enter Tax Payer Name.<br>"
        If txtSocialSecurityNumber.Text.Trim = "" Then msg &= "Please enter SSN.<br>"
        If Me.txtLoanNumber.Visible AndAlso txtLoanNumber.Text.Trim = "" Then msg &= "Please enter Loan Number.<br>"
        If SelectedIDs(Me.chkTaxyears).Count = 0 Then msg &= "Please select at least one year to order.<br>"
        If Me.rdoTaxForms.SelectedItem Is Nothing Then msg &= "Please select at least one form type.<br>"
        If Not fuform4506C.HasFile Then msg &= "Please attach a PDF file.<br>"

        If msg = "" Then
            Return True
        Else
            lblMessage.Text = msg
            Return False
        End If
    End Function
    Private Function GetTypeOfFormsSelected() As Generic.List(Of TypeOfForm)
        Dim typeOfForms As New Generic.List(Of TypeOfForm)
        Select Case Me.rdoTaxForms.SelectedValue
            Case "1040"
                typeOfForms.Add(TypeOfForm.S_1040)
            Case "1040/W2"
                typeOfForms.Add(TypeOfForm.S_1040)
                typeOfForms.Add(TypeOfForm.S_W2)
            Case "1120"
                typeOfForms.Add(TypeOfForm.S_1120)
            Case "1065"
                typeOfForms.Add(TypeOfForm.S_1065)
            Case "W-2"
                typeOfForms.Add(TypeOfForm.S_W2)
            Case "1099"
                typeOfForms.Add(TypeOfForm.S_1099)
            Case Else
                Throw New Exception("Form type " & rdoTaxForms.SelectedValue & " is not a valid form type.")
        End Select
        Return typeOfForms
    End Function
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Not ValidateForm() Then Return
        Dim savedFilePath As String = Nothing
        If fuform4506C.HasFile Then
            savedFilePath = UploadFile(fuform4506C.PostedFile, "~/Uploads/")
        End If
        Dim years As Generic.List(Of Integer) = SelectedIDs(chkTaxyears)

        Dim resultOrderIDs As New Generic.List(Of Integer)
        Dim typeOfForms As Generic.List(Of TypeOfForm) = GetTypeOfFormsSelected()
        Dim o As New Orders.Order
        With o
            .fldCompanyID = StoreInstance.GetCustomerId()
            .fldcustomeriD = StoreInstance.GetCustomerId()
            .fldemail = "OFF"
            .fldfax = "OFF"
            .fldfaxno = ""
            .fldListid = 0
            .fldlisttype = 1
            .fldLoanNumber = Me.txtLoanNumber.Text.Trim
            .fldOrderdate = Now.ToShortDateString
            .fldrequestname = txtTaxPayerName.Text.Trim()
            .fldssnno = txtSocialSecurityNumber.Text.Trim()
            .fldstatus = "p"
            .fldPdf = fuform4506C.PostedFile.FileName
            For Each Year As Integer In years
                Select Case Year
                    Case 2020 : .fldTaxyear2020 = True
                    Case 2021 : .fldTaxyear2021 = True
                    Case 2022 : .fldTaxyear2022 = True
                    Case 2023 : .fldTaxyear2023 = True
                    Case 2024 : .fldTaxyear2024 = True
                End Select
            Next
            For Each formType As TypeOfForm In typeOfForms
                Dim listType As ListTypeCodeType = ListServices.GetListTypeFromFormType(formType)

                Dim listID As Integer = ListServices.GetCurrentListID(listType)

                .fldListid = listID
                .fldlisttype = CInt(listType)
                .FormType = formType
                .fldordernumber = 0

                OrderServices.CreateNewOrder(o)
                If o.fldordernumber < 1 Then
                    lblMessage.Text = "Failed to save order. " & DataHelper.LastErrorMessage
                End If
                resultOrderIDs.Add(o.fldordernumber)
            Next
        End With

        Response.Redirect("~/Confirmation.aspx?form=" & 4506)
    End Sub

    Private Function SelectedIDs(ByVal chk As CheckBoxList) As Generic.List(Of Integer)
        Dim list As New Generic.List(Of Integer)
        For Each item As ListItem In chk.Items
            If item.Selected Then list.Add(item.Value)
        Next
        Return list
    End Function

End Class