Imports System.Data

Public Class ISO
    Public TableName As String = "ISO"
    Public MainId As String = "CustomerId"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return

        MyAttach.Flag = MyAttachments.MyFlag.ISO

        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})

        bm.FillCombo("ShippingMethods", ShippingMethodId, "")

        bm.FillCombo("Ports", PortId, "")
        bm.FillCombo("Ports2", ToPortId, "")
        bm.FillCombo("Countries", CountryId, "")
        bm.FillCombo("ShippingLines", ShippingLineId, "")
        bm.FillCombo("Forwarders", ForwarderId, "")
        bm.FillCombo("Truckers", TruckerId, "")
        bm.FillCombo("Stations", StationId, "")
        bm.FillCombo("Varieties", VarietyId, "")


        LoadResource()
        bm.Fields = New String() {MainId, SubId, SubId2, "DayDate", "Canceled", "IsPosted", "ShippingMethodId", "NoOfContainers", "ShippingInvoiceNo", "PortId", "CountryId", "ShippingLineId", "ForwarderId", "TruckerId", "StationId", "VarietyId", "PrimaryFreight", "ToPortId", "DayDate2", "DayDate3", "DayDate6", "DayDate7", "DayDate9", "BillNo", "FileNo"}
        bm.control = New Control() {CustomerId, txtFlag, txtID, DayDate, Canceled, IsPosted, ShippingMethodId, NoOfContainers, ShippingInvoiceNo, PortId, CountryId, ShippingLineId, ForwarderId, TruckerId, StationId, VarietyId, PrimaryFreight, ToPortId, DayDate2, DayDate3, DayDate6, DayDate7, DayDate9, BillNo, FileNo}
        bm.KeyFields = New String() {MainId, SubId, SubId2}
        bm.Table_Name = TableName


        btnNew_Click(sender, e)
        CustomerId.Focus()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True


        bm.FillControls(Me)
        CustomerId_LostFocus(Nothing, Nothing)

        MyAttach.Key1 = Val(CustomerId.Text)
        MyAttach.Key2 = Val(txtID.Text)
        MyAttach.LoadTree()

        DayDate.Focus()
        lop = False
        
        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
        If Val(CustomerId.Text) = 0 Then
            bm.ShowMSG("Please, Select a customer")
            CustomerId.Focus()
            Return
        End If
        If Not bm.TestDateValidity(DayDate) Then Return
         

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If


        bm.DefineValues()


        If Not bm.Save(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text.Trim, txtID.Text}) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True
    End Sub

    Dim lop As Boolean = False



    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        If lop OrElse lv Then Return
        lop = True

        DayDate.SelectedDate = bm.MyGetDate()

        PrimaryFreight.Clear()

        CustomerId_LostFocus(Nothing, Nothing)
        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtFlag.Text = Flag
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & MainId & "=" & CustomerId.Text & " and " & SubId & "=" & txtFlag.Text)
        If txtID.Text = "" Then txtID.Text = "1"

        MyAttach.Key1 = Val(CustomerId.Text)
        MyAttach.Key2 = Val(txtID.Text)
        MyAttach.LoadTree()

        'DayDate.Focus()
        txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & MainId & "=" & CustomerId.Text & " and " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text.Trim, txtID.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles PrimaryFreight.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub CustomerId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustomerId.LostFocus

        If Val(CustomerId.Text.Trim) = 0 Then
            CustomerId.Clear()
            CustomerName.Clear()
            Return
        End If
        bm.LostFocus(CustomerId, CustomerName, "select Name from Customers where Id=" & CustomerId.Text.Trim())
        If lop Then Return
        btnNew_Click(Nothing, Nothing)
    End Sub
    Private Sub CustomerId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CustomerId.KeyUp
        If bm.ShowHelp("Customers", CustomerId, CustomerName, e, "select cast(Id as varchar(100)) Id,Name from Customers") Then
            CustomerId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")

        'lblDayDate.SetResourceReference(Label.ContentProperty, "DayDate")
        lblNotes.SetResourceReference(Label.ContentProperty, "Notes")
    End Sub



    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False



    Private Sub BillNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles BillNo.LostFocus
        dt = bm.ExecuteAdapter("select top 1 * from Bills where BillNo='" & BillNo.Text.Trim & "'")
        If dt.Rows.Count > 0 Then
            FileNo.Text = dt.Rows(0)("FileNo")
            DayDate2.Text = dt.Rows(0)("DayDate2")
            DayDate3.Text = dt.Rows(0)("DayDate3")
            DayDate6.Text = dt.Rows(0)("DayDate6")
            DayDate7.Text = dt.Rows(0)("DayDate7")
            DayDate9.Text = dt.Rows(0)("DayDate9")
        End If
    End Sub

    Private Sub FileNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles FileNo.LostFocus
        dt = bm.ExecuteAdapter("select top 1 * from Bills where FileNo='" & FileNo.Text.Trim & "'")
        If dt.Rows.Count > 0 Then
            BillNo.Text = dt.Rows(0)("BillNo")
            DayDate2.Text = dt.Rows(0)("DayDate2")
            DayDate3.Text = dt.Rows(0)("DayDate3")
            DayDate6.Text = dt.Rows(0)("DayDate6")
            DayDate7.Text = dt.Rows(0)("DayDate7")
            DayDate9.Text = dt.Rows(0)("DayDate9")
        End If
    End Sub


End Class
