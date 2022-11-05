Imports System.Data

Public Class LoadingSheet
    Dim bm As New BasicMethods
    Dim MyTextBoxes() As TextBox = {}

    Dim dt As New DataTable
    Dim dv As New DataView
    Public Header As String = ""
    Public Flag As Integer = 0

    Public Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()

        btnRefresh_Click(Nothing, Nothing)
        If Flag <> 4 Then
            btnDelete.Visibility = Windows.Visibility.Hidden
        End If
    End Sub

    Sub FillGrid()
        Try
            Dim c As DataGridCellInfo = DataGridView1.CurrentCell
            dt = bm.ExecuteAdapter("select * from LoadingSheet order by cast(RecordNo as bigint)")
            dt.TableName = "tbl"
            DataGridView1.Foreground = System.Windows.Media.Brushes.Black
            dv.Table = dt
            If Not DataGridView1.ItemsSource Is Nothing Then
                DataGridView1.ItemsSource = Nothing
            End If
            DataGridView1.ItemsSource = dv
            DataGridView1.Focus()
            DataGridView1.BeginEdit()

            For i As Integer = 0 To dt.Columns.Count - 1
                SetColumnStyle(i)
            Next

            Dim t As New System.Windows.Threading.DispatcherTimer With {.IsEnabled = True, .Interval = New System.TimeSpan(0, 0, 1)}
            AddHandler t.Tick, AddressOf t_Tick
        Catch
        End Try
    End Sub

    Private Sub txt_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.Sort = DataGridView1.Columns(sender.Tag).Header
        Catch
        End Try
    End Sub

    Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.RowFilter = " 1=1"
            For i As Integer = 0 To dt.Columns.Count - 1
                dv.RowFilter &= " and [" & dt.Columns(i).ColumnName & "] like '%" & MyTextBoxes(i).Text & "%' "
            Next
        Catch
        End Try
    End Sub

    Private Sub DataGridView1_CellEditEnding(sender As Object, e As DataGridCellEditEndingEventArgs) Handles DataGridView1.CellEditEnding
        Try
            Dim str As String = CType(e.EditingElement, TextBox).Text.ToString.Replace("'", "''")
            If e.Column.Header.ToString.ToLower.Contains("date") Then
                str = bm.ToStrDate2(str)
            End If

            If DataGridView1.Items(e.Row.GetIndex)("RecordNo").ToString = "" Then
                bm.ExecuteNonQuery("insert LoadingSheet ([" & e.Column.Header.ToString.Replace("'", "''") & "]) select '" & str & "'")
            Else
                bm.ExecuteNonQuery("Update LoadingSheet set [" & e.Column.Header.ToString.Replace("'", "''") & "]='" & str & "' where RecordNo='" & DataGridView1.Items(e.Row.GetIndex)("RecordNo") & "'")
            End If
            btnRefresh_Click(Nothing, Nothing)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DataGridView1_ColumnHeaderDragCompleted(sender As Object, e As Primitives.DragCompletedEventArgs) Handles DataGridView1.ColumnHeaderDragCompleted
        'MessageBox.Show(6)
    End Sub

    Private Sub DataGridView1_KeyDown1(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyDown
        Try
            If e.Key = Key.F1 Then
                If DataGridView1.CurrentColumn.Header = "PI no" OrElse DataGridView1.CurrentColumn.Header = "Draft" OrElse DataGridView1.CurrentColumn.Header = "Original/Telax" OrElse DataGridView1.CurrentColumn.Header = "Final" Then
                    Dim frm As New MyWindow
                    Dim MyAttach As New MyAttachments With {.Flag = MyAttachments.MyFlag.LoadingSheet}
                    frm.Content = MyAttach
                    MyAttach.Key1 = DataGridView1.CurrentItem("RecordNo")
                    MyAttach.Key2 = DataGridView1.CurrentColumn.DisplayIndex
                    frm.ShowDialog()
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub LoadResource()
    End Sub

    Private Sub t_Tick(sender As Object, e As EventArgs)
        If Not sender Is Nothing Then CType(sender, System.Windows.Threading.DispatcherTimer).Stop()
        SC.Children.Clear()
        'Dim CurrentActualWidth As Integer = 0
        Try
            For i As Integer = 0 To dt.Columns.Count - 1
                Dim txt As New TextBox With {.Height = 30, .Margin = New Thickness(0, 0, 0, 10)}
                ReDim Preserve MyTextBoxes(MyTextBoxes.Length + 1)
                MyTextBoxes(i) = txt
                'Dim d = DataGridView1.Columns(i).ActualWidth
                'txt.Width = d
                'txt.SetResourceReference(TextBox.WidthProperty, Val(txt.Text))

                Dim binding As New Binding("ActualWidth")
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                binding.Source = DataGridView1.Columns(i)
                txt.SetBinding(TextBox.WidthProperty, binding)

                'CurrentActualWidth += DataGridView1.Columns(i).ActualWidth
                txt.Tag = i
                txt.TabIndex = i
                txt.HorizontalAlignment = Windows.HorizontalAlignment.Left
                txt.VerticalAlignment = Windows.VerticalAlignment.Top
                txt.Visibility = DataGridView1.Columns(i).Visibility
                SC.Children.Add(txt)
                AddHandler txt.GotFocus, AddressOf txt_Enter
                AddHandler txt.TextChanged, AddressOf txt_TextChanged
            Next
            'DataGridView1.SelectedIndex = 0
        Catch
        End Try
        'DataGridView1.IsReadOnly = True

    End Sub

    Sub SetColumnStyle(i As Integer)
        Try
            Dim s1 As New Style
            s1.Setters.Add(New Setter(BackgroundProperty, Brushes.Yellow))
            s1.Setters.Add(New Setter(ForegroundProperty, Brushes.Black))

            Dim s2 As New Style
            's2.Setters.Add(New Setter(BackgroundProperty, Brushes.Silver))
            s2.Setters.Add(New Setter(ForegroundProperty, Brushes.Black))

            Select Case i
                Case 0 To 3
                    DataGridView1.Columns(i).CellStyle = s1
                    If Flag = 3 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 4
                    DataGridView1.Columns(i).CellStyle = s2
                    If False Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 6 To 8
                    DataGridView1.Columns(i).CellStyle = s2
                    If False Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 9 To 10
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 2 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 11
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 2 OrElse Flag = 3 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 12 To 13
                    DataGridView1.Columns(i).CellStyle = s2
                    If False Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 15
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 2 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 16
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 2 OrElse Flag = 3 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 17
                    DataGridView1.Columns(i).CellStyle = s1
                    If Flag = 3 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 18
                    DataGridView1.Columns(i).CellStyle = s1
                    If Flag = 2 OrElse Flag = 3 OrElse Flag = 4 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 19
                    DataGridView1.Columns(i).CellStyle = s1
                    If Flag = 3 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 20
                    DataGridView1.Columns(i).CellStyle = s1
                Case 21 To 27
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 2 OrElse Flag = 4 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 30
                    DataGridView1.Columns(i).CellStyle = s1
                    If Flag = 4 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 31 To 35
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 2 OrElse Flag = 3 OrElse Flag = 4 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 36
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 2 OrElse Flag = 3 OrElse Flag = 4 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 37
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 3 OrElse Flag = 4 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 38
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 2 OrElse Flag = 4 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 39
                    DataGridView1.Columns(i).CellStyle = s2
                    If Flag = 2 OrElse Flag = 3 Then
                        DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
                    End If
                Case 40
                    DataGridView1.Columns(i).CellStyle = s1
                    DataGridView1.Columns(i).IsReadOnly = True
                    DataGridView1.Columns(i).Visibility = Windows.Visibility.Collapsed
            End Select
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnPrintAll_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintAll.Click
        For i As Integer = 1 To 4
            Dim rpt As New ReportViewer
            rpt.Rpt = "LoadingSheet1.rpt"
            rpt.paraname = New String() {"Header", "Flag", "@Where"}
            rpt.paravalue = New String() {CType(Parent, Page).Title, i, dv.RowFilter}
            rpt.ShowDialog()
        Next
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"Header", "@Where"}
        rpt.paravalue = New String() {CType(Parent, Page).Title, dv.RowFilter}
        rpt.Rpt = "LoadingSheet2.rpt"
        rpt.Show()
    End Sub


    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        Try
            If bm.ShowDeleteMSG("Are you sure you want to delete?") Then
                bm.ExecuteNonQuery("delete LoadingSheet where RecordNo='" & DataGridView1.SelectedItem("RecordNo") & "'")
                btnRefresh_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As RoutedEventArgs) Handles btnRefresh.Click
        Try
            Dim i As Integer = DataGridView1.SelectedIndex
            FillGrid()
            DataGridView1.Focus()
            DataGridView1.SelectedIndex = i
            If Not DataGridView1.SelectedItem Is Nothing Then
                DataGridView1.ScrollIntoView(DataGridView1.SelectedItem)
            End If
        Catch ex As Exception
        End Try
    End Sub
     
    Private Sub btnUp_Click(sender As Object, e As RoutedEventArgs) Handles btnUp.Click
        Try
            If DataGridView1.SelectedItem Is Nothing Then Return
            bm.ExecuteNonQuery("ChangeRecordNo", {"RecordNo1", "RecordNo2"}, {DataGridView1.Items(DataGridView1.SelectedIndex)("RecordNo"), DataGridView1.Items(DataGridView1.SelectedIndex - 1)("RecordNo")})
            btnRefresh_Click(Nothing, Nothing)
        Catch
        End Try
    End Sub

    Private Sub btnDown_Click(sender As Object, e As RoutedEventArgs) Handles btnDown.Click
        Try
            If DataGridView1.SelectedItem Is Nothing Then Return
            bm.ExecuteNonQuery("ChangeRecordNo", {"RecordNo1", "RecordNo2"}, {DataGridView1.Items(DataGridView1.SelectedIndex)("RecordNo"), DataGridView1.Items(DataGridView1.SelectedIndex + 1)("RecordNo")})
            btnRefresh_Click(Nothing, Nothing)
        Catch
        End Try
    End Sub
End Class