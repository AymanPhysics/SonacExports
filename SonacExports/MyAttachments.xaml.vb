Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.ComponentModel

Public Class MyAttachments
    Public Flag As MyFlag
    Dim bm As New BasicMethods
    Public Key1 As Integer = 0
    Public Key2 As Integer = 0 

    Enum MyFlag
        ProForma
        ISO
        Files
        Bills
        Outcomes
        LoadingSheet
    End Enum

    Dim WithEvents BackgroundWorker1 As New BackgroundWorker


    Private Sub btnAttach_Click(sender As Object, e As RoutedEventArgs) Handles btnAttach.Click
        Dim o As New Forms.OpenFileDialog
        o.Multiselect = True
        If o.ShowDialog = Forms.DialogResult.OK Then
            For i As Integer = 0 To o.FileNames.Length - 1
                bm.SaveFile("MyAttachments", "Flag", Flag, "Key1", Key1, "Key2", Key2, "AttachedName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "Image", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As RoutedEventArgs) Handles btnDownload.Click
        Try
            MyImagedata = Nothing
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize <> 18 Then Return
            Dim s As New Forms.SaveFileDialog With {.Filter = "All files (*.*)|*.*"}
            s.FileName = CType(TreeView1.SelectedItem, TreeViewItem).Header

            If IsNothing(sender) Then
                MyBath = bm.GetNewTempName(s.FileName)
            Else
                If Not s.ShowDialog = Forms.DialogResult.OK Then Return
                MyBath = s.FileName
            End If

            btnDownload.IsEnabled = False
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim myCommand As SqlClient.SqlCommand
            myCommand = New SqlClient.SqlCommand("select Image from MyAttachments where Flag =" & Flag & " and Key1='" & Key1 & "' and Key2='" & Key2 & "'", con)
            If con.State <> ConnectionState.Open Then con.Open()
            MyImagedata = CType(myCommand.ExecuteScalar(), Byte())
        Catch ex As Exception
        End Try
        con.Close()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            File.WriteAllBytes(MyBath, MyImagedata)
            Process.Start(MyBath)
        Catch ex As Exception
        End Try
        btnDownload.IsEnabled = True
    End Sub


    Dim MyImagedata() As Byte
    Dim MyBath As String = ""
    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        Try
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize = 18 Then
                If bm.ShowDeleteMSG("MsgDeleteFile") Then
                    bm.ExecuteNonQuery("delete from MyAttachments where Flag =" & Flag & " and Key1='" & Key1 & "' and Key2='" & Key2 & "' and AttachedName='" & TreeView1.SelectedItem.Header & "'" & bm.AppendWhere)
                    LoadTree()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub LoadTree()
        Dim dt As DataTable = bm.ExecuteAdapter("select AttachedName from MyAttachments where Flag =" & Flag & " and Key1='" & Key1 & "' and Key2='" & Key2 & "'")
        TreeView1.Items.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim nn As New TreeViewItem
            nn.Foreground = Brushes.DarkRed
            nn.FontSize = 18
            nn.Tag = dt.Rows(i)(0).ToString
            nn.Header = dt.Rows(i)(0).ToString
            TreeView1.Items.Add(nn)
        Next
    End Sub

    Private Sub TreeView1_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles TreeView1.MouseDoubleClick
        btnDownload_Click(Nothing, Nothing)
    End Sub

    Private Sub MyAttachments_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LoadTree()
    End Sub

End Class
