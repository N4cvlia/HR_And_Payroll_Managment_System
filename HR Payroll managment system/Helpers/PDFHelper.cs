using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
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
    
    ActivityLogsRepository _activityLogsRepository = new ActivityLogsRepository();
    Logging _logging = new Logging();

    public void ExportToPDF(Payroll payroll, User currentUser)
    {
        string payslipsFolder = Path.Combine(_currentDirectory, "Payslips");
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

        _activityLogsRepository.Add(activityLog);
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