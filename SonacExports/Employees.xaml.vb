Imports System.Data
Public Class Employees
    Public TableName As String = "Employees"
    Public TableNameDetailed As String = "VisitingTypeEmployees"
    Public SubId As String = "Id"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    WithEvents G As New MyGrid


    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})

        bm.FillCombo("LinkFile", MainLinkFile, "", , True)
        LoadResource()

        lblAcc.Visibility = Windows.Visibility.Hidden
        AccNo.Visibility = Windows.Visibility.Hidden
        AccName.Visibility = Windows.Visibility.Hidden

        lblTax.Visibility = Windows.Visibility.Hidden
        lblTax2.Visibility = Windows.Visibility.Hidden
        Tax.Visibility = Windows.Visibility.Hidden

        lblTaxAcc.Visibility = Windows.Visibility.Hidden
        TaxAccNo.Visibility = Windows.Visibility.Hidden
        TaxAccName.Visibility = Windows.Visibility.Hidden

        Doctor.Visibility = Windows.Visibility.Hidden
        Nurse.Visibility = Windows.Visibility.Hidden
        Receptionist.Visibility = Windows.Visibility.Hidden
        Waiter.Visibility = Windows.Visibility.Visible


        bm.Fields = New String() {SubId, "Name", "Address", "DateOfBirth", "DepartmentId", "Notes", "Nurse", "Receptionist", "Manager", "SystemUser", "NationalId", "HomePhone", "Mobile", "Email", "Password", "EnName", "LevelId", "Doctor", "Stopped", "HiringDate", "Duration", "Cnt", "hh", "hh2", "hh3", "hh4", "hh5", "hh6", "hh7", "mm", "mm2", "mm3", "mm4", "mm5", "mm6", "mm7", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "IsSalary", "IsFreelancer", "LateAllowance", "Allowance", "SpAllowance", "FromHH", "ToHH", "FromMM", "ToMM", "Salary", "SalaryOnly", "ShiftsOnly", "SalaryOrShifts", "ShiftCount", "ShiftValue", "Tax", "Bonus", "Insurance", "Annual", "NoofDaysOff", "NoofMonthlyExecuses", "HolidayWorkValue", "DelayValue", "OvertimeValue", "IsFixedHours", "TotalHours", "TotalMinutes", "HasAttendance", "MainJobId", "SubJobId", "TaxAccNo", "AccNo", "DefaultStore", "DefaultSave", "Waiter", "DefaultBank", "FromHHTemp", "FromMMTemp", "ToHHTemp", "ToMMTemp", "TotalHoursTemp", "TotalMinutesTemp", "FromDate", "ToDate", "EditPrices", "DegreeId", "GraduationYear", "NoLicenseToPractice", "SalesPrice", "PurchasesPrice", "ImportPrice", "BADGENUMBER", "UserCanRptExportButton", "UserCanRecieve1", "UserCanRecieve2"}
        bm.control = New Control() {txtID, ArName, Address, DateOfBirth, DepartmentId, Notes, Nurse, Receptionist, Manager, SystemUser, NationalId, HomePhone, Mobile, Email, Password, EnName, LevelId, Doctor, Stopped, HiringDate, Duration, Cnt, hh, hh2, hh3, hh4, hh5, hh6, hh7, mm, mm2, mm3, mm4, mm5, mm6, mm7, Saturday, Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, IsSalary, IsFreelancer, LateAllowance, Allowance, SpAllowance, FromHH, ToHH, FromMM, ToMM, Salary, SalaryOnly, ShiftsOnly, SalaryOrShifts, ShiftCount, ShiftValue, Tax, Bonus, Insurance, Annual, NoofDaysOff, NoofMonthlyExecuses, HolidayWorkValue, DelayValue, OvertimeValue, IsFixedHours, TotalHours, TotalMinutes, HasAttendance, MainJobId, SubJobId, TaxAccNo, AccNo, StoreId, SaveId, Waiter, BankId, FromHHTemp, FromMMTemp, ToHHTemp, ToMMTemp, TotalHoursTemp, TotalMinutesTemp, FromDate, ToDate, EditPrices, DegreeId, GraduationYear, NoLicenseToPractice, SalesPrice, PurchasesPrice, ImportPrice, BADGENUMBER, UserCanRptExportButton, UserCanRecieve1, UserCanRecieve2}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName


        LoadWFH()


        SalesPrice.Visibility = Windows.Visibility.Hidden
        PurchasesPrice.Visibility = Windows.Visibility.Hidden
        ImportPrice.Visibility = Windows.Visibility.Hidden

        UserCanRecieve1.Visibility = Windows.Visibility.Hidden
        UserCanRecieve2.Visibility = Windows.Visibility.Hidden

        TabControl1.Visibility = Windows.Visibility.Hidden

        btnNew_Click(sender, e)
        Doctor_UnChecked(Nothing, Nothing)
        IsSalary_Checked(Nothing, Nothing)
        IsFixedHours_Unchecked(Nothing, Nothing)
    End Sub


    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Price As String = "Price"
    End Structure

    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Id, "الكود")
        G.Columns.Add(GC.Name, "الاسم")
        G.Columns.Add(GC.Price, "القيمة")

        G.Columns(GC.Name).FillWeight = 300
        G.Columns(GC.Id).ReadOnly = True
        G.Columns(GC.Name).ReadOnly = True
        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False
    End Sub


    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim loplop As Boolean = False
    Sub FillControls()
        loplop = True
        bm.FillControls(Me)
        bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        GetGrid()

        LevelId_LostFocus(Nothing, Nothing)
        DepartmentId_LostFocus(Nothing, Nothing)

        StoreId_LostFocus(Nothing, Nothing)
        SaveId_LostFocus(Nothing, Nothing)
        BankId_LostFocus(Nothing, Nothing)

        MainJobId_LostFocus(Nothing, Nothing)
        SubJobId_LostFocus(Nothing, Nothing)
        TaxAccNo_LostFocus(Nothing, Nothing)
        AccNo_LostFocus(Nothing, Nothing)
        DegreeId_LostFocus(Nothing, Nothing)
        loplop = False
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
        If ArName.Text.Trim = "" OrElse Not bm.TestNames(ArName, EnName) Then
            ArName.Focus()
            Return
        End If
        Salary.Text = Val(Salary.Text)
        ShiftCount.Text = Val(ShiftCount.Text)
        ShiftValue.Text = Val(ShiftValue.Text)
        Tax.Text = Val(Tax.Text)

        G.EndEdit()
        If Tax.Visibility = Windows.Visibility.Visible AndAlso Val(Tax.Text) <> 0 AndAlso Val(TaxAccNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد حساب الضريبة")
            TaxAccNo.Focus()
            Return
        End If


        
        Bonus.Text = Val(Bonus.Text)
        Allowance.Text = Val(Allowance.Text)
        SpAllowance.Text = Val(SpAllowance.Text)
        LateAllowance.Text = Val(LateAllowance.Text)
        Insurance.Text = Val(Insurance.Text)
        Annual.Text = Val(Annual.Text)
        NoofDaysOff.Text = Val(NoofDaysOff.Text)
        NoofMonthlyExecuses.Text = Val(NoofMonthlyExecuses.Text)
        HolidayWorkValue.Text = Val(HolidayWorkValue.Text)

        TotalHours.Text = Val(TotalHours.Text)
        TotalMinutes.Text = Val(TotalMinutes.Text)

        FromHH.Text = Val(FromHH.Text)
        ToHH.Text = Val(ToHH.Text)

        FromMM.Text = Val(FromMM.Text)
        ToMM.Text = Val(ToMM.Text)
        DelayValue.Text = Val(DelayValue.Text)
        OvertimeValue.Text = Val(OvertimeValue.Text)

        TotalHoursTemp.Text = Val(TotalHoursTemp.Text)
        TotalMinutesTemp.Text = Val(TotalMinutesTemp.Text)

        FromHHTemp.Text = Val(FromHHTemp.Text)
        ToHHTemp.Text = Val(ToHHTemp.Text)

        FromMMTemp.Text = Val(FromMMTemp.Text)
        ToMMTemp.Text = Val(ToMMTemp.Text)

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If Not bm.SaveGrid(G, TableNameDetailed, New String() {"EmpId"}, New String() {txtID.Text}, New String() {"VisitingTypeId", "Price"}, New String() {GC.Id, GC.Price}, New VariantType() {VariantType.Integer, VariantType.Decimal}, New String() {GC.Id}) Then Return

        bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Private Sub MainJobId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainJobId.KeyUp
        bm.ShowHelp("MainJobs", MainJobId, MainJobName, e, "select cast(Id as varchar(100)) Id,Name from MainJobs", "MainJobs")
    End Sub

    Private Sub SubJobId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubJobId.KeyUp
        bm.ShowHelp("SubJobs", SubJobId, SubJobName, e, "select cast(Id as varchar(100)) Id,Name from SubJobs where MainJobId=" & MainJobId.Text.Trim, "SubJobs", {"MainJobId"}, {Val(MainJobId.Text)})
    End Sub

    Private Sub DegreeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles DegreeId.KeyUp
        If bm.ShowHelp("Degrees", DegreeId, DegreeName, e, "select cast(Id as varchar(100)) Id,Name from Degrees", "Degrees") Then
            DegreeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub MainJobId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainJobId.LostFocus
        bm.LostFocus(MainJobId, MainJobName, "select Name from MainJobs where Id=" & MainJobId.Text.Trim())
        SubJobId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub SubJobId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubJobId.LostFocus
        bm.LostFocus(SubJobId, SubJobName, "select Name from SubJobs where MainJobId=" & MainJobId.Text.Trim & " and Id=" & SubJobId.Text.Trim())
    End Sub

    Private Sub DegreeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DegreeId.LostFocus
        bm.LostFocus(DegreeId, DegreeName, "select Name from Degrees where Id=" & DegreeId.Text.Trim())

        If loplop Then Return
        GetGridDegree()
    End Sub

    Sub ClearControls()
        bm.ClearControls()

        SalesPrice.IsChecked = True
        PurchasesPrice.IsChecked = True
        ImportPrice.IsChecked = True

        bm.SetNoImage(Image1, True)
        GetGrid()

        Password.Password = 123
        IsSalary.IsChecked = True
        SalaryOnly.IsChecked = True

        Saturday.IsChecked = True
        Sunday.IsChecked = True
        Monday.IsChecked = True
        Tuesday.IsChecked = True
        Wednesday.IsChecked = True
        Thursday.IsChecked = True
        Friday.IsChecked = True

        LevelName.Clear()
        DepartmentName.Clear()
        MainJobName.Clear()
        SubJobName.Clear()
        TaxAccName.Clear()
        AccName.Clear()
        StoreName.Clear()
        SaveName.Clear()
        BankName.Clear()
        DegreeName.Clear()

        ArName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        BADGENUMBER.Text = txtID.Text

        ArName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False


    Private Sub txtID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp("Employees", txtID, ArName, e, "Select cast(Id as varchar(10))Id," & Resources.Item("CboName") & " Name from Employees") Then
            txtID_LostFocus(sender, Nothing)
        End If
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
            ArName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        ArName.SelectAll()
        ArName.Focus()
        ArName.SelectAll()
        ArName.Focus()
        'arName.Text = dt(0)("Name")
    End Sub

    Private Sub LevelId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles LevelId.KeyUp
        bm.ShowHelp("Security Levels", LevelId, LevelName, e, "select cast(Id as varchar(100)) Id,Name from Levels")
    End Sub

    Private Sub DepartmentId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles DepartmentId.KeyUp
        bm.ShowHelp("Departments", DepartmentId, DepartmentName, e, "select cast(Id as varchar(100)) Id,Name from Departments", "Departments")
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Stores", "Stores")
    End Sub

    Private Sub SaveId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SaveId.KeyUp
        bm.ShowHelp("Saves", SaveId, SaveName, e, "select cast(Id as varchar(100)) Id,Name from Saves", "Saves")
    End Sub

    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyUp
        bm.ShowHelp("Banks", BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Banks", "Banks")
    End Sub


    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, LevelId.KeyDown, DepartmentId.KeyDown, Duration.KeyDown, Cnt.KeyDown, hh.KeyDown, hh2.KeyDown, hh3.KeyDown, hh4.KeyDown, hh5.KeyDown, hh6.KeyDown, hh7.KeyDown, mm.KeyDown, mm2.KeyDown, mm3.KeyDown, mm4.KeyDown, mm5.KeyDown, mm6.KeyDown, mm7.KeyDown, TotalHours.KeyDown, TotalMinutes.KeyDown, StoreId.KeyDown, SaveId.KeyDown, BankId.KeyDown, TotalHoursTemp.KeyDown, TotalMinutesTemp.KeyDown, DegreeId.KeyDown, BADGENUMBER.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub




    Private Sub LevelId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles LevelId.LostFocus
        bm.LostFocus(LevelId, LevelName, "select Name from Levels where Id=" & LevelId.Text.Trim())
    End Sub

    Private Sub DepartmentId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DepartmentId.LostFocus
        bm.LostFocus(DepartmentId, DepartmentName, "select Name from Departments where Id=" & DepartmentId.Text.Trim())
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Stores where Id=" & StoreId.Text.Trim())
    End Sub

    Private Sub SaveId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.LostFocus
        bm.LostFocus(SaveId, SaveName, "select Name from Saves where Id=" & SaveId.Text.Trim())
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus
        bm.LostFocus(BankId, BankName, "select Name from Banks where Id=" & BankId.Text.Trim())
    End Sub

    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        DontClear = True
        btnSave_Click(btnSave, Nothing)
        DontClear = False
        If Not AllowSave Then Return
        bm.SetImage(Image1)
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, True, True)
    End Sub

    Private Sub ArName_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ArName.LostFocus
        ArName.Text = ArName.Text.Trim
        EnName.Text = bm.GetEnName(ArName.Text.Trim)
    End Sub



    Private Sub Doctor_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Doctor.Checked
        lblDuration.Visibility = Visibility.Visible
        lblMinutes.Visibility = Visibility.Visible
        'lblWorkDays.Visibility = Visibility.Visible
        Duration.Visibility = Visibility.Visible

        lblCnt.Visibility = Visibility.Visible
        Cnt.Visibility = Visibility.Visible


        lblmm.Visibility = Visibility.Visible
        lblhh.Visibility = Visibility.Visible

        hh.Visibility = Visibility.Visible
        hh2.Visibility = Visibility.Visible
        hh3.Visibility = Visibility.Visible
        hh4.Visibility = Visibility.Visible
        hh5.Visibility = Visibility.Visible
        hh6.Visibility = Visibility.Visible
        hh7.Visibility = Visibility.Visible
        mm.Visibility = Visibility.Visible
        mm2.Visibility = Visibility.Visible
        mm3.Visibility = Visibility.Visible
        mm4.Visibility = Visibility.Visible
        mm5.Visibility = Visibility.Visible
        mm6.Visibility = Visibility.Visible
        mm7.Visibility = Visibility.Visible

        'Saturday.Visibility = Visibility.Visible
        'Sunday.Visibility = Visibility.Visible
        'Monday.Visibility = Visibility.Visible
        'Tuesday.Visibility = Visibility.Visible
        'Wednesday.Visibility = Visibility.Visible
        'Thursday.Visibility = Visibility.Visible
        'Friday.Visibility = Visibility.Visible

        WFH.Visibility = Visibility.Visible

        'Saturday.IsChecked = True
        'Sunday.IsChecked = True
        'Monday.IsChecked = True
        'Tuesday.IsChecked = True
        'Wednesday.IsChecked = True
        'Thursday.IsChecked = True
        'Friday.IsChecked = True


    End Sub

    Private Sub Doctor_UnChecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Doctor.Unchecked
        lblDuration.Visibility = Visibility.Hidden
        lblMinutes.Visibility = Visibility.Hidden
        'lblWorkDays.Visibility = Visibility.Hidden
        Duration.Visibility = Visibility.Hidden
        Duration.Clear()

        lblCnt.Visibility = Visibility.Hidden
        Cnt.Visibility = Visibility.Hidden
        Cnt.Clear()


        lblmm.Visibility = Visibility.Hidden
        lblhh.Visibility = Visibility.Hidden

        hh.Visibility = Visibility.Hidden
        hh2.Visibility = Visibility.Hidden
        hh3.Visibility = Visibility.Hidden
        hh4.Visibility = Visibility.Hidden
        hh5.Visibility = Visibility.Hidden
        hh6.Visibility = Visibility.Hidden
        hh7.Visibility = Visibility.Hidden
        mm.Visibility = Visibility.Hidden
        mm2.Visibility = Visibility.Hidden
        mm3.Visibility = Visibility.Hidden
        mm4.Visibility = Visibility.Hidden
        mm5.Visibility = Visibility.Hidden
        mm6.Visibility = Visibility.Hidden
        mm7.Visibility = Visibility.Hidden

        hh.Clear()
        hh2.Clear()
        hh3.Clear()
        hh4.Clear()
        hh5.Clear()
        hh6.Clear()
        hh7.Clear()
        mm.Clear()
        mm2.Clear()
        mm3.Clear()
        mm4.Clear()
        mm5.Clear()
        mm6.Clear()
        mm7.Clear()

        'Saturday.Visibility = Visibility.Hidden
        'Sunday.Visibility = Visibility.Hidden
        'Monday.Visibility = Visibility.Hidden
        'Tuesday.Visibility = Visibility.Hidden
        'Wednesday.Visibility = Visibility.Hidden
        'Thursday.Visibility = Visibility.Hidden
        'Friday.Visibility = Visibility.Hidden

        WFH.Visibility = Visibility.Hidden

        'Saturday.IsChecked = False
        'Sunday.IsChecked = False
        'Monday.IsChecked = False
        'Tuesday.IsChecked = False
        'Wednesday.IsChecked = False
        'Thursday.IsChecked = False
        'Friday.IsChecked = False


    End Sub


    Private Sub IsSalary_Checked(sender As Object, e As RoutedEventArgs) Handles IsSalary.Checked, IsSalary.Unchecked, IsFreelancer.Checked, IsFreelancer.Unchecked
        If sender Is IsSalary Then IsFreelancer.IsChecked = Not IsSalary.IsChecked Else IsSalary.IsChecked = Not IsFreelancer.IsChecked
        If IsSalary.IsChecked Then
            LblSalaryType.Content = Resources.Item("Salary")
            lblLE3.Content = Resources.Item("L.E.")
            LateAllowance.Visibility = Windows.Visibility.Visible
            FromHH.Visibility = Windows.Visibility.Visible
            ToHH.Visibility = Windows.Visibility.Visible
            FromMM.Visibility = Windows.Visibility.Visible
            ToMM.Visibility = Windows.Visibility.Visible
            lblLateAllowance.Visibility = Windows.Visibility.Visible
            lblMinutes2.Visibility = Windows.Visibility.Visible

            FromHHTemp.Visibility = Windows.Visibility.Visible
            ToHHTemp.Visibility = Windows.Visibility.Visible
            FromMMTemp.Visibility = Windows.Visibility.Visible
            ToMMTemp.Visibility = Windows.Visibility.Visible
            
            IsFixedHours.Visibility = Windows.Visibility.Visible
            lblFromHH.Visibility = Windows.Visibility.Visible
            lblToHH.Visibility = Windows.Visibility.Visible
            lblFromMM.Visibility = Windows.Visibility.Visible
            lblToMM.Visibility = Windows.Visibility.Visible
            lblFrom.Visibility = Windows.Visibility.Visible
            lblTo.Visibility = Windows.Visibility.Visible

            lblFromHHTemp.Visibility = Windows.Visibility.Visible
            lblToHHTemp.Visibility = Windows.Visibility.Visible
            lblFromMMTemp.Visibility = Windows.Visibility.Visible
            lblToMMTemp.Visibility = Windows.Visibility.Visible
            lblFromTemp.Visibility = Windows.Visibility.Visible
            lblToTemp.Visibility = Windows.Visibility.Visible

        Else
            LblSalaryType.Content = Resources.Item("Perc")
            lblLE3.Content = "%"
            LateAllowance.Visibility = Windows.Visibility.Hidden
            FromHH.Visibility = Windows.Visibility.Hidden
            ToHH.Visibility = Windows.Visibility.Hidden
            FromMM.Visibility = Windows.Visibility.Hidden
            ToMM.Visibility = Windows.Visibility.Hidden
            lblLateAllowance.Visibility = Windows.Visibility.Hidden
            lblMinutes2.Visibility = Windows.Visibility.Hidden

            FromHHTemp.Visibility = Windows.Visibility.Hidden
            ToHHTemp.Visibility = Windows.Visibility.Hidden
            FromMMTemp.Visibility = Windows.Visibility.Hidden
            ToMMTemp.Visibility = Windows.Visibility.Hidden

            IsFixedHours.Visibility = Windows.Visibility.Hidden
            IsFixedHours.IsChecked = False
            lblFromHH.Visibility = Windows.Visibility.Hidden
            lblToHH.Visibility = Windows.Visibility.Hidden
            lblFromMM.Visibility = Windows.Visibility.Hidden
            lblToMM.Visibility = Windows.Visibility.Hidden
            lblFrom.Visibility = Windows.Visibility.Hidden
            lblTo.Visibility = Windows.Visibility.Hidden

            lblFromHHTemp.Visibility = Windows.Visibility.Hidden
            lblToHHTemp.Visibility = Windows.Visibility.Hidden
            lblFromMMTemp.Visibility = Windows.Visibility.Hidden
            lblToMMTemp.Visibility = Windows.Visibility.Hidden
            lblFromTemp.Visibility = Windows.Visibility.Hidden
            lblToTemp.Visibility = Windows.Visibility.Hidden

        End If

        Salary.Clear()
        LateAllowance.Clear()
        FromHH.Clear()
        ToHH.Clear()
        FromMM.Clear()
        ToMM.Clear()

        FromHHTemp.Clear()
        ToHHTemp.Clear()
        FromMMTemp.Clear()
        ToMMTemp.Clear()

    End Sub


    Private Sub LoadResource()
        btnSetImage.SetResourceReference(Button.ContentProperty, "Change")
        btnSetNoImage.SetResourceReference(Button.ContentProperty, "Cancel")

        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")

        Doctor.SetResourceReference(CheckBox.ContentProperty, "Doctor")
        Nurse.SetResourceReference(CheckBox.ContentProperty, "Nurse")
        Receptionist.SetResourceReference(CheckBox.ContentProperty, "Receptionist")
        Manager.SetResourceReference(CheckBox.ContentProperty, "Manager")
        SystemUser.SetResourceReference(CheckBox.ContentProperty, "SystemUser")
        Stopped.SetResourceReference(CheckBox.ContentProperty, "Stopped")
        IsFreelancer.SetResourceReference(CheckBox.ContentProperty, "IsFreelancer")
        IsSalary.SetResourceReference(CheckBox.ContentProperty, "IsSalary")
        Saturday.SetResourceReference(CheckBox.ContentProperty, "Saturday")
        Sunday.SetResourceReference(CheckBox.ContentProperty, "Sunday")
        Monday.SetResourceReference(CheckBox.ContentProperty, "Monday")
        Tuesday.SetResourceReference(CheckBox.ContentProperty, "Tuesday")
        Wednesday.SetResourceReference(CheckBox.ContentProperty, "Wednesday")
        Thursday.SetResourceReference(CheckBox.ContentProperty, "Thursday")
        Friday.SetResourceReference(CheckBox.ContentProperty, "Friday")

        lblId.SetResourceReference(Label.ContentProperty, "Id")

        lblAddress.SetResourceReference(Label.ContentProperty, "Address")
        lblLateAllowance.SetResourceReference(Label.ContentProperty, "Late Allowance")
        lblMinutes2.SetResourceReference(Label.ContentProperty, "Minutes")
        lblMinutes.SetResourceReference(Label.ContentProperty, "Minutes")
        lblArName.SetResourceReference(Label.ContentProperty, "ArName")
        lblCnt.SetResourceReference(Label.ContentProperty, "Max Res. count")
        lblDateOfBirth.SetResourceReference(Label.ContentProperty, "DateOfBirth")
        lblDepartmentId.SetResourceReference(Label.ContentProperty, "Department")
        lblDuration.SetResourceReference(Label.ContentProperty, "Duration")
        lblEmail.SetResourceReference(Label.ContentProperty, "Email")
        lblEnName.SetResourceReference(Label.ContentProperty, "EnName")

        lblFrom.SetResourceReference(Label.ContentProperty, "From")
        lblFromHH.SetResourceReference(Label.ContentProperty, "hh")
        lblFromMM.SetResourceReference(Label.ContentProperty, "mm")
        lblFromHH2.SetResourceReference(Label.ContentProperty, "hh")
        lblFromMM2.SetResourceReference(Label.ContentProperty, "mm")

        lblFromTemp.SetResourceReference(Label.ContentProperty, "From")
        lblFromHHTemp.SetResourceReference(Label.ContentProperty, "hh")
        lblFromMMTemp.SetResourceReference(Label.ContentProperty, "mm")
        lblFromHH2Temp.SetResourceReference(Label.ContentProperty, "hh")
        lblFromMM2Temp.SetResourceReference(Label.ContentProperty, "mm")

        lblhh.SetResourceReference(Label.ContentProperty, "hh")
        lblHiringDate.SetResourceReference(Label.ContentProperty, "HiringDate")
        lblLevelId.SetResourceReference(Label.ContentProperty, "Security Level")
        lblLE3.SetResourceReference(Label.ContentProperty, "L.E.")
        lblmm.SetResourceReference(Label.ContentProperty, "mm")
        lblMobile.SetResourceReference(Label.ContentProperty, "Mobile")
        lblMobile.SetResourceReference(Label.ContentProperty, "Mobile")
        lblNationalID.SetResourceReference(Label.ContentProperty, "National ID")
        lblNotes.SetResourceReference(Label.ContentProperty, "Notes")
        lblPassword.SetResourceReference(Label.ContentProperty, "Password")
        LblSalaryType.SetResourceReference(Label.ContentProperty, "Salary Type")
        lblTel.SetResourceReference(Label.ContentProperty, "Tel")
        lblBADGENUMBER.SetResourceReference(Label.ContentProperty, "BADGENUMBER")

        lblTo.SetResourceReference(Label.ContentProperty, "To")
        lblToHH.SetResourceReference(Label.ContentProperty, "hh")
        lblToMM.SetResourceReference(Label.ContentProperty, "mm")
        lblhh.SetResourceReference(Label.ContentProperty, "hh")
        lblmm.SetResourceReference(Label.ContentProperty, "mm")

        lblToTemp.SetResourceReference(Label.ContentProperty, "To")
        lblToHHTemp.SetResourceReference(Label.ContentProperty, "hh")
        lblToMMTemp.SetResourceReference(Label.ContentProperty, "mm")

        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")

        lblShiftCount.SetResourceReference(Label.ContentProperty, "Shift Count")
        lblShiftValue.SetResourceReference(Label.ContentProperty, "Shift Value")

        SalaryOnly.SetResourceReference(CheckBox.ContentProperty, "Salary Only")
        ShiftsOnly.SetResourceReference(CheckBox.ContentProperty, "Shifts Only")
        SalaryOrShifts.SetResourceReference(CheckBox.ContentProperty, "Salary Or Shifts")
        lblTax.SetResourceReference(Label.ContentProperty, "Tax")

        LblBonus.SetResourceReference(Label.ContentProperty, "Bonus")
        LblAllowance.SetResourceReference(Label.ContentProperty, "Allowance")
        LblSpAllowance.SetResourceReference(Label.ContentProperty, "SpAllowance")
        LblInsurance.SetResourceReference(Label.ContentProperty, "Insurance")

        lblLE4.SetResourceReference(Label.ContentProperty, "L.E.")
        lblLE5.SetResourceReference(Label.ContentProperty, "L.E.")
        lblLE6.SetResourceReference(Label.ContentProperty, "L.E.")
        lblLE7.SetResourceReference(Label.ContentProperty, "L.E.")

        lblDays.SetResourceReference(Label.ContentProperty, "Days")
        lblDays2.SetResourceReference(Label.ContentProperty, "Days")
        lblTimes.SetResourceReference(Label.ContentProperty, "Times")
        lblDays3.SetResourceReference(Label.ContentProperty, "Days")
        LblNoofAnnual.SetResourceReference(Label.ContentProperty, "No of Annual")
        LblNoofDaysOff.SetResourceReference(Label.ContentProperty, "No of DaysOff")
        LblNoofMonthlyExecuses.SetResourceReference(Label.ContentProperty, "No of Monthly Execuses")
        LblHolidayWorkValue.SetResourceReference(Label.ContentProperty, "Holiday Work Value")

        TabItemSalary.SetResourceReference(TabItem.HeaderProperty, "Fixed Salary Items")
        TabSpecialDyas.SetResourceReference(TabItem.HeaderProperty, "Special Dyas")
        TabItemChangedSalary.SetResourceReference(TabItem.HeaderProperty, "Changed Salary Items")
        TabItemDaysOff.SetResourceReference(TabItem.HeaderProperty, "DaysOff")
        TabItemShifts.SetResourceReference(TabItem.HeaderProperty, "Shifts")
        TabItemJobDescribtion.SetResourceReference(TabItem.HeaderProperty, "JobDescribtion")

        LblDelayValue.SetResourceReference(Label.ContentProperty, "DelayValue")
        LblOvertimeValue.SetResourceReference(Label.ContentProperty, "OvertimeValue")
        lblhh1.SetResourceReference(Label.ContentProperty, "hh")
        lblhh2.SetResourceReference(Label.ContentProperty, "hh")
        IsFixedHours.SetResourceReference(CheckBox.ContentProperty, "FixedHours")
        HasAttendance.SetResourceReference(CheckBox.ContentProperty, "HasAttendance")

        lblMainJob.SetResourceReference(Label.ContentProperty, "MainJob")
        lblSubJobId.SetResourceReference(Label.ContentProperty, "SubJob")
        lblTaxAcc.SetResourceReference(Label.ContentProperty, "TaxAcc")
        lblStoreId.SetResourceReference(Label.ContentProperty, "Store")
        lblSaveId.SetResourceReference(Label.ContentProperty, "Safe")
        lblBankId.SetResourceReference(Label.ContentProperty, "Bank")

        EditPrices.SetResourceReference(Label.ContentProperty, "EditPrices")

        lblDegreeId.SetResourceReference(Label.ContentProperty, "Degree")
        lblGraduationYear.SetResourceReference(Label.ContentProperty, "GraduationYear")
        lblNoLicenseToPractice.SetResourceReference(Label.ContentProperty, "NoLicenseToPractice")

        SalesPrice.SetResourceReference(CheckBox.ContentProperty, "SalesPrice")
        PurchasesPrice.SetResourceReference(CheckBox.ContentProperty, "PurchasesPrice")
        ImportPrice.SetResourceReference(CheckBox.ContentProperty, "ImportPrice")

    End Sub

    Dim lop As Boolean = False
    Private Sub SalaryOnly_Checked(sender As Object, e As RoutedEventArgs) Handles SalaryOnly.Checked, ShiftsOnly.Checked, SalaryOrShifts.Checked
        If lop Then Return
        lop = True
        If sender Is SalaryOnly Then
            ShiftsOnly.IsChecked = False
            SalaryOrShifts.IsChecked = False
        ElseIf sender Is ShiftsOnly Then
            SalaryOnly.IsChecked = False
            SalaryOrShifts.IsChecked = False
        ElseIf sender Is SalaryOrShifts Then
            ShiftsOnly.IsChecked = False
            SalaryOnly.IsChecked = False
        End If
        lop = False
    End Sub

    Private Sub SalaryOnly_Unchecked(sender As Object, e As RoutedEventArgs) Handles SalaryOnly.Unchecked, ShiftsOnly.Unchecked, SalaryOrShifts.Unchecked
        If lop Then Return
        lop = True
        If sender Is SalaryOnly Then
            ShiftsOnly.IsChecked = Not SalaryOnly.IsChecked
        ElseIf sender Is ShiftsOnly Then
            SalaryOrShifts.IsChecked = Not ShiftsOnly.IsChecked
        ElseIf sender Is SalaryOrShifts Then
            SalaryOnly.IsChecked = Not SalaryOrShifts.IsChecked
        End If
        lop = False
    End Sub

    Private Sub IsFixedHours_Checked(sender As Object, e As RoutedEventArgs) Handles IsFixedHours.Checked
        lblFromHH2.IsEnabled = True
        lblFromMM2.IsEnabled = True
        TotalHours.IsEnabled = True
        TotalMinutes.IsEnabled = True

        lblFromHH2Temp.IsEnabled = True
        lblFromMM2Temp.IsEnabled = True
        TotalHoursTemp.IsEnabled = True
        TotalMinutesTemp.IsEnabled = True

        lblFrom.Visibility = Windows.Visibility.Hidden
        lblTo.Visibility = Windows.Visibility.Hidden
        lblFromHH.Visibility = Windows.Visibility.Hidden
        lblToHH.Visibility = Windows.Visibility.Hidden
        lblFromMM.Visibility = Windows.Visibility.Hidden
        lblToMM.Visibility = Windows.Visibility.Hidden
        FromHH.Visibility = Windows.Visibility.Hidden
        ToHH.Visibility = Windows.Visibility.Hidden
        FromMM.Visibility = Windows.Visibility.Hidden
        ToMM.Visibility = Windows.Visibility.Hidden

        lblFromTemp.Visibility = Windows.Visibility.Hidden
        lblToTemp.Visibility = Windows.Visibility.Hidden
        lblFromHHTemp.Visibility = Windows.Visibility.Hidden
        lblToHHTemp.Visibility = Windows.Visibility.Hidden
        lblFromMMTemp.Visibility = Windows.Visibility.Hidden
        lblToMMTemp.Visibility = Windows.Visibility.Hidden
        FromHHTemp.Visibility = Windows.Visibility.Hidden
        ToHHTemp.Visibility = Windows.Visibility.Hidden
        FromMMTemp.Visibility = Windows.Visibility.Hidden
        ToMMTemp.Visibility = Windows.Visibility.Hidden

    End Sub

    Private Sub IsFixedHours_Unchecked(sender As Object, e As RoutedEventArgs) Handles IsFixedHours.Unchecked
        lblFromHH2.IsEnabled = False
        lblFromMM2.IsEnabled = False
        TotalHours.IsEnabled = False
        TotalMinutes.IsEnabled = False

        lblFromHH2Temp.IsEnabled = False
        lblFromMM2Temp.IsEnabled = False
        TotalHoursTemp.IsEnabled = False
        TotalMinutesTemp.IsEnabled = False

        lblFrom.Visibility = Windows.Visibility.Visible
        lblTo.Visibility = Windows.Visibility.Visible
        lblFromHH.Visibility = Windows.Visibility.Visible
        lblToHH.Visibility = Windows.Visibility.Visible
        lblFromMM.Visibility = Windows.Visibility.Visible
        lblToMM.Visibility = Windows.Visibility.Visible
        FromHH.Visibility = Windows.Visibility.Visible
        ToHH.Visibility = Windows.Visibility.Visible
        FromMM.Visibility = Windows.Visibility.Visible
        ToMM.Visibility = Windows.Visibility.Visible

        lblFromTemp.Visibility = Windows.Visibility.Visible
        lblToTemp.Visibility = Windows.Visibility.Visible
        lblFromHHTemp.Visibility = Windows.Visibility.Visible
        lblToHHTemp.Visibility = Windows.Visibility.Visible
        lblFromMMTemp.Visibility = Windows.Visibility.Visible
        lblToMMTemp.Visibility = Windows.Visibility.Visible
        FromHHTemp.Visibility = Windows.Visibility.Visible
        ToHHTemp.Visibility = Windows.Visibility.Visible
        FromMMTemp.Visibility = Windows.Visibility.Visible
        ToMMTemp.Visibility = Windows.Visibility.Visible

    End Sub

    Sub CalcTotalHoursMinutes() Handles FromHH.LostFocus, ToHH.LostFocus, FromMM.LostFocus, ToMM.LostFocus, FromHHTemp.LostFocus, ToHHTemp.LostFocus, FromMMTemp.LostFocus, ToMMTemp.LostFocus, IsFixedHours.Checked, IsFixedHours.Unchecked
        If TotalHours.IsEnabled Then Return
        Dim i As Integer = (Val(ToHH.Text) * 60 + Val(ToMM.Text)) - (Val(FromHH.Text) * 60 + Val(FromMM.Text))
        If i < 0 Then
            TotalHours.Text = 0
            TotalMinutes.Text = 0
            ToHH.Text = FromHH.Text
            ToMM.Text = FromMM.Text
            Return
        End If
        TotalMinutes.Text = i Mod 60
        TotalHours.Text = (i - Val(TotalMinutes.Text)) / 60

        i = (Val(ToHHTemp.Text) * 60 + Val(ToMMTemp.Text)) - (Val(FromHHTemp.Text) * 60 + Val(FromMMTemp.Text))
        If i < 0 Then
            TotalHoursTemp.Text = 0
            TotalMinutesTemp.Text = 0
            ToHHTemp.Text = FromHHTemp.Text
            ToMMTemp.Text = FromMMTemp.Text
            Return
        End If
        TotalMinutesTemp.Text = i Mod 60
        TotalHoursTemp.Text = (i - Val(TotalMinutesTemp.Text)) / 60
    End Sub

    Private Sub TaxAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TaxAccNo.LostFocus
        bm.AccNoLostFocus(TaxAccNo, TaxAccName, , , )
    End Sub

    Private Sub TaxAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TaxAccNo.KeyUp
        bm.AccNoShowHelp(TaxAccNo, TaxAccName, e, , , )
    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , , )
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , , )
    End Sub


    Private Sub Saturday_Checked(sender As Object, e As RoutedEventArgs) Handles Saturday.Checked
        hh.IsEnabled = True
        mm.IsEnabled = True
    End Sub
    Private Sub Saturday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Saturday.Unchecked
        hh.Clear()
        mm.Clear()
        hh.IsEnabled = False
        mm.IsEnabled = False
    End Sub

    Private Sub Sunday_Checked(sender As Object, e As RoutedEventArgs) Handles Sunday.Checked
        hh2.IsEnabled = True
        mm2.IsEnabled = True
    End Sub
    Private Sub Sunday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Sunday.Unchecked
        hh2.Clear()
        mm2.Clear()
        hh2.IsEnabled = False
        mm2.IsEnabled = False
    End Sub

    Private Sub Monday_Checked(sender As Object, e As RoutedEventArgs) Handles Monday.Checked
        hh3.IsEnabled = True
        mm3.IsEnabled = True
    End Sub
    Private Sub Monday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Monday.Unchecked
        hh3.Clear()
        mm3.Clear()
        hh3.IsEnabled = False
        mm3.IsEnabled = False
    End Sub

    Private Sub Tuesday_Checked(sender As Object, e As RoutedEventArgs) Handles Tuesday.Checked
        hh4.IsEnabled = True
        mm4.IsEnabled = True
    End Sub
    Private Sub Tuesday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Tuesday.Unchecked
        hh4.Clear()
        mm4.Clear()
        hh4.IsEnabled = False
        mm4.IsEnabled = False
    End Sub

    Private Sub Wednesday_Checked(sender As Object, e As RoutedEventArgs) Handles Wednesday.Checked
        hh5.IsEnabled = True
        mm5.IsEnabled = True
    End Sub
    Private Sub Wednesday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Wednesday.Unchecked
        hh5.Clear()
        mm5.Clear()
        hh5.IsEnabled = False
        mm5.IsEnabled = False
    End Sub

    Private Sub Thursday_Checked(sender As Object, e As RoutedEventArgs) Handles Thursday.Checked
        hh6.IsEnabled = True
        mm6.IsEnabled = True
    End Sub
    Private Sub Thursday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Thursday.Unchecked
        hh6.Clear()
        mm6.Clear()
        hh6.IsEnabled = False
        mm6.IsEnabled = False
    End Sub

    Private Sub Friday_Checked(sender As Object, e As RoutedEventArgs) Handles Friday.Checked
        hh7.IsEnabled = True
        mm7.IsEnabled = True
    End Sub
    Private Sub Friday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Friday.Unchecked
        hh7.Clear()
        mm7.Clear()
        hh7.IsEnabled = False
        mm7.IsEnabled = False
    End Sub

    Private Sub btnStorePermissions_Click(sender As Object, e As RoutedEventArgs) Handles btnStorePermissions.Click
        bm.ShowHelpPermissions("Stores", Val(txtID.Text), "select cast(S.Id as varchar(100)) Id,S.Name,cast((case when E.EmpId is null then 0 else 1 end) as bit) Permission from EmpStores E right join Stores S on(E.StoreId=S.Id and(EmpId=" & Val(txtID.Text) & " or EmpId is null)) ", "EmpStores")
    End Sub

    Private Sub btnPermissions_Click(sender As Object, e As RoutedEventArgs) Handles btnPermissions.Click
        bm.ShowHelpPermissions(MainLinkFile.Text, Val(txtID.Text), "select cast(T.Id as varchar(100)) Id,T.Name,cast((case when E.EmpId is null then 0 else 1 end) as bit) Permission from EmpPermissions E right join AllSub T on(E.LinkFile=T.LinkFile and E.Id=T.Id and(EmpId=" & Val(txtID.Text) & " or EmpId is null)) where T.LinkFile=" & MainLinkFile.SelectedValue, "EmpPermissions", MainLinkFile.SelectedValue, New String() {"LinkFile"}, New String() {MainLinkFile.SelectedValue})
    End Sub

    Private Sub GetGrid()
        Dim dt As DataTable = bm.ExecuteAdapter("select T.Id,T.Name,isnull(E.Price,0)Price from VisitingTypes T left join VisitingTypeEmployees E on(T.Id=E.VisitingTypeId and (E.EmpId=" & Val(txtID.Text) & " or E.EmpId is null))")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("Id").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("Name").ToString
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
        Next
        G.RefreshEdit()
    End Sub

    Private Sub GetGridDegree()
        Dim dt As DataTable = bm.ExecuteAdapter("select T.Id,T.Name,isnull(E.Price,0)Price from VisitingTypes T left join VisitingTypeDegrees E on(T.Id=E.VisitingTypeId) where (E.DegreeId=" & Val(DegreeId.Text) & " or E.DegreeId is null)")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("Id").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("Name").ToString
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
        Next
        G.RefreshEdit()
    End Sub

End Class
