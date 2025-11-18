using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;
using HR_Payroll_managment_system.Validators;

namespace HR_Payroll_managment_system.Services;

public class AuthService : IAuthService
{
    UserRepository _userRepository = new UserRepository();
    ActivityLogsRepository _activityLogsRepository = new ActivityLogsRepository();
    RoleRepository _roleRepository = new RoleRepository();
    EmailLogsRepository _emailLogsRepository = new EmailLogsRepository();
    
    Logging _logging = new Logging();
    EmailSender _emailSender = new EmailSender();
    
    LoginValidator loginValidator = new LoginValidator();
    
    UserService _userService = new UserService();
    
    public User Login()
    {
        Console.Clear();
        Console.WriteLine("[Login Menu]");
        Console.Write("Enter Your Email: ");
        string email = Console.ReadLine();
        Console.Write("Enter Your Password: ");
        string password = Console.ReadLine();

        User user = new User()
        {
            Email = email,
            Password = password
        };
    
        var isLoginValid = loginValidator.Validate(user);

        if (isLoginValid.IsValid)
        {
            User foundUser = _userRepository.GetUserByEmail(email);

            if (BCrypt.Net.BCrypt.Verify(password, foundUser.Password))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Logged In!");
                Console.ResetColor();

                ActivityLog activityLog = new ActivityLog()
                {
                    UserId = foundUser.Id,
                    Action = "LogIn",
                    Description = "User Logged into their account"
                };
        
                _logging.LogActivity(activityLog, foundUser.Email);
                _activityLogsRepository.Add(activityLog);
                return foundUser;
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Credentials");
                Console.ResetColor();
                return null;
            }
        }
        else
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Credentials");
            Console.ResetColor();
            return null;
        }
    }

    public void SignUp()
    {
        Console.Clear();
        Console.WriteLine("[SignUp Menu]");
        Console.Write("Enter Your Email: ");
        var email = Console.ReadLine();
        Console.Write("Enter Your Password: ");
        var password = Console.ReadLine();

        var user = new User
        {
            Email = email,
            Password = password
        };

        var isLoginValid = loginValidator.Validate(user);

        if (isLoginValid.IsValid)
        {
            Console.Clear();
            var isSuccessful = false;
            do
            {
                var roles = _roleRepository.GetAll();


                Console.WriteLine("[Choose Role]");
                foreach (var item in roles) Console.WriteLine($"{item.Id}. {item.RoleName}");

                Console.WriteLine("Choose an Option:");

                var choice1 = int.Parse(Console.ReadLine());
                var result = roles.FirstOrDefault(r => r.Id == choice1);

                if (result != null)
                {
                    Console.Clear();

                    var random = new Random();
                    var code = random.Next(1000, 9999);
                    var isVerified = false;

                    var emailLog = new EmailLog
                    {
                        ToEmail = user.Email,
                        Subject = "Your Verification Code - HR Payroll System",
                        Body =
                            $@"<div style='font-family: Arial, sans-serif; padding: 20px;'><h2>HR Payroll System - Verification Code</h2><p>Hello,</p><p>Your verification code is:</p><div style='font-size: 32px; font-weight: bold; color: #007cba; margin: 20px 0;'> {code}</div><p>Enter this code in the application to verify your email address.</p><p><strong>This code expires in 10 minutes.</strong></p><hr><p style='color: #666; font-size: 12px;'>If you didn't request this code, please ignore this email.</p></div>",
                        IsSent = false
                    };

                    _emailSender.Send(emailLog);
                    _emailLogsRepository.Add(emailLog);

                    do
                    {
                        Console.WriteLine("[Verify Email]");
                        Console.WriteLine("Enter Code Sent to your Email:");
                        var codeSent = Console.ReadLine();

                        if (codeSent == code.ToString())
                        {
                            var user1 = new User
                            {
                                Email = user.Email,
                                Password = BCrypt.Net.BCrypt.HashPassword(password),
                                Role = result
                            };

                            _userRepository.Add(user1);

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Successfully Signed Up!");
                            Console.ResetColor();
                            isSuccessful = true;
                            isVerified = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Wrong Code Try again!");
                            Console.ResetColor();
                        }
                    } while (!isVerified);
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Option!");
                    Console.ResetColor();
                }
            } while (!isSuccessful);
        }
        else
        {
            Console.Clear();
            foreach (var e in isLoginValid.Errors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
            }
        }
    }
}