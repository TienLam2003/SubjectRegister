using FluentValidation;
using SubjectRegister.Models;

namespace SubjectRegister.Controllers.Validator
{
    public class StudentValidator : AbstractValidator<StudentModel>
    {
        public StudentValidator()
        {
            RuleFor(student => student.StudentCode)
                .NotEmpty()
                .WithMessage("Mã sinh viên không được để trống!");

            RuleFor(student => student.Fullname)
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Họ và tên không được để trống!");

            RuleFor(student => student.Class)
                .NotEmpty()
                .WithMessage("Lớp không được để trống!");

            RuleFor(student => student.YearOfBirth)
                .NotEmpty()
                .WithMessage("Năm sinh không được để trống!");

            RuleFor(student => student.Phone)
                .NotEmpty()
                .WithMessage("Số điện thoại không được để trống!")
                .Matches(@"^(0|\+84)(\d{9})$")
                .WithMessage("Số điện thoại không hợp lệ. Ví dụ: 0901234567 hoặc +84901234567.");
        }
    }
}
