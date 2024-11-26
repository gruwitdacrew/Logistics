namespace Logistics.Services.Utils
{
    public class CostСalculator
    {
        private const float distancePerDayInKilometers  = 900;

        private const float driverDailyWageInRubles  = 3000;

        private const float driverSalaryPerKilometer = 11;

        private const float fuelVolumePer100Kilometers = 40;

        private const float fuelCostPerLiterInRubles = 75;

        private const float seasonalityСoefficient = 1.05f;

        private const int marginToTruckOwnerInPercents = 23;

        private const float shareOfShipmentInTruck = 1; //определяется массой, объемом, типом груза(для mvp-0 D = 1, все грузы массой только 20 тонн, а фуры – грузоподъемностью 20 тонн)

        private const float serviceFeeInRubles = 500;
        public static float calculateCostInRubles(int distanceBetweenCitiesInKilometers)
        {
            var defaultPayment = (int)Math.Round(distanceBetweenCitiesInKilometers / distancePerDayInKilometers) * driverDailyWageInRubles;

            var premium = distanceBetweenCitiesInKilometers * driverSalaryPerKilometer;

            var compensationOfExpenses = distanceBetweenCitiesInKilometers / 100 * fuelVolumePer100Kilometers * fuelCostPerLiterInRubles * seasonalityСoefficient;


            return (defaultPayment + premium + compensationOfExpenses) * marginToTruckOwnerInPercents * shareOfShipmentInTruck + serviceFeeInRubles;
        }
    }
}
