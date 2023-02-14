using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ZeroFriction.DB.Domain.Contracts;
using ZeroFriction.DB.Domain.Documents;
using ZeroFriction.DB.Domain.Dtos;
using ZeroFriction.DB.Domain.Exceptions;
using ZeroFriction.Domain.Contracts;
using ZeroFriction.Domain.Dtos;
using ZeroFriction.Domain.Exceptions;

namespace ZeroFriction.Services.Tests
{
    public class InvoiceServiceTest
    {
        public class CreateInvoiceTest
        {
            // Valid Invoice test data for creation test
            public static IEnumerable<object[]> ValidInvoiceData =>
               new List<object[]>
               {
                    new object[]
                    {
                        new InvoiceDto
                        {
                            Date = DateTime.UtcNow,
                            Description = "First Invoice",
                            TotalAmount = 100,
                            InvoiceLines = new List<InvoiceLineDto>
                            {
                                new InvoiceLineDto{Amount = 100, LineAmount = 100, Quantity =1, UnitPrice =100 }
                            }
                        }
                    },
                    new object[]
                    {
                        new InvoiceDto
                        {
                            Date = DateTime.UtcNow,
                            Description = "Second Invoice",
                            TotalAmount = 200,
                            InvoiceLines = new List<InvoiceLineDto>
                            {
                                new InvoiceLineDto{Amount = 100, LineAmount = 100, Quantity = 1, UnitPrice = 100 },
                                new InvoiceLineDto{Amount = 100, LineAmount = 100, Quantity = 1, UnitPrice = 100 },
                            }
                        }
                    }
               };

            /// <summary>
            /// Testing successful invoice creation data when valid data
            /// </summary>
            /// <param name="invoiceDto"></param>
            /// <returns></returns>
            [Theory]
            [MemberData(nameof(ValidInvoiceData))]
            public async Task WhenPassingCorrectData_CreateSuccessfully(InvoiceDto invoiceDto)
            {
                Mock<IDocumentDbService> mockDocumentDbService = new();
                mockDocumentDbService.Setup(s => s.CreateDocumentAsync(It.IsAny<string>(), It.IsAny<Invoice>()))
                    .ReturnsAsync(new DocumentUpdateResultDto { Id = "CreatedId" });
                var invoiceService = CreateInvoiceService(mockDocumentDbService);

                var result = await invoiceService.CreateAsync(invoiceDto);
                Assert.NotNull(result);
                Assert.NotNull(result.Id);
                mockDocumentDbService.Verify(s => s.CreateDocumentAsync(It.IsAny<string>(), It.IsAny<Invoice>()), Times.Once());
            }

            // Invalid test data for invoice creation test
            public static IEnumerable<object[]> InvalidInvoiceData =>
               new List<object[]>
               {
                    new object[]
                    {
                        new InvoiceDto
                        {
                            Date = DateTime.MinValue,
                            Description = "First Invoice",
                            TotalAmount = 100,
                            InvoiceLines = new List<InvoiceLineDto>
                            {
                                new InvoiceLineDto{Amount = 100, LineAmount = 100, Quantity =1, UnitPrice =100 }
                            }
                        },
                        "ERROR_VALID_DATE_REQUIRED"
                    },
                    new object[]
                    {
                        new InvoiceDto
                        {
                            Date = DateTime.UtcNow,
                            Description = "",
                            TotalAmount = 100,
                            InvoiceLines = new List<InvoiceLineDto>
                            {
                                new InvoiceLineDto{Amount = 100, LineAmount = 100, Quantity =1, UnitPrice =100 }
                            }
                        },
                        "ERROR_VALID_DESCRIPTION_REQUIRED"
                    },
                    new object[]
                    {
                        new InvoiceDto
                        {
                            Date = DateTime.UtcNow,
                            Description = "First Invoice",
                            TotalAmount = 100,
                            InvoiceLines = new List<InvoiceLineDto>()
                        },
                        "ERROR_AT_LEAST_ONE_INVOICELINE_REQUIRED"
                    },
               };

            /// <summary>
            /// Testing validation exceptions when try to create with invalid data
            /// </summary>
            /// <param name="invoiceDto"></param>
            /// <param name="expectedErrorMessage"></param>
            /// <returns></returns>
            [Theory]
            [MemberData(nameof(InvalidInvoiceData))]
            public async Task WhenPassingIncorrectData_ThrowsException(InvoiceDto invoiceDto, string expectedErrorMessage)
            {
                Mock<IDocumentDbService> mockDocumentDbService = new();
                var invoiceService = CreateInvoiceService(mockDocumentDbService);

                var ex = await Assert.ThrowsAsync<BusinessException>(() => invoiceService.CreateAsync(invoiceDto));
                Assert.Equal(expectedErrorMessage, ex.Message);
                mockDocumentDbService.Verify(s => s.CreateDocumentAsync(It.IsAny<string>(), It.IsAny<Invoice>()), Times.Never());
            }
        }

        public class DeleteInvoiceTest
        {
            /// <summary>
            /// Test successful invoice deletion
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            [Theory]
            [InlineData("id-1")]
            public async Task WhenDeletingInvoice_SuccessfullyDelete(string id)
            {
                Mock<IDocumentDbService> mockDocumentDbService = new();
                var invoiceService = CreateInvoiceService(mockDocumentDbService);

                await invoiceService.DeleteAsync(id);
                mockDocumentDbService.Verify(s => s.DeleteDocumentAsync<Invoice>(id, id), Times.Once());
            }
        }

        public class GetInvoiceTest
        {
            /// <summary>
            /// Test successful data get when retrieving an invoice
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            [Theory]
            [InlineData("id-1")]
            public async Task WhenRetrieving_ReturnCorrectData(string id)
            {
                Mock<IDocumentDbService> mockDocumentDbService = new();
                mockDocumentDbService.Setup(s => s.GetDocumentAsync<Invoice>(id, id))
                    .ReturnsAsync(new Invoice
                    {
                        Id = id,
                        ETag = "ETAG-1",
                        Date = DateTime.UtcNow,
                        TotalAmount = 100,
                        InvoiceLines = new List<InvoiceLine>
                        {
                            new InvoiceLine
                            {
                                Amount = 100,
                                LineAmount = 100,
                                Quantity = 1,
                                UnitPrice = 100
                            }
                        }
                    });
                var invoiceService = CreateInvoiceService(mockDocumentDbService);

                var result = await invoiceService.GetAsync(id);
                Assert.NotNull(result);
                Assert.Equal(id, result.Id);
                mockDocumentDbService.Verify(s => s.GetDocumentAsync<Invoice>(id, id), Times.Once());
            }

            /// <summary>
            /// Test throwing DocumentNotFoundException when trying to retrieve non exsititng document
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            [Theory]
            [InlineData("id-not-exist")]
            public async Task WhenRetrievingNonExisitngRecord_ThrowsException(string id)
            {
                Mock<IDocumentDbService> mockDocumentDbService = new();
                mockDocumentDbService.Setup(s => s.GetDocumentAsync<Invoice>(id, id))
                    .ThrowsAsync(new DocumentNotFoundException());
                var invoiceService = CreateInvoiceService(mockDocumentDbService);

                await Assert.ThrowsAsync<DocumentNotFoundException>(() => invoiceService.GetAsync(id));
            }
        }

        public class UpdateInvoiceTest
        {
            // Valid invoice data for update
            public static IEnumerable<object[]> ValidInvoiceData =>
               new List<object[]>
               {
                    new object[]
                    {
                        new InvoiceDto
                        {
                            Id = "ID-1",
                            ETag = "ETAG-1",
                            Date = DateTime.UtcNow,
                            Description = "First Invoice",
                            TotalAmount = 100,
                            InvoiceLines = new List<InvoiceLineDto>
                            {
                                new InvoiceLineDto{Amount = 100, LineAmount = 100, Quantity =1, UnitPrice =100 }
                            }
                        }
                    },
               };

            /// <summary>
            /// Test successful invoice update when valid data
            /// </summary>
            /// <param name="invoiceDto"></param>
            /// <returns></returns>
            [Theory]
            [MemberData(nameof(ValidInvoiceData))]
            public async Task WhenPassingCorrectData_UpdateSuccessfully(InvoiceDto invoiceDto)
            {
                Mock<IDocumentDbService> mockDocumentDbService = new();
                mockDocumentDbService.Setup(s => s.GetDocumentAsync<Invoice>(invoiceDto.Id, invoiceDto.Id))
                   .ReturnsAsync(new Invoice
                   {
                       Id = invoiceDto.Id,
                       ETag = "ETAG-1",
                       Date = DateTime.UtcNow,
                       TotalAmount = 100,
                       InvoiceLines = new List<InvoiceLine>
                       {
                            new InvoiceLine
                            {
                                Amount = 100,
                                LineAmount = 100,
                                Quantity = 1,
                                UnitPrice = 100
                            }
                       }
                   });
                mockDocumentDbService.Setup(s => s.ReplaceDocumentAsync(invoiceDto.Id, invoiceDto.Id, It.IsAny<Invoice>(), invoiceDto.ETag))
                    .ReturnsAsync(new DocumentUpdateResultDto { Id = invoiceDto.Id });
                var invoiceService = CreateInvoiceService(mockDocumentDbService);

                var result = await invoiceService.UpdateAsync(invoiceDto.Id, invoiceDto);
                Assert.NotNull(result);
                Assert.NotNull(result.Id);
                mockDocumentDbService.Verify(s => s.GetDocumentAsync<Invoice>(result.Id, result.Id), Times.Once());
                mockDocumentDbService.Verify(s => s.ReplaceDocumentAsync(result.Id, result.Id, It.IsAny<Invoice>(), It.IsAny<string>()), Times.Once());
            }

            // Invalid test data for update test
            public static IEnumerable<object[]> InvalidInvoiceData =>
               new List<object[]>
               {
                    new object[]
                    {
                        new InvoiceDto
                        {
                            Date = DateTime.MinValue,
                            Description = "First Invoice",
                            TotalAmount = 100,
                            InvoiceLines = new List<InvoiceLineDto>
                            {
                                new InvoiceLineDto{Amount = 100, LineAmount = 100, Quantity =1, UnitPrice =100 }
                            }
                        },
                        "ERROR_VALID_DATE_REQUIRED"
                    },
                    new object[]
                    {
                        new InvoiceDto
                        {
                            Date = DateTime.UtcNow,
                            Description = "",
                            TotalAmount = 100,
                            InvoiceLines = new List<InvoiceLineDto>
                            {
                                new InvoiceLineDto{Amount = 100, LineAmount = 100, Quantity =1, UnitPrice =100 }
                            }
                        },
                        "ERROR_VALID_DESCRIPTION_REQUIRED"
                    },
                    new object[]
                    {
                        new InvoiceDto
                        {
                            Date = DateTime.UtcNow,
                            Description = "First Invoice",
                            TotalAmount = 100,
                            InvoiceLines = new List<InvoiceLineDto>()
                        },
                        "ERROR_AT_LEAST_ONE_INVOICELINE_REQUIRED"
                    },
               };

            /// <summary>
            /// Testing validation exceptions when try to update with invalid data
            /// </summary>
            /// <param name="invoiceDto"></param>
            /// <param name="expectedErrorMessage"></param>
            /// <returns></returns>
            [Theory]
            [MemberData(nameof(InvalidInvoiceData))]
            public async Task WhenPassingIncorrectData_ThrowsException(InvoiceDto invoiceDto, string expectedErrorMessage)
            {
                Mock<IDocumentDbService> mockDocumentDbService = new();
                var invoiceService = CreateInvoiceService(mockDocumentDbService);

                var ex = await Assert.ThrowsAsync<BusinessException>(() => invoiceService.CreateAsync(invoiceDto));
                Assert.Equal(expectedErrorMessage, ex.Message);
                mockDocumentDbService.Verify(s => s.CreateDocumentAsync(It.IsAny<string>(), It.IsAny<Invoice>()), Times.Never());
            }
        }

        private static IInvoiceService CreateInvoiceService(Mock<IDocumentDbService> mockDocumetnDbService)
        {
            return new InvoiceService(mockDocumetnDbService.Object);
        }
    }
}