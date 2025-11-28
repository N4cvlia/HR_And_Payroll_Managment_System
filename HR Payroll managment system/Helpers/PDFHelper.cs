using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HR_Payroll_managment_system.Helpers;

public class PDFHelper
{
    private string _currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    
    ActivityLogsService  _activityLogsService = new ActivityLogsService();
    
    Logging _logging = new Logging();

    public void ExportToPDF(Payroll payroll, User currentUser)
    {
        string payslipsFolder = Path.Combine(_currentDirectory,"ExportFiles", "Payslips");
        string payslipsPath = Path.Combine(payslipsFolder, $"Payslip_{payroll.PaymentDate}_{currentUser.EmployeeProfile.FirstName}_{currentUser.EmployeeProfile.LastName}.pdf");
        
        using (var writer = new PdfWriter(payslipsPath))
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            
                // HEADER SECTION
                var header = new Paragraph("PAYSLIP")
                    .SetFont(boldFont)
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(ColorConstants.BLUE);
                document.Add(header);
            
                document.Add(new Paragraph(" ")); // Spacing
            
                // COMPANY INFO
                var companyInfo = new Paragraph("HR Payroll Management System")
                    .SetFont(normalFont)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(ColorConstants.DARK_GRAY);
                document.Add(companyInfo); 
                
                document.Add(new Paragraph(" ")); // Spacing
            
                // EMPLOYEE & PAYMENT INFO TABLE
                float[] columnWidths = { 1, 1 };
                Table infoTable = new Table(columnWidths);
            
                // Employee Details
                infoTable.AddCell(CreateCell("Employee:", true));
                infoTable.AddCell(CreateCell($"{currentUser.EmployeeProfile.FirstName} {currentUser.EmployeeProfile.LastName}", false));
            
                infoTable.AddCell(CreateCell("Employee ID:", true));
                infoTable.AddCell(CreateCell(payroll.EmployeeId.ToString(), false));
            
                infoTable.AddCell(CreateCell("Department:", true));
                infoTable.AddCell(CreateCell(currentUser.EmployeeProfile.Department.DepartmentName, false));
            
                infoTable.AddCell(CreateCell("Position:", true));
                infoTable.AddCell(CreateCell(currentUser.EmployeeProfile.JobPosition.PositionTitle, false));
                
                infoTable.AddCell(CreateCell("Pay Period:", true));
                infoTable.AddCell(CreateCell(payroll.PaymentDate.ToString("MMMM yyyy"), false));
            
                infoTable.AddCell(CreateCell("Payment Date:", true));
                infoTable.AddCell(CreateCell(payroll.PaymentDate.ToString("MMM dd, yyyy"), false));
            
                infoTable.AddCell(CreateCell("Status:", true));
                infoTable.AddCell(CreateCell(payroll.IsProcessed ? "PAID" : "PENDING", false));
            
                document.Add(infoTable);
                document.Add(new Paragraph(" ")); // Spacing
                
                // EARNINGS SECTION
                var earningsHeader = new Paragraph("EARNINGS")
                    .SetFont(boldFont)
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.DARK_GRAY);
                document.Add(earningsHeader);
            
                Table earningsTable = new Table(new float[] { 3, 1 });
                earningsTable.SetWidth(UnitValue.CreatePercentValue(100));
            
                // Earnings Header Row
                earningsTable.AddHeaderCell(CreateHeaderCell("Description"));
                earningsTable.AddHeaderCell(CreateHeaderCell("Amount"));
            
                // Basic Salary
                earningsTable.AddCell(CreateCell("Basic Salary", false));
                earningsTable.AddCell(CreateCell(payroll.BaseSalary.ToString("C2"), false));
                
                // Bonuses
                foreach (var bonus in payroll.Bonuses)
                {
                    earningsTable.AddCell(CreateCell(bonus.BonusType, false));
                    earningsTable.AddCell(CreateCell(bonus.Amount.ToString("C2"), false));
                }
            
                // Total Earnings
                earningsTable.AddCell(CreateCell("TOTAL EARNINGS", true));
                earningsTable.AddCell(CreateCell(payroll.NetSalary.ToString("C2"), true));
            
                document.Add(earningsTable);
                document.Add(new Paragraph(" ")); // Spacing
                
                // DEDUCTIONS SECTION
                var deductionsHeader = new Paragraph("DEDUCTIONS")
                    .SetFont(boldFont)
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.RED);
                document.Add(deductionsHeader);
            
                Table deductionsTable = new Table(new float[] { 3, 1 });
                deductionsTable.SetWidth(UnitValue.CreatePercentValue(100));
            
                // Deductions Header Row
                deductionsTable.AddHeaderCell(CreateHeaderCell("Description"));
                deductionsTable.AddHeaderCell(CreateHeaderCell("Amount"));
            
                // Deductions
                foreach (var deduction in payroll.Deductions)
                {
                    deductionsTable.AddCell(CreateCell(deduction.DeductionType, false));
                    deductionsTable.AddCell(CreateCell("-" + deduction.Amount.ToString("C2"), false));
                }

                // Total Deductions
                decimal totalDeductions = payroll.Deductions.Sum(d => d.Amount);
                deductionsTable.AddCell(CreateCell("TOTAL DEDUCTIONS", true));
                deductionsTable.AddCell(CreateCell("-" + totalDeductions.ToString("C2"), true));
            
                document.Add(deductionsTable);
                document.Add(new Paragraph(" ")); // Spacing
            
                // NET PAY SECTION
                Table netPayTable = new Table(new float[] { 3, 1 });
                netPayTable.SetWidth(UnitValue.CreatePercentValue(100));
            
                decimal netPay = payroll.BaseSalary + payroll.Bonuses.Sum(b => b.Amount) - totalDeductions;
            
                netPayTable.AddCell(CreateCell("NET PAY", true, ColorConstants.LIGHT_GRAY));
                netPayTable.AddCell(CreateCell(netPay.ToString("C2"), true, ColorConstants.LIGHT_GRAY));
            
                document.Add(netPayTable);

                // FOOTER
                document.Add(new Paragraph(" ")); // Spacing
                var footer = new Paragraph("This is a computer-generated document. No signature is required.")
                    .SetFont(normalFont)
                    .SetFontSize(8)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(ColorConstants.GRAY);
                document.Add(footer);
            }
        
        Console.Clear();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Payslip exported to: {payslipsPath}");
        Console.ResetColor();

        ActivityLog activityLog = new ActivityLog()
        {
            UserId = currentUser.Id,
            Action = "Exported Payslips PDF",
            Description = $"{currentUser.Email} Has Exported Their Payslips PDF"
        };

        _activityLogsService.AddActivityLog(activityLog);
        _logging.LogActivity(activityLog, currentUser.Email);
    }
    
    public void ExportToPDFMonthly(Payroll payroll, User currentUser, EmployeeProfile employee)
{
    string payslipsFolder = Path.Combine(_currentDirectory,"ExportFiles", "Payslips");
    
    
    string payslipsPath = Path.Combine(payslipsFolder, 
        $"Payslip_{payroll.PaymentDate:yyyyMMdd}_{employee.FirstName}_{employee.LastName}.pdf");
    
    using (var writer = new PdfWriter(payslipsPath))
    using (var pdf = new PdfDocument(writer))
    using (var document = new Document(pdf))
    {
        var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
    
        // HEADER SECTION
        var header = new Paragraph("PAYSLIP")
            .SetFont(boldFont)
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(ColorConstants.BLUE);
        document.Add(header);
    
        document.Add(new Paragraph(" "));
    
        // COMPANY INFO
        var companyInfo = new Paragraph("HR Payroll Management System")
            .SetFont(normalFont)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(ColorConstants.DARK_GRAY);
        document.Add(companyInfo); 
        
        document.Add(new Paragraph(" "));
    
        // EMPLOYEE & PAYMENT INFO TABLE
        float[] columnWidths = { 1, 1 };
        Table infoTable = new Table(columnWidths);
    
        infoTable.AddCell(CreateCell("Employee:", true));
        infoTable.AddCell(CreateCell($"{employee.FirstName} {employee.LastName}", false));
    
        infoTable.AddCell(CreateCell("Employee ID:", true));
        infoTable.AddCell(CreateCell(payroll.EmployeeId.ToString(), false));
    
        infoTable.AddCell(CreateCell("Department:", true));
        infoTable.AddCell(CreateCell(employee.Department.DepartmentName, false));
    
        infoTable.AddCell(CreateCell("Position:", true));
        infoTable.AddCell(CreateCell(employee.JobPosition.PositionTitle, false));
        
        infoTable.AddCell(CreateCell("Pay Period:", true));
        infoTable.AddCell(CreateCell($"{payroll.PayPeriodStartDate:MMM dd} - {payroll.PayPeriodEndDate:MMM dd, yyyy}", false));
    
        infoTable.AddCell(CreateCell("Payment Date:", true));
        infoTable.AddCell(CreateCell(payroll.PaymentDate.ToString("MMM dd, yyyy"), false));
    
        infoTable.AddCell(CreateCell("Status:", true));
        infoTable.AddCell(CreateCell(payroll.IsProcessed ? "PAID" : "PENDING", false));
    
        document.Add(infoTable);
        document.Add(new Paragraph(" "));
        
        // EARNINGS SECTION
        var earningsHeader = new Paragraph("EARNINGS")
            .SetFont(boldFont)
            .SetFontSize(12)
            .SetFontColor(ColorConstants.GREEN);
        document.Add(earningsHeader);
    
        Table earningsTable = new Table(new float[] { 3, 1 });
        earningsTable.SetWidth(UnitValue.CreatePercentValue(100));
    
        earningsTable.AddHeaderCell(CreateHeaderCell("Description"));
        earningsTable.AddHeaderCell(CreateHeaderCell("Amount"));
    
        // Basic Salary
        earningsTable.AddCell(CreateCell("Basic Salary", false));
        earningsTable.AddCell(CreateCell(payroll.BaseSalary.ToString("C2"), false));
        
        // Bonuses
        foreach (var bonus in payroll.Bonuses)
        {
            earningsTable.AddCell(CreateCell(bonus.BonusType, false));
            earningsTable.AddCell(CreateCell(bonus.Amount.ToString("C2"), false));
        }
    
        // Total Earnings
        decimal totalEarnings = payroll.BaseSalary + payroll.TotalBonus;
        earningsTable.AddCell(CreateCell("TOTAL EARNINGS", true));
        earningsTable.AddCell(CreateCell(totalEarnings.ToString("C2"), true));
    
        document.Add(earningsTable);
        document.Add(new Paragraph(" "));
        
        // DEDUCTIONS SECTION
        var deductionsHeader = new Paragraph("DEDUCTIONS")
            .SetFont(boldFont)
            .SetFontSize(12)
            .SetFontColor(ColorConstants.RED);
        document.Add(deductionsHeader);
    
        Table deductionsTable = new Table(new float[] { 3, 1 });
        deductionsTable.SetWidth(UnitValue.CreatePercentValue(100));
    
        deductionsTable.AddHeaderCell(CreateHeaderCell("Description"));
        deductionsTable.AddHeaderCell(CreateHeaderCell("Amount"));
    
        foreach (var deduction in payroll.Deductions)
        {
            deductionsTable.AddCell(CreateCell(deduction.DeductionType, false));
            deductionsTable.AddCell(CreateCell("-" + deduction.Amount.ToString("C2"), false));
        }

        deductionsTable.AddCell(CreateCell("TOTAL DEDUCTIONS", true));
        deductionsTable.AddCell(CreateCell("-" + payroll.TotalDeduction.ToString("C2"), true));
    
        document.Add(deductionsTable);
        document.Add(new Paragraph(" "));
    
        // NET PAY SECTION
        Table netPayTable = new Table(new float[] { 3, 1 });
        netPayTable.SetWidth(UnitValue.CreatePercentValue(100));
    
        netPayTable.AddCell(CreateCell("NET PAY", true, ColorConstants.LIGHT_GRAY));
        netPayTable.AddCell(CreateCell(payroll.NetSalary.ToString("C2"), true, ColorConstants.LIGHT_GRAY));
    
        document.Add(netPayTable);

        // FOOTER
        document.Add(new Paragraph(" "));
        var footer = new Paragraph("This is a computer-generated document. No signature is required.")
            .SetFont(normalFont)
            .SetFontSize(8)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(ColorConstants.GRAY);
        document.Add(footer);
    }
    
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Payslip exported to: {payslipsPath}");
    Console.ResetColor();

    // Log activity
    ActivityLog activityLog = new ActivityLog()
    {
        UserId = currentUser.Id,
        Action = "Exported Payslip PDF",
        Description = $"{currentUser.Email} exported payslip for {employee.FirstName} {employee.LastName}"
    };
    _activityLogsService.AddActivityLog(activityLog);
    _logging.LogActivity(activityLog, currentUser.Email);
}

    public void ExportTimesheetToPDF(
        EmployeeProfile employeeProfile,
        User currentUser,
        DateTime fromDate,
        DateTime toDate)
    {
        var records = employeeProfile.AttendanceRecords.ToList();
        var folder = Path.Combine(_currentDirectory,"ExportFiles", "Timesheets");

        var filePath = Path.Combine(
            folder,
            $"Timesheet_{employeeProfile.FirstName}_{employeeProfile.LastName}_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.pdf"
        );

        using (var writer = new PdfWriter(filePath))
        using (var pdf = new PdfDocument(writer))
        using (var document = new Document(pdf))
        {
            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            // HEADER
            document.Add(new Paragraph("TIMESHEET REPORT")
                .SetFont(bold).SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.BLUE));

            document.Add(new Paragraph("HR Payroll Management System")
                .SetFont(normal).SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.DARK_GRAY));

            document.Add(new Paragraph(" "));

            // EMPLOYEE INFO TABLE
            var info = new Table(new float[] { 1, 1 });

            info.AddCell(CreateCell("Employee:", true));
            info.AddCell(CreateCell($"{employeeProfile.FirstName} {employeeProfile.LastName}"));

            info.AddCell(CreateCell("Employee ID:", true));
            info.AddCell(CreateCell(employeeProfile.Id.ToString()));

            info.AddCell(CreateCell("Department:", true));
            info.AddCell(CreateCell(employeeProfile.Department.DepartmentName));

            info.AddCell(CreateCell("Period:", true));
            info.AddCell(CreateCell($"{fromDate:MMM dd} - {toDate:MMM dd, yyyy}"));

            document.Add(info);
            document.Add(new Paragraph(" "));

            // MAIN TABLE
            var table = new Table(new[] { 1.2f, 1, 1, 1, 1 });

            table.AddHeaderCell(CreateHeaderCell("Date"));
            table.AddHeaderCell(CreateHeaderCell("Day"));
            table.AddHeaderCell(CreateHeaderCell("Check-In"));
            table.AddHeaderCell(CreateHeaderCell("Check-Out"));
            table.AddHeaderCell(CreateHeaderCell("Hours Worked"));

            decimal totalHours = 0;

            foreach (var r in records)
            {
                table.AddCell(CreateCell(r.WorkDate.ToString("MMM dd")));
                table.AddCell(CreateCell(r.WorkDate.DayOfWeek.ToString()));
                table.AddCell(CreateCell(r.CheckIn.ToString("HH:mm")));
                table.AddCell(CreateCell(r.CheckOut?.ToString("HH:mm") ?? "-"));
                table.AddCell(CreateCell(r.HoursWorked.ToString("0.00")));

                totalHours += r.HoursWorked;
            }

            document.Add(table);

            document.Add(new Paragraph(" "));

            // SUMMARY
            var summary = new Table(new float[] { 2, 1 });
            summary.SetWidth(UnitValue.CreatePercentValue(40));

            summary.AddCell(CreateCell("Total Hours Worked:", true));
            summary.AddCell(CreateCell(totalHours.ToString("0.00"), true));

            document.Add(summary);

            // FOOTER
            document.Add(new Paragraph("\nThis is a computer-generated timesheet. No signature required.")
                .SetFont(normal).SetFontSize(8)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.GRAY));
        }
        
        Console.Clear();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Timesheet exported to: {filePath}");
        Console.ResetColor();

        ActivityLog activityLog = new ActivityLog()
        {
            UserId = employeeProfile.UserId,
            Action = "Exported Timesheet",
            Description = $"{currentUser.Email} Has Exported {employeeProfile.User.Email} TimeSheet PDF"
        };

        _activityLogsService.AddActivityLog(activityLog);
        _logging.LogActivity(activityLog, currentUser.Email);
    }

     
    // Helper method to create table cells
    private Cell CreateCell(string text, bool isBold = false, Color backgroundColor = null)
    {
        var font = isBold ? 
            PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD) : 
            PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            
        var cell = new Cell().Add(new Paragraph(text).SetFont(font).SetFontSize(10));
        
        if (backgroundColor != null)
        {
            cell.SetBackgroundColor(backgroundColor);
        }
        
        cell.SetPadding(5);
        return cell;
    }
    private Cell CreateHeaderCell(string text)
    {
        return new Cell()
            .Add(new Paragraph(text)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(10))
            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
            .SetPadding(5);
    }
}