Imports System.Data

Public Class Outcomes
    Public TableName As String = "Outcomes"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return

        MyAttach.Flag = MyAttachments.MyFlag.Outcomes

        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()
        bm.Fields = New String() {SubId, SubId2, "DayDate", "Canceled", "BillNo", "FileNo", "PrimaryFreight"}
        bm.control = New Control() {txtFlag, txtID, DayDate, Canceled, BillNo, FileNo, PrimaryFreight}
        bm.KeyFields = New String() {SubId, SubId2}
        bm.Table_Name = TableName

        LoadWFH()


        btnNew_Click(sender, e)
    End Sub



    Structure GC
        Shared Value As String = "Value"
        Shared OutcomeId As String = "OutcomeId"
        Shared CurrencyId As String = "CurrencyId"
        Shared Line As String = "Line"
        Shared SD_Notes As String = "SD_Notes"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Value, "Value")

        Dim GCOutcomeId As New Forms.DataGridViewComboBoxColumn
        GCOutcomeId.HeaderText = "Invoice Type"
        GCOutcomeId.Name = GC.OutcomeId
        bm.FillCombo("select 0 Id,'' Name union select Id,cast(Id as nvarchar(100))+' - '+Name From OutcomeTypes", GCOutcomeId)
        G.Columns.Add(GCOutcomeId)

        Dim GCCurrencyId As New Forms.DataGridViewComboBoxColumn
        GCCurrencyId.HeaderText = "Currency"
        GCCurrencyId.Name = GC.CurrencyId
        bm.FillCombo("select 0 Id,'' Name union select Id,cast(Id as nvarchar(100))+' - '+Name From Currencies", GCCurrencyId)
        G.Columns.Add(GCCurrencyId)

        G.Columns.Add(GC.Line, "Line")


        G.Columns.Add(GC.SD_Notes, "Notes")


        G.Columns(GC.OutcomeId).FillWeight = 300 '110
        G.Columns(GC.CurrencyId).FillWeight = 200

        G.Columns(GC.SD_Notes).FillWeight = 400

        G.Columns(GC.Line).Visible = False

        G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill


        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True


        bm.FillControls(Me) 

        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableName & " where " & SubId & "=" & txtFlag.Text.Trim & " and " & SubId2 & "=" & txtID.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.CurrencyId).Value = dt.Rows(i)("CurrencyId").ToString
            G.Rows(i).Cells(GC.OutcomeId).Value = dt.Rows(i)("OutcomeId").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            G.Rows(i).Cells(GC.SD_Notes).Value = dt.Rows(i)("SD_Notes").ToString
        Next


        MyAttach.Key1 = Val(txtID.Text)
        MyAttach.Key2 = 0
        MyAttach.LoadTree()

        DayDate.Focus()
        G.RefreshEdit()
        lop = False
        CalcTotal()

        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
        If Not bm.TestDateValidity(DayDate) Then Return

        G.EndEdit()

        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.Value).Value) = 0 Then
                Continue For
            End If

        Next

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If


        bm.DefineValues()

        If Not bm.SaveGrid(G, TableName, New String() {SubId, SubId2}, New String() {txtFlag.Text.Trim, txtID.Text}, New String() {"OutcomeId", "CurrencyId", "Value", "SD_Notes"}, New String() {GC.OutcomeId, GC.CurrencyId, GC.Value, GC.SD_Notes}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.String}, New String() {}) Then Return

        If Not bm.Save(New String() {SubId, SubId2}, New String() {txtFlag.Text.Trim, txtID.Text}) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True
    End Sub

    Dim lop As Boolean = False

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.CurrencyId).Value = Nothing
        G.Rows(i).Cells(GC.OutcomeId).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
        G.Rows(i).Cells(GC.SD_Notes).Value = Nothing
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.Value Then
                G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value)
                'G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo)

            End If

            CalcTotal()
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
    End Sub



    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId, SubId2}, "Min", dt)
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
        G.Rows.Clear()
        CalcTotal()

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtFlag.Text = Flag
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & SubId & "=" & txtFlag.Text)
        If txtID.Text = "" Then txtID.Text = "1"

        MyAttach.Key1 = Val(txtID.Text)
        MyAttach.Key2 = 0
        MyAttach.LoadTree()

        'DayDate.Focus()
        txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {txtFlag.Text, txtID.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {SubId, SubId2}, New String() {txtFlag.Text.Trim, txtID.Text}, dt)
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

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")

        lblID.SetResourceReference(Label.ContentProperty, "Id")


        lblDayDate.SetResourceReference(Label.ContentProperty, "DayDate")
        lblNotes.SetResourceReference(Label.ContentProperty, "Notes")
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
                CalcTotal()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim LopCalc As Boolean = False
    Private Sub CalcTotal()
        If LopCalc Or lop Then Return
        Try
            LopCalc = True
            'Value.Text = Math.Round(0, 2)
            'For i As Integer = 0 To G.Rows.Count - 1
            '    Value.Text += Val(G.Rows(i).Cells(GC.Value).Value)
            'Next

            LopCalc = False
        Catch ex As Exception
        End Try
    End Sub


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        'e.Handled = True
        If G.CurrentCell Is Nothing Then Return
        If G.CurrentCell.ReadOnly Then Return
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If

        Catch ex As Exception
        End Try
        G.CommitEdit(Forms.DataGridViewDataErrorContexts.Commit)
    End Sub



    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False


    Private Sub FileNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles FileNo.LostFocus
        BillNo.Text = bm.ExecuteScalar("select dbo.getBillNo('" & FileNo.Text.Trim & "')")
        PrimaryFreight.Text = bm.ExecuteScalar("select dbo.getPrimaryFreight('" & FileNo.Text.Trim & "')")
    End Sub

    Private Sub BillNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles BillNo.LostFocus
        FileNo.Text = bm.ExecuteScalar("select dbo.getFileNo('" & BillNo.Text.Trim & "')")
        PrimaryFreight.Text = bm.ExecuteScalar("select dbo.getPrimaryFreight('" & FileNo.Text.Trim & "')")
    End Sub

End Class
