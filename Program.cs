class Program
{
    public static void Main()
    {
        Menu.GreetingsMenu();

        string input = Console.ReadLine();

        switch (input.ToLower())
        {
            case "start":

                int weekHours;
                int hourRate;

                Console.WriteLine("How much did you worked on this weekend ? ");
                bool param1 = int.TryParse(Console.ReadLine(), out weekHours);

                Console.WriteLine("How much did you earned per hour ? ");
                bool param2 = int.TryParse(Console.ReadLine(), out hourRate);

                if (param1 && param2)
                {
                    Console.WriteLine("Here is your calculation, buddy. IRS is watching you....");
                    SalaryCalculator salaryCalculator = new SalaryCalculator(weekHours, hourRate);

                    int earnedPerWeek = salaryCalculator.CalculatePerWeak();
                    int earnedPerYear = salaryCalculator.CalculateYearSalary(earnedPerWeek);

                    Dictionary<int, int> yearSalaryWithTaxRate = salaryCalculator.CalculateYearSalaryWithTax(earnedPerYear);

                    Console.Write($"\t Earned per week: {earnedPerWeek} \n\t Earned this year: {earnedPerYear} \n\t Taxed income \n\t\t Clear income: {yearSalaryWithTaxRate.Keys.First()} " +
                        $"\n\t\t Your tax rate: {yearSalaryWithTaxRate.Values.First()}%");

                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Input validation error. Please enter INT numbers not string! ");
                }
                break;

            default:
                Console.WriteLine("Invalid input, brother. Try again :) ");
                break;
        }
    }
}

class Menu
{
    public static void GreetingsMenu()
    {
        Console.WriteLine("Welcome to the best salary calculation system :) ");
        Console.WriteLine("Enter start to begin our journey");
        Console.Write("> ");
    }
}
class SalaryCalculator
{
    private const int WeeksInYear = 52;
    private const int FirstTaxGroup = 19;
    private const int SecondTaxGroup = 21;
    private const int ThirdTaxGroup = 28;

    private int TotalWeekHours;
    private int HourShiftRate;
    public SalaryCalculator(int totalWeaklyHours, int hourShiftRate)
    {
        TotalWeekHours = totalWeaklyHours;
        HourShiftRate = hourShiftRate;
    }

    public int CalculatePerWeak()
    {
        int weakSalaryWithoutOverWorking;
        int weakSalaryWithOverWorking;

        if (TotalWeekHours <= 40)
        {
            weakSalaryWithoutOverWorking = CalculateSalary(TotalWeekHours, HourShiftRate);

            return weakSalaryWithoutOverWorking;
        }
        else
        {

            /* 
               Calculate amount of time of overworking

               *Example of hour rate changing with different hour shift rate*
               ______________________________________________
               Total worked hours - hour rate -> our rate changes

               41 - 12 -> 24
               42 - 12 -> 48
               43 - 12 -> 96
               _____________

               41 - 18 -> 36
               42 - 18 -> 72
               42 - 18 -> 144
             
             */

            int overTimedPart = TotalWeekHours - 40;
            int temp = HourShiftRate * 2;

            int overTimeRate = 0;

            if (overTimedPart == 1)
            {
                return CalculateSalary(TotalWeekHours, temp);
            }

            for (int i = 0; i < overTimedPart + 1; i++)
            {
                overTimeRate += temp * 2;
            }

            weakSalaryWithOverWorking = CalculateSalary(TotalWeekHours, overTimeRate);
            return weakSalaryWithOverWorking;
        }
    }

    private int CalculateSalary(int hoursWorked, int hourRate) => hoursWorked * hourRate;

    public int CalculateYearSalary(int weekSalary) => weekSalary * WeeksInYear;

    public Dictionary<int, int> CalculateYearSalaryWithTax(int totalYearSalary)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        int taxedYearIncome = 0;

        if (totalYearSalary < 20000)
        {
            taxedYearIncome = (totalYearSalary / 100) * FirstTaxGroup;

            result.Add(taxedYearIncome, FirstTaxGroup);
            return result;
        }

        if (totalYearSalary > 20000 && totalYearSalary < 30000)
        {
            taxedYearIncome = (totalYearSalary / 100) * SecondTaxGroup;

            result.Add(taxedYearIncome, SecondTaxGroup);
            return result;
        }

        if (totalYearSalary > 30000)
        {
            taxedYearIncome = (totalYearSalary / 100) * ThirdTaxGroup;

            result.Add(taxedYearIncome, ThirdTaxGroup);
            return result;
        }

        return result;
    }
}