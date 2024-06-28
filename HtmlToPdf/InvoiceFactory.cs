using Bogus;
using HtmlToPdf.Contracts;

namespace HtmlToPdf;

internal sealed class InvoiceFactory
{
    public Invoice Create()
    {
        var faker = new Faker();

        return new Invoice
        {
            Number = faker.Random.Number(100_000, 1_000_000).ToString(),
            IssuedDate = DateOnly.FromDateTime(DateTime.UtcNow),
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
            SellerAddress = new Address
            {
                CompanyName = faker.Company.CompanyName(),
                Street = faker.Address.StreetAddress(),
                City = faker.Address.City(),
                State = faker.Address.State(),
                Email = faker.Internet.Email()
            },
            CustomerAddress = new Address
            {
                CompanyName = faker.Company.CompanyName(),
                Street = faker.Address.StreetAddress(),
                City = faker.Address.City(),
                State = faker.Address.State(),
                Email = faker.Internet.Email()
            },
            LineItems = Enumerable
                .Range(1, 13)
                .Select(i => new LineItem
                {
                    Id = i,
                    Name = faker.Commerce.ProductName(),
                    Price = faker.Random.Decimal(10, 1000),
                    Quantity = faker.Random.Decimal(1, 10)
                })
                .ToArray()
        };
    }
}
