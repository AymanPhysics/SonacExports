Imports System.Data

Public Class Bills
    Public TableName As String = "Bills"
    Public SubId As String = "Id"
    Public SubName As String = "BillNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()
        bm.FillCombo("ShippingMethods", ShippingMethodId, "")
        bm.FillCombo("Ports", PortId, "")
        bm.FillCombo("Ports2", ToPortId, "")
        bm.FillCombo("Countries", CountryId, "")
        bm.FillCombo("ShippingLines", ShippingLineId, "")
        bm.FillCombo("Forwarders", ForwarderId, "")
        bm.FillCombo("Truckers", TruckerId, "")
        bm.FillCombo("Stations", StationId, "")
        bm.FillCombo("Varieties", VarietyId, "")


        MyAttach.Flag = MyAttachments.MyFlag.Bills

        bm.Fields = {SubId, SubName, "DayDate1", "DayDate2", "DayDate3", "DayDate4", "DayDate5", "DayDate6", "DayDate7", "DayDate8", "DayDate9", "DayDate10", "FileNo", "ShippingLineId", "ShippingMethodId", "PortId", "CountryId", "ForwarderId", "TruckerId", "StationId", "VarietyId", "ToPortId"}
        bm.control = {txtID, BillNo, DayDate1, DayDate2, DayDate3, DayDate4, DayDate5, DayDate6, DayDate7, DayDate8, DayDate9, DayDate10, FileNo, ShippingLineId, ShippingMethodId, PortId, CountryId, ForwarderId, TruckerId, StationId, VarietyId, ToPortId}
        bm.KeyFields = {SubId}

        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(Application.Current.MainWindow, MainWindow).TabControl1.Items.Remove(Me.Parent)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If BillNo.Text.Trim = "" Then
            BillNo.Focus()
            Return
        End If
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        btnNew_Click(sender, e)

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        MyAttach.Key1 = Val(txtID.Text)
        MyAttach.Key2 = Val(0)
        MyAttach.LoadTree()

        BillNo.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG("MsgDelete") Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Sub FillControls()
        bm.FillControls(Me)

        MyAttach.Key1 = Val(txtID.Text)
        MyAttach.Key2 = Val(0)
        MyAttach.LoadTree()

    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyUp
        bm.ShowHelp(CType(Parent, Page).Title, txtID, BillNo, e, "select cast(Id as varchar(100)) Id," & SubName & " Name from " & TableName)
    End Sub

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            BillNo.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        BillNo.SelectAll()
        BillNo.Focus()

        BillNo.SelectAll()
        BillNo.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub



    'Private Sub MyBase_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    If Not btnSave.Enabled Then Exit Sub
    '    Select Case bm.RequestDelete
    '        Case BasicMethods.CloseState.Yes
    '            
    '            btnSave_Click(Nothing, Nothing)
    '            If Not AllowClose Then e.Cancel = True
    '        Case BasicMethods.CloseState.No

    '        Case BasicMethods.CloseState.Cancel
    '            e.Cancel = True
    '    End Select
    'End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")

    End Sub

    Private Sub FileNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles FileNo.LostFocus
        dt = bm.ExecuteAdapter("select top 1 * from ISO where FileNo='" & FileNo.Text.Trim & "'")
        If dt.Rows.Count > 0 Then
            BillNo.Text = dt.Rows(0)("BillNo")
            DayDate2.Text = dt.Rows(0)("DayDate2")
            DayDate3.Text = dt.Rows(0)("DayDate3")
            DayDate6.Text = dt.Rows(0)("DayDate6")
            DayDate7.Text = dt.Rows(0)("DayDate7")
            DayDate9.Text = dt.Rows(0)("DayDate9")

            ShippingMethodId.SelectedValue = dt.Rows(0)("ShippingMethodId")
            PortId.SelectedValue = dt.Rows(0)("PortId")
            ToPortId.SelectedValue = dt.Rows(0)("ToPortId")
            CountryId.SelectedValue = dt.Rows(0)("CountryId")
            ShippingLineId.SelectedValue = dt.Rows(0)("ShippingLineId")
            ForwarderId.SelectedValue = dt.Rows(0)("ForwarderId")
            TruckerId.SelectedValue = dt.Rows(0)("TruckerId")
            StationId.SelectedValue = dt.Rows(0)("StationId")
            VarietyId.SelectedValue = dt.Rows(0)("VarietyId")

        End If

    End Sub

    Private Sub BillNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles BillNo.LostFocus
        dt = bm.ExecuteAdapter("select top 1 * from ISO where BillNo='" & BillNo.Text.Trim & "'")
        If dt.Rows.Count > 0 Then
            FileNo.Text = dt.Rows(0)("FileNo")
            DayDate2.Text = dt.Rows(0)("DayDate2")
            DayDate3.Text = dt.Rows(0)("DayDate3")
            DayDate6.Text = dt.Rows(0)("DayDate6")
            DayDate7.Text = dt.Rows(0)("DayDate7")
            DayDate9.Text = dt.Rows(0)("DayDate9")

            ShippingMethodId.SelectedValue = dt.Rows(0)("ShippingMethodId")
            PortId.SelectedValue = dt.Rows(0)("PortId")
            ToPortId.SelectedValue = dt.Rows(0)("ToPortId")
            CountryId.SelectedValue = dt.Rows(0)("CountryId")
            ShippingLineId.SelectedValue = dt.Rows(0)("ShippingLineId")
            ForwarderId.SelectedValue = dt.Rows(0)("ForwarderId")
            TruckerId.SelectedValue = dt.Rows(0)("TruckerId")
            StationId.SelectedValue = dt.Rows(0)("StationId")
            VarietyId.SelectedValue = dt.Rows(0)("VarietyId")

        End If

    End Sub

End Class
