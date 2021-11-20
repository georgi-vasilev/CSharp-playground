namespace WindowsFormsApp1.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public static class Utilities
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> arr)
        {
            return arr is null || !arr.Any();
        }

        public static int[] ParseToIntArray(this String nums)
        {
            return nums.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
        }

        public static TextBox GenerateRandomNumbers(this TextBox input)
        {
            input.Text = string.Join(", ", Enumerable.Range(0, 100)
                .OrderBy(x => Guid.NewGuid())
                .Take(10));

            return input;
        }
    }
}
