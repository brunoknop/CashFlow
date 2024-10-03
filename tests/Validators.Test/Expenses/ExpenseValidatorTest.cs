using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enum;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using TagEnum = CashFlow.Communication.Enum.Tag;

namespace Validators.Test.Expenses;

public class ExpenseValidatorTest
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpanseJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Error_Title_Empty(string title)
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpanseJsonBuilder.Build();
        request.Title = title;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public void Error_Date_Future()
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpanseJsonBuilder.Build();
        request.Date = DateTime.Now.AddDays(1);

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EXPENSES_CANNOT_BE_FOR_THE_FUTURE));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-20)]
    public void Error_Amount_Zero(decimal amount)
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpanseJsonBuilder.Build();
        request.Amount = amount;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }

    [Fact]
    public void Error_Payment_Type()
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpanseJsonBuilder.Build();
        request.PaymentType = (PaymentType)10;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_IS_INVALID));
    }
    
    [Fact]
    public void Error_Unsupported_Tag()
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpanseJsonBuilder.Build();
        request.Tags.Add((TagEnum)1000);

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.TAG_TYPE_NOT_SUPPORTED));
    }
}
