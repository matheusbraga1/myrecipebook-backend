using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncrypter _passwordEncrypter;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IUnitOfWork unitOfWork,
        IPasswordEncrypter passwordEncrypter)
    {
        _loggedUser = loggedUser;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncrypter = passwordEncrypter;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();

        Validade(request, loggedUser);

        var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

        user.Password = _passwordEncrypter.Encrypt(request.NewPassword);

        _userUpdateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validade(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var currentPasswordEncrypted = _passwordEncrypter.Encrypt(request.CurrentPassword);

        if (!currentPasswordEncrypted.Equals(loggedUser.Password))
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessageException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        var newPasswordEncrypted = _passwordEncrypter.Encrypt(request.NewPassword);

        if (newPasswordEncrypted.Equals(loggedUser.Password))
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessageException.PASSWORD_CURRENT_EQUALS_NEW_PASSWORD));

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).ToList());
    }
}
