using FluentValidation;
using MediatR;

namespace SportLookup.Backend.Infrastructure.Implementation.PipelineHandlers;

public class ValidationPipelineHandler<TRequest, TResponce> : IPipelineBehavior<TRequest, TResponce>
    where TRequest : IRequest<TResponce>
{
    private readonly IEnumerable<IValidator<IRequest>> _validators;

    public ValidationPipelineHandler(IEnumerable<IValidator<IRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponce> Handle(TRequest request, RequestHandlerDelegate<TResponce> next, CancellationToken cancellationToken)
    {
        ValidationContext<TRequest> validationContext = new(request);

        var fails = _validators
            .Select(vr => vr.Validate(validationContext))
            .Where(vRes => !vRes.IsValid)
            .SelectMany(vRes => vRes.Errors)
            .ToList();

        if (fails.Any())
            throw new ValidationException(fails);

        return next.Invoke();
    }
}
