# HR & Payroll Management System

A comprehensive, console-based Human Resources and Payroll Management System built with C# .NET. This application provides full-featured HR operations, payroll processing, attendance tracking, and employee management through an intuitive console interface.

![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![EF Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?logo=.net&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Console App](https://img.shields.io/badge/Console%20Application-4EAA25?logo=windowsterminal&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green)

## ✨ Features

### 👥 **Employee Management**
- Complete employee profiles with personal details and employment history
- Department and position management with organizational hierarchy
- Employee status tracking (Active, Inactive, On Leave, Terminated)
- Bulk import/export operations for employee data
- Document management for certificates, contracts, and IDs

### 📊 **Attendance & Time Tracking**
- Daily check-in/check-out system with timestamp recording
- Automatic calculation of total hours worked and overtime
- Attendance reports with daily, weekly, and monthly summaries
- Late arrival and absence tracking with notifications
- Leave and holiday integration for accurate attendance calculation

### 🏖️ **Leave Management**
- Multiple leave types (Annual, Sick, Maternity, Paternity, Emergency)
- Leave request workflow with approval/rejection system
- Leave balance tracking with accrual calculations
- Leave calendar visualization for team planning
- Carry-over policy implementation for unused leaves

### 💰 **Payroll Processing**
- Automated salary calculations based on attendance and overtime
- Comprehensive tax computation with configurable tax brackets
- Bonus management (performance, holiday, special occasion)
- Deduction handling (loans, advances, insurance, other deductions)
- Professional PDF payslip generation with company branding
- Batch payroll processing for multiple employees

### 📈 **Reporting & Analytics**
- Financial reports: Salary summaries, tax reports, payment histories
- HR analytics: Employee turnover, department costs, headcount trends
- Attendance reports: Punctuality analysis, absenteeism patterns
- Custom report generation with various filters and criteria
- Export capabilities to PDF, CSV, and Excel formats

### 🔐 **Security & Administration**
- Role-Based Access Control (RBAC) with HR, Manager, and Employee roles
- Comprehensive audit logging of all system activities
- Automated database backups and recovery options
- User management with permission levels
- Data encryption for sensitive information

## 🏗️ System Architecture

### **Technology Stack**
- **Backend Framework**: C# .NET Console Application
- **Database**: SQL Server with Entity Framework Core (Code-First Approach)
- **PDF Generation**: iTextSharp library for professional document creation
- **Architecture Pattern**: Multi-layer with Repository and Service patterns
- **Validation**: Comprehensive input validation and business rule enforcement
