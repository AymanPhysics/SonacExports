Imports System.Data

Public Class RPT01
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Flag = 1 AndAlso ForwarderTypeId.SelectedIndex < 1 Then
            bm.ShowMSG("Please, Select a Type")
            ForwarderTypeId.Focus()
            Return
        End If

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@DestinationId", "@PortId", "Flag", "FlagName", "Header", "@Where"}
        rpt.paravalue = New String() {DestinationId.SelectedValue, PortId.SelectedValue, ForwarderTypeId.SelectedValue, ForwarderTypeId.Text, CType(Parent, Page).Title, ""}
        Select Case Flag
            Case 1
                rpt.Rpt = "DestinationPrices.rpt"
            Case 2
                For i As Integer = 1 To 4
                    Dim rpt1 As New ReportViewer
                    rpt1.Rpt = "LoadingSheet1.rpt"
                    rpt1.paraname = New String() {"Header", "Flag", "@Where"}
                    rpt1.paravalue = New String() {CType(Parent, Page).Title, i, ""}
                    rpt1.ShowDialog()
                Next
                Return
            Case 3
                rpt.Rpt = "LoadingSheet2.rpt"
        End Select

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.FillCombo("Ports2", DestinationId, "")
        bm.FillCombo("FreightTypes", ForwarderTypeId, "", , True)
        bm.FillCombo("Ports", PortId, "")

        If Flag = 2 OrElse Flag = 3 Then
            Grid1.Children.Clear()
        End If
    End Sub



End Class
