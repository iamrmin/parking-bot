using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ParkingBot
{
    [TestFixture]
    class ProgramTests
    {
        public static readonly object[] DateToCheckFreeParking =
        {
            new Object[] {new DateTime(2018, 08, 13, 7, 59, 0), true}, // August month
            new Object[] {new DateTime(2018, 12, 25, 7, 59, 0), true}, // Public holiday
            new Object[] {new DateTime(2018, 05, 13, 7, 59, 0), true},
            new Object[] {new DateTime(2018, 05, 13, 8, 0, 0), true},
            new Object[] {new DateTime(2018, 05, 13, 8, 10, 0), true },
            new Object[] {new DateTime(2018, 05, 16, 7, 59, 0), true},
            new Object[] {new DateTime(2018, 05, 16, 8, 0, 0), false},
            new Object[] {new DateTime(2018, 05, 16, 8, 10, 0), false },
            new Object[] {new DateTime(2018, 05, 16, 11, 59, 0), false},
            new Object[] {new DateTime(2018, 05, 16, 12, 0, 0), false },
            new Object[] {new DateTime(2018, 05, 16, 12, 1, 0), true },

            new Object[] {new DateTime(2018, 05, 13, 14, 0, 0), true},
            new Object[] {new DateTime(2018, 05, 13, 14, 10, 0), true },
            new Object[] {new DateTime(2018, 05, 16, 13, 59, 0), true},
            new Object[] {new DateTime(2018, 05, 16, 14, 0, 0), false},
            new Object[] {new DateTime(2018, 05, 16, 14, 10, 0), false },
            new Object[] {new DateTime(2018, 05, 16, 17, 59, 0), false},
            new Object[] {new DateTime(2018, 05, 16, 18, 0, 0), false },
            new Object[] {new DateTime(2018, 05, 16, 18, 1, 0), true }
        };

        public static readonly object[] DateToCheckCalculatedTime =
        {
            new Object[] {8, DateTime.Now.AddDays(1)},
            new Object[] {0, DateTime.Now},
            new Object[] {0.10f, DateTime.Now},
            new Object[] {0.20f, DateTime.Now},
            new Object[] {0.50f, DateTime.Now},
            new Object[] {1, DateTime.Now.AddMinutes(20)},
            new Object[] {2, DateTime.Now.AddHours(1)},
            new Object[] {3, DateTime.Now.AddHours(2)},
            new Object[] {5, DateTime.Now.AddHours(12)},
            new Object[] {1.80f, DateTime.Now.AddMinutes(20)},
            new Object[] {15, DateTime.Now.AddHours(37)},
            new Object[] {11.70f, DateTime.Now.AddHours(26)}
        };

        [Test, TestCaseSource(nameof(DateToCheckFreeParking))]
        public void IsParkingFree_VerifyScenario(DateTime dateTime, bool expectedOutput)
        {
            // Setup
            EuropeanParking europeanParking = new EuropeanParking();

            // Act
            var actualResult = europeanParking.IsParkingFree(dateTime);

            // Assert
            Assert.AreEqual(expectedOutput, actualResult);
        }

        [Test]
        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(0.10f, true)]
        [TestCase(0.20f, true)]
        [TestCase(0.50f, true)]
        [TestCase(0.60f, false)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(16, false)]
        public void IsCoinAllowed_VerifyScenario(float coinValue, bool expectedOutput)
        {
            // Setup
            EuropeanParking europeanParking = new EuropeanParking();

            // Act
            var actualResult = europeanParking.IsCoinAllowed(coinValue);

            // Assert
            Assert.AreEqual(expectedOutput, actualResult);
        }

        [Test, TestCaseSource(nameof(DateToCheckCalculatedTime))]
        public void CalculateTime_VerifyScenario(float money, DateTime expectedOutput)
        {
            // Setup
            EuropeanParking europeanParking = new EuropeanParking();

            // Act
            var actualResult = europeanParking.CalculateTime(money);

            // Assert
            Assert.AreEqual(expectedOutput.Date, actualResult.Date);
            Assert.AreEqual(expectedOutput.Hour, actualResult.Hour);
            Assert.AreEqual(expectedOutput.Minute, actualResult.Minute);
        }
    }
}
