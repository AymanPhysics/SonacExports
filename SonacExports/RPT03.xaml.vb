Imports System.Data

Public Class RPT03
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "Header", "@CustomerId", "@ToPortId", "@FromPortId", "@ItemId", "@MarkId"}
        rpt.paravalue = New String() {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), CType(Parent, Page).Title, Val(CustomerId.Text), Val(DestinationId.SelectedValue), Val(PortId.SelectedValue), Val(ItemId.SelectedValue), Val(MarkId.SelectedValue)}
        Select Case Flag
            Case 1
                rpt.Rpt = "ProForma.rpt"
        End Select

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return

        bm.Addcontrol_MouseDoubleClick({CustomerId})

        bm.FillCombo("Ports2", DestinationId, "")
        bm.FillCombo("Ports", PortId, "")
        bm.FillCombo("Items", ItemId, "")
        bm.FillCombo("Marks", MarkId, "")


        FromDate.SelectedDate = Now.Date.AddYears(-1)
        ToDate.SelectedDate = Now.Date

    End Sub

    Private Sub CustomerId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CustomerId.KeyUp
        If bm.ShowHelp("Customers", CustomerId, CustomerName, e, "select cast(Id as varchar(100)) Id,Name from Customers") Then
            CustomerId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub CustomerId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustomerId.LostFocus
        If Val(CustomerId.Text.Trim) = 0 Then
            CustomerId.Clear()
            CustomerName.Clear()
            Return
        End If
        bm.LostFocus(CustomerId, CustomerName, "select Name from Customers where Id=" & CustomerId.Text.Trim())
    End Sub


End Class
