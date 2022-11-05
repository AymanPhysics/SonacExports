' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Windows.Threading
Imports System.Data
Imports System.Xml
Imports System.IO.Ports
Imports System.Threading

Partial Public Class MainPage
    Inherits Page
    Public NLevel As Boolean = False
    Dim m As MainWindow = Application.Current.MainWindow
    Dim bm As New BasicMethods
    WithEvents t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 1)}

    Private sampleGridOpacityAnimation As DoubleAnimation
    Private sampleGridTranslateTransformAnimation As DoubleAnimation
    Private borderTranslateDoubleAnimation As DoubleAnimation

    Public Sub New()
        InitializeComponent()

        Dim widthBinding As New Binding("ActualWidth")
        widthBinding.Source = Me

        sampleGridOpacityAnimation = New DoubleAnimation()
        sampleGridOpacityAnimation.To = 0
        sampleGridOpacityAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        sampleGridTranslateTransformAnimation = New DoubleAnimation()
        sampleGridTranslateTransformAnimation.BeginTime = TimeSpan.FromSeconds(0.15)
        sampleGridTranslateTransformAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        borderTranslateDoubleAnimation = New DoubleAnimation()
        borderTranslateDoubleAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.3))
        borderTranslateDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0)

    End Sub
    Private Shared _packUri As New Uri("pack://application:,,,/")

    Private Sub btnBack_Click(sender As Object, e As RoutedEventArgs) Handles btnBack.Click
        borderTranslateDoubleAnimation.From = 0
        borderTranslateDoubleAnimation.To = -ActualWidth
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
        GridSampleViewer_Loaded(Nothing, Nothing)
        Md.Currentpage = ""
    End Sub

    Private Sub selectedSampleChanged(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If TypeOf args.Source Is RadioButton Then
            Dim theButton As RadioButton = CType(args.Source, RadioButton)

            Dim theFrame
            If TypeOf theButton.Tag Is MyPage Then
                theFrame = CType(theButton.Tag, MyPage)
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    theFrame.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                If Not Md.MyProjectType = ProjectType.PCs Then
                    CType(theButton.Tag, MyPage).MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowEdit") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowDelete") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowNavigate") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowPrint") = 1
                End If
            ElseIf TypeOf theButton.Tag Is Window Then
                theFrame = CType(theButton.Tag, MyWindow)
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    theFrame.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                If Not Md.MyProjectType = ProjectType.PCs Then
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowEdit") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowDelete") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowNavigate") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowPrint") = 1
                End If
            End If

            theButton.IsTabStop = False
            CType(args.Source, RadioButton).IsChecked = False

            If TypeOf theButton.Tag Is Window Then
                CType(theFrame, Window).Show()
                CType(theFrame, Window).WindowState = WindowState.Minimized
                CType(theFrame, Window).WindowState = WindowState.Maximized
            ElseIf m.layoutSwitcher.SelectedIndex = 1 Then
                Dim frm As New MyWindow
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    frm.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    frm.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                frm.Content = theButton.Tag
                frm.WindowState = WindowState.Maximized
                frm.Show()
                frm.WindowState = WindowState.Minimized
                frm.WindowState = WindowState.Maximized
            Else
                SampleDisplayFrame.Content = theButton.Tag
                SampleDisplayBorder.Visibility = Visibility.Visible
                Try
                    theFrame.Tag = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Catch ex As Exception
                End Try
                sampleDisplayFrameLoaded(theFrame, args)
            End If
        End If

    End Sub

    Private Sub sampleDisplayFrameLoaded(ByVal sender As Object, ByVal args As EventArgs)
        If TypeOf sender Is MyWindow Then
            Try
                If Resources.Item(CType(sender, MyWindow).Tag) = "" Then
                    CType(sender, MyWindow).Title = CType(sender, MyWindow).Tag
                Else
                    CType(sender, MyWindow).Title = Resources.Item(CType(sender, MyWindow).Tag)
                End If
                Md.Currentpage = CType(sender, MyWindow).Title
            Catch ex As Exception
            End Try
        ElseIf TypeOf sender Is Page Then
            Try
                If Resources.Item(CType(sender, Page).Tag) = "" Then
                    CType(sender, Page).Title = CType(sender, Page).Tag
                Else
                    CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                End If
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        ElseIf Not sender Is Nothing AndAlso TypeOf CType(sender, Frame).Content Is Page Then
            Try
                If Resources.Item(CType(CType(sender, Frame).Content, Page).Tag) = "" Then
                    CType(CType(sender, Frame).Content, Page).Title = CType(CType(sender, Frame).Content, Page).Tag
                Else
                    CType(CType(sender, Frame).Content, Page).Title = Resources.Item(CType(CType(sender, Frame).Content, Page).Tag)
                End If
                Md.Currentpage = CType(CType(sender, Frame).Content, Page).Title
            Catch ex As Exception
            End Try
            Try
                If Resources.Item(CType(sender, Page).Tag) = "" Then
                    CType(sender, Page).Title = CType(sender, Page).Tag
                Else
                    CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                End If
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        End If

        sampleGridTranslateTransformAnimation.To = -ActualWidth
        borderTranslateDoubleAnimation.From = -ActualWidth
        borderTranslateDoubleAnimation.To = 0

        SampleDisplayBorder.Visibility = Visibility.Visible
        SampleGrid.BeginAnimation(Grid.OpacityProperty, sampleGridOpacityAnimation)
        SampleGridTranslateTransform.BeginAnimation(TranslateTransform.XProperty, sampleGridTranslateTransformAnimation)
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
    End Sub

    Private Sub galleryLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If bm.TestIsLoaded(Me, True) Then Return
        tab.Margin = New Thickness(0)
        tab.HorizontalAlignment = HorizontalAlignment.Stretch
        tab.VerticalAlignment = VerticalAlignment.Stretch
        'tab.Style = FindResource("TabControlLeftStyle")
        'tab.Style = FindResource("OutlookTabControlStyle")

        Load()

        SampleDisplayBorderTranslateTransform.X = -ActualWidth
        SampleDisplayBorder.Visibility = Visibility.Hidden
    End Sub

    Private Sub pageSizeChanged(ByVal sender As Object, ByVal args As SizeChangedEventArgs)
        SampleDisplayBorderTranslateTransform.X = Me.ActualWidth
    End Sub

    Dim DynamicMenuitem As Integer = 0
    Dim DtCurrentMenuitem As New DataTable With {.TableName = "T"}
    Sub TestCurrentMenuitem(CurrentMenuitem As Integer)
        If DtCurrentMenuitem.Columns.Count = 0 Then DtCurrentMenuitem.Columns.Add("C")
        If DtCurrentMenuitem.Select("C=" & CurrentMenuitem).Length > 0 Then MessageBox.Show(CurrentMenuitem)
        DtCurrentMenuitem.Rows.Add(CurrentMenuitem)
    End Sub
    Sub LoadLabel(CurrentMenuitem As Integer, ByVal G As WrapPanel, Ttl As String)
        TestCurrentMenuitem(CurrentMenuitem)

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                While rd.Item(Ttl).Length < 16
                    rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                End While
            Catch ex As Exception
            End Try
        Next

        Dim lbl0 As New Label With {.Height = ActualHeight, .Margin = New Windows.Thickness(24, 0, 0, 0)}
        G.Children.Add(lbl0)

        Dim lbl As New Label With {.Name = "menuitem" & CurrentMenuitem, .FontFamily = New System.Windows.Media.FontFamily("khalaad al-arabeh 2"), .FontSize = 30, .HorizontalContentAlignment = Windows.HorizontalAlignment.Center, .Foreground = New SolidColorBrush(Color.FromArgb(255, 9, 103, 168)), .FontWeight = FontWeight.FromOpenTypeWeight(1), .Height = 90}
        lbl.SetResourceReference(Label.ContentProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            lbl.Content = Ttl
        End If

        G.Children.Add(lbl)
        
        If Ttl = "" Then lbl.Height = 0


        If Not Lvl Then
            Dim it As New MenuItem With {.Header = "-----------------", .Name = "NewMenuItemSub" & CurrentMenuitem}
            it.Visibility = Windows.Visibility.Collapsed
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it)

            Dim it2 As New MenuItem With {.Header = "-----------------", .Name = "NewMenuItemSub" & CurrentMenuitem}
            it2.IsEnabled = False
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it2)
        End If
    End Sub

    Function LoadRadio(CurrentMenuitem As Integer, ByVal G As WrapPanel, ByVal Ttl As String) As RadioButton
        TestCurrentMenuitem(CurrentMenuitem)

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                While rd.Item(Ttl).Length < 16
                    rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                End While
            Catch ex As Exception
            End Try
        Next

        Dim RName As String = "menuitem" & CurrentMenuitem
        Dim r As New RadioButton With {.Name = RName, .Style = Application.Current.FindResource("GlassRadioButtonStyle"), .Width = 180, .Height = 90}
        'r.Tag = New Page With {.Content = frm}
        

        Dim t As New TranslateTextAnimationExample
        t.RealText.Tag = Ttl
        t.RealText.SetResourceReference(TextBlock.TextProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            t.RealText.Text = Ttl
        End If

        r.SetResourceReference(RadioButton.BackgroundProperty, "SC")
        t.SetResourceReference(RadioButton.BackgroundProperty, "SC")

        r.Content = t
        G.Children.Add(r)

        r.SetResourceReference(RadioButton.ToolTipProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            r.ToolTip = Ttl
        End If


        If Not Lvl Then
            Dim it As New MenuItem With {.Header = Ttl, .Name = "NewMenuItemSub" & CurrentMenuitem}
            it.Tag = r
            it.SetResourceReference(MenuItem.HeaderProperty, Ttl)
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it)
            AddHandler it.Click, AddressOf it_Click
        End If
        Return r
    End Function

    Private Sub it_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim x As RadioButton = CType(sender.Tag, RadioButton)
            x.RaiseEvent(New RoutedEventArgs(RadioButton.CheckedEvent))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GridSampleViewer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        bm.TestIsLoaded(Me)
    End Sub

    Private Sub ResizeHeader(G As WrapPanel)
        If Lvl Then Return
        Dim Ttl As String = CType(CType(G.Parent, ScrollViewer).Parent, TabItem).Header
        Try
            While Md.DictionaryCurrent.Item(Ttl).Length < 16
                Md.DictionaryCurrent.Item(Ttl) = " " & Md.DictionaryCurrent.Item(Ttl) & " "
            End While
        Catch
        End Try
    End Sub


    Public Lvl As Boolean = False
    Public Sub Load()

        If MyProjectType = ProjectType.PCs Then
            LoadGPCs(0)
            Return
        End If

        LoadTabs()

        If Not Lvl Then
            dtLevelsMenuitems = bm.ExecuteAdapter("select * from LevelsMenuitems where LevelId=" & Md.LevelId)
            Dim dtLevelsTabs As DataTable = bm.ExecuteAdapter("select * from LevelsTabs where LevelId=" & Md.LevelId)
            If dtLevelsMenuitems.Rows.Count = 0 Then Application.Current.Shutdown()

            For i As Integer = 0 To tab.Items.Count - 1
                Dim item As TabItem = CType(tab.Items(i), TabItem)

                If dtLevelsTabs.Select("Id=" & tab.Items(i).Name.ToString.Replace("tab", "")).Length = 0 Then
                    item.Visibility = Windows.Visibility.Collapsed
                    CType(m.MyMenu.Items(i), MenuItem).Visibility = Windows.Visibility.Collapsed
                End If
                item.Content.Visibility = item.Visibility

                For x As Integer = 0 To CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children.Count - 1
                    If CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(RadioButton) Then
                        Dim t As RadioButton = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), RadioButton)
                        If dtLevelsMenuitems.Select("Id=" & t.Name.ToString.Replace("menuitem", "")).Length = 0 Then
                            t.Visibility = Windows.Visibility.Collapsed
                            CType(CType(m.MyMenu.Items(i), MenuItem).Items(x), MenuItem).Visibility = Windows.Visibility.Collapsed
                        End If
                    ElseIf CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(Label) Then
                        Dim t As Label = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), Label)
                        If t.Name = "" Then
                            t.Visibility = Windows.Visibility.Visible
                        ElseIf dtLevelsMenuitems.Select("Id=" & t.Name.ToString.Replace("menuitem", "")).Length = 0 Then
                            t.Visibility = Windows.Visibility.Collapsed
                            CType(CType(m.MyMenu.Items(i), MenuItem).Items(x), MenuItem).Visibility = Windows.Visibility.Collapsed
                        End If
                    End If
                Next
            Next

            For i As Integer = 0 To tab.Items.Count - 1
                If CType(tab.Items(i), TabItem).Visibility = Windows.Visibility.Visible Then
                    CType(tab.Items(i), TabItem).IsSelected = True
                    Exit For
                End If
            Next

        End If

    End Sub

    Function MakePanel(CurrentTab As Integer, MyHeader As String, ImagePath As String) As WrapPanel
        Dim SV As New MyScrollViewer
        bm.SetImage(SV.Img, ImagePath)
        Dim t As New TabItem With {.Content = SV, .Name = "tab" & CurrentTab, .Header = MyHeader, .Tag = MyHeader}

        'Template.ControlTemplate().Grid().Border().TextBlock()
        'FontFamily="khalaad al-arabeh 2" FontSize="12

        t.Style = FindResource("MyTabItem")
        't.Style = FindResource("OutlookTabItemStyle")
        't.Background = FindResource("OutlookButtonBackground")
        't.Foreground = FindResource("OutlookButtonForeground")

        tab.Items.Add(t)
        Dim G As WrapPanel = SV.MyWrapPanel
        G.Name = "MyWrapPanel" & CurrentTab
        G.AddHandler(System.Windows.Controls.Primitives.ToggleButton.CheckedEvent, New System.Windows.RoutedEventHandler(AddressOf Me.selectedSampleChanged))

        ResizeHeader(G)
        t.SetResourceReference(TabItem.HeaderProperty, t.Header)

        If Not Lvl Then
            Dim it As New MenuItem With {.Header = MyHeader, .MaxWidth = 150, .Name = "NewMenuItem" & CurrentTab}
            it.Tag = t
            it.SetResourceReference(MenuItem.HeaderProperty, MyHeader)
            m.MyMenu.Items.Add(it)
            AddHandler it.MouseEnter, AddressOf itm_Click
        End If

        Return G
    End Function

    Private Sub itm_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim x As TabItem = CType(sender.Tag, TabItem)
            x.Focus()
            x.IsSelected = True
            x.BringIntoView()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadGPCs(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "File", "Omega.jpg")

        AddHandler LoadRadio(0, G, "PCs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       Dim frm As New BasicForm With {.TableName = "PCs"}
                                                       bm.SetImage(CType(frm, BasicForm).Img, "password.jpg")
                                                       frm.txtName.MaxLength = 1000
                                                       m.TabControl1.Items.Clear()
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

    End Sub

    Private Sub LoadGFile(CurrentTab As Integer)
        Dim s As String = "buttonscreen.jpg"
        s = "MainOMEGA.jpg"


        Dim G As WrapPanel = MakePanel(CurrentTab, "File", s)
        Dim frm As UserControl

        AddHandler LoadRadio(101, G, "Employees").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               sender.Tag = New MyPage With {.Content = New Employees}
                                                           End Sub


        AddHandler LoadRadio(102, G, "Countries").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm With {.TableName = "Countries"}

                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(103, G, "Cities").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New BasicForm2 With {.MainTableName = "Countries", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Country", .TableName = "Cities", .MainId = "CountryId", .SubId = "Id", .SubName = "Name"}

                                                            bm.SetImage(CType(frm, BasicForm2).Img, "MainOMEGA.jpg")
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub


        AddHandler LoadRadio(104, G, "Areas").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New BasicForm3 With {.MainTableName = "Countries", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Country", .Main2TableName = "Cities", .Main2MainId = "CountryId", .Main2SubId = "Id", .Main2SubName = "Name", .lblMain2_Content = "City", .TableName = "Areas", .MainId = "CountryId", .MainId2 = "CityId", .SubId = "Id", .SubName = "Name"}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

         

        'AddHandler LoadRadio(107, G, "Statics").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                     frm = New Statics
        '                                                     sender.Tag = New MyPage With {.Content = frm}
        '                                                 End Sub


        AddHandler LoadRadio(108, G, "Marks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New BasicForm With {.TableName = "Marks"}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        'شروط الدفع
        AddHandler LoadRadio(109, G, "Payment Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BasicForm With {.TableName = "PaymentTypes"}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(110, G, "Ports of Loading").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm With {.TableName = "Ports"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(124, G, "Discharge Ports").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BasicForm With {.TableName = "Ports2"}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        AddHandler LoadRadio(111, G, "Shipping Lines").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm1_2 With {.TableName = "ShippingLines", .lblName2_text = "Transit Time", .SubName2 = "TransitTime"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(112, G, "Shipping Methods").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm With {.TableName = "ShippingMethods"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(113, G, "Couriers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New BasicForm With {.TableName = "Couriers"}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(114, G, "Currencies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Currencies"}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        'AddHandler LoadRadio(115, G, "Freight Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                           frm = New BasicForm With {.TableName = "FreightTypes"}
        '                                                           sender.Tag = New MyPage With {.Content = frm}
        '                                                       End Sub

        AddHandler LoadRadio(123, G, "Invoices Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm With {.TableName = "OutcomeTypes"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(126, G, "Forwarders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Forwarders"}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(127, G, "Truckers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New BasicForm With {.TableName = "Truckers"}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(128, G, "Loading Locations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New BasicForm With {.TableName = "LoadingLocations"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(129, G, "Varieties").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm With {.TableName = "Varieties"}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(130, G, "Forwarder Prices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New ForwarderPrices
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub


        AddHandler LoadRadio(131, G, "Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New BasicForm With {.TableName = "Items"}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        AddHandler LoadRadio(132, G, "Units Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm1_2 With {.TableName = "UnitsWeights", .lblName2_text = "Weight", .SubName2 = "Weights"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        AddHandler LoadRadio(133, G, "Banks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New BasicForm With {.TableName = "Banks", .IsMultiLine = True}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub


        AddHandler LoadRadio(134, G, "Stations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New BasicForm With {.TableName = "Stations"}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(135, G, "Shipped Per").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "ShippedPers"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
    End Sub

    Private Sub LoadGAccountants(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Accounts", s)
        Dim frm As UserControl



        LoadLabel(201, G, "File")

        AddHandler LoadRadio(202, G, "Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Customers
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        LoadLabel(203, G, "Motion")

        AddHandler LoadRadio(204, G, "Invoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New ProForma
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(205, G, "Loadings").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New ISO
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(206, G, "Files").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Files
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        AddHandler LoadRadio(207, G, "Bills").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Bills
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        AddHandler LoadRadio(208, G, "Payments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New Outcomes
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(209, G, "Loadings Sheet 1").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New LoadingSheet With {.Flag = 1}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(210, G, "Loadings Sheet 2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New LoadingSheet With {.Flag = 2}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(211, G, "Loadings Sheet 3").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New LoadingSheet With {.Flag = 3}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(212, G, "Loadings Sheet 4").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New LoadingSheet With {.Flag = 4}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

    End Sub

    Private Sub LoadGSecurity(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Options", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1101, G, "Change Password").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New ChangePassword
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1102, G, "Levels").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New Levels
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub
         

    End Sub

    Private Sub LoadGAccountsReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Accounts Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1201, G, "Destination Prices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT01 With {.Flag = 1}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        'AddHandler LoadRadio(1202, G, "Loading Sheet").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                            frm = New RPT02 With {.Flag = 1}
        '                                                            sender.Tag = New MyPage With {.Content = frm}
        '                                                        End Sub

        AddHandler LoadRadio(1203, G, "UnPayed Files").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT02 With {.Flag = 2}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub


        AddHandler LoadRadio(1204, G, "Loading Sheet All").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT01 With {.Flag = 2}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(1205, G, "Loading Sheet").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT01 With {.Flag = 3}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(1206, G, "Invoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT03 With {.Flag = 1}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

    End Sub

    Private Sub LoadAbout(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "About", s)
        Dim wb As New WebBrowser With {.Margin = New Thickness(0)}
        wb.Navigate("http://omegaapp.blogspot.com.eg/")
        G.Children.Add(wb)
        wb.Width = tab.ActualWidth - 20
        wb.Height = tab.ActualHeight - 60
    End Sub



    Private Sub LoadTabs() 
        LoadGFile(1)

        LoadGAccountants(2)
        
        LoadGSecurity(11)

        LoadGAccountsReports(12)
        


    End Sub

  

End Class

