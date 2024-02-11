namespace Uceme.Foundation.Tests.Utilities;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class DateTimeUtilsTests
{
    [TestMethod]
    public void CanCallTimeToDecimal()
    {
        // Arrange
        string input = "12,30";
        decimal output = 12.50M;
        string input2 = "12,00";
        decimal output2 = 12.00M;
        string input3 = "15,45";
        decimal output3 = 15.75M;

        // Act
        decimal result = Uceme.Foundation.Utilities.DateTimeUtils.TimeToDecimal(input);
        decimal result2 = Uceme.Foundation.Utilities.DateTimeUtils.TimeToDecimal(input2);
        decimal result3 = Uceme.Foundation.Utilities.DateTimeUtils.TimeToDecimal(input3);

        // Assert
        Assert.AreEqual(output, result);
        Assert.AreEqual(output2, result2);
        Assert.AreEqual(output3, result3);
    }

    [DataTestMethod]
    [DataRow(null)]
    public void CannotCallTimeToDecimalWithNullStrhora(string value)
    {
        Assert.ThrowsException<ArgumentNullException>(() => Uceme.Foundation.Utilities.DateTimeUtils.TimeToDecimal(value));
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void CannotCallTimeToDecimalWithInvalidStrhora(string value)
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Uceme.Foundation.Utilities.DateTimeUtils.TimeToDecimal(value));
    }

    [TestMethod]
    public void CanCallTimeToString()
    {
        // Arrange
        string input = "12:30";
        decimal output = 12.50M;
        string input2 = "12:00";
        decimal output2 = 12.00M;
        string input3 = "15:45";
        decimal output3 = 15.75M;
        string input4 = "01:45";
        decimal output4 = 1.75M;

        // Act
        string? result = Uceme.Foundation.Utilities.DateTimeUtils.TimeToString(output);
        string? result2 = Uceme.Foundation.Utilities.DateTimeUtils.TimeToString(output2);
        string? result3 = Uceme.Foundation.Utilities.DateTimeUtils.TimeToString(output3);
        string? result4 = Uceme.Foundation.Utilities.DateTimeUtils.TimeToString(output4);

        // Assert
        Assert.AreEqual(input, result);
        Assert.AreEqual(input2, result2);
        Assert.AreEqual(input3, result3);
        Assert.AreEqual(input4, result4);
    }
}
