namespace ProjectEtoPrototype.Classes
{
    static class IdManager
    {
        // smallest random number possible
        public static int MinValue { get; set; } = 0;
        // greatest random number possible
        public static int MaxValue { get; set; } = 9;
        // but there may be smaller/greater numbers that are not random


        public static string GenerateNewId()
        {
            try
            {
                Random rnd = new Random();

                // this array will store our id
                var values2DArray = new int[3, 3];

                /* 
                {
                    { 0, 1, 2 } row 0
                    { 3, 4, 5 } row 1
                    { 6, 7, 8 } row 2
                }
                values2DArray[row, col]
                */

                // each coulmn and row's sum must be equal to the magic number
                // Each ID can have a diffrent magic number
                var magicNumber = 0;

                // for 3 times
                for (int i = 0; i < 3; i++)
                {
                    // get a random number between min and max including both
                    var number = rnd.Next(MinValue, MaxValue + 1);

                    // calculating the magic number based on the first row
                    magicNumber += number;

                    // add that numgber to the first row
                    values2DArray[0, i] = number;
                }

                // working on the second row

                // another totlly random number between min and max including both
                values2DArray[1, 0] = rnd.Next(MinValue, MaxValue + 1);

                // reminder until magic number
                var remainder = magicNumber - values2DArray[1, 0];

                // random number between min and remainer including both
                values2DArray[1, 1] = rnd.Next(MinValue, remainder + 1);

                // reminder until magic number
                remainder = magicNumber - (values2DArray[1, 0] + values2DArray[1, 1]);

                // assign row 1 col 2 to the remainder (remainder could be negative)
                values2DArray[1, 2] = remainder;

                // checking for duplicates in the pattren (2, -2, 0) for row 0
                if (Math.Abs(values2DArray[0, 0]) == Math.Abs(values2DArray[0, 1]) ||
                    Math.Abs(values2DArray[0, 0]) == Math.Abs(values2DArray[0, 2]) ||
                    Math.Abs(values2DArray[0, 1]) == Math.Abs(values2DArray[0, 2])
                    
                    // uncomment to get even less duplicates for row 1

                    /*|| Math.Abs(values2DArray[1, 0]) == Math.Abs(values2DArray[1, 1]) ||
                    Math.Abs(values2DArray[1, 0]) == Math.Abs(values2DArray[1, 2]) ||
                    Math.Abs(values2DArray[1, 1]) == Math.Abs(values2DArray[1, 2])*/
                    )
                {
                    // if duplicates found, just make another one
                    return GenerateNewId();
                }

                // once again duplicates, but this time between
                // each item in row 0 and each item in row 1
                // it will fix pattrens like 
                // {-1, -2, -3}
                // { 1,  2,  3}
                foreach (int x in GetRow(values2DArray, 0))
                {
                    foreach (int y in GetRow(values2DArray, 1))
                    {
                        if (Math.Abs(x) == Math.Abs(y))
                        {
                            return GenerateNewId();
                        }
                    }
                }

                // last row will just be what's left until the magic number
                for (int i = 0; i < 3; i++)
                {
                    values2DArray[2, i] = magicNumber - (values2DArray[0, i] + values2DArray[1, i]);
                }

                // the array will be re sorted and stored in the ID
                string id = String.Empty;

                // items will be re sorted according to this array
                int[,] tmpArrayToSort = new int[3, 3] { { 0, 1, 2 }, { 1, 2, 0 }, { 2, 0, 1 } };

                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        // we will take one number each time
                        var number = values2DArray[i, tmpArrayToSort[j, i]];

                        // if it's 10 or larger we will add "T" and then the remainder of number/10
                        // so 13 becomes T3, and 25 becomes TT5. Each "T" represent a 10
                        // if it's negative then replace the sign with "M"
                        if (Math.Abs(number) >= 10)
                        {
                            id += $"{GetMultpliedString("T", (int)Math.Floor(Math.Abs((decimal)number / 10)))}" +
                                ((number < 0) ? "M" : String.Empty) +
                                $"{Math.Abs(number) % 10}";
                        }
                        else
                        {
                            id += $"{number}".Replace("-", "M");
                        }
                    }

                    // each row separated by a dot "." except the last row
                    if (j < 2)
                    {
                        id += ".";
                    }
                }

                return id;
            }
            // just in case any thing went wrong like calling rnd.Next(4, 2)
            catch
            {
                return GenerateNewId();
            }
        }

        public static bool VerifyId(string id)
        {
            try
            {
                var values2DArray = new int[3, 3];
                string[] strArray = id.Split(".");
                for (int i = 0; i < 3; i++)
                {
                    var add = 0;
                    var mult = 1;
                    var index = 0;
                    foreach (char c in strArray[i])
                    {
                        try
                        {
                            values2DArray[i, index] = (Convert.ToInt32(c.ToString()) + add) * mult;
                            add = 0;
                            mult = 1;
                            index += 1;
                        }
                        catch
                        {
                            if (c == 'T')
                            {
                                add += 10;
                            }
                            else if (c == 'M')
                            {
                                mult = -1;
                            }
                        }
                    }
                }

                var sortedArray = new int[3, 3];
                var x = new int[3, 3]
                {
                { 0, 1, 2},
                { 2, 0, 1},
                { 1, 2, 0}
                };
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        sortedArray[i, j] = values2DArray[x[i, j], i];
                    }
                }

                var test1 = (sortedArray[0, 0] + sortedArray[0, 1] + sortedArray[0, 2]) == (sortedArray[1, 0] + sortedArray[1, 1] + sortedArray[1, 2]);
                var test2 = (sortedArray[2, 0] + sortedArray[2, 1] + sortedArray[2, 2]) == (sortedArray[1, 0] + sortedArray[1, 1] + sortedArray[1, 2]);

                var test3 = (sortedArray[0, 0] + sortedArray[1, 0] + sortedArray[2, 0]) == (sortedArray[0, 1] + sortedArray[1, 1] + sortedArray[2, 1]);
                var test4 = (sortedArray[0, 2] + sortedArray[1, 2] + sortedArray[2, 2]) == (sortedArray[0, 1] + sortedArray[1, 1] + sortedArray[2, 1]);

                var testResult = test4 && test3 && test2 && test1;
                return testResult;
            }
            catch
            {
                return false;
            }
        }

        // kinda like the (string * int) in python
        private static string GetMultpliedString(string str, int times)
        {
            string newStr = String.Empty;
            for (int i = 0; i < times; i++)
            {
                newStr += str;
            }
            return newStr;
        }

        // get one row of a 2D array
        private static IEnumerable<int> GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x]);
        }

        // used while debugging
        private static void Visualize2DIntArray(int[,] ints)
        {
            for (int i = 0; i < ints.GetLength(0); i++)
            {
                Console.WriteLine(ints[i, 0] + " " + ints[i, 1] + " " + ints[i, 2]);
            }
            Console.WriteLine();
        }
    }

}
