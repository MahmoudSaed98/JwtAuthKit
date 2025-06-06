﻿
namespace Infrastructure.Services;

using Domain.Abstractions;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTime => DateTime.UtcNow;
}
