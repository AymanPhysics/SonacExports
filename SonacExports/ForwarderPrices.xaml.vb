Imports System.Data

Public Class ForwarderPrices
    Public TableName As String = "ForwarderPrices"
    Public MainId As String = "ForwarderId"
    Public SubId As String = "ShippingLineId"
    Public SubId2 As String = "PortId"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    WithEvents G As New MyGrid

    Private Sub BasicForm3_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        bm.FillCombo("Forwarders", ForwarderId, "")
        bm.FillCombo("ShippingLines", ShippingLineId, "")
        bm.FillCombo("Ports", PortId, "")

        bm.Table_Name = TableName

        LoadWFH()

        btnNew_Click(sender, e)
    End Sub


    Structure GC
        Shared DestinationId As String = "DestinationId"
        Shared DestinationName As String = "DestinationName"
        Shared PriceDry As String = "PriceDry"
        Shared Price20 As String = "Price20"
        Shared Price40 As String = "Price40"
        Shared TransitTime As String = "TransitTime"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.DestinationId, "Destination Id")
        G.Columns.Add(GC.DestinationName, "Destination Name")
        G.Columns.Add(GC.PriceDry, "Price Dry")
        G.Columns.Add(GC.Price20, "Price 20")
        G.Columns.Add(GC.Price40, "Price 40")
        G.Columns.Add(GC.TransitTime, "Transit Time")
        
        G.Columns(GC.DestinationName).FillWeight = 300 '110
        G.Columns(GC.DestinationId).Visible = False
        G.Columns(GC.DestinationName).ReadOnly = True

        G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill

        G.AllowUserToAddRows = False


    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If ForwarderId.SelectedValue.ToString = 0 OrElse ShippingLineId.SelectedValue.ToString = 0 OrElse PortId.SelectedValue.ToString = 0 Then
            Return
        End If

        G.EndEdit()

        If Not bm.SaveGrid(G, TableName, New String() {"ForwarderId", "ShippingLineId", "PortId"}, New String() {ForwarderId.SelectedValue, ShippingLineId.SelectedValue, PortId.SelectedValue}, New String() {"DestinationId", "PriceDry", "Price20", "Price40", "TransitTime"}, New String() {GC.DestinationId, GC.PriceDry, GC.Price20, GC.Price40, GC.TransitTime}, New VariantType() {VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal}, New String() {}) Then Return
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        Try
            ShippingLineId.SelectedIndex = 0
            PortId.SelectedIndex = 0
            G.Rows.Clear()
            ShippingLineId.Focus()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & MainId & "='" & ForwarderId.SelectedValue & "' and " & SubId & " ='" & ShippingLineId.SelectedValue.ToString & "' and " & SubId2 & " ='" & PortId.SelectedValue.ToString & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

    End Sub


    Private Sub FillControls()
        If ForwarderId.SelectedIndex < 1 OrElse ShippingLineId.SelectedIndex < 1 OrElse PortId.SelectedIndex < 1 Then
            G.Rows.Clear()
            Return
        End If
        Dim dt As DataTable = bm.ExecuteAdapter("select T.Id DestinationId,T.Name DestinationName,F.PriceDry,F.Price20,F.Price40,F.TransitTime from Ports2 T left join ForwarderPrices F on(F.ForwarderId='" & ForwarderId.SelectedValue & "' and F.ShippingLineId='" & ShippingLineId.SelectedValue & "' and F.PortId='" & PortId.SelectedValue & "' and T.Id=F.DestinationId)")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.DestinationId).Value = dt.Rows(i)("DestinationId").ToString
            G.Rows(i).Cells(GC.DestinationName).Value = dt.Rows(i)("DestinationName").ToString
            G.Rows(i).Cells(GC.PriceDry).Value = dt.Rows(i)("PriceDry").ToString
            G.Rows(i).Cells(GC.Price20).Value = dt.Rows(i)("Price20").ToString
            G.Rows(i).Cells(GC.Price40).Value = dt.Rows(i)("Price40").ToString
            G.Rows(i).Cells(GC.TransitTime).Value = dt.Rows(i)("TransitTime").ToString
        Next
    End Sub

    Private Sub ForwarderId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ForwarderId.SelectionChanged
        FillControls()
    End Sub

    Private Sub ShippingLineId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ShippingLineId.SelectionChanged
        FillControls()
    End Sub

    Private Sub PortId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles PortId.SelectionChanged
        FillControls()
    End Sub

End Class
