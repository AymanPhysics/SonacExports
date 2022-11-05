Imports System.Data

Public Class RPT02
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "ISO.rpt"
            Case 2
                rpt.Rpt = "OutcomeTypesFiles.rpt"
        End Select

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return

        FromDate.SelectedDate = Now.Date
        ToDate.SelectedDate = Now.Date

        If Flag = 2 Then
            lblFromDate.Visibility = Windows.Visibility.Hidden
            FromDate.Visibility = Windows.Visibility.Hidden
            lblToDate.Visibility = Windows.Visibility.Hidden
            ToDate.Visibility = Windows.Visibility.Hidden
        End If
    End Sub



End Class
