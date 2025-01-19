using FluentValidation;

namespace HackerNewsASP.Validators;

public record StoriesRequest(int NumberOfStories);

public class StoriesRequestValidator: AbstractValidator<StoriesRequest>
{
    public StoriesRequestValidator()
    {
        RuleFor(x => x.NumberOfStories)
            .GreaterThanOrEqualTo(1)
            .WithMessage("The minimal number of stories is 1");
    }
}